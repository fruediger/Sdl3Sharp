namespace Sdl3Sharp.Video.Coloring;

/// <summary>
/// Represents a pixel format
/// </summary>
public enum PixelFormat : uint
{
#pragma warning disable CS1591 // Not sure if it's actually necessary and reasonable to document all of these predefined formats

	Unknown = 0,

	Index1Lsb = (1u                      << 28)
		      | ((uint)PixelType.Index1  << 24)
		      | ((uint)BitmapOrder._4321 << 20)
		      | ((uint)PackedLayout.None << 16)
		      | ((uint)1                 << 8)
		      | ((uint)0                 << 0),

	Index1Msb = (1u                      << 28)
		      | ((uint)PixelType.Index1  << 24)
		      | ((uint)BitmapOrder._1234 << 20)
		      | ((uint)PackedLayout.None << 16)
		      | ((uint)1                 << 8)
		      | ((uint)0                 << 0),

	Index2Lsb = (1u                      << 28)
		      | ((uint)PixelType.Index2  << 24)
		      | ((uint)BitmapOrder._4321 << 20)
		      | ((uint)PackedLayout.None << 16)
		      | ((uint)2                 << 8)
		      | ((uint)0                 << 0),

	Index2Msb = (1u                      << 28)
		      | ((uint)PixelType.Index2  << 24)
		      | ((uint)BitmapOrder._1234 << 20)
		      | ((uint)PackedLayout.None << 16)
		      | ((uint)2                 << 8)
		      | ((uint)0                 << 0),

	Index4Lsb = (1u                      << 28)
		      | ((uint)PixelType.Index4  << 24)
		      | ((uint)BitmapOrder._4321 << 20)
		      | ((uint)PackedLayout.None << 16)
		      | ((uint)4                 << 8)
		      | ((uint)0                 << 0),

	Index4Msb = (1u                      << 28)
		      | ((uint)PixelType.Index4  << 24)
		      | ((uint)BitmapOrder._1234 << 20)
		      | ((uint)PackedLayout.None << 16)
		      | ((uint)4                 << 8)
		      | ((uint)0                 << 0),

	Index8 = (1u                      << 28)
		   | ((uint)PixelType.Index8  << 24)
		   | ((uint)BitmapOrder.None  << 20)
		   | ((uint)PackedLayout.None << 16)
		   | ((uint)8                 << 8)
		   | ((uint)1                 << 0),

	Rgb332 = (1u                      << 28)
		   | ((uint)PixelType.Packed8 << 24)
		   | ((uint)PackedOrder.Xrgb  << 20)
		   | ((uint)PackedLayout._332 << 16)
		   | ((uint)8                 << 8)
		   | ((uint)1                 << 0),

	Xrgb4444 = (1u                       << 28)
		     | ((uint)PixelType.Packed16 << 24)
		     | ((uint)PackedOrder.Xrgb   << 20)
		     | ((uint)PackedLayout._4444 << 16)
		     | ((uint)12                 << 8)
		     | ((uint)2                  << 0),

	Xbgr4444 = (1u                       << 28)
		     | ((uint)PixelType.Packed16 << 24)
		     | ((uint)PackedOrder.Xbgr   << 20)
		     | ((uint)PackedLayout._4444 << 16)
		     | ((uint)12                 << 8)
		     | ((uint)2                  << 0),

	Xrgb1555 = (1u                       << 28)
			 | ((uint)PixelType.Packed16 << 24)
			 | ((uint)PackedOrder.Xrgb   << 20)
			 | ((uint)PackedLayout._1555 << 16)
			 | ((uint)15                 << 8)
			 | ((uint)2                  << 0),

	Xbgr1555 = (1u                       << 28)
			 | ((uint)PixelType.Packed16 << 24)
			 | ((uint)PackedOrder.Xbgr   << 20)
			 | ((uint)PackedLayout._1555 << 16)
			 | ((uint)15                 << 8)
			 | ((uint)2                  << 0),

	Argb4444 = (1u                       << 28)
			 | ((uint)PixelType.Packed16 << 24)
			 | ((uint)PackedOrder.Argb   << 20)
			 | ((uint)PackedLayout._4444 << 16)
			 | ((uint)16                 << 8)
			 | ((uint)2                  << 0),

	Rgba4444 = (1u                       << 28)
			 | ((uint)PixelType.Packed16 << 24)
			 | ((uint)PackedOrder.Rgba   << 20)
			 | ((uint)PackedLayout._4444 << 16)
			 | ((uint)16                 << 8)
			 | ((uint)2                  << 0),

	Abgr4444 = (1u                       << 28)
			 | ((uint)PixelType.Packed16 << 24)
			 | ((uint)PackedOrder.Abgr   << 20)
			 | ((uint)PackedLayout._4444 << 16)
			 | ((uint)16                 << 8)
			 | ((uint)2                  << 0),

