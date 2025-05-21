using System;

namespace BackEnd.Entities
{
    public class BodyMetric
    {
        public int BodyMetricID { get; set; }
        public DateTime MeasurementDate { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? BodyFatPercentage { get; set; }
        public decimal? BMI { get; set; }
        public decimal? WaistCircumference { get; set; }
        public decimal? ChestCircumference { get; set; }
        public decimal? ArmCircumference { get; set; }
        public decimal? LegCircumference { get; set; }
        public string Notes { get; set; }
    }
}