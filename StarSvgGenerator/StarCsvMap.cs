using CsvHelper.Configuration;

namespace StarSvgGenerator
{
    public class StarCsvMap : ClassMap<CsvStar>
    {
        public StarCsvMap()
        {
            Map(x => x.Constellation).Name("con");
            Map(x => x.Declination).Name("dec");
            Map(x => x.RightAscension).Name("ra");
            Map(x => x.Name).Name("proper");
            Map(x => x.Magnitude).Name("mag");
        }
    }
}
