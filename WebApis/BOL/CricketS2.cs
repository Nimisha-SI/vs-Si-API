using System;
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
    public class CricketS2 : AbstractClasses, ICricketS2
    {

        private ESInterface _oLayer;
        CommonFunction objCf = new CommonFunction();
        public CricketS2(ESInterface oLayer) {
            _oLayer = oLayer;
        }
        public IEnumerable<SearchCricketResultData> MapcricketS1datascopy(List<SearchCricketResultTempData> _objs1Data, MatchDetail _objmatchdetail)
        {
            List<SearchCricketResultData> Final_result = new List<SearchCricketResultData>();
            string[] Id = _objmatchdetail.LanguageId.Split(',');
            switch (_objmatchdetail.IsAsset)
            {
                case true:
                    Final_result = MapcricketS1data(_objs1Data, "4");
                    break;
                case false:
                    if (string.IsNullOrEmpty(_objmatchdetail.LanguageId))
                    {
                        for (int i = 1; i <= 6; i++)
                        {
                            if (i == 1)
                            {
                                List<SearchCricketResultData> FinalTempResult = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId != "").ToList();
                                FinalTempResult = MapcricketS1data(temp, "1");
                                Final_result = FinalTempResult;
                                FinalTempResult = null;
                            }
                            else if (i == 2)
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId2 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "2");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (i == 3)
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId3 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "3");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (i == 4)
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId4 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "6");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (i == 5)
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId5 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "5");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (i == 6)
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId6 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "8");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                        }

                    }
                    else
                    {

                        for (int i = 0; i < Id.Count(); i++)
                        {
                            if (Id[i] == "1")
                            {
                                List<SearchCricketResultData> FinalTempResult = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId != "").ToList();
                                FinalTempResult = MapcricketS1data(temp, "1");
                                Final_result = FinalTempResult;
                                FinalTempResult = null;
                            }
                            else if (Id[i] == "2")
                            {
                                List<SearchCricketResultData> FinalTempResult = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId2 != "").ToList();
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                FinalTempResult1 = MapcricketS1data(temp, "2");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;

                            }
                            else if (Id[i] == "3")
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId3 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "3");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (Id[i] == "6")
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId4 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "6");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (Id[i] == "5")
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId5 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "5");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                            else if (Id[i] == "8")
                            {
                                List<SearchCricketResultData> FinalTempResult1 = new List<SearchCricketResultData>();
                                List<SearchCricketResultTempData> temp = new List<SearchCricketResultTempData>();
                                temp = _objs1Data.Where(t => t.ClearId6 != "").ToList();
                                FinalTempResult1 = MapcricketS1data(temp, "8");
                                Final_result = FinalTempResult1.Union(Final_result).ToList();
                                FinalTempResult1 = null;
                            }
                        }
                    }
                    break;
            }

            return Final_result;
        }
        public  List<SearchCricketResultData> MapcricketS1data(List<SearchCricketResultTempData> _objs1Data, string cases)
        {
            List<SearchCricketResultData> _objresult = new List<SearchCricketResultData>();
            try
            {



                switch (cases)
                {

                    case "1":
                        foreach (var str in _objs1Data)
                        {
                            SearchCricketResultData result = new SearchCricketResultData()
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
                                BatsmanRuns = str.BatsmanRuns,
                                BatsmanBallsFaced = str.BatsmanBallsFaced,
                                BowlerBallsBowled = str.BowlerBallsBowled,
                                BowlerWickets = str.BowlerWickets,
                                BowlerRunsConceeded = str.BowlerRunsConceeded,
                                TeamOver = str.TeamOver,
                                TeamScore = str.TeamScore,
                                ShotTypeId = str.ShotTypeId,
                                ShotType = str.ShotType,
                                ShotZoneId = str.ShotZoneId,
                                ShotZone = str.ShotZone,
                                Dismissal = str.Dismissal,
                                DismissalId = str.DismissalId,
                                DeliveryTypeId = str.DeliveryTypeId,
                                DeliveryType = str.DeliveryType,
                                BowlingLengthId = str.BowlingLengthId,
                                BowlingLength = str.BowlingLength,
                                BowlingLineId = str.BowlingLineId,
                                BowlingLine = str.BowlingLine,
                                FielderPositionId = str.FielderPositionId,
                                FielderPosition = str.FielderPosition,
                                BattingOrder = str.BattingOrder,
                                BowlingArm = str.BowlingArm,
                                LanguageId = "1"

                            };
                            _objresult.Add(result);
                        }
                        break;
                    case "2":
                        foreach (var str in _objs1Data)
                        {
                            SearchCricketResultData result = new SearchCricketResultData()
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
                                BatsmanRuns = str.BatsmanRuns,
                                BatsmanBallsFaced = str.BatsmanBallsFaced,
                                BowlerBallsBowled = str.BowlerBallsBowled,
                                BowlerWickets = str.BowlerWickets,
                                BowlerRunsConceeded = str.BowlerRunsConceeded,
                                TeamOver = str.TeamOver,
                                TeamScore = str.TeamScore,
                                ShotTypeId = str.ShotTypeId,
                                ShotType = str.ShotType,
                                ShotZoneId = str.ShotZoneId,
                                ShotZone = str.ShotZone,
                                Dismissal = str.Dismissal,
                                DeliveryTypeId = str.DeliveryTypeId,
                                DeliveryType = str.DeliveryType,
                                BowlingLengthId = str.BowlingLengthId,
                                BowlingLength = str.BowlingLength,
                                BowlingLineId = str.BowlingLineId,
                                BowlingLine = str.BowlingLine,
                                FielderPositionId = str.FielderPositionId,
                                FielderPosition = str.FielderPosition,
                                BattingOrder = str.BattingOrder,
                                BowlingArm = str.BowlingArm,
                                LanguageId = "2"
                            };
                            _objresult.Add(result);
                        }
                        break;
                    case "3":
                        foreach (var str in _objs1Data)
                        {
                            SearchCricketResultData result = new SearchCricketResultData()
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
                                BatsmanRuns = str.BatsmanRuns,
                                BatsmanBallsFaced = str.BatsmanBallsFaced,
                                BowlerBallsBowled = str.BowlerBallsBowled,
                                BowlerWickets = str.BowlerWickets,
                                BowlerRunsConceeded = str.BowlerRunsConceeded,
                                TeamOver = str.TeamOver,
                                TeamScore = str.TeamScore,
                                ShotTypeId = str.ShotTypeId,
                                ShotType = str.ShotType,
                                ShotZoneId = str.ShotZoneId,
                                ShotZone = str.ShotZone,
                                Dismissal = str.Dismissal,
                                DeliveryTypeId = str.DeliveryTypeId,
                                DeliveryType = str.DeliveryType,
                                BowlingLengthId = str.BowlingLengthId,
                                BowlingLength = str.BowlingLength,
                                BowlingLineId = str.BowlingLineId,
                                BowlingLine = str.BowlingLine,
                                FielderPositionId = str.FielderPositionId,
                                FielderPosition = str.FielderPosition,
                                BattingOrder = str.BattingOrder,
                                BowlingArm = str.BowlingArm,
                                LanguageId = "3"
                            };
                            _objresult.Add(result);
                        }

                        break;
                    case "6":
                        foreach (var str in _objs1Data)
                        {
                            SearchCricketResultData result = new SearchCricketResultData()
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
                                BatsmanRuns = str.BatsmanRuns,
                                BatsmanBallsFaced = str.BatsmanBallsFaced,
                                BowlerBallsBowled = str.BowlerBallsBowled,
                                BowlerWickets = str.BowlerWickets,
                                BowlerRunsConceeded = str.BowlerRunsConceeded,
                                TeamOver = str.TeamOver,
                                TeamScore = str.TeamScore,
                                ShotTypeId = str.ShotTypeId,
                                ShotType = str.ShotType,
                                ShotZoneId = str.ShotZoneId,
                                ShotZone = str.ShotZone,
                                Dismissal = str.Dismissal,
                                DeliveryTypeId = str.DeliveryTypeId,
                                DeliveryType = str.DeliveryType,
                                BowlingLengthId = str.BowlingLengthId,
                                BowlingLength = str.BowlingLength,
                                BowlingLineId = str.BowlingLineId,
                                BowlingLine = str.BowlingLine,
                                FielderPositionId = str.FielderPositionId,
                                FielderPosition = str.FielderPosition,
                                BattingOrder = str.BattingOrder,
                                BowlingArm = str.BowlingArm,
                                LanguageId = "6"
                            };
                            _objresult.Add(result);
                        }

                        break;
                    case "5":
                        foreach (var str in _objs1Data)
                        {
                            SearchCricketResultData result = new SearchCricketResultData()
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
                                BatsmanRuns = str.BatsmanRuns,
                                BatsmanBallsFaced = str.BatsmanBallsFaced,
                                BowlerBallsBowled = str.BowlerBallsBowled,
                                BowlerWickets = str.BowlerWickets,
                                BowlerRunsConceeded = str.BowlerRunsConceeded,
                                TeamOver = str.TeamOver,
                                TeamScore = str.TeamScore,
                                ShotTypeId = str.ShotTypeId,
                                ShotType = str.ShotType,
                                ShotZoneId = str.ShotZoneId,
                                ShotZone = str.ShotZone,
                                Dismissal = str.Dismissal,
                                DeliveryTypeId = str.DeliveryTypeId,
                                DeliveryType = str.DeliveryType,
                                BowlingLengthId = str.BowlingLengthId,
                                BowlingLength = str.BowlingLength,
                                BowlingLineId = str.BowlingLineId,
                                BowlingLine = str.BowlingLine,
                                FielderPositionId = str.FielderPositionId,
                                FielderPosition = str.FielderPosition,
                                BattingOrder = str.BattingOrder,
                                BowlingArm = str.BowlingArm,
                                LanguageId = "5"
                            };
                            _objresult.Add(result);
                        }
                        break;
                    case "8":
                        foreach (var str in _objs1Data)
                        {
                            SearchCricketResultData result = new SearchCricketResultData()
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
                                BatsmanRuns = str.BatsmanRuns,
                                BatsmanBallsFaced = str.BatsmanBallsFaced,
                                BowlerBallsBowled = str.BowlerBallsBowled,
                                BowlerWickets = str.BowlerWickets,
                                BowlerRunsConceeded = str.BowlerRunsConceeded,
                                TeamOver = str.TeamOver,
                                TeamScore = str.TeamScore,
                                ShotTypeId = str.ShotTypeId,
                                ShotType = str.ShotType,
                                ShotZoneId = str.ShotZoneId,
                                ShotZone = str.ShotZone,
                                Dismissal = str.Dismissal,
                                DeliveryTypeId = str.DeliveryTypeId,
                                DeliveryType = str.DeliveryType,
                                BowlingLengthId = str.BowlingLengthId,
                                BowlingLength = str.BowlingLength,
                                BowlingLineId = str.BowlingLineId,
                                BowlingLine = str.BowlingLine,
                                FielderPositionId = str.FielderPositionId,
                                FielderPosition = str.FielderPosition,
                                BattingOrder = str.BattingOrder,
                                BowlingArm = str.BowlingArm,
                                LanguageId = "8"
                            };
                            _objresult.Add(result);
                        }
                        break;
                    default:
                        foreach (var str in _objs1Data)
                        {
                            SearchCricketResultData result = new SearchCricketResultData()
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
                                BatsmanRuns = str.BatsmanRuns,
                                BatsmanBallsFaced = str.BatsmanBallsFaced,
                                BowlerBallsBowled = str.BowlerBallsBowled,
                                BowlerWickets = str.BowlerWickets,
                                BowlerRunsConceeded = str.BowlerRunsConceeded,
                                TeamOver = str.TeamOver,
                                TeamScore = str.TeamScore,
                                ShotTypeId = str.ShotTypeId,
                                ShotType = str.ShotType,
                                ShotZoneId = str.ShotZoneId,
                                ShotZone = str.ShotZone,
                                Dismissal = str.Dismissal,
                                DeliveryTypeId = str.DeliveryTypeId,
                                DeliveryType = str.DeliveryType,
                                BowlingLengthId = str.BowlingLengthId,
                                BowlingLength = str.BowlingLength,
                                BowlingLineId = str.BowlingLineId,
                                BowlingLine = str.BowlingLine,
                                FielderPositionId = str.FielderPositionId,
                                FielderPosition = str.FielderPosition,
                                BattingOrder = str.BattingOrder,
                                BowlingArm = str.BowlingArm,
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

        //public  IEnumerable<SearchCricketResultData> returnSportResult(ElasticClient EsClient, QueryContainer _objNestedQuery, string IndexName)
        //{
        //    List<SearchCricketResultData> Final_result = new List<SearchCricketResultData>();
        //    EsClient = oLayer.CreateConnection();
        //    searchcricket sc = new searchcricket();

        //    var result = EsClient.Search<SearchCricketData>(s => s.Index(IndexName).Query(q => _objNestedQuery).Size(409846));
        //    //Final_result = SearchResultFilterDataMap(result);
        //    return Final_result;
        //}

        //public  List<SearchResultFilterData> SearchResultFilterDataMap(ISearchResponse<SearchCricketData> result)
        //{
        //    List<SearchResultFilterData> ListObj = new List<SearchResultFilterData>();
        //    foreach (var hit in result.Hits)
        //    {
        //        ListObj.Add(new SearchResultFilterData
        //        {
        //            ClearId = hit.Source.ClearId.ToString(),
        //            Description = hit.Source.Description.ToString(),
        //            MarkIn = hit.Source.MarkIn.ToString(),
        //            MarkOut = hit.Source.MarkOut.ToString(),
        //            ShotType = hit.Source.MarkOut.ToString(),
        //            Duration = hit.Source.Duration.ToString(),
        //            DeliveryType = hit.Source.DeliveryType.ToString(),
        //            DeliveryTypeId = hit.Source.DeliveryTypeId.ToString(),
        //            EventId = hit.Source.EventId.ToString(),
        //            EventName = hit.Source.EventText.ToString(),
        //            Id = hit.Source.Id.ToString(),
        //            MatchId = hit.Source.MatchId.ToString(),
        //            MediaId = hit.Source.MediaId.ToString(),
        //            Title = hit.Source.Title.ToString(),
        //            ShotTypeId = hit.Source.ShotTypeId.ToString(),
        //        });
        //    }
        //    return ListObj;
        //}


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
                //if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                //{

                //    QueryContainer q5 = new TermQuery { Field = "team1Id", Value = _objMatchDetail.Team2Id.ToLower().ToString() };
                //    QueryContainer q6 = new TermQuery { Field = "team2Id", Value = _objMatchDetail.Team2Id.ToLower().ToString() };
                //    queryShould |= q5 |= q6;
                //    _objNestedQuery &= queryShould;
                //}

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

                //if (!string.IsNullOrEmpty(_objMatchDetail.LanguageId) && _objMatchDetail.LanguageId != "0")
                //{
                //    QueryContainer q19 = new TermQuery { Field = "languageId", Value = _objMatchDetail.LanguageId };
                //    _objNestedQuery &= q19;

                //}


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




                //string input = Convert.ToInt32(Convert.ToBoolean(_objMatchDetail.IsAsset)).ToString();
                //QueryContainer q18 = new TermQuery { Field = "isAsset", Value = input };
                //_objNestedQuery &= q18;
                _objNestedQuery &= queryShould;

            }

            return _objNestedQuery;
        }

        //public override Dictionary<string, object> bindS1andS2Dropdown(dynamic _objS1Data)
        //{
        //    Dictionary<string, object> ddlS2Dropwons = new Dictionary<string, object>();
        //    string[] _objReqShotTypes = _objS1Data.ShotType.Contains(",") ? _objReqShotTypes = _objS1Data.ShotType.Split(',') : _objReqShotTypes = new string[] { _objS1Data.ShotType };
        //    string[] _objReqShotZones = _objS1Data.ShotZone.Contains(",") ? _objReqShotZones = _objS1Data.ShotZone.Split(',') : _objReqShotZones = new string[] { _objS1Data.ShotZone };
        //    string[] _objReqDismissalTypes = _objS1Data.DismissalType.Contains(",") ? _objReqDismissalTypes = _objS1Data.DismissalType.Split(',') : _objReqDismissalTypes = new string[] { _objS1Data.DismissalType };
        //    string[] _objReqDeliveryType = _objS1Data.DeliveryType.Contains(",") ? _objReqDeliveryType = _objS1Data.DeliveryType.Split(',') : _objReqDeliveryType = new string[] { _objS1Data.DeliveryType };
        //    string[] _objReqBowlingLength = _objS1Data.BowlingLength.Contains(",") ? _objReqBowlingLength = _objS1Data.BowlingLength.Split(',') : _objReqBowlingLength = new string[] { _objS1Data.BowlingLength };
        //    string[] _objReqBowlingLine = _objS1Data.BowlingLine.Contains(",") ? _objReqBowlingLine = _objS1Data.BowlingLine.Split(',') : _objReqBowlingLine = new string[] { _objS1Data.BowlingLine };
        //    string[] _objReqFielderPosition = _objS1Data.FieldingPosition.Contains(",") ? _objReqFielderPosition = _objS1Data.FieldingPosition.Split(',') : _objReqFielderPosition = new string[] { _objS1Data.FieldingPosition };
        //    string[] _objReqBattingOrder = _objS1Data.BattingOrder.Contains(",") ? _objReqBattingOrder = _objS1Data.BattingOrder.Split(',') : _objReqBattingOrder = new string[] { _objS1Data.BattingOrder };
        //    string[] _objReqBowlingArm = _objS1Data.BowlingArm.Contains(",") ? _objReqBowlingArm = _objS1Data.BowlingArm.Split(',') : _objReqBowlingArm = new string[] { _objS1Data.BowlingArm };

        //    if (_objS1Data.ShotZone != "" && _objReqShotZones.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("shotZoneId", _objReqShotZones);
        //    }
        //    if (_objS1Data.ShotType != "" && _objReqShotTypes.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("shotTypeId", _objReqShotTypes);
        //    }
        //    if (_objS1Data.DismissalType != "" && _objReqDismissalTypes.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("dismissalTypeId", _objReqDismissalTypes);
        //    }
        //    if (_objS1Data.DeliveryType != "" && _objReqDeliveryType.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("deliveryTypeId", _objReqDeliveryType);
        //    }
        //    if (_objS1Data.BowlingLength != "" && _objReqBowlingLength.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("bowlingLengthId", _objReqBowlingLength);
        //    }
        //    if (_objS1Data.BowlingLine != "" && _objReqBowlingLine.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("bowlingLineId", _objReqBowlingLine);
        //    }
        //    if (_objS1Data.FieldingPosition != "" && _objReqFielderPosition.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("fieldingPositionId", _objReqFielderPosition);
        //    }
        //    if (_objS1Data.BattingOrder != "" && _objReqBattingOrder.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("battingOrder", _objReqBattingOrder);
        //    }
        //    if (_objS1Data.BowlingArm != "" && _objReqBowlingArm.Length > 0)
        //    {
        //        ddlS2Dropwons.Add("bowlingArm", _objReqBowlingArm);
        //    }
        //    return ddlS2Dropwons;
        //}


        public override Dictionary<string, object> bindS1andS2Dropdown(dynamic _objS1Data)
        {
            Dictionary<string, object> ddlS2Dropwons = new Dictionary<string, object>();
            string[] _objReqShotTypes = _objS1Data.ShotType.Contains(",") ? _objReqShotTypes = _objS1Data.ShotType.Split(',') : _objReqShotTypes = new string[] { _objS1Data.ShotType };
            string[] _objReqShotZones = _objS1Data.ShotZone.Contains(",") ? _objReqShotZones = _objS1Data.ShotZone.Split(',') : _objReqShotZones = new string[] { _objS1Data.ShotZone };
            string[] _objReqDismissalTypes = _objS1Data.DismissalType.Contains(",") ? _objReqDismissalTypes = _objS1Data.DismissalType.Split(',') : _objReqDismissalTypes = new string[] { _objS1Data.DismissalType };
            string[] _objReqDeliveryType = _objS1Data.DeliveryType.Contains(",") ? _objReqDeliveryType = _objS1Data.DeliveryType.Split(',') : _objReqDeliveryType = new string[] { _objS1Data.DeliveryType };
            string[] _objReqBowlingLength = _objS1Data.BowlingLength.Contains(",") ? _objReqBowlingLength = _objS1Data.BowlingLength.Split(',') : _objReqBowlingLength = new string[] { _objS1Data.BowlingLength };
            string[] _objReqBowlingLine = _objS1Data.BowlingLine.Contains(",") ? _objReqBowlingLine = _objS1Data.BowlingLine.Split(',') : _objReqBowlingLine = new string[] { _objS1Data.BowlingLine };
            string[] _objReqFielderPosition = _objS1Data.FieldingPosition.Contains(",") ? _objReqFielderPosition = _objS1Data.FieldingPosition.Split(',') : _objReqFielderPosition = new string[] { _objS1Data.FieldingPosition };
            string[] _objReqBattingOrder = _objS1Data.BattingOrder.Contains(",") ? _objReqBattingOrder = _objS1Data.BattingOrder.Split(',') : _objReqBattingOrder = new string[] { _objS1Data.BattingOrder };
            string[] _objReqBowlingArm = _objS1Data.BowlingArm.Contains(",") ? _objReqBowlingArm = _objS1Data.BowlingArm.Split(',') : _objReqBowlingArm = new string[] { _objS1Data.BowlingArm };

            if (_objS1Data.ShotZone != "" && _objReqShotZones.Length > 0)
            {
                string strReqShotZones = objCf.ConvertStringArrayToString(_objReqShotZones);
                ddlS2Dropwons.Add("shotZoneId", strReqShotZones);
            }
            if (_objS1Data.ShotType != "" && _objReqShotTypes.Length > 0)
            {
                string strReqShotTypes = objCf.ConvertStringArrayToString(_objReqShotTypes);
                ddlS2Dropwons.Add("shotTypeId", strReqShotTypes);
            }
            if (_objS1Data.DismissalType != "" && _objReqDismissalTypes.Length > 0)
            {
                string strReqDismissalTypes = objCf.ConvertStringArrayToString(_objReqDismissalTypes);
                ddlS2Dropwons.Add("dismissalTypeId", strReqDismissalTypes);
            }
            if (_objS1Data.DeliveryType != "" && _objReqDeliveryType.Length > 0)
            {
                string strReqDeliveryType = objCf.ConvertStringArrayToString(_objReqDeliveryType);
                ddlS2Dropwons.Add("deliveryTypeId", strReqDeliveryType);
            }
            if (_objS1Data.BowlingLength != "" && _objReqBowlingLength.Length > 0)
            {
                string strReqBowlingLength = objCf.ConvertStringArrayToString(_objReqBowlingLength);
                ddlS2Dropwons.Add("bowlingLengthId", strReqBowlingLength);
            }
            if (_objS1Data.BowlingLine != "" && _objReqBowlingLine.Length > 0)
            {
                string strReqBowlingLine = objCf.ConvertStringArrayToString(_objReqBowlingLine);
                ddlS2Dropwons.Add("bowlingLineId", strReqBowlingLine);
            }
            if (_objS1Data.FieldingPosition != "" && _objReqFielderPosition.Length > 0)
            {
                string strReqFielderPosition = objCf.ConvertStringArrayToString(_objReqFielderPosition);
                ddlS2Dropwons.Add("fieldingPositionId", strReqFielderPosition);
            }
            if (_objS1Data.BattingOrder != "" && _objReqBattingOrder.Length > 0)
            {
                string strReqBowlingLength = objCf.ConvertStringArrayToString(_objReqBattingOrder);
                ddlS2Dropwons.Add("battingOrder", strReqBowlingLength);
            }
            if (_objS1Data.BowlingArm != "" && _objReqBowlingArm.Length > 0)
            {
                string strReqBowlingArm = objCf.ConvertStringArrayToString(_objReqBowlingArm);
                ddlS2Dropwons.Add("bowlingArm", strReqBowlingArm);
            }
            return ddlS2Dropwons;
        }

        public override QueryContainer GetPlayerDetails(dynamic _objS1Data, QueryContainer qFinal, List<string> valueObj, int sportid, bool isMasterData = false)
        {
            CommonFunction objCf = new CommonFunction();
            QueryContainer qShould = new QueryContainer();
            QueryContainer queryShouldS = new QueryContainer();
            //QueryContainer queryShould = new QueryContainer();
            QueryContainer queryShouldB = new QueryContainer();
            QueryContainer queryAnd_should = new QueryContainer();
            if (_objS1Data != null)
            {
                if (_objS1Data.IsDefault != null && Convert.ToBoolean(_objS1Data.IsDefault))
                {
                    //QueryContainer query = new QueryContainer();
                    string[] isDefaultvalues = objCf.ArrayIsDefaultForSport(sportid);
                    for (int i = 0; i <= isDefaultvalues.Length - 1; i++)
                    {
                        if (isDefaultvalues[i] != "shotTypeId" && isDefaultvalues[i] != "deliveryTypeId")
                        {
                            QueryContainer query1 = new TermQuery { Field = isDefaultvalues[i], Value = 1 };
                            qShould |= query1;
                        }

                    }
                    qFinal &= qShould;
                }
                else
                {
                    if (_objS1Data != null)
                    {

                        if (!string.IsNullOrEmpty(_objS1Data.BatsmanID))
                        {
                            QueryContainer q1 = new TermQuery { Field = "batsmanId", Value = _objS1Data.BatsmanID };
                            qFinal &= q1;
                            //_objNestedQuery.Add(query, Occur.MUST);
                        }
                        if (_objS1Data.BatsmanFours || _objS1Data.BatsmanSixes || _objS1Data.BatsmanDots || _objS1Data.BatsmanEdged || _objS1Data.BastsmanBeaten || _objS1Data.BatsmanDismissal || _objS1Data.BatsmanAppeal)
                        {

                            if (_objS1Data.BatsmanFours)
                            {
                                QueryContainer q2 = new TermQuery { Field = "isFour", Value = 1 };
                                qShould |= q2;

                            }
                            if (_objS1Data.BatsmanSixes)
                            {
                                QueryContainer q3 = new TermQuery { Field = "isSix", Value = 1 };
                                qShould |= q3;
                            }
                            if (_objS1Data.BatsmanDots)
                            {

                                QueryContainer q4 = new TermQuery { Field = "isDot", Value = 1 };
                                qShould |= q4;
                            }
                            if (_objS1Data.BatsmanEdged)
                            {


                                QueryContainer q5 = new TermQuery { Field = "isEdged", Value = 1 };
                                qShould |= q5;
                            }
                            if (_objS1Data.BastsmanBeaten)
                            {

                                QueryContainer q6 = new TermQuery { Field = "isBeaten", Value = 1 };
                                qShould |= q6;

                            }
                            if (_objS1Data.BatsmanDismissal)
                            {
                                QueryContainer q7 = new TermQuery { Field = "isWicket", Value = 1 };
                                qShould |= q7;
                            }
                            if (_objS1Data.BatsmanAppeal)
                            {
                                QueryContainer q8 = new TermQuery { Field = "isAppeal", Value = 1 };
                                qShould |= q8;

                            }
                            //_objNestedQuery.Add(Bq, Occur.MUST);
                        }
                        if (!string.IsNullOrEmpty(_objS1Data.ShotType) && !isMasterData)
                        {

                            string Dlist = _objS1Data.ShotType;
                            string[] strnumbers = Dlist.Split(',');
                            foreach (string str in strnumbers)
                            {

                                QueryContainer q9 = new TermQuery { Field = "shotTypeId", Value = str };
                                qShould |= q9;
                            }

                        }
                        if (!string.IsNullOrEmpty(_objS1Data.ShotZone) && !isMasterData)
                        {
                            //BooleanQuery bq = new BooleanQuery();
                            string Dlist = _objS1Data.ShotZone;
                            string[] strnumbers = Dlist.Split(',');
                            foreach (string str in strnumbers)
                            {
                                QueryContainer q10 = new TermQuery { Field = "shotZoneId", Value = str };
                                qShould |= q10;
                            }
                            //_objNestedQuery.Add(bq, Occur.MUST);
                        }
                        if (!string.IsNullOrEmpty(_objS1Data.DismissalType) && !isMasterData)
                        {

                            string Dlist = _objS1Data.DismissalType;
                            string[] strnumbers = Dlist.Split(',');
                            foreach (string str in strnumbers)
                            {

                                QueryContainer q11 = new TermQuery { Field = "dismissalId", Value = str };
                                qShould |= q11;
                            }

                        }
                        if (_objS1Data.FielderStumping)
                        {

                            QueryContainer q12 = new TermQuery { Field = "dismissalId", Value = "st" };
                            qFinal &= q12;
                        }
                        if (!string.IsNullOrEmpty(_objS1Data.BowlerID))
                        {


                            QueryContainer q13 = new TermQuery { Field = "bowlerId", Value = _objS1Data.BowlerID };
                            qFinal &= q13;
                        }
                        if (_objS1Data.BowlerWides || _objS1Data.BowlerNoBalls || _objS1Data.BowlerBeaten || _objS1Data.BowlerDots || _objS1Data.BowlerEdged || _objS1Data.BowlerDismissal || _objS1Data.BowlerAppeal)
                        {
                            ;
                            if (_objS1Data.BowlerWides)
                            {


                                QueryContainer q14 = new TermQuery { Field = "isWide", Value = "1" };
                                qShould |= q14;
                            }
                            if (_objS1Data.BowlerNoBalls)
                            {

                                QueryContainer q15 = new TermQuery { Field = "isNoBall", Value = "1" };
                                qShould |= q15;
                            }
                            if (_objS1Data.BowlerDots)
                            {

                                QueryContainer q16 = new TermQuery { Field = "isDot", Value = "1" };
                                qShould |= q16;
                            }
                            if (_objS1Data.BowlerEdged)
                            {
                                QueryContainer q17 = new TermQuery { Field = "isEdged", Value = "1" };
                                qShould |= q17;
                            }
                            if (_objS1Data.BowlerBeaten)
                            {
                                QueryContainer q18 = new TermQuery { Field = "isBeaten", Value = "1" };
                                qShould |= q18;
                            }
                            if (_objS1Data.BowlerDismissal)
                            {

                                QueryContainer q19 = new TermQuery { Field = "isWicket", Value = "1" };
                                qShould |= q19;

                            }
                            if (_objS1Data.BowlerAppeal)
                            {
                                QueryContainer q20 = new TermQuery { Field = "isAppeal", Value = "1" };
                                qShould |= q20;
                            }

                        }
                        if (!string.IsNullOrEmpty(_objS1Data.DeliveryType) && !isMasterData)
                        {

                            string Dlist = _objS1Data.DeliveryType;
                            string[] strnumbers = Dlist.Split(',');
                            foreach (string str in strnumbers)
                            {
                                QueryContainer q21 = new TermQuery { Field = "deliveryTypeId", Value = str };
                                qShould |= q21;
                            }

                        }
                        if (!string.IsNullOrEmpty(_objS1Data.BowlingLength) && !isMasterData)
                        {

                            string Dlist = _objS1Data.BowlingLength;
                            string[] strnumbers = Dlist.Split(',');
                            foreach (string str in strnumbers)
                            {
                                QueryContainer q22 = new TermQuery { Field = "bowlingLengthId", Value = str };
                                qShould |= q22;
                            }

                        }
                        if (!string.IsNullOrEmpty(_objS1Data.BowlingLine) && !isMasterData)
                        {

                            string Dlist = _objS1Data.BowlingLine;
                            string[] strnumbers = Dlist.Split(',');
                            foreach (string str in strnumbers)
                            {
                                QueryContainer q23 = new TermQuery { Field = "bowlingLineId", Value = str };
                                qShould |= q23;
                            }

                        }
                        if (_objS1Data.BowlingOver || _objS1Data.BowlingRound)
                        {
                            if (_objS1Data.BowlingOver)
                            {

                                QueryContainer q24 = new TermQuery { Field = "isOverTheWicket", Value = "1" };
                                qShould |= q24;
                            }
                            if (_objS1Data.BowlingRound)
                            {
                                QueryContainer q25 = new TermQuery { Field = "isRoundTheWicket", Value = "1" };
                                qShould |= q25;

                            }

                        }
                        if (_objS1Data.BowlerSpin || _objS1Data.BowlerPace)
                        {

                            if (_objS1Data.BowlerSpin)
                            {
                                QueryContainer q26 = new TermQuery { Field = "skill", Value = "s" };
                                qShould |= q26;

                            }

                            if (_objS1Data.BowlerPace)
                            {


                                QueryContainer q27 = new TermQuery { Field = "skill", Value = "p" };
                                qShould |= q27;
                            }

                        }
                        if (!string.IsNullOrEmpty(_objS1Data.FielderID))
                        {


                            QueryContainer q27 = new TermQuery { Field = "fielderId", Value = _objS1Data.FielderID };
                            qFinal &= q27;
                        }
                        if (_objS1Data.FielderCatch || _objS1Data.FielderRunOut || _objS1Data.FielderDrops || _objS1Data.FielderMisFields)
                        {

                            if (_objS1Data.FielderCatch)
                            {

                                QueryContainer q28 = new TermQuery { Field = "isCatch", Value = "1" };
                                qShould |= q28;
                            }
                            if (_objS1Data.FielderRunOut)
                            {

                                QueryContainer q29 = new TermQuery { Field = "isRunOut", Value = "1" };
                                qShould |= q29;
                            }
                            if (_objS1Data.FielderDrops)
                            {
                                QueryContainer q30 = new TermQuery { Field = "isDropped", Value = "1" };
                                qShould |= q30;
                            }
                            if (_objS1Data.FielderMisFields)
                            {

                                QueryContainer q31 = new TermQuery { Field = "isMisField", Value = "1" };
                                qShould |= q31;
                            }

                        }
                        if (!string.IsNullOrEmpty(_objS1Data.FieldingPosition) && !isMasterData)
                        {

                            string Dlist = _objS1Data.FieldingPosition;
                            string[] strnumbers = Dlist.Split(',');
                            foreach (string str in strnumbers)
                            {
                                QueryContainer q32 = new TermQuery { Field = "fielderPositionId", Value = str };
                                qShould |= q32;
                            }

                        }
                        if (!string.IsNullOrEmpty(_objS1Data.RunsSaved) && _objS1Data.RunsSaved != "0")
                        {
                            QueryContainer q33 = new TermQuery { Field = "runsSaved", Value = Convert.ToString(_objS1Data.RunsSaved) };
                            qFinal &= q33;

                        }
                        if (!string.IsNullOrEmpty(_objS1Data.BattingOrder) && !isMasterData)
                        {
                            string Dlist = _objS1Data.BattingOrder;
                            string[] strnumbers = Dlist.Split(',');
                            foreach (string str in strnumbers)
                            {
                                QueryContainer q34 = new TermQuery { Field = "battingOrder", Value = str };
                                qShould |= q34;
                            }

                        }
                        if (!string.IsNullOrEmpty(_objS1Data.BowlingArm) && !isMasterData)
                        {
                            string Dlist = _objS1Data.BowlingArm;
                            string[] strnumbers = Dlist.Split(',');
                            foreach (string str in strnumbers)
                            {
                                QueryContainer q35 = new TermQuery { Field = "bowlingArm", Value = str.ToLower() };
                                qShould |= q35;
                            }

                        }
                    }

                }


            }


            qFinal &= qShould;
            return qFinal;
        }

        public QueryContainer GetCricketMatchSituationQuery(MatchDetail _objMatchDetail, PlayerDetail _objPlayerDetail, MatchSituation _objMatchSituation, QueryContainer _objNestedQuery)
        {
            QueryContainer qcShould = new QueryContainer();
            QueryContainer qcAnd = new QueryContainer();

            if (_objMatchSituation != null)
            {
                if (!string.IsNullOrEmpty(_objMatchSituation.Result))
                {
                    SearchQueryModel _objSqModel = new SearchQueryModel();
                    if (_objMatchSituation.Result.Contains("Wins"))
                    {
                        if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id) || !string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                        {

                            if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id))
                            {
                                QueryContainer q1 = new TermQuery { Field = "winner", Value = _objMatchDetail.Team1Id };
                                qcAnd &= q1;
                            }
                            else if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                            {
                                QueryContainer q2 = new TermQuery { Field = "winner", Value = _objMatchDetail.Team2Id };
                                qcAnd &= q2;
                            }

                        }
                        if (!string.IsNullOrEmpty(_objPlayerDetail.BatsmanID))
                        {

                            QueryContainer q3 = new TermQuery { Field = "winnerPlayer", Value = _objPlayerDetail.BatsmanID };
                            qcAnd &= q3;
                        }
                        if (!string.IsNullOrEmpty(_objPlayerDetail.BowlerID))
                        {

                            QueryContainer q4 = new TermQuery { Field = "winnerPlayer", Value = _objPlayerDetail.BowlerID };
                            qcAnd &= q4;
                        }
                        if (!string.IsNullOrEmpty(_objPlayerDetail.FielderID))
                        {
                            QueryContainer q5 = new TermQuery { Field = "winningFielder", Value = _objPlayerDetail.FielderID };
                            qcAnd &= q5;
                        }
                    }
                    else if (_objMatchSituation.Result.Contains("Losses"))
                    {
                        if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id) || !string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                        {

                            if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id))
                            {
                                QueryContainer q6 = new TermQuery { Field = "loser", Value = _objMatchDetail.Team1Id };
                                qcAnd &= q6;

                            }
                            else if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                            {
                                QueryContainer q7 = new TermQuery { Field = "loser", Value = _objMatchDetail.Team2Id };
                                qcAnd &= q7;
                            }
                        }
                        if (!string.IsNullOrEmpty(_objPlayerDetail.BatsmanID))
                        {

                            QueryContainer q8 = new TermQuery { Field = "loserPlayer", Value = _objPlayerDetail.BatsmanID };
                            qcAnd &= q8;
                        }
                        if (!string.IsNullOrEmpty(_objPlayerDetail.BowlerID))
                        {
                            QueryContainer q9 = new TermQuery { Field = "loserPlayer", Value = _objPlayerDetail.BowlerID };
                            qcAnd &= q9;
                        }
                        if (!string.IsNullOrEmpty(_objPlayerDetail.FielderID))
                        {
                            QueryContainer q10 = new TermQuery { Field = "winningFielder", Value = _objPlayerDetail.FielderID };
                            qcAnd &= q10;
                        }
                    }
                    else
                    {

                        string matchresult = string.Empty;
                        if (_objMatchSituation.Result.Contains("No Result"))
                        {
                            matchresult = "No Result";
                            QueryContainer q11 = new TermQuery { Field = "matchResult", Value = matchresult.ToLower() };
                            qcAnd &= q11;
                        }
                        else if (_objMatchSituation.Result.Contains("Draw"))
                        {
                            matchresult = "Drawn";
                            QueryContainer q12 = new TermQuery { Field = "matchResult", Value = matchresult.ToLower() };
                            qcAnd &= q12;
                        }
                        else if (_objMatchSituation.Result.Contains("Tie"))
                        {
                            matchresult = "Tied";
                            QueryContainer q13 = new TermQuery { Field = "matchResult", Value = matchresult.ToLower() };
                            qcAnd &= q13;
                        }

                    }
                }
                if (!string.IsNullOrEmpty(_objMatchSituation.Innings))
                {
                    QueryContainer q14 = new TermQuery { Field = "innings", Value = _objMatchSituation.Innings };
                    qcAnd &= q14;
                }

                if (!string.IsNullOrEmpty(_objMatchSituation.MatchInstance))
                {

                    if (_objMatchSituation.MatchInstance.Contains("Powerplay"))
                    {

                        QueryContainer q15 = new TermQuery { Field = "isPowerPlayOver", Value = 1 };
                        qcAnd &= q15;
                    }
                    else if (_objMatchSituation.MatchInstance.Contains("Middle"))
                    {
                        QueryContainer q16 = new TermQuery { Field = "is_middle", Value = 1 };
                        qcAnd &= q16;
                    }
                    else if (_objMatchSituation.MatchInstance.Contains("Death"))
                    {
                        QueryContainer q17 = new TermQuery { Field = "is_death", Value = 1 };
                        qcAnd &= q17;
                    }
                    else if (_objMatchSituation.MatchInstance.Contains("Final Over"))
                    {
                        QueryContainer q18 = new TermQuery { Field = "is_lastover", Value = 1 };
                        qcAnd &= q18;
                    }
                    else if (_objMatchSituation.MatchInstance.Contains("Last Ball"))
                    {
                        QueryContainer q19 = new TermQuery { Field = "is_lastBall", Value = 1 };
                        qcAnd &= q19;
                    }

                }
            }



            return _objNestedQuery;
        }

        public override dynamic getFinalResult(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, ElasticClient EsClient, string sportid = "1") {

            string input = Convert.ToInt32(Convert.ToBoolean(_objMatchDetail.IsAsset)).ToString();
            QueryContainer query = new TermQuery { Field = "isAsset", Value = input };
            _objNestedQuery &= query;
            dynamic result;
            List<S2MasterData> lstsearchresults = new List<S2MasterData>();
            var results2MasterData = EsClient.Search<S2MasterData>(s => s.Index("crickets2data").Query(q => _objNestedQuery)
            .Aggregations(a => a.Terms("agg_E1", st => st.Script(p => p.Source("doc['attribute_Id_Level1.keyword'].value + '|' + doc['attribute_Name_Level1.keyword'].value + '|' + doc['attribute_Id_Level2.keyword'].value + '|' + doc['attribute_Name_Level2.keyword'].value + '|' + doc['attribute_Id_Level3.keyword'].value + '|' + doc['attribute_Name_Level3.keyword'].value + '|' + doc['attribute_Id_Level4.keyword'].value + '|' + doc['attribute_Name_Level4.keyword'].value + '|' + doc['emotionId.keyword'].value  + '|' + doc['emotionName.keyword'].value"))
            .Size(409846))));

            //var results2MasterData1 = EsClient.Search<S2MasterData>(s => s.Index("crickets2data").Query(q => _objNestedQuery)
            //.Aggregations(a => a.Terms("agg_E1", st => st.Script(p => p.Source("doc['attribute_Id_Level1.keyword'].value + '|' + doc['attribute_Name_Level1.keyword'].value"))
            //.Size(409846))));

            //var results2MasterData2 = EsClient.Search<S2MasterData>(s => s.Index("crickets2data").Query(q => _objNestedQuery)
            //.Aggregations(a => a.Terms("agg_E2", st => st.Script(p => p.Source("doc['attribute_Id_Level2.keyword'].value + '|' + doc['attribute_Name_Level2.keyword'].value"))
            //.Size(409846))));


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

        public override IEnumerable<SearchResultFilterData> returnSportResult(ElasticClient EsClient, QueryContainer _objNestedQuery, string IndexName)
        {
            throw new NotImplementedException();
        }

        public override List<SearchResultFilterData> SearchResultFilterDataMap(ISearchResponse<SearchCricketData> result)
        {
            throw new NotImplementedException();
        }

        public int GetPlayerMatchDetailsMaxCount(QueryContainer _objNestedQuery, ElasticClient EsClient, string sType)
        {
            int count = 0;
            //var response = ISearchResponse<SearchCricketData>();
            switch (sType)
            {
                case "MaxBatsmanScore":
                    var resRuns = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).
                     Query(q => _objNestedQuery)
                    .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.BatsmanRuns.Suffix("keyword")).Size(802407)
                    )));
                    var aggRuns = resRuns.Aggregations.Terms("commit_count");
                    count = aggRuns.Buckets.Count;
                    break;
                case "MaxBatsmanBalls":
                    var resFaced = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).
                         Query(q => _objNestedQuery)
                        .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.BatsmanBallsFaced.Suffix("keyword")).Size(802407)
                        )));
                    var aggFaced = resFaced.Aggregations.Terms("commit_count");
                    count = aggFaced.Buckets.Count;
                    break;

                case "MaxBowlerWickets":
                    var resWicket = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).
                         Query(q => _objNestedQuery)
                        .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.BowlerWickets.Suffix("keyword")).Size(802407)
                        )));
                    var aggWicket = resWicket.Aggregations.Terms("commit_count");
                    count = aggWicket.Buckets.Count;
                    break;
                case "MaxBowlerBalls":
                    var resBowled = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).
                         Query(q => _objNestedQuery)
                        .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.BowlerBallsBowled.Suffix("keyword")).Size(802407)
                        )));
                    var aggBowled = resBowled.Aggregations.Terms("commit_count");
                    count = aggBowled.Buckets.Count;
                    break;
                case "MaxBowlerRuns":
                    var resConceeded = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).
                         Query(q => _objNestedQuery)
                        .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.BowlerRunsConceeded.Suffix("keyword")).Size(802407)
                        )));
                    var aggConceeded = resConceeded.Aggregations.Terms("commit_count");
                    count = aggConceeded.Buckets.Count;
                    break;
                case "MaxTeamScore":
                    var resTeamScore = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).
                         Query(q => _objNestedQuery)
                        .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.TeamScore.Suffix("keyword")).Size(802407)
                        )));
                    var aggTeamScore = resTeamScore.Aggregations.Terms("commit_count");
                    count = aggTeamScore.Buckets.Count;
                    break;
                case "MaxTeamOver":
                    var resOver = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).
                         Query(q => _objNestedQuery)
                        .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.BatsmanBallsFaced.Suffix("keyword")).Size(802407)
                        )));
                    var aggOver = resOver.Aggregations.Terms("commit_count");
                    count = aggOver.Buckets.Count;
                    break;

            }
            return count;
        }
        //public int getMatchCount(QueryContainer _objNestedQuery, ElasticClient EsClient, string sType)
        //{
        //    int Count = 0;
        //    if (sType == "Matches")
        //    {
        //        var response = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).
        //        Query(q => _objNestedQuery)
        //        .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.MatchId.Suffix("keyword")).Size(802407)
        //        )));
        //        var agg = response.Aggregations.Terms("commit_count");
        //        Count = agg.Buckets.Count;
        //    }
        //    else if (sType == "Videos")
        //    {
        //       // QueryContainer TempQuery = new QueryContainer(); 
        //        QueryContainer qMust = new TermQuery { Field = "isAsset", Value = "0" };

        //        _objNestedQuery &= qMust;
        //        var response = EsClient.Search<SearchCricketResultTempData>(a => a.Index("cricket").Size(0). //SearchS2Data
        //        Query(q => _objNestedQuery)
        //       .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.IsAsset.Suffix("keyword")).Size(802407)
        //       )));
        //        var agg = response.Aggregations.Terms("commit_count");
        //        Count = agg.Buckets.Count;

        //    }
        //    else if (sType == "Assets")
        //    {
        //       // QueryContainer TempQuery = new QueryContainer();
        //        QueryContainer qMust = new TermQuery { Field="isAsset", Value="1"};
        //        //TempQuery = _objNestedQuery;
        //        _objNestedQuery &= qMust;
        //        var response = EsClient.Search<SearchCricketResultTempData>(a => a.Index("cricket").Size(0). //SearchS2Data
        //         Query(q => _objNestedQuery)
        //         .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.MatchId.Suffix("keyword")).Size(802407)
        //         )));
        //        var agg = response.Aggregations.Terms("commit_count");
        //        Count = agg.Buckets.Count;

        //    }


        //    return Count;
        //}

        public int getMatchCount(QueryContainer _objNestedQuery, ElasticClient EsClient, string sType)
        {

            int Count = 0;
            if (sType == "Matches")
            {
                var response = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).
                Query(q => _objNestedQuery)
                .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.MatchId.Suffix("keyword")).Size(802407)//409846 802407
                )));
                var agg = response.Aggregations.Terms("commit_count");
                Count = agg.Buckets.Count;
                //DistinctCountDetector distinctCountDetector = new dis
            }
            else if (sType == "Videos")
            {
                QueryContainer TempQuery = new QueryContainer();
                QueryContainer qMust = new TermQuery { Field = "isAsset", Value = "0" };
                TempQuery = _objNestedQuery;
                TempQuery &= qMust;
                // var response = EsClient.Search<SearchCricketResultTempData>(a => a.Index("cricket").Size(0). //SearchS2Data
                // Query(q => TempQuery)
                //.Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.IsAsset.Suffix("keyword")).Size(802407)
                //)));
                // var agg = response.Aggregations.Terms("commit_count");
                // Count = agg.Buckets.Count;


                // var response1 = EsClient.Search<SearchCricketData>(a => a.Index("cricket"). //SearchS2Data
                //Query(q => TempQuery).Size(802407)
                //var result = EsClient.Search<SearchCricketData>(s => s.Index("cricket").From(0).Take(10000).Scroll("2m").Query(q => TempQuery).Size(10000));
                //List<SearchCricketData> results = new List<SearchCricketData>();
                //if (!result.IsValid || string.IsNullOrEmpty(result.ScrollId))
                //    throw new Exception(result.ServerError.Error.Reason);
                //if (result.Documents.Any())
                //    results.AddRange(result.Documents);
                //string scrollid = result.ScrollId;
                //bool isScrollSetHasData = true;
                //while (isScrollSetHasData)
                //{
                //    ISearchResponse<SearchCricketData> loopingResponse = EsClient.Scroll<SearchCricketData>("2m", scrollid);
                //    if (loopingResponse.IsValid)
                //    {
                //        results.AddRange(loopingResponse.Documents);
                //        scrollid = loopingResponse.ScrollId;
                //    }
                //    isScrollSetHasData = loopingResponse.Documents.Any();
                //}
                //.Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.IsAsset.Suffix("keyword")).Size(802407)
                //)
                //)
                //);
                //  Count = Convert.ToInt32(result.Hits);
                CountRequest countRequest = new CountRequest()
                {

                    Query = TempQuery
                };
                var result = EsClient.Count(countRequest).Count;

                //var agg = result.Aggregations.Terms("commit_count");
                Count = Convert.ToInt32(result);

            }
            else if (sType == "Assets")
            {
                QueryContainer TempQuery = new QueryContainer();
                QueryContainer qMust = new TermQuery { Field = "isAsset", Value = "1" };
                TempQuery = _objNestedQuery;
                TempQuery &= qMust;

                //var response = EsClient.Search<SearchCricketResultTempData>(a => a.Index("cricket").Size(0). //SearchS2Data
                // Query(q => TempQuery)
                // .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.MatchId.Suffix("keyword")).Size(802407)
                // )));

                //var response = EsClient.Search<SearchCricketData>(a => a.Index("cricket"). //SearchS2Data
                //Query(q => TempQuery).Size(802407)
                //// .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.MatchId.Suffix("keyword")).Size(802407)
                ////))
                //);
                // var result = EsClient.Search<SearchCricketData>(s => s.Index("cricket").Query(q => TempQuery).Sort(a => a.Ascending(p => p.Id.Suffix("keyword"))).Size(10000));
                // var result = EsClient.coun<SearchCricketData>(s => s.Index("cricket").Query(q => TempQuery).Size(802407));
                CountRequest countRequest = new CountRequest()
                {

                    Query = TempQuery
                };
                var result = EsClient.Count(countRequest).Count;

                //var agg = result.Aggregations.Terms("commit_count");
                Count = Convert.ToInt32(result);

            }


            return Count;
        }


        public dynamic SearchS1(QueryContainer _objNestedQuery, ElasticClient EsClient)
        {

            //QueryContainer qcIstagged = new TermQuery { Field = "isAsset", Value = "1" };
            //_objNestedQuery &= qcIstagged;
            dynamic result1 = null;
            IEnumerable<SearchCricketResultTempData> _objSearchResultFilterData = new List<SearchCricketResultTempData>();
            List<SearchCricketResultTempData> _objSearchResult = new List<SearchCricketResultTempData>();
            var result = EsClient.Search<SearchCricketResultTempData>(s => s.Index("cricket").Query(q => _objNestedQuery).Size(802407));
            var response = result.Hits;

            foreach (var items in result.Hits)
            {
                _objSearchResult.Add(new SearchCricketResultTempData
                {
                    ClearId = items.Source.ClearId.ToString(),
                    Id = items.Source.Id.ToString(),
                    ClearId2= items.Source.ClearId2.ToString(),
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
                    BatsmanRuns = Convert.ToInt32(items.Source.BatsmanRuns.ToString()),
                    BatsmanBallsFaced = Convert.ToInt32(items.Source.BatsmanBallsFaced.ToString()),
                    BowlerBallsBowled = Convert.ToInt32(items.Source.BowlerBallsBowled.ToString()),
                    BowlerWickets = Convert.ToInt32(items.Source.BowlerWickets.ToString()),
                    BowlerRunsConceeded = Convert.ToInt32(items.Source.BowlerRunsConceeded.ToString()),
                    TeamOver = Convert.ToInt32(items.Source.TeamOver.ToString()),
                    TeamScore = Convert.ToInt32(items.Source.TeamScore.ToString()),
                    IsAsset = items.Source.IsAsset.ToString(),
                    MatchDate = Convert.ToInt32(items.Source.MatchDate.ToString()),
                    ShotTypeId = items.Source.ShotTypeId.ToString(),
                    ShotType = items.Source.ShotType.ToString(),
                    ShotZoneId = items.Source.ShotZoneId.ToString(),
                    ShotZone = items.Source.ShotZone.ToString(),
                    Dismissal = items.Source.Dismissal.ToString(),
                    DismissalId = items.Source.DismissalId.ToString(),
                    DeliveryTypeId = items.Source.DeliveryTypeId.ToString(),
                    DeliveryType = items.Source.DeliveryType.ToString(),
                    BowlingLengthId = items.Source.BowlingLengthId.ToString(),
                    BowlingLine = items.Source.BowlingLine.ToString(),
                    BowlingLineId = items.Source.BowlingLineId.ToString(),
                    BowlingLength = items.Source.BowlingLength.ToString(),
                    FielderPosition = items.Source.FielderPosition.ToString(),
                    FielderPositionId = items.Source.FielderPositionId.ToString(),
                    BattingOrder = items.Source.BattingOrder.ToString(),
                    BowlingArm = items.Source.BowlingArm.ToString(),
                    LanguageId = items.Source.LanguageId.ToString(),
                });
            }
            return _objSearchResult;
        }
        //public dynamic SearchS1MasterData(QueryContainer _objNestedQuery, ElasticClient EsClient)
        //{
        //    dynamic result1 = null;
        //    IEnumerable<SearchS1CricketMasterData> _objSearchResultFilterData = new List<SearchS1CricketMasterData>();
        //    var result = EsClient.Search<SearchS1CricketMasterData>(s => s.Index("cricket").Query(q => _objNestedQuery).Size(802407));
        //    return result1 = result.Hits.ToList();
        //}

        public dynamic SearchS1MasterData(QueryContainer _objNestedQuery, ElasticClient EsClient)
        {
            dynamic result1 = null;
            IEnumerable<SearchS1CricketMasterData> _objSearchResultFilterData = new List<SearchS1CricketMasterData>();

            List<SearchS1CricketMasterData> _objSearchResult = new List<SearchS1CricketMasterData>();
            var result = EsClient.Search<SearchS1CricketMasterData>(s => s.Index("cricket").Query(q => _objNestedQuery).Size(802407));
            var response = result.Hits.ToList();
            foreach (var items in result.Hits)
            {
                _objSearchResult.Add(new SearchS1CricketMasterData
                {
                    BattingOrder = items.Source.BattingOrder.ToString(),
                    BowlingArm = items.Source.BowlingArm.ToString(),
                    MatchDate = Convert.ToInt32(items.Source.MatchDate.ToString()),
                    FielderPosition = items.Source.FielderPosition.ToString(),
                    FielderPositionId = items.Source.FielderPositionId.ToString(),
                    BowlingLine = items.Source.BowlingLine.ToString(),
                    BowlingLineId = items.Source.BowlingLineId.ToString(),
                    BowlingLengthId = items.Source.BowlingLengthId.ToString(),
                    DeliveryType = items.Source.DeliveryType.ToString(),
                    DeliveryTypeId = items.Source.DeliveryTypeId.ToString(),
                    Dismissal = items.Source.Dismissal.ToString(),
                    DismissalId = items.Source.DismissalId.ToString(),
                    ShotZone = items.Source.ShotZone.ToString(),
                    ShotType = items.Source.ShotType.ToString(),
                    ShotZoneId = items.Source.ShotZoneId.ToString(),
                    ShotTypeId = items.Source.ShotTypeId.ToString(),
                });
            }
            return _objSearchResult;
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

        public QueryContainer GetS2SearchResults(SearchS2RequestData _objReqData, QueryContainer _objNestedQuery) {
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
                        _objNestedQuery = GetMatchDetailQuery(_objNestedQuery,_objMatchDetail);
                    }

                    if (_objActionData != null)
                    {
                        _objNestedQuery = GetS2ActionQueryResult(_objActionData,_objNestedQuery);
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
                            QueryContainer qMust = new TermRangeQuery{ Field ="matchDate", GreaterThanOrEqualTo= start.ToString(), LessThanOrEqualTo = End.ToString() };
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
            catch (Exception ex) {
                result = ex.Message.ToString();
            }
            return _objNestedQuery;
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
                    QueryContainer query = new TermQuery { Field="isAsset", Value=input };
                    _objNestedquery &= query;
                    SearchS2ResultData = MapS2Resuldata(_objNestedquery, search, _oLayer.CreateConnection());
                }
            }
            catch (Exception ex) {

            }
            return SearchS2ResultData;
        }

        public dynamic MapS2Resuldata(QueryContainer _objNestedquery, string Search, ElasticClient EsClient) {
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
                    var result_E1 = EsClient.Search<S2FilteredEntity>(s => s.Index("crickets2data").Query(q => _objNestedquery)
                    .Aggregations(a => a.Terms("agg_E1", st => st.Script(p => p.Source("doc['"+ KeyValue_E1.Key + ".keyword'].value + '|' + doc['"+ KeyValue_E1.Value + ".keyword'].value + '|' + doc['entityId_3.keyword'].value"))
                    .Size(409846))));

                    var result_E2 = EsClient.Search<S2FilteredEntity>(s => s.Index("crickets2data").Query(q => _objNestedquery)
                    .Aggregations(a => a.Terms("agg_E2", st => st.Script(p => p.Source("doc['" + KeyValueS_E2.Key + ".keyword'].value + '|' + doc['" + KeyValueS_E2.Value + ".keyword'].value + '|' + doc['entityId_3.keyword'].value"))
                    .Size(409846))));

                    var result_E3 = EsClient.Search<S2FilteredEntity>(s => s.Index("crickets2data").Query(q => _objNestedquery)
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
                //else
                //{

                //    var result_E1 = EsClient.Search<SearchS2ResultData>(s => s.Index("crickets2data").Query(q => _objNestedquery)
                //   .Aggregations(a => a.Terms("agg_E1", st => st.Script(p => p.Source("doc['" + KeyValue_E1.Key + ".keyword'].value + '|' + doc['" + KeyValue_E1.Value + ".keyword'].value + '|' + doc['entityId_3.keyword'].value"))
                //   .Size(409846))));

                //    var result_E2 = EsClient.Search<SearchS2ResultData>(s => s.Index("crickets2data").Query(q => _objNestedquery)
                //    .Aggregations(a => a.Terms("agg_E2", st => st.Script(p => p.Source("doc['" + KeyValueS_E2.Key + ".keyword'].value + '|' + doc['" + KeyValueS_E2.Value + ".keyword'].value + '|' + doc['entityId_3.keyword'].value"))
                //    .Size(409846))));

                //    var result_E3 = EsClient.Search<SearchS2ResultData>(s => s.Index("crickets2data").Query(q => _objNestedquery)
                //    .Aggregations(a => a.Terms("agg_E3", st => st.Script(p => p.Source("doc['" + KeyValueS_E3.Key + ".keyword'].value + '|' + doc['" + KeyValueS_E3.Value + ".keyword'].value + '|' + doc['entityId_3.keyword'].value"))
                //    .Size(409846))));

                //    var agg1 = result_E1.Aggregations.Terms("agg_E1").Buckets;
                //    var agg2 = result_E2.Aggregations.Terms("agg_E2").Buckets;
                //    var agg3 = result_E3.Aggregations.Terms("agg_E3").Buckets;


                  
                //  S2Result = _objFilterentityt;
                //}
            }
            catch (Exception ex) {

            }
            return S2Result;
        }

        public dynamic MapS2Resuldata(QueryContainer _objNested, string Search)
        {
            throw new NotImplementedException();
        }
    }
}
