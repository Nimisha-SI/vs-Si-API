using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;
using WebApis.BOL;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.Controllers
{
    [ApiController]
    public class SearchDataFilterController : ControllerBase
    {
        private ISearchDataFilter _sObj;
        private ExtendedSearchResultFilterData _objResult = new ExtendedSearchResultFilterData();
        //private ExtendedSearchResultFilterData _objSearchResults2;
        //private ExtendedSearchResultFilterData _objResult;
        private MatchDetail _objMatchDetail = new MatchDetail();
        public SearchDataFilterController(ISearchDataFilter sObj)
        {
            _sObj = sObj;
        }
        [System.Web.Http.HttpPost]
        [Route("api/GetSearchResultForFilters")]
        public IActionResult GetSearchResultsFilter(STFilterRequestData _objReqData)
        {
            string _objResult = "";
            try
            {
                if (_objReqData != null)
                {
                    _objResult = _sObj.GetSearchResultsFilter(_objReqData);
                }
                return Ok(new { responseText = _objResult });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [System.Web.Http.HttpPost]
        [Route("api/GetFilterBySport")]
        public IActionResult GetFilteredEntitiesBySport(SearchEntityRequestData _objReqData)
        //public IActionResult GetFilteredEntitiesBySport(dynamic _objReqData)
        {
            try
            {
                CommonFunction objCF = new CommonFunction();


               // string jsonData = JsonConvert.SerializeObject(_objReqData);
                //List<SearchEntityRequestData> _objLstReqData = new List<SearchEntityRequestData>();
                //_objLstReqData = JsonConvert.DeserializeObject<List<SearchEntityRequestData>>(jsonData);

                //string SportName = objCF.getType(_objReqData.MatchDetails.SportID);
                if (_objReqData != null)
                {
                    //if (SportName == "Cricket") {
                    //    responseResult = _sObj.GetFilteredEntitiesBySport(_objReqData);
                    //}
                    if (_objReqData.MatchDetails.SportID == 1)
                    {
                        return Ok(new { responseText = _sObj.GetFilteredEntitiesBySport(_objReqData) });
                    }
                    else if (_objReqData.MatchDetails.SportID == 3)
                    {
                        return Ok(new { responseText = _sObj.GetFilteredEntitiesBySportKabaddi(_objReqData) });
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [System.Web.Http.HttpPost]
        [Route("api/GetSearchedFilters")]
        public IActionResult GetSearchedFilters(SaveSearchesRequestData _objReqData)
        {
            try
            {
                string response = string.Empty;
                //SaveSearchesRequestData _objReqData = new SaveSearchesRequestData();
                // string jsonData = JsonConvert.SerializeObject(data);
                // _objReqData = JsonConvert.DeserializeObject<SaveSearchesRequestData>(jsonData);
                if (_objReqData != null)
                {
                    response = _sObj.GetSavedSearches(_objReqData);
                }
                return Ok(new { responseText = response });
                //return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }


        }

        [System.Web.Http.HttpPost]
        [Route("api/GetSearchResultCount")]
        //public IActionResult GetSearchResultCount(IEnumerable<SearchRequestData> _objReqData) {
        public IActionResult GetSearchResultCount(dynamic _objReqData)
        {
            try
            {
                string result = string.Empty;
                //ExtendedSearchResultFilterData _objrespData = new ExtendedSearchResultFilterData();
                // _objLstReqData.MatchDetails = new List<MatchDetail>();
                //_objLstReqData.MatchSituations = new List<MatchSituation>();
                //_objLstReqData.PlayerDetails = new List<PlayerDetail>();
                string jsonData = JsonConvert.SerializeObject(_objReqData);
                List<SearchRequestData> _objLstReqData = new List<SearchRequestData>();
                _objLstReqData = JsonConvert.DeserializeObject<List<SearchRequestData>>(jsonData);
                //_objLstReqData.MatchDetails = JsonConvert.DeserializeObject<List<MatchDetail>>(jsonData);
                //_objLstReqData.MatchSituations = JsonConvert.DeserializeObject<List<MatchSituation>>(jsonData);
                //_objLstReqData.PlayerDetails = JsonConvert.DeserializeObject<List<PlayerDetail>>(jsonData);
                if (_objLstReqData != null)
                {

                    SearchRequestData _objReqDataRes = _objLstReqData.FirstOrDefault();
                    switch (_objReqDataRes.MatchDetails.FirstOrDefault().SportID)
                    {
                        case 1:
                            result = _sObj.GetSearchResultCount(_objReqDataRes);
                            break;
                        case 3:
                            var _objLstReqDatakabaddi = JsonConvert.DeserializeObject<List<KabaddiRequestData>>(jsonData);
                            result = _sObj.GetSearchResultCountForKabaddi(_objLstReqDatakabaddi.FirstOrDefault());
                            break;
                    }

                }
                //string jsonString = JsonConvert.SerializeObject(result);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

        //[System.Web.Http.HttpPost]
        //[Route("api/GetSearchResults")]
        ////public IActionResult GetSearchResultCount(IEnumerable<SearchRequestData> _objReqData) {
        //public IActionResult GetSearchResultsForKabaddi(dynamic _objReqData)
        //{
        //    try
        //    {
        //        string result = string.Empty;
        //        string jsonData = JsonConvert.SerializeObject(_objReqData);
        //        List<SearchRequestData> _objLstReqData = new List<SearchRequestData>();
        //        _objLstReqData = JsonConvert.DeserializeObject<List<SearchRequestData>>(jsonData);
        //        if (_objLstReqData != null)
        //        {
        //            SearchRequestData _objReqDataRes = _objLstReqData.FirstOrDefault();
        //            switch (_objReqDataRes.MatchDetails.FirstOrDefault().SportID)
        //            {
        //                case 1:
        //                    result = _sObj.GetSearchResultCount(_objReqDataRes);
        //                    break;
        //                case 3:
        //                    var _objLstReqDatakabaddi = JsonConvert.DeserializeObject<List<KabaddiRequestData>>(jsonData);
        //                    result = _sObj.GetSearchResultsForKabaddi(_objLstReqDatakabaddi.FirstOrDefault());
        //                    break;
        //            }

        //        }
        //        //string jsonString = JsonConvert.SerializeObject(result);
        //        return Ok(new { result });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message.ToString());
        //    }

        //}

        [System.Web.Http.HttpPost]
        [Route("api/GetAllS2MastersBySport")]
        //  public IActionResult GetAllS2MastersBySport(IEnumerable<SearchS2RequestData> _objReqData)
        public IActionResult GetAllS2MastersBySport(dynamic _objReqData)
        {
            try
            {
                string result = string.Empty;
                List<SearchS2RequestData> _objLstReqData = new List<SearchS2RequestData>();
                string jsonData = JsonConvert.SerializeObject(_objReqData);
                _objLstReqData = JsonConvert.DeserializeObject<List<SearchS2RequestData>>(jsonData);

                if (_objLstReqData != null)
                {
                    SearchS2RequestData _objReqDataRes = _objLstReqData.FirstOrDefault();
                    result = _sObj.GetAllS2MastersBySport(_objReqDataRes);
                }
                return Ok(new { Response = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

        [System.Web.Http.HttpPost]
        [Route("api/GetMultiSelectForMatchDetail")]
        //public IActionResult GetMultiSelectForMatchDetail(IEnumerable<MatchDetailMultiSelectRequestData> _objReqData)
        public IActionResult GetMultiSelectForMatchDetail(dynamic _objReqData)
        {
            try
            {
                string result = string.Empty;
                List<MatchDetailMultiSelectRequestData> _objLstReqData = new List<MatchDetailMultiSelectRequestData>();

                string jsonData1 = JsonConvert.SerializeObject(_objReqData);
                var _objReqDatakabaddi = JsonConvert.DeserializeObject<MatchDetailMultiSelectRequestData>(jsonData1);

                //string jsonData = JsonConvert.SerializeObject(_objReqData);
                //_objLstReqData = JsonConvert.DeserializeObject<List<MatchDetailMultiSelectRequestData>>(jsonData);

                if (_objLstReqData != null)
                {
                    //MatchDetailMultiSelectRequestData _objReqDataRes = _objLstReqData.FirstOrDefault();
                    result = _sObj.GetMultiSelectForMatchDetail(_objReqDatakabaddi);
                }
                return Ok(new { Response = result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

        [System.Web.Http.HttpPost]
        [Route("api/GetS2SearchResultCount")]
        public IActionResult GetS2SearchResultCount(IEnumerable<SearchS2RequestData> _objReqData)
        {
            string result = string.Empty;
            try
            {

                List<SearchS2RequestData> _objLstReqData = new List<SearchS2RequestData>();
                string jsonData = JsonConvert.SerializeObject(_objReqData);
                _objLstReqData = JsonConvert.DeserializeObject<List<SearchS2RequestData>>(jsonData);
                if (_objLstReqData != null)
                {
                    SearchS2RequestData _objReqsearchS2 = _objLstReqData.FirstOrDefault();
                    result = _sObj.GetS2SearchResultCount(_objReqsearchS2);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
            return Ok(new { Response = result });
        }

        [System.Web.Http.HttpPost]
        [Route("api/GetMediaSearchResultsAutoComplete")]
        public IActionResult GetMediaSearchResultsAutoComplete(string sportid, string searchtext = "", string assettype = "")
        {
            string result = string.Empty;
            IEnumerable<SearchResultFilterData> searchResults = new List<SearchResultFilterData>();
            try
            {

                SearchRequestMediaData _objReqData = new SearchRequestMediaData();
                _objReqData.SportId = Convert.ToInt32(sportid);
                _objReqData.SearchText = searchtext;
                _objReqData.AssetTypeId = assettype;

                searchResults = _sObj.GetMediaSearchResult(_objReqData, 1);
            }
            catch (Exception ex)
            {

            }
            return Ok(new { Response = searchResults });
        }

        [System.Web.Http.HttpPost]
        [Route("api/GetAutoCompleteData")] //pending
        public IActionResult GetAutoCompleteData(string sportid, string stype, string term = "")
        {
            string result = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(term)) {
                    result = _sObj.GetAutoCompleteData(sportid, stype,term);
                }
            }
            catch (Exception ex)
            {

            }
            return Ok(new { Response = result });
        }

        [System.Web.Http.HttpPost]
        [Route("api/GetSearchResult")]
        public IActionResult GetSearchResult(IEnumerable<SearchRequestData> _objReqData)
        {
            try
            {
                string result = string.Empty;

                SearchRequestData _ObjReqData = new SearchRequestData();
                SearchCricketExtendedResultData _objSearchResults = new SearchCricketExtendedResultData();
                IEnumerable<SearchCricketResultData> searchResults = new List<SearchCricketResultData>();
                Dictionary<string, Int64> _objDicSearchResultCount = new Dictionary<string, Int64>();

                List<SearchRequestData> _objLstReqData = new List<SearchRequestData>();
                string jsonData = JsonConvert.SerializeObject(_objReqData);
                _objLstReqData = JsonConvert.DeserializeObject<List<SearchRequestData>>(jsonData);

                if (_objLstReqData != null)
                {
                    SearchRequestData _objReqDataRes = _objLstReqData.FirstOrDefault();
                    result = _sObj.GetSearchResults(_objReqDataRes);
                }
                //string jsonString = JsonConvert.SerializeObject(result);
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }

        [System.Web.Http.HttpPost]
        [Route("api/GetFilteredEntityBySportForS2")]
        public IActionResult GetFilteredEntityBySportForS2(IEnumerable<SearchS2RequestData> _objReqData)
        {
            string result = string.Empty;
            try
            {
                QueryContainer _objNestedquery = new QueryContainer();
                MatchDetail _objMatchDetail = null;
                List<SearchS2RequestData> _objReqSearchS2Data = new List<SearchS2RequestData>();

                string jsonData = JsonConvert.SerializeObject(_objReqData);
                _objReqSearchS2Data = JsonConvert.DeserializeObject<List<SearchS2RequestData>>(jsonData);
                if (_objReqData != null)
                {
                    SearchS2RequestData _objReqEntityS2 = _objReqData.FirstOrDefault();
                    result = _sObj.GetFilteredEntityBySportForS2(_objReqEntityS2);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
            return Ok(new { Response = result });
        }


        //[System.Web.Http.HttpPost]
        //[Route("api/GetAutoCompleteData")]
        //public IActionResult GetAutoCompleteData(string sportid, string stype, string term = "")
        //{
        //    string result = string.Empty;
        //    try
        //    {
              

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message.ToString());
        //    }
        //    return Ok(new { Response = result });
        //}

        [System.Web.Http.HttpPost]
        [Route("api/AddUpdateLuceneForSearch")]
        // public IActionResult AddUpdateForSearch(Stream data)
        public IActionResult AddUpdateForSearch(IEnumerable<SearchCricketData> data)
        {
            bool isSuccess = false;
            string RequestData = string.Empty;
            List<SearchCricketData> _objReqSearchS2Data = new List<SearchCricketData>();
            string jsonData = JsonConvert.SerializeObject(data);
            _objReqSearchS2Data = JsonConvert.DeserializeObject<List<SearchCricketData>>(jsonData);
            try
            {
                
                if (!string.IsNullOrEmpty(jsonData))
                {
                    isSuccess = _sObj.AddUpdateForSearch(jsonData, 1, false);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

            return Ok(new { Response = isSuccess });
        }

        [System.Web.Http.HttpPost]
        [Route("api/AddUpdateLuceneForKabaddiSearch")]
        public IActionResult AddUpdateAddUpdateElasticForKabaddi(IEnumerable<KabaddiS1Data> data)
        {
            bool isSuccess = false;
            string RequestData = string.Empty;
            try
            {
                
                string jsonData = JsonConvert.SerializeObject(data);
                //using (StreamReader reader = new StreamReader(data))
                //{
                //    RequestData = reader.ReadToEnd();
                //    reader.Close();
                //    reader.Dispose();
                //}KabaddiS1Data
                if (!string.IsNullOrEmpty(jsonData))
                {
                    isSuccess = _sObj.AddUpdateForSearch(jsonData, 3, false);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

            return Ok(new { Response = isSuccess });
        }

        [System.Web.Http.HttpPost]
        [Route("api/AddUpdateS2LuceneForSearch")]
        public IActionResult AddUpdateS2ElasticForSearch(IEnumerable<SearchS2Data> data)
        {
            bool isSuccess = false;
            string RequestData = string.Empty;
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);
                //using (StreamReader reader = new StreamReader(data))
                //{
                //    RequestData = reader.ReadToEnd();
                //    reader.Close();
                //    reader.Dispose();
                //}
                if (!string.IsNullOrEmpty(jsonData))
                {
                    isSuccess = _sObj.AddUpdateForSearch(jsonData, 1, true);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

            return Ok(new { Response = isSuccess });
        }

        [System.Web.Http.HttpPost]
        [Route("api/GetSearchResultCountForKabaddi")]
        public IActionResult GetSearchResultCountForKabaddi(dynamic _objReqData)
        {
            try
            {
                string result = string.Empty;
                string jsonData = JsonConvert.SerializeObject(_objReqData);
                var _objLstReqDatakabaddi = JsonConvert.DeserializeObject<List<KabaddiRequestData>>(jsonData);
                result = _sObj.GetSearchResultCountForKabaddi(_objLstReqDatakabaddi.FirstOrDefault());

                //List<SearchRequestData> _objLstReqData = new List<SearchRequestData>();
                //_objLstReqData = JsonConvert.DeserializeObject<List<SearchRequestData>>(jsonData);
                //if (_objLstReqData != null)
                //{

                //    SearchRequestData _objReqDataRes = _objLstReqData.FirstOrDefault();
                //    switch (_objReqDataRes.MatchDetails.FirstOrDefault().SportID)
                //    {
                //        case 1:
                //            result = _sObj.GetSearchResultCount(_objReqDataRes);
                //            break;
                //        case 3:
                //            var _objLstReqDatakabaddi = JsonConvert.DeserializeObject<List<KabaddiRequestData>>(jsonData);
                //            result = _sObj.GetSearchResultCountForKabaddi(_objLstReqDatakabaddi.FirstOrDefault());
                //            break;
                //    }

                //}
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [System.Web.Http.HttpPost]
        [Route("api/GetSearchResultsForKabaddi")]
        public IActionResult GetSearchResultsForKabaddi(dynamic _objReqData)
        {
            try
            {
                string result = string.Empty;
                string jsonData = JsonConvert.SerializeObject(_objReqData);
                var _objLstReqDatakabaddi = JsonConvert.DeserializeObject<List<KabaddiRequestData>>(jsonData);
                result = _sObj.GetSearchResultsForKabaddi(_objLstReqDatakabaddi.FirstOrDefault());
                return Ok(new { result });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}