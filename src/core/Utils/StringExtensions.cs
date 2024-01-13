namespace Invocative.Neko.Framework.Utils;

public static class StringExtensions
{
    public static string Join(this IEnumerable<string> e, string symbol)
        => string.Join(symbol, e);
}
