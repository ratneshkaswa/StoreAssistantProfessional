# StoreAssistantPro — Business-Logic Feature Reference

Generated catalogue of every business-logic interface in this repo, grouped by the folder it lives in under `src/StoreAssistantPro.Services/Services/`. One-line purpose where the source has a doc comment; interface name + file otherwise. **UI / UX code is intentionally excluded** — this is only the portable service layer.

**Counts:** 75 folders, 889 interfaces.

**How to use as a reference:**
1. Scan the folder headings to find the domain area (Billing is in `Cart` / `ShoppingFlow` / `ShoppingSession`, GST in `TaxBreakdown`, vendor ledger in `VendorExtras`, etc.).
2. Look up the interface — the concrete implementation is in the same folder, same name minus the leading `I`.
3. The domain POCOs (`Sale`, `Vendor`, `Product`, etc.) live under `src/StoreAssistantPro.Core/Models/`.

---

## Table of contents

- [Accessibility](#accessibility) — 5 interfaces
- [Activity](#activity) — 5 interfaces
- [AiAdapters](#aiadapters) — 5 interfaces
- [AnalyticsEdge](#analyticsedge) — 5 interfaces
- [Auth](#auth) — 1 interface
- [BackupHelpers](#backuphelpers) — 5 interfaces
- [BarcodeQr](#barcodeqr) — 5 interfaces
- [Batch14](#batch14) — 61 interfaces
- [Branch](#branch) — 5 interfaces
- [Cart](#cart) — 7 interfaces
- [ClothingRetail](#clothingretail) — 5 interfaces
- [Compliance](#compliance) — 10 interfaces
- [ConfigValidators](#configvalidators) — 5 interfaces
- [Core](#core) — 48 interfaces
- [CustomerEngagement](#customerengagement) — 6 interfaces
- [CustomerProfile](#customerprofile) — 5 interfaces
- [DataValidators2](#datavalidators2) — 5 interfaces
- [DebugTooling](#debugtooling) — 5 interfaces
- [Delivery](#delivery) — 5 interfaces
- [Diagnostics](#diagnostics) — 5 interfaces
- [Events](#events) — 5 interfaces
- [FeatureFlags](#featureflags) — 5 interfaces
- [Files](#files) — 5 interfaces
- [FuzzyMatch](#fuzzymatch) — 5 interfaces
- [Helpers](#helpers) — 64 interfaces
- [Hr](#hr) — 9 interfaces
- [Hygiene](#hygiene) — 6 interfaces
- [ImportPipelines](#importpipelines) — 6 interfaces
- [Indexes](#indexes) — 6 interfaces
- [Infrastructure](#infrastructure) — 78 interfaces
- [Input](#input) — 5 interfaces
- [IntegrationHealth](#integrationhealth) — 6 interfaces
- [IntegrationPayloads](#integrationpayloads) — 5 interfaces
- [Integrations](#integrations) — 21 interfaces
- [Inventory](#inventory) — 10 interfaces
- [InvoiceTemplate](#invoicetemplate) — 5 interfaces
- [Keyboard](#keyboard) — 5 interfaces
- [Localization](#localization) — 4 interfaces
- [Marketing](#marketing) — 5 interfaces
- [Messaging](#messaging) — 5 interfaces
- [Meta](#meta) — 5 interfaces
- [Migration](#migration) — 5 interfaces
- [MiscRetail](#miscretail) — 5 interfaces
- [MobileAdapters](#mobileadapters) — 5 interfaces
- [Notifications](#notifications) — 8 interfaces
- [Observability](#observability) — 6 interfaces
- [OwnerDashboard](#ownerdashboard) — 5 interfaces
- [PaymentRails](#paymentrails) — 5 interfaces
- [Performance](#performance) — 5 interfaces
- [Platform](#platform) — 4 interfaces
- [Preferences](#preferences) — 8 interfaces
- [PreferencesAdv](#preferencesadv) — 5 interfaces
- [PrinterQueue](#printerqueue) — 5 interfaces
- [Printing](#printing) — 7 interfaces
- [Qc](#qc) — 5 interfaces
- [Qol](#qol) — 5 interfaces
- [Queries](#queries) — 8 interfaces
- [Reconciliation](#reconciliation) — 5 interfaces
- [ReportFormats](#reportformats) — 5 interfaces
- [Reporting](#reporting) — 193 interfaces
- [Resilience](#resilience) — 5 interfaces
- [Returns](#returns) — 5 interfaces
- [Scheduler](#scheduler) — 5 interfaces
- [SearchTyping](#searchtyping) — 5 interfaces
- [Secrets](#secrets) — 5 interfaces
- [Security](#security) — 6 interfaces
- [ShoppingFlow](#shoppingflow) — 5 interfaces
- [ShoppingSession](#shoppingsession) — 5 interfaces
- [StockMisc](#stockmisc) — 5 interfaces
- [TaxBreakdown](#taxbreakdown) — 5 interfaces
- [TextProcessing](#textprocessing) — 6 interfaces
- [UxHints](#uxhints) — 52 interfaces
- [Validation](#validation) — 8 interfaces
- [VendorExtras](#vendorextras) — 5 interfaces
- [Workflows](#workflows) — 5 interfaces

---

## Accessibility

- **`IHighContrastModeToggleService`** — `AccessibilityServices.cs`
- **`IFontSizeScaleService`** — `AccessibilityServices.cs`
- **`IScreenReaderHintService`** — `AccessibilityServices.cs`
- **`IRtlLayoutSupportService`** — `AccessibilityServices.cs`
- **`IColorBlindFriendlyPaletteService`** — `AccessibilityServices.cs`

## Activity

- **`IUserActivityRecorderService`** — `ActivityTrailServices.cs`
- **`IEntityChangeDiffService`** — `ActivityTrailServices.cs`
- **`ICommandHistoryService`** — `ActivityTrailServices.cs`
- **`ISaveAttemptCounterService`** — `ActivityTrailServices.cs`
- **`ICrashReportCollectorService`** — `ActivityTrailServices.cs`

## AiAdapters

- **`IEmbeddingDistanceService`** — `AiAdapterServices.cs`
- **`IKMeansClustererService`** — `AiAdapterServices.cs`
- **`ITfIdfVectorizerService`** — `AiAdapterServices.cs`
- **`ISimpleClassifierService`** — `AiAdapterServices.cs`
- **`IAnomalyScorerService`** — `AiAdapterServices.cs`

## AnalyticsEdge

- **`INthCustomerAnnouncerService`** — `AnalyticsEdgeServices.cs`
- **`ILatestMilestoneCrosserService`** — `AnalyticsEdgeServices.cs`
- **`IStreakDayDetectorService`** — `AnalyticsEdgeServices.cs`
- **`ITrophyCaseService`** — `AnalyticsEdgeServices.cs`
- **`IPeakDayTagService`** — `AnalyticsEdgeServices.cs`

## Auth

- **`ILoginCookieHook`** — Host-specific hook fired after a successful admin PIN verification in LoginViewModel. *(`ILoginCookieHook.cs`)*

## BackupHelpers

- **`IBackupManifestReaderService`** — `BackupHelperServices.cs`
- **`IRestorePreflightCheckerService`** — `BackupHelperServices.cs`
- **`IBackupRotationPlannerService`** — `BackupHelperServices.cs`
- **`IBackupSizeEstimatorService`** — `BackupHelperServices.cs`
- **`IBackupIntegrityProbeService`** — `BackupHelperServices.cs`

## BarcodeQr

- **`IBarcodeGeneratorService`** — `BarcodeQrServices.cs`
- **`IQrCodeDataBuilderService`** — `BarcodeQrServices.cs`
- **`IBarcodeBatchGeneratorService`** — `BarcodeQrServices.cs`
- **`IShelfLabelPayloadService`** — `BarcodeQrServices.cs`
- **`IBarcodeOffsetTrackerService`** — `BarcodeQrServices.cs`

## Batch14

- **`ITrialBalanceBuilderService`** — `AccountingExtras.cs`
- **`IProfitLossBuilderService`** — `AccountingExtras.cs`
- **`IBalanceSheetBuilderService`** — `AccountingExtras.cs`
- **`IDepreciationCalculatorService`** — `AccountingExtras.cs`
- **`IAmortizationScheduleService`** — `AccountingExtras.cs`
- **`IInterestCalculatorService`** — `AccountingExtras.cs`
- **`IExpenseCategoryClassifierService`** — `AccountingExtras.cs`
- **`IVendorAgingBucketizerService`** — `AccountingExtras.cs`
- **`IDebtorAgingBucketizerService`** — `AccountingExtras.cs`
- **`ICapitalizeVsExpenseService`** — `AccountingExtras.cs`
- **`IEWaybillEligibilityService`** — `ComplianceExtras.cs`
- **`IIrnTrackerService`** — `ComplianceExtras.cs`
- **`IAuditTrailEmitterService`** — `ComplianceExtras.cs`
- **`IRetentionPolicyService`** — `ComplianceExtras.cs`
- **`IPiiInventoryService`** — `ComplianceExtras.cs`
- **`ICsvSplitterService`** — `CsvFileManipulation.cs`
- **`ICsvMergerService`** — `CsvFileManipulation.cs`
- **`ICsvSorterService`** — `CsvFileManipulation.cs`
- **`ICsvDedupService`** — `CsvFileManipulation.cs`
- **`ICsvFilterService`** — `CsvFileManipulation.cs`
- **`ICsvAggregatorService`** — `CsvFileManipulation.cs`
- **`IRowVersionCheckerService`** — `DatabaseIntegrity.cs`
- **`IDeadlockDetectorService`** — `DatabaseIntegrity.cs`
- **`ISchemaValidatorService`** — `DatabaseIntegrity.cs`
- **`IHsnDescriptionEnricherService`** — `DataEnrichment.cs`
- **`IProductAttributeInferrerService`** — `DataEnrichment.cs`
- **`IColourNameStandardizerService`** — `DataEnrichment.cs`
- **`ISizeNormalizerService`** — `DataEnrichment.cs`
- **`IBonusCalculatorService`** — `HrExtras.cs`
- **`IProfessionalTaxCalculatorService`** — `HrExtras.cs`
- **`IProvidentFundCalculatorService`** — `HrExtras.cs`
- **`IOvertimeCalculatorService`** — `HrExtras.cs`
- **`ILeaveAccrualCalculatorService`** — `HrExtras.cs`
- **`IShiftSwapRequestService`** — `HrExtras.cs`
- **`ILeavePolicyEngineService`** — `HrExtras.cs`
- **`ICurrencyLocalizerService`** — `LocalizationExtras.cs`
- **`IOrdinalFormatterService`** — `LocalizationExtras.cs`
- **`IDurationFormatterService`** — `LocalizationExtras.cs`
- **`IRelativeTimeFormatterService`** — `LocalizationExtras.cs`
- **`IPluralizationService`** — `LocalizationExtras.cs`
- **`ITranslationCatalogService`** — `LocalizationExtras.cs`
- **`IDiscountCapEnforcerService`** — `MiscRetail2.cs`
- **`IPromoExpiryWarningService`** — `MiscRetail2.cs`
- **`IOfferRedemptionLimiterService`** — `MiscRetail2.cs`
- **`ICustomerBadgeService`** — `MiscRetail2.cs`
- **`IOrderPriorityBadgeService`** — `MiscRetail2.cs`
- **`IFirstSaleOfDayService`** — `MiscRetail2.cs`
- **`IAverageBasketTodayService`** — `MiscRetail2.cs`
- **`ITodayRevenueTrendBadgeService`** — `MiscRetail2.cs`
- **`IReceiptOcrParserService`** — `OcrImageServices.cs`
- **`ICardOcrReaderService`** — `OcrImageServices.cs`
- **`IImageFormatDetectorService`** — `OcrImageServices.cs`
- **`IInvoiceHeaderBuilderService`** — `PdfPrintingExtras.cs`
- **`IInvoiceFooterBuilderService`** — `PdfPrintingExtras.cs`
- **`ITableFormatterService`** — `PdfPrintingExtras.cs`
- **`ITimeZoneResolverService`** — `TimeZoneTimeDate.cs`
- **`IIsBusinessDayService`** — `TimeZoneTimeDate.cs`
- **`INthWeekdayOfMonthService`** — `TimeZoneTimeDate.cs`
- **`ILastWeekdayOfMonthService`** — `TimeZoneTimeDate.cs`
- **`IDaysBetweenHelperService`** — `TimeZoneTimeDate.cs`
- **`IQuarterFromDateHelperService`** — `TimeZoneTimeDate.cs`

## Branch

- **`IBranchRegistryService`** — `BranchSyncServices.cs`
- **`IBranchHandoffService`** — `BranchSyncServices.cs`
- **`IStockTransferManifestService`** — `BranchSyncServices.cs`
- **`IBranchRevenueSnapshotService`** — `BranchSyncServices.cs`
- **`IBranchActiveStaffService`** — `BranchSyncServices.cs`

## Cart

- **`ICartDraftStoreService`** — `CartDomainServices.cs`
- **`ICartTotalRecomputerService`** — `CartDomainServices.cs`
- **`ICartDiscountStackerService`** — `CartDomainServices.cs`
- **`ICartTaxRecomputerService`** — `CartDomainServices.cs`
- **`ICartLineValidationService`** — `CartDomainServices.cs`
- **`ICartQuantityAdjusterService`** — `CartDomainServices.cs`
- **`ICartRemoveItemService`** — `CartDomainServices.cs`

## ClothingRetail

- **`IFabricPairingSuggestionService`** — `ClothingRetailServices.cs`
- **`ISizeAvailabilityMatrixService`** — `ClothingRetailServices.cs`
- **`IFitSubstituteService`** — `ClothingRetailServices.cs`
- **`ISeasonalCategoryRotationService`** — `ClothingRetailServices.cs`
- **`IClothingSizeConverterService`** — `ClothingRetailServices.cs`

## Compliance

- **`ICustomerDataExportService`** — `ComplianceBatchServices.cs`
- **`IDataRetentionReviewService`** — `ComplianceBatchServices.cs`
- **`IPiiMaskingService`** — `ComplianceBatchServices.cs`
- **`ITheftDetectionHeuristicsService`** — `ComplianceBatchServices.cs`
- **`IEInvoiceThresholdCheckerService`** — `ComplianceServices.cs`
- **`ITcsApplicabilityCheckerService`** — `ComplianceServices.cs`
- **`ITdsApplicabilityCheckerService`** — `ComplianceServices.cs`
- **`IRcmApplicabilityCheckerService`** — `ComplianceServices.cs`
- **`IHsnRequirementCheckerService`** — `ComplianceServices.cs`
- **`IGstrReadinessChecklistService`** — `ComplianceServices.cs`

## ConfigValidators

- **`IFirmSettingsValidatorService`** — `ConfigValidatorServices.cs`
- **`IEmailConfigValidatorService`** — `ConfigValidatorServices.cs`
- **`IPrinterConfigValidatorService`** — `ConfigValidatorServices.cs`
- **`IBackupPathValidatorService`** — `ConfigValidatorServices.cs`
- **`ICloudConnectionTesterService`** — `ConfigValidatorServices.cs`

## Core

- **`IAlterationService`** — `AlterationService.cs`
- **`IAttributeService`** — `AttributeService.cs`
- **`IAutoPurchaseOrderService`** — `AutoPurchaseOrderService.cs`
- **`IAutoReorderService`** — `AutoReorderService.cs`
- **`IBillingService`** — Manages the lifecycle of sale invoices: barcode/SKU lookup, product search, cart-to-sale persistence, held bills, returns, cash-register bookkeeping, and sale cancellation. *(`BillingService.cs`)*
- **`IBranchService`** — `BranchService.cs`
- **`IBrandService`** — `BrandService.cs`
- **`ICashMovementService`** — `CashMovementService.cs`
- **`ICategoryService`** — `CategoryService.cs`
- **`ICategoryTypeService`** — `CategoryTypeService.cs`
- **`IColourService`** — `ColourService.cs`
- **`ICustomerDepthService`** — `CustomerDepthService.cs`
- **`ICustomerService`** — Manages customer records, purchase-history lookups, post-sale loyalty updates (visit count, total purchases), and CSV bulk import. *(`CustomerService.cs`)*
- **`IDebtorService`** — Tracks credit-sale debtors, records partial/full payments against outstanding balances, and maintains the customer's outstanding balance in sync. *(`DebtorService.cs`)*
- **`IDiscountEnforcementService`** — `DiscountEnforcementService.cs`
- **`IDiscountService`** — `DiscountService.cs`
- **`IExpenseArchiveService`** — `ExpenseArchiveService.cs`
- **`IExpenseService`** — Manages expense categories and individual expense records, with automatic journal and audit entries on save and category-wise/date-range aggregation queries. *(`ExpenseService.cs`)*
- **`IFirmService`** — Manages the store's firm/business details (name, GSTIN, bank info, invoice settings) and keeps AppConfig. *(`FirmService.cs`)*
- **`IGrnService`** — `GrnService.cs`
- **`IHsnService`** — `HsnService.cs`
- **`IInventoryLedgerService`** — `InventoryLedgerService.cs`
- **`IInwardService`** — Tracks inward goods receipts (parcels from vendors), auto-updates vendor balances and ledger entries on save, and enforces credit-limit/overdue warnings. *(`InwardService.cs`)*
- **`IIroningService`** — Tracks outsourced ironing jobs: creation, cost tracking, and received-status updates for garments sent to external ironers. *(`IroningService.cs`)*
- **`IJournalService`** — `JournalService.cs`
- **`IOrderService`** — Manages custom/advance orders: sequential order-number generation, CRUD, status transitions, and automatic journal and audit entries on save. *(`OrderService.cs`)*
- **`IPaymentMethodService`** — `PaymentMethodService.cs`
- **`IPettyCashService`** — `PettyCashService.cs`
- **`IPriceHistoryService`** — `PriceHistoryService.cs`
- **`IProductService`** — Provides CRUD operations for the product catalog, including soft-delete/restore, duplicate-name checking, and cache invalidation. *(`ProductService.cs`)*
- **`IProductVariantService`** — `ProductVariantService.cs`
- **`IPurchaseOrderService`** — `PurchaseOrderService.cs`
- **`IPurchaseReturnService`** — `PurchaseReturnService.cs`
- **`IQuotationService`** — `QuotationService.cs`
- **`IRecurringExpenseService`** — `RecurringExpenseService.cs`
- **`ISalaryFormulaEngine`** — Per-staff-member salary formula input variables. *(`SalaryFormulaEngine.cs`)*
- **`ISalaryService`** — Generates monthly salary records for active staff, computing base salary plus sales-based commission, and tracks bonus/deduction adjustments and payment status. *(`SalaryService.cs`)*
- **`ISizeService`** — `SizeService.cs`
- **`IStaffService`** — Provides CRUD and soft-delete/restore operations for staff members, including commission percentage and salary configuration. *(`StaffService.cs`)*
- **`IStockAdjustmentService`** — `StockAdjustmentService.cs`
- **`IStockAlertService`** — `StockAlertService.cs`
- **`IStockService`** — Manages stock entries and their constituent stock items (SKUs), including quantity adjustments, price-history tracking, and inventory ledger integration via IInventoryLedgerService. *(`StockService.cs`)*
- **`IStockTakeService`** — `StockTakeService.cs`
- **`ITaskService`** — `TaskService.cs`
- **`ITaxService`** — Manages GST tax rates, slab-based rate resolution, tax configuration (inclusive/exclusive), and provides rate lookup used by billing to auto-pick the correct tax slab at a given price. *(`TaxService.cs`)*
- **`IVendorLedgerService`** — `VendorLedgerService.cs`
- **`IVendorPerformanceService`** — `VendorPerformanceService.cs`
- **`IVendorService`** — Manages vendor records, balance tracking, payment history, and credit-limit/overdue validation. *(`VendorService.cs`)*

## CustomerEngagement

- **`ICustomerSegmentBuilderService`** — `CustomerEngagementServices.cs`
- **`ICustomerTagStoreService`** — `CustomerEngagementServices.cs`
- **`ICustomerPreferenceSnapshotService`** — `CustomerEngagementServices.cs`
- **`IGiftVoucherBalanceService`** — `CustomerEngagementServices.cs`
- **`ILoyaltyPointCalculatorService`** — `CustomerEngagementServices.cs`
- **`IFollowUpReminderService`** — `CustomerEngagementServices.cs`

## CustomerProfile

- **`IAgeDerivationService`** — `CustomerProfileHelperServices.cs`
- **`ICustomerTierBadgeService`** — `CustomerProfileHelperServices.cs`
- **`ICustomerIcebreakerService`** — `CustomerProfileHelperServices.cs`
- **`ICustomerVisitHistorySparkService`** — `CustomerProfileHelperServices.cs`
- **`ICustomerStickyNoteService`** — `CustomerProfileHelperServices.cs`

## DataValidators2

- **`IAmountRangeValidatorService`** — `DataValidatorServices2.cs`
- **`IDateRangeValidatorService`** — `DataValidatorServices2.cs`
- **`ITextLengthValidatorService`** — `DataValidatorServices2.cs`
- **`INumericSequenceValidatorService`** — `DataValidatorServices2.cs`
- **`IEnumValueValidatorService`** — `DataValidatorServices2.cs`

## DebugTooling

- **`IDebugLogTailService`** — `DebugToolingServices.cs`
- **`IStackTraceFormatterService`** — `DebugToolingServices.cs`
- **`IPerfFlagEnablerService`** — `DebugToolingServices.cs`
- **`IHotReloadHintService`** — `DebugToolingServices.cs`
- **`IAssemblyVersionReporterService`** — `DebugToolingServices.cs`

## Delivery

- **`IDeliverySlotService`** — `DeliveryLogisticsServices.cs`
- **`IDeliveryRouteOrderService`** — `DeliveryLogisticsServices.cs`
- **`IPackageSizeEstimatorService`** — `DeliveryLogisticsServices.cs`
- **`ICourierRateLookupService`** — `DeliveryLogisticsServices.cs`
- **`ITrackingNumberFormatterService`** — `DeliveryLogisticsServices.cs`

## Diagnostics

- **`ISchemaVersionReporter`** — `DiagnosticProbeServices.cs`
- **`ITableRowCountReporter`** — `DiagnosticProbeServices.cs`
- **`ILargestTablesReporter`** — `DiagnosticProbeServices.cs`
- **`IIndexInventoryReporter`** — `DiagnosticProbeServices.cs`
- **`ISqliteSizeProbeService`** — `DiagnosticProbeServices.cs`

## Events

- **`IInProcessEventBusService`** — `DomainEventServices.cs`
- **`ISaleFinalizedSignalService`** — `DomainEventServices.cs`
- **`IStockLowSignalService`** — `DomainEventServices.cs`
- **`IDebtorCreatedSignalService`** — `DomainEventServices.cs`
- **`ICatalogChangedSignalService`** — `DomainEventServices.cs`

## FeatureFlags

- **`IFeatureGateEvaluatorService`** — `FeatureFlagHelperServices.cs`
- **`IOverrideScopeResolverService`** — `FeatureFlagHelperServices.cs`
- **`IKillSwitchRegistryService`** — `FeatureFlagHelperServices.cs`
- **`IRolloutPercentService`** — `FeatureFlagHelperServices.cs`
- **`IDeprecationWarningRegistryService`** — `FeatureFlagHelperServices.cs`

## Files

- **`ISafeFileMoveService`** — `FileHandlingServices.cs`
- **`ITempFileCleanupService`** — `FileHandlingServices.cs`
- **`IFileHashService`** — `FileHandlingServices.cs`
- **`IDiskSpaceReporterService`** — `FileHandlingServices.cs`
- **`IPathSanitizerService`** — `FileHandlingServices.cs`

## FuzzyMatch

- **`ITextSimilarityService`** — `FuzzyMatchServices.cs`
- **`IFuzzyCustomerMatchService`** — `FuzzyMatchServices.cs`
- **`IFuzzyProductMatchService`** — `FuzzyMatchServices.cs`
- **`IMinHashSignatureService`** — `FuzzyMatchServices.cs`
- **`ITrigramIndexService`** — `FuzzyMatchServices.cs`

## Helpers

- **`IHolidayCalendarService`** — `BusinessCalendarServices.cs`
- **`IPaydaySuggesterService`** — `BusinessCalendarServices.cs`
- **`IGstFilingDueDateService`** — `BusinessCalendarServices.cs`
- **`IFinancialYearStartResolverService`** — `BusinessCalendarServices.cs`
- **`IBillPaymentDueDateCalculatorService`** — `BusinessCalendarServices.cs`
- **`IAddressNormalizerService`** — `DataNormalizerServices.cs`
- **`INameTitleCaseService`** — `DataNormalizerServices.cs`
- **`IWhitespaceNormalizerService`** — `DataNormalizerServices.cs`
- **`IEmojiStripperService`** — `DataNormalizerServices.cs`
- **`IEmailDomainExtractorService`** — `DataNormalizerServices.cs`
- **`IUrlCanonicalizerService`** — `DataNormalizerServices.cs`
- **`ISuggestedPaymentSplitService`** — `FlowHelperServices.cs`
- **`IChangeDueCalculator`** — `FlowHelperServices.cs`
- **`IHeldBillRecallOrderService`** — `FlowHelperServices.cs`
- **`ILoyaltyDiscountCalculator`** — `FlowHelperServices.cs`
- **`ILowStockReplenishmentQuantityService`** — `FlowHelperServices.cs`
- **`ISafetyStockCalculator`** — `FlowHelperServices.cs`
- **`IReorderPointCalculator`** — `FlowHelperServices.cs`
- **`IAbcClassificationService`** — `FlowHelperServices.cs`
- **`IInventoryTurnoverByVendorService`** — `FlowHelperServices.cs`
- **`IIndianCurrencyFormatter`** — `FormattingHelperServices.cs`
- **`IPhoneNumberNormalizer`** — `FormattingHelperServices.cs`
- **`IGstinChecksumValidator`** — `FormattingHelperServices.cs`
- **`IHsnFormatValidator`** — `FormattingHelperServices.cs`
- **`IPincodeValidator`** — `FormattingHelperServices.cs`
- **`ISkuPatternGenerator`** — `FormattingHelperServices.cs`
- **`IInvoiceNumberFormatValidator`** — `FormattingHelperServices.cs`
- **`IPoNumberFormatValidator`** — `FormattingHelperServices.cs`
- **`IBarcodeFormatValidator`** — `FormattingHelperServices.cs`
- **`IColourHexNormalizer`** — `FormattingHelperServices.cs`
- **`IRupeesInWordsService`** — `FormattingMiscServices.cs`
- **`ITaxInWordsService`** — `FormattingMiscServices.cs`
- **`INumberToWordsService`** — `FormattingMiscServices.cs`
- **`IFractionSeparatorService`** — `FormattingMiscServices.cs`
- **`IAnonymizerService`** — `FormattingMiscServices.cs`
- **`IShortIdGeneratorService`** — `FormattingMiscServices.cs`
- **`ISlugService`** — `FormattingMiscServices.cs`
- **`IUrlShortenerStubService`** — `FormattingMiscServices.cs`
- **`IClipboardPayloadFormatterService`** — `FormattingMiscServices.cs`
- **`IHindiRupeesInWordsService`** — ROADMAP #214 (partial) + #218 Hindi variant — rupees-in-words in Hindi using the Indian Lakh/Crore system. *(`HindiRupeesInWordsService.cs`)*
- **`IIfscCodeValidatorService`** — `IdValidatorServices.cs`
- **`IPanValidatorService`** — `IdValidatorServices.cs`
- **`IAadhaarMaskValidatorService`** — `IdValidatorServices.cs`
- **`IBankAccountValidatorService`** — `IdValidatorServices.cs`
- **`IUpiIdValidatorService`** — `IdValidatorServices.cs`
- **`IChequeNumberValidatorService`** — `IdValidatorServices.cs`
- **`ICsvRowNormalizerService`** — `ImportHelperServices.cs`
- **`IJsonPatchApplicator`** — `ImportHelperServices.cs`
- **`IDuplicateRowDetectorService`** — `ImportHelperServices.cs`
- **`ITaxableValueCalculator`** — `PricingCalculators.cs`
- **`IRoundOffCalculator`** — `PricingCalculators.cs`
- **`IMarkupPriceCalculator`** — `PricingCalculators.cs`
- **`IReverseTaxCalculator`** — `PricingCalculators.cs`
- **`ILineDiscountCalculator`** — `PricingCalculators.cs`
- **`IBillDiscountDistributor`** — `PricingCalculators.cs`
- **`IExchangeCreditCalculator`** — `PricingCalculators.cs`
- **`IReturnRefundCalculator`** — `PricingCalculators.cs`
- **`IDateRangeResolver`** — `UtilityServices.cs`
- **`IFiscalYearResolver`** — `UtilityServices.cs`
- **`IStateCodeLookup`** — `UtilityServices.cs`
- **`IGstSplitCalculator`** — `UtilityServices.cs`
- **`IWeekNumberResolver`** — `UtilityServices.cs`
- **`IHolidayDetector`** — `UtilityServices.cs`
- **`IBusinessHoursChecker`** — `UtilityServices.cs`

## Hr

- **`IShiftHandoverChecklistService`** — `OpsBatchServices.cs`
- **`IQualityChecklistService`** — `OpsBatchServices.cs`
- **`IExpenseApprovalQueueService`** — `OpsBatchServices.cs`
- **`IStaffKpiAttainmentService`** — `OpsBatchServices.cs`
- **`IStaffAttendanceRollCallService`** — `StaffHrServices.cs`
- **`IStaffShiftTemplateService`** — `StaffHrServices.cs`
- **`IStaffCommissionCalculatorService`** — `StaffHrServices.cs`
- **`IStaffLeaveBalanceService`** — `StaffHrServices.cs`
- **`IStaffOnboardingChecklistService`** — `StaffHrServices.cs`

## Hygiene

- **`IDuplicateProductMergeSuggestionService`** — `HygieneSuggestionServices.cs`
- **`IOrphanStockEntryCleanupSuggestionService`** — `HygieneSuggestionServices.cs`
- **`IDuplicatePaymentDetectorService`** — `HygieneSuggestionServices.cs`
- **`IZombieCustomerCleanupSuggestionService`** — `HygieneSuggestionServices.cs`
- **`IStaleDraftSaleCleanupSuggestionService`** — `HygieneSuggestionServices.cs`
- **`IUnusedCategoryCleanupSuggestionService`** — `HygieneSuggestionServices.cs`

## ImportPipelines

- **`ITallyXmlReaderService`** — `ImportPipelineServices.cs`
- **`IBusyCsvReaderService`** — `ImportPipelineServices.cs`
- **`ITallyXmlWriterService`** — `ImportPipelineServices.cs`
- **`IImportMappingService`** — `ImportPipelineServices.cs`
- **`IColumnTypeInferencerService`** — `ImportPipelineServices.cs`
- **`IDateFormatDetectorService`** — `ImportPipelineServices.cs`

## Indexes

- **`IProductNameIndexService`** — `IndexServices.cs`
- **`ICustomerPhoneIndexService`** — `IndexServices.cs`
- **`IVendorPhoneIndexService`** — `IndexServices.cs`
- **`ISkuBarcodeIndexService`** — `IndexServices.cs`
- **`IInvoiceNumberIndexService`** — `IndexServices.cs`
- **`IRecentSearchesService`** — `IndexServices.cs`

## Infrastructure

- **`IAccountantShareService`** — `AccountantShareService.cs`
- **`IArchiveService`** — `ArchiveService.cs`
- **`IAuditServiceSharedContext`** — Records tamper-evident audit log entries with HMAC-SHA256 hash chaining, supports chain verification/resealing, and provides time-based purge of old entries. *(`AuditService.cs`)*
- **`IAuditService`** — `AuditService.cs`
- **`ISecretKeyProvider`** — `AuditService.cs`
- **`IAuthorizationService`** — `AuthorizationService.cs`
- **`IAutoBackupService`** — `AutoBackupService.cs`
- **`IAutoLogoutService`** — `AutoLogoutService.cs`
- **`IAutomationService`** — `AutomationService.cs`
- **`IBackgroundJobCoordinator`** — `BackgroundJobCoordinator.cs`
- **`IBackupService`** — Provides encrypted SQLite database backup, restore (with integrity validation and rollback), factory reset, automatic on-close backup with retention policy, and cloud upload. *(`BackupService.cs`)*
- **`IBackupTieredService`** — `BackupTieredService.cs`
- **`IBlazorNavigator`** — Bridges shell-level navigation into the Razor router. *(`BlazorNavigator.cs`)*
- **`IBusinessConfigService`** — `BusinessConfigService.cs`
- **`ICacheService`** — In-memory cache for frequently accessed master data. *(`CacheService.cs`)*
- **`IClockService`** — `ClockService.cs`
- **`IConfigurationService`** — `ConfigurationService.cs`
- **`ICredentialStoreService`** — `CredentialStoreService.cs`
- **`IDispatcherService`** — Platform-agnostic UI thread dispatcher abstraction. *(`DispatcherService.cs`)*
- **`IDuplicateDetectionService`** — `DuplicateDetectionService.cs`
- **`IFeatureFlagService`** — `FeatureFlagService.cs`
- **`IFinancialYearService`** — Resolves FY boundaries from AppConfig. *(`FinancialYearService.cs`)*
- **`IFyOpsService`** — `FyOpsService.cs`
- **`IFyRolloverService`** — `FyRolloverService.cs`
- **`IGlobalSearchService`** — Provides cross-entity fuzzy search across products, vendors, staff, sales, stock, inwards, debtors, and expenses with smart number routing, Hindi keyword translation, and quick-action suggestions. *(`GlobalSearchService.cs`)*
- **`IThermalReceiptPrinterService`** — `HardwareDriverServices.cs`
- **`IHardwareDriverRegistry`** — `HardwareDriverServices.cs`
- **`ILabelPrinterService`** — `HardwareHalServices.cs`
- **`IBarcodeScannerService`** — `HardwareHalServices.cs`
- **`INfcReaderService`** — `HardwareHalServices.cs`
- **`ICashDrawerService`** — `HardwareHalServices.cs`
- **`ICustomerDisplayService`** — `HardwareHalServices.cs`
- **`IBatteryStateService`** — `HardwareHalServices.cs`
- **`IFootfallSensorService`** — `HardwareHalServices.cs`
- **`IAmbientControlService`** — `HardwareHalServices.cs`
- **`IWebcamService`** — `HardwareHalServices.cs`
- **`IHardwareInventoryService`** — `HardwareHalServices.cs`
- **`IHotkeyBridge`** — Routes keyboard shortcuts into the active Razor page. *(`HotkeyBridge.cs`)*
- **`IAccessibilityService`** — OS-level accessibility hints the UI reads at render time. *(`IAccessibilityService.cs`)*
- **`IClipboardService`** — Platform-agnostic clipboard abstraction. *(`IClipboardService.cs`)*
- **`IConfirmationService`** — Portable confirm/info prompt surface. *(`IConfirmationService.cs`)*
- **`IFilePickerService`** — Native file picker abstraction. *(`IFilePickerService.cs`)*
- **`IGlobalHotkeyService`** — System-wide hotkey abstraction. *(`IGlobalHotkeyService.cs`)*
- **`IHostShutdownService`** — Graceful shell teardown. *(`IHostShutdownService.cs`)*
- **`IMasterPinService`** — Master-PIN gate before destructive operations (delete vendor with history, delete bill, factory reset, large discount override). *(`IMasterPinService.cs`)*
- **`IPageSimulatorStrategy`** — Encapsulates the logic for simulating a single page's data for the PageSimulatorService. *(`IPageSimulatorStrategy.cs`)*
- **`IPrintService`** — Print abstraction. *(`IPrintService.cs`)*
- **`IRoleBroadcaster`** — Audit B1 — per-tab SessionId (DESIGN-PER-TAB-SESSION. *(`IRoleBroadcaster.cs`)*
- **`ISoundService`** — UI feedback sounds (scan beep, sale confirm, error). *(`ISoundService.cs`)*
- **`IJobCoordinator`** — Durable idempotency coordinator for recurring / startup / scheduled jobs. *(`JobCoordinator.cs`)*
- **`IJobProgressService`** — §111 — Observable background-job progress. *(`JobProgressService.cs`)*
- **`IKeyboardHintsService`** — Per-page keyboard-shortcut advertisement. *(`KeyboardHintsService.cs`)*
- **`ILastSeenService`** — Tracks the timestamp at which the current station last closed the app and returns a "what changed since then" summary on next open. *(`LastSeenService.cs`)*
- **`IMasterConfirmationStore`** — `MasterConfirmationStore.cs`
- **`IMasterPinState`** — Observable process-wide state for the Master PIN privilege window. *(`MasterPinState.cs`)*
- **`IMobileTokenService`** — Per-device bearer-token issuer for the mobile dashboard. *(`MobileTokenService.cs`)*
- **`INotificationService`** — `NotificationService.cs`
- **`IOperationalAlertService`** — `OperationalAlertService.cs`
- **`IPageSimulatorService`** — `PageSimulatorService.cs`
- **`IPermissionService`** — `PermissionService.cs`
- **`IPredictivePrecomputeCache`** — §113 — A tiny in-process cache for precomputed values that a few UI surfaces hit hard (Dashboard, Workbench, Universal Search ranking). *(`PredictivePrecomputeCacheService.cs`)*
- **`IBranchContextService`** — `ReadinessFrameServices.cs`
- **`IPlugin`** — `ReadinessFrameServices.cs`
- **`IPluginHost`** — `ReadinessFrameServices.cs`
- **`IMobileSurfaceContract`** — `ReadinessFrameServices.cs`
- **`IOAuthFlowService`** — `ReadinessFrameServices.cs`
- **`IRepairService`** — `RepairService.cs`
- **`IResumableUploadService`** — Resumable cloud-upload client. *(`ResumableUploadService.cs`)*
- **`ISessionDefaultsService`** — §140 — Remembers the user's last-used value per named key for the lifetime of a session. *(`SessionDefaultsService.cs`)*
- **`ISessionService`** — Tracks the current user session's role (Operator, Admin, Master) and unique session ID, and raises RoleChanged events on privilege escalation or de-escalation. *(`SessionService.cs`)*
- **`ISessionTimeoutService`** — Central 1-second tick service that drives all session-timing observers. *(`SessionTimeoutService.cs`)*
- **`IStationIdentityService`** — `StationIdentityService.cs`
- **`ISupportBundleService`** — `SupportBundleService.cs`
- **`ISyntheticDataSeederService`** — `SyntheticDataSeederService.cs`
- **`ISystemHealthService`** — `SystemHealthService.cs`
- **`IThemeService`** — `ThemeService.cs`
- **`IUnsavedChangesService`** — Tracks unsaved changes across pages. *(`UnsavedChangesService.cs`)*
- **`IWorkflowTimingMetrics`** — §179 — Lightweight, in-memory workflow-timing collector. *(`WorkflowTimingMetricsService.cs`)*

## Input

- **`IInputMaskService`** — `InputRuleServices.cs`
- **`ITypeaheadFilterService`** — `InputRuleServices.cs`
- **`IAutocompleteDebouncerService`** — `InputRuleServices.cs`
- **`IPasteRecognizerService`** — `InputRuleServices.cs`
- **`IClipboardScrubberService`** — `InputRuleServices.cs`

## IntegrationHealth

- **`IIntegrationStatusBase`** — `IntegrationHealthServices.cs`
- **`IWhatsAppHealthCheckService`** — `IntegrationHealthServices.cs`
- **`ISmsGatewayHealthCheckService`** — `IntegrationHealthServices.cs`
- **`ITallySyncStatusService`** — `IntegrationHealthServices.cs`
- **`IUpiGatewayHealthService`** — `IntegrationHealthServices.cs`
- **`IGoogleSheetsHealthService`** — `IntegrationHealthServices.cs`

## IntegrationPayloads

- **`IWhatsAppPayloadBuilderService`** — `IntegrationPayloadServices.cs`
- **`ISmsPayloadBuilderService`** — `IntegrationPayloadServices.cs`
- **`IEmailPayloadBuilderService`** — `IntegrationPayloadServices.cs`
- **`IPushNotificationPayloadBuilderService`** — `IntegrationPayloadServices.cs`
- **`IWebhookPayloadBuilderService`** — `IntegrationPayloadServices.cs`

## Integrations

- **`ICustomerOutreachOrchestrator`** — `CustomerOutreachOrchestrator.cs`
- **`IDailyOwnerDigestService`** — `DailyOwnerDigestService.cs`
- **`IEmailProviderService`** — `EmailProviderService.cs`
- **`ITallyService`** — `IntegrationsBatchServices.cs`
- **`IWeatherService`** — `IntegrationsBatchServices.cs`
- **`IShiprocketService`** — `IntegrationsBatchServices.cs`
- **`IGoogleMyBusinessService`** — `IntegrationsBatchServices.cs`
- **`IAccountingExportService`** — `IntegrationsBatchServices.cs`
- **`IMarketplaceSyncService`** — `IntegrationsBatchServices.cs`
- **`IMetaShopService`** — `IntegrationsBatchServices.cs`
- **`IReviewPlatformService`** — `IntegrationsBatchServices.cs`
- **`IInsuranceIntegrationService`** — `IntegrationsBatchServices.cs`
- **`IShopifyStorefrontService`** — `IntegrationsBatchServices.cs`
- **`IBankStatementService`** — `IntegrationsBatchServices.cs`
- **`IIntegrationTesterService`** — `IntegrationTesterService.cs`
- **`ISupplyChainOrchestrator`** — `OrchestratorBatchServices.cs`
- **`IFinancialCloseOrchestrator`** — `OrchestratorBatchServices.cs`
- **`ICustomerJourneyService`** — `OrchestratorBatchServices.cs`
- **`IRazorpayService`** — `RazorpayService.cs`
- **`ISmsProviderService`** — `SmsProviderService.cs`
- **`IWhatsAppBusinessService`** — `WhatsAppBusinessService.cs`

## Inventory

- **`IStockSplitPreviewService`** — `InventoryExpansionServices.cs`
- **`IStockMergePreviewService`** — `InventoryExpansionServices.cs`
- **`IStockTransferPreviewService`** — `InventoryExpansionServices.cs`
- **`IExpiryTrackerService`** — `InventoryExpansionServices.cs`
- **`IWarrantyCountdownService`** — `InventoryExpansionServices.cs`
- **`IBarcodeToStockItemService`** — `InventoryMicroServices.cs`
- **`ISkuSplitterService`** — `InventoryMicroServices.cs`
- **`IStockLevelBucketService`** — `InventoryMicroServices.cs`
- **`IReorderCostEstimatorService`** — `InventoryMicroServices.cs`
- **`IStockValuationSnapshotService`** — `InventoryMicroServices.cs`

## InvoiceTemplate

- **`IInvoiceFooterTextService`** — `InvoiceTemplateServices.cs`
- **`IInvoiceQrPayloadService`** — `InvoiceTemplateServices.cs`
- **`IInvoiceWatermarkService`** — `InvoiceTemplateServices.cs`
- **`IInvoicePaperSizeChooserService`** — `InvoiceTemplateServices.cs`
- **`IInvoiceBrandingPaletteService`** — `InvoiceTemplateServices.cs`

## Keyboard

- **`IKeyChordParserService`** — `KeyboardInputServices.cs`
- **`IKeyChordMatcherService`** — `KeyboardInputServices.cs`
- **`IPaletteCommandHistoryService`** — `KeyboardInputServices.cs`
- **`IInputShortcutSuggesterService`** — `KeyboardInputServices.cs`
- **`IDoubleClickGuardService`** — `KeyboardInputServices.cs`

## Localization

- **`IHindiNumeralFormatterService`** — `LocalizationServices.cs`
- **`IStateNameLocalizerService`** — `LocalizationServices.cs`
- **`IMonthNameLocalizerService`** — `LocalizationServices.cs`
- **`IUiStringLocalizer`** — ROADMAP #214 (slice 1) — lightweight UI-string localizer. *(`UiStringLocalizer.cs`)*

## Marketing

- **`ICampaignStartEligibilityService`** — `MarketingServices.cs`
- **`ISmsCampaignBuilderService`** — `MarketingServices.cs`
- **`IOfferTemplateRendererService`** — `MarketingServices.cs`
- **`ILandingPageSlugService`** — `MarketingServices.cs`
- **`IReferralCodeGeneratorService`** — `MarketingServices.cs`

## Messaging

- **`IWhatsAppLinkBuilderService`** — `MessagingLinkServices.cs`
- **`ISmsLinkBuilderService`** — `MessagingLinkServices.cs`
- **`IEmailLinkBuilderService`** — `MessagingLinkServices.cs`
- **`IMessageTemplateService`** — `MessagingLinkServices.cs`
- **`IMessagePlaceholderResolverService`** — `MessagingLinkServices.cs`

## Meta

- **`ILoadingMessageService`** — `MetaBatchServices.cs`
- **`ICrashReportEnvelopeService`** — `MetaBatchServices.cs`
- **`INpsScoreCalculatorService`** — `MetaBatchServices.cs`
- **`IPerformanceBudgetService`** — `PerformanceBudgetService.cs`
- **`IReleaseNotesService`** — `ReleaseNotesService.cs`

## Migration

- **`ISchemaDriftReporterService`** — `MigrationHelperServices.cs`
- **`IMissingForeignKeyDetectorService`** — `MigrationHelperServices.cs`
- **`ILegacyFieldCoercionService`** — `MigrationHelperServices.cs`
- **`ISchemaGenerationBlueprintService`** — `MigrationHelperServices.cs`
- **`IBulkUpdatePreviewService`** — `MigrationHelperServices.cs`

## MiscRetail

- **`IUnitConverterService`** — `MiscRetailServices.cs`
- **`IGstRateBuiltinLookupService`** — `MiscRetailServices.cs`
- **`IDuplicateInvoiceNumberDetectorService`** — `MiscRetailServices.cs`
- **`ISaleWithNoCustomerDetectorService`** — `MiscRetailServices.cs`
- **`IFloorLabelResolverService`** — `MiscRetailServices.cs`

## MobileAdapters

- **`IMobileSyncPayloadBuilderService`** — `MobileAdapterServices.cs`
- **`IMobileDeltaSignerService`** — `MobileAdapterServices.cs`
- **`IMobilePushTokenRegistryService`** — `MobileAdapterServices.cs`
- **`IMobileClientVersionGuardService`** — `MobileAdapterServices.cs`
- **`IMobileConcurrentSessionLimiterService`** — `MobileAdapterServices.cs`

## Notifications

- **`ILowStockNotifier`** — `NotificationGeneratorServices.cs`
- **`IDebtorDueNotifier`** — `NotificationGeneratorServices.cs`
- **`INewDayOpeningNotifier`** — `NotificationGeneratorServices.cs`
- **`IBigBillNotifier`** — `NotificationGeneratorServices.cs`
- **`IStaleHeldBillNotifier`** — `NotificationGeneratorServices.cs`
- **`IBackupOverdueNotifier`** — `NotificationGeneratorServices.cs`
- **`IPriceDriftNotifier`** — `NotificationGeneratorServices.cs`
- **`IDiscountBreachNotifier`** — `NotificationGeneratorServices.cs`

## Observability

- **`IHeartbeatService`** — `ObservabilityServices.cs`
- **`IHealthCheckRunnerService`** — `ObservabilityServices.cs`
- **`IMetricCounterService`** — `ObservabilityServices.cs`
- **`IMetricHistogramService`** — `ObservabilityServices.cs`
- **`ITelemetrySessionService`** — `ObservabilityServices.cs`
- **`IDiagnosticSnapshotBundleService`** — `ObservabilityServices.cs`

## OwnerDashboard

- **`IOwnerKpiSnapshotService`** — `OwnerDashboardServices.cs`
- **`IOwnerAlertStreamService`** — `OwnerDashboardServices.cs`
- **`IOwnerTodoGeneratorService`** — `OwnerDashboardServices.cs`
- **`IOwnerGreetingService`** — `OwnerDashboardServices.cs`
- **`IOwnerWeekInReviewService`** — `OwnerDashboardServices.cs`

## PaymentRails

- **`IUpiVpaValidatorService`** — `PaymentRailServices.cs`
- **`IPaymentRetryQueueService`** — `PaymentRailServices.cs`
- **`ISettlementCalendarService`** — `PaymentRailServices.cs`
- **`IRefundReasonCatalogService`** — `PaymentRailServices.cs`
- **`IPaymentInstructionsService`** — `PaymentRailServices.cs`

## Performance

- **`IPerfBudgetCheckerService`** — `PerfTimingServices.cs`
- **`ISlowQueryFlagService`** — `PerfTimingServices.cs`
- **`IHttpLatencyBudgetService`** — `PerfTimingServices.cs`
- **`IBackgroundJobBurndownService`** — `PerfTimingServices.cs`
- **`IStartupSequenceTimingService`** — `PerfTimingServices.cs`

## Platform

- **`IDataDictionaryService`** — `PlatformBatchServices.cs`
- **`ITelemetryConsentService`** — `PlatformBatchServices.cs`
- **`IWebhookSubscriptionRegistry`** — `PlatformBatchServices.cs`
- **`IMemoryBudgetService`** — `PlatformBatchServices.cs`

## Preferences

- **`IUserPreferenceService`** — `PreferenceServices.cs`
- **`IDefaultPaymentModePreference`** — `PreferenceServices.cs`
- **`IDefaultCategoryPreference`** — `PreferenceServices.cs`
- **`IPrintPreferenceService`** — `PreferenceServices.cs`
- **`IDashboardLayoutPreferenceService`** — `PreferenceServices.cs`
- **`IKeyboardShortcutPreferenceService`** — `PreferenceServices.cs`
- **`ICurrencyDisplayPreferenceService`** — `PreferenceServices.cs`
- **`IDateFormatPreferenceService`** — `PreferenceServices.cs`

## PreferencesAdv

- **`INotificationSoundPreferenceService`** — `PreferencesAdvancedServices.cs`
- **`ICompactModePreferenceService`** — `PreferencesAdvancedServices.cs`
- **`IAutoFocusPreferenceService`** — `PreferencesAdvancedServices.cs`
- **`ILanguagePreferenceService`** — `PreferencesAdvancedServices.cs`
- **`ISidebarOrderPreferenceService`** — `PreferencesAdvancedServices.cs`

## PrinterQueue

- **`IPrintJobQueueService`** — `PrinterQueueServices.cs`
- **`IPrinterStatusProbeService`** — `PrinterQueueServices.cs`
- **`IPrintTemplateCacheService`** — `PrinterQueueServices.cs`
- **`IBatchPrintOrchestratorService`** — `PrinterQueueServices.cs`
- **`IPrintReconciliationService`** — `PrinterQueueServices.cs`

## Printing

- **`IReceiptTextFormatter`** — `PrintFormatterServices.cs`
- **`IThermalPrinterFormatService`** — `PrintFormatterServices.cs`
- **`IInvoiceHtmlRendererService`** — `PrintFormatterServices.cs`
- **`ICsvExportWriterService`** — `PrintFormatterServices.cs`
- **`IExcelWorkbookBuilderService`** — `PrintFormatterServices.cs`
- **`IPdfLayoutStubService`** — `PrintFormatterServices.cs`
- **`IBarcodeLabelBuilderService`** — `PrintFormatterServices.cs`

## Qc

- **`IDamagedGoodsFlagService`** — `QcServices.cs`
- **`IQuickQcChecklistService`** — `QcServices.cs`
- **`ICounterfeitRiskService`** — `QcServices.cs`
- **`IVendorRejectLogService`** — `QcServices.cs`
- **`IStockQualityBadgeService`** — `QcServices.cs`

## Qol

- **`IUndoableActionRegistryService`** — `QolServices.cs`
- **`ILastActionFooterService`** — `QolServices.cs`
- **`IFirstUseDetectorService`** — `QolServices.cs`
- **`IOfflineSignalService`** — `QolServices.cs`
- **`ITouchscreenTapRecognizerService`** — `QolServices.cs`

## Queries

- **`IQuickProductSearchService`** — `QueryServices.cs`
- **`IRecentSalesSearchService`** — `QueryServices.cs`
- **`ICustomerByPhoneService`** — `QueryServices.cs`
- **`IVendorByGstinService`** — `QueryServices.cs`
- **`IOpenOrdersQueryService`** — `QueryServices.cs`
- **`ITopSellersQueryService`** — `QueryServices.cs`
- **`ILowMarginSkuQueryService`** — `QueryServices.cs`
- **`IInactiveStaffQueryService`** — `QueryServices.cs`

## Reconciliation

- **`IBankStatementReconcilerService`** — `ReconcilerServices.cs`
- **`IPaymentMatchHeuristicService`** — `ReconcilerServices.cs`
- **`ISaleVsBankReconcilerService`** — `ReconcilerServices.cs`
- **`IDailyTotalReconcilerService`** — `ReconcilerServices.cs`
- **`IDebtorReconcilerService`** — `ReconcilerServices.cs`

## ReportFormats

- **`IPngChartStubService`** — `ReportFormatServices.cs`
- **`ISvgChartBuilderService`** — `ReportFormatServices.cs`
- **`IJsonReportExporterService`** — `ReportFormatServices.cs`
- **`IMarkdownReportBuilderService`** — `ReportFormatServices.cs`
- **`IPlainTextReportBuilderService`** — `ReportFormatServices.cs`

## Reporting

- **`IAdminDataHealthReportService`** — §219 — Admin data-health report. *(`AdminDataHealthReportService.cs`)*
- **`IInvoiceNumberingIntegrityService`** — `AdvancedReportServices.cs`
- **`IRoundOffImpactService`** — `AdvancedReportServices.cs`
- **`IDiscountEffectivenessService`** — `AdvancedReportServices.cs`
- **`ICashFlowAnomalyService`** — `AdvancedReportServices.cs`
- **`IStaffAttendanceInferenceService`** — `AdvancedReportServices.cs`
- **`ISlowBillPatternService`** — `AdvancedReportServices.cs`
- **`IConvertedAbandonCartService`** — `AdvancedReportServices.cs`
- **`ILoyaltyUtilizationService`** — `AdvancedReportServices.cs`
- **`IAdvancedReportsService`** — `AdvancedReportsService.cs`
- **`IDailyHealthReminderService`** — `CadenceServices.cs`
- **`IWeeklyOwnerDigestService`** — `CadenceServices.cs`
- **`IMonthlyGstReadinessService`** — `CadenceServices.cs`
- **`IAnnualReportBundleService`** — `CadenceServices.cs`
- **`IQuarterlyReviewBundleService`** — `CadenceServices.cs`
- **`ICashFlowProjectionService`** — §304 — Forward cash-flow projection. *(`CashFlowProjectionService.cs`)*
- **`ICatalogCompletenessService`** — `CatalogHealthLensServices.cs`
- **`IHsnComplianceSnapshotService`** — `CatalogHealthLensServices.cs`
- **`ISkuAdditionVelocityService`** — `CatalogHealthLensServices.cs`
- **`IUnsaleableItemFlagService`** — `CatalogHealthLensServices.cs`
- **`INewProductRampService`** — `CatalogHealthLensServices.cs`
- **`IProductLifecycleStageService`** — `CatalogHealthLensServices.cs`
- **`IStockEntryErrorRateService`** — `CatalogHealthLensServices.cs`
- **`IBrandRevenueDistributionService`** — `CategoryBrandLensServices.cs`
- **`IHsnRevenueDistributionService`** — `CategoryBrandLensServices.cs`
- **`ICategoryDeepDiveService`** — `CategoryBrandLensServices.cs`
- **`ITopProductComboService`** — `CategoryBrandLensServices.cs`
- **`IBasketAvgGrowthService`** — `CategoryBrandLensServices.cs`
- **`IRepeatIntervalService`** — `CategoryBrandLensServices.cs`
- **`ICumulativeProfitService`** — `CategoryBrandLensServices.cs`
- **`IBranchMixService`** — `CategoryBrandLensServices.cs`
- **`IChartDataService`** — `ChartDataService.cs`
- **`IColourPopularityRankerService`** — `ClothingLensServices.cs`
- **`IFabricPerformanceRankerService`** — `ClothingLensServices.cs`
- **`IFitPopularityRankerService`** — `ClothingLensServices.cs`
- **`ISeasonTagPerformanceService`** — `ClothingLensServices.cs`
- **`IPatternPopularityRankerService`** — `ClothingLensServices.cs`
- **`IMonthlyComparisonService`** — `ComparisonLensServices.cs`
- **`IYoyRevenueService`** — `ComparisonLensServices.cs`
- **`IQuarterlyGrowthService`** — `ComparisonLensServices.cs`
- **`IWeekdayVsWeekendSplitService`** — `ComparisonLensServices.cs`
- **`IPeakWeekdayService`** — `ComparisonLensServices.cs`
- **`IMonthlyGrowthTrajectoryService`** — `ComparisonLensServices.cs`
- **`ICumulativeRevenueService`** — `ComparisonLensServices.cs`
- **`ICrossSellPairsService`** — Basket analysis: finds product pairs that are co-purchased unusually often. *(`CrossSellPairsService.cs`)*
- **`ICohortRetentionService`** — `CustomerAnalyticsBatchServices.cs`
- **`IMarketBasketRecommendationService`** — `CustomerAnalyticsBatchServices.cs`
- **`IReEngagementListService`** — `CustomerAnalyticsBatchServices.cs`
- **`ISalesFunnelService`** — `CustomerAnalyticsBatchServices.cs`
- **`IAnniversaryCelebrationService`** — `CustomerCelebrationBatchServices.cs`
- **`ICustomerAffinityService`** — `CustomerCelebrationBatchServices.cs`
- **`IEditConflictGuardService`** — `CustomerCelebrationBatchServices.cs`
- **`ICustomerChurnRiskService`** — Flags previously-frequent customers who have stopped visiting. *(`CustomerChurnRiskService.cs`)*
- **`INewVsReturningCustomerService`** — `CustomerLensServices.cs`
- **`ICustomerPurchaseFrequencyService`** — `CustomerLensServices.cs`
- **`ILoyaltyTierComputeService`** — `CustomerLensServices.cs`
- **`ICustomerLocationInsightsService`** — `CustomerLensServices.cs`
- **`ICustomerDemographicsFillRateService`** — `CustomerLensServices.cs`
- **`ICustomerBasketDiversityService`** — `CustomerLensServices.cs`
- **`ICustomerDormancyScoreService`** — `CustomerLensServices.cs`
- **`ICustomerLifetimeValueService`** — Computes per-customer lifetime value (CLV) from finalised sales history. *(`CustomerLifetimeValueService.cs`)*
- **`IDashboardService`** — Aggregates real-time dashboard KPIs: today/month/yesterday sales, stock valuation, low/dead stock counts, profit, expenses, outstanding debtors, vendor dues, and top products. *(`DashboardService.cs`)*
- **`IDataConsistencySpotCheckService`** — §159 — Data-consistency spot-check. *(`DataConsistencySpotCheckService.cs`)*
- **`IDayEndPreflightService`** — §158 — Day-end preflight. *(`DayEndPreflightService.cs`)*
- **`IDayEndService`** — `DayEndService.cs`
- **`IExcelService`** — Handles CSV export of all major entities (products, vendors, stock, expenses, etc. *(`ExcelService.cs`)*
- **`IExpenseBudgetTrackerService`** — §303 — Expense-budget tracker. *(`ExpenseBudgetTrackerService.cs`)*
- **`IExpensePdfService`** — `ExpensePdfService.cs`
- **`IPaymentMixAnalyzerService`** — `FinanceLensServices.cs`
- **`IRefundDelayTrackerService`** — `FinanceLensServices.cs`
- **`ICashVarianceTrendService`** — `FinanceLensServices.cs`
- **`ICreditLimitBreachService`** — `FinanceLensServices.cs`
- **`IDebtorAgingService`** — `FinanceLensServices.cs`
- **`IExpenseCategoryTrendService`** — `FinanceLensServices.cs`
- **`INextDayRevenueForecastService`** — `ForecastLensServices.cs`
- **`IWeeklyCashPositionForecastService`** — `ForecastLensServices.cs`
- **`IStockDaysToZeroForecastService`** — `ForecastLensServices.cs`
- **`ISeasonalReorderPlannerService`** — `ForecastLensServices.cs`
- **`ICustomerNextVisitForecastService`** — `ForecastLensServices.cs`
- **`IForecastService`** — `ForecastService.cs`
- **`IFyClosureReportService`** — `FyClosureReportService.cs`
- **`IGstrExportService`** — `GstrExportService.cs`
- **`IDeadStockLiquidationService`** — `InventoryLensServices.cs`
- **`IStockTurnoverRatioService`** — `InventoryLensServices.cs`
- **`IStockoutFrequencyService`** — `InventoryLensServices.cs`
- **`ISkuProliferationWatchService`** — `InventoryLensServices.cs`
- **`ICarryoverRiskService`** — `InventoryLensServices.cs`
- **`IOverstockDetectorService`** — `InventoryLensServices.cs`
- **`IStockHealthScorecardService`** — `InventoryLensServices.cs`
- **`IDeadstockAgingBandsService`** — `InventoryLensServices.cs`
- **`IAverageProductMarginService`** — `MarginLensServices.cs`
- **`IGrossProfitTrendService`** — `MarginLensServices.cs`
- **`INetSalesAfterReturnsService`** — `MarginLensServices.cs`
- **`IPurchaseReturnRatioService`** — `MarginLensServices.cs`
- **`IExpenseToRevenueRatioService`** — `MarginLensServices.cs`
- **`IDailyDiscountBudgetService`** — `MarginLensServices.cs`
- **`IDiscountedVsFullPriceService`** — `MarginLensServices.cs`
- **`IMobileDashboardService`** — K1: Lightweight HTTP server for mobile dashboard. *(`MobileDashboardService.cs`)*
- **`IAvgTimePerBillService`** — `OpsLens2Services.cs`
- **`ICartAbandonmentService`** — `OpsLens2Services.cs`
- **`ITransactionsPerHourService`** — `OpsLens2Services.cs`
- **`IBillsPerCustomerAvgService`** — `OpsLens2Services.cs`
- **`IZeroRevenueDayFlagService`** — `OpsLens2Services.cs`
- **`IStockEntryAccuracyService`** — `OpsLens2Services.cs`
- **`IShrinkageEstimateService`** — `OpsLens2Services.cs`
- **`IIdleCashRegisterService`** — `OpsLensServices.cs`
- **`IAbnormalSaleSizeService`** — `OpsLensServices.cs`
- **`IStaffOvertimeWatcherService`** — `OpsLensServices.cs`
- **`IHeldBillAgeAnalyzerService`** — `OpsLensServices.cs`
- **`IBackupFreshnessMonitorService`** — `OpsLensServices.cs`
- **`IBulkPriceAdjustmentPreviewService`** — `OrchestrationServices.cs`
- **`ISeasonClearanceRecommenderService`** — `OrchestrationServices.cs`
- **`IStaffRoleAccessAuditService`** — `OrchestrationServices.cs`
- **`ICashRegisterHandoffChecklistService`** — `OrchestrationServices.cs`
- **`IEndOfDayReconciliationChecklistService`** — `OrchestrationServices.cs`
- **`IInventoryCountMissingService`** — `OrchestrationServices.cs`
- **`IPromoSuggestionService`** — `OrchestrationServices.cs`
- **`IDailyPriorityActionsService`** — `OrchestrationServices.cs`
- **`IVendorPaymentCycleService`** — `PartnerLensServices.cs`
- **`IVendorOnboardingTimeService`** — `PartnerLensServices.cs`
- **`IOrderFulfillmentRateService`** — `PartnerLensServices.cs`
- **`IReturnWindowAdherenceService`** — `PartnerLensServices.cs`
- **`IReturnsByStaffService`** — `PartnerLensServices.cs`
- **`ICashRatioService`** — `PaymentLensServices.cs`
- **`IUpiAdoptionTrendService`** — `PaymentLensServices.cs`
- **`ICreditVsCashRatioService`** — `PaymentLensServices.cs`
- **`IGstTaxCollectedService`** — `PaymentLensServices.cs`
- **`ICustomerPrefersCashService`** — `PaymentLensServices.cs`
- **`IPdfInvoiceService`** — `PdfInvoiceService.cs`
- **`IPdfRenderer`** — Portable QuestPDF document renderers returning raw bytes. *(`PdfRenderer.cs`)*
- **`IPeakHourStaffingAdvisor`** — Profile of bill volume by day-of-week × hour-of-day over a rolling window. *(`PeakHourStaffingAdvisorService.cs`)*
- **`IStaffOfTheMonthService`** — `PeopleLensServices.cs`
- **`IDiscountAuthorizationService`** — `PeopleLensServices.cs`
- **`ITopReturningCustomerService`** — `PeopleLensServices.cs`
- **`IFirstTimePurchaseCategoryService`** — `PeopleLensServices.cs`
- **`IPricePointHeatmapService`** — Shows where the store's revenue actually lives on the price ladder. *(`PricePointHeatmapService.cs`)*
- **`IQrService`** — `QrService.cs`
- **`IQuickStatsService`** — Small service powering the top quick-access bar. *(`QuickStatsService.cs`)*
- **`IReconciliationService`** — `ReconciliationService.cs`
- **`ISaleLedgerReportService`** — `ReportGeneratorServices.cs`
- **`IStockMovementReportService`** — `ReportGeneratorServices.cs`
- **`ICustomerStatementGeneratorService`** — `ReportGeneratorServices.cs`
- **`IVendorStatementGeneratorService`** — `ReportGeneratorServices.cs`
- **`IDailySummaryReportService`** — `ReportGeneratorServices.cs`
- **`IStaffPayrollReportService`** — `ReportGeneratorServices.cs`
- **`ITaxSummaryReportService`** — `ReportGeneratorServices.cs`
- **`IDebtorStatementGeneratorService`** — `ReportGeneratorServices.cs`
- **`IExpenseLedgerReportService`** — `ReportGeneratorServices.cs`
- **`IReportNarrativeService`** — `ReportNarrativeService.cs`
- **`IReportsService`** — Generates date-ranged business reports: daily/monthly sales, product performance, tax summaries (GST/HSN/bill-wise), profit/loss, expense breakdowns, debtor aging, staff commissions, size-stock mat... *(`ReportsService.cs`)*
- **`IRestockUrgencyRanker`** — Ranks products by restock urgency — how close each is to running out of stock given recent sales velocity. *(`RestockUrgencyRankerService.cs`)*
- **`ICustomerReturnFrequencyService`** — `RetailAnalyticsBatchServices.cs`
- **`INewArrivalVelocityService`** — `RetailAnalyticsBatchServices.cs`
- **`IMarkdownEffectivenessService`** — `RetailAnalyticsBatchServices.cs`
- **`ICategoryContributionService`** — `RetailAnalyticsBatchServices.cs`
- **`IRepeatPurchaseIntervalService`** — `RetailAnalyticsBatchServices.cs`
- **`IHangtagContentService`** — `RetailOperationsBatchServices.cs`
- **`ICsvImportMappingService`** — `RetailOperationsBatchServices.cs`
- **`IPerformanceScalingAdvisorService`** — `RetailOperationsBatchServices.cs`
- **`IAttachmentSalesScoreService`** — `RetailScienceBatchServices.cs`
- **`IAverageItemsPerBillTrendService`** — `RetailScienceBatchServices.cs`
- **`IMarginVolumeMapService`** — `RetailScienceBatchServices.cs`
- **`ICustomerAcquisitionCostService`** — `RetailScienceBatchServices.cs`
- **`IReturnReasonInsightsService`** — Groups sale-return records by their free-text "Reason" field to highlight patterns — size problem, defect, customer change-of-mind, wrong colour, etc. *(`ReturnReasonInsightsService.cs`)*
- **`IDailyRevenueRollupService`** — `SalesLensServices.cs`
- **`IWeekOverWeekDeltaService`** — `SalesLensServices.cs`
- **`IHourlyRevenueShapeService`** — `SalesLensServices.cs`
- **`IBasketSizeDistributionService`** — `SalesLensServices.cs`
- **`IDiscountDepthTrackerService`** — `SalesLensServices.cs`
- **`IMrpVsSoldPriceGapService`** — `SalesLensServices.cs`
- **`ISalesByAttributeService`** — `SalesLensServices.cs`
- **`IProductReturnRateService`** — `SalesLensServices.cs`
- **`IQuickRatioService`** — `SalesLensServices.cs`
- **`ISeasonalTrendService`** — Computes month-of-year revenue seasonality across the last 24 months (or fewer if history is shorter). *(`SeasonalTrendService.cs`)*
- **`ISizeMixIntelligenceService`** — §233 — Retail-clothing specific analysis. *(`SizeMixIntelligenceService.cs`)*
- **`ISmartInsightsService`** — `SmartInsightsService.cs`
- **`ISmartQueryService`** — K3: Smart retail chatbot — understands natural language questions about sales, stock, customers. *(`SmartQueryService.cs`)*
- **`IStaffPerformanceRanker`** — Per-staff sales performance summary over a rolling window. *(`StaffPerformanceRankerService.cs`)*
- **`IStockAgingService`** — `StockAgingService.cs`
- **`IStockValuationService`** — `StockValuationService.cs`
- **`IAssortmentOptimizationService`** — `StrategicAdvisoryBatchServices.cs`
- **`IDemandSensingService`** — `StrategicAdvisoryBatchServices.cs`
- **`IDeviceTrustService`** — `StrategicAdvisoryBatchServices.cs`
- **`IBetaProgramService`** — `StrategicAdvisoryBatchServices.cs`
- **`ISystemHealthCardService`** — §168 — One-glance "All systems operational" card for the Dashboard. *(`SystemHealthCardService.cs`)*
- **`ITallyExportService`** — `TallyExportService.cs`
- **`ITimeToProfitEmitter`** — §485 — Time-to-profit (TTP) emitter. *(`TimeToProfitEmitterService.cs`)*
- **`IVendorDependenceService`** — `VendorLensServices.cs`
- **`IPurchaseOrderAgingService`** — `VendorLensServices.cs`
- **`IInwardCycleTimeService`** — `VendorLensServices.cs`
- **`IVendorMixDiversityService`** — `VendorLensServices.cs`
- **`IVendorProductOverlapService`** — `VendorLensServices.cs`
- **`IVendorPriceVolatilityService`** — Surfaces vendors whose cost prices for the same product drift upward between receipts. *(`VendorPriceVolatilityService.cs`)*

## Resilience

- **`ICircuitBreakerService`** — `ResilienceServices.cs`
- **`IRateLimiterService`** — `ResilienceServices.cs`
- **`IRetryQueueService`** — `ResilienceServices.cs`
- **`ITimeoutEnvelopeService`** — `ResilienceServices.cs`
- **`IGracefulShutdownService`** — `ResilienceServices.cs`

## Returns

- **`IReturnAcceptanceService`** — `ReturnExchangeServices.cs`
- **`IExchangeCreditAppliedService`** — `ReturnExchangeServices.cs`
- **`IPartialReturnCalculatorService`** — `ReturnExchangeServices.cs`
- **`IReturnReceiptBuilderService`** — `ReturnExchangeServices.cs`
- **`IReturnPolicyBadgeService`** — `ReturnExchangeServices.cs`

## Scheduler

- **`INextOccurrenceResolverService`** — `SchedulerHelperServices.cs`
- **`IRecurringReminderListService`** — `SchedulerHelperServices.cs`
- **`IOneShotScheduleService`** — `SchedulerHelperServices.cs`
- **`IDelayedJobRetryPolicyService`** — `SchedulerHelperServices.cs`
- **`IJobDeadlineCalculatorService`** — `SchedulerHelperServices.cs`

## SearchTyping

- **`IFuzzyMatchRankerService`** — `SearchTypingServices.cs`
- **`ISuggestionCollapserService`** — `SearchTypingServices.cs`
- **`IAcronymExpanderService`** — `SearchTypingServices.cs`
- **`IDictionaryCorrectorService`** — `SearchTypingServices.cs`
- **`ISpellSuggestionService`** — `SearchTypingServices.cs`

## Secrets

- **`IApiTokenGeneratorService`** — `SecretManagementServices.cs`
- **`IApiTokenRotatorService`** — `SecretManagementServices.cs`
- **`ISecretRedactorService`** — `SecretManagementServices.cs`
- **`IHashedCheckService`** — `SecretManagementServices.cs`
- **`ISecretStrengthScorerService`** — `SecretManagementServices.cs`

## Security

- **`IFailedLoginCounterService`** — `SecurityAuditServices.cs`
- **`IPermissionDenialLoggerService`** — `SecurityAuditServices.cs`
- **`ISuspiciousDiscountDetectorService`** — `SecurityAuditServices.cs`
- **`IUnusualTransactionTimingService`** — `SecurityAuditServices.cs`
- **`IDataExportAuditService`** — `SecurityAuditServices.cs`
- **`IAnonymousSaleDetectorService`** — `SecurityAuditServices.cs`

## ShoppingFlow

- **`IUpsellSuggestionService`** — `ShoppingFlowServices.cs`
- **`ICrossSellSuggestionService`** — `ShoppingFlowServices.cs`
- **`IPriceComparisonBadgeService`** — `ShoppingFlowServices.cs`
- **`IGiftWrapSuggestionService`** — `ShoppingFlowServices.cs`
- **`IFavoriteItemsService`** — `ShoppingFlowServices.cs`

## ShoppingSession

- **`ICartResumeDetectorService`** — `ShoppingSessionServices.cs`
- **`IFrequentItemsPaletteService`** — `ShoppingSessionServices.cs`
- **`IRecentItemsPaletteService`** — `ShoppingSessionServices.cs`
- **`IQuickAddSuggestionService`** — `ShoppingSessionServices.cs`
- **`IScanAssemblyGuardService`** — `ShoppingSessionServices.cs`

## StockMisc

- **`IStockStockoutHistoryService`** — `StockMiscServices.cs`
- **`IStockReservedBlockService`** — `StockMiscServices.cs`
- **`IStockLocationLabelService`** — `StockMiscServices.cs`
- **`IStockShelfMapService`** — `StockMiscServices.cs`
- **`IStockConsolidationPlannerService`** — `StockMiscServices.cs`

## TaxBreakdown

- **`IRateSlabResolverService`** — `TaxBreakdownServices.cs`
- **`IHsnToRateResolverService`** — `TaxBreakdownServices.cs`
- **`ICessRateResolverService`** — `TaxBreakdownServices.cs`
- **`IEligibleItcCalculatorService`** — `TaxBreakdownServices.cs`
- **`ILineLevelTaxBreakdownService`** — `TaxBreakdownServices.cs`

## TextProcessing

- **`IStopwordRemoverService`** — `TextProcessingServices.cs`
- **`ITokenizerService`** — `TextProcessingServices.cs`
- **`ITextSnippetExtractorService`** — `TextProcessingServices.cs`
- **`ISemanticKeywordService`** — `TextProcessingServices.cs`
- **`IQueryExpansionService`** — `TextProcessingServices.cs`
- **`ITextNormalizationService`** — `TextProcessingServices.cs`

## UxHints

- **`ICssUpgradeCatalogService`** — `CssUpgradeCatalogService.cs`
- **`IFabricGarmentCatalogService`** — `DomainCatalogBatchServices.cs`
- **`ILocalizationBundleService`** — `DomainCatalogBatchServices.cs`
- **`IWhistleblowerIntakeService`** — `DomainCatalogBatchServices.cs`
- **`IDataFreshnessService`** — `FreshnessAndFeedbackBatchServices.cs`
- **`ISoundSignatureService`** — `FreshnessAndFeedbackBatchServices.cs`
- **`IDensityProfileService`** — `FreshnessAndFeedbackBatchServices.cs`
- **`IValidationFeedbackService`** — `FreshnessAndFeedbackBatchServices.cs`
- **`ITrainingReplayService`** — `HostModeBatchServices.cs`
- **`IKioskModeService`** — `HostModeBatchServices.cs`
- **`IEnergySaverModeService`** — `HostModeBatchServices.cs`
- **`IRollbackSnapshotService`** — `HostModeBatchServices.cs`
- **`IProductThesisContentService`** — `MetaAndBlockedRegistryServices.cs`
- **`IBlockedItemRegistry`** — `MetaAndBlockedRegistryServices.cs`
- **`IRowInlineEditService`** — `RemainingTodosBatchServices.cs`
- **`IWhatIfScenarioService`** — `RemainingTodosBatchServices.cs`
- **`IReportDefinitionRegistry`** — `RemainingTodosBatchServices.cs`
- **`IPluginRegistryService`** — `RemainingTodosBatchServices.cs`
- **`ICollectionGroupingService`** — `RemainingTodosBatchServices.cs`
- **`IDeveloperCertificationRegistry`** — `RemainingTodosBatchServices.cs`
- **`ICommunityForumLinkService`** — `RemainingTodosBatchServices.cs`
- **`IFarewellTaglineService`** — `RemainingTodosBatchServices.cs`
- **`IRegionalCustomizationService`** — `RemainingTodosBatchServices.cs`
- **`ICarbonOffsetTrackerService`** — `RemainingTodosBatchServices.cs`
- **`ISustainabilityStoryService`** — `RemainingTodosBatchServices.cs`
- **`IIndustryCertificationRegistry`** — `RemainingTodosBatchServices.cs`
- **`IIndexableEntityCatalog`** — `RemainingTodosBatchServices.cs`
- **`IPdfReportThemeService`** — `RemainingTodosBatchServices.cs`
- **`IRecyclingDropoffService`** — `RemainingTodosBatchServices.cs`
- **`ISupplyChainTransparencyService`** — `RemainingTodosBatchServices.cs`
- **`IPackagingMinimisationService`** — `RemainingTodosBatchServices.cs`
- **`IReusablePackagingCatalog`** — `RemainingTodosBatchServices.cs`
- **`IRetailVerticalCatalogService`** — `RetailVerticalCatalogService.cs`
- **`IUserSkillLevelService`** — `UxBehaviourBatchServices.cs`
- **`IOccasionRecommendationService`** — `UxBehaviourBatchServices.cs`
- **`ISurpriseAndDelightService`** — `UxBehaviourBatchServices.cs`
- **`IRegulatoryUpdateRegistry`** — `UxBehaviourBatchServices.cs`
- **`IUndoStackEstimatorService`** — `UxHintServices.cs`
- **`IAutosaveDirtyFlagService`** — `UxHintServices.cs`
- **`INavigationHintService`** — `UxHintServices.cs`
- **`IFocusRestorationHintService`** — `UxHintServices.cs`
- **`IKeyboardShortcutConflictDetectorService`** — `UxHintServices.cs`
- **`IPersistentFilterStateService`** — `UxUtilityBatchServices.cs`
- **`INotificationPriorityService`** — `UxUtilityBatchServices.cs`
- **`ITimeOfDayGreetingService`** — `UxUtilityBatchServices.cs`
- **`IPagingService`** — `WorkflowHelperBatchServices.cs`
- **`IPredictiveCacheService`** — `WorkflowHelperBatchServices.cs`
- **`IActionReconcilerService`** — `WorkflowHelperBatchServices.cs`
- **`IOptimisticUpdateTracker`** — `WorkflowStateBatchServices.cs`
- **`IForensicModeService`** — `WorkflowStateBatchServices.cs`
- **`IWizardStateService`** — `WorkflowStateBatchServices.cs`
- **`IEasterEggRegistry`** — `WorkflowStateBatchServices.cs`

## Validation

- **`ISaleLineValidator`** — `ValidationServices.cs`
- **`IInvoiceNumberUniquenessChecker`** — `ValidationServices.cs`
- **`IStockItemSaleFeasibility`** — `ValidationServices.cs`
- **`IDebtorCreditLimitChecker`** — `ValidationServices.cs`
- **`IPaymentOverpayChecker`** — `ValidationServices.cs`
- **`IReturnEligibilityChecker`** — `ValidationServices.cs`
- **`IProductDuplicateChecker`** — `ValidationServices.cs`
- **`IVendorDuplicateChecker`** — `ValidationServices.cs`

## VendorExtras

- **`IVendorRatingService`** — `VendorExtraServices.cs`
- **`IVendorTurnaroundTrackerService`** — `VendorExtraServices.cs`
- **`IVendorContractExpiryService`** — `VendorExtraServices.cs`
- **`IVendorQualityScoreService`** — `VendorExtraServices.cs`
- **`IVendorPreferredPartnerService`** — `VendorExtraServices.cs`

## Workflows

- **`ISaleLifecycleStateMachineService`** — `WorkflowStateMachines.cs`
- **`IReturnWorkflowStateMachineService`** — `WorkflowStateMachines.cs`
- **`IPurchaseOrderLifecycleStateMachineService`** — `WorkflowStateMachines.cs`
- **`IAlterationLifecycleStateMachineService`** — `WorkflowStateMachines.cs`
- **`ICustomerOnboardingWorkflowService`** — `WorkflowStateMachines.cs`

---

## Appendix A — Domain models (POCOs)

Source: `src/StoreAssistantPro.Core/Models/`. These are the persisted entities — most have a matching `DbSet<T>` on `AppDbContext`.

- **`AdHocNotification`** — `AdHocNotification.cs`
- **`AlterationEntry`** — `AlterationEntry.cs`
- **`AppConfig`** — `AppConfig.cs`
- **`AttributeValue`** — Predefined values for product attributes.
- **`AuditLog`** — `AuditLog.cs`
- **`AuditQuery`** — `AuditQuery.cs`
- **`AuditRetentionPolicy`** — Audit A5 / DESIGN-AUDIT-LOG-REDESIGN.
- **`BarcodeLabelConfig`** — `BarcodeLabelConfig.cs`
- **`Branch`** — `Branch.cs`
- **`CashMovement`** — `CashMovement.cs`
- **`Category`** — `Category.cs`
- **`CloudBackupEndpoint`** — Audit B4 — cross-region backup replication (DESIGN-CROSS-REGION-REPLICATION.
- **`CloudBackupHealthStatus`** — `CloudBackupHealthStatus.cs`
- **`CloudUploadJournal`** — Audit B4 #19 / DESIGN-CROSS-REGION-REPLICATION.
- **`CloudUploadSession`** — Persistent state for a resumable cloud-upload session.
- **`Colour`** — `Colour.cs`
- **`Customer`** — `Customer.cs`
- **`DayReconciliationSession`** — `DayReconciliation.cs`
- **`DiscountRule`** — `DiscountRule.cs`
- **`ExpenseCategory`** — `Expense.cs`
- **`ExpenseRevision`** — `ExpenseRevision.cs`
- **`ExtraCharge`** — `ExtraCharge.cs`
- **`FinancialYearEntity`** — `FinancialYearEntity.cs`
- **`FirmDetails`** — `FirmDetails.cs`
- **`FyClosure`** — One row per closed financial year.
- **`FyRolloverIntent`** — `FyRolloverIntent.cs`
- **`GoodsReceivedNote`** — `GoodsReceivedNote.cs`
- **`HsnCode`** — `HsnCode.cs`
- **`InventoryMovement`** — `InventoryMovement.cs`
- **`InwardEntry`** — An inward receipt — parcels received via transportation.
- **`IroningEntry`** — `IroningEntry.cs`
- **`IroningLineItem`** — `IroningLineItem.cs`
- **`JobRun`** — Idempotency ledger for scheduled / startup / recurring jobs.
- **`JournalEntry`** — `JournalEntry.cs`
- **`MobileToken`** — Per-device mobile-dashboard bearer token.
- **`NotificationState`** — `NotificationState.cs`
- **`PaymentMethod`** — `PaymentMethod.cs`
- **`PermissionEntry`** — `PermissionEntry.cs`
- **`PettyCashDeposit`** — `PettyCashDeposit.cs`
- **`PriceHistory`** — `PriceHistory.cs`
- **`Product`** — `Product.cs`
- **`ProductSize`** — `ProductSize.cs`
- **`ProductVariant`** — `ProductVariant.cs`
- **`PurchaseInvoice`** — `PurchaseInvoice.cs`
- **`PurchaseReturn`** — `PurchaseReturn.cs`
- **`Quotation`** — `Quotation.cs`
- **`ReceiptConfig`** — Receipt customization settings (F9).
- **`RecurringExpense`** — `RecurringExpense.cs`
- **`Sale`** — `Sale.cs`
- **`Staff`** — `Staff.cs`
- **`StockAdjustment`** — `StockAdjustment.cs`
- **`StockAlert`** — `StockAlert.cs`
- **`StockEntry`** — Stock added from an inward parcel for a specific product.
- **`StockTake`** — `StockTake.cs`
- **`TaskItem`** — `TaskItem.cs`
- **`TaxConfig`** — `TaxConfig.cs`
- **`TaxRate`** — A named tax entry with rate and optional price slab.
- **`Vendor`** — `Vendor.cs`
- **`VendorLedgerEntry`** — `VendorLedgerEntry.cs`
- **`VendorPayment`** — Payment history log for a vendor.

## Appendix B — Helper utilities

Source: `src/StoreAssistantPro.Services/Helpers/`. Stateless utilities used across services.

- **`BarcodeParser`** — `src/StoreAssistantPro.Services/Helpers/BarcodeParser.cs`
- **`CrashCounter`** — ROADMAP #49 — consecutive-crash detector wired into the self-update rollback path.
- **`CsvImportHelper`** — `src/StoreAssistantPro.Services/Helpers/CsvImportHelper.cs`
- **`CsvUtility`** — `src/StoreAssistantPro.Services/Helpers/CsvUtility.cs`
- **`Debouncer`** — Coalesces a stream of frequent calls into a single delayed invocation.
- **`ExpensePreferences`** — Per-user, per-machine prefs for the Expenses screen — stored as JSON in %AppData%\StoreAssistantPro\expense_prefs.
- **`ExpressionEvaluator`** — Tiny recursive-descent calculator for amount fields: supports + - * / and parentheses on decimal literals.
- **`LogRedaction`** — `src/StoreAssistantPro.Services/Helpers/LogRedaction.cs`
- **`LogService`** — Structured logging via Serilog.
- **`OnboardingService`** — Manages first-time user onboarding tour (F7).
- **`PaginatedList`** — Reusable pagination + sorting helper for list pages.
- **`BarcodeGenerator`** — Generates auto-incrementing barcode numbers with zero-padding.
- **`QueryProfiler`** — `src/StoreAssistantPro.Services/Helpers/QueryProfiler.cs`
- **`FinancialYear`** — Indian financial year helper (April 1 – March 31).
- **`IndianPincodes`** — Maps Indian pincode first 2 digits to state.
- **`IndianState`** — `src/StoreAssistantPro.Services/Helpers/Reference/IndianStates.cs`
- **`Units`** — `src/StoreAssistantPro.Services/Helpers/Reference/Units.cs`
- **`SafeShellOpen`** — Guarded wrapper around for "open this file with the OS default handler" flows.
- **`SafeTimer`** — A defensive wrapper around for ViewModel / background callers that must never crash the app on an unobserved callback exception.
- **`SamvatCalendar`** — ROADMAP #480 — Vikram Samvat ↔ Gregorian dual-calendar helper.
- **`SecretProtector`** — `src/StoreAssistantPro.Services/Helpers/SecretProtector.cs`
- **`SecurityHelper`** — `src/StoreAssistantPro.Services/Helpers/SecurityHelper.cs`
- **`ServiceProviderExtensions`** — Extension methods on that eliminate the three-line "create scope / resolve AppDbContext / use" boilerplate that appears in every service method.
- **`SlowQueryTelemetry`** — Lightweight in-memory telemetry for queries that exceed a threshold (#71).
- **`SqlLikeEscape`** — Escape helpers for SQLite LIKE patterns fed from user input.
- **`SsrfGuard`** — Centralized SSRF guard for outbound HTTPS requests (cloud backup upload, custom webhook integrations, etc.
- **`UndoService`** — Simple undo stack for reversible operations (F6).
- **`UpdateChecker`** — `src/StoreAssistantPro.Services/Helpers/UpdateChecker.cs`
- **`UpdateInstaller`** — Audit H200 — rollback mechanism for the self-update flow.
- **`UpdatePackageVerifier`** — `src/StoreAssistantPro.Services/Helpers/UpdatePackageVerifier.cs`
- **`UpdatePreferences`** — ROADMAP #76 — persistent update-check preferences.
- **`InputSanitizer`** — Sanitizes user input — trims, normalizes whitespace, strips invisible Unicode characters, and enforces max length.

## Appendix C — Data layer key points

- `src/StoreAssistantPro.Services/Data/AppDbContext.cs` — single DbContext, ~60 DbSets, SQLite provider. All `DateTime` properties go through a value converter that writes as UTC and reads back with `DateTimeKind.Utc` (set once centrally; see lines 133-152).
- `src/StoreAssistantPro.Services/Data/MigrationHelper.cs` — versioned schema migrations (NOT EF migrations). `CurrentSchemaVersion` constant at top (currently v69); each `ApplyVersion(db, ref version, N, applyAction, () => validator, "vN description")` block is idempotent and records the new version only after a validator confirms the shape. Table rebuilds use `RebuildTableWithTextDecimalColumns` (DROP → CREATE → INSERT → DROP → RENAME in one transaction).
- Per-station data lives at `%LOCALAPPDATA%\StoreAssistantPro\Station_<n>\` — the SQLite DB, an `audit.key` per station, `AutoBackups/`, rolling logs.

## Appendix D — Key architectural features worth porting carefully

- **Chained-hash audit log** (`Services/Reporting/AuditService.cs` + `Models/AuditLog.cs`) — append-only ledger where each row's hash chains the previous. `audit.key` per station is required to verify and extend the chain. CSV exports neutralise spreadsheet formula injection before RFC-4180 escaping.
- **Master-PIN privilege tickets** (`Auth/` folder + `MasterConfirmationStore`) — destructive actions (delete, bulk edit, financial-year close, etc.) require the user to enter the Master PIN, which issues a single-use ticket. `IAuthorizationService.EnsureAuthorized(...)` consumes the ticket. Tickets are audit-anchored.
- **Held bills + resume** (`Cart/` and `ShoppingSession/`) — in-progress carts can be suspended with a label, recalled later; recall rehydrates the cart including selected customer, staff, discounts.
- **Split payments + quick-cash** (`PaymentRails/` + `Cart/`) — a single sale can settle across Cash / Card / UPI / Credit / Other rows that must sum to GrandTotal; the credit portion becomes a `Debtor` row.
- **Returns + exchanges** (`Returns/` + `ShoppingFlow/`) — per-line return qty (capped by `OriginalQty - AlreadyReturned`), refund is the actual paid price per unit (LineTotal / Qty), not MRP. Exchange swaps returned lines for new cart lines atomically.
- **GST + cess + tax-inclusive pricing** (`TaxBreakdown/`) — supports tax-inclusive MRP (reverse-calculates base), HSN codes, cess rates, price-slab tax rates (different rate per price band chained by Min/Max), CGST/SGST split.
- **Day reconciliation** (`Reconciliation/`) — a per-business-date immutable snapshot of declared-vs-expected cash/card/UPI/other/credit, tied to a cash register session. Once finalised, cannot be re-run for that date.
- **Schema-versioned backup + restore** (`BackupHelpers/` + `Services/Backup/`) — backups carry the DB schema version in the file; restore refuses to load a newer schema into an older binary. Auto-backup ring keeps last N on disk.
- **FY rollover** (`Workflows/FyRolloverService`) — end-of-year close: seals all data, writes a closure hash, carries forward opening balances (cash, debtor, vendor outstanding, stock valuation) into the new FY.
- **Cross-region replication** (`Branch/` + `Services/CloudReplication/`) — slice-based: schema v64 added the entity tables, journaled uploads + per-attempt history, resumable.
- **Multi-station (peer-to-peer, not cloud)** — each station has its own SQLite file + audit chain; no central server. Station ID is a CLI arg (`--station=2`).

## Appendix E — Things this repo moved AWAY from (do not port)

History lessons — the current state of this repo *removed* these on purpose. If you port them, you're reintroducing work the owner explicitly undid.

- All AI / LLM / HealBot / DevLoop / MCP integrations — removed 2026-04-22. App is deterministic-only.
- All OAuth-gated integrations (Gmail, Outlook, Sheets, SharePoint, Google My Business) — declined 2026-04-21.
- Enterprise items (SSO / LDAP / SCIM / SIEM / BYOK / ISO / PCI / HIPAA) — declined 2026-04-21.
- Custom CSS / custom JS / custom MudBlazor theme — removed; UI is 100% stock MudBlazor.
- `TabScopedComponent` per-tab DI scope — removed in commit `e72ca520` (security regression noted: master-PIN tickets now scope to the circuit, not the tab).
