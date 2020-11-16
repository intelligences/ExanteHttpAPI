using System;

namespace Intelligences.ExanteHttpAPI.DAO
{
    internal class CandleDAO
    {
        public long Timestamp { get; set; }
        public decimal Open { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal High { get; set; }
    }
}
