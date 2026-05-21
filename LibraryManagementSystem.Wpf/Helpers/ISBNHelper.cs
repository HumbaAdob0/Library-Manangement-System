using System.Text.RegularExpressions;

namespace LibraryManagementSystem.Helpers;

public static class ISBNHelper
{
    private static readonly Regex DigitsOnlyRegex = new(@"[^\d]", RegexOptions.Compiled);
    private static readonly Regex FormattedISBN13Regex = new(@"^\d{3}-\d-\d{3}-\d{5}-\d$", RegexOptions.Compiled);

    /// <summary>
    /// Formats ISBN-13 with dashes: 978-0-123-45678-9
    /// </summary>
    public static string FormatISBN13(string isbn)
    {
        var digits = GetDigits(isbn);
        
        // Limit to 13 digits
        if (digits.Length > 13)
            digits = digits.Substring(0, 13);
        
        // Format with dashes
        if (digits.Length <= 3)
            return digits;
        if (digits.Length <= 4)
            return $"{digits.Substring(0, 3)}-{digits.Substring(3)}";
        if (digits.Length <= 7)
            return $"{digits.Substring(0, 3)}-{digits.Substring(3, 1)}-{digits.Substring(4)}";
        if (digits.Length <= 12)
            return $"{digits.Substring(0, 3)}-{digits.Substring(3, 1)}-{digits.Substring(4, 3)}-{digits.Substring(7)}";
        
        return $"{digits.Substring(0, 3)}-{digits.Substring(3, 1)}-{digits.Substring(4, 3)}-{digits.Substring(7, 5)}-{digits.Substring(12)}";
    }

    /// <summary>
    /// Validates the required 13-digit dash-separated format.
    /// </summary>
    public static bool IsValidISBN13(string isbn)
    {
        return FormattedISBN13Regex.IsMatch(isbn ?? string.Empty);
    }

    public static string GetDigits(string isbn)
    {
        return DigitsOnlyRegex.Replace(isbn ?? string.Empty, string.Empty);
    }

    /// <summary>
    /// Gets the cursor position after formatting
    /// </summary>
    public static int GetCursorPosition(string oldText, string newText, int oldCursorPos)
    {
        // Count dashes before cursor in old text
        int dashesBeforeOld = oldText.Substring(0, Math.Min(oldCursorPos, oldText.Length))
            .Count(c => c == '-');
        
        // Count digits before cursor in old text
        int digitsBeforeOld = oldText.Substring(0, Math.Min(oldCursorPos, oldText.Length))
            .Count(char.IsDigit);
        
        // Find position in new text with same number of digits
        int digitsCount = 0;
        for (int i = 0; i < newText.Length; i++)
        {
            if (char.IsDigit(newText[i]))
            {
                digitsCount++;
                if (digitsCount == digitsBeforeOld)
                    return i + 1;
            }
        }
        
        return newText.Length;
    }
}
