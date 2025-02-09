using System.Collections;
using NUnit.Framework;

namespace SentencePiece.NET.Test;

public class TestData
{
    public static readonly Piece StartToken = new(Pieces.Start, Pieces.StartId);
    
    public static readonly Piece StopToken = new (Pieces.Stop, Pieces.StopId);
    public static readonly Piece UnkToken = new (Pieces.Unk, Pieces.UnkId);
    public static readonly Piece WorldToken = Piece.Whitespaced("world", 8998);

    public static IEnumerable<TestCaseData> Values =>
    [
        StartToken,
        StopToken,
        UnkToken,
        WorldToken,
    ];
    
    public static IEnumerable<TestCaseData> UnkTestData
    {
        get
        {
            yield return StartToken.Returns(false);
            yield return StopToken.Returns(false);
            yield return UnkToken.Returns(true);
        }
    }
    

    public static IEnumerable<TestCaseData> ControlTestData
    {
        get
        {
            yield return StartToken.Returns(true);
            yield return StopToken.Returns(true);
            yield return UnkToken.Returns(false);
        }
    }
}