using System.Linq;
using System.Collections.Generic;

namespace BackendAuthTemplate.Application.Common.Utils;

public static class StringExtensions
{
    /// <summary>
    /// Capitalizes a person's name while handling spaces, hyphens and apostrophes.
    /// </summary>
    public static string CapitalizeProperName(this string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return name;
        }

        string[] words = name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        IEnumerable<string> result = words.Select(word =>
        {
            string[] parts = word.Split(['-', '\''], StringSplitOptions.None);
            List<char> separators = word.Where(c => c is '-' or '\'').ToList();

            List<string> formatted = parts.Select(p => char.ToUpper(p[0]) + p[1..].ToLower()).ToList();

            string reassembled = formatted[0];
            for (int i = 1; i < formatted.Count; i++)
            {
                reassembled += separators[i - 1] + formatted[i];
            }

            return reassembled;
        });

        return string.Join(" ", result);
    }
}
