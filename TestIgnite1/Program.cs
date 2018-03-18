using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache;
using Apache.Ignite.Core.Cache.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using misysdatamodel;
using Apache.Ignite.Core.Cache.Configuration;

namespace TestIgnite1
{
    class Program
    {
        public class QueryFilter : ICacheEntryFilter<string, Forex>
        {
            public bool Invoke(ICacheEntry<string, Forex> entry)
                => entry.Value.Common.HasId && entry.Value.Common.Id == "EUR";
        }
        static void Main(string[] args)
        {
            var cfg = new IgniteConfiguration
            {
                CacheConfiguration = new[]
                {
                    new CacheConfiguration
                    {
                        Name = "myCache",
                        QueryEntities = new[]
                        {
                            new QueryEntity
                            {
                                KeyType = typeof(string),
                                ValueType = typeof(Forex),
                                Fields = new[]
                                {
                                    new QueryField {Name = "FINANCING", FieldType = typeof(bool)}
                                }
                            }
                        }
                    }
                }
            };
            using (var ignite = Ignition.Start(cfg))
            {
                var cache = ignite.GetOrCreateCache<string, Forex>("myCache");

                var forexWritten = Forex.CreateBuilder()
                    .SetCommon(CommonFields.CreateBuilder().SetId("EUR").SetName("EUR").SetTimestamp(misysdatamodel.DateTime.DefaultInstance).SetSourceRef("tt").SetInstanceRef("zzz").Build())
                    .SetFinancing(true)
                    .AddQuotes(Forex.Types.ForexQuote.CreateBuilder().SetLast(12).Build())
                    .Build();

                cache.Put("EUR", forexWritten);
                var f = cache.Get("EUR");

                var ff = cache.Query(new SqlQuery(typeof(Forex), "FINANCING = TRUE")).GetAll();
                int count = ff.Count;
                Forex f1 = ff.First().Value;

                // Store keys in cache (values will end up on different cache nodes).
                //for (int i = 0; i < 10; i++)
                //    cache.Put(i, $"toto {i}");

                //for (int i = 0; i < 10; i++)
                //    Console.WriteLine("Got [key={0}, val={1}]", i, cache.Get(i));
            }
        }
    }
}