	Bgra4444 = (1u                       << 28)
			 | ((uint)PixelType.Packed16 << 24)
			 | ((uint)PackedOrder.Bgra   << 20)
			 | ((uint)PackedLayout._4444 << 16)
			 | ((uint)16                 << 8)
			 | ((uint)2                  << 0),

	Argb1555 = (1u                       << 28)
			 | ((uint)PixelType.Packed16 << 24)
			 | ((uint)PackedOrder.Argb   << 20)
			 | ((uint)PackedLayout._1555 << 16)
			 | ((uint)16                 << 8)
			 | ((uint)2                  << 0),

	Rgba5551 = (1u                       << 28)
			 | ((uint)PixelType.Packed16 << 24)
			 | ((uint)PackedOrder.Rgba   << 20)
			 | ((uint)PackedLayout._5551 << 16)
			 | ((uint)16                 << 8)
			 | ((uint)2                  << 0),

	Abgr1555 = (1u                       << 28)
			 | ((uint)PixelType.Packed16 << 24)
			 | ((uint)PackedOrder.Abgr   << 20)
			 | ((uint)PackedLayout._1555 << 16)
			 | ((uint)16                 << 8)
			 | ((uint)2                  << 0),

	Bgra5551 = (1u                       << 28)
			 | ((uint)PixelType.Packed16 << 24)
			 | ((uint)PackedOrder.Bgra   << 20)
			 | ((uint)PackedLayout._5551 << 16)
			 | ((uint)16                 << 8)
			 | ((uint)2                  << 0),

	Rgb565 = (1u                       << 28)
		   | ((uint)PixelType.Packed16 << 24)
		   | ((uint)PackedOrder.Xrgb   << 20)
		   | ((uint)PackedLayout._565  << 16)
		   | ((uint)16                 << 8)
		   | ((uint)2                  << 0),

	Bgr565 = (1u                       << 28)
		   | ((uint)PixelType.Packed16 << 24)
		   | ((uint)PackedOrder.Xbgr   << 20)
		   | ((uint)PackedLayout._565  << 16)
		   | ((uint)16                 << 8)
		   | ((uint)2                  << 0),

	Rgb24 = (1u                      << 28)
		  | ((uint)PixelType.ArrayU8 << 24)
		  | ((uint)ArrayOrder.Rgb    << 20)
		  | ((uint)PackedLayout.None << 16)
		  | ((uint)24                << 8)
		  | ((uint)3                 << 0),

	Bgr24 = (1u                      << 28)
		  | ((uint)PixelType.ArrayU8 << 24)
		  | ((uint)ArrayOrder.Bgr    << 20)
		  | ((uint)PackedLayout.None << 16)
		  | ((uint)24                << 8)
		  | ((uint)3                 << 0),

	Xrgb8888 = (1u                       << 28)
			 | ((uint)PixelType.Packed32 << 24)
			 | ((uint)PackedOrder.Xrgb   << 20)
			 | ((uint)PackedLayout._8888 << 16)
			 | ((uint)24                 << 8)
			 | ((uint)4                  << 0),

	Rgbx8888 = (1u                        << 28)
			  | ((uint)PixelType.Packed32 << 24)
			  | ((uint)PackedOrder.Rgbx   << 20)
			  | ((uint)PackedLayout._8888 << 16)
			  | ((uint)24                 << 8)
			  | ((uint)4                  << 0),

	Xbgr8888 = (1u                       << 28)
			 | ((uint)PixelType.Packed32 << 24)
			 | ((uint)PackedOrder.Xbgr   << 20)
			 | ((uint)PackedLayout._8888 << 16)
			 | ((uint)24                 << 8)
			 | ((uint)4                  << 0),

	Bgrx8888 = (1u                       << 28)
			 | ((uint)PixelType.Packed32 << 24)
			 | ((uint)PackedOrder.Bgrx   << 20)
			 | ((uint)PackedLayout._8888 << 16)
			 | ((uint)24                 << 8)
			 | ((uint)4                  << 0),

	Argb8888 = (1u                       << 28)
			 | ((uint)PixelType.Packed32 << 24)
			 | ((uint)PackedOrder.Argb   << 20)
			 | ((uint)PackedLayout._8888 << 16)
			 | ((uint)32                 << 8)
			 | ((uint)4                  << 0),

	Rgba8888 = (1u                       << 28)
			 | ((uint)PixelType.Packed32 << 24)
			 | ((uint)PackedOrder.Rgba   << 20)
			 | ((uint)PackedLayout._8888 << 16)
			 | ((uint)32                 << 8)
			 | ((uint)4                  << 0),

