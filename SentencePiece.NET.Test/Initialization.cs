using System.Net;
using NUnit.Framework;

namespace SentencePiece.NET.Test;

[SetUpFixture]
public class Initialization
{
    
    public const string SentencePieceModelPath = "test_model.model";

    private const string DownloadUrl =
        "https://huggingface.co/intfloat/multilingual-e5-large/resolve/main/sentencepiece.bpe.model";
    
    [OneTimeSetUp]
    public static async Task Initialize()
    {
        if (!File.Exists(SentencePieceModelPath))
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(DownloadUrl);
        
            if (response.IsSuccessStatusCode)
            {
                var modelData = await response.Content.ReadAsByteArrayAsync();
            
                // Save the file to the desired path
                await File.WriteAllBytesAsync(SentencePieceModelPath, modelData);
            }
            else
            {
                throw new Exception($"Failed to download model. Status Code: {response.StatusCode}");
            }
        }
    }
}