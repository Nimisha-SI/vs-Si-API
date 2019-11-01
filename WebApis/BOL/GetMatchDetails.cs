using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using WebApis.DAL;
using WebApis.elastic;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
    public class GetMatchDetails : ISearchDataFilter
    {
        private ICon _con;
        private ESInterface _oLayer;
        private ICricket _cricket;
        ICricketS2 _cricketS2;
        IKabaddiS2 _kabaddiS2;
        IKabaddi _kabaddi;
        IAddUpdateIndex _addUpdateIndex;
        string SportType = "";
        //searchcricket sc = new searchcricket();
        GetSearchS1DataForCricket _searchResult = new GetSearchS1DataForCricket();
        ExtendedSearchResultFilterData _objSearchResults = new ExtendedSearchResultFilterData();
        //ExtendedSearchResultFilterData _objSearchResults;
        ExtendedSearchResultFilterData _objSearchResults2 = new ExtendedSearchResultFilterData();
        ExtendedSearchResultFilterData _objResult = new ExtendedSearchResultFilterData();
        List<SearchQueryModel> _objLstSearchQuery;
        SearchEntityRequestData _objEntityReqData;
        MatchDetail _objMatchDetail;
        PlayerDetail _objPlayerDetails;
       // Cricket objCS = new Cricket();
        CommonFunction objCF = new CommonFunction();
        MatchSituation _objMatchSituation;
        Moments _objMomentsData;
        QueryContainer _objNestedQuery = new QueryContainer();
        GetSearchS1DataForCricket sqObj = new GetSearchS1DataForCricket();
        ElasticClient EsClient_obj;
        Dictionary<string, string> _columns;

       public GetMatchDetails(ICon con, ESInterface oLayer, ICricket cricket,ICricketS2 cricketS2, IKabaddi kabaddi,IAddUpdateIndex addUpdateIndex, IKabaddiS2 kabaddiS2) {
            _con = con;
            _oLayer = oLayer;
            _cricket = cricket;
            _cricketS2 = cricketS2;
            _kabaddi = kabaddi;
            _kabaddiS2 = kabaddiS2;
            _addUpdateIndex = addUpdateIndex;
        }

        //public ExtendedSearchResultFilterData searchStoryTeller(ELModels.MatchDetail _objMatchDetail, QueryContainer _objNestedQuery, dynamic _objS1Data, Dictionary<string, object> ObjectArray, IEnumerable<SearchResultFilterData> obj, string value, string IndexName)
        //{
        //    EsClient_obj = _oLayer.CreateConnection();
        //    ExtendedSearchResultFilterData _objSearchResults = new ExtendedSearchResultFilterData();
        //    _objSearchResults.ResultData = new List<SearchResultFilterData>();
        //    _objSearchResults.Master = new MasterDatas();
        //    _objSearchResults.Master.MasterData = new Dictionary<string, object>();
        //    CommonFunction cf = new CommonFunction();
        //    //Cricket objDetails = new Cricket();
        //    //searchcricket sc = new searchcricket();
        //    // SportType = sc.getType(_objMatchDetail.SportID);
        //    //if ("CRICKET" == SportType) {
        //    //string ReqShotType = _objS1Data["ShotType"]; string ReqDeliveryType = _objS1Data["DeliveryType"];
        //    //string[] _objReqShotType = ReqShotType.Contains(",") ? _objReqShotType = ReqShotType.Split(",") : _objReqShotType = new string[] { _objS1Data["ShotType"] };
        //    //string[] _objReqDeliveryType = ReqDeliveryType.Contains(",") ? _objReqDeliveryType = ReqDeliveryType.Split(",") : _objReqDeliveryType = new string[] { _objS1Data["DeliveryType"] };
        //    //ObjectArray = GetDropdowns(_objNestedQuery, ObjectArray, EsClient, "cricket", GetColumnForEntity(13), _objReqShotType);
        //    //ObjectArray = GetDropdowns(_objNestedQuery, ObjectArray, EsClient, "cricket", GetColumnForEntity(32), _objReqDeliveryType);

        //    //_objSearchResults.Master.MasterData = ObjectArray;
        //    Dictionary<string, object> ddlDropdowns = new Dictionary<string, object>();
        //    //GetMatchDetails _objMatchDetails = new GetMatchDetails();
        //    ddlDropdowns = _cricket.bindS1andS2Dropdown(_objS1Data);
        //    if (value != null)
        //    {
        //        string[] valuess = value.Split(",");
        //        foreach (var items in valuess)
        //        {
        //            var item = items.Split("::");
        //            string Type = _objS1Data[item[0]];
        //            string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
        //            foreach (KeyValuePair<string, object> entry in ddlDropdowns)
        //            {
        //                // if (_objType.ToString() != "") {
        //                if (item.ToString().Split(",")[0] != entry.Key.ToString())
        //                {
        //                    if (entry.Value.ToString() != "")
        //                    {
        //                        QueryContainer query = new TermQuery { Field = entry.Key, Value = entry.Value };
        //                        _objNestedQuery &= query;
        //                    }

        //                }
        //                //}


        //            }

        //            _objSearchResults.Master.MasterData = cf.GetDropdowns(_objNestedQuery, _objSearchResults.Master.MasterData, EsClient_obj, IndexName, cf.GetColumnForEntity(Convert.ToInt16(item[1])), _objType);
        //        }
        //    }

        //    obj = cf.returnSportResult(EsClient_obj, _objNestedQuery, IndexName);
        //    _objSearchResults.ResultData = obj;
        //    return _objSearchResults;
        //    //throw new NotImplementedException();
        //}

        public string GetSearchResultsFilter(STFilterRequestData _objReqData)
        //public ExtendedSearchResultFilterData GetSearchResultsFilter(STFilterRequestData _objReqData)
        {
            string jsonDataresult = "";
            if (_objReqData != null)
            {
                
                dynamic _objS1Data = _objReqData.S1Data;
                _objResult.ResultData = new List<SearchResultFilterData>();
                _objSearchResults2.ResultData = new List<SearchResultFilterData>();
                _objSearchResults.ResultData = new List<SearchResultFilterData>();
                _objResult.Master = new MasterDatas();
                _objResult.Master.MasterData = new Dictionary<string, object>();
                _objSearchResults.Master = new MasterDatas();
                _objSearchResults.Master.MasterData = new Dictionary<string, object>();
                _objMatchDetail = _objReqData.MatchDetail;
                _objMatchSituation = _objReqData.MatchSituation;
                _objMomentsData = _objReqData.Moments;
                string value = _con.GetKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID), "DropdwonKey");
                List<string> valueObj = _con.LstKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID).ToLower(), "PlayerDetails");
                string SportName = objCF.getType(_objMatchDetail.SportID);
                if (_objS1Data != null)
                {
                    _objNestedQuery = getDetailsAsPerSport(_objS1Data, _objNestedQuery, _objMatchDetail, _objMatchSituation, valueObj, _objMatchDetail.SportID);
                    _objSearchResults = searchStoryTeller(_objMatchDetail, _objNestedQuery, _objS1Data, _objResult.Master.MasterData, _objResult.ResultData, value, SportName.ToLower());
                }

                if (_objMomentsData != null)
                {
                    QueryContainer objMoment = new QueryContainer();
                    objMoment = objCF.GetMomentDetailsQueryST(_objMatchDetail, objMoment, _objMomentsData);
                    if (_objMatchDetail.SportID == 1)
                    { }
                    // _objSearchResults2.ResultData = _cricket.returnSportResult(EsClient_obj, objMoment, SportName);
                    else if (_objMatchDetail.SportID == 3)
                    {
                        //_objSearchResults2.ResultData = _kabaddi.returnSportResult(EsClient_obj, objMoment, SportName);
                        _objSearchResults2.ResultData = _objSearchResults2.ResultData.ToList().GroupBy(t => t.Id, (key, group) => group.First());
                    }
                }

                _objResult.ResultData = _objSearchResults.ResultData.Union(_objSearchResults2.ResultData);
                _objResult.Master = _objSearchResults.Master;

                if (_objMatchDetail.SportID == 1)
                {
                    string[] _objReqInnings = _objMatchSituation.Innings.Contains(",") ? _objReqInnings = _objMatchSituation.Innings.Split(',') : _objReqInnings = new string[] { _objMatchSituation.Innings };
                    var innings = _cricket.getDropDownForMatch(_objResult.Master.MasterData, _objReqInnings);
                    //_objResult.Master.MasterData.Add("Innings", innings);
                }
                jsonDataresult = JsonConvert.SerializeObject(_objResult);
            }
            return jsonDataresult;
        }
        //List<FilteredEntityForCricket>
        public string GetFilteredEntitiesBySport(SearchEntityRequestData _objReqData)
        {
            string jsonDataresult = "";
            var responseResult = new List<FilteredEntityForCricket>();
            string searchtext = string.Empty;
            if (_objReqData != null) {
                EsClient_obj = _oLayer.CreateConnection();
                _objLstSearchQuery = new List<SearchQueryModel>();
                string jsonData = JsonConvert.SerializeObject(_objReqData);
                _objEntityReqData = JsonConvert.DeserializeObject<SearchEntityRequestData>(jsonData);
                searchtext = _objEntityReqData.EntityText.Trim().ToLower();
                _objMatchDetail.SeriesId = _objEntityReqData.EntityTypeId != 5 ? _objMatchDetail.SeriesId : string.Empty;
                _objMatchDetail.MatchId = _objEntityReqData.EntityTypeId != 9 ? _objMatchDetail.MatchId : string.Empty;
                if (_objEntityReqData != null)
                {
                    _columns = objCF.GetColumnForEntity(_objEntityReqData.EntityTypeId);
                    IEnumerable<FilteredEntityForCricket> _objFilteredEntityForCricket = new List<FilteredEntityForCricket>();
                    _objNestedQuery = _cricketS2.GetMatchDetailQuery(_objNestedQuery, _objMatchDetail);
                    _objNestedQuery = _cricket.GetPlayerDetailQueryForFilteredEntityBySport(_objNestedQuery, _objEntityReqData.playerDetails, _objMatchDetail.SportID);//GetCricketPlayerDetailQuery(_objEntityReqData.playerDetails, _objNestedBoolQuery);
                    if (!string.IsNullOrEmpty(_objMatchDetail.MatchDate))
                    {
                        _objNestedQuery = _cricket.GetEntityBySport(_objNestedQuery, _objMatchDetail, _columns, searchtext);
                    }
                    if (_columns.Count > 0)
                    {
                        List<string> EntityIds = new List<string>();
                        List<string> EntityNames = new List<string>();
                        foreach (var col in _columns)
                        {
                            EntityIds.Add(col.Key);
                            EntityNames.Add(col.Value);

                        }

                        if (_objEntityReqData.EntityTypeId == 5)
                        {
                            var s1 = _cricket.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(1), EntityNames.ElementAt(1), EsClient_obj, searchtext);
                            var s1Result = s1.Select(a => a.ParentSeriesId ="1");
                            var s2 = _cricket.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(0), EntityNames.ElementAt(0), EsClient_obj, searchtext);
                            var s2Result = s1.Select(a => a.ParentSeriesId = "0");
                            var s = s1Result.Union(s2Result);
                            //result = _objSerializer.Serialize(s);
                            //responseResult = s;
                        }
                        if (_objEntityReqData.EntityTypeId == 3 || _objEntityReqData.EntityTypeId == 4)
                        {
                            //var s1 = _objFilteredEntityForCricket.Select(t => new { EntityId = t.Team1Id, EntityName = t.Team1 }).Distinct();
                            //var s2 = _objFilteredEntityForCricket.Select(t => new { EntityId = t.Team2Id, EntityName = t.Team2 }).Distinct();
                            //var s = s1.Union(s2).Where(r => r.EntityName.ToString() != "" && r.EntityName.ToLower().Contains(searchtext)).Distinct().OrderBy(r => r.EntityName);
                            ////result = _objSerializer.Serialize(s);

                            var s1 = _cricket.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(1), EntityNames.ElementAt(1), EsClient_obj, searchtext);
                            var s2 = _cricket.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(0), EntityNames.ElementAt(0), EsClient_obj, searchtext);
                            var s = s1.Union(s2);
                        }
                        if (searchtext != "")
                        {
                            var Result = _cricket.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(0), EntityNames.ElementAt(0), EsClient_obj, searchtext);
                            responseResult = Result;
                        }

                    }
                }

            }
            jsonDataresult = JsonConvert.SerializeObject(responseResult);
            return jsonDataresult;
        }

        public string GetSavedSearches(SaveSearchesRequestData objSavedSearchData)
        {
            string response = string.Empty;
            string conString = _con.GetKeyValueAppSetting("ConnectionStrings", "DefaultConnection");
            response = _searchResult.GetSavedSearches(conString, objSavedSearchData);
            return response;
        }

        public string GetSearchResultCount(SearchRequestData _objReqData)
        {
            string result = string.Empty;
            bool checkIfCascade = true;
            string strSeacrchType = "";
            try
            {


                Dictionary<string, object> ddlDropdowns = new Dictionary<string, object>();
                SearchResultsExtendedData _objSearchResults = new SearchResultsExtendedData();
                string response = string.Empty;
                int sportid = 1;
                string languageid = "1";
                _objSearchResults.ResultDerivedData = new SearchResultsDerivedData();
                _objSearchResults.ResultDerivedData.RangeData = new Dictionary<string, long>();
                QueryContainer _objNestedQuery = new QueryContainer();
                QueryContainer _objNestedDropdownQuery = new QueryContainer();
                _objMatchDetail = _objReqData.MatchDetails.FirstOrDefault();
                _objPlayerDetails = _objReqData.PlayerDetails.FirstOrDefault();
                string objDropdown = _con.GetKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID), "S2Dropdowns");
                dynamic _objS2Data = _objPlayerDetails;
                _objSearchResults.ResultCount = new Dictionary<string, Int64>();
                _objSearchResults.ResultData = new List<KabaddiResultData>();

                MatchSituation _objMatchSituation = _objReqData.MatchSituations.FirstOrDefault();
                List<string> valueObj = _con.LstKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID).ToLower() + "S2", "PlayerDetails");
                string S2DataObj1 = _con.GetKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID).ToLower(), "S2DataCount");
                string S2DataObj2 = _con.GetKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID).ToLower(), "S2DataMaxCount");
                if (_objMatchDetail != null)
                {
                    string input = "";
                    EsClient_obj = _oLayer.CreateConnection();
                    sportid = _objMatchDetail.SportID;
                    languageid = _objMatchDetail.LanguageId;
                    input = Convert.ToInt32(Convert.ToBoolean(_objMatchDetail.IsAsset)).ToString();
                    _objNestedQuery = _cricketS2.GetMatchDetailQuery(_objNestedQuery, _objMatchDetail);
                    if (_objPlayerDetails != null)
                    {
                        _objNestedQuery = _cricketS2.GetPlayerDetails(_objS2Data, _objNestedQuery, valueObj, sportid);//new
                        if (_objMatchSituation != null)
                        {
                            _objNestedQuery = _cricketS2.GetCricketMatchSituationQuery(_objMatchDetail, _objPlayerDetails, _objMatchSituation, _objNestedQuery);//new
                        }
                    }
                    string[] arrayS2Count = S2DataObj1.Split(",");
                    string[] arrayS2Max = S2DataObj2.Split(",");
                    foreach (var items in arrayS2Count)
                    {
                        _objSearchResults.ResultCount.Add(items, _cricketS2.getMatchCount(_objNestedQuery, EsClient_obj, items));
                    }
                    foreach (var items in arrayS2Max)
                    {
                        _objSearchResults.ResultDerivedData.RangeData.Add(items, _cricketS2.GetPlayerMatchDetailsMaxCount(_objNestedQuery, EsClient_obj, items));
                    }
                    _objSearchResults.ResultCount.Add("RequestCount", _objReqData.MatchDetails[0].RequestCount);
                    if (!string.IsNullOrEmpty(_objMatchDetail.MatchDate) || _objMatchSituation != null)
                    {
                        _objNestedQuery = FinalSearchCricketData(_objMatchDetail, _objMatchSituation, _objNestedQuery);
                    }
                    QueryContainer qcIstagged = new TermQuery { Field = "isAsset", Value = input };
                    _objNestedQuery &= qcIstagged;
                    IEnumerable<SearchCricketResultTempData> searchResults = new List<SearchCricketResultTempData>();
                    IEnumerable<SearchCricketResultData> _objresult = new List<SearchCricketResultData>();
                    searchResults = _cricketS2.SearchS1(_objNestedQuery, EsClient_obj);
                    //_ob_objresultjresult = 
                    var searchresult = _cricketS2.MapcricketS1datascopy(searchResults.ToList(), _objMatchDetail);
                    _objSearchResults.ResultData = searchresult != null ? searchresult : null;
                    //_objSearchResults.ResultData = _objresult != null ? _objresult : null;
                    //int matchescount = _objSearchResults.ResultData.Select(s => s.MatchId).Distinct().Count();
                    //int assetcount = _objSearchResults.ResultData.Where(x => x.IsAsset == "1").Count();
                    //Int64 videoscount = _objSearchResults.ResultData.Where(x => x.IsAsset == "0").Count();

                    //_objSearchResults.ResultCount.Add("Matches", matchescount);
                    //_objSearchResults.ResultCount.Add("Videos", videoscount);
                    //_objSearchResults.ResultCount.Add("Assets", assetcount);


                    if (_objReqData.PlayerDetails != null && _objPlayerDetails.IsDefault)
                    {

                        MatchSituation _objMatchSituations = _objReqData.MatchSituations.FirstOrDefault();

                        if (!_objMatchDetail.IsAsset && _objMatchDetail.MasterData)
                        {
                            _objSearchResults.ResultDerivedData.MasterData = new Dictionary<string, object>();

                            if (_objSearchResults.ResultData.ToList().Count > 0)
                            {
                                ddlDropdowns = _cricketS2.bindS1andS2Dropdown(_objPlayerDetails);

                                if (objDropdown != null)
                                {
                                    string[] valuess = objDropdown.Split(",");

                                    //foreach (var items in valuess)
                                    //{
                                    //    var item = items.Split("::");

                                    //    foreach (KeyValuePair<string, object> entry in ddlDropdowns)
                                    //    {
                                    //        if (item.ToString().Split(",")[0] != entry.Key.ToString())
                                    //        {
                                    //            QueryContainer query = new TermQuery { Field = entry.Key, Value = entry.Value };
                                    //            _objNestedDropdownQuery &= query;
                                    //        }

                                    //    }

                                    //    var Type = ddlDropdowns[item.ToString().Split(",")[0]].ToString();
                                    //    string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
                                    //    _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "cricket", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType);
                                    //}
                                    foreach (var items in valuess)
                                    {
                                        var item = items.Split("::");
                                        foreach (KeyValuePair<string, object> itemss in ddlDropdowns)
                                        {
                                            string strValue = objCF.ConvertStringArrayToString(item);
                                            var Type = strValue.Split(",")[0].ToString().ToString();
                                            if (Type != itemss.Key.ToString())
                                            {

                                                string slist = itemss.Value.ToString();
                                                if (slist.Contains(","))
                                                {
                                                    string[] strValues = slist.Split(',');
                                                    foreach (string str in strValues)
                                                    {
                                                        QueryContainer query1 = new TermQuery { Field = itemss.Key, Value = str };
                                                        _objNestedDropdownQuery &= query1;
                                                    }
                                                }
                                                else
                                                {
                                                    QueryContainer query = new TermQuery { Field = itemss.Key, Value = itemss.Value };
                                                    _objNestedDropdownQuery &= query;
                                                }

                                            }
                                            else
                                            {
                                                checkIfCascade = false;
                                            }

                                        }
                                        if (!checkIfCascade)
                                        {
                                            string[] itemsArray = item;
                                            string strValue = objCF.ConvertStringArrayToString(itemsArray);
                                            var Type = ddlDropdowns[strValue.Split(",")[0].ToString()].ToString();
                                            string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
                                            _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "cricket", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType,_objMatchDetail.SportID);

                                        }
                                        else
                                        {
                                            //var Type = ddlDropdowns[item.ToString().Split(",")[0]].ToString();
                                            //string[] _objType = new string[] { "0" };
                                            string[] _objType = new string[] { "" };
                                            _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "cricket", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType, _objMatchDetail.SportID);

                                        }
                                        checkIfCascade = true;
                                    }
                                }



                            }
                        }
                    }
                    else if (_objReqData.PlayerDetails != null && !_objPlayerDetails.IsDefault)
                    {
                        MatchSituation _objMatchSituations = _objReqData.MatchSituations.FirstOrDefault();
                        if (!_objMatchDetail.IsAsset)
                        {
                            _objSearchResults.ResultDerivedData.MasterData = new Dictionary<string, object>();
                            string currentselector = _objPlayerDetails.CurrentSelector;
                            _objNestedQuery = null;
                            _objNestedQuery = _cricketS2.GetMatchDetailQuery(_objNestedQuery, _objMatchDetail, true);//new 
                            _objNestedQuery = _cricketS2.GetPlayerDetails(_objS2Data, _objNestedQuery, valueObj, sportid, true);
                            _objNestedQuery = _cricketS2.GetCricketMatchSituationQuery(_objMatchDetail, _objPlayerDetails, _objMatchSituation, _objNestedQuery);//new
                            IEnumerable<SearchS1CricketMasterData> _objSearchS1MasterData = new List<SearchS1CricketMasterData>();
                            _objSearchS1MasterData = _cricketS2.SearchS1MasterData(_objNestedQuery, EsClient_obj);

                            ddlDropdowns = _cricketS2.bindS1andS2Dropdown(_objPlayerDetails);

                            if (objDropdown != null)
                            {
                                string[] valuess = objDropdown.Split(",");

                                foreach (var items in valuess)
                                {
                                    var item = items.Split("::");
                                    foreach (KeyValuePair<string, object> itemss in ddlDropdowns)
                                    {
                                        string strValue = objCF.ConvertStringArrayToString(item);
                                        var Type = strValue.Split(",")[0].ToString().ToString();
                                        if (Type != itemss.Key.ToString())
                                        {

                                            string slist = itemss.Value.ToString();
                                            if (slist.Contains(","))
                                            {
                                                string[] strValues = slist.Split(',');
                                                foreach (string str in strValues)
                                                {
                                                    QueryContainer query1 = new TermQuery { Field = itemss.Key, Value = str };
                                                    // _objNestedDropdownQuery &= query1;
                                                    _objNestedQuery &= query1;
                                                }
                                            }
                                            else
                                            {
                                                QueryContainer query = new TermQuery { Field = itemss.Key, Value = itemss.Value };
                                                //_objNestedDropdownQuery &= query;
                                                _objNestedQuery &= query;
                                            }

                                        }
                                        else
                                        {
                                            checkIfCascade = false;
                                        }

                                    }


                                    if (!checkIfCascade)
                                    {
                                        string[] itemsArray = item;
                                        string strValue = objCF.ConvertStringArrayToString(itemsArray);
                                        var Type = ddlDropdowns[strValue.Split(",")[0].ToString()].ToString();
                                        string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
                                        _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "cricket", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType, _objMatchDetail.SportID);

                                    }
                                    else
                                    {
                                        //var Type = ddlDropdowns[item.ToString().Split(",")[0]].ToString();
                                        //string[] _objType = new string[] { "0" };

                                        string[] _objType = new string[] { "" };
                                        _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "cricket", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType, _objMatchDetail.SportID);

                                    }
                                    checkIfCascade = true;
                                }
                            }



                        }
                    }

                    //_objSearchResults.ResultData = null;
                    string jsonData = JsonConvert.SerializeObject(_objSearchResults);
                    result = jsonData;
                    //string JSONstring = JsonConvert.SerializeObject<List<SearchRequestData>>(_objSearchResults);
                    //result = _objSerializer.Serialize(_objSearchResults);
                    //_objSerializer = null;
                    //_objSearchResults.ResultCount = null;
                    //_objSearchResults.ResultDerivedData = null;


                }

            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public string GetAllS2MastersBySport(SearchS2RequestData _objS2ReqData) {
            string response = string.Empty;
            try
            {
                List<S2MasterData> _objLstS2MasterData = new List<S2MasterData>();
                SearchS2RequestData _objReqData = new SearchS2RequestData();
                List<SearchQueryModel> _objLstSearchQuery = new List<SearchQueryModel>();
                MatchDetail _objMatchDetail = null;
                List<SearchS2RequestData> _objLstReqData;
                QueryContainer _objNestedQuery = new QueryContainer();
                if (_objS2ReqData != null) {
                    _objMatchDetail = _objS2ReqData.MatchDetails.FirstOrDefault();
                    _objMatchDetail.IsAsset = false;
                    _objNestedQuery = _cricketS2.GetMatchDetailQuery(_objNestedQuery,_objMatchDetail);
                    S2ActionData _objActionData = _objS2ReqData.ActionData.FirstOrDefault();
                    Moments _objMomentData = _objS2ReqData.Moments.FirstOrDefault();
                    if (_objActionData != null)
                    {
                        _objNestedQuery = _cricketS2.GetS2ActionQueryResult(_objActionData, _objNestedQuery);

                    }
                    if (_objMomentData != null)
                    {

                        _objNestedQuery = _cricketS2.GetS2MomentQueryResult(_objMomentData, _objNestedQuery);
                    }
                    _objLstS2MasterData = _cricketS2.getFinalResult(_objNestedQuery, _objMatchDetail, _oLayer.CreateConnection(),  "1");
                    string JsonString = JsonConvert.SerializeObject(_objLstS2MasterData);
                    response=JsonString;
                }
            }
            catch (Exception ex) {
                response = ex.Message.ToString();
            }
            return response;
        }

        public string GetMultiSelectForMatchDetail(MatchDetailMultiSelectRequestData _objMultiSelectResult)
        {
            string result = string.Empty;
            QueryContainer _objNestedQuery = new QueryContainer();
            try
            {
                Dictionary<string, object> _ObjMasterData = new Dictionary<string, object>();
                IEnumerable<MatchDetailMultiSelectResulttData> _objResultData = new List<MatchDetailMultiSelectResulttData>();
                List<FilteredEntityData> _objResult;
                MatchDetailMultiSelectRequestData _objReqData;
                if (_objMultiSelectResult != null)
                {

                    MatchDetail _objmatchdetail = _objMultiSelectResult.MatchDetails.FirstOrDefault();
                    _objResult = _cricket.getFinalResult(_objNestedQuery, _objmatchdetail, _oLayer.CreateConnection());
                    var compResult = _objResult.OrderBy(t => t.EntityName).ToList();
                    _ObjMasterData.Add("CompType", compResult);
                    string JsonString = JsonConvert.SerializeObject(_ObjMasterData);
                    result = JsonString;
                }

                }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return result;
        }

        public string GetS2SearchResultCount(SearchS2RequestData _objS2RequestData) {
            string result = string.Empty;
            try
            {
                QueryContainer _objNestedQuery = new QueryContainer();
                SearchS2RequestData _objReqData = new SearchS2RequestData();
                List<SearchS2RequestData> _objLstReqData;
                Dictionary<string, Int64> _objDicSearchResultCount = new Dictionary<string, Int64>();
                if (_objS2RequestData != null)
                {
                    MatchDetail _objMatchDetail = _objS2RequestData.MatchDetails.FirstOrDefault();
                    IEnumerable<SearchS2ResultData> searchResults = new List<SearchS2ResultData>();
                    _objNestedQuery = _cricketS2.GetS2SearchResults(_objS2RequestData, _objNestedQuery);
                    string S2DataObj1 = _con.GetKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID).ToLower(), "S2DataCount");
                    string[] arrayS2Count = S2DataObj1.Split(",");
                    foreach (var items in arrayS2Count)
                    {
                        _objDicSearchResultCount.Add(items, _cricketS2.getMatchCount(_objNestedQuery, _oLayer.CreateConnection(), items));
                    }
                    _objDicSearchResultCount.Add("RequestCount", _objS2RequestData.MatchDetails[0].RequestCount);
                    string JsonString = JsonConvert.SerializeObject(_objDicSearchResultCount);
                    result = JsonString;
                }
            }
            catch (Exception ex) {
                result = ex.Message.ToString();
            }
            return result;
            }

        public IEnumerable<SearchResultFilterData> GetMediaSearchResult(SearchRequestMediaData _objReqData, int type) {
            IEnumerable<SearchResultFilterData> searchResults = new List<SearchResultFilterData>();
            List<SearchQueryModel> _objLstSearchQuery = new List<SearchQueryModel>();
            try
              {
                QueryContainer _objNestedQuery = new QueryContainer();
                QueryContainer _objShould = new QueryContainer();
               

                if (!string.IsNullOrEmpty(_objReqData.SearchText))
                {
                    int sportid = _objReqData.SportId;
                    QueryContainer q1 = new TermQuery { Field = "isAsset", Value = "1" };
                    _objNestedQuery &= q1;
                    
                    if (!string.IsNullOrEmpty(_objReqData.AssetTypeId))
                    {
                        QueryContainer q2 = new TermQuery { Field = "AssetTypeId", Value = _objReqData.AssetTypeId };
                        _objNestedQuery &= q2;
                    }
                    if (type == 2)
                    {

                        string[] searchTextArr = null;
                        if (_objReqData.SearchText.Trim().Contains(":"))
                        {
                            searchTextArr = _objReqData.SearchText.Trim().Split(':');
                            QueryContainer q3 = new TermQuery { Field = "mediaId", Value = searchTextArr[0].Trim().ToLower() };
                            _objNestedQuery &= q3;
                            QueryContainer q4 = new TermQuery { Field = "qDescription", Value = searchTextArr[1].Trim().Replace(" ", string.Empty).ToLower() };
                            _objNestedQuery &= q4;
                        }
                        else
                        {
                            QueryContainer q5 = new TermQuery { Field = "qClearId", Value = _objReqData.SearchText.Trim().Replace("-", string.Empty).ToLower() };
                            _objNestedQuery &= q5;
                        }

                    }
                    else
                    {
                        string[] strnum = _objReqData.SearchText.Split('-');
                       
                        QueryContainer q6 = new TermQuery { Field = "displayTitle", Value = _objReqData.SearchText.Trim() };
                        _objNestedQuery &= q6;
                        QueryContainer q7 = new TermQuery { Field = "qClearId", Value = _objReqData.SearchText.Trim().Replace("-", string.Empty).ToLower() };
                        _objNestedQuery &= q7;
                        QueryContainer q8 = new TermQuery { Field = "mediaId", Value = strnum[0].ToLower() };
                        _objNestedQuery &= q8;
                        _objShould |= q6 |= q7 |= q8;

                    }
                    _objNestedQuery &= _objShould;
                    
                }
                searchResults = getFinalResult(_objNestedQuery, _objMatchDetail, _oLayer.CreateConnection());
            }
         
            catch (Exception EX) {

            }
            return searchResults;

        }

        public dynamic getFinalResult(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, ElasticClient EsClient, string sportid = "1") {
            dynamic result;
            var resultMediaSearch = EsClient.Search<MatchDetailMultiSelectResulttData>(s => s.Index("crickets").Query(q => _objNestedQuery)
          .Aggregations(a => a.Terms("agg_MediaSearch", t => t.Field(p => p.CompType)))
          .Size(409846));
            var response = resultMediaSearch.Aggregations.Terms("agg_MediaSearch");
            return result= response;
        }

        public string GetFilteredEntityBySportForS2(SearchS2RequestData _ObjreqData) {
            string result = string.Empty;
            try {
                QueryContainer _objNestedQuery = new QueryContainer();
                S2ActionData _objActionData = _ObjreqData.ActionData.FirstOrDefault();
                Moments _objMomentData = _ObjreqData.Moments.FirstOrDefault();
                if (_ObjreqData != null)
                {
                    _objMatchDetail = _ObjreqData.MatchDetails.FirstOrDefault();
                    _objMatchDetail.IsAsset = false;
                    _objNestedQuery = _cricketS2.GetMatchDetailQuery(_objNestedQuery, _objMatchDetail);
                    if (_objActionData != null)
                    {

                        _objNestedQuery = _cricketS2.GetS2ActionQueryResult(_objActionData, _objNestedQuery);

                    }
                    if (_objMomentData != null)
                    {

                        _objNestedQuery = _cricketS2.GetS2MomentQueryResult(_objMomentData, _objNestedQuery);
                    }
                    IEnumerable<S2FilteredEntity> Search = _cricketS2.SearchS2(_objNestedQuery, _objMatchDetail, _objMatchDetail.SportID, _ObjreqData.EntityText);

                }
            }
            catch (Exception ex) {
            }
            return result;
        }

        //public Dictionary<string,object> bindS2Dropdown(PlayerDetail _objPlayerDetail) {
        //    Dictionary<string, object> ddlS2Dropwons = new Dictionary<string, object>();
        //    string[] _objReqShotTypes = _objPlayerDetail.ShotType.Contains(",") ? _objReqShotTypes = _objPlayerDetail.ShotType.Split(',') : _objReqShotTypes = new string[] { _objPlayerDetail.ShotType };
        //    string[] _objReqShotZones = _objPlayerDetail.ShotZone.Contains(",") ? _objReqShotZones = _objPlayerDetail.ShotZone.Split(',') : _objReqShotZones = new string[] { _objPlayerDetail.ShotZone };
        //    string[] _objReqDismissalTypes = _objPlayerDetail.DismissalType.Contains(",") ? _objReqDismissalTypes = _objPlayerDetail.DismissalType.Split(',') : _objReqDismissalTypes = new string[] { _objPlayerDetail.DismissalType };
        //    string[] _objReqDeliveryType = _objPlayerDetail.DeliveryType.Contains(",") ? _objReqDeliveryType = _objPlayerDetail.DeliveryType.Split(',') : _objReqDeliveryType = new string[] { _objPlayerDetail.DeliveryType };
        //    string[] _objReqBowlingLength = _objPlayerDetail.BowlingLength.Contains(",") ? _objReqBowlingLength = _objPlayerDetail.BowlingLength.Split(',') : _objReqBowlingLength = new string[] { _objPlayerDetail.BowlingLength };
        //    string[] _objReqBowlingLine = _objPlayerDetail.BowlingLine.Contains(",") ? _objReqBowlingLine = _objPlayerDetail.BowlingLine.Split(',') : _objReqBowlingLine = new string[] { _objPlayerDetail.BowlingLine };
        //    string[] _objReqFielderPosition = _objPlayerDetail.FieldingPosition.Contains(",") ? _objReqFielderPosition = _objPlayerDetail.FieldingPosition.Split(',') : _objReqFielderPosition = new string[] { _objPlayerDetail.FieldingPosition };
        //    string[] _objReqBattingOrder = _objPlayerDetail.BattingOrder.Contains(",") ? _objReqBattingOrder = _objPlayerDetail.BattingOrder.Split(',') : _objReqBattingOrder = new string[] { _objPlayerDetail.BattingOrder };
        //    string[] _objReqBowlingArm = _objPlayerDetail.BowlingArm.Contains(",") ? _objReqBowlingArm = _objPlayerDetail.BowlingArm.Split(',') : _objReqBowlingArm = new string[] { _objPlayerDetail.BowlingArm };

        //    if (_objPlayerDetail.ShotZone != "" && _objReqShotZones.Length > 0) {
        //        ddlS2Dropwons.Add("shotZoneId", _objReqShotZones);
        //    }
        //    if (_objPlayerDetail.ShotType != "" && _objReqShotTypes.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("shotTypeId", _objReqShotTypes);
        //    }
        //    if (_objPlayerDetail.DismissalType != "" && _objReqDismissalTypes.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("dismissalTypeId", _objReqDismissalTypes);
        //    }
        //    if (_objPlayerDetail.DeliveryType != "" && _objReqDeliveryType.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("deliveryTypeId", _objReqDeliveryType);
        //    }
        //    if (_objPlayerDetail.BowlingLength != "" && _objReqBowlingLength.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("bowlingLengthId", _objReqBowlingLength);
        //    }
        //    if (_objPlayerDetail.BowlingLine != "" && _objReqBowlingLine.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("bowlingLineId", _objReqBowlingLine);
        //    }
        //    if (_objPlayerDetail.FieldingPosition != "" && _objReqFielderPosition.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("fieldingPositionId", _objReqFielderPosition);
        //    }
        //    if (_objPlayerDetail.BattingOrder != "" && _objReqBattingOrder.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("battingOrder", _objReqBattingOrder);
        //    }
        //    if (_objPlayerDetail.BowlingArm != "" && _objReqBowlingArm.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("bowlingArm", _objReqBowlingArm);
        //    }
        //    return ddlS2Dropwons;
        //}


        //public Dictionary<string, object> bindS1andS2Dropdown(dynamic _objS1Data) {
        //    Dictionary<string, object> ddlS1Dropwons = new Dictionary<string, object>();
        //    string ReqShotType = _objS1Data["ShotType"]; string ReqDeliveryType = _objS1Data["DeliveryType"];
        //    string[] _objReqShotType = ReqShotType.Contains(",") ? _objReqShotType = ReqShotType.Split(",") : _objReqShotType = new string[] { _objS1Data["ShotType"] };
        //    string[] _objReqDeliveryType = ReqDeliveryType.Contains(",") ? _objReqDeliveryType = ReqDeliveryType.Split(",") : _objReqDeliveryType = new string[] { _objS1Data["DeliveryType"] };

        //    if (_objS1Data["ShotType"] != "")
        //    {
        //        ddlS1Dropwons.Add("shotTypeId", _objReqShotType[0].ToString());
        //    }
        //    if (_objS1Data["DeliveryType"] != "")
        //    {
        //        ddlS1Dropwons.Add("deliveryTypeId", _objReqDeliveryType[0].ToString());
        //    }
        //    return ddlS1Dropwons;

        //}

        private QueryContainer FinalSearchCricketData(MatchDetail _objMatchDetail, MatchSituation _objMatchSituation,QueryContainer _objNested)
        {
            QueryContainer qRangeQuery = new QueryContainer();
            if (_objMatchSituation != null)
            {
                if (!string.IsNullOrEmpty(_objMatchSituation.BatsmanRunsRange))
                {
                    string dlist = _objMatchSituation.BatsmanRunsRange;
                    string[] strNumbers = dlist.Split('-');
                    int start = int.Parse(strNumbers[0]);
                    int End = int.Parse(strNumbers[1]);
                    qRangeQuery = new TermRangeQuery { Field = "batsmanRunsRange", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                }
                if (!string.IsNullOrEmpty(_objMatchSituation.BatsmanBallsFacedRange))
                {
                    string dlist = _objMatchSituation.BatsmanBallsFacedRange;
                    string[] strNumbers = dlist.Split('-');
                    int start = int.Parse(strNumbers[0]);
                    int End = int.Parse(strNumbers[1]);
                    qRangeQuery = new TermRangeQuery { Field = "batsmanBallsFacedRange", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                }
                if (!string.IsNullOrEmpty(_objMatchSituation.BowlerBallsBowledRange))
                {

                    string dlist = _objMatchSituation.BowlerBallsBowledRange;
                    string[] strNumbers = dlist.Split('-');
                    int start = int.Parse(strNumbers[0]);
                    int End = int.Parse(strNumbers[1]);
                    qRangeQuery = new TermRangeQuery { Field = "bowlerBallsBowledRange", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                }
                if (!string.IsNullOrEmpty(_objMatchSituation.BowlerWicketsRange))
                {

                    string dlist = _objMatchSituation.BowlerWicketsRange;
                    string[] strNumbers = dlist.Split('-');
                    int start = int.Parse(strNumbers[0]);
                    int End = int.Parse(strNumbers[1]);
                    qRangeQuery = new TermRangeQuery { Field = "bowlerWicketsRange", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                }
                if (!string.IsNullOrEmpty(_objMatchSituation.BowlerRunsRange))
                {

                    string dlist = _objMatchSituation.BowlerRunsRange;
                    string[] strNumbers = dlist.Split('-');
                    int start = int.Parse(strNumbers[0]);
                    int End = int.Parse(strNumbers[1]);
                    qRangeQuery = new TermRangeQuery { Field = "bowlerRunsRange", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                }
                if (!string.IsNullOrEmpty(_objMatchSituation.TeamOversRange))
                {
                    string dlist = _objMatchSituation.TeamOversRange;
                    string[] strNumbers = dlist.Split('-');
                    int start = int.Parse(strNumbers[0]);
                    int End = int.Parse(strNumbers[1]);
                    qRangeQuery = new TermRangeQuery { Field = "teamOversRange", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                }
                if (!string.IsNullOrEmpty(_objMatchSituation.TeamScoreRange))
                {

                    string dlist = _objMatchSituation.TeamScoreRange;
                    string[] strNumbers = dlist.Split('-');
                    int start = int.Parse(strNumbers[0]);
                    int End = int.Parse(strNumbers[1]);
                    qRangeQuery = new TermRangeQuery { Field = "teamScoreRange", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                }
            }
            if (_objMatchDetail != null)
            {

                if (!string.IsNullOrEmpty(_objMatchDetail.MatchDate))
                {
                    string dlist = _objMatchDetail.MatchDate;
                    if (dlist.Contains("-"))
                    {
                        string[] strNumbers = dlist.Split('-');
                        int start = int.Parse(DateTime.ParseExact(strNumbers[0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                        int End = int.Parse(DateTime.ParseExact(strNumbers[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                        qRangeQuery = new TermRangeQuery { Field = "matchDate", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                    }
                    else
                    {
                        int date = int.Parse(DateTime.ParseExact(_objMatchDetail.MatchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                        qRangeQuery = new TermRangeQuery { Field = "matchDate", Name = date.ToString() };
                    }
                }

            }
            _objNested &= qRangeQuery;
            return _objNested;
        }

        private QueryContainer FinalSearchKabaddiData(MatchDetail _objMatchDetail, MatchSituation _objMatchSituation, QueryContainer _objNested)
        {
            QueryContainer qRangeQuery = new QueryContainer();
            if (_objMatchSituation != null)
            {

                if (_objMatchDetail != null)
                {

                    if (!string.IsNullOrEmpty(_objMatchDetail.MatchDate))
                    {
                        string dlist = _objMatchDetail.MatchDate;
                        if (dlist.Contains("-"))
                        {
                            string[] strNumbers = dlist.Split('-');
                            int start = int.Parse(DateTime.ParseExact(strNumbers[0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            int End = int.Parse(DateTime.ParseExact(strNumbers[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            qRangeQuery = new TermRangeQuery { Field = "matchDate", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                        }
                        else
                        {
                            int date = int.Parse(DateTime.ParseExact(_objMatchDetail.MatchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            qRangeQuery = new TermRangeQuery { Field = "matchDate", Name = date.ToString() };
                        }
                    }

                }
            }
            _objNested &= qRangeQuery;
            return _objNested;
        }

        //public QueryContainer GetMatchDetailQueryST(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail)
        //{
        //    searchcricket sc = new searchcricket();
        //    SportType = sc.getType(_objMatchDetail.SportID);
        //        if (_objMatchDetail != null)
        //        {
        //            QueryContainer queryShould = new QueryContainer();
        //            if (!string.IsNullOrEmpty(_objMatchDetail.MatchFormat))
        //            {
        //                QueryContainer q1 = new TermQuery { Field = "compType", Value = _objMatchDetail.MatchFormat.ToLower() };

        //                _objNestedQuery &= q1;
        //            }
        //            if (!string.IsNullOrEmpty(_objMatchDetail.VenueId))
        //            {
        //                QueryContainer q2 = new TermQuery { Field = "venueId", Value = _objMatchDetail.MatchFormat.ToLower().ToString() };
        //                _objNestedQuery &= q2;
        //            }

        //            if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id))
        //            {
        //                var myQuery = new BoolQuery();
        //                QueryContainer q3 = new TermQuery { Field = "team1Id", Value = _objMatchDetail.Team1Id.ToLower().ToString() };
        //                QueryContainer q4 = new TermQuery { Field = "team2Id", Value = _objMatchDetail.Team1Id.ToLower().ToString() };
        //                queryShould |= q3 |= q4;
        //                _objNestedQuery &= queryShould;
        //            }
        //            if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
        //            {

        //                QueryContainer q5 = new TermQuery { Field = "team1Id", Value = _objMatchDetail.Team2Id.ToLower().ToString() };
        //                QueryContainer q6 = new TermQuery { Field = "team2Id", Value = _objMatchDetail.Team2Id.ToLower().ToString() };
        //                queryShould |= q5 |= q6;
        //                _objNestedQuery &= queryShould;
        //            }

        //            if (!string.IsNullOrEmpty(_objMatchDetail.SeriesId) && _objMatchDetail.IsParentSeries)
        //            {
        //                QueryContainer q7 = new TermQuery { Field = "parentSeriesId", Value = _objMatchDetail.SeriesId };
        //                _objNestedQuery &= q7;

        //            }
        //            if (!string.IsNullOrEmpty(_objMatchDetail.SeriesId) && !_objMatchDetail.IsParentSeries)
        //            {
        //                QueryContainer q8 = new TermQuery { Field = "seriesId", Value = _objMatchDetail.SeriesId };
        //                _objNestedQuery &= q8;
        //            }
        //            if (!string.IsNullOrEmpty(_objMatchDetail.MatchId))
        //            {
        //                QueryContainer q9 = new TermQuery { Field = "matchId", Value = _objMatchDetail.MatchId };
        //                _objNestedQuery &= q9;
        //            }
        //            if (!string.IsNullOrEmpty(_objMatchDetail.ClearId))
        //            {
        //                var myQuery = new BoolQuery();
        //                QueryContainer q10 = new TermQuery { Field = "qClearId", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
        //                QueryContainer q11 = new TermQuery { Field = "qClearId2", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
        //                QueryContainer q12 = new TermQuery { Field = "qClearId3", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
        //                QueryContainer q13 = new TermQuery { Field = "qClearId4", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
        //                QueryContainer q14 = new TermQuery { Field = "qClearId5", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
        //                QueryContainer q15 = new TermQuery { Field = "qClearId6", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
        //                queryShould |= q10 |= q11 |= q12 |= q13 |= q14 |= q15;
        //                _objNestedQuery &= queryShould;

        //            }
        //            if (!string.IsNullOrEmpty(_objMatchDetail.MatchStageId))
        //            {

        //                var myQuery = new BoolQuery();
        //                string Dlist = Convert.ToString(_objMatchDetail.MatchStageId);
        //                string[] strnumbers = Dlist.Split(',');
        //                foreach (string str in strnumbers)
        //                {
        //                    QueryContainer q16 = new TermQuery { Field = "matchStageId", Value = str };
        //                    queryShould |= q16;

        //                }
        //                _objNestedQuery &= queryShould;

        //            }
        //            if (!string.IsNullOrEmpty(_objMatchDetail.CompTypeId))
        //            {
        //                var myQuery = new BoolQuery();
        //                string Dlist = Convert.ToString(_objMatchDetail.CompTypeId);
        //                string[] strnumbers = Dlist.Split(',');
        //                foreach (string str in strnumbers)
        //                {
        //                    QueryContainer q17 = new TermQuery { Field = "CompTypeId", Value = str };
        //                     queryShould |= q17;
        //                }
        //                _objNestedQuery &= queryShould;

        //            }


        //            if (_objMatchDetail.HasShortClip)
        //            {
        //                QueryContainer q21 = new TermQuery { Field = "hasShortClip", Value = "1" };
        //                _objNestedQuery &= q21;
        //            }

        //            if (!_objMatchDetail.IsAssetSearch)
        //            {
        //                QueryContainer q20 = new TermQuery { Field = "isTagged", Value = "1" };
        //                _objNestedQuery &= q20;

        //            }

        //            if (!string.IsNullOrEmpty(_objMatchDetail.LanguageId) && _objMatchDetail.LanguageId != "0")
        //            {
        //                QueryContainer q19 = new TermQuery { Field = "languageId", Value = _objMatchDetail.LanguageId };
        //                _objNestedQuery &= q19;

        //            }
        //            string input = Convert.ToInt32(Convert.ToBoolean(_objMatchDetail.IsAsset)).ToString();
        //            QueryContainer q18 = new TermQuery { Field = "isAsset", Value = input };
        //            _objNestedQuery &= q18;

        //        }
        //        return _objNestedQuery;
        // }

        public QueryContainer GetPlayerDetails(dynamic _objS1Data, QueryContainer qFinal, List<string> valueObj, int sportid, string _sType, bool isMasterData = false)
        {
            CommonFunction objCf = new CommonFunction();
            QueryContainer queryShouldS = new QueryContainer();
            QueryContainer queryShould = new QueryContainer();
            QueryContainer queryShouldB = new QueryContainer();
            QueryContainer queryAnd_should = new QueryContainer();
            if (_objS1Data != null)
                {
                    if (_objS1Data["IsDefault"] != null && Convert.ToBoolean(_objS1Data["IsDefault"]))
                    {
                        //QueryContainer query = new QueryContainer();
                    string[] isDefaultvalues = objCf.ArrayIsDefaultForSport(sportid);
                    for (int i = 0; i <= isDefaultvalues.Length - 1; i++) {
                        if (isDefaultvalues[i] != "shotTypeId" && isDefaultvalues[i] != "deliveryTypeId") {
                            QueryContainer query1 = new TermQuery { Field = isDefaultvalues[i], Value = 1 };
                            queryShouldB |= query1;
                        }
                        
                    }
                    //QueryContainer q1 = new TermQuery { Field = "isFour", Value = "1" };
                    //QueryContainer q2 = new TermQuery { Field = "isSix", Value = "1" };
                    //QueryContainer q3 = new TermQuery { Field = "isWicket", Value = "1" };
                    //QueryContainer q5 = new TermQuery { Field = "isAppeal", Value = "1" };
                    //QueryContainer q6 = new TermQuery { Field = "isDropped", Value = "1" };
                    //QueryContainer q7 = new TermQuery { Field = "isMisField", Value = "1" };
                    //query = q1 |= q2 |= q3 |= q5 |= q6 |= q7;
                    
                    qFinal &= queryShould;
                        //qAdd.Must query;
                    }
                    else
                    {
                        for (int i = 0; i <= valueObj.Count - 1; i++)
                        {
                            string sType = valueObj[i].Split(",")[1];
                            if (sType == "Boolean")
                            {
                                if (Convert.ToBoolean(_objS1Data[valueObj[i].Split(":")[1].Split(",")[0]]))
                                {
                                    QueryContainer query1 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = "1" };
                                    queryShouldB |= query1;
                                }
                            //qFinal &= queryShould;
                            }
                            if (sType == "string")
                            {
                                if (Convert.ToString(_objS1Data[valueObj[i].Split(":")[1]]) != "")
                                {
                                    string slist = Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]);
                                    if (slist.Contains(","))
                                    {
                                    if (_sType == "S2" && !isMasterData)
                                    {
                                        string[] strArray = slist.Split(',');
                                        foreach (string str in strArray)
                                        {
                                            QueryContainer query9 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = str };
                                            queryShouldS |= query9;
                                        }
                                    }
                                    else {
                                        string[] strArray = slist.Split(',');
                                        foreach (string str in strArray)
                                        {
                                            QueryContainer query9 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = str };
                                            queryShouldS |= query9;
                                        }
                                    }

                                    
                                    //qFinal &= queryShould;
                                }
                                    else
                                    {
                                    if (_sType != "S2")
                                    {
                                        if (Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]) != "")
                                        {
                                            QueryContainer query10 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]) };
                                            qFinal &= query10;
                                        }
                                    }
                                    else {
                                        string sEntityName = _objS1Data[valueObj[i].Split(",")[0].Split(":")[1]];
                                        if (sEntityName == "RunsSaved" && Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]) != 0) {
                                            QueryContainer query10 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]) };
                                            qFinal &= query10;
                                        }
                                    }
                                    
                                    }
                                }

                            }
                        //if (valueObj[i].Split(":")[0] == "OR")
                        //{
                        //    queryAnd_should &= queryShould;
                        //}
                       

                    }
                    }


                }
          
           // if (queryShouldB != null)
            //{
                qFinal &= queryShouldB;
            //}
            //else {
            return qFinal;
            //}

            
        }

        //private static QueryContainer GetCricketMatchSituationQuery(MatchDetail _objMatchDetail, PlayerDetail _objPlayerDetail, MatchSituation _objMatchSituation, QueryContainer _objNestedQuery)
        //{
        //    QueryContainer qcShould = new QueryContainer();
        //    QueryContainer qcAnd = new QueryContainer();

        //    if (_objMatchSituation != null)
        //    {
        //        if (!string.IsNullOrEmpty(_objMatchSituation.Result))
        //        {
        //            SearchQueryModel _objSqModel = new SearchQueryModel();
        //            if (_objMatchSituation.Result.Contains("Wins"))
        //            {
        //                if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id) || !string.IsNullOrEmpty(_objMatchDetail.Team2Id))
        //                {
                            
        //                    if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id))
        //                    {
        //                        QueryContainer q1 = new TermQuery { Field = "winner", Value = _objMatchDetail.Team1Id };
        //                        qcAnd &= q1;
        //                    }
        //                    else if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
        //                    {
        //                        QueryContainer q2 = new TermQuery { Field = "winner", Value = _objMatchDetail.Team2Id };
        //                        qcAnd &= q2;
        //                    }

        //                }
        //                if (!string.IsNullOrEmpty(_objPlayerDetail.BatsmanID))
        //                {
                        
        //                    QueryContainer q3 = new TermQuery { Field = "winnerPlayer", Value = _objPlayerDetail.BatsmanID };
        //                    qcAnd &= q3;
        //                }
        //                if (!string.IsNullOrEmpty(_objPlayerDetail.BowlerID))
        //                {
                           
        //                    QueryContainer q4 = new TermQuery { Field = "winnerPlayer", Value = _objPlayerDetail.BowlerID };
        //                    qcAnd &= q4;
        //                }
        //                if (!string.IsNullOrEmpty(_objPlayerDetail.FielderID))
        //                {
        //                    QueryContainer q5 = new TermQuery { Field = "winningFielder", Value = _objPlayerDetail.FielderID };
        //                    qcAnd &= q5;
        //                }
        //            }
        //            else if (_objMatchSituation.Result.Contains("Losses"))
        //            {
        //                if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id) || !string.IsNullOrEmpty(_objMatchDetail.Team2Id))
        //                {

        //                    if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id))
        //                    {
        //                        QueryContainer q6 = new TermQuery { Field = "loser", Value = _objMatchDetail.Team1Id };
        //                        qcAnd &= q6;

        //                    }
        //                    else if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
        //                    {
        //                        QueryContainer q7 = new TermQuery { Field = "loser", Value = _objMatchDetail.Team2Id };
        //                        qcAnd &= q7;
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(_objPlayerDetail.BatsmanID))
        //                {

        //                    QueryContainer q8 = new TermQuery { Field = "loserPlayer", Value = _objPlayerDetail.BatsmanID };
        //                    qcAnd &= q8;
        //                }
        //                if (!string.IsNullOrEmpty(_objPlayerDetail.BowlerID))
        //                {
        //                    QueryContainer q9 = new TermQuery { Field = "loserPlayer", Value = _objPlayerDetail.BowlerID };
        //                    qcAnd &= q9;
        //                }
        //                if (!string.IsNullOrEmpty(_objPlayerDetail.FielderID))
        //                {
        //                    QueryContainer q10 = new TermQuery { Field = "winningFielder", Value = _objPlayerDetail.FielderID };
        //                    qcAnd &= q10;
        //                }
        //            }
        //            else
        //            {

        //                string matchresult = string.Empty;
        //                if (_objMatchSituation.Result.Contains("No Result"))
        //                {
        //                    matchresult = "No Result";
        //                    QueryContainer q11 = new TermQuery { Field = "matchResult", Value = matchresult.ToLower() };
        //                    qcAnd &= q11;
        //                }
        //                else if (_objMatchSituation.Result.Contains("Draw"))
        //                {
        //                    matchresult = "Drawn";
        //                    QueryContainer q12 = new TermQuery { Field = "matchResult", Value = matchresult.ToLower() };
        //                    qcAnd &= q12;
        //                }
        //                else if (_objMatchSituation.Result.Contains("Tie"))
        //                {
        //                    matchresult = "Tied";
        //                    QueryContainer q13 = new TermQuery { Field = "matchResult", Value = matchresult.ToLower() };
        //                    qcAnd &= q13;
        //                }

        //            }
        //        }
        //        if (!string.IsNullOrEmpty(_objMatchSituation.Innings))
        //        {
        //            QueryContainer q14 = new TermQuery { Field = "innings", Value = _objMatchSituation.Innings };
        //            qcAnd &= q14;
        //        }

        //        if (!string.IsNullOrEmpty(_objMatchSituation.MatchInstance))
        //        {

        //            if (_objMatchSituation.MatchInstance.Contains("Powerplay"))
        //            {

        //                QueryContainer q15 = new TermQuery { Field = "isPowerPlayOver", Value =1 };
        //                qcAnd &= q15;
        //            }
        //            else if (_objMatchSituation.MatchInstance.Contains("Middle"))
        //            {
        //                QueryContainer q16 = new TermQuery { Field = "is_middle", Value = 1};
        //                qcAnd &= q16;
        //            }
        //            else if (_objMatchSituation.MatchInstance.Contains("Death"))
        //            {
        //                QueryContainer q17 = new TermQuery { Field = "is_death", Value = 1 };
        //                qcAnd &= q17;
        //            }
        //            else if (_objMatchSituation.MatchInstance.Contains("Final Over"))
        //            {
        //                QueryContainer q18 = new TermQuery { Field = "is_lastover", Value =1 };
        //                qcAnd &= q18;
        //            }
        //            else if (_objMatchSituation.MatchInstance.Contains("Last Ball"))
        //            {
        //                QueryContainer q19 = new TermQuery { Field = "is_lastBall", Value = 1 };
        //                qcAnd &= q19;
        //            }

        //        }
        //    }



        //    return _objNestedQuery;
        //}

        public QueryContainer getDetailsAsPerSport(dynamic _objS1Data, QueryContainer _objNestedQuery, MatchDetail _ObjMatchDetails, MatchSituation _objMatchSituation, List<string> valueObj, int sportid)
        {
            //QueryContainer _objMatchResult = new QueryContainer();
            dynamic obj;
            if (_ObjMatchDetails != null)
            {
                try
                {
                    switch (_ObjMatchDetails.SportID)
                    {
                        case 1:
                            _objNestedQuery = _cricket.GetMatchDetailQuery(_objNestedQuery, _ObjMatchDetails);
                            _objNestedQuery = _cricket.GetPlayerDetails(_objS1Data, _objNestedQuery, valueObj, sportid);
                            obj = _cricket;
                            break;
                        case 3:
                            _objNestedQuery = _kabaddi.GetMatchDetailQuery(_objNestedQuery, _ObjMatchDetails);
                            _objNestedQuery = _kabaddi.GetPlayerDetails(_objS1Data, _objNestedQuery, valueObj, sportid);
                            obj = _kabaddi;
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                }

            }
            
            if (sportid == 1)// Only For Cricket
            {
                _objNestedQuery = _cricket.GetCricketMatchSituationQueryST(_objNestedQuery, _objMatchSituation);
            }
            return _objNestedQuery;
        }

        public string GetAutoCompleteData(string sportid, string stype, string term = "")
        {
            throw new NotImplementedException();
        }

        //private QueryContainer GetMatchDetailQuery(MatchDetail _objMatchDetail, QueryContainer _objNestedQuery, bool isMasterData = false)
        //{
        //    if (_objMatchDetail != null)
        //    {
        //        QueryContainer queryShould = new QueryContainer();

        //        if (!string.IsNullOrEmpty(_objMatchDetail.MatchFormat))
        //        {
        //            if (_objMatchDetail.MatchFormat.Contains(","))
        //            {
        //                string[] _objaaray = _objMatchDetail.MatchFormat.Split(',');
        //                foreach (var v in _objaaray)
        //                {
        //                    QueryContainer q1 = new TermQuery { Field = "compType", Value = v.ToLower() };
        //                    queryShould |= q1;
        //                }
        //                //_objNestedQuery.Add(bq, Occur.MUST);
        //            }
        //            else {
        //                QueryContainer q1 = new TermQuery { Field = "compType", Value = _objMatchDetail.MatchFormat.ToLower() };
        //                _objNestedQuery &= q1;
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(_objMatchDetail.VenueId))
        //        {
        //            QueryContainer q2 = new TermQuery { Field = "venueId", Value = _objMatchDetail.MatchFormat.ToLower().ToString() };
        //            _objNestedQuery &= q2;
        //        }

        //        if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id))
        //        {
        //            QueryContainer q3 = new TermQuery { Field = "team1Id", Value = _objMatchDetail.Team1Id.ToLower().ToString() };
        //            queryShould |= q3;
        //            _objNestedQuery &= queryShould;
        //        }
        //        if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
        //        {
        //            QueryContainer q4 = new TermQuery { Field = "team2Id", Value = _objMatchDetail.Team1Id.ToLower().ToString() };
        //            queryShould |= q4;
        //            _objNestedQuery &= queryShould;
        //        }
        //        if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
        //        {

        //            QueryContainer q5 = new TermQuery { Field = "team1Id", Value = _objMatchDetail.Team2Id.ToLower().ToString() };
        //            QueryContainer q6 = new TermQuery { Field = "team2Id", Value = _objMatchDetail.Team2Id.ToLower().ToString() };
        //            queryShould |= q5 |= q6;
        //            _objNestedQuery &= queryShould;
        //        }

        //        if (!string.IsNullOrEmpty(_objMatchDetail.SeriesId) && _objMatchDetail.IsParentSeries)
        //        {
        //            string Dlist = _objMatchDetail.SeriesId;
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {
        //                QueryContainer q7 = new TermQuery { Field = "parentSeriesId", Value = str };
        //                queryShould  |= q7;
        //            }
        //            // QueryContainer q7 = new TermQuery { Field = "parentSeriesId", Value = _objMatchDetail.SeriesId };
        //            //_objNestedQuery &= q7;

        //        }
        //        if (!string.IsNullOrEmpty(_objMatchDetail.SeriesId) && !_objMatchDetail.IsParentSeries)
        //        {


        //            string Dlist = _objMatchDetail.SeriesId;
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {

        //                QueryContainer q8 = new TermQuery { Field = "seriesId", Value = str };
        //                queryShould |= q8;
        //            }


        //        }
        //        if (!string.IsNullOrEmpty(_objMatchDetail.MatchId))
        //        {

        //            string Dlist = _objMatchDetail.MatchId;
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {
        //                QueryContainer q9 = new TermQuery { Field = "matchId", Value = str };
        //                queryShould |= q9;
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(_objMatchDetail.ClearId))
        //        {
        //            var myQuery = new BoolQuery();
        //            QueryContainer q10 = new TermQuery { Field = "qClearId", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
        //            QueryContainer q11 = new TermQuery { Field = "qClearId2", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
        //            QueryContainer q12 = new TermQuery { Field = "qClearId3", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
        //            QueryContainer q13 = new TermQuery { Field = "qClearId4", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
        //            QueryContainer q14 = new TermQuery { Field = "qClearId5", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
        //            QueryContainer q15 = new TermQuery { Field = "qClearId6", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
        //            queryShould |= q10 |= q11 |= q12 |= q13 |= q14 |= q15;
        //            _objNestedQuery &= queryShould;

        //        }
        //        if (!string.IsNullOrEmpty(_objMatchDetail.MatchStageId))
        //        {

        //            var myQuery = new BoolQuery();
        //            string Dlist = Convert.ToString(_objMatchDetail.MatchStageId);
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {
        //                QueryContainer q16 = new TermQuery { Field = "matchStageId", Value = str };
        //                queryShould |= q16;

        //            }
        //            _objNestedQuery &= queryShould;

        //        }
        //        if (!string.IsNullOrEmpty(_objMatchDetail.CompTypeId))
        //        {
        //            var myQuery = new BoolQuery();
        //            string Dlist = Convert.ToString(_objMatchDetail.CompTypeId);
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {
        //                QueryContainer q17 = new TermQuery { Field = "CompTypeId", Value = str };
        //                queryShould |= q17;
        //            }
        //            _objNestedQuery &= queryShould;

        //        }

        //        if (!string.IsNullOrEmpty(_objMatchDetail.GameTypeId))
        //        {
        //            var myQuery = new BoolQuery();
        //            string Dlist = Convert.ToString(_objMatchDetail.GameTypeId);
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {
        //                QueryContainer q23 = new TermQuery { Field = "gameTypeId", Value = str };
        //                queryShould |= q23;
        //            }
        //           // _objNestedQuery &= queryShould;

        //        }



        //        if (_objMatchDetail.HasShortClip)
        //        {
        //            QueryContainer q21 = new TermQuery { Field = "hasShortClip", Value = "1" };
        //            _objNestedQuery &= q21;
        //        }

        //        if (!_objMatchDetail.IsAssetSearch)
        //        {
        //            QueryContainer q20 = new TermQuery { Field = "isTagged", Value = "1" };
        //            _objNestedQuery &= q20;


        //        }

        //        //if (!string.IsNullOrEmpty(_objMatchDetail.LanguageId) && _objMatchDetail.LanguageId != "0")
        //        //{
        //        //    QueryContainer q19 = new TermQuery { Field = "languageId", Value = _objMatchDetail.LanguageId };
        //        //    _objNestedQuery &= q19;

        //        //}


        //        if (!string.IsNullOrEmpty(_objMatchDetail.LanguageId) && _objMatchDetail.LanguageId != "0" && _objMatchDetail.IsAsset)
        //        {
        //            string[] Id = _objMatchDetail.LanguageId.Split(',');

        //            foreach (string str in Id)
        //            {

        //                QueryContainer q19 = new TermQuery { Field = "languageId", Value = str };
        //                queryShould |= q19;
        //            }
        //            //_objNestedQuery.Add(bq, Occur.MUST);
        //        }




        //        string input = Convert.ToInt32(Convert.ToBoolean(_objMatchDetail.IsAsset)).ToString();
        //        QueryContainer q18 = new TermQuery { Field = "isAsset", Value = input };
        //        _objNestedQuery &= q18;
        //        _objNestedQuery &= queryShould;

        //    }

        //    return _objNestedQuery;
        //}
        //private QueryContainer GetCricketPlayerDetailQueryS2(PlayerDetail _objPlayerDetail, QueryContainer _objNestedQuery, bool isMasterData = false)
        //{
        //    QueryContainer qShould = new QueryContainer();
        //    if (_objPlayerDetail != null)
        //    {

        //        if (!string.IsNullOrEmpty(_objPlayerDetail.BatsmanID))
        //        {
        //            QueryContainer q1 = new TermQuery { Field = "batsmanId", Value = _objPlayerDetail.BatsmanID };
        //            _objNestedQuery &= q1;
        //            //_objNestedQuery.Add(query, Occur.MUST);
        //        }
        //        if (_objPlayerDetail.BatsmanFours || _objPlayerDetail.BatsmanSixes || _objPlayerDetail.BatsmanDots || _objPlayerDetail.BatsmanEdged || _objPlayerDetail.BastsmanBeaten || _objPlayerDetail.BatsmanDismissal || _objPlayerDetail.BatsmanAppeal)
        //        {

        //            if (_objPlayerDetail.BatsmanFours)
        //            {
        //                QueryContainer q2 = new TermQuery { Field = "isFour", Value = 1 };
        //                qShould |= q2;

        //            }
        //            if (_objPlayerDetail.BatsmanSixes)
        //            {
        //                QueryContainer q3 = new TermQuery { Field = "isSix", Value = 1 };
        //                qShould |= q3;
        //            }
        //            if (_objPlayerDetail.BatsmanDots)
        //            {

        //                QueryContainer q4 = new TermQuery { Field = "isDot", Value = 1 };
        //                qShould |= q4;
        //            }
        //            if (_objPlayerDetail.BatsmanEdged)
        //            {


        //                QueryContainer q5 = new TermQuery { Field = "isEdged", Value = 1 };
        //                qShould |= q5;
        //            }
        //            if (_objPlayerDetail.BastsmanBeaten)
        //            {

        //                QueryContainer q6 = new TermQuery { Field = "isBeaten", Value = 1 };
        //                qShould |= q6;

        //            }
        //            if (_objPlayerDetail.BatsmanDismissal)
        //            {
        //                QueryContainer q7 = new TermQuery { Field = "isWicket", Value = 1 };
        //                qShould |= q7;
        //            }
        //            if (_objPlayerDetail.BatsmanAppeal)
        //            {
        //                QueryContainer q8 = new TermQuery { Field = "isAppeal", Value = 1 };
        //                qShould |= q8;

        //            }
        //            //_objNestedQuery.Add(Bq, Occur.MUST);
        //        }
        //        if (!string.IsNullOrEmpty(_objPlayerDetail.ShotType) && !isMasterData)
        //        {

        //            string Dlist = _objPlayerDetail.ShotType;
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {

        //                QueryContainer q9 = new TermQuery { Field = "shotTypeId", Value = str };
        //                qShould |= q9;
        //            }

        //        }
        //        if (!string.IsNullOrEmpty(_objPlayerDetail.ShotZone) && !isMasterData)
        //        {
        //            //BooleanQuery bq = new BooleanQuery();
        //            string Dlist = _objPlayerDetail.ShotZone;
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {
        //                QueryContainer q10 = new TermQuery { Field = "shotZoneId", Value = str };
        //                qShould |= q10;
        //            }
        //            //_objNestedQuery.Add(bq, Occur.MUST);
        //        }
        //        if (!string.IsNullOrEmpty(_objPlayerDetail.DismissalType) && !isMasterData)
        //        {

        //            string Dlist = _objPlayerDetail.DismissalType;
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {

        //                QueryContainer q11 = new TermQuery { Field = "dismissalId", Value = str };
        //                qShould |= q11;
        //            }

        //        }
        //        if (_objPlayerDetail.FielderStumping)
        //        {

        //            QueryContainer q12 = new TermQuery { Field = "dismissalId", Value = "st" };
        //            _objNestedQuery &= q12;
        //        }
        //        if (!string.IsNullOrEmpty(_objPlayerDetail.BowlerID))
        //        {


        //            QueryContainer q13 = new TermQuery { Field = "bowlerId", Value = _objPlayerDetail.BowlerID };
        //            _objNestedQuery &= q13;
        //        }
        //        if (_objPlayerDetail.BowlerWides || _objPlayerDetail.BowlerNoBalls || _objPlayerDetail.BowlerBeaten || _objPlayerDetail.BowlerDots || _objPlayerDetail.BowlerEdged || _objPlayerDetail.BowlerDismissal || _objPlayerDetail.BowlerAppeal)
        //        {
        //            ;
        //            if (_objPlayerDetail.BowlerWides)
        //            {


        //                QueryContainer q14 = new TermQuery { Field = "isWide", Value = "1" };
        //                qShould |= q14;
        //            }
        //            if (_objPlayerDetail.BowlerNoBalls)
        //            {

        //                QueryContainer q15 = new TermQuery { Field = "isNoBall", Value = "1" };
        //                qShould |= q15;
        //            }
        //            if (_objPlayerDetail.BowlerDots)
        //            {

        //                QueryContainer q16 = new TermQuery { Field = "isDot", Value = "1" };
        //                qShould |= q16;
        //            }
        //            if (_objPlayerDetail.BowlerEdged)
        //            {
        //                QueryContainer q17 = new TermQuery { Field = "isEdged", Value = "1" };
        //                qShould |= q17;
        //            }
        //            if (_objPlayerDetail.BowlerBeaten)
        //            {
        //                QueryContainer q18 = new TermQuery { Field = "isBeaten", Value = "1" };
        //                qShould |= q18;
        //            }
        //            if (_objPlayerDetail.BowlerDismissal)
        //            {

        //                QueryContainer q19 = new TermQuery { Field = "isWicket", Value = "1" };
        //                qShould |= q19;

        //            }
        //            if (_objPlayerDetail.BowlerAppeal)
        //            {
        //                QueryContainer q20 = new TermQuery { Field = "isAppeal", Value = "1" };
        //                qShould |= q20;
        //            }

        //        }
        //        if (!string.IsNullOrEmpty(_objPlayerDetail.DeliveryType) && !isMasterData)
        //        {

        //            string Dlist = _objPlayerDetail.DeliveryType;
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {
        //                QueryContainer q21 = new TermQuery { Field = "deliveryTypeId", Value = str };
        //                qShould |= q21;
        //            }

        //        }
        //        if (!string.IsNullOrEmpty(_objPlayerDetail.BowlingLength) && !isMasterData)
        //        {

        //            string Dlist = _objPlayerDetail.BowlingLength;
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {
        //                QueryContainer q22 = new TermQuery { Field = "bowlingLengthId", Value = str };
        //                qShould |= q22;
        //            }

        //        }
        //        if (!string.IsNullOrEmpty(_objPlayerDetail.BowlingLine) && !isMasterData)
        //        {

        //            string Dlist = _objPlayerDetail.BowlingLine;
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {
        //                QueryContainer q23 = new TermQuery { Field = "bowlingLineId", Value = str };
        //                qShould |= q23;
        //            }

        //        }
        //        if (_objPlayerDetail.BowlingOver || _objPlayerDetail.BowlingRound)
        //        {
        //            if (_objPlayerDetail.BowlingOver)
        //            {

        //                QueryContainer q24 = new TermQuery { Field = "isOverTheWicket", Value = "1" };
        //                qShould |= q24;
        //            }
        //            if (_objPlayerDetail.BowlingRound)
        //            {
        //                QueryContainer q25 = new TermQuery { Field = "isRoundTheWicket", Value = "1" };
        //                qShould |= q25;

        //            }

        //        }
        //        if (_objPlayerDetail.BowlerSpin || _objPlayerDetail.BowlerPace)
        //        {

        //            if (_objPlayerDetail.BowlerSpin)
        //            {
        //                QueryContainer q26 = new TermQuery { Field = "skill", Value = "s" };
        //                qShould |= q26;

        //             }

        //            if (_objPlayerDetail.BowlerPace)
        //            {


        //                QueryContainer q27 = new TermQuery { Field = "skill", Value = "p" };
        //                qShould |= q27;
        //            }

        //        }
        //        if (!string.IsNullOrEmpty(_objPlayerDetail.FielderID))
        //        {


        //            QueryContainer q27 = new TermQuery { Field = "fielderId", Value = _objPlayerDetail.FielderID };
        //            _objNestedQuery &= q27;
        //        }
        //        if (_objPlayerDetail.FielderCatch || _objPlayerDetail.FielderRunOut || _objPlayerDetail.FielderDrops || _objPlayerDetail.FielderMisFields)
        //        {

        //            if (_objPlayerDetail.FielderCatch)
        //            {

        //                QueryContainer q28 = new TermQuery { Field = "isCatch", Value = "1" };
        //                qShould |= q28;
        //            }
        //            if (_objPlayerDetail.FielderRunOut)
        //            {

        //                QueryContainer q29 = new TermQuery { Field = "isRunOut", Value = "1" };
        //                qShould |= q29;
        //            }
        //            if (_objPlayerDetail.FielderDrops)
        //            {
        //                QueryContainer q30 = new TermQuery { Field = "isDropped", Value = "1" };
        //                qShould |= q30;
        //            }
        //            if (_objPlayerDetail.FielderMisFields)
        //            {

        //                QueryContainer q31 = new TermQuery { Field = "isMisField", Value = "1" };
        //                qShould |= q31;
        //            }

        //        }
        //        if (!string.IsNullOrEmpty(_objPlayerDetail.FieldingPosition) && !isMasterData)
        //        {

        //            string Dlist = _objPlayerDetail.FieldingPosition;
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {
        //                 QueryContainer q32 = new TermQuery { Field = "fielderPositionId", Value = str };
        //                qShould |= q32;
        //            }

        //        }
        //        if (!string.IsNullOrEmpty(_objPlayerDetail.RunsSaved) && _objPlayerDetail.RunsSaved != "0")
        //        {
        //            QueryContainer q33 = new TermQuery { Field = "runsSaved", Value = Convert.ToString(_objPlayerDetail.RunsSaved) };
        //            _objNestedQuery &= q33;

        //        }
        //        if (!string.IsNullOrEmpty(_objPlayerDetail.BattingOrder) && !isMasterData)
        //        {
        //            string Dlist = _objPlayerDetail.BattingOrder;
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {
        //                QueryContainer q34 = new TermQuery { Field = "battingOrder", Value = str };
        //                qShould |= q34;
        //            }

        //        }
        //        if (!string.IsNullOrEmpty(_objPlayerDetail.BowlingArm) && !isMasterData)
        //        {
        //            string Dlist = _objPlayerDetail.BowlingArm;
        //            string[] strnumbers = Dlist.Split(',');
        //            foreach (string str in strnumbers)
        //            {
        //                QueryContainer q35 = new TermQuery { Field = "bowlingArm", Value = str.ToLower() };
        //                qShould |= q35;
        //            }

        //        }
        //    }
        //    _objNestedQuery &= qShould;
        //    return _objNestedQuery;
        //}

        public ExtendedSearchResultFilterData searchStoryTeller(ELModels.MatchDetail _objMatchDetail, QueryContainer _objNestedQuery, dynamic _objS1Data, Dictionary<string, object> ObjectArray, IEnumerable<SearchResultFilterData> obj, string value, string IndexName)
        {
            ExtendedSearchResultFilterData _objSearchResults = new ExtendedSearchResultFilterData();
            try
            {
                EsClient_obj = _oLayer.CreateConnection();
                _objSearchResults.ResultData = new List<SearchResultFilterData>();
                _objSearchResults.Master = new MasterDatas();
                _objSearchResults.Master.MasterData = new Dictionary<string, object>();
                //searchcricket sc = new searchcricket();
                // SportType = sc.getType(_objMatchDetail.SportID);
                //if ("CRICKET" == SportType) {
                //string ReqShotType = _objS1Data["ShotType"]; string ReqDeliveryType = _objS1Data["DeliveryType"];
                //string[] _objReqShotType = ReqShotType.Contains(",") ? _objReqShotType = ReqShotType.Split(",") : _objReqShotType = new string[] { _objS1Data["ShotType"] };
                //string[] _objReqDeliveryType = ReqDeliveryType.Contains(",") ? _objReqDeliveryType = ReqDeliveryType.Split(",") : _objReqDeliveryType = new string[] { _objS1Data["DeliveryType"] };
                //ObjectArray = GetDropdowns(_objNestedQuery, ObjectArray, EsClient, "cricket", GetColumnForEntity(13), _objReqShotType);
                //ObjectArray = GetDropdowns(_objNestedQuery, ObjectArray, EsClient, "cricket", GetColumnForEntity(32), _objReqDeliveryType);

                //_objSearchResults.Master.MasterData = ObjectArray;
                Dictionary<string, object> ddlDropdowns = new Dictionary<string, object>();
                //GetMatchDetails _objMatchDetails = new GetMatchDetails();

                switch (_objMatchDetail.SportID)
                {
                    case 1:
                        ddlDropdowns = _cricket.bindS1andS2Dropdown(_objS1Data);
                        break;
                    case 3:
                        ddlDropdowns = _kabaddi.bindS1andS2Dropdown(_objS1Data);
                        break;
                    default:
                        break;
                }

                if (value != null)
                {
                    string[] valuess = value.Split(",");
                    foreach (var items in valuess)
                    {
                        var item = items.Split("::");
                        string Type = _objS1Data[item[0]];
                        string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
                        foreach (KeyValuePair<string, object> entry in ddlDropdowns)
                        {
                            if (item.ToString().Split(",")[0] != entry.Key.ToString())
                            {
                                if (entry.Value.ToString() != "")
                                {
                                    QueryContainer query = new TermQuery { Field = entry.Key, Value = entry.Value };
                                    _objNestedQuery &= query;
                                }

                            }
                        }
                        _objSearchResults.Master.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.Master.MasterData, EsClient_obj, IndexName, objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType, _objMatchDetail.SportID);
                    }
                }

                switch (_objMatchDetail.SportID)
                {
                    case 1:
                        obj = _cricket.returnSportResult(EsClient_obj, _objNestedQuery, IndexName);
                        break;
                    case 3:
                        obj = _kabaddi.returnSportResult(EsClient_obj, _objNestedQuery, IndexName);
                        break;
                    default:
                        break;
                }

                _objSearchResults.ResultData = obj;
                
                //throw new NotImplementedException();
            }
            catch (Exception e)
            {

            }
            return _objSearchResults;
        }

        //public List<FilteredEntityKabaddi> GetFilteredEntitiesBySportKabaddi(SearchEntityRequestData _objReqData)

        public string GetFilteredEntitiesBySportKabaddi(SearchEntityRequestData _objReqData)
        {
            //System.Web.Script.Serialization.JavaScriptSerializer _objSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string jsonDataresult = "";
            var responseResult = new List<FilteredEntityKabaddi>();
            string searchtext = string.Empty;
            string result = string.Empty;
            if (_objReqData != null)
            {
                EsClient_obj = _oLayer.CreateConnection();
                _objLstSearchQuery = new List<SearchQueryModel>();
                string jsonData = JsonConvert.SerializeObject(_objReqData);
                _objEntityReqData = JsonConvert.DeserializeObject<SearchEntityRequestData>(jsonData);
                searchtext = _objEntityReqData.EntityText.Trim().ToLower();
                _objMatchDetail = _objEntityReqData.MatchDetails;
                _objMatchDetail.SeriesId = _objEntityReqData.EntityTypeId != 5 ? _objMatchDetail.SeriesId : string.Empty;
                _objMatchDetail.MatchId = _objEntityReqData.EntityTypeId != 9 ? _objMatchDetail.MatchId : string.Empty;
                if (_objEntityReqData != null)
                {
                    _columns = objCF.GetColumnForEntity(_objEntityReqData.EntityTypeId);
                    IEnumerable<FilteredEntityKabaddi> _objFilteredEntityKabaddi = new List<FilteredEntityKabaddi>();
                    _objNestedQuery = _kabaddi.GetMatchDetailQuery(_objNestedQuery, _objMatchDetail);
                    _objNestedQuery = objCF.GetPlayerDetailQueryForFilteredEntityBySport(_objNestedQuery, _objEntityReqData.playerDetails, _objMatchDetail.SportID);//GetCricketPlayerDetailQuery(_objEntityReqData.playerDetails, _objNestedBoolQuery);
                    if (!string.IsNullOrEmpty(_objMatchDetail.MatchDate))
                    {
                        _objNestedQuery = _kabaddi.GetEntityBySport(_objNestedQuery, _objMatchDetail, _columns, searchtext);
                    }
                    if (_columns.Count > 0)
                    {
                        List<string> EntityIds = new List<string>();
                        List<string> EntityNames = new List<string>();
                        foreach (var col in _columns)
                        {
                            EntityIds.Add(col.Key);
                            EntityNames.Add(col.Value);

                        }
                        if (_objEntityReqData.EntityTypeId == 5)
                        {
                            var s1 = _kabaddi.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(1), EntityNames.ElementAt(1), EsClient_obj, searchtext);
                            var s1Result = s1.Select(a => a.IsParentSeries = "1");
                            var s2 = _kabaddi.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(0), EntityNames.ElementAt(0), EsClient_obj, searchtext);
                            var s2Result = s1.Select(a => a.IsParentSeries = "0");
                            var s = s1Result.Union(s2Result);
                            //result = _objSerializer.Serialize(s);
                            //responseResult = s;
                        }
                        if (_objEntityReqData.EntityTypeId == 3 || _objEntityReqData.EntityTypeId == 4)
                        {
                            //var s1 = _objFilteredEntityForCricket.Select(t => new { EntityId = t.Team1Id, EntityName = t.Team1 }).Distinct();
                            //var s2 = _objFilteredEntityForCricket.Select(t => new { EntityId = t.Team2Id, EntityName = t.Team2 }).Distinct();
                            //var s = s1.Union(s2).Where(r => r.EntityName.ToString() != "" && r.EntityName.ToLower().Contains(searchtext)).Distinct().OrderBy(r => r.EntityName);
                            ////result = _objSerializer.Serialize(s);

                            var s1 = _kabaddi.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(1), EntityNames.ElementAt(1), EsClient_obj, searchtext);
                            var s2 = _kabaddi.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(0), EntityNames.ElementAt(0), EsClient_obj, searchtext);
                            var s = s1.Union(s2);
                        }

                        if (_objEntityReqData.EntityTypeId == 30 || _objEntityReqData.EntityTypeId == 31)
                        {
                            var s1 = _kabaddi.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(1), EntityNames.ElementAt(1), EsClient_obj, searchtext);
                            var s2 = _kabaddi.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(0), EntityNames.ElementAt(0), EsClient_obj, searchtext);
                            var s = s1.Union(s2);
                        }

                        if (searchtext != "")
                        {
                            var Result = _kabaddi.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(0), EntityNames.ElementAt(0), EsClient_obj, searchtext);
                            responseResult = Result;
                        }
                        jsonDataresult = JsonConvert.SerializeObject(responseResult);

                        _objFilteredEntityKabaddi = null;
                    }
                }
            }
            return jsonDataresult;
        }

        public bool AddUpdateForSearch(string RequestData, int sportId, bool isS2= false)
        {
            bool isSuccess = false;
            if (sportId == 1)
            {
                if (isS2)
                {

                    List<SearchS2Data> _objLstSearchData = JsonConvert.DeserializeObject<List<SearchS2Data>>(RequestData);
                    try
                    {
                        if (_objLstSearchData != null && _objLstSearchData.Count > 0)
                        {
                            isSuccess = _addUpdateIndex.AddUpdateElasticIndex(_objLstSearchData, 3, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                    }

                }
                else {
                    List<SearchCricketData> _objLstSearchData = JsonConvert.DeserializeObject<List<SearchCricketData>>(RequestData);
                    try
                    {
                        if (_objLstSearchData != null && _objLstSearchData.Count > 0)
                        {
                            isSuccess = _addUpdateIndex.AddUpdateElasticIndex(_objLstSearchData, 1, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        isSuccess = false;
                    }
                }
            }
            else if (sportId == 3) {
                List<KabaddiS1Data> _objLstSearchData = JsonConvert.DeserializeObject<List<KabaddiS1Data>>(RequestData);
                try
                {
                    if (_objLstSearchData != null && _objLstSearchData.Count > 0)
                    {
                        isSuccess = _addUpdateIndex.AddUpdateElasticIndex(_objLstSearchData, 3, false);
                    }
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }

            }
            
            return isSuccess;
        }

        public string GetSearchResults(SearchRequestData _objReqData)
        {
            string result = string.Empty;
            SearchCricketExtendedResultData _objSearchResults = new SearchCricketExtendedResultData();
            Dictionary<string, Int64> _objDicSearchResultCount = new Dictionary<string, Int64>();
            IEnumerable<SearchCricketResultTempData> searchResults = new List<SearchCricketResultTempData>();
            IEnumerable<SearchCricketResultData> _objresult = new List<SearchCricketResultData>();
            List<string> valueObj = _con.LstKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID).ToLower() + "S2", "PlayerDetails");
            string S2DataObj1 = _con.GetKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID).ToLower(), "S2DataCount");
            string[] arrayS2Count = S2DataObj1.Split(",");
            List<SearchQueryModel> _objLstSearchQuery = new List<SearchQueryModel>();
            _objSearchResults.ResultData = new List<SearchCricketResultData>();
            QueryContainer _objNestedQuery = new QueryContainer();
            int sportid = 1;
            string languageid = "1";
            if (_objReqData != null)
            {
                string input = "";
                MatchDetail _objMatchDetail = _objReqData.MatchDetails.FirstOrDefault();
                PlayerDetail _objPlayerDetail = _objReqData.PlayerDetails.FirstOrDefault();
                MatchSituation _objMatchSituation = _objReqData.MatchSituations.FirstOrDefault();
                _objPlayerDetails = _objReqData.PlayerDetails.FirstOrDefault();
                dynamic _objS1Data = _objPlayerDetails;
                if (_objMatchDetail != null)
                {
                    sportid = _objMatchDetail.SportID;
                    languageid = _objMatchDetail.LanguageId;
                    input = Convert.ToInt32(Convert.ToBoolean(_objMatchDetail.IsAsset)).ToString();
                    _objNestedQuery = _cricketS2.GetMatchDetailQuery(_objNestedQuery, _objMatchDetail);
                    if (_objPlayerDetail != null)
                    {
                        _objNestedQuery = _cricketS2.GetPlayerDetails(_objS1Data, _objNestedQuery, valueObj, sportid);
                        if (_objMatchSituation != null)
                        {
                            _objNestedQuery = _cricketS2.GetCricketMatchSituationQuery(_objMatchDetail, _objPlayerDetails, _objMatchSituation, _objNestedQuery);//new
                        }
                    }
                }
                foreach (var items in arrayS2Count)
                {
                    _objDicSearchResultCount.Add(items, _cricketS2.getMatchCount(_objNestedQuery, EsClient_obj, items));
                }
                QueryContainer qcIstagged = new TermQuery { Field = "isAsset", Value = input };
                _objNestedQuery &= qcIstagged;
                searchResults = _cricketS2.SearchS1(_objNestedQuery, EsClient_obj);
                if (searchResults != null)
                {
                    if (!string.IsNullOrEmpty(_objMatchDetail.MatchDate) || _objMatchSituation != null)
                    {
                        _objNestedQuery = FinalSearchCricketData(_objMatchDetail, _objMatchSituation, _objNestedQuery);
                    }

                    var searchresult = _cricketS2.MapcricketS1datascopy(searchResults.ToList(), _objMatchDetail);
                    _objresult = searchresult;

                }
                _objSearchResults.ResultData = _objresult.ToList();
                _objSearchResults.ResultData = _objSearchResults.ResultData.OrderByDescending(x => x.MatchDate).ThenBy(y => y.MarkIn).ToList();
                _objSearchResults.ResultCount = _objDicSearchResultCount;
                result = JsonConvert.SerializeObject(_objSearchResults);
            }
            return result;
        }



        #region "commented code for searchresultcount S2"  

        //public string GetSearchResultCount(SearchRequestData _objReqData)
        //{
        //    string result = string.Empty;
        //    try
        //    {


        //        Dictionary<string, object> ddlDropdowns = new Dictionary<string, object>();
        //        SearchCricketExtendedResultData _objSearchResults = new SearchCricketExtendedResultData();
        //        string response = string.Empty;
        //        int sportid = 1;
        //        string languageid = "1";
        //        _objSearchResults.ResultDerivedData = new CricketDerivedData();
        //        _objSearchResults.ResultDerivedData.RangeData = new Dictionary<string, long>();
        //        QueryContainer _objNestedQuery = new QueryContainer();
        //        QueryContainer _objNestedDropdownQuery = new QueryContainer();
        //        _objMatchDetail = _objReqData.MatchDetails.FirstOrDefault();
        //        _objPlayerDetails = _objReqData.PlayerDetails.FirstOrDefault();
        //        string objDropdown = _con.GetKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID), "S2Dropdowns");
        //        dynamic _objS2Data = _objPlayerDetails;
        //        _objSearchResults.ResultCount = new Dictionary<string, Int64>();
        //        _objSearchResults.ResultData = new List<SearchCricketResultData>();

        //        MatchSituation _objMatchSituation = _objReqData.MatchSituations.FirstOrDefault();
        //        List<string> valueObj = _con.LstKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID).ToLower() + "S2", "PlayerDetails");
        //        string S2DataObj1 = _con.GetKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID).ToLower(), "S2DataCount");
        //        string S2DataObj2 = _con.GetKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID).ToLower(), "S2DataMaxCount");
        //        if (_objMatchDetail != null)
        //        {
        //            EsClient_obj = _oLayer.CreateConnection();
        //            sportid = _objMatchDetail.SportID;
        //            languageid = _objMatchDetail.LanguageId;
        //            _objNestedQuery = _cricketS2.GetMatchDetailQuery(_objNestedQuery, _objMatchDetail);
        //            if (_objPlayerDetails != null)
        //            {
        //                _objNestedQuery = _cricketS2.GetPlayerDetails(_objS2Data, _objNestedQuery, valueObj, sportid);//new
        //                if (_objMatchSituation != null)
        //                {
        //                    _objNestedQuery = _cricketS2.GetCricketMatchSituationQuery(_objMatchDetail, _objPlayerDetails, _objMatchSituation, _objNestedQuery);//new
        //                }
        //            }
        //            string[] arrayS2Count = S2DataObj1.Split(",");
        //            string[] arrayS2Max = S2DataObj2.Split(",");
        //            foreach (var items in arrayS2Count)
        //            {
        //                _objSearchResults.ResultCount.Add(items, _cricketS2.getMatchCount(_objNestedQuery, EsClient_obj, items));
        //            }
        //            foreach (var items in arrayS2Max)
        //            {
        //                _objSearchResults.ResultDerivedData.RangeData.Add(items, _cricketS2.GetPlayerMatchDetailsMaxCount(_objNestedQuery, EsClient_obj, items));
        //            }
        //            _objSearchResults.ResultCount.Add("RequestCount", _objReqData.MatchDetails[0].RequestCount);
        //            if (!string.IsNullOrEmpty(_objMatchDetail.MatchDate) || _objMatchSituation != null)
        //            {
        //                _objNestedQuery = FinalSearchCricketData(_objMatchDetail, _objMatchSituation, _objNestedQuery);
        //            }
        //            IEnumerable<SearchCricketResultTempData> searchResults = new List<SearchCricketResultTempData>();
        //            IEnumerable<SearchCricketResultData> _objresult = new List<SearchCricketResultData>();
        //            searchResults = _cricketS2.SearchS1(_objNestedQuery, EsClient_obj);
        //            //_ob_objresultjresult = 
        //            var searchresult = _cricketS2.MapcricketS1datascopy(searchResults.ToList(), _objMatchDetail);
        //            _objSearchResults.ResultData = _objresult != null ? _objresult : null;
        //            // _objSearchResults.ResultData = _objresult != null ? _objresult : null;

        //            if (_objReqData.PlayerDetails != null && _objPlayerDetails.IsDefault)
        //            {

        //                MatchSituation _objMatchSituations = _objReqData.MatchSituations.FirstOrDefault();

        //                if (!_objMatchDetail.IsAsset && _objMatchDetail.MasterData)
        //                {
        //                    _objSearchResults.ResultDerivedData.MasterData = new Dictionary<string, object>();

        //                    if (_objSearchResults.ResultData.ToList().Count > 0)
        //                    {
        //                        ddlDropdowns = _cricketS2.bindS1andS2Dropdown(_objPlayerDetails);

        //                        if (objDropdown != null)
        //                        {
        //                            string[] valuess = objDropdown.Split(",");

        //                            foreach (var items in valuess)
        //                            {
        //                                var item = items.Split("::");

        //                                foreach (KeyValuePair<string, object> entry in ddlDropdowns)
        //                                {
        //                                    if (item.ToString().Split(",")[0] != entry.Key.ToString())
        //                                    {
        //                                        QueryContainer query = new TermQuery { Field = entry.Key, Value = entry.Value };
        //                                        _objNestedDropdownQuery &= query;
        //                                    }

        //                                }

        //                                var Type = ddlDropdowns[item.ToString().Split(",")[0]].ToString();
        //                                string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
        //                                _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "cricket", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType);
        //                            }
        //                        }



        //                    }
        //                }
        //            }
        //            else if (_objReqData.PlayerDetails != null && !_objPlayerDetails.IsDefault)
        //            {
        //                MatchSituation _objMatchSituations = _objReqData.MatchSituations.FirstOrDefault();
        //                if (!_objMatchDetail.IsAsset)
        //                {
        //                    _objSearchResults.ResultDerivedData.MasterData = new Dictionary<string, object>();
        //                    string currentselector = _objPlayerDetails.CurrentSelector;
        //                    _objNestedQuery = _cricketS2.GetMatchDetailQuery(_objNestedQuery, _objMatchDetail, true);//new 
        //                    _objNestedQuery = _cricketS2.GetPlayerDetails(_objS2Data, _objNestedQuery, valueObj, sportid, true);
        //                    _objNestedQuery = _cricketS2.GetCricketMatchSituationQuery(_objMatchDetail, _objPlayerDetails, _objMatchSituation, _objNestedQuery);//new
        //                    IEnumerable<SearchS1CricketMasterData> _objSearchS1MasterData = new List<SearchS1CricketMasterData>();
        //                    //_objSearchS1MasterData = _cricketS2.SearchS1MasterData(_objNestedQuery, EsClient_obj);

        //                    ddlDropdowns = _cricketS2.bindS1andS2Dropdown(_objPlayerDetails);

        //                    if (objDropdown != null)
        //                    {
        //                        string[] valuess = objDropdown.Split(",");

        //                        foreach (var items in valuess)
        //                        {
        //                            var item = items.Split("::");
        //                            if (ddlDropdowns.Count > 0)
        //                            {
        //                                foreach (KeyValuePair<string, object> entry in ddlDropdowns)
        //                                {
        //                                    if (item.ToString().Split(",")[0] != entry.Key.ToString())
        //                                    {
        //                                        QueryContainer query = new TermQuery { Field = entry.Key, Value = entry.Value };
        //                                        _objNestedDropdownQuery &= query;
        //                                    }

        //                                }
        //                            }

        //                            if (ddlDropdowns.Count > 0)
        //                            {
        //                                var Type = ddlDropdowns[item.ToString().Split(",")[0]].ToString();
        //                                string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
        //                                _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "cricket", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType);

        //                            }
        //                            else
        //                            {
        //                                //var Type = ddlDropdowns[item.ToString().Split(",")[0]].ToString();
        //                                string[] _objType = new string[] { "0" };
        //                                _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "cricket", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType);

        //                            }
        //                        }
        //                    }



        //                }
        //            }

        //            //_objSearchResults.ResultData = null;
        //            string jsonData = JsonConvert.SerializeObject(_objSearchResults);
        //            result = jsonData;
        //            //string JSONstring = JsonConvert.SerializeObject<List<SearchRequestData>>(_objSearchResults);
        //            //result = _objSerializer.Serialize(_objSearchResults);
        //            //_objSerializer = null;
        //            _objSearchResults.ResultCount = null;
        //            _objSearchResults.ResultDerivedData = null;


        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return result;
        //}

        #endregion

        public string GetSearchResultCountForKabaddi(KabaddiRequestData _objReqData, bool forCount = true)
        {
            KabaddiRequestData _objEntityReqDatakabaddi = new KabaddiRequestData();
            ELModels.KabaddiPlayerDetail _objPlayerDetailskabaddi = new ELModels.KabaddiPlayerDetail();
            string result = string.Empty;
            try
            {
                bool checkIfCascade = true;
                string strSeacrchType = "";
                Dictionary<string, object> ddlDropdowns = new Dictionary<string, object>();
                SearchResultsExtendedData _objSearchResults = new SearchResultsExtendedData();
                string response = string.Empty;
                int sportid = 3;
                string languageid = "1";
                _objSearchResults.ResultDerivedData = new SearchResultsDerivedData();
                _objSearchResults.ResultDerivedData.RangeData = new Dictionary<string, long>();
                QueryContainer _objNestedQuery = new QueryContainer();
                QueryContainer _objNestedDropdownQuery = new QueryContainer();

                string jsonData = JsonConvert.SerializeObject(_objReqData);
                _objEntityReqDatakabaddi = JsonConvert.DeserializeObject<KabaddiRequestData>(jsonData);
                _objMatchDetail = _objEntityReqDatakabaddi.MatchDetails.FirstOrDefault();
                _objPlayerDetailskabaddi = _objEntityReqDatakabaddi.PlayerDetails.FirstOrDefault();

                List<string> valueObj = _con.LstKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID).ToLower() + "S2", "PlayerDetails");
                string S2DataObj1 = _con.GetKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID).ToLower(), "S2DataCount");
                dynamic _objS2Data = _objPlayerDetailskabaddi;
                _objSearchResults.ResultCount = new Dictionary<string, Int64>();
                _objSearchResults.ResultData = new List<SearchKabaddiData>();

                string objDropdown = _con.GetKeyValueAppSetting(objCF.getType(_objMatchDetail.SportID), "S2Dropdowns");
                if (_objMatchDetail != null)
                {
                    EsClient_obj = _oLayer.CreateConnection();
                    sportid = _objMatchDetail.SportID;
                    languageid = _objMatchDetail.LanguageId;
                    _objNestedQuery = _kabaddiS2.GetMatchDetailQuery(_objNestedQuery, _objMatchDetail);
                    string input = "";
                    input = Convert.ToInt32(Convert.ToBoolean(_objMatchDetail.IsAsset)).ToString();
                    if (_objPlayerDetailskabaddi != null)
                    {
                        _objNestedQuery = _kabaddiS2.GetPlayerDetails(_objS2Data, _objNestedQuery, valueObj, sportid, _objMatchDetail.MasterData);//new
                        if (_objMatchSituation != null)
                        {
                            //_objNestedQuery = _kabaddiS2.GetCricketMatchSituationQuery(_objMatchDetail, _objPlayerDetails, _objMatchSituation, _objNestedQuery);//new
                        }
                    }
                    string[] arrayS2Count = S2DataObj1.Split(",");
                    foreach (var items in arrayS2Count)
                    {
                        _objSearchResults.ResultCount.Add(items, _kabaddiS2.getMatchCount(_objNestedQuery, EsClient_obj, items));
                    }
                    _objSearchResults.ResultCount.Add("RequestCount", _objMatchDetail.RequestCount);
                    if (!string.IsNullOrEmpty(_objMatchDetail.MatchDate) || _objMatchSituation != null)
                    {
                        _objNestedQuery = FinalSearchKabaddiData(_objMatchDetail, _objMatchSituation, _objNestedQuery);
                    }
                    QueryContainer qcIstagged = new TermQuery { Field = "isAsset", Value = input };
                    _objNestedQuery &= qcIstagged;
                    IEnumerable<KabaddiResultDataTempdata> searchResults = new List<KabaddiResultDataTempdata>();
                    IEnumerable<SearchResultsExtendedData> _objresult = new List<SearchResultsExtendedData>();
                    searchResults = _kabaddiS2.SearchS1(_objNestedQuery, EsClient_obj);
                    var searchresult = _kabaddiS2.MapkabaddiS1dataCopy(searchResults.ToList(), _objMatchDetail);
                    _objSearchResults.ResultData = _objresult != null ? _objresult : null;

                    if (!forCount)
                    {
                        _objSearchResults.ResultData = searchResults.ToList();
                    }

                    if (_objPlayerDetailskabaddi != null && _objPlayerDetailskabaddi.IsDefault)
                    {

                        //MatchSituation _objMatchSituations = _objReqData.MatchSituations.FirstOrDefault();

                        if (!_objMatchDetail.IsAsset && _objMatchDetail.MasterData)
                        {
                            _objSearchResults.ResultDerivedData.MasterData = new Dictionary<string, object>();

                            if (_objSearchResults.ResultData.ToList().Count > 0)
                            {
                                ddlDropdowns = _kabaddiS2.bindS1andS2Dropdown(_objPlayerDetailskabaddi);

                                if (objDropdown != null)
                                {
                                    string[] valuess = objDropdown.Split(",");

                                    //foreach (var items in valuess)
                                    //{
                                    //    var item = items.Split("::");

                                    //    foreach (KeyValuePair<string, object> entry in ddlDropdowns)
                                    //    {
                                    //        if (item.ToString().Split(",")[0] != entry.Key.ToString())
                                    //        {
                                    //            QueryContainer query = new TermQuery { Field = entry.Key, Value = entry.Value };
                                    //            _objNestedDropdownQuery &= query;
                                    //        }

                                    //    }

                                    //    var Type = ddlDropdowns[item.ToString().Split(",")[0]].ToString();
                                    //    string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
                                    //    _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "cricket", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType);
                                    //}
                                    foreach (var items in valuess)
                                    {
                                        var item = items.Split("::");
                                        foreach (KeyValuePair<string, object> itemss in ddlDropdowns)
                                        {
                                            string strValue = objCF.ConvertStringArrayToString(item);
                                            var Type = strValue.Split(",")[0].ToString().ToString();
                                            if (Type != itemss.Key.ToString())
                                            {

                                                string slist = itemss.Value.ToString();
                                                if (slist.Contains(","))
                                                {
                                                    string[] strValues = slist.Split(',');
                                                    foreach (string str in strValues)
                                                    {
                                                        QueryContainer query1 = new TermQuery { Field = itemss.Key, Value = str };
                                                        _objNestedDropdownQuery &= query1;
                                                    }
                                                }
                                                else
                                                {
                                                    QueryContainer query = new TermQuery { Field = itemss.Key, Value = itemss.Value };
                                                    _objNestedDropdownQuery &= query;
                                                }

                                            }
                                            else
                                            {
                                                checkIfCascade = false;
                                            }

                                        }
                                        if (!checkIfCascade)
                                        {
                                            string[] itemsArray = item;
                                            string strValue = objCF.ConvertStringArrayToString(itemsArray);
                                            var Type = ddlDropdowns[strValue.Split(",")[0].ToString()].ToString();
                                            string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
                                            _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "kabaddi", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType, _objMatchDetail.SportID);

                                        }
                                        else
                                        {
                                            //var Type = ddlDropdowns[item.ToString().Split(",")[0]].ToString();
                                            //string[] _objType = new string[] { "0" };
                                            string[] _objType = new string[] { "" };
                                            _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "kabaddi", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType, _objMatchDetail.SportID);

                                        }
                                        checkIfCascade = true;
                                    }
                                }

                                /*if (objDropdown != null)
                                {
                                    string[] valuess = objDropdown.Split(",");

                                    foreach (var items in valuess)
                                    {
                                        var item = items.Split("::");

                                        foreach (KeyValuePair<string, object> entry in ddlDropdowns)
                                        {
                                            if (item.ToString().Split(",")[0] != entry.Key.ToString())
                                            {
                                                QueryContainer query = new TermQuery { Field = entry.Key, Value = entry.Value };
                                                _objNestedDropdownQuery &= query;
                                            }

                                        }

                                        var Type = ddlDropdowns[item.ToString().Split(",")[0]].ToString();
                                        string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
                                        _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "kabaddi", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType, _objMatchDetail.SportID);
                                    }
                                }*/
                            }
                        }
                    }
                    else if (_objPlayerDetailskabaddi != null && !_objPlayerDetailskabaddi.IsDefault)
                    {
                        //MatchSituation _objMatchSituations = _objReqData.MatchSituations.FirstOrDefault();
                        if (!_objMatchDetail.IsAsset)
                        {
                            _objSearchResults.ResultDerivedData.MasterData = new Dictionary<string, object>();
                            string currentselector = _objPlayerDetailskabaddi.CurrentSelector;
                            _objNestedQuery = null;
                            _objNestedQuery = _kabaddiS2.GetMatchDetailQuery(_objNestedQuery, _objMatchDetail, true);//new 
                            _objNestedQuery = _kabaddiS2.GetPlayerDetails(_objS2Data, _objNestedQuery, valueObj, sportid, true);

                            ////_objSearchResults.ResultDerivedData.MasterData = new Dictionary<string, object>();
                            ////string currentselector = _objPlayerDetailskabaddi.CurrentSelector;
                            ////_objNestedQuery = _kabaddiS2.GetMatchDetailQuery(_objNestedQuery, _objMatchDetail, true);//new 
                            ////_objNestedQuery = _kabaddiS2.GetPlayerDetails(_objS2Data, _objNestedQuery, valueObj, sportid, true);
                            // _objNestedQuery = _cricketS2.GetCricketMatchSituationQuery(_objMatchDetail, _objPlayerDetails, _objMatchSituation, _objNestedQuery);//new
                            IEnumerable<SearchS1CricketMasterData> _objSearchS1MasterData = new List<SearchS1CricketMasterData>();
                            //_objSearchS1MasterData = _cricketS2.SearchS1MasterData(_objNestedQuery, EsClient_obj);

                            ddlDropdowns = _kabaddiS2.bindS1andS2Dropdown(_objPlayerDetailskabaddi);

                            if (objDropdown != null)
                            {
                                string[] valuess = objDropdown.Split(",");

                                foreach (var items in valuess)
                                {
                                    var item = items.Split("::");
                                    foreach (KeyValuePair<string, object> itemss in ddlDropdowns)
                                    {
                                        string strValue = objCF.ConvertStringArrayToString(item);
                                        var Type = strValue.Split(",")[0].ToString().ToString();
                                        if (Type != itemss.Key.ToString())
                                        {
                                            string slist = itemss.Value.ToString();
                                            if (slist.Contains(","))
                                            {
                                                string[] strValues = slist.Split(',');
                                                foreach (string str in strValues)
                                                {
                                                    QueryContainer query1 = new TermQuery { Field = itemss.Key, Value = str };
                                                    // _objNestedDropdownQuery &= query1;
                                                    _objNestedQuery &= query1;
                                                }
                                            }
                                            else
                                            {
                                                QueryContainer query = new TermQuery { Field = itemss.Key, Value = itemss.Value };
                                                //_objNestedDropdownQuery &= query;
                                                _objNestedQuery &= query;
                                            }
                                        }
                                        else
                                        {
                                            checkIfCascade = false;
                                        }
                                    }

                                    if (!checkIfCascade)
                                    {
                                        string[] itemsArray = item;
                                        string strValue = objCF.ConvertStringArrayToString(itemsArray);
                                        var Type = ddlDropdowns[strValue.Split(",")[0].ToString()].ToString();
                                        string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
                                        _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "kabaddi", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType, _objMatchDetail.SportID);

                                    }
                                    else
                                    {
                                        //var Type = ddlDropdowns[item.ToString().Split(",")[0]].ToString();
                                        //string[] _objType = new string[] { "0" };

                                        string[] _objType = new string[] { "" };
                                        _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "kabaddi", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType, _objMatchDetail.SportID);

                                    }
                                    checkIfCascade = true;
                                }
                            }

                            //if (objDropdown != null)
                            //{
                            //    string[] valuess = objDropdown.Split(",");

                            //    foreach (var items in valuess)
                            //    {
                            //        var item = items.Split("::");
                            //        if (ddlDropdowns.Count > 0)
                            //        {
                            //            foreach (KeyValuePair<string, object> entry in ddlDropdowns)
                            //            {
                            //                if (item.ToString().Split(",")[0] != entry.Key.ToString())
                            //                {
                            //                    QueryContainer query = new TermQuery { Field = entry.Key, Value = entry.Value };
                            //                    _objNestedDropdownQuery &= query;
                            //                }

                            //            }
                            //        }

                            //        if (ddlDropdowns.Count > 0)
                            //        {
                            //            if (ddlDropdowns.Keys.Contains(item[0]))
                            //            {
                            //                //var t = ddlDropdowns.Values.ToList()[0];
                            //                //string s1 = string.Join(",", ddlDropdowns.Values.ToList()[0]);
                            //                string[] resultArray = ddlDropdowns.Values.ToList()[0] as string[];
                            //                //var Type = ddlDropdowns[item[0]].ToString();
                            //                //_objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
                            //                _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "kabaddi", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), resultArray, _objMatchDetail.SportID);
                            //            }
                            //            else
                            //            {
                            //                string[] _objType = new string[] { "0" };
                            //                _objSearchResults.ResultDerivedData.MasterData = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "kabaddi", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType, _objMatchDetail.SportID);
                            //            }
                            //        }
                            //        else
                            //        {
                            //            //var Type = ddlDropdowns[item.ToString().Split(",")[0]].ToString();
                            //            string[] _objType = new string[] { "0" };
                            //            var data = objCF.GetDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "kabaddi", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType, _objMatchDetail.SportID);

                            //            _objSearchResults.ResultDerivedData.MasterData = data;
                            //        }
                            //    }
                            //}
                        }
                    }

                    //_objSearchResults.ResultData = null;
                    string jsonDataresult = JsonConvert.SerializeObject(_objSearchResults);
                    result = jsonDataresult;
                    //string JSONstring = JsonConvert.SerializeObject<List<SearchRequestData>>(_objSearchResults);
                    //result = _objSerializer.Serialize(_objSearchResults);
                    //_objSerializer = null;
                    //_objSearchResults.ResultCount = null;
                    //_objSearchResults.ResultDerivedData = null;
                }
            }
            catch (Exception e)
            {

            }
            return result;
        }

        public string GetSearchResultsForKabaddi(KabaddiRequestData _objReqData, bool forCount = false)
        {
            return GetSearchResultCountForKabaddi(_objReqData, false);
        }
    }
}
