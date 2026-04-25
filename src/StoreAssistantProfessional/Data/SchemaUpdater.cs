using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace StoreAssistantProfessional.Data;

/// <summary>
/// Lightweight schema bumper for our SQLite store.
///
/// EF's <c>EnsureCreated</c> is one-shot — it only runs when the DB file is missing.
/// Once the schema exists, columns added in later versions of the model are NOT applied.
/// This class adds idempotent ALTER TABLE / CREATE TABLE statements that bring older
/// databases up to the current model. Future schema changes go here as new lines.
///
/// Strategy:
///   1. <c>EnsureCreated</c> still creates everything for brand-new installs.
///   2. For upgrade installs, we discover existing tables/columns via <c>PRAGMA</c>
///      and run targeted ALTERs only for what's missing.
///   3. Each statement is its own transaction, idempotent (existence-checked first),
///      and never throws into the boot path.
/// </summary>
public static class SchemaUpdater
{
    public static async Task ApplyAsync(AppDbContext db)
    {
        var conn = (SqliteConnection)db.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync();

        // -- Bills: Kind, IsComposition (added in Wave 13)
        await EnsureColumnAsync(conn, "Bills", "Kind", "TEXT NOT NULL DEFAULT 'TaxInvoice'");
        await EnsureColumnAsync(conn, "Bills", "IsComposition", "INTEGER NOT NULL DEFAULT 0");

        // -- AppSettings: many fields added incrementally
        await EnsureColumnAsync(conn, "Settings", "GoalNudgeEnabled", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "ShowEomWrapUp", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "EomDismissedFor", "TEXT NULL");
        await EnsureColumnAsync(conn, "Settings", "InventoryValuationMethod", "TEXT NOT NULL DEFAULT 'LastPurchase'");
        await EnsureColumnAsync(conn, "Settings", "ReceiptPaper", "TEXT NOT NULL DEFAULT '3in'");
        await EnsureColumnAsync(conn, "Settings", "ReceiptShowLogo", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "ReceiptShowAddress", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "ReceiptShowGstin", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "ReceiptShowBillNumber", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "ReceiptShowCashier", "INTEGER NOT NULL DEFAULT 0");
        await EnsureColumnAsync(conn, "Settings", "ReceiptShowCustomer", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "ReceiptShowItems", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "ReceiptShowTaxSplit", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "ReceiptShowTender", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "ReceiptShowQrUpi", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "ReceiptShowFooter", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "ReceiptFooterText", "TEXT NOT NULL DEFAULT 'Thank you for shopping with us!'");
        await EnsureColumnAsync(conn, "Settings", "ReceiptLanguage", "TEXT NOT NULL DEFAULT 'en'");
        await EnsureColumnAsync(conn, "Settings", "ReceiptBilingual", "INTEGER NOT NULL DEFAULT 0");
        await EnsureColumnAsync(conn, "Settings", "UserCanGiveDiscount", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "UserCanRefund", "INTEGER NOT NULL DEFAULT 0");
        await EnsureColumnAsync(conn, "Settings", "UserCanVoidBill", "INTEGER NOT NULL DEFAULT 0");
        await EnsureColumnAsync(conn, "Settings", "UserCanEditSavedBill", "INTEGER NOT NULL DEFAULT 0");
        await EnsureColumnAsync(conn, "Settings", "UserCanAdjustStock", "INTEGER NOT NULL DEFAULT 0");
        await EnsureColumnAsync(conn, "Settings", "UserCanEditSettings", "INTEGER NOT NULL DEFAULT 0");
        await EnsureColumnAsync(conn, "Settings", "UserCanOpenDrawer", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "UserCanRunReports", "INTEGER NOT NULL DEFAULT 1");
        await EnsureColumnAsync(conn, "Settings", "UserCanExport", "INTEGER NOT NULL DEFAULT 0");
        await EnsureColumnAsync(conn, "Settings", "UserCanReset", "INTEGER NOT NULL DEFAULT 0");
        await EnsureColumnAsync(conn, "Settings", "EwayEnabled", "INTEGER NOT NULL DEFAULT 0");
        await EnsureColumnAsync(conn, "Settings", "EwayUserName", "TEXT NULL");
        await EnsureColumnAsync(conn, "Settings", "EwayPassword", "TEXT NULL");
        await EnsureColumnAsync(conn, "Settings", "EwayGstin", "TEXT NULL");
        await EnsureColumnAsync(conn, "Settings", "EinvoiceEnabled", "INTEGER NOT NULL DEFAULT 0");
        await EnsureColumnAsync(conn, "Settings", "EinvoiceClientId", "TEXT NULL");
        await EnsureColumnAsync(conn, "Settings", "EinvoiceClientSecret", "TEXT NULL");
        await EnsureColumnAsync(conn, "Settings", "EinvoiceTurnoverThreshold", "TEXT NOT NULL DEFAULT '50000000'");

        // -- StockAdjustment: operator role
        await EnsureColumnAsync(conn, "StockAdjustments", "OperatorRole", "TEXT NULL");

        // -- TaxRate.Name unique index needs to ignore soft-deleted rows so a user
        //    who removed "GST 18%" can recreate it. EnsureCreated installed the
        //    unfiltered version on older DBs; replace it with the filtered one.
        await ReplaceIndexAsync(conn,
            "IX_TaxRates_Name",
            "CREATE UNIQUE INDEX \"IX_TaxRates_Name\" ON \"TaxRates\" (\"Name\") WHERE \"IsActive\" = 1");
    }

