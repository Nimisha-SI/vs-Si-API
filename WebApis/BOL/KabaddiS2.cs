﻿using System;
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
    public class KabaddiS2 : AbstractClasses, IKabaddiS2
    {
        private ESInterface _oLayer;
        CommonFunction objCf = new CommonFunction();
        public KabaddiS2(ESInterface oLayer)
        {
            _oLayer = oLayer;
        }

        public override Dictionary<string, object> bindS1andS2Dropdown(dynamic _objS1Data)
        {
            Dictionary<string, object> ddlS2Dropwons = new Dictionary<string, object>();
            string[] _objReqTouchTypes = _objS1Data.TouchTypeId.Contains(",") ? _objReqTouchTypes = _objS1Data.TouchTypeId.Split(',') : _objReqTouchTypes = new string[] { _objS1Data.TouchTypeId };
            string[] _objReqTackleTypes = _objS1Data.TackleTypeId.Contains(",") ? _objReqTackleTypes = _objS1Data.TackleTypeId.Split(',') : _objReqTackleTypes = new string[] { _objS1Data.TackleTypeId };
            string[] _objReqEvents = _objS1Data.EventId.Contains(",") ? _objReqEvents = _objS1Data.EventId.Split(',') : _objReqEvents = new string[] { _objS1Data.EventId };
            string[] _objReqAssistType = _objS1Data.AssistTypeId.Contains(",") ? _objReqAssistType = _objS1Data.AssistTypeId.Split(',') : _objReqAssistType = new string[] { _objS1Data.AssistTypeId };
           
            string[] _objReqAssistType1 = _objS1Data.AssistTypeId != null ? _objS1Data.AssistTypeId.Contains(",") ? _objReqAssistType1 = _objS1Data.AssistTypeId.Split(',') : _objReqAssistType1 = new string[] { _objS1Data.AssistTypeId } : new string[] { "" };
            string[] _objReqAssistType2 = _objS1Data.AssistType2Id != null ? _objS1Data.AssistType2Id.Contains(",") ? _objReqAssistType2 = _objS1Data.AssistType2Id.Split(',') : _objReqAssistType2 = new string[] { _objS1Data.AssistType2Id }: new string[] { "" };
            string[] _objReqNoOfDefenders = _objS1Data.NoOfDefenders != null ? _objS1Data.NoOfDefenders.Contains(",") ? _objReqNoOfDefenders = _objS1Data.NoOfDefenders.Split(',') : _objReqNoOfDefenders = new string[] { _objS1Data.NoOfDefenders }: new string[] { "" };

            string[] _objReqDiscEvents = "9,10,11".Split(',');//Green Card, Yellow Card, Red Card

                //if (!string.IsNullOrEmpty(_objS1Data.TouchTypeId) && _objReqTouchTypes.Length > 0)
                {
                    string touchTypeIds = objCf.ConvertStringArrayToString(_objReqTouchTypes);
                    ddlS2Dropwons.Add("touchTypeId", touchTypeIds);
                }
                //if (!string.IsNullOrEmpty(_objS1Data.TackleTypeId) && _objReqTackleTypes.Length > 0)
                {
                    string tackleTypeIds = objCf.ConvertStringArrayToString(_objReqTackleTypes);
                    ddlS2Dropwons.Add("tackleTypeId", tackleTypeIds);
                }
                //if (!string.IsNullOrEmpty(_objS1Data.EventId) && _objReqEvents.Length > 0)
                {
                    string eventIds = objCf.ConvertStringArrayToString(_objReqEvents);
                    ddlS2Dropwons.Add("eventId", eventIds);
                }
                //if (!string.IsNullOrEmpty(_objS1Data.AssistTypeId) && _objReqAssistType.Length > 0)
                {
                    string assistType1Ids = objCf.ConvertStringArrayToString(_objReqAssistType1);
                    ddlS2Dropwons.Add("assistType1Id", assistType1Ids);
                }
                //if (!string.IsNullOrEmpty(_objS1Data.AssistType2Id) && _objReqAssistType2.Length > 0)
                {
                    string assistType2Ids = objCf.ConvertStringArrayToString(_objReqAssistType2);
                    ddlS2Dropwons.Add("assistType2Id", assistType2Ids);
                }
                //if (!string.IsNullOrEmpty(_objS1Data.NoOfDefenders) && _objReqNoOfDefenders.Length > 0)
                {
                    string noOfDefenderss = objCf.ConvertStringArrayToString(_objReqNoOfDefenders);
                    ddlS2Dropwons.Add("noOfDefenders", noOfDefenderss);
                }

            return ddlS2Dropwons;
        }

        public override dynamic getFinalResult(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, ElasticClient EsClient, string sportid = "1")
        {
            string input = Convert.ToInt32(Convert.ToBoolean(_objMatchDetail.IsAsset)).ToString();
            QueryContainer query = new TermQuery { Field = "isAsset", Value = input };
            _objNestedQuery &= query;
            dynamic result;
            List<S2MasterData> lstsearchresults = new List<S2MasterData>();
            var results2MasterData = EsClient.Search<S2MasterData>(s => s.Index("kabaddis2data").Query(q => _objNestedQuery)
            .Aggregations(a => a.Terms("agg_E1", st => st.Script(p => p.Source("doc['attribute_Id_Level1.keyword'].value + '|' + doc['attribute_Name_Level1.keyword'].value + '|' + doc['attribute_Id_Level2.keyword'].value + '|' + doc['attribute_Name_Level2.keyword'].value + '|' + doc['attribute_Id_Level3.keyword'].value + '|' + doc['attribute_Name_Level3.keyword'].value + '|' + doc['attribute_Id_Level4.keyword'].value + '|' + doc['attribute_Name_Level4.keyword'].value + '|' + doc['emotionId.keyword'].value  + '|' + doc['emotionName.keyword'].value"))
            .Size(409846))));

            var response = results2MasterData.Aggregations.Terms("agg_E1");
            //var response2 = results2MasterData2.Aggregations.Terms("agg_E2").Buckets.ToList();
            //var resultss = response.Union(response2);

            foreach (var items in response.Buckets)
            {
                lstsearchresults.Add(new S2MasterData
                {
                    Attribute_Id_Level1 = items.Key.ToString().Split("|")[0],
                    Attribute_Name_Level1 = items.Key.ToString().Split("|")[1],
                    Attribute_Id_Level2 = items.Key.ToString().Split("|")[2],
                    Attribute_Name_Level2 = items.Key.ToString().Split("|")[3],
                    Attribute_Id_Level3 = items.Key.ToString().Split("|")[4],
                    Attribute_Name_Level3 = items.Key.ToString().Split("|")[5],
                    Attribute_Id_Level4 = items.Key.ToString().Split("|")[6],
                    Attribute_Name_Level4 = items.Key.ToString().Split("|")[7],
                    EmotionId = items.Key.ToString().Split("|")[8],
                    EmotionName = items.Key.ToString().Split("|")[9],
                });
            }
            //result = resultss;
            return lstsearchresults;
        }

        public int getMatchCount(QueryContainer _objNestedQuery, ElasticClient EsClient, string sType)
        {
            int Count = 0;
            if (sType == "Matches")
            {
                var response = EsClient.Search<KabaddiResultDataTempdata>(a => a.Index("kabaddi").Size(0).
                Query(q => _objNestedQuery)
                .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.MatchId.Suffix("keyword")).Size(65243)
                )));
                var agg = response.Aggregations.Terms("commit_count");
                Count = agg.Buckets.Count;
            }
            else if (sType == "Videos")
            {
                // QueryContainer TempQuery = new QueryContainer(); 
                QueryContainer qMust = new TermQuery { Field = "isAsset", Value = "0" };

                _objNestedQuery &= qMust;
                var response = EsClient.Search<KabaddiResultDataTempdata>(a => a.Index("kabaddi").Size(0). //SearchS2Data
                Query(q => _objNestedQuery)
               .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.IsAsset.Suffix("keyword")).Size(65243)
               )));
                var agg = response.Aggregations.Terms("commit_count");
                Count = agg.Buckets.Count;

            }
            else if (sType == "Assets")
            {
                // QueryContainer TempQuery = new QueryContainer();
                QueryContainer qMust = new TermQuery { Field = "isAsset", Value = "1" };
                //TempQuery = _objNestedQuery;
                _objNestedQuery &= qMust;
                var response = EsClient.Search<KabaddiResultDataTempdata>(a => a.Index("kabaddi").Size(0). //SearchS2Data
                 Query(q => _objNestedQuery)
                 .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.MatchId.Suffix("keyword")).Size(65243)
                 )));
                var agg = response.Aggregations.Terms("commit_count");
                Count = agg.Buckets.Count;

            }
            return Count;
        }

        public override QueryContainer GetMatchDetailQuery(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, bool isMasterData = false)
        {
            if (_objMatchDetail != null)
            {
                QueryContainer queryShould = new QueryContainer();

                if (!string.IsNullOrEmpty(_objMatchDetail.MatchFormat))
                {
                    if (_objMatchDetail.MatchFormat.Contains(","))
                    {
                        string[] _objaaray = _objMatchDetail.MatchFormat.Split(',');
                        foreach (var v in _objaaray)
                        {
                            QueryContainer q1 = new TermQuery { Field = "compType", Value = v.ToLower() };
                            queryShould |= q1;
                        }
                        //_objNestedQuery.Add(bq, Occur.MUST);
                    }
                    else
                    {
                        QueryContainer q1 = new TermQuery { Field = "compType", Value = _objMatchDetail.MatchFormat.ToLower() };
                        _objNestedQuery &= q1;
                    }
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.VenueId))
                {
                    QueryContainer q2 = new TermQuery { Field = "venueId", Value = _objMatchDetail.MatchFormat.ToLower().ToString() };
                    _objNestedQuery &= q2;
                }

                if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id))
                {
                    QueryContainer q3 = new TermQuery { Field = "team1Id", Value = _objMatchDetail.Team1Id.ToLower().ToString() };
                    //queryShould |= q3;
                    _objNestedQuery &= q3;
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                {
                    QueryContainer q4 = new TermQuery { Field = "team2Id", Value = _objMatchDetail.Team1Id.ToLower().ToString() };
                    //queryShould |= q4;
                    _objNestedQuery &= q4;
                }

                if (!string.IsNullOrEmpty(_objMatchDetail.SeriesId) && _objMatchDetail.IsParentSeries)
                {
                    string Dlist = _objMatchDetail.SeriesId;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q7 = new TermQuery { Field = "parentSeriesId", Value = str };
                        queryShould |= q7;
                    }
                    // QueryContainer q7 = new TermQuery { Field = "parentSeriesId", Value = _objMatchDetail.SeriesId };
                    //_objNestedQuery &= q7;
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.SeriesId) && !_objMatchDetail.IsParentSeries)
                {
                    string Dlist = _objMatchDetail.SeriesId;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {

                        QueryContainer q8 = new TermQuery { Field = "seriesId", Value = str };
                        queryShould |= q8;
                    }
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.MatchId))
                {

                    string Dlist = _objMatchDetail.MatchId;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q9 = new TermQuery { Field = "matchId", Value = str };
                        queryShould |= q9;
                    }
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
                    //_objNestedQuery &= queryShould;

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
                    //_objNestedQuery &= queryShould;

                }
                if (!string.IsNullOrEmpty(_objMatchDetail.CompTypeId))
                {
                    var myQuery = new BoolQuery();
                    string Dlist = Convert.ToString(_objMatchDetail.CompTypeId);
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q17 = new TermQuery { Field = "compTypeId", Value = str };
                        queryShould |= q17;
                    }
                    // _objNestedQuery &= queryShould;

                }

                if (!string.IsNullOrEmpty(_objMatchDetail.GameTypeId))
                {
                    var myQuery = new BoolQuery();
                    string Dlist = Convert.ToString(_objMatchDetail.GameTypeId);
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q23 = new TermQuery { Field = "gameTypeId", Value = str };
                        queryShould |= q23;
                    }
                    // _objNestedQuery &= queryShould;

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

                if (!string.IsNullOrEmpty(_objMatchDetail.LanguageId) && _objMatchDetail.LanguageId != "0" && _objMatchDetail.IsAsset)
                {
                    string[] Id = _objMatchDetail.LanguageId.Split(',');

                    foreach (string str in Id)
                    {

                        QueryContainer q19 = new TermQuery { Field = "languageId", Value = str };
                        queryShould |= q19;
                    }
                    //_objNestedQuery.Add(bq, Occur.MUST);
                }

                _objNestedQuery &= queryShould;

            }

            return _objNestedQuery;
        }

        public override QueryContainer GetPlayerDetails(dynamic _objS1Data, QueryContainer qFinal, List<string> valueObj, int sportid, bool isMasterData = false)
        {
            CommonFunction objCf = new CommonFunction();
            QueryContainer qShould = new QueryContainer();
            QueryContainer queryShouldS = new QueryContainer();
            QueryContainer queryShouldB = new QueryContainer();
            QueryContainer queryAnd_should = new QueryContainer();
            if (_objS1Data != null)
            {
                string e = Convert.ToString(_objS1Data.EventId);
                if (isMasterData)
                {
                    string[] _objReqEvents = "2,3,4".Split(',');
                    if (!string.IsNullOrEmpty(_objS1Data.EventId) && !_objReqEvents.ToList().Contains(_objS1Data.EventId))
                    {
                        string[] strnums = e.Contains(",") ? Convert.ToString(_objS1Data.EventId).Split(',') : new string[] { _objS1Data.EventId };

                        foreach (string str in strnums)
                        {
                            QueryContainer query1 = new TermQuery { Field = "eventId", Value = str };
                            qShould |= query1;
                        }
                        qFinal &= qShould;
                    }
                }
                else if (!string.IsNullOrEmpty(_objS1Data.EventId))
                {
                    string[] strnums = e.Contains(",") ? Convert.ToString(_objS1Data.EventId).Split(','): new string[] { _objS1Data.EventId };
                    foreach (string str in strnums)
                    {
                        QueryContainer query1 = new TermQuery { Field = "eventId", Value = str };
                        qShould |= query1;
                    }
                    qFinal &= qShould;
                }

                if (_objS1Data.IsAllOut || _objS1Data.IsDeclaration || _objS1Data.IsTimeOut || _objS1Data.IsSubstitution || _objS1Data.IsTechnicalPoint || _objS1Data.IsPursuit 
                    || _objS1Data.IsDoOrDieRaid || _objS1Data.IsSuperRaid)
                {
                    if (_objS1Data.IsAllOut)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "isAllOut", Value = _objS1Data.IsAllOut };
                        qShould |= q1ors;
                    }
                    if (_objS1Data.IsDeclaration)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "isDeclaration", Value = _objS1Data.IsDeclaration };
                        qShould |= q1ors;
                    }
                    if (_objS1Data.IsTimeOut)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "isTimeOut", Value = _objS1Data.IsTimeOut };
                        qShould |= q1ors;
                    }
                    if (_objS1Data.IsSubstitution)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "isSubstitution", Value = _objS1Data.IsSubstitution };
                        qShould |= q1ors;
                    }
                    if (_objS1Data.IsTechnicalPoint)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "isTechnicalPoint", Value = _objS1Data.IsTechnicalPoint };
                        qShould |= q1ors;
                    }
                    if (_objS1Data.IsPursuit)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "isPursuit", Value = _objS1Data.IsPursuit };
                        qShould |= q1ors;
                    }
                    if (_objS1Data.IsDoOrDieRaid)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "isDoOrDieRaid", Value = _objS1Data.IsDoOrDieRaid };
                        qShould |= q1ors;
                    }
                    if (_objS1Data.IsSuperRaid)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "isSuperRaid", Value = _objS1Data.IsSuperRaid };
                        qShould |= q1ors;
                    }

                    qFinal &= qShould;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(_objS1Data.OffensivePlayerId)))
                {
                    QueryContainer q1 = new TermQuery { Field = "offensivePlayerId", Value = _objS1Data.OffensivePlayerId };
                    qFinal &= q1;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(_objS1Data.DefensivePlayerId)))
                {
                    QueryContainer q2 = new TermQuery { Field = "defensivePlayerId", Value = _objS1Data.DefensivePlayerId };
                    qFinal &= q2;
                }
                if (!string.IsNullOrEmpty(Convert.ToString(_objS1Data.AssistPlayerId)))
                {
                    QueryContainer q3 = new TermQuery { Field = "assistPlayer1Id", Value = _objS1Data.AssistPlayerId } ||
                     new TermQuery { Field = "assistPlayer2Id", Value = _objS1Data.AssistPlayerId };
                    qFinal &= q3;
                }
                if (_objS1Data.IsSuccessfulRaid || _objS1Data.IsEmptyRaid || _objS1Data.IsFailedRaid || _objS1Data.IsBonusPoint)
                {
                    if (_objS1Data.IsSuccessfulRaid)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "isSuccessfulRaid", Value = _objS1Data.IsSuccessfulRaid };
                        qShould |= q1ors;
                    }
                    if (_objS1Data.IsEmptyRaid)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "isEmptyRaid", Value = _objS1Data.IsEmptyRaid };
                        qShould |= q1ors;
                    }
                    if (_objS1Data.IsFailedRaid)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "isFailedRaid", Value = _objS1Data.IsFailedRaid };
                        qShould |= q1ors;
                    }
                    if (_objS1Data.IsBonusPoint)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "isBonusPoint", Value = _objS1Data.IsBonusPoint };
                        qShould |= q1ors;
                    }

                    qFinal &= qShould;
                }
                if (!string.IsNullOrEmpty(_objS1Data.TouchTypeId) && !isMasterData)
                {
                    string[] strnums = Convert.ToString(_objS1Data.TouchTypeId).Split(',');
                    foreach (string str in strnums)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "touchTypeId", Value = str };
                        qShould |= q1ors;
                    }
                    qFinal &= qShould;
                }
                if (_objS1Data.IsFailedTackle || _objS1Data.IsSuperTackle)
                {
                    if (_objS1Data.IsFailedTackle)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "isFailedTackle", Value = _objS1Data.IsFailedTackle };
                        qShould |= q1ors;
                    }
                    if (_objS1Data.IsSuperTackle)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "isSuperTackle", Value = _objS1Data.IsSuperTackle };
                        qShould |= q1ors;
                    }
                    qFinal &= qShould;
                }
                if (_objS1Data.IsSuccessfulTackle)
                {
                    QueryContainer q1ors = new TermQuery { Field = "isSuccessfulTackle", Value = _objS1Data.IsSuccessfulTackle };
                    qShould &= q1ors;
                }
                if (!string.IsNullOrEmpty(_objS1Data.TackleTypeId) && !isMasterData)
                {
                    string[] strnums = Convert.ToString(_objS1Data.TackleTypeId).Split(',');
                    foreach (string str in strnums)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "tackleTypeId", Value = str };
                        qShould |= q1ors;
                    }
                    qFinal &= qShould;
                }
                if (!string.IsNullOrEmpty(_objS1Data.AssistTypeId) && !isMasterData)
                {
                    string[] strnums = Convert.ToString(_objS1Data.AssistTypeId).Split(',');
                    foreach (string str in strnums)
                    {
                        QueryContainer q3 = new TermQuery { Field = "assistType1Id", Value = str } ||
                        new TermQuery { Field = "assistType2Id", Value = str };
                        qFinal &= q3;
                    }
                }
                if (!string.IsNullOrEmpty(_objS1Data.AssistPlayerId))
                {
                    if (_objS1Data.AssistPlayerId.Contains(","))
                    {
                        string[] ArrAssistsPlayers = _objS1Data.AssistPlayerId.Split(',');

                        QueryContainer q3 = new TermQuery { Field = "assistType1Id", Value = ArrAssistsPlayers[0] } &&
                       new TermQuery { Field = "assistType2Id", Value = ArrAssistsPlayers[1] };

                        qFinal &= q3;
                    }
                    else
                    {
                        QueryContainer q3 = new TermQuery { Field = "assistType1Id", Value = _objS1Data["AssistTypeId"].Trim() } ||
                        new TermQuery { Field = "assistType2Id", Value = _objS1Data["AssistType2Id"].Trim() };
                        qFinal &= q3;
                    }
                }
                if (!string.IsNullOrEmpty(_objS1Data.NoOfDefenders) && !isMasterData)
                {
                    string[] strnums = Convert.ToString(_objS1Data.NoOfDefenders).Split(',');
                    foreach (string str in strnums)
                    {
                        QueryContainer q1ors = new TermQuery { Field = "tackleTypeId", Value = str };
                        qShould |= q1ors;
                    }
                    qFinal &= qShould;
                }
            }

            //qFinal &= qShould;
            return qFinal;
        }

        public int GetPlayerMatchDetailsMaxCount(QueryContainer _objNestedQuery, ElasticClient EsClient, string sType)
        {
            throw new NotImplementedException();
        }

        public QueryContainer GetS2ActionQueryResult(S2ActionData _objActionData, QueryContainer _objNestedQuery, bool isMasterData = false)
        {
            QueryContainer qShould = new QueryContainer();
            if (!string.IsNullOrEmpty(_objActionData.AttributeId_Level1))
            {
                QueryContainer bq = new QueryContainer();
                string SearchText = Convert.ToString(_objActionData.AttributeId_Level1);
                string[] Dlist = SearchText.Split(',').ToArray();
                foreach (string str in Dlist)
                {
                    QueryContainer q1 = new TermQuery { Field = "attribute_Id_Level1", Value = str };
                    qShould |= q1;
                }
            }
            if (!string.IsNullOrEmpty(_objActionData.AttributeId_Level2))
            {
                string SearchText = Convert.ToString(_objActionData.AttributeId_Level2);
                string[] Dlist = SearchText.Split(',').ToArray();
                foreach (string str in Dlist)
                {

                    QueryContainer q2 = new TermQuery { Field = "attribute_Id_Level2", Value = str };
                    qShould |= q2;
                }
            }
            if (!string.IsNullOrEmpty(_objActionData.AttributeId_Level3))
            {
                string SearchText = Convert.ToString(_objActionData.AttributeId_Level3);
                string[] Dlist = SearchText.Split(',').ToArray();
                foreach (string str in Dlist)
                {
                    QueryContainer q3 = new TermQuery { Field = "attribute_Id_Level3", Value = str };
                    qShould |= q3;
                }
            }
            if (!string.IsNullOrEmpty(_objActionData.AttributeId_Level4))
            {
                string SearchText = Convert.ToString(_objActionData.AttributeId_Level4);
                string[] Dlist = SearchText.Split(',').ToArray();
                foreach (string str in Dlist)
                {
                    QueryContainer q4 = new TermQuery { Field = "attribute_Id_Level4", Value = str };
                    qShould |= q4;
                }
            }

            if (!string.IsNullOrEmpty(_objActionData.Emotion))
            {
                string SearchText = Convert.ToString(_objActionData.Emotion);
                string[] Dlist = SearchText.Split(',').ToArray();
                foreach (string str in Dlist)
                {
                    QueryContainer q5 = new TermQuery { Field = "emotionId", Value = str };
                    qShould |= q5;
                }
            }
            if (!string.IsNullOrEmpty(_objActionData.Entities))
            {

                if (_objActionData.Entities.Contains(","))
                {
                    string[] arrEntities = _objActionData.Entities.Split(',');
                    for (int iEntCtr = 0; iEntCtr < arrEntities.Length; iEntCtr++)
                    {
                        if (!string.IsNullOrEmpty(arrEntities[iEntCtr]))
                        {
                            QueryContainer q6 = new TermQuery { Field = "entityId_1", Value = arrEntities[iEntCtr] };
                            qShould |= q6;

                            QueryContainer q7 = new TermQuery { Field = "entityId_2", Value = arrEntities[iEntCtr] };
                            qShould |= q7;

                            QueryContainer q8 = new TermQuery { Field = "entityId_3", Value = arrEntities[iEntCtr] };
                            qShould |= q8;
                        }
                    }
                }
                else
                {
                    QueryContainer q9 = new TermQuery { Field = "entityId_1", Value = _objActionData.Entities };
                    _objNestedQuery &= q9;
                }
            }
            _objNestedQuery &= qShould;
            return _objNestedQuery;
        }

        public QueryContainer GetS2MomentQueryResult(Moments _objMomentData, QueryContainer _objNestedQuery, bool isMasterData = false)
        {
            QueryContainer qShould = new QueryContainer();
            if (!string.IsNullOrEmpty(_objMomentData.Entities))
            {

                if (_objMomentData.Entities.Contains(","))
                {
                    string[] arrEntities = _objMomentData.Entities.Split(',');
                    for (int iEntCtr = 0; iEntCtr < arrEntities.Length; iEntCtr++)
                    {

                        if (!string.IsNullOrEmpty(arrEntities[iEntCtr]))
                        {
                            QueryContainer q1 = new TermQuery { Field = "entityId_1", Value = arrEntities[iEntCtr] };
                            qShould |= q1;
                            QueryContainer q2 = new TermQuery { Field = "entityId_2", Value = arrEntities[iEntCtr] };
                            qShould |= q2;
                            QueryContainer q3 = new TermQuery { Field = "entityId_3", Value = arrEntities[iEntCtr] };
                            qShould |= q3;
                        }
                    }
                }
                else
                {
                    QueryContainer q1 = new TermQuery { Field = "entityId_1", Value = _objMomentData.Entities };
                    _objNestedQuery &= q1;

                }
            }
            if (_objMomentData.IsMoments)
            {
                if (_objMomentData.IsBigMoment || _objMomentData.IsFunnyMoment || _objMomentData.IsAudioPiece)
                {

                    if (_objMomentData.IsBigMoment)
                    {
                        QueryContainer q4 = new TermQuery { Field = "isBigMoment", Value = "1" };
                        qShould |= q4;
                    }
                    if (_objMomentData.IsFunnyMoment)
                    {
                        QueryContainer q5 = new TermQuery { Field = "isFunnyMoment", Value = "1" };
                        qShould |= q5;
                    }
                    if (_objMomentData.IsAudioPiece)
                    {
                        QueryContainer q6 = new TermQuery { Field = "isAudioPiece", Value = "1" };
                        qShould |= q6;
                    }
                }
                else
                {
                    QueryContainer q7 = new TermQuery { Field = "isBigMoment", Value = "1" };
                    qShould |= q7;
                    QueryContainer q8 = new TermQuery { Field = "isFunnyMoment", Value = "1" };
                    qShould |= q8;
                    QueryContainer q9 = new TermQuery { Field = "isAudioPiece", Value = "1" };
                    qShould |= q9;
                }
            }
            _objNestedQuery &= qShould;
            return _objNestedQuery;
        }

        public QueryContainer GetS2SearchResults(SearchS2RequestData _objReqData, QueryContainer _objNestedQuery)
        {
            // QueryContainer _objNestedQuery = new QueryContainer();
            string result = string.Empty;
            try
            {
                IEnumerable<SearchS2ResultData> searchResults = new List<SearchS2ResultData>();
                List<SearchQueryModel> _objLstSearchQuery = new List<SearchQueryModel>();

                if (_objReqData != null)
                {
                    MatchDetail _objMatchDetail = _objReqData.MatchDetails.FirstOrDefault();
                    S2ActionData _objActionData = _objReqData.ActionData.FirstOrDefault();
                    Moments _objMomentData = _objReqData.Moments.FirstOrDefault();
                    int sportid = _objMatchDetail.SportID;

                    if (_objMatchDetail != null)
                    {
                        _objNestedQuery = GetMatchDetailQuery(_objNestedQuery, _objMatchDetail);
                    }

                    if (_objActionData != null)
                    {
                        _objNestedQuery = GetS2ActionQueryResult(_objActionData, _objNestedQuery);
                    }
                    if (_objMomentData != null)
                    {
                        _objNestedQuery = GetS2MomentQueryResult(_objMomentData, _objNestedQuery);

                    }
                    if (!string.IsNullOrEmpty(_objMatchDetail.LanguageId) && _objMatchDetail.LanguageId != "0" && !_objMatchDetail.IsAsset)
                    {
                        string[] Id = _objMatchDetail.LanguageId.Split(',');
                        QueryContainer qShould = new QueryContainer();
                        foreach (string str in Id)
                        {

                            QueryContainer query = new TermQuery { Field = "languageId", Value = str };
                            qShould |= query;
                        }
                        _objNestedQuery &= qShould;
                    }
                    if (!string.IsNullOrEmpty(_objMatchDetail.MatchDate))
                    {
                        string dlist = _objMatchDetail.MatchDate;
                        if (dlist.Contains("-"))
                        {
                            string[] strNumbers = dlist.Split('-');

                            int start = int.Parse(DateTime.ParseExact(strNumbers[0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            int End = int.Parse(DateTime.ParseExact(strNumbers[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            QueryContainer qMust = new TermRangeQuery { Field = "matchDate", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                            _objNestedQuery &= qMust;
                        }
                        else
                        {
                            int date = int.Parse(DateTime.ParseExact(_objMatchDetail.MatchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            QueryContainer qMust = new TermRangeQuery { Field = "matchDate", GreaterThanOrEqualTo = date.ToString() };
                            _objNestedQuery &= qMust;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return _objNestedQuery;
        }

        public List<KabaddiResultData> MapkabaddiS1data(List<KabaddiResultDataTempdata> _objs1Data, string cases)
        {
            List<KabaddiResultData> _objresult = new List<KabaddiResultData>();
            try
            {
                switch (cases)
                {

                    case "1":
                        foreach (var str in _objs1Data)
                        {
                            KabaddiResultData result = new KabaddiResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId,
                                MediaId = str.MediaId,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Description = str.Description,
                                Title = str.Title,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                EventId = str.EventId,
                                EventName = str.EventName,
                                EventText = str.EventText,
                                TouchTypeId = str.TouchTypeId,
                                TouchType = str.TouchType,
                                TackleTypeId = str.TackleTypeId,
                                TackleType = str.TackleType,
                                AssistType1Id = str.AssistType1Id,
                                AssistType1 = str.AssistType1,
                                AssistType2Id = str.AssistType2Id,
                                AssistType2 = str.AssistType2,
                                NoOfDefenders = str.NoOfDefenders,
                                LanguageId = "1"
                            };
                            _objresult.Add(result);
                        }
                        break;
                    case "2":
                        foreach (var str in _objs1Data)
                        {
                            KabaddiResultData result = new KabaddiResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId2,
                                MediaId = str.MediaId2,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Description = str.Description,
                                Title = str.Title,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                EventId = str.EventId,
                                EventName = str.EventName,
                                EventText = str.EventText,
                                TouchTypeId = str.TouchTypeId,
                                TouchType = str.TouchType,
                                TackleTypeId = str.TackleTypeId,
                                TackleType = str.TackleType,
                                AssistType1Id = str.AssistType1Id,
                                AssistType1 = str.AssistType1,
                                AssistType2Id = str.AssistType2Id,
                                AssistType2 = str.AssistType2,
                                NoOfDefenders = str.NoOfDefenders,
                                LanguageId = "2"
                            };
                            _objresult.Add(result);
                        }
                        break;
                    case "5":
                        foreach (var str in _objs1Data)
                        {
                            KabaddiResultData result = new KabaddiResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId3,
                                MediaId = str.MediaId3,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Description = str.Description,
                                Title = str.Title,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                EventId = str.EventId,
                                EventName = str.EventName,
                                EventText = str.EventText,
                                TouchTypeId = str.TouchTypeId,
                                TouchType = str.TouchType,
                                TackleTypeId = str.TackleTypeId,
                                TackleType = str.TackleType,
                                AssistType1Id = str.AssistType1Id,
                                AssistType1 = str.AssistType1,
                                AssistType2Id = str.AssistType2Id,
                                AssistType2 = str.AssistType2,
                                NoOfDefenders = str.NoOfDefenders,
                                LanguageId = "5",
                            };
                            _objresult.Add(result);
                        }

                        break;

                    case "6":
                        foreach (var str in _objs1Data)
                        {
                            KabaddiResultData result = new KabaddiResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId4,
                                MediaId = str.MediaId4,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Description = str.Description,
                                Title = str.Title,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                EventId = str.EventId,
                                EventName = str.EventName,
                                EventText = str.EventText,
                                TouchTypeId = str.TouchTypeId,
                                TouchType = str.TouchType,
                                TackleTypeId = str.TackleTypeId,
                                TackleType = str.TackleType,
                                AssistType1Id = str.AssistType1Id,
                                AssistType1 = str.AssistType1,
                                AssistType2Id = str.AssistType2Id,
                                AssistType2 = str.AssistType2,
                                NoOfDefenders = str.NoOfDefenders,
                                LanguageId = "6"
                            };
                            _objresult.Add(result);
                        }

                        break;

                    case "7":
                        foreach (var str in _objs1Data)
                        {
                            KabaddiResultData result = new KabaddiResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId5,
                                MediaId = str.MediaId5,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Description = str.Description,
                                Title = str.Title,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                EventId = str.EventId,
                                EventName = str.EventName,
                                EventText = str.EventText,
                                TouchTypeId = str.TouchTypeId,
                                TouchType = str.TouchType,
                                TackleTypeId = str.TackleTypeId,
                                TackleType = str.TackleType,
                                AssistType1Id = str.AssistType1Id,
                                AssistType1 = str.AssistType1,
                                AssistType2Id = str.AssistType2Id,
                                AssistType2 = str.AssistType2,
                                NoOfDefenders = str.NoOfDefenders,
                                LanguageId = "7"
                            };
                            _objresult.Add(result);
                        }

                        break;

                    case "8":
                        foreach (var str in _objs1Data)
                        {
                            KabaddiResultData result = new KabaddiResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId6,
                                MediaId = str.MediaId6,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Description = str.Description,
                                Title = str.Title,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                EventId = str.EventId,
                                EventName = str.EventName,
                                EventText = str.EventText,
                                TouchTypeId = str.TouchTypeId,
                                TouchType = str.TouchType,
                                TackleTypeId = str.TackleTypeId,
                                TackleType = str.TackleType,
                                AssistType1Id = str.AssistType1Id,
                                AssistType1 = str.AssistType1,
                                AssistType2Id = str.AssistType2Id,
                                AssistType2 = str.AssistType2,
                                NoOfDefenders = str.NoOfDefenders,
                                LanguageId = "8"
                            };
                            _objresult.Add(result);
                        }

                        break;

                    case "50":
                        foreach (var str in _objs1Data)
                        {
                            KabaddiResultData result = new KabaddiResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId7,
                                MediaId = str.MediaId7,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Description = str.Description,
                                Title = str.Title,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                EventId = str.EventId,
                                EventName = str.EventName,
                                EventText = str.EventText,
                                TouchTypeId = str.TouchTypeId,
                                TouchType = str.TouchType,
                                TackleTypeId = str.TackleTypeId,
                                TackleType = str.TackleType,
                                AssistType1Id = str.AssistType1Id,
                                AssistType1 = str.AssistType1,
                                AssistType2Id = str.AssistType2Id,
                                AssistType2 = str.AssistType2,
                                NoOfDefenders = str.NoOfDefenders,
                                LanguageId = "50"
                            };
                            _objresult.Add(result);
                        }

                        break;

                    default:
                        foreach (var str in _objs1Data)
                        {
                            KabaddiResultData result = new KabaddiResultData()
                            {
                                Id = str.Id,
                                ClearId = str.ClearId,
                                MediaId = str.MediaId,
                                MatchDate = str.MatchDate,
                                MatchId = str.MatchId,
                                MarkIn = str.MarkIn,
                                MarkOut = str.MarkOut,
                                ShortMarkIn = str.ShortMarkIn,
                                ShortMarkOut = str.ShortMarkOut,
                                Title = str.Title,
                                Description = str.Description,
                                IsAsset = str.IsAsset,
                                Duration = str.Duration,
                                EventId = str.EventId,
                                EventName = str.EventName,
                                EventText = str.EventText,
                                TouchTypeId = str.TouchTypeId,
                                TouchType = str.TouchType,
                                TackleTypeId = str.TackleTypeId,
                                TackleType = str.TackleType,
                                AssistType1Id = str.AssistType1Id,
                                AssistType1 = str.AssistType1,
                                AssistType2Id = str.AssistType2Id,
                                AssistType2 = str.AssistType2,
                                NoOfDefenders = str.NoOfDefenders,
                                LanguageId = str.LanguageId

                            };
                            _objresult.Add(result);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

            return _objresult;
        }

        public List<KabaddiResultData> MapkabaddiS1dataCopy(List<KabaddiResultDataTempdata> _objs1Data, MatchDetail _objmatchdetail)
        {
            List<KabaddiResultData> Final_result = new List<KabaddiResultData>();
            string[] Id = _objmatchdetail.LanguageId.Split(',');
            switch (_objmatchdetail.IsAsset)
            {
                case true:
                    Final_result = MapkabaddiS1data(_objs1Data, "3");
                    break;
                case false:
                    if (string.IsNullOrEmpty(_objmatchdetail.LanguageId))
                    {
                        for (int i = 1; i <= 7; i++)
                        {
                            if (i == 1)
                            {
                                List<KabaddiResultData> FinalTempResult = new List<KabaddiResultData>();
                                List<KabaddiResultDataTempdata> temp = new List<KabaddiResultDataTempdata>();
                                temp = _objs1Data.Where(t => t.ClearId != "").ToList();
                                FinalTempResult = MapkabaddiS1data(temp, "1");
                                if (!_objmatchdetail.IsAsset && FinalTempResult.ToList().Count > 0)
                                {
                                    FinalTempResult = FinalTempResult.ToList().GroupBy(x => x.Id).Select(z => z.First()).ToList();//.GroupBy(t => t.Id, (key, group) => group.First());
                                }
                                Final_result = FinalTempResult;
                                FinalTempResult = null;
                            }
                            else if (i == 2)
                            {
                                List<KabaddiResultData> FinalTempResult1 = new List<KabaddiResultData>();
                                List<KabaddiResultDataTempdata> temp = new List<KabaddiResultDataTempdata>();
                                temp = _objs1Data.Where(t => t.ClearId2 != "").ToList();
                                FinalTempResult1 = MapkabaddiS1data(temp, "2");
                                if (!_objmatchdetail.IsAsset && FinalTempResult1.ToList().Count > 0)
                                {
                                    FinalTempResult1 = FinalTempResult1.ToList().GroupBy(x => x.Id).Select(z => z.First()).ToList();//.GroupBy(t => t.Id, (key, group) => group.First());
                                }
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (i == 3)
                            {
                                List<KabaddiResultData> FinalTempResult1 = new List<KabaddiResultData>();
                                List<KabaddiResultDataTempdata> temp = new List<KabaddiResultDataTempdata>();
                                temp = _objs1Data.Where(t => t.ClearId3 != "").ToList();
                                FinalTempResult1 = MapkabaddiS1data(temp, "5");
                                if (!_objmatchdetail.IsAsset && FinalTempResult1.ToList().Count > 0)
                                {
                                    FinalTempResult1 = FinalTempResult1.ToList().GroupBy(x => x.Id).Select(z => z.First()).ToList();//.GroupBy(t => t.Id, (key, group) => group.First());
                                }
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (i == 4)
                            {
                                List<KabaddiResultData> FinalTempResult4 = new List<KabaddiResultData>();
                                List<KabaddiResultDataTempdata> temp = new List<KabaddiResultDataTempdata>();
                                temp = _objs1Data.Where(t => t.ClearId4 != "").ToList();
                                FinalTempResult4 = MapkabaddiS1data(temp, "6");
                                if (!_objmatchdetail.IsAsset && FinalTempResult4.ToList().Count > 0)
                                {
                                    FinalTempResult4 = FinalTempResult4.ToList().GroupBy(x => x.Id).Select(z => z.First()).ToList();
                                }
                                Final_result = FinalTempResult4.Union(Final_result).ToList();
                                FinalTempResult4 = null;
                            }
                            else if (i == 5)
                            {
                                List<KabaddiResultData> FinalTempResult5 = new List<KabaddiResultData>();
                                List<KabaddiResultDataTempdata> temp = new List<KabaddiResultDataTempdata>();
                                temp = _objs1Data.Where(t => t.ClearId5 != "").ToList();
                                FinalTempResult5 = MapkabaddiS1data(temp, "7");
                                if (!_objmatchdetail.IsAsset && FinalTempResult5.ToList().Count > 0)
                                {
                                    FinalTempResult5 = FinalTempResult5.ToList().GroupBy(x => x.Id).Select(z => z.First()).ToList();
                                }
                                Final_result = FinalTempResult5.Union(Final_result).ToList();
                                FinalTempResult5 = null;
                            }
                            else if (i == 6)
                            {
                                List<KabaddiResultData> FinalTempResult6 = new List<KabaddiResultData>();
                                List<KabaddiResultDataTempdata> temp = new List<KabaddiResultDataTempdata>();
                                temp = _objs1Data.Where(t => t.ClearId6 != "").ToList();
                                FinalTempResult6 = MapkabaddiS1data(temp, "8");
                                if (!_objmatchdetail.IsAsset && FinalTempResult6.ToList().Count > 0)
                                {
                                    FinalTempResult6 = FinalTempResult6.ToList().GroupBy(x => x.Id).Select(z => z.First()).ToList();
                                }
                                Final_result = FinalTempResult6.Union(Final_result).ToList();
                                FinalTempResult6 = null;
                            }
                            else if (i == 7)
                            {
                                List<KabaddiResultData> FinalTempResult7 = new List<KabaddiResultData>();
                                List<KabaddiResultDataTempdata> temp = new List<KabaddiResultDataTempdata>();
                                temp = _objs1Data.Where(t => t.ClearId6 != "").ToList();
                                FinalTempResult7 = MapkabaddiS1data(temp, "50");
                                if (!_objmatchdetail.IsAsset && FinalTempResult7.ToList().Count > 0)
                                {
                                    FinalTempResult7 = FinalTempResult7.ToList().GroupBy(x => x.Id).Select(z => z.First()).ToList();
                                }
                                Final_result = FinalTempResult7.Union(Final_result).ToList();
                                FinalTempResult7 = null;
                            }
                        }
                    }
                    else
                    {

                        for (int i = 0; i < Id.Count(); i++)
                        {
                            if (Id[i] == "1")
                            {
                                List<KabaddiResultData> FinalTempResult = new List<KabaddiResultData>();
                                List<KabaddiResultDataTempdata> temp = new List<KabaddiResultDataTempdata>();
                                temp = _objs1Data.Where(t => t.ClearId != "").ToList();
                                FinalTempResult = MapkabaddiS1data(temp, "1");
                                if (!_objmatchdetail.IsAsset && FinalTempResult.ToList().Count > 0)
                                {
                                    FinalTempResult = FinalTempResult.ToList().GroupBy(x => x.Id).Select(z => z.First()).ToList();//.GroupBy(t => t.Id, (key, group) => group.First());
                                }
                                Final_result = FinalTempResult;
                                FinalTempResult = null;
                            }
                            else if (Id[i] == "2")
                            {
                                List<KabaddiResultData> FinalTempResult = new List<KabaddiResultData>();
                                List<KabaddiResultDataTempdata> temp = new List<KabaddiResultDataTempdata>();
                                temp = _objs1Data.Where(t => t.ClearId2 != "").ToList();
                                List<KabaddiResultData> FinalTempResult1 = new List<KabaddiResultData>();
                                FinalTempResult1 = MapkabaddiS1data(temp, "2");
                                if (!_objmatchdetail.IsAsset && FinalTempResult1.ToList().Count > 0)
                                {
                                    FinalTempResult1 = FinalTempResult1.ToList().GroupBy(x => x.Id).Select(z => z.First()).ToList();//.GroupBy(t => t.Id, (key, group) => group.First());
                                }
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;

                            }
                            else if (Id[i] == "5")
                            {
                                List<KabaddiResultData> FinalTempResult2 = new List<KabaddiResultData>();
                                List<KabaddiResultDataTempdata> temp = new List<KabaddiResultDataTempdata>();
                                temp = _objs1Data.Where(t => t.ClearId3 != "").ToList();
                                FinalTempResult2 = MapkabaddiS1data(temp, "5");
                                if (!_objmatchdetail.IsAsset && FinalTempResult2.ToList().Count > 0)
                                {
                                    FinalTempResult2 = FinalTempResult2.ToList().GroupBy(x => x.Id).Select(z => z.First()).ToList();//.GroupBy(t => t.Id, (key, group) => group.First());
                                }
                                Final_result = FinalTempResult2.Union(Final_result).ToList();
                                FinalTempResult2 = null;
                            }
                            else if (Id[i] == "6")
                            {
                                List<KabaddiResultData> FinalTempResult3 = new List<KabaddiResultData>();
                                List<KabaddiResultDataTempdata> temp = new List<KabaddiResultDataTempdata>();
                                temp = _objs1Data.Where(t => t.ClearId4 != "").ToList();
                                FinalTempResult3 = MapkabaddiS1data(temp, "6");
                                if (!_objmatchdetail.IsAsset && FinalTempResult3.ToList().Count > 0)
                                {
                                    FinalTempResult3 = FinalTempResult3.ToList().GroupBy(x => x.Id).Select(z => z.First()).ToList();//.GroupBy(t => t.Id, (key, group) => group.First());
                                }
                                Final_result = FinalTempResult3.Union(Final_result).ToList();
                                FinalTempResult3 = null;
                            }
                            else if (Id[i] == "7")
                            {
                                List<KabaddiResultData> FinalTempResult4 = new List<KabaddiResultData>();
                                List<KabaddiResultDataTempdata> temp = new List<KabaddiResultDataTempdata>();
                                temp = _objs1Data.Where(t => t.ClearId5 != "").ToList();
                                FinalTempResult4 = MapkabaddiS1data(temp, "7");
                                if (!_objmatchdetail.IsAsset && FinalTempResult4.ToList().Count > 0)
                                {
                                    FinalTempResult4 = FinalTempResult4.ToList().GroupBy(x => x.Id).Select(z => z.First()).ToList();
                                }
                                Final_result = FinalTempResult4.Union(Final_result).ToList();
                                FinalTempResult4 = null;
                            }
                            else if (Id[i] == "8")
                            {
                                List<KabaddiResultData> FinalTempResult5 = new List<KabaddiResultData>();
                                List<KabaddiResultDataTempdata> temp = new List<KabaddiResultDataTempdata>();
                                temp = _objs1Data.Where(t => t.ClearId6 != "").ToList();
                                FinalTempResult5 = MapkabaddiS1data(temp, "8");
                                if (!_objmatchdetail.IsAsset && FinalTempResult5.ToList().Count > 0)
                                {
                                    FinalTempResult5 = FinalTempResult5.ToList().GroupBy(x => x.Id).Select(z => z.First()).ToList();
                                }
                                Final_result = FinalTempResult5.Union(Final_result).ToList();
                                FinalTempResult5 = null;
                            }
                            else if (Id[i] == "50")
                            {
                                List<KabaddiResultData> FinalTempResult6 = new List<KabaddiResultData>();
                                List<KabaddiResultDataTempdata> temp = new List<KabaddiResultDataTempdata>();
                                temp = _objs1Data.Where(t => t.ClearId6 != "").ToList();
                                FinalTempResult6 = MapkabaddiS1data(temp, "50");
                                if (!_objmatchdetail.IsAsset && FinalTempResult6.ToList().Count > 0)
                                {
                                    FinalTempResult6 = FinalTempResult6.ToList().GroupBy(x => x.Id).Select(z => z.First()).ToList();
                                }
                                Final_result = FinalTempResult6.Union(Final_result).ToList();
                                FinalTempResult6 = null;
                            }
                        }
                    }
                    break;
            }
            return Final_result;
        }

        public dynamic MapS2Resuldata(QueryContainer _objNestedquery, string Search, ElasticClient EsClient)
        {
            dynamic S2Result = null;

            try
            {
                KeyValuePair<string, string> KeyValue_E1 = objCf.GetColumnForEntity(Convert.ToInt32(40)).FirstOrDefault();
                KeyValuePair<string, string> KeyValueS_E2 = objCf.GetColumnForEntity(Convert.ToInt32(41)).FirstOrDefault();
                KeyValuePair<string, string> KeyValueS_E3 = objCf.GetColumnForEntity(Convert.ToInt32(42)).FirstOrDefault();
                List<S2FilteredEntity> obj = new List<S2FilteredEntity>();

                if (!string.IsNullOrEmpty(Search))
                {

                    List<S2FilteredEntity> _objFilterentityt = new List<S2FilteredEntity>();
                    var result_E1 = EsClient.Search<S2FilteredEntity>(s => s.Index("kabaddis2data").Query(q => _objNestedquery)
                    .Aggregations(a => a.Terms("agg_E1", st => st.Script(p => p.Source("doc['" + KeyValue_E1.Key + ".keyword'].value + '|' + doc['" + KeyValue_E1.Value + ".keyword'].value + '|' + doc['entityId_3.keyword'].value"))
                    .Size(409846))));

                    var result_E2 = EsClient.Search<S2FilteredEntity>(s => s.Index("kabaddis2data").Query(q => _objNestedquery)
                    .Aggregations(a => a.Terms("agg_E2", st => st.Script(p => p.Source("doc['" + KeyValueS_E2.Key + ".keyword'].value + '|' + doc['" + KeyValueS_E2.Value + ".keyword'].value + '|' + doc['entityId_3.keyword'].value"))
                    .Size(409846))));

                    var result_E3 = EsClient.Search<S2FilteredEntity>(s => s.Index("kabaddis2data").Query(q => _objNestedquery)
                    .Aggregations(a => a.Terms("agg_E3", st => st.Script(p => p.Source("doc['" + KeyValueS_E3.Key + ".keyword'].value + '|' + doc['" + KeyValueS_E3.Value + ".keyword'].value + '|' + doc['entityId_3.keyword'].value"))
                    .Size(409846))));

                    var agg1 = result_E1.Aggregations.Terms("agg_E1").Buckets;
                    var agg2 = result_E2.Aggregations.Terms("agg_E2").Buckets;
                    var agg3 = result_E3.Aggregations.Terms("agg_E3").Buckets;

                    foreach (var items in agg1)
                    {
                        obj.Add(new S2FilteredEntity
                        {
                            EntityId_1 = items.Key.ToString().Split("|")[1],
                            EntityName_1 = items.Key.ToString().Split("|")[0],

                        });
                    }
                    foreach (var items in agg2)
                    {
                        obj.Add(new S2FilteredEntity
                        {
                            EntityId_2 = items.Key.ToString().Split("|")[1],
                            EntityName_2 = items.Key.ToString().Split("|")[0],

                        });
                    }
                    foreach (var items in agg3)
                    {
                        obj.Add(new S2FilteredEntity
                        {
                            EntityId_3 = items.Key.ToString().Split("|")[1],
                            EntityName_3 = items.Key.ToString().Split("|")[0],

                        });
                    }
                    _objFilterentityt = obj;
                    S2Result = _objFilterentityt;
                }
            }
            catch (Exception ex)
            {

            }
            return S2Result;
        }

        public override IEnumerable<SearchResultFilterData> returnSportResult(ElasticClient EsClient, QueryContainer _objNestedQuery, string IndexName)
        {
            throw new NotImplementedException();
        }

        public override List<SearchResultFilterData> SearchResultFilterDataMap(ISearchResponse<SearchCricketData> result)
        {
            throw new NotImplementedException();
        }

        public List<SearchResultFilterData> SearchResultFilterDataMap(ISearchResponse<SearchKabaddiData> result)
        {
            throw new NotImplementedException();
        }

        public dynamic SearchS1(QueryContainer _objNestedQuery, ElasticClient EsClient)
        {
            //QueryContainer qcIstagged = new TermQuery { Field = "isAsset", Value = "1" };
            //_objNestedQuery &= qcIstagged;
            dynamic result1 = null;
            IEnumerable<KabaddiResultDataTempdata> _objSearchResultFilterData = new List<KabaddiResultDataTempdata>();
            List<KabaddiResultDataTempdata> _objSearchResult = new List<KabaddiResultDataTempdata>();
            var result = EsClient.Search<KabaddiResultDataTempdata>(s => s.Index("kabaddi").Query(q => _objNestedQuery).Size(802407));
            var response = result.Hits;

            foreach (var items in result.Hits)
            {
                _objSearchResult.Add(new KabaddiResultDataTempdata
                {
                    ClearId = items.Source.ClearId.ToString(),
                    Id = items.Source.Id.ToString(),
                    ClearId2 = items.Source.ClearId2.ToString(),
                    ClearId3 = items.Source.ClearId3.ToString(),
                    ClearId4 = items.Source.ClearId4.ToString(),
                    ClearId5 = items.Source.ClearId5.ToString(),
                    ClearId6 = items.Source.ClearId6.ToString(),
                    MediaId = items.Source.MediaId.ToString(),
                    MediaId2 = items.Source.MediaId2.ToString(),
                    MediaId3 = items.Source.MediaId3.ToString(),
                    MediaId4 = items.Source.MediaId4.ToString(),
                    MediaId5 = items.Source.MediaId5.ToString(),
                    MediaId6 = items.Source.MediaId6.ToString(),
                    MatchId = items.Source.MatchId.ToString(),
                    MarkIn = items.Source.MarkIn.ToString(),
                    MarkOut = items.Source.MarkOut.ToString(),
                    ShortMarkIn = items.Source.ShortMarkIn.ToString(),
                    ShortMarkOut = items.Source.MarkOut.ToString(),
                    Description = items.Source.Description.ToString(),
                    Title = items.Source.Title.ToString(),
                    Duration = items.Source.Duration.ToString(),
                    IsAsset = items.Source.IsAsset.ToString(),
                    MatchDate = Convert.ToInt32(items.Source.MatchDate.ToString()),
                    LanguageId = items.Source.LanguageId.ToString(),
                    AssistType1 = items.Source.AssistType1.ToString(),
                    AssistType1Id = items.Source.AssistType1Id.ToString(),
                    AssistType2 = items.Source.AssistType2.ToString(),
                    AssistType2Id = items.Source.AssistType2Id.ToString(),
                    ClearId7 = items.Source.ClearId7.ToString(),
                    EventId = items.Source.EventId.ToString(),
                    EventName = items.Source.EventName.ToString(),
                    EventText = items.Source.EventText.ToString(),
                    MediaId7 = items.Source.MediaId7.ToString(),
                    TackleType = items.Source.TackleType.ToString(),
                    TackleTypeId = items.Source.TackleTypeId.ToString(),
                    TouchType = items.Source.TouchType.ToString(),
                    TouchTypeId = items.Source.TouchTypeId.ToString(),
                    NoOfDefenders = items.Source.NoOfDefenders.ToString()
                });
            }
                return _objSearchResult;
        }

        public dynamic SearchS1MasterData(QueryContainer _objNestedQuery, ElasticClient EsClient)
        {
            throw new NotImplementedException();
        }

        public override dynamic SearchS2(QueryContainer _objNestedquery, MatchDetail _objmatch, int Sportid = 6, string search = "")
        {
            string[] EntityName = new string[] { "EntityName_1", "EntityName_2", "EntityName_3", "EntityName_4", "EntityName_5" };
            dynamic SearchS2ResultData = null;
            string input = Convert.ToInt32(Convert.ToBoolean(_objmatch.IsAsset)).ToString();
            try
            {
                if (search != "")
                {

                    QueryContainer qShould = new QueryContainer();
                    foreach (var Entityname in EntityName)
                    {
                        QueryContainer q = new WildcardQuery { Field = Entityname, Value = (search.Trim() + "*") };
                        qShould |= q;
                    }
                    _objNestedquery &= qShould;
                    input = input.Trim();
                    QueryContainer query = new TermQuery { Field = "isAsset", Value = input };
                    _objNestedquery &= query;
                    SearchS2ResultData = MapS2Resuldata(_objNestedquery, search, _oLayer.CreateConnection());
                }
            }
            catch (Exception ex)
            {

            }
            return SearchS2ResultData;
        }
    }
}
