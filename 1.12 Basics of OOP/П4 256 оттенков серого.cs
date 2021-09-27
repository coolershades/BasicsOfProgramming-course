using System.Collections.Generic;
using System.Drawing;
using GeometryTasks;

namespace GeometryPainting
{
    public static class SegmentExtensions
    {
        public static Dictionary<Segment, Color> SegmentColors = new Dictionary<Segment, Color>();

        public static void SetColor(this Segment segment, Color color)
        {
            if (!SegmentColors.ContainsKey(segment))
                SegmentColors.Add(segment, color);
            else
                SegmentColors[segment] = color;
        }

        public static Color GetColor(this Segment segment)
        {
            if (SegmentColors.ContainsKey(segment))
                return SegmentColors[segment];
            return Color.Black;
        }
    }
}