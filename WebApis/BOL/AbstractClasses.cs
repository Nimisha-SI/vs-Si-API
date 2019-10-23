using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
    public abstract class AbstractClasses
    {
        public abstract QueryContainer GetMatchDetailQuery(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, bool isMasterData = false);
        // public Dictionary<string, object> fetchDropdowns(QueryContainer _objNestedQuery, Dictionary<string, object> ObjectArray, ElasticClient EsClient, string IndexName, Dictionary<string, string> _columns, string[] sFilterArray) { return ObjectArray; }
        // public abstract Dictionary<string, object> fetchDropDownForMatch(Dictionary<string, object> ObjectArray, string[] sInnings);
        public abstract Dictionary<string, object> bindS1andS2Dropdown(dynamic _objS1Data);
        public abstract QueryContainer GetPlayerDetails(dynamic _objS1Data, QueryContainer qFinal, List<string> valueObj, int sportid, bool isMasterData = false);
        public abstract IEnumerable<SearchResultFilterData> returnSportResult(ElasticClient EsClient, QueryContainer _objNestedQuery, string IndexName);
        public abstract List<SearchResultFilterData> SearchResultFilterDataMap(ISearchResponse<SearchCricketData> result);
        public abstract dynamic SearchS2(QueryContainer Bq, MatchDetail _objmatch, int Sportid = 6, string search = "");
       public abstract dynamic getFinalResult(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, ElasticClient EsClient, string sportid = "1");

    }
}