    private static async Task<bool> ColumnExistsAsync(SqliteConnection conn, string table, string column)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = $"PRAGMA table_info('{table}')";
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var name = reader.GetString(1); // 0=cid, 1=name, ...
            if (string.Equals(name, column, StringComparison.OrdinalIgnoreCase)) return true;
        }
        return false;
    }

    private static async Task<bool> TableExistsAsync(SqliteConnection conn, string table)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name=$n";
        cmd.Parameters.AddWithValue("$n", table);
        var result = await cmd.ExecuteScalarAsync();
        return result is not null;
    }

    private static async Task<string?> IndexCreateSqlAsync(SqliteConnection conn, string indexName)
    {
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT sql FROM sqlite_master WHERE type='index' AND name=$n";
        cmd.Parameters.AddWithValue("$n", indexName);
        var result = await cmd.ExecuteScalarAsync();
        return result is null or DBNull ? null : (string)result;
    }

    private static async Task ReplaceIndexAsync(SqliteConnection conn, string indexName, string createSql)
    {
        try
        {
            var existing = await IndexCreateSqlAsync(conn, indexName);
            if (existing is not null && string.Equals(existing.Trim(), createSql.Trim(), StringComparison.OrdinalIgnoreCase))
                return;

            await using (var drop = conn.CreateCommand())
            {
                drop.CommandText = $"DROP INDEX IF EXISTS \"{indexName}\"";
                await drop.ExecuteNonQueryAsync();
            }
            await using (var create = conn.CreateCommand())
            {
                create.CommandText = createSql;
                await create.ExecuteNonQueryAsync();
            }
        }
        catch
        {
            // Defensive: never block boot.
        }
    }

    private static async Task EnsureColumnAsync(SqliteConnection conn, string table, string column, string spec)
    {
        try
        {
            if (!await TableExistsAsync(conn, table)) return;
            if (await ColumnExistsAsync(conn, table, column)) return;

            await using var cmd = conn.CreateCommand();
            cmd.CommandText = $"ALTER TABLE \"{table}\" ADD COLUMN \"{column}\" {spec}";
            await cmd.ExecuteNonQueryAsync();
        }
        catch
        {
            // Defensive: never block boot. The model validation will surface real schema mismatches at first query.
        }
    }
}
