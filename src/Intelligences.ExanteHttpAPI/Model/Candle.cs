using System;

namespace Intelligences.ExanteHttpAPI.Model
{
    public class Candle
    {
        private readonly decimal open;
        private readonly decimal hight;
        private readonly decimal low;
        private readonly decimal close;
        private readonly string symbol;
        private readonly DateTimeOffset dateTime;

        public Candle(
            decimal open,
            decimal hight,
            decimal low,
            decimal close,
            string symbol,
            DateTimeOffset dateTime
        ) {
            this.open = open;
            this.hight = hight;
            this.low = low;
            this.close = close;
            this.symbol = symbol;
            this.dateTime = dateTime;
        }

        public decimal GetOpen()
        {
            return this.open;
        }

        public decimal GetHight()
        {
            return this.hight;
        }

        public decimal GetLow()
        {
            return this.low;
        }

        public decimal GetClose()
        {
            return this.close;
        }

        public string GetSymbol()
        {
            return this.symbol;
        }

        public DateTimeOffset GetDateTime()
        {
            return this.dateTime;
        }
    }
}
