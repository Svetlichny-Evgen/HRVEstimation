using System;
using System.Collections.Generic;
using System.Text;

namespace HRVEstimation
{
    public class HeartRateVariabilityCalculator
    {
        public double CalculateMeanRR(List<double> intervals)
        {
            double sum = 0;
            foreach (var interval in intervals)
            {
                sum += interval;
            }
            return sum / intervals.Count;
        }

        public double CalculateSDNN(List<double> intervals)
        {
            double meanRR = CalculateMeanRR(intervals);
            double sumOfSquares = 0;
            foreach (var interval in intervals)
            {
                sumOfSquares += Math.Pow(interval - meanRR, 2);
            }
            return Math.Sqrt(sumOfSquares / intervals.Count);
        }

        public double CalculateSDSD(List<double> intervals)
        {
            List<double> differences = new List<double>();
            for (int i = 0; i < intervals.Count - 1; i++)
            {
                differences.Add(intervals[i + 1] - intervals[i]);
            }
            double sumOfSquares = 0;
            foreach (var difference in differences)
            {
                sumOfSquares += Math.Pow(difference, 2);
            }
            return Math.Sqrt(sumOfSquares / differences.Count);
        }

        public double CalculateRMSSD(List<double> intervals)
        {
            List<double> differences = new List<double>();
            for (int i = 0; i < intervals.Count - 1; i++)
            {
                differences.Add(intervals[i + 1] - intervals[i]);
            }
            double sumOfSquares = 0;
            foreach (var difference in differences)
            {
                sumOfSquares += Math.Pow(difference, 2);
            }
            return Math.Sqrt(sumOfSquares / intervals.Count);
        }

        public double CalculatePNN50(List<double> intervals)
        {
            int nn50 = 0;
            for (int i = 0; i < intervals.Count - 1; i++)
            {
                if (Math.Abs(intervals[i + 1] - intervals[i]) > 50)
                {
                    nn50++;
                }
            }
            return ((double)nn50 / intervals.Count) * 100;
        }

        public double CalculatePNN20(List<double> intervals)
        {
            int nn20 = 0;
            for (int i = 0; i < intervals.Count - 1; i++)
            {
                if (Math.Abs(intervals[i + 1] - intervals[i]) > 20)
                {
                    nn20++;
                }
            }
            return ((double)nn20 / intervals.Count) * 100;
        }
    }
}
