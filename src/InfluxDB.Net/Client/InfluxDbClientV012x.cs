using InfluxDB.Net.Contracts;
using InfluxDB.Net.Enums;
using InfluxDB.Net.Infrastructure.Configuration;
using InfluxDB.Net.Infrastructure.Formatters;

namespace InfluxDB.Net.Client
{
    internal class InfluxDbClientV012x : InfluxDbClientBase
    {
        public InfluxDbClientV012x(InfluxDbClientConfiguration configuration)
            : base(configuration)
        {
        }

        public override IFormatter GetFormatter()
        {
            return new FormatterV012x();
        }

        public override InfluxVersion GetVersion()
        {
            return InfluxVersion.v012x;
        }
    }
}