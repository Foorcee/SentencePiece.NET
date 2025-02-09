using NUnit.Framework;

namespace SentencePiece.NET.Test;

public record Piece(string Name, int Id)
{
    public const string Underscore = "\u2581";
    
    public static Piece Whitespaced(string name, int id) => new($"{Underscore}{name}", id);

    public static implicit operator TestCaseData(Piece piece) => new (piece.Name, piece.Id);
    
    public TestCaseData Returns(object value) => ((TestCaseData) this).Returns(value);
}