using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebApis.Model.ELModels;
using System.IO;
using WebApis.Model;

namespace WebApis.BOL
{
    public interface ISearchDataFilter
    {
        //ExtendedSearchResultFilterData GetSearchResultsFilter(STFilterRequestData _objReqData);
        string GetSearchResultsFilter(STFilterRequestData _objReqData);
        //List<FilteredEntityForCricket> GetFilteredEntitiesBySport(SearchEntityRequestData _objReqData);
        string GetFilteredEntitiesBySport(SearchEntityRequestData _objReqData);
        //List<FilteredEntityKabaddi> GetFilteredEntitiesBySportKabaddi(SearchEntityRequestData _objReqData);
        string GetFilteredEntitiesBySportKabaddi(SearchEntityRequestData _objReqData);
        string GetSavedSearches(SaveSearchesRequestData objSavedSearchData);
        string GetSearchResultCount(SearchRequestData _objReqData);
        string GetAllS2MastersBySport(SearchS2RequestData _objS2ReqData);
        string GetMultiSelectForMatchDetail(MatchDetailMultiSelectRequestData _objMultiSelectResult);
        string GetS2SearchResultCount(SearchS2RequestData _objS2RequestData);
        IEnumerable<SearchResultFilterData> GetMediaSearchResult(SearchRequestMediaData _objReqData, int type);
        string GetAutoCompleteData(string sportid, string stype, string term = "");
        string GetFilteredEntityBySportForS2(SearchS2RequestData _ObjreqData);
        bool AddUpdateForSearch(string RequestData, int sportId,bool isS2= false);
        string GetSearchResults(SearchRequestData _objReqData);
        // bool AddUpdateLuceneIndexForS2(IEnumerable<SearchS2Data> sampleDatas, bool isFullIndex = true, int sportid = 1);
    }
}
