using System.Text;

namespace TerrytLookup.Core.Helpers;

public static class StringExtensions
{
    private static readonly Dictionary<char, char> PolishToLatinMap = new()
    {
        {
            'Ą', 'A'
        },
        {
            'Ć', 'C'
        },
        {
            'Ę', 'E'
        },
        {
            'Ł', 'L'
        },
        {
            'Ń', 'N'
        },
        {
            'Ó', 'O'
        },
        {
            'Ś', 'S'
        },
        {
            'Ź', 'Z'
        },
        {
            'Ż', 'Z'
        }
    };

    public static string NormalizeName(this string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var result = new StringBuilder();

        foreach (var c in text)
            result.Append(PolishToLatinMap.GetValueOrDefault(char.ToUpper(c), char.ToUpper(c)));

        return result.ToString();
    }
}