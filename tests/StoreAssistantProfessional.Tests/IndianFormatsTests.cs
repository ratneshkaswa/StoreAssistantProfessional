using StoreAssistantProfessional.Validation;
using Xunit;

namespace StoreAssistantProfessional.Tests;

public class IndianFormatsTests
{
    [Theory]
    [InlineData("27ABCDE1234F1Z5", true)]   // valid shape
    [InlineData("27abcde1234f1z5", true)]   // case-insensitive
    [InlineData("", true)]                    // blank treated as "not provided"
    [InlineData(null, true)]
    [InlineData("27ABCDE1234F1Z", false)]   // 14 chars
    [InlineData("AB12345678901Z5", false)]  // letters in state-code position
    [InlineData("00ABCDE1234F1Z5", true)]   // shape ok even if state code is bogus (StateCodeFromGstin handles that)
    public void IsGstin_ChecksShapeOnly(string? input, bool expected) =>
        Assert.Equal(expected, IndianFormats.IsGstin(input));

    [Theory]
    [InlineData("ABCDE1234F", true)]
    [InlineData("abcde1234f", true)]
    [InlineData("", true)]
    [InlineData("ABCDE12345", false)]      // last char must be letter
    [InlineData("ABCD1234F", false)]       // 9 chars
    public void IsPan_ChecksShape(string? input, bool expected) =>
        Assert.Equal(expected, IndianFormats.IsPan(input));

    [Theory]
    [InlineData("HDFC0001234", true)]
    [InlineData("hdfc0001234", true)]
    [InlineData("", true)]
    [InlineData("HDFC1001234", false)]    // 5th char must be 0
    [InlineData("HDFC00012", false)]      // too short
    public void IsIfsc_ChecksShape(string? input, bool expected) =>
        Assert.Equal(expected, IndianFormats.IsIfsc(input));

    [Theory]
    [InlineData("400001", true)]
    [InlineData("110001", true)]
    [InlineData("", true)]
    [InlineData("000001", false)]    // can't start with 0
    [InlineData("12345", false)]     // 5 digits
    public void IsPincode_ChecksShape(string? input, bool expected) =>
        Assert.Equal(expected, IndianFormats.IsPincode(input));

    [Theory]
    [InlineData("9876543210", true)]
    [InlineData("+91 9876543210", true)]
    [InlineData("+91-9876-543-210", true)]
    [InlineData("(98) 765 43210", true)]   // parens and spaces
    [InlineData("98.7654.3210", true)]      // dots
    [InlineData("098765 43210", true)]     // leading 0 trunk
    [InlineData("", true)]
    [InlineData("12345", false)]            // too short
    [InlineData("5876543210", false)]       // doesn't start with 6-9
    public void IsIndianPhone_ToleratesFormatting(string? input, bool expected) =>
        Assert.Equal(expected, IndianFormats.IsIndianPhone(input));

    [Theory]
    [InlineData("9876543210", "9876543210")]
    [InlineData("+91 9876543210", "9876543210")]
    [InlineData("(98) 7654-3210", "9876543210")]     // strip parens, spaces, dashes
    [InlineData("987.654.3210", "9876543210")]       // strip dots
    [InlineData("098765 43210", "9876543210")]      // trunk-0 prefix
    [InlineData("", "")]
    public void StripPhone_CanonicalizesIndianMobile(string input, string expected) =>
        Assert.Equal(expected, IndianFormats.StripPhone(input));

    [Theory]
    [InlineData("user@example.com", true)]
    [InlineData("a@b.c", true)]
    [InlineData("", true)]
    [InlineData("user@", false)]
    [InlineData("user@@example.com", false)]
    public void IsEmail_AcceptsBasicShape(string? input, bool expected) =>
        Assert.Equal(expected, IndianFormats.IsEmail(input));

    [Theory]
    [InlineData("INV", true)]
    [InlineData("INV01", true)]
    [InlineData("inv", true)]    // case-normalized internally
    [InlineData("INV-01", false)] // no dashes
    [InlineData("INV 01", false)] // no spaces
    [InlineData("", true)]
    public void IsInvoicePrefix_AlphanumericOnly(string? input, bool expected) =>
        Assert.Equal(expected, IndianFormats.IsInvoicePrefix(input));

    [Theory]
    [InlineData("12", true)]
    [InlineData("12345678", true)]
    [InlineData("", true)]
    [InlineData("1", false)]           // too short
    [InlineData("123456789", false)]   // too long
    [InlineData("12AB", false)]        // non-digit
    public void IsHsn_TwoToEightDigits(string? input, bool expected) =>
        Assert.Equal(expected, IndianFormats.IsHsn(input));

    [Theory]
    [InlineData("27ABCDE1234F1Z5", "ABCDE1234F")]
    [InlineData("", null)]
    [InlineData("invalid", null)]
    public void PanFromGstin_ExtractsPositions3To12(string? input, string? expected) =>
        Assert.Equal(expected, IndianFormats.PanFromGstin(input));

    [Theory]
    [InlineData("27", "Maharashtra")]
    [InlineData("07", "Delhi")]
    [InlineData("99", null)]   // out of range
    [InlineData("XX", null)]
    public void StateNameFromCode_KnowsAllRegisteredStates(string? code, string? expected) =>
        Assert.Equal(expected, IndianFormats.StateNameFromCode(code));

    [Theory]
    [InlineData("27ABCDE1234F1Z5", "27")]   // valid range
    [InlineData("99ABCDE1234F1Z5", null)]   // out of range — should NOT echo the bad code
    [InlineData("ABABCDE1234F1Z5", null)]   // non-digit prefix
    [InlineData("", null)]
    public void StateCodeFromGstin_RejectsUnknownCodes(string? input, string? expected) =>
        Assert.Equal(expected, IndianFormats.StateCodeFromGstin(input));

    [Theory]
    [InlineData("", true)]
    [InlineData("invalid", true)]   // shape-invalid: defer to IsGstin
    public void IsGstinChecksumValid_DefersToShapeForInvalidInput(string? input, bool expected) =>
        Assert.Equal(expected, IndianFormats.IsGstinChecksumValid(input));
}
