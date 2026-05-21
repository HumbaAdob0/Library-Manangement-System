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
    /// Validates ISBN-13 dash formatting and checksum.
    /// </summary>
    public static bool IsValidISBN13(string isbn)
    {
        if (!FormattedISBN13Regex.IsMatch(isbn))
            return false;

        var digits = GetDigits(isbn);
        
        // Must be exactly 13 digits
        if (digits.Length != 13)
            return false;
        
        // Must start with 978 or 979
        if (!digits.StartsWith("978") && !digits.StartsWith("979"))
            return false;
        
        // Validate checksum
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            int digit = int.Parse(digits[i].ToString());
            sum += (i % 2 == 0) ? digit : digit * 3;
        }
        
        int checkDigit = (10 - (sum % 10)) % 10;
        return checkDigit == int.Parse(digits[12].ToString());
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
