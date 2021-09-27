namespace Recognizer
{
	public static class GrayscaleTask
	{
		public static double[,] ToGrayscale(Pixel[,] original)
		{
			var width = original.GetLength(0);
			var height = original.GetLength(1);
			var grayscaleImage = new double[width, height];
			
			for (var i = 0; i < width; i++)
				for (var j = 0; j < height; j++)
				{
					var pixel = original[i, j];
					grayscaleImage[i, j] = (pixel.R * 0.299 
					                        + pixel.G * 0.587 
					                        + pixel.B * 0.114) / 255;
				}
				return grayscaleImage;
		}
	}
}