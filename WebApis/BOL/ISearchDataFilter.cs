using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
    public interface ISearchDataFilter
    {
        ExtendedSearchResultFilterData GetSearchResultsFilter(STFilterRequestData _objReqData);
        List<FilteredEntityForCricket> GetFilteredEntitiesBySport(SearchEntityRequestData _objReqData);
        List<FilteredEntityKabaddi> GetFilteredEntitiesBySportKabaddi(SearchEntityRequestData _objReqData);
        string GetSavedSearches(SaveSearchesRequestData objSavedSearchData);
        string GetSearchResultCount(SearchRequestData _objReqData);
        string GetAllS2MastersBySport(SearchS2RequestData _objS2ReqData);
        string GetMultiSelectForMatchDetail(MatchDetailMultiSelectRequestData _objMultiSelectResult);
        string GetS2SearchResultCount(SearchS2RequestData _objS2RequestData);
        IEnumerable<SearchResultFilterData> GetMediaSearchResult(SearchRequestMediaData _objReqData, int type);
        string GetAutoCompleteData(string sportid, string stype, string term = "");
        string GetFilteredEntityBySportForS2(SearchS2RequestData _ObjreqData);
    }
}
