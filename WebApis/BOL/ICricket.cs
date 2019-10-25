using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
   public interface ICricket
    {
        Dictionary<string, object> getDropDownForMatch(Dictionary<string, object> ObjectArray, string[] sInnings);
        IEnumerable<ELModels.SearchResultFilterData> returnSportResult(ElasticClient EsClient, QueryContainer _objNestedQuery, string IndexName);
        List<SearchResultFilterData> SearchResultFilterDataMap(ISearchResponse<SearchCricketData> result);
        QueryContainer GetPlayerDetails(dynamic _objS1Data, QueryContainer qFinal, List<string> valueObj, int sportid, bool isMasterData = false);
        Dictionary<string, object> bindS1andS2Dropdown(dynamic _objS1Data);
        QueryContainer GetMatchDetailQuery(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, bool isMasterData = false);
        QueryContainer GetCricketMatchSituationQueryST(QueryContainer _objNestedQuery, MatchSituation _objMatchSituation);
        QueryContainer GetPlayerDetailQueryForFilteredEntityBySport(QueryContainer _objNestedQuery, dynamic _objS1Data, int SportsId = 1);
        QueryContainer GetFilteredEntitiesBySport(MatchDetail _objReqData, QueryContainer _objNestedQuery, string sCase, int sDate, Dictionary<string, string> _columns, string searchText);
        QueryContainer GetEntityBySport(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, Dictionary<string, string> _columns, string searchtext);
        List<FilteredEntityForCricket> GetFilteredEntitiesBySportResult(QueryContainer qc, string EntityId, string EntityName, ElasticClient EsClient, string searchText, int sDate = 0, int Edate = 0);
       dynamic getFinalResult(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, ElasticClient EsClient, string sportid = "1");
        //IEnumerable<SearchCricketResultData> GetSearchResults(SearchRequestData _objReqData, bool isMasterData = false, string CurrentSelector = "");
    }
}
