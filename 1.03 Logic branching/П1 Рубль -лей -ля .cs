namespace Pluralize
{
	public static class PluralizeTask
	{
		public static string PluralizeRubles(int count)
		{
			if (count % 100 > 10 && count % 100 < 20)
				return "рублей";

			count %= 10;
			if (count == 1)
				return "рубль";
			if (count >= 2 && count <= 4)
				return "рубля";
			return "рублей";
		}
	}
}