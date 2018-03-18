using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Client;
using Apache.Ignite.Core.Client.Cache;
using misysdatamodel;

namespace TestIgniteThinClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var cfg = new IgniteClientConfiguration
            {
                Host = "127.0.0.1"
            };

            using (IIgniteClient client = Ignition.StartClient(cfg))
            {
                var cache = client.GetCache<string, Forex>("myCache");

                var forexWritten = Forex.CreateBuilder()
                    .SetCommon(CommonFields.CreateBuilder().SetId("EUR").SetName("EUR").SetTimestamp(misysdatamodel.DateTime.DefaultInstance).SetSourceRef("tt").SetInstanceRef("zzz").Build())
                    .AddQuotes(Forex.Types.ForexQuote.CreateBuilder().SetLast(12).Build())
                    .Build();

                cache.Put("EUR", forexWritten);
                var f = cache.Get("EUR");

                //for (int i = 0; i < 10; i++)
                //    cache.Put(i, i.ToString());

                //for (int i = 0; i < 10; i++)
                //    Console.WriteLine("Got [key={0}, val={1}]", i, cache.Get(i));
            }
        }
    }
}
