using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sophis.GridCache.Service;
using Sophis.GridCache.DotNet;
using misysdatamodel;

namespace TestGridCacheService
{
    class Program
    {
        const string _region = "/root/sophis";
        static void Main(string[] args)
        {
            DotNet_GridCacheAccess.Initialize(new DotNet_GridCacheConfiguration());
            var gridCache = new SophisGridCacheBasicService();
            //var obj = gridCache.get("/root/sophis", "TestHeartBeatServerPARD013447_15628");
            var forexWritten = Forex.CreateBuilder()
                .SetCommon(CommonFields.CreateBuilder().SetId("EUR").SetName("EUR").SetTimestamp(misysdatamodel.DateTime.DefaultInstance).SetSourceRef("tt").SetInstanceRef("zzz").Build())
                .AddQuotes(Forex.Types.ForexQuote.CreateBuilder().SetLast(12).Build())
                .Build();
            gridCache.put(_region, "forex1", new Sophis.GridCache.Contract.SophisGridCacheObject() { Bytes = forexWritten.ToByteArray() });
            gridCache.put(_region, "forex2", new Sophis.GridCache.Contract.SophisGridCacheObject() { Bytes = forexWritten.ToByteArray() });
            var objs = gridCache.getList(_region, new[] { "forex1", "forex2" });
            if (objs != null)
            {
                foreach (var obj in objs)
                {
                    var forexRead = Forex.ParseFrom(obj.Value.Bytes);
                    Console.WriteLine("Got: {0}", forexRead.ToString());
                }
            }
            else
                Console.WriteLine("Not found :(");
        }
    }
}
