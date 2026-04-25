using StoreAssistantProfessional.Services;
using Xunit;

namespace StoreAssistantProfessional.Tests;

public class PinRulesTests
{
    [Theory]
    [InlineData("0000")]
    [InlineData("9999")]
    [InlineData("1234")]
    [InlineData("4321")]
    [InlineData("2580")]
    [InlineData("0852")]
    [InlineData("1379")]
    [InlineData("8901")]   // arithmetic with wraparound
    [InlineData("9012")]   // arithmetic with wraparound
    [InlineData("1357")]   // step 2
    [InlineData("2468")]   // step 2
    [InlineData("1212")]   // 2-block repeat
    [InlineData("1221")]   // palindrome
    [InlineData("1235")]   // last digit perturbed from 1234
    [InlineData("8769")]   // last digit perturbed from 8765
    [InlineData("9870")]   // last digit perturbed from 9876
    [InlineData("2469")]   // last digit perturbed from 2468
    [InlineData("1359")]   // last digit perturbed from 1357
    public void IsWeak_ShouldFlag_FourDigitWeakPins(string pin)
    {
        Assert.True(PinRules.IsWeak(pin), $"Expected {pin} to be weak.");
    }

    [Theory]
    [InlineData("0007")]   // 3-of-one, single odd digit
    [InlineData("1110")]
    [InlineData("9888")]
    [InlineData("2882")]   // already palindrome but also low-uniqueness
    [InlineData("0001")]
    [InlineData("0009")]
    [InlineData("1100")]
    [InlineData("1133")]
    public void IsWeak_ShouldFlag_LowUniquenessFourDigitPins(string pin)
    {
        Assert.True(PinRules.IsWeak(pin), $"Expected {pin} to be weak (low uniqueness).");
    }

    [Theory]
    [InlineData("1990")]
    [InlineData("2003")]
    [InlineData("1985")]
    [InlineData("2024")]
    [InlineData("1900")]
    public void IsWeak_ShouldFlag_PlausibleYearPins(string pin)
    {
        Assert.True(PinRules.IsWeak(pin), $"Expected year-shaped {pin} to be weak.");
    }

    [Fact]
    public void IsWeak_DoesNotFlag_FourDigitsBelowYearWindow()
    {
        // 1899 sits just below the [1900, currentYear+5] window. It's also
        // distinct enough to escape the low-uniqueness and near-sequence rules,
        // so it stays as a stable "not weak" reference across calendar years.
        Assert.False(PinRules.IsWeak("1899"));
    }

    [Theory]
    [InlineData("000000")]
    [InlineData("123456")]
    [InlineData("121212")]
    [InlineData("123123")]
    [InlineData("987987")]
    [InlineData("112233")]
    [InlineData("445566")]
    [InlineData("147258")]
    [InlineData("258369")]
    [InlineData("369258")]
    [InlineData("987654")]
    public void IsWeak_ShouldFlag_SixDigitWeakPins(string pin)
    {
        Assert.True(PinRules.IsWeak(pin), $"Expected {pin} to be weak.");
    }

    [Theory]
    [InlineData("010290")]   // 1 Feb 1990 DDMMYY
    [InlineData("311299")]   // 31 Dec 1999 DDMMYY
    [InlineData("150892")]   // 15 Aug 1992 DDMMYY (Indian Independence)
    [InlineData("010100")]   // 1 Jan 2000 DDMMYY
    [InlineData("122000")]   // 12/20/00 MMDDYY (Dec 20, 2000)
    public void IsWeak_ShouldFlag_PlausibleDateSixDigitPins(string pin)
    {
        Assert.True(PinRules.IsWeak(pin), $"Expected date-shaped {pin} to be weak.");
    }

    [Theory]
    [InlineData("4938")]
    [InlineData("8294")]
    [InlineData("5821")]
    [InlineData("7426")]
    [InlineData("9182")]
    public void IsWeak_ShouldNotFlag_RandomFourDigitPins(string pin)
    {
        Assert.False(PinRules.IsWeak(pin), $"Expected {pin} to not be weak.");
    }

    [Theory]
    [InlineData("493827")]
    [InlineData("829471")]
    [InlineData("371956")]
    [InlineData("582143")]
    [InlineData("742681")]
    public void IsWeak_ShouldNotFlag_RandomSixDigitPins(string pin)
    {
        Assert.False(PinRules.IsWeak(pin), $"Expected {pin} to not be weak.");
    }

    [Theory]
    [InlineData("")]
    [InlineData("12")]
    [InlineData("abcd")]
    [InlineData("12a4")]
    public void IsWeak_ReturnsFalse_ForInvalidInput(string pin)
    {
        Assert.False(PinRules.IsWeak(pin));
    }

    [Theory]
    [InlineData("4938")]
    [InlineData("582143")]
    public void IsStrong_TrueForRandomDistinctPins(string pin)
    {
        Assert.True(PinRules.IsStrong(pin));
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("0000")]
    [InlineData("1212")]
    [InlineData("123456")]
    [InlineData("000000")]
    [InlineData("1990")]   // year — newly weak
    [InlineData("0007")]   // low-uniqueness — newly weak
    public void IsStrong_FalseForWeakPins(string pin)
    {
        Assert.False(PinRules.IsStrong(pin));
    }

    [Theory]
    [InlineData("0123", true)]
    [InlineData("9876", true)]
    [InlineData("", true)]
    [InlineData("abc", false)]
    [InlineData("12 4", false)]
    [InlineData("१२३४", false)]   // Devanagari १२३४
    [InlineData("１２３４", false)]   // Fullwidth １２３４
    public void IsAsciiDigits_OnlyAcceptsAscii09(string input, bool expected)
    {
        Assert.Equal(expected, PinRules.IsAsciiDigits(input));
    }
}
