namespace QMS.Application.Issues.Service.Issues.AnalyzeData
{
    public class DataPairOutput
    {
        public DateTime Day { get; set; }
        public StatisticData Data { get; set; }

        public DataPairOutput(DateTime day)
        {
            this.Day = day;
            this.Data = StatisticData.DefaultEmpty;
        }


        public DataPairOutput(DateTime day, StatisticData data)
        {
            this.Day = day;
            this.Data = data;
        }
    }
}
