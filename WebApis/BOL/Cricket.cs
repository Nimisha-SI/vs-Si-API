using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using WebApis.elastic;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
    //private static ElasticClient EsClient;
   
    public class Cricket : AbstractClasses , IDdlForSpecificMatch, ICricket
    {
        private ESInterface _oLayer;
      public Cricket(ESInterface oLayer) {
            _oLayer = oLayer;
        }
        public Dictionary<string, object> getDropDownForMatch(Dictionary<string, object> ObjectArray, string[] sInnings)
        {
            List<FilteredEntityData> obj2 = new List<FilteredEntityData>();
            for (int i = 1; i <= 2; i++)
            {
                obj2.Add(new FilteredEntityData
                {
                    EntityId = i.ToString(),
                    EntityName = i.ToString(),
                    IsSelectedEntity = sInnings.Contains(i.ToString()) ? 1 : 0
                });
            }
            ObjectArray.Add("Innings", obj2.AsEnumerable());
            return ObjectArray;
        }

        public override IEnumerable<ELModels.SearchResultFilterData> returnSportResult(ElasticClient EsClient, QueryContainer _objNestedQuery, string IndexName)
        {
            EsClient = _oLayer.CreateConnection();
         
            IEnumerable<SearchResultFilterData> _objSearchResultFilterData = new List<SearchResultFilterData>();
            var result = EsClient.Search<SearchCricketData>(s => s.Index(IndexName).Query(q => _objNestedQuery).Sort(q=>q.Ascending(u=>u.Id.Suffix("keyword"))).Size(802407));
            _objSearchResultFilterData = SearchResultFilterDataMap(result);
            return _objSearchResultFilterData;
        }

        public override List<SearchResultFilterData> SearchResultFilterDataMap(ISearchResponse<SearchCricketData> result)
        {
            List<SearchResultFilterData> ListObj = new List<SearchResultFilterData>();
            foreach (var hit in result.Hits)
            {
                ListObj.Add(new SearchResultFilterData
                {
                    ClearId = hit.Source.ClearId.ToString(),
                    Description = hit.Source.Description.ToString(),
                    MarkIn = hit.Source.MarkIn.ToString(),
                    MarkOut = hit.Source.MarkOut.ToString(),
                    ShotType = hit.Source.MarkOut.ToString(),
                    Duration = hit.Source.Duration.ToString(),
                    DeliveryType = hit.Source.DeliveryType.ToString(),
                    DeliveryTypeId = hit.Source.DeliveryTypeId.ToString(),
                    EventId = hit.Source.EventId.ToString(),
                    EventName = hit.Source.EventText.ToString(),
                    Id = hit.Source.Id.ToString(),
                    MatchId = hit.Source.MatchId.ToString(),
                    MediaId = hit.Source.MediaId.ToString(),
                    Title = hit.Source.Title.ToString(),
                    ShotTypeId = hit.Source.ShotTypeId.ToString(),
                });
            }
            return ListObj;
        }

        public override QueryContainer GetPlayerDetails(dynamic _objS1Data, QueryContainer qFinal, List<string> valueObj, int sportid, bool isMasterData = false)
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
                    for (int i = 0; i <= isDefaultvalues.Length - 1; i++)
                    {
                        if (isDefaultvalues[i] != "shotTypeId" && isDefaultvalues[i] != "deliveryTypeId")
                        {
                            QueryContainer query1 = new TermQuery { Field = isDefaultvalues[i], Value = 1 };
                            queryShouldB |= query1;
                        }

                    }
                    //qFinal &= queryShould;
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
                            
                        }
                        if (sType == "string")
                        {
                            if (Convert.ToString(_objS1Data[valueObj[i].Split(":")[1]]) != "")
                            {
                                string slist = Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]);
                                if (slist.Contains(","))
                                {

                                        string[] strArray = slist.Split(',');
                                        foreach (string str in strArray)
                                        {
                                            QueryContainer query9 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = str };
                                            queryShouldS |= query9;
                                        }
                                }
                                else
                                {
                                        if (Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]) != "")
                                        {
                                            QueryContainer query10 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]) };
                                            qFinal &= query10;
                                        }
                                }
                            }

                        }
                     }
                }
            }
           
            qFinal &= queryShouldB;
            return qFinal;
           }

        public override Dictionary<string, object> bindS1andS2Dropdown(dynamic _objS1Data)
        {
            Dictionary<string, object> ddlS1Dropwons = new Dictionary<string, object>();
            string ReqShotType = _objS1Data["ShotType"]; string ReqDeliveryType = _objS1Data["DeliveryType"];
            string[] _objReqShotType = ReqShotType.Contains(",") ? _objReqShotType = ReqShotType.Split(",") : _objReqShotType = new string[] { _objS1Data["ShotType"] };
            string[] _objReqDeliveryType = ReqDeliveryType.Contains(",") ? _objReqDeliveryType = ReqDeliveryType.Split(",") : _objReqDeliveryType = new string[] { _objS1Data["DeliveryType"] };

            if (_objS1Data["ShotType"] != "")
            {
                ddlS1Dropwons.Add("shotTypeId", _objReqShotType[0].ToString());
            }
            if (_objS1Data["DeliveryType"] != "")
            {
                ddlS1Dropwons.Add("deliveryTypeId", _objReqDeliveryType[0].ToString());
            }
            return ddlS1Dropwons;

        }

        public override QueryContainer GetMatchDetailQuery(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, bool isMasterData = false)
        {
            
            if (_objMatchDetail != null)
            {
                QueryContainer queryShould = new QueryContainer();
                if (!string.IsNullOrEmpty(_objMatchDetail.MatchFormat))
                {
                    QueryContainer q1 = new TermQuery { Field = "compType", Value = _objMatchDetail.MatchFormat.ToLower() };

                    _objNestedQuery &= q1;
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.VenueId))
                {
                    QueryContainer q2 = new TermQuery { Field = "venueId", Value = _objMatchDetail.MatchFormat.ToLower().ToString() };
                    _objNestedQuery &= q2;
                }

                if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id))
                {
                    var myQuery = new BoolQuery();
                    QueryContainer q3 = new TermQuery { Field = "team1Id", Value = _objMatchDetail.Team1Id.ToLower().ToString() };
                    QueryContainer q4 = new TermQuery { Field = "team2Id", Value = _objMatchDetail.Team1Id.ToLower().ToString() };
                    queryShould |= q3 |= q4;
                    _objNestedQuery &= queryShould;
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                {

                    QueryContainer q5 = new TermQuery { Field = "team1Id", Value = _objMatchDetail.Team2Id.ToLower().ToString() };
                    QueryContainer q6 = new TermQuery { Field = "team2Id", Value = _objMatchDetail.Team2Id.ToLower().ToString() };
                    queryShould |= q5 |= q6;
                    _objNestedQuery &= queryShould;
                }

                if (!string.IsNullOrEmpty(_objMatchDetail.SeriesId) && _objMatchDetail.IsParentSeries)
                {
                    QueryContainer q7 = new TermQuery { Field = "parentSeriesId", Value = _objMatchDetail.SeriesId };
                    _objNestedQuery &= q7;

                }
                if (!string.IsNullOrEmpty(_objMatchDetail.SeriesId) && !_objMatchDetail.IsParentSeries)
                {
                    QueryContainer q8 = new TermQuery { Field = "seriesId", Value = _objMatchDetail.SeriesId };
                    _objNestedQuery &= q8;
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.MatchId))
                {
                    QueryContainer q9 = new TermQuery { Field = "matchId", Value = _objMatchDetail.MatchId };
                    _objNestedQuery &= q9;
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.ClearId))
                {
                    var myQuery = new BoolQuery();
                    QueryContainer q10 = new TermQuery { Field = "qClearId", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
                    QueryContainer q11 = new TermQuery { Field = "qClearId2", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
                    QueryContainer q12 = new TermQuery { Field = "qClearId3", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
                    QueryContainer q13 = new TermQuery { Field = "qClearId4", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
                    QueryContainer q14 = new TermQuery { Field = "qClearId5", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
                    QueryContainer q15 = new TermQuery { Field = "qClearId6", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
                    queryShould |= q10 |= q11 |= q12 |= q13 |= q14 |= q15;
                    _objNestedQuery &= queryShould;

                }
                if (!string.IsNullOrEmpty(_objMatchDetail.MatchStageId))
                {

                    var myQuery = new BoolQuery();
                    string Dlist = Convert.ToString(_objMatchDetail.MatchStageId);
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q16 = new TermQuery { Field = "matchStageId", Value = str };
                        queryShould |= q16;

                    }
                    _objNestedQuery &= queryShould;

                }
                if (!string.IsNullOrEmpty(_objMatchDetail.CompTypeId))
                {
                    var myQuery = new BoolQuery();
                    string Dlist = Convert.ToString(_objMatchDetail.CompTypeId);
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q17 = new TermQuery { Field = "CompTypeId", Value = str };
                        queryShould |= q17;
                    }
                    _objNestedQuery &= queryShould;

                }


                if (_objMatchDetail.HasShortClip)
                {
                    QueryContainer q21 = new TermQuery { Field = "hasShortClip", Value = "1" };
                    _objNestedQuery &= q21;
                }

                if (!_objMatchDetail.IsAssetSearch)
                {
                    QueryContainer q20 = new TermQuery { Field = "isTagged", Value = "1" };
                    _objNestedQuery &= q20;

                }

                if (!string.IsNullOrEmpty(_objMatchDetail.LanguageId) && _objMatchDetail.LanguageId != "0")
                {
                    QueryContainer q19 = new TermQuery { Field = "languageId", Value = _objMatchDetail.LanguageId };
                    _objNestedQuery &= q19;

                }
                string input = Convert.ToInt32(Convert.ToBoolean(_objMatchDetail.IsAsset)).ToString();
                QueryContainer q18 = new TermQuery { Field = "isAsset", Value = input };
                _objNestedQuery &= q18;

            }
            return _objNestedQuery;
        }

        public QueryContainer GetCricketMatchSituationQueryST(QueryContainer _objNestedQuery, MatchSituation _objMatchSituation)
        {
            if (_objMatchSituation != null)
            {
                if (!string.IsNullOrEmpty(_objMatchSituation.Innings))
                {
                    List<QueryContainer> myDynamicTermQuery = new List<QueryContainer>();
                    QueryContainer q1 = new TermQuery { Field = "innings", Value = Convert.ToString(_objMatchSituation.Innings) };
                    _objNestedQuery &= q1;


                }

            }
            return _objNestedQuery;
        }

        public QueryContainer GetPlayerDetailQueryForFilteredEntityBySport(QueryContainer _objNestedQuery, dynamic _objS1Data, int SportsId = 1)
        {
            QueryContainer qShould = new QueryContainer();
            if (Convert.ToString(_objS1Data["BatsmanID"]) != "")
            {
                QueryContainer q1 = new TermQuery { Field = "batsmanId", Value = _objS1Data["BatsmanID"] };

                _objNestedQuery &= q1;
            }
            if (Convert.ToString(_objS1Data["BowlerID"]) != null)
            {
                QueryContainer q2 = new TermQuery { Field = "bowlerId", Value = _objS1Data["BowlerID"] };
                _objNestedQuery &= q2;

            }
            if (Convert.ToString(_objS1Data["FielderID"]) != null)
            {
                QueryContainer q3 = new TermQuery { Field = "fielderId", Value = _objS1Data["fielderId"] };
                _objNestedQuery &= q3;

            }
            return _objNestedQuery;

        }

        public QueryContainer GetFilteredEntitiesBySport(MatchDetail _objReqData, QueryContainer _objNestedQuery, string sCase, int sDate, Dictionary<string, string> _columns, string searchText, string Edate ="")
        {
            dynamic Result = null;
            string input = Convert.ToInt32(Convert.ToBoolean(_objReqData.IsAsset)).ToString();
            List<string> EntityId = new List<string>();
            List<string> EntityName = new List<string>();
            foreach (var col in _columns)
            {
                EntityId.Add(col.Key);
                EntityName.Add(col.Value);

            }
            var terms = searchText.Trim().Replace("-", " ").Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            searchText = string.Join(" ", terms).ToLower();

            if (searchText != "")
            {
                QueryContainer query = new QueryContainer();
                if (EntityName.ElementAt(0) != "CompType" && EntityName.ElementAt(0) != "MatchStage")
                {
                    for (int i = 0; i < _columns.Count; i++)
                    {
                        QueryContainer q2 = new TermQuery { Field = EntityName.ElementAt(i), Value = searchText };
                        query |= q2;
                    }
                    _objNestedQuery &= query;
                    //Bq.Add(bq, Occur.MUST);
                }
                QueryContainer q3 = new TermQuery { Field = "isAsset", Value = searchText };
                _objNestedQuery &= q3;
                if (sCase == "2")
                {
                    QueryContainer q4 = new TermQuery { Field = "matchDate", Value = sDate };
                    _objNestedQuery &= q4;
                }
                else if (sCase == "3") {
                    QueryContainer q4 = new TermRangeQuery { Field = "matchDate", GreaterThanOrEqualTo = sDate.ToString(), LessThanOrEqualTo = Edate.ToString() };
                    _objNestedQuery &= q4;
                }
            }
            return _objNestedQuery;
        }

        public QueryContainer GetEntityBySport(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, Dictionary<string, string> _columns, string searchtext)
        {
            string dlist = _objMatchDetail.MatchDate;
            if (dlist.Contains("-"))
            {
                string[] strNumbers = dlist.Split('-');
                int start = int.Parse(DateTime.ParseExact(strNumbers[0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                int End = int.Parse(DateTime.ParseExact(strNumbers[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                _objNestedQuery = GetFilteredEntitiesBySport(_objMatchDetail, _objNestedQuery, "1", start, _columns, searchtext,End.ToString());
            }
            else
            {
                int date = int.Parse(DateTime.ParseExact(_objMatchDetail.MatchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                _objNestedQuery = GetFilteredEntitiesBySport(_objMatchDetail, _objNestedQuery, "2", date, _columns, searchtext);
            }
            return _objNestedQuery;

        }

        public List<FilteredEntityForCricket> GetFilteredEntitiesBySportResult(QueryContainer qc, string EntityId, string EntityName, ElasticClient EsClient, string searchText, int sDate = 0, int Edate = 0)
        {
            List<FilteredEntityForCricket> obj = new List<FilteredEntityForCricket>();
            string eName = "", eId = "";
            //if (EntityName == "Bowler")
            //{
            //    eName = "bowler";
            //}
            //if (EntityId == "BowlerId")
            //{
            //    eId = "bowlerId";
            //}
            //if (EntityName == "Batsman")
            //{
            //    eName = "batsman";
            //}
            //if (EntityId == "BatsmanId")
            //{
            //    eId = "batsmanId";
            //}
            var terms = searchText.Trim().Replace("-", " ").Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            searchText = string.Join(" ", terms).ToLower();

            if (sDate != 0 && Edate != 0)
            {
                var response = EsClient.Search<SearchCricketData>(s => s.Index("cricket").Size(0).
                Query(q => qc && q.Range(v => v.Field(p => p.MatchDate).GreaterThanOrEquals(sDate).LessThanOrEquals(Edate)))
                .Aggregations(a => a.Terms("my_agg", st => st.Script(p => p.Source("doc['" + EntityId + ".keyword'].value + '|' + doc['" + EntityName + ".keyword'].value")).Size(802407))));
                var agg = response.Aggregations.Terms("my_agg").Buckets;
                foreach (var hits in agg)
                {
                    obj.Add(new FilteredEntityForCricket
                    {
                        EntitTypeName = EntityName,
                        EntityName = hits.Key.ToString().Split("|")[0],
                        EntityId = hits.Key.ToString().Split("|")[1],
                    });
                }


            }
            else
            {
                if (searchText != "")
                {

                    if (EntityName == "bowler")
                    {
                        var response = EsClient.Search<SearchCricketData>(s => s.Index("cricket").Size(0).
                Query(q => qc && q.Wildcard(c => c.Name("named_query").Field(f => f.Bowler).Value(searchText)))
                .Aggregations(a => a.Terms("my_agg", st => st.Script(p => p.Source("doc['" + EntityName + ".keyword'].value + '|' + doc['" + EntityId + ".keyword'].value")).Size(802407))));
                        var agg = response.Aggregations.Terms("my_agg").Buckets;
                        foreach (var hits in agg)
                        {
                            obj.Add(new FilteredEntityForCricket
                            {
                                EntitTypeName = EntityName,
                                EntityName = hits.Key.ToString().Split("|")[0],
                                EntityId = hits.Key.ToString().Split("|")[1],
                            });
                        }
                    }
                    if (eName == "batsman")
                    {
                        var response = EsClient.Search<SearchCricketData>(s => s.Index("cricket").Size(0).
                Query(q => qc && q.Wildcard(c => c.Name("named_query").Field(f => f.Batsman).Value(searchText)))
                .Aggregations(a => a.Terms("my_agg", st => st.Script(p => p.Source("doc['" + EntityName + ".keyword'].value + '|' + doc['" + EntityId + ".keyword'].value")).Size(802407))));
                        var agg = response.Aggregations.Terms("my_agg").Buckets;
                        foreach (var hits in agg)
                        {
                            obj.Add(new FilteredEntityForCricket
                            {
                                EntitTypeName = EntityName,
                                EntityName = hits.Key.ToString().Split("|")[0],
                                EntityId = hits.Key.ToString().Split("|")[1],
                            });
                        }
                    }

                }
                else
                {
                    var response = EsClient.Search<SearchCricketData>(s => s.Index("cricket").Size(0).
                Query(q => qc)
                .Aggregations(a => a.Terms("my_agg", st => st.Script(p => p.Source("doc['" + EntityName + ".keyword'].value + '|' + doc['" + EntityId + ".keyword'].value")).Size(802407))));
                    var agg = response.Aggregations.Terms("my_agg").Buckets;
                    foreach (var hits in agg)
                    {
                        obj.Add(new FilteredEntityForCricket
                        {
                            EntitTypeName = EntityName,
                            EntityName = hits.Key.ToString().Split("|")[0],
                            EntityId = hits.Key.ToString().Split("|")[1],
                        });
                    }
                }
            }
            return obj;


        }

        public override dynamic getFinalResult(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, ElasticClient EsClient, string sportid = "1")
        {
            string input = Convert.ToInt32(Convert.ToBoolean(_objMatchDetail.IsAsset)).ToString();
            QueryContainer query = new TermQuery { Field = "isAsset", Value = input };
            _objNestedQuery &= query;
            dynamic result;
            List<FilteredEntityData> lstsearchresults = new List<FilteredEntityData>();
            var resultsMultiSelectData = EsClient.Search<MatchDetailMultiSelectResulttData>(s => s.Index("cricket").Query(q => _objNestedQuery)
            .Aggregations(a => a.Terms("agg_MultiSelect", t=>t.Field(p=>p.CompType)))
            .Size(802407));
            var response = resultsMultiSelectData.Aggregations.Terms("agg_MultiSelect");
            foreach (var items in response.Buckets)
            {
                lstsearchresults.Add(new FilteredEntityData
                {
                    EntityId = items.Key.ToString(),
                    EntityName = items.Key.ToString()
                });
            }
            result = lstsearchresults;
            return result;
        }

        //public QueryContainer GetFilteredEntitiesBySport(MatchDetail _objReqData, QueryContainer _objNestedQuery, string sCase, int sDate, Dictionary<string, string> _columns, string searchText)
        //{
        //    throw new NotImplementedException();
        //}

        public override dynamic SearchS2(QueryContainer Bq, MatchDetail _objmatch, int Sportid = 6, string search = "")
        {
            throw new NotImplementedException();
        }
    }
}
