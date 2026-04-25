namespace StoreAssistantProfessional.Services;

public static class PinRules
{
    // Common dialpad columns/diagonals and "spirals" attackers try first. We keep this
    // list short — broader patterns (sequences, palindromes, repeats) are caught
    // structurally below; anything we have to enumerate by hand goes here.
    private static readonly HashSet<string> KnownWeakPins = new(StringComparer.Ordinal)
    {
        "2580", "0852",
        "1379", "9731", "3197", "7913",
        "1593", "3951", "7531", "1357",
        "147258", "258369", "369258",
        "147369", "963741", "159753", "357159",
    };

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

    private static bool IsAsciiDigits(string s)
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
