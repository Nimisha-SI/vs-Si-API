using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApis.Model;

namespace WebApis.elastic
{
    public interface ESInterface
    {
          ElasticClient CreateConnection();
        void BulkInsert<T>(ElasticClient EsClient, List<T> documents, string IndexName) where T : class;
          //void BulkInsert(ElasticClient EsClient, List<SearchS2Data> cricketdata);
    }
}