	Abgr8888 = (1u                       << 28)
			 | ((uint)PixelType.Packed32 << 24)
			 | ((uint)PackedOrder.Abgr   << 20)
			 | ((uint)PackedLayout._8888 << 16)
			 | ((uint)32                 << 8)
			 | ((uint)4                  << 0),

	Bgra8888 = (1u                       << 28)
			 | ((uint)PixelType.Packed32 << 24)
			 | ((uint)PackedOrder.Bgra   << 20)
			 | ((uint)PackedLayout._8888 << 16)
			 | ((uint)32                 << 8)
			 | ((uint)4                  << 0),

	Xrgb2101010 = (1u                          << 28)
			    | ((uint)PixelType.Packed32    << 24)
			    | ((uint)PackedOrder.Xrgb      << 20)
			    | ((uint)PackedLayout._2101010 << 16)
			    | ((uint)32                    << 8)
			    | ((uint)4                     << 0),

	Xbgr2101010 = (1u                          << 28)
				| ((uint)PixelType.Packed32    << 24)
				| ((uint)PackedOrder.Xbgr      << 20)
				| ((uint)PackedLayout._2101010 << 16)
				| ((uint)32                    << 8)
				| ((uint)4                     << 0),

	Argb2101010 = (1u                          << 28)
				| ((uint)PixelType.Packed32    << 24)
				| ((uint)PackedOrder.Argb      << 20)
				| ((uint)PackedLayout._2101010 << 16)
				| ((uint)32                    << 8)
				| ((uint)4                     << 0),

	Abgr2101010 = (1u                          << 28)
				| ((uint)PixelType.Packed32    << 24)
				| ((uint)PackedOrder.Abgr      << 20)
				| ((uint)PackedLayout._2101010 << 16)
				| ((uint)32                    << 8)
				| ((uint)4                     << 0),

	Rgb48 = (1u                       << 28)
		  | ((uint)PixelType.ArrayU16 << 24)
		  | ((uint)ArrayOrder.Rgb     << 20)
		  | ((uint)PackedLayout.None  << 16)
		  | ((uint)48                 << 8)
		  | ((uint)6                  << 0),

	Bgr48 = (1u                       << 28)
		  | ((uint)PixelType.ArrayU16 << 24)
		  | ((uint)ArrayOrder.Bgr     << 20)
		  | ((uint)PackedLayout.None  << 16)
		  | ((uint)48                 << 8)
		  | ((uint)6                  << 0),

	Rgba64 = (1u                       << 28)
		   | ((uint)PixelType.ArrayU16 << 24)
		   | ((uint)ArrayOrder.Rgba    << 20)
		   | ((uint)PackedLayout.None  << 16)
		   | ((uint)64                 << 8)
		   | ((uint)8                  << 0),

	Argb64 = (1u                       << 28)
		   | ((uint)PixelType.ArrayU16 << 24)
		   | ((uint)ArrayOrder.Argb    << 20)
		   | ((uint)PackedLayout.None  << 16)
		   | ((uint)64                 << 8)
		   | ((uint)8                  << 0),

	Bgra64 = (1u                       << 28)
		   | ((uint)PixelType.ArrayU16 << 24)
		   | ((uint)ArrayOrder.Bgra    << 20)
		   | ((uint)PackedLayout.None  << 16)
		   | ((uint)64                 << 8)
		   | ((uint)8                  << 0),

	Abgr64 = (1u                       << 28)
		   | ((uint)PixelType.ArrayU16 << 24)
		   | ((uint)ArrayOrder.Abgr    << 20)
		   | ((uint)PackedLayout.None  << 16)
		   | ((uint)64                 << 8)
		   | ((uint)8                  << 0),

	Rgb48Float = (1u                        << 28)
				| ((uint)PixelType.ArrayF16 << 24)
				| ((uint)ArrayOrder.Rgb     << 20)
				| ((uint)PackedLayout.None  << 16)
				| ((uint)48                 << 8)
				| ((uint)6                  << 0),

	Bgr48Float = (1u                        << 28)
				| ((uint)PixelType.ArrayF16 << 24)
				| ((uint)ArrayOrder.Bgr     << 20)
				| ((uint)PackedLayout.None  << 16)
				| ((uint)48                 << 8)
				| ((uint)6                  << 0),

	Rgba64Float = (1u                        << 28)
				 | ((uint)PixelType.ArrayF16 << 24)
				 | ((uint)ArrayOrder.Rgba    << 20)
				 | ((uint)PackedLayout.None  << 16)
				 | ((uint)64                 << 8)
				 | ((uint)8                  << 0),

	Argb64Float = (1u                        << 28)
				 | ((uint)PixelType.ArrayF16 << 24)
				 | ((uint)ArrayOrder.Argb    << 20)
				 | ((uint)PackedLayout.None  << 16)
				 | ((uint)64                 << 8)
				 | ((uint)8                  << 0),

