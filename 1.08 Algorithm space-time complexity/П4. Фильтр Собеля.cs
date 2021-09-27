using System;

namespace Recognizer
{
    internal static class SobelFilterTask
    {
        public static double[,] SobelFilter(double[,] g, double[,] sx)
        {
            var width = g.GetLength(0);
            var height = g.GetLength(1);
            var filterCenter = sx.GetLength(0) / 2;
            var result = new double[width, height];
            
            for (var x = filterCenter; x < width - filterCenter; x++)
                for (var y = filterCenter; y < height - filterCenter; y++)
                {
                    var gx = 0.0;
                    for (var i = -filterCenter; i <= filterCenter; i++)
                        for (var j = -filterCenter; j <= filterCenter; j++)
                            gx += g[x + j, y + i] * sx[j + filterCenter, i + filterCenter];
                    
                    var gy = 0.0;
                    for (var i = -filterCenter; i <= filterCenter; i++)
                        for (var j = -filterCenter; j <= filterCenter; j++)
                            gy += g[x + j, y + i] * sx[i + filterCenter, j + filterCenter];
                    
                    result[x, y] = Math.Sqrt(gx * gx + gy * gy);
                }
            
            return result;
        }
    }
}