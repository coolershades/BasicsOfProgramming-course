using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace linq_slideviews
{
	public class ParsingTask
	{
		public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
		{
			var id = 0;
			var type = SlideType.Exercise;
			
			return lines
				.Skip(1)
				.Select(line => line.Split(';'))
				.Where(data => data.Length == 3
				               && int.TryParse(data[0], out id)
				               && Enum.TryParse(ToTitleCase(data[1]), out type))
				.ToDictionary(data => id, 
					data => new SlideRecord(id, type, data[2]));
		}
		
		private static string ToTitleCase(string str)
			=> CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());

		public static IEnumerable<VisitRecord> ParseVisitRecords(
			IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
			=> lines.Skip(1).Select(line => ParseVisitRecord(slides, line));

		private static VisitRecord ParseVisitRecord(IDictionary<int, SlideRecord> slides, string line)
		{
			try
			{
				var data = line.Split(';');
				var slideId = int.Parse(data[1]);
				var date = DateTime.ParseExact(
					data[2] + ' ' + data[3],
					"yyyy-MM-dd HH:mm:ss",
					CultureInfo.InvariantCulture,
					DateTimeStyles.None);
				
				return new VisitRecord(
					int.Parse(data[0]),
					slideId,
					date, 
					slides[slideId].SlideType);
			}
			catch (Exception e)
			{
				throw new FormatException($"Wrong line [{line}]", e);
			}
		}
	}
}