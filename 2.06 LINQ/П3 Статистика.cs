using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class StatisticsTask
	{
		public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
		{
			return visits
				.GroupBy(rec => rec.UserId)
				.SelectMany(group => group
					.OrderBy(visit => visit.DateTime)
					.Bigrams()
					.Where(pair => pair.Item1.SlideType == slideType
					               && pair.Item1.SlideId != pair.Item2.SlideId)
					.Select(pair => (pair.Item2.DateTime - pair.Item1.DateTime).TotalMinutes))
				.Where(dt => dt >= 1 && dt <= 120)
				.DefaultIfEmpty(0)
				.Median();
		}
	}
}