using Intelligences.ExanteHttpAPI.Enum;
using Intelligences.ExanteHttpAPI.Model;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Intelligences.ExanteHttpAPI.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Settings settings = new Settings(
                HttpApiMode.Demo,
                "CLIENT_ID",
                "APP_ID",
                "SHARED_KEY",
                3600
            );

            ExanteClient client = new ExanteClient(settings);
            TimeSpan timeFrame = TimeSpan.FromSeconds(60);
            DateTimeOffset from = DateTimeOffset.Parse("9.01.2020 10:30:00");
            DateTimeOffset to = DateTimeOffset.Parse("9.01.2020 19:30:00");
            int candlesCount = 500;
            CandleSource candleSource = CandleSource.Trades;

            List<Candle> candles = client.GetCandles("ES.CME.H2020", timeFrame, from, to, candlesCount, candleSource);

            Console.WriteLine(candles);
        }
    }
}
