namespace StoreAssistantProfessional.Services;

public static class PinRules
{
    public static bool IsStrong(string pin)
    {
        if (pin.Length < 4 || !pin.All(char.IsDigit) || IsWeak(pin)) return false;
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
        if (pin.Length < 2 || !pin.All(char.IsDigit)) return false;

        if (pin.All(c => c == pin[0])) return true;

        if (IsPalindrome(pin)) return true;

        var firstStep = pin[1] - pin[0];
        if (firstStep is >= -5 and <= 5 && firstStep != 0)
        {
            var uniform = true;
            for (var i = 2; i < pin.Length; i++)
                if (pin[i] - pin[i - 1] != firstStep) { uniform = false; break; }
            if (uniform) return true;
        }

        if (pin.Length % 2 == 0)
        {
            var block = pin[..2];
            var repeats = true;
            for (var i = 2; i < pin.Length; i += 2)
            {
                if (pin.AsSpan(i, 2).SequenceEqual(block)) continue;
                repeats = false;
                break;
            }
            if (repeats) return true;

            if (IsSteppedPairs(pin)) return true;
        }

        if (pin.Length == 6 && pin[..3] == pin[3..]) return true;

        if (pin.Length == 6 && AllSameInHalves(pin)) return true;

        return false;
    }

    private static bool IsPalindrome(string pin)
    {
        for (int i = 0, j = pin.Length - 1; i < j; i++, j--)
            if (pin[i] != pin[j]) return false;
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
}
