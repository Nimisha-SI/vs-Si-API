using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
   public interface ICricketS2
    {
        QueryContainer GetMatchDetailQuery(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, bool isMasterData = false);
        Dictionary<string, object> bindS1andS2Dropdown(dynamic _objS1Data);
        QueryContainer GetPlayerDetails(dynamic _objS1Data, QueryContainer qFinal, List<string> valueObj, int sportid, bool isMasterData = false);
        IEnumerable<SearchResultFilterData> returnSportResult(ElasticClient EsClient, QueryContainer _objNestedQuery, string IndexName);
        List<SearchResultFilterData> SearchResultFilterDataMap(ISearchResponse<SearchCricketData> result);
        QueryContainer GetCricketMatchSituationQuery(MatchDetail _objMatchDetail, PlayerDetail _objPlayerDetail, MatchSituation _objMatchSituation, QueryContainer _objNestedQuery);
        int GetPlayerMatchDetailsMaxCount(QueryContainer _objNestedQuery, ElasticClient EsClient, string sType);
        int getMatchCount(QueryContainer _objNestedQuery, ElasticClient EsClient, string sType);
        dynamic SearchS1(QueryContainer _objNestedQuery, ElasticClient EsClient);
        dynamic SearchS1MasterData(QueryContainer _objNestedQuery, ElasticClient EsClient);
        QueryContainer GetS2ActionQueryResult(S2ActionData _objActionData, QueryContainer _objNestedQuery, bool isMasterData = false);
        QueryContainer GetS2MomentQueryResult(Moments _objMomentData, QueryContainer _objNestedQuery, bool isMasterData = false);
        QueryContainer GetS2SearchResults(SearchS2RequestData _objReqData, QueryContainer _objNestedQuery);
        dynamic MapS2Resuldata(QueryContainer _objNested, string Search);
        dynamic SearchS2(QueryContainer _objNestedquery, MatchDetail _objmatch, int Sportid = 6, string search = "");
        dynamic getFinalResult(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, ElasticClient EsClient, string sportid = "1");
        IEnumerable<SearchCricketResultData> MapcricketS1datascopy(List<SearchCricketResultTempData> _objs1Data, MatchDetail _objmatchdetail);
        List<SearchCricketResultData> MapcricketS1data(List<SearchCricketResultTempData> _objs1Data, string cases);
    }
}
