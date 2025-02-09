﻿using System.Runtime.InteropServices;
using SentencePiece.NET.Native;

namespace SentencePiece.NET;

public class SentencePieceProcessor : IDisposable
{
    private IntPtr _processor = SentencePieceNativeLib.spp_create();
    
    public bool Load(string modelPath)
    {
        return SentencePieceNativeLib.spp_load(_processor, modelPath) == 0;
    }
    
    public int[] Encode(string text)
    {
        var encodedPtr = IntPtr.Zero;
        try
        {
            var tokenCount = SentencePieceNativeLib.spp_encode(_processor, text, out encodedPtr);
            if (tokenCount < 0)
                throw new InvalidOperationException("Unable to encode text.");
        
            var tokenIds = new int[tokenCount];
            Marshal.Copy(encodedPtr, tokenIds, 0, tokenCount);
            return tokenIds;
        }
        finally
        {
            if (encodedPtr != IntPtr.Zero)
                SentencePieceNativeLib.spp_free_array(encodedPtr);
        }
    }

    public int GetPieceSize()
    {
        return SentencePieceNativeLib.spp_get_piece_size(_processor);
    }
    
    public int PieceToId(string piece)
    {
        ArgumentNullException.ThrowIfNull(piece, nameof(piece));
        return SentencePieceNativeLib.spp_piece_to_id(_processor, piece);
    }
    
    public string IdToPiece(int pieceId)
    {
        var ptr = SentencePieceNativeLib.spp_id_to_piece(_processor, pieceId);
        var value = Marshal.PtrToStringUTF8(ptr);
        if (value == null)
            throw new InvalidOperationException("Unable to get piece.");
        
        return value;
    }
    
    public bool IsUnknown(int pieceId)
    {
        return SentencePieceNativeLib.spp_is_unknown(_processor, pieceId);
    }
    
    public bool IsControl(int pieceId)
    {
        return SentencePieceNativeLib.spp_is_control(_processor, pieceId);
    }
    
    public int EosId()
    {
        return SentencePieceNativeLib.spp_eos_id(_processor);
    }
    
    public int BosId()
    {
        return SentencePieceNativeLib.spp_bos_id(_processor);
    }
    
    public int PadId()
    {
        return SentencePieceNativeLib.spp_pad_id(_processor);
    }
    
    public int UnkId()
    {
        return SentencePieceNativeLib.spp_unk_id(_processor);
    }
    
    public void SetEncodeExtraOptions(string extraOptions)
    {
        ArgumentNullException.ThrowIfNull(extraOptions, nameof(extraOptions));
        SentencePieceNativeLib.spp_set_encode_extra_options(_processor, extraOptions);
    }
    
    public void Dispose()
    {
        if (_processor == IntPtr.Zero) 
            return;
        
        SentencePieceNativeLib.spp_destroy(_processor);
        _processor = IntPtr.Zero;
    }
}