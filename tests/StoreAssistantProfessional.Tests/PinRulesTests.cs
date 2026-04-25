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
    public void IsStrong_FalseForWeakPins(string pin)
    {
        Assert.False(PinRules.IsStrong(pin));
    }
}