	Bgra64Float = (1u                        << 28)
				 | ((uint)PixelType.ArrayF16 << 24)
				 | ((uint)ArrayOrder.Bgra    << 20)
				 | ((uint)PackedLayout.None  << 16)
				 | ((uint)64                 << 8)
				 | ((uint)8                  << 0),

	Abgr64Float = (1u                        << 28)
				 | ((uint)PixelType.ArrayF16 << 24)
				 | ((uint)ArrayOrder.Abgr    << 20)
				 | ((uint)PackedLayout.None  << 16)
				 | ((uint)64                 << 8)
				 | ((uint)8                  << 0),

	Rgb96Float = (1u                        << 28)
				| ((uint)PixelType.ArrayF32 << 24)
				| ((uint)ArrayOrder.Rgb     << 20)
				| ((uint)PackedLayout.None  << 16)
				| ((uint)96                 << 8)
				| ((uint)12                 << 0),

	Bgr96Float = (1u                        << 28)
				| ((uint)PixelType.ArrayF32 << 24)
				| ((uint)ArrayOrder.Bgr     << 20)
				| ((uint)PackedLayout.None  << 16)
				| ((uint)96                 << 8)
				| ((uint)12                 << 0),

	Rgba128Float = (1u                        << 28)
				  | ((uint)PixelType.ArrayF32 << 24)
				  | ((uint)ArrayOrder.Rgba    << 20)
				  | ((uint)PackedLayout.None  << 16)
				  | ((uint)128                << 8)
				  | ((uint)16                 << 0),

	Argb128Float = (1u                        << 28)
				  | ((uint)PixelType.ArrayF32 << 24)
				  | ((uint)ArrayOrder.Argb    << 20)
				  | ((uint)PackedLayout.None  << 16)
				  | ((uint)128                << 8)
				  | ((uint)16                 << 0),

	Bgra128Float = (1u                        << 28)
				  | ((uint)PixelType.ArrayF32 << 24)
				  | ((uint)ArrayOrder.Bgra    << 20)
				  | ((uint)PackedLayout.None  << 16)
				  | ((uint)128                << 8)
				  | ((uint)16                 << 0),

	Abgr128Float = (1u                        << 28)
				  | ((uint)PixelType.ArrayF32 << 24)
				  | ((uint)ArrayOrder.Abgr    << 20)
				  | ((uint)PackedLayout.None  << 16)
				  | ((uint)128                << 8)
				  | ((uint)16                 << 0),

	Yv12 = ((uint)(byte)'Y' << 0)
		 | ((uint)(byte)'V' << 8)
		 | ((uint)(byte)'1' << 16)
		 | ((uint)(byte)'2' << 24),

	Iyuv = ((uint)(byte)'I' << 0)
		 | ((uint)(byte)'Y' << 8)
		 | ((uint)(byte)'U' << 16)
		 | ((uint)(byte)'V' << 24),

	Yuy2 = ((uint)(byte)'Y' << 0)
		 | ((uint)(byte)'U' << 8)
		 | ((uint)(byte)'Y' << 16)
		 | ((uint)(byte)'2' << 24),

	Uyvy = ((uint)(byte)'U' << 0)
		 | ((uint)(byte)'Y' << 8)
		 | ((uint)(byte)'V' << 16)
		 | ((uint)(byte)'Y' << 24),

	Yvyu = ((uint)(byte)'Y' << 0)
		 | ((uint)(byte)'V' << 8)
		 | ((uint)(byte)'Y' << 16)
		 | ((uint)(byte)'U' << 24),

	Nv12 = ((uint)(byte)'N' << 0)
		 | ((uint)(byte)'V' << 8)
		 | ((uint)(byte)'1' << 16)
		 | ((uint)(byte)'2' << 24),

	Nv21 = ((uint)(byte)'N' << 0)
		 | ((uint)(byte)'V' << 8)
		 | ((uint)(byte)'2' << 16)
		 | ((uint)(byte)'1' << 24),

	P010 = ((uint)(byte)'P' << 0)
		 | ((uint)(byte)'0' << 8)
		 | ((uint)(byte)'1' << 16)
		 | ((uint)(byte)'0' << 24),

	ExternalOes = ((uint)(byte)'O' << 0)
		        | ((uint)(byte)'E' << 8)
		        | ((uint)(byte)'S' << 16)
		        | ((uint)(byte)' ' << 24),

	MJPG = ((uint)(byte)'M' << 0)
		 | ((uint)(byte)'J' << 8)
		 | ((uint)(byte)'P' << 16)
		 | ((uint)(byte)'G' << 24),

#pragma warning restore CS1591
}
