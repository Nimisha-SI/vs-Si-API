using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
   public interface IKeyTags
    {
        IEnumerable<FTSResultData> FetchSearchFTSResultData(List<SearchQueryModel> lstSearchFields, Dictionary<string, string> str, int sportid = 1, string stype = "s1");
        List<KTData> FreeTextMapping(QueryContainer _objQuery, ElasticClient EsClient, string IndexName);
        IEnumerable<SearchCricketResultDataFreeText> FetchSearchResultData(QueryContainer qc, ElasticClient EsClient);
        List<SearchCricketResultDataFreeText> FetchSearchResultDataKeyTags(ElasticClient elasticClient, QueryContainer _objNestedQuery);
        List<KTData> GetSelectedFTData(List<SearchQueryModel> objlstSearchQueryModel, int sportid = 1, int stype = 1, bool isAutoComplete = true);
        List<FTSResultData> FTSresultdataMap(ElasticClient elasticClient, QueryContainer _objNestedQuery, string IndexName);
        QueryContainer SearchResultData(List<SearchQueryModel> lstSearchFields, int sportid = 1, string languageid = "1");
    }
}
