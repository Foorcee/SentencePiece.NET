namespace SentencePiece.NET.Test;

public static class Extensions
{
    public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
    {
        Random rnd = new Random();
        return source.OrderBy(_ => rnd.Next());
    }
}