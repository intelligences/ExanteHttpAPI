using Intelligences.ExanteHttpAPI.DAO;
using Intelligences.ExanteHttpAPI.Enum;
using Intelligences.ExanteHttpAPI.Model;
using Intelligences.ExanteHttpAPI.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Intelligences.ExanteHttpAPI
{
    public class ExanteClient
    {
        private readonly HttpClientService apiClient;

        public ExanteClient(Settings settings)
        {
            this.apiClient = new HttpClientService(settings);
        }

        public List<Candle> GetCandles(string symbol, TimeSpan? timeFrame = null, DateTimeOffset? from = null, DateTimeOffset? to = null, int count = 60, CandleSource candleSource = CandleSource.Trades)
        {
            if (timeFrame is null)
            {
                timeFrame = TimeSpan.FromMinutes(1);
            }

            Dictionary<string, dynamic> parameters = new Dictionary<string, dynamic>();

            if (from != null)
            {
                parameters.Add("from", from.GetValueOrDefault().ToUnixTimeMilliseconds());
            }

            if (to != null)
            {
                parameters.Add("to", to.GetValueOrDefault().ToUnixTimeMilliseconds());
            }

            parameters.Add("size", count);
            parameters.Add("type", candleSource.ToString().ToLower());


            string uri = string.Format("/md/2.0/ohlc/{0}/{1}", symbol.ToUpper(), timeFrame.GetValueOrDefault().TotalSeconds);

            string response = this.apiClient.SendRequest(HttpMethod.GET, uri, parameters);

            List<CandleDAO> candlesDAO = JsonConvert.DeserializeObject<List<CandleDAO>>(response);

            List <Candle> candles = new List<Candle>();

            foreach(CandleDAO candle in candlesDAO)
            {
                DateTimeOffset date = DateTimeOffset.FromUnixTimeMilliseconds(candle.Timestamp);

                candles.Add(new Candle(
                    candle.Open,
                    candle.High,
                    candle.Low,
                    candle.Close,
                    symbol,
                    date
                ));
            }

            return candles;
        }
    }
}
