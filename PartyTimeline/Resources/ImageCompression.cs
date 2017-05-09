using System;
namespace PartyTimeline
{
	public static class ImageCompression
	{
		public enum ScaleDown
		{
			Height,
			Width,
			None
		}

		public readonly static int MaximumDimension = 1280; // px
		public readonly static int CompressionFactorJpeg = 80; // percent, [0-100]

		public static ScaleDown DeterminePrimaryScaleDimension(int height, int width)
		{
			if (height < MaximumDimension && width < MaximumDimension)
			{
				return ScaleDown.None;
			}
			return height > width ? ScaleDown.Height : ScaleDown.Width;
		}

		public static int SecondaryTargetSize(int primaryDimensionOriginal, int secondaryDimensionOriginal)
		{
			double factor = MaximumDimension / (double)primaryDimensionOriginal;
			return (int)Math.Round(secondaryDimensionOriginal * factor);
		}
	}
}
