using ImageMagick;
using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;

namespace CG_Projekt.Framework
{
	public static class Texture
	{
		public static int Load(Stream stream)
		{
			using var image = new MagickImage(stream);
			var format = PixelFormat.Rgba;
			switch (image.ChannelCount)
			{
				case 3: break;
				case 4: format = PixelFormat.Rgba; break;
				default: throw new ArgumentOutOfRangeException("Unexpected image format");
			}
			image.Flip();
			var bytes = image.GetPixelsUnsafe().ToArray();
			var handle = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, handle);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);

			GL.TexImage2D(TextureTarget.Texture2D, 0, (PixelInternalFormat)format, image.Width, image.Height, 0, format, PixelType.UnsignedByte, bytes);
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.GenerateMipmap, 1);
			GL.BindTexture(TextureTarget.Texture2D, 0);
			return handle;
		}
	}
}
