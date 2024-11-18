using System.Net.NetworkInformation;
using NetworkUtility.DNS;

namespace NetworkUtility.Ping
{
    public class NetworkService(IDNS dNS)
    {
        private readonly IDNS _dNS = dNS;
        public string SendPing()
        {
            var dnsSuccess = _dNS.SendDNS();
            if (dnsSuccess)
                return "Success: Ping Sent!";
            else
                return "Failed: Ping not sent!";
        }

        public int PingTimeout(int a, int b)
        {
            return a + b;
        }

        public DateTime LastPingDate()
        {
            return DateTime.Now;
        }

        public PingOptions GetPingOptions()
        {
            return new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };
        }

        public IEnumerable<PingOptions> MostRecentPings()
        {
            IEnumerable<PingOptions> pingOptions = [
               new PingOptions()
                {
                    DontFragment = false,
                    Ttl = 1
                },
                new PingOptions()
                {
                    DontFragment = true,
                    Ttl = 2
                },
                new PingOptions()
                {
                    DontFragment = false,
                    Ttl = 3
                },
                new PingOptions()
                {
                    DontFragment = true,
                    Ttl = 4
                }
            ];

            return pingOptions;
        }
    }
}