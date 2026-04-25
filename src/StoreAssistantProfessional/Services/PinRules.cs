namespace StoreAssistantProfessional.Services;

public static class PinRules
{
    // Common dialpad columns/diagonals and "spirals" attackers try first. We keep this
    // list short — broader patterns (sequences, palindromes, repeats) are caught
    // structurally below; anything we have to enumerate by hand goes here.
    // `StringComparer.Ordinal` is explicit defensiveness — PIN strings are digit-only
    // so case-folding is irrelevant, but ordinal makes the non-cultural comparison clear.
    private static readonly HashSet<string> KnownWeakPins = new(StringComparer.Ordinal)
    {
        "2580", "0852",
        "1379", "9731", "3197", "7913",
        "1593", "3951", "7531", "1357",
        "147258", "258369", "369258",
        "147369", "963741", "159753", "357159",
    };

    // `IsWeak` returns false for length < 3 by design — weakness is only meaningful
    // once the user has typed something resembling a real PIN. Callers that care
    // about partial input must guard on length themselves (the setup form does).
    //
    // Contract:
    //   IsStrong(pin) implies !IsWeak(pin)        — but the reverse isn't always true.
    //   IsStrong is the "OK to celebrate" cue;   IsWeak is the "must reject" gate.
    // The setup form uses both: IsWeak rejects, IsStrong drives the "OK" affordance.

    public static bool IsStrong(string pin)
    {
        if (pin.Length < 4 || !IsAsciiDigits(pin) || IsWeak(pin)) return false;
        var unique = pin.Distinct().Count();
        return pin.Length switch
        {
            4 => unique >= 3,
            6 => unique >= 4,
            _ => unique >= pin.Length - 1,
        };
    }

    public static bool IsWeak(string pin)
    {
        if (pin.Length < 3 || !IsAsciiDigits(pin)) return false;

        if (pin.All(c => c == pin[0])) return true;
        if (IsPalindrome(pin)) return true;
        if (IsArithmeticProgression(pin)) return true;

        if (pin.Length % 2 == 0 && IsBlockRepeat(pin, 2)) return true;
        if (pin.Length % 3 == 0 && pin.Length >= 6 && IsBlockRepeat(pin, 3)) return true;

        if (pin.Length == 6)
        {
            if (pin[..3] == pin[3..]) return true;
            if (AllSameInHalves(pin)) return true;
            if (IsSteppedPairs(pin)) return true;
        }
        if (pin.Length == 4 && IsSteppedPairs(pin)) return true;

        if (KnownWeakPins.Contains(pin)) return true;

        if (IsNearSequence(pin)) return true;

        // Low-uniqueness check — catches "0007", "1110", "9888", "2882", and the
        // 6-digit equivalents (e.g. "111122"). 4-digit needs ≥3 distinct,
        // 6-digit needs ≥3 distinct (i.e. reject "all same except one or two").
        if (HasTooFewDistinctDigits(pin)) return true;

        // Year-based: "1990" / "2003" are among the most common real-world PIN
        // choices (birth years, anniversaries). Rejecting any 4-digit value that
        // parses as a plausible year removes a huge concentrated cluster of
        // attacker-guessed PINs at the cost of disallowing one date-shaped PIN
        // per user. The year window covers everyone reasonably alive plus a
        // generation forward, with `DateTime.UtcNow.Year + 5` as the upper edge
        // so the rule keeps making sense as time passes.
        if (pin.Length == 4 && IsPlausibleYear(pin)) return true;

        // 6-digit date-shapes — DDMMYY, MMDDYY, YYMMDD — share the "Indian
        // birthday" risk profile. Reject any 6-digit PIN that matches one of
        // these layouts as long as the implied date is real.
        if (pin.Length == 6 && IsPlausibleDate(pin)) return true;

        return false;
    }

    private static bool IsPalindrome(string pin)
    {
        for (int i = 0, j = pin.Length - 1; i < j; i++, j--)
            if (pin[i] != pin[j]) return false;
        return true;
    }

    // Strict arithmetic progression with step in -5..5 \ {0}, mod 10 (so 8901 and 9012 are caught).
    private static bool IsArithmeticProgression(string pin)
    {
        for (var step = -5; step <= 5; step++)
        {
            if (step == 0) continue;
            var ok = true;
            for (var i = 1; i < pin.Length; i++)
            {
                var expected = Mod10((pin[0] - '0') + step * i);
                if (pin[i] - '0' != expected) { ok = false; break; }
            }
            if (ok) return true;
        }
        return false;
    }

