using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Color HexToColor(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex[0] != '#' || (hex.Length != 7 && hex.Length != 9))
        {
            Debug.LogError($"Hex color '{hex}' is not valid. Use format like '#RRGGBB' or '#RRGGBBAA'.");
            return Color.white;
        }

        byte r = byte.Parse(hex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
        byte a = 255;

        if (hex.Length == 9)
        {
            a = byte.Parse(hex.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
        }

        return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }

    public static string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255f);
        int g = Mathf.RoundToInt(color.g * 255f);
        int b = Mathf.RoundToInt(color.b * 255f);
        int a = Mathf.RoundToInt(color.a * 255f);

        return a < 255
            ? $"#{r:X2}{g:X2}{b:X2}{a:X2}"
            : $"#{r:X2}{g:X2}{b:X2}";
    }

    public static string FormatNumber(long value, char separator)
    {
        if (value < 1000)
        {
            return value.ToString();
        }

        List<char> chars = new();

        var stringValue = value.ToString();
        var offset = 0;

        for (var i = stringValue.Length - 1; i >= 0; i--)
        {
            chars.Add(stringValue[i]);

            if ((chars.Count - offset) % 3 == 0)
            {
                chars.Add(separator);
                offset++;
            }
        }

        chars.Reverse();

        return string.Join("", chars.ToArray()).Trim(separator);
    }

    public static string FormatCompactNumber(float value)
    {
        if (value < 1_000f)
        {
            return value.ToString("F1");
        }
        else if (value < 1_000_000f)
        {
            return $"{value / 1_000f:F1}K";
        }
        else if (value < 1_000_000_000f)
        {
            return $"{value / 1_000_000f:F1}M";
        }
        else
        {
            return $"{value / 1_000_000_000f:F1}B";
        }
    }
}