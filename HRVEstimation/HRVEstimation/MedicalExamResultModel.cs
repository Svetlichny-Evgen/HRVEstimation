using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace HRVEstimation
{
    public class MedicalExamResultModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int PersonId { get; set; }
        public double MeanRR { get; set; }
        public double SDNN { get; set; }
        public double SDSD { get; set; }
        public double RMSSD { get; set; }
        public double PNN50 { get; set; }
        public double PNN20 { get; set; }

        public MedicalExamResultModel()
        {
        }

        public MedicalExamResultModel(int personId, double meanRR, double sdnn, double sdsd, double rmssd, double pnn50, double pnn20)
        {
            PersonId = personId;
            MeanRR = meanRR;
            SDNN = sdnn;
            SDSD = sdsd;
            RMSSD = rmssd;
            PNN50 = pnn50;
            PNN20 = pnn20;
        }
    }
}