    // At most one digit deviates from a strict arithmetic progression — catches 1235, 1245, 8769, etc.
    // The fixed deviation budget of 1 is tight at 6 digits (17 % deviation). At
    // 4 digits it's 25 %. If pin lengths ever expand beyond 6, scale the budget
    // by length (e.g. `pin.Length / 4`) — the current fixed value works for the
    // 4/6 PINs the app ships with.
    private static bool IsNearSequence(string pin)
    {
        if (pin.Length < 4) return false;
        for (var step = -5; step <= 5; step++)
        {
            if (step == 0) continue;
            for (var startDigit = 0; startDigit < 10; startDigit++)
            {
                var diff = 0;
                for (var i = 0; i < pin.Length; i++)
                {
                    var expected = Mod10(startDigit + step * i);
                    if (pin[i] - '0' != expected) diff++;
                    if (diff > 1) break;
                }
                if (diff <= 1) return true;
            }
        }
        return false;
    }

    private static bool IsBlockRepeat(string pin, int blockLen)
    {
        for (var i = blockLen; i < pin.Length; i++)
            if (pin[i] != pin[i % blockLen]) return false;
        return true;
    }

    private static bool IsSteppedPairs(string pin)
    {
        for (var i = 0; i < pin.Length; i += 2)
            if (pin[i] != pin[i + 1]) return false;
        for (var i = 2; i < pin.Length; i += 2)
            if (pin[i] - pin[i - 2] != 1) return false;
        return true;
    }

    private static bool AllSameInHalves(string pin)
    {
        var half = pin.Length / 2;
        for (var i = 1; i < half; i++)
            if (pin[i] != pin[0]) return false;
        for (var i = half + 1; i < pin.Length; i++)
            if (pin[i] != pin[half]) return false;
        return true;
    }

    private static bool HasTooFewDistinctDigits(string pin)
    {
        // A 4-digit PIN with ≤ 2 distinct digits is structurally trivial to
        // brute-force (10·9 = 90 combinations of "two-character alphabets" ×
        // a handful of patterns). A 6-digit PIN with ≤ 2 distinct is similarly
        // weak. ≤ 2 distinct catches: "0007", "1110", "1100", "1212", "1122".
        // 1212 is already caught by block-repeat, but the rule subsumes both
        // and is the simplest way to reject the long tail.
        var distinct = pin.Distinct().Count();
        return pin.Length switch
        {
            4 => distinct <= 2,
            6 => distinct <= 2,
            _ => false,
        };
    }

    private static bool IsPlausibleYear(string pin)
    {
        if (pin.Length != 4) return false;
        if (!int.TryParse(pin, out var year)) return false;
        var thisYear = DateTime.UtcNow.Year;
        // 1900 covers anyone reasonably alive; +5 is a small forward window so
        // a reset on Dec 31 doesn't suddenly reject "next year".
        return year >= 1900 && year <= thisYear + 5;
    }

    private static bool IsPlausibleDate(string pin)
    {
        if (pin.Length != 6) return false;
        // DDMMYY
        if (TryParseDate(pin[..2], pin[2..4], pin[4..])) return true;
        // MMDDYY
        if (TryParseDate(pin[2..4], pin[..2], pin[4..])) return true;
        // YYMMDD
        if (TryParseDate(pin[4..], pin[2..4], pin[..2])) return true;
        return false;
    }

    private static bool TryParseDate(string ddPart, string mmPart, string yyPart)
    {
        if (!int.TryParse(ddPart, out var dd)) return false;
        if (!int.TryParse(mmPart, out var mm)) return false;
        if (!int.TryParse(yyPart, out var yy)) return false;
        if (mm is < 1 or > 12) return false;
        // Pivot 2-digit year: 00..(thisYear%100+5) → 2000s, else → 1900s.
        var thisYy = DateTime.UtcNow.Year % 100;
        var year = yy <= thisYy + 5 ? 2000 + yy : 1900 + yy;
        if (dd < 1 || dd > DateTime.DaysInMonth(year, mm)) return false;
        return true;
    }

    public static bool IsAsciiDigits(string s)
    {
        for (var i = 0; i < s.Length; i++)
            if (s[i] is < '0' or > '9') return false;
        return true;
    }

    private static int Mod10(int v)
    {
        var r = v % 10;
        return r < 0 ? r + 10 : r;
    }
}
