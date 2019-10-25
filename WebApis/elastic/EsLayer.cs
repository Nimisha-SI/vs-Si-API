using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using WebApis.DAL;
using WebApis.Model;

namespace WebApis.elastic
{
    public class EsLayer : ESInterface
    {
        private ICon _con;
       // GetSearchS1DataForCricket obj = new GetSearchS1DataForCricket();
        string con;
        string index;

      public EsLayer(ICon con) {
            _con = con;
        }

        public ElasticClient CreateConnection()
        {
            con = _con.GetKeyValueAppSetting("elasticsearch", "url");
            index = _con.GetKeyValueAppSetting("elasticsearch", "index");
            Uri EsInstance = new Uri(con);
            ConnectionSettings EsConfiguration = new ConnectionSettings(EsInstance);
            EsConfiguration.DefaultIndex(index);
            EsConfiguration.DisableDirectStreaming();
            ElasticClient EsClient = new ElasticClient(EsConfiguration);
            return EsClient;
        }

        public void BulkInsert(ElasticClient EsClient,  List<SearchS2Data> documents)
            {
           var bulkAllObservable = EsClient.BulkAll(documents, b => b
         .Index("crickets2data")
                  
         .BackOffTime("30s")
         .BackOffRetries(2)
         .RefreshOnCompleted()
         .MaxDegreeOfParallelism(Environment.ProcessorCount)
         .Size(10000)
       )
       .Wait(TimeSpan.FromMinutes(15), next =>
       {
       });
            }

        
    }
}
