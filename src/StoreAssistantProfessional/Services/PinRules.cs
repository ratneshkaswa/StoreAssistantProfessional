namespace StoreAssistantProfessional.Services;

public static class PinRules
{
    public static bool IsWeak(string pin)
    {
        if (pin.Length < 2 || !pin.All(char.IsDigit)) return false;

        if (pin.All(c => c == pin[0])) return true;

        bool asc = true, desc = true;
        for (var i = 1; i < pin.Length; i++)
        {
            if (pin[i] - pin[i - 1] != 1) asc = false;
            if (pin[i - 1] - pin[i] != 1) desc = false;
        }
        if (asc || desc) return true;

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
        }

        if (pin.Length == 6 && pin[..3] == pin[3..]) return true;

        return false;
    }
}
