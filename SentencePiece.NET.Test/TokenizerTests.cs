using NUnit.Framework;

namespace SentencePiece.NET.Test;

public class TokenizerTests
{
    private SentencePieceProcessor _processor;
    
    [OneTimeSetUp]
    public void LoadModel()
    {
        _processor = new SentencePieceProcessor();
        var status = _processor.Load(Initialization.SentencePieceModelPath);
        Assert.That(status, Is.True);
        
        _processor.SetEncodeExtraOptions(ExtraOptions.EncodeBosEos);
    }
    
    [OneTimeTearDown]
    public void Dispose()
    {
        _processor.Dispose();
    }
    
    [TestCase("Hello, world!", new[] {1, 35377, 3, 8998, 37, 2})]
    [TestCase("Hello world", new[] {1, 35377, 8998, 2})]
    [TestCase("", new[] {1, 2})]
    public void TestEncodeWithBosEos(string value, int[] tokenIds)
    {
        Assert.That(_processor.Encode(value), Is.EquivalentTo(tokenIds));
    }

    [Test]
    public void TestEncoderOptions()
    {
        try
        {
            var pieceIds = _processor.Encode("Hello world");
            Assert.That(pieceIds, Is.EquivalentTo([1, 35377, 8998, 2]));
        
            _processor.SetEncodeExtraOptions(ExtraOptions.EncodeBosEosReverse);
        
            pieceIds = _processor.Encode("Hello world");
            Assert.That(pieceIds, Is.EquivalentTo([1, 8998, 35377, 2]));
        
            _processor.SetEncodeExtraOptions(ExtraOptions.Default);
        
            pieceIds = _processor.Encode("Hello world");
            Assert.That(pieceIds, Is.EquivalentTo([8998, 35377]));
        
            Assert.That(() => _processor.SetEncodeExtraOptions(null), Throws.ArgumentNullException);
        }
        finally
        {
            _processor.SetEncodeExtraOptions(ExtraOptions.EncodeBosEos);
        }
    }
    
    [Test]
    public void TestGetPieceSize()
    {
        var size = _processor.GetPieceSize();
        Assert.That(size, Is.EqualTo(250000));
    }

    [Test]
    public void TestAllPieces()
    {
        var size = _processor.GetPieceSize();
        foreach (var i in Enumerable.Range(0, size).Randomize())
        {
            var piece = _processor.IdToPiece(i);
            var pieceId = _processor.PieceToId(piece);
            Assert.That(pieceId, Is.EqualTo(i));
        }
        
        Assert.That(() => _processor.PieceToId(null), Throws.ArgumentNullException);
        Assert.That(_processor.PieceToId(string.Empty), Is.EqualTo(TestData.UnkToken.Id));
        
        Assert.That(_processor.PieceToId("ÄsÄsÄ"), Is.EqualTo(TestData.UnkToken.Id));
    }
    
    [TestCaseSource(typeof(TestData), nameof(TestData.Values))]
    public void TestPieceToTokenId(string piece, int pieceId)
    {
        Assert.That(_processor.PieceToId(piece), Is.EqualTo(pieceId));
    }

    [TestCaseSource(typeof(TestData), nameof(TestData.Values))]
    public void TestTokenIdToPiece(string piece, int pieceId)
    {
        Assert.That(_processor.IdToPiece(pieceId), Is.EqualTo(piece));
    }
    
    [TestCaseSource(typeof(TestData), nameof(TestData.UnkTestData))]
    public bool TestTokenIsUnk(string name, int pieceId)
    {
        return _processor.IsUnknown(pieceId);
    }
    
    [TestCaseSource(typeof(TestData), nameof(TestData.ControlTestData))]
    public bool TestTokenIsControl(string name, int pieceId)
    {
        return _processor.IsControl(pieceId);
    }
    
    [Test]
    public void TestEosId()
    {
        Assert.That(_processor.EosId(), Is.EqualTo(2));
    }
    
    [Test]
    public void TestBosId()
    {
        Assert.That(_processor.BosId(), Is.EqualTo(1));
    }
    
    [Test]
    public void TestPadId()
    {
        Assert.That(_processor.PadId(), Is.EqualTo(-1));
    }
    
    [Test]
    public void TestUnkId()
    {
        Assert.That(_processor.UnkId(), Is.EqualTo(0));
    }
}