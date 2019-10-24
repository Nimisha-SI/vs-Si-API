using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using WebApis.BOL;
using static WebApis.Model.ELModels;

namespace WebApis.Model
{
    public class GetSearchS1DataForKabaddi
    {
        string con;

        public string GetConnectionString(string conString, string subCon)
        {
            var configuration = GetConfiguration();
            // con= configuration.GetSection("ConnectionStrings").GetSection("DefaultConnection").Value;
            con = configuration.GetSection(conString).GetSection(subCon).Value;
            return con;
        }

        public IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

        public List<SearchKabaddiData> getKabaddiData(string DBConnectionString, bool isFullDownload = true, int FullBatchCounter = 1, int FullRowCount = 0)
        {
            // public static string DBConnectionString { get { return ConfigurationManager.ConnectionStrings[GlobalInfo.DBSchemeName].ConnectionString; } }

            //string DBConnectionString;
            int FullDownloadFlag = isFullDownload ? 1 : 0;
            List<SearchKabaddiData> _objLstData = new List<SearchKabaddiData>();
            try
            {
                DataSet _objDsSearch = new DataSet();
                switch (isFullDownload)
                {
                    case true:
                        _objDsSearch = DBTask.ExecuteDataset(DBConnectionString, "dh_s1_data_search_full", 1, FullDownloadFlag, FullBatchCounter, FullRowCount);
                        break;
                    case false:
                        _objDsSearch = DBTask.ExecuteDataset(DBConnectionString, "dh_s1_data_search", 1, FullDownloadFlag);
                        break;
                }


                if (_objDsSearch.Tables != null && _objDsSearch.Tables.Count > 0)
                {
                    SearchKabaddiData _objSearchData;
                    foreach (DataRow rowitem in _objDsSearch.Tables[0].Rows)
                    {
                        _objSearchData = new SearchKabaddiData();
                        _objSearchData.Id = Convert.ToString(rowitem["s_id"]);
                        _objSearchData.RId = Convert.ToString(rowitem["Row_Id"]);
                        _objSearchData.MatchId = Convert.ToString(rowitem["match_id"]);
                        _objSearchData.MarkIn = Convert.ToString(rowitem["markin_timecode"]);
                        _objSearchData.MarkOut = Convert.ToString(rowitem["markout_timecode"]);
                        _objSearchData.ShortMarkIn = Convert.ToString(rowitem["markin_short"]);
                        _objSearchData.ShortMarkOut = Convert.ToString(rowitem["markout_short"]);
                        _objSearchData.ClearId = Convert.ToString(rowitem["clear_id"]).Trim();
                        _objSearchData.QClearId = Convert.ToString(rowitem["clear_id"]).Replace("-", string.Empty);
                        _objSearchData.MediaId = Convert.ToString(rowitem["media_id"]);
                        _objSearchData.CompType = Convert.ToString(rowitem["comp_type"]);
                        _objSearchData.VenueId = Convert.ToString(rowitem["venue_id"]);
                        _objSearchData.Venue = Convert.ToString(rowitem["venue_name"]);
                        _objSearchData.SeriesId = Convert.ToString(rowitem["series_id"]);
                        _objSearchData.Series = Convert.ToString(rowitem["series_name"]);
                        //_objSearchData.MatchDate = Convert.ToString(rowitem["matchdate"]) != string.Empty ? Convert.ToDateTime(rowitem["matchdate"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty;
                        _objSearchData.MatchDate = Convert.ToString(rowitem["event_date"]) != "" ? int.Parse(Convert.ToDateTime(rowitem["event_date"]).ToString("yyyyMMdd", CultureInfo.InvariantCulture)) : 0;
                        _objSearchData.Team1Id = Convert.ToString(rowitem["team1_id"]);
                        _objSearchData.Team1 = Convert.ToString(rowitem["team1"]);
                        _objSearchData.Team2Id = Convert.ToString(rowitem["team2_id"]);
                        _objSearchData.Team2 = Convert.ToString(rowitem["team2"]);
                        _objSearchData.SType = Convert.ToString(rowitem["s_type"]);
                        _objSearchData.Title = Convert.ToString(rowitem["title"]);
                        _objSearchData.Description = Convert.ToString(rowitem["description"]);
                        _objSearchData.Duration = Convert.ToString(rowitem["duration"]);

                        
                        _objSearchData.ParentSeriesId = Convert.ToString(rowitem["parent_series_id"]);

                        _objSearchData.HasShortClip = Convert.ToString(rowitem["has_short_clip"]);
                        _objSearchData.EventId = Convert.ToString(rowitem["event_id"]);
                        _objSearchData.EventText = Convert.ToString(rowitem["event_text"]);

                        _objSearchData.IsTagged = Convert.ToString(rowitem["is_tagged"]);
                        _objSearchData.Match = Convert.ToString(rowitem["game_event_name"]);
                        _objSearchData.ParentSeriesName = Convert.ToString(rowitem["parent_series_name"]);
                        _objSearchData.AssetTypeId = Convert.ToString(rowitem["asset_type_id"]);
                        _objSearchData.ClearId2 = Convert.ToString(rowitem["clear_id_2"]);
                        _objSearchData.QClearId2 = Convert.ToString(rowitem["clear_id_2"]).Replace("-", string.Empty);
                        _objSearchData.ClearId3 = Convert.ToString(rowitem["clear_id_3"]);
                        _objSearchData.QClearId3 = Convert.ToString(rowitem["clear_id_3"]).Replace("-", string.Empty);
                        _objSearchData.ClearId4 = Convert.ToString(rowitem["clear_id_4"]);
                        _objSearchData.QClearId4 = Convert.ToString(rowitem["clear_id_4"]).Replace("-", string.Empty);
                        _objSearchData.ClearId5 = Convert.ToString(rowitem["clear_id_5"]);
                        _objSearchData.QClearId5 = Convert.ToString(rowitem["clear_id_5"]).Replace("-", string.Empty);
                        _objSearchData.ClearId6 = Convert.ToString(rowitem["clear_id_6"]);
                        _objSearchData.QClearId6 = Convert.ToString(rowitem["clear_id_6"]).Replace("-", string.Empty);

                        _objSearchData.MediaId2 = Convert.ToString(rowitem["media_id_2"]);
                        _objSearchData.MediaId3 = Convert.ToString(rowitem["media_id_3"]);
                        _objSearchData.MediaId4 = Convert.ToString(rowitem["media_id_4"]);
                        _objSearchData.MediaId5 = Convert.ToString(rowitem["media_id_5"]);
                        _objSearchData.MediaId6 = Convert.ToString(rowitem["media_id_6"]);
                        _objSearchData.LanguageId = Convert.ToString(rowitem["language_id"]);
                        _objSearchData.Language = Convert.ToString(rowitem["language"]);

                       
                        _objLstData.Add(_objSearchData);
                        _objSearchData = null;
                    }
                }
            }

            catch (Exception ex)
            {
                //ErrorLog.LogServiceError("SearchDAO.cs", "GetSearchS1DataForCricket", ex.ToString());
            }
            return _objLstData;
        }

        public List<SearchS2Data> GetAllSearchS2Data(string DBConnectionString, int SportId, bool isFullDownload = true)
        {
            int FullDownloadFlag = isFullDownload ? 1 : 0;
            List<SearchS2Data> _objLstData = new List<SearchS2Data>();
            try
            {
                DataSet _objDsSearch = new DataSet();

                _objDsSearch = DBTask.ExecuteDataset(DBConnectionString, "dh_s2_data_search_enhanced", SportId, FullDownloadFlag);
                //  _objDsSearch = DBTask.ExecuteDataset(DBConnectionString, "dh_s2_data_search", SportId, FullDownloadFlag);
                if (_objDsSearch.Tables != null && _objDsSearch.Tables.Count > 0)
                {
                    foreach (DataRow rowitem in _objDsSearch.Tables[0].Rows)
                    {
                        SearchS2Data _objSearchData = new SearchS2Data();
                        _objSearchData.Id = Convert.ToString(rowitem["s_id"]);
                        _objSearchData.RId = Convert.ToInt64(rowitem["s_id"]);
                        _objSearchData.MatchId = Convert.ToString(rowitem["match_id"]);
                        //_objSearchData.Match = Convert.ToString(rowitem["match_name"]); 
                        _objSearchData.MarkIn = Convert.ToString(rowitem["mark_in"]);
                        _objSearchData.MarkOut = Convert.ToString(rowitem["mark_out"]);
                        _objSearchData.ClearId = Convert.ToString(rowitem["clear_id"]).Trim();
                        _objSearchData.QClearId = Convert.ToString(rowitem["clear_id"]).Replace("-", string.Empty);
                        _objSearchData.MediaId = Convert.ToString(rowitem["media_id"]);
                        _objSearchData.CompType = Convert.ToString(rowitem["comp_type"]);
                        _objSearchData.VenueId = Convert.ToString(rowitem["venue_id"]);
                        _objSearchData.Venue = Convert.ToString(rowitem["venue_name"]);
                        _objSearchData.SeriesId = Convert.ToString(rowitem["series_id"]);
                        _objSearchData.Series = Convert.ToString(rowitem["series_name"]);
                        //_objSearchData.MatchDate = Convert.ToString(rowitem["matchdate"]) != string.Empty ? Convert.ToDateTime(rowitem["matchdate"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : string.Empty;
                        _objSearchData.MatchDate = Convert.ToString(rowitem["event_date"]) != "" ? int.Parse(Convert.ToDateTime(rowitem["event_date"]).ToString("yyyyMMdd", CultureInfo.InvariantCulture)) : 0;
                        _objSearchData.MatchStageId = Convert.ToString(rowitem["match_stage_id"]);
                        _objSearchData.MatchStage = Convert.ToString(rowitem["match_stage"]);
                        _objSearchData.Team1Id = Convert.ToString(rowitem["team1_id"]);
                        _objSearchData.Team1 = Convert.ToString(rowitem["team1"]);
                        _objSearchData.Team2Id = Convert.ToString(rowitem["team2_id"]);
                        _objSearchData.Team2 = Convert.ToString(rowitem["team2"]);
                        _objSearchData.Attribute_Id_Level1 = Convert.ToString(rowitem["attribute_id_level_1"]);
                        _objSearchData.Attribute_Name_Level1 = Convert.ToString(rowitem["attribute_name_level_1"]);
                        _objSearchData.Attribute_Id_Level2 = Convert.ToString(rowitem["attribute_id_level_2"]);
                        _objSearchData.Attribute_Name_Level2 = Convert.ToString(rowitem["attribute_name_level_2"]);
                        _objSearchData.Attribute_Id_Level3 = Convert.ToString(rowitem["attribute_id_level_3"]);
                        _objSearchData.Attribute_Name_Level3 = Convert.ToString(rowitem["attribute_name_level_3"]);
                        _objSearchData.Attribute_Id_Level4 = Convert.ToString(rowitem["attribute_id_level_4"]);
                        _objSearchData.Attribute_Name_Level4 = Convert.ToString(rowitem["attribute_name_level_4"]);
                        _objSearchData.EmotionId = Convert.ToString(rowitem["emotion_id"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["emotion_id"])) : "0";
                        _objSearchData.EmotionName = Convert.ToString(rowitem["emotion_name"]);
                        //_objSearchData.EntityName = Convert.ToString(rowitem["entity_name"]);
                        _objSearchData.EntityName_1 = Convert.ToString(rowitem["entity_1"]);
                        _objSearchData.EntityName_2 = Convert.ToString(rowitem["entity_2"]);
                        _objSearchData.EntityName_3 = Convert.ToString(rowitem["entity_3"]);
                        _objSearchData.EntityName_4 = Convert.ToString(rowitem["entity_4"]);
                        _objSearchData.EntityName_5 = Convert.ToString(rowitem["entity_5"]);
                        _objSearchData.EntityId_1 = Convert.ToString(rowitem["entity_1_id"]);
                        _objSearchData.EntityId_2 = Convert.ToString(rowitem["entity_2_id"]);
                        _objSearchData.EntityId_3 = Convert.ToString(rowitem["entity_3_id"]);
                        _objSearchData.EntityId_4 = Convert.ToString(rowitem["entity_4_id"]);
                        _objSearchData.EntityId_5 = Convert.ToString(rowitem["entity_5_id"]);
                        _objSearchData.ParentSeriesId = Convert.ToString(rowitem["parent_series_id"]);
                        _objSearchData.IsAsset = Convert.ToString(rowitem["is_asset"]);
                        _objSearchData.IsBigMoment = Convert.ToString(rowitem["is_big_moment"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_big_moment"])) : "0";
                        _objSearchData.IsFunnyMoment = Convert.ToString(rowitem["is_funny_moment"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_funny_moment"])) : "0";
                        _objSearchData.IsAudioPiece = Convert.ToString(rowitem["is_audio_piece"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_audio_piece"])) : " 0";
                        _objSearchData.Title = Convert.ToString(rowitem["title"]);
                        _objSearchData.Description = Convert.ToString(rowitem["description"]);
                        _objSearchData.Duration = Convert.ToString(rowitem["duration"]);
                        _objSearchData.ClearId2 = Convert.ToString(rowitem["clear_id_2"]);
                        _objSearchData.MediaId2 = Convert.ToString(rowitem["media_id_2"]);
                        _objSearchData.IsTagged = Convert.ToString(Convert.ToInt32(rowitem["is_tagged"]));
                        _objSearchData.LanguageId = Convert.ToString(rowitem["language_id"]);
                        _objSearchData.Language = Convert.ToString(rowitem["language"]); ;
                        _objSearchData.SType = Convert.ToString(rowitem["s_type"]);//new
                        _objSearchData.EventId = _objSearchData.IsBigMoment == "1" ? "5" : _objSearchData.IsFunnyMoment == "1" ? "6" : _objSearchData.IsAudioPiece == "1" ? "12" : "0";
                        _objSearchData.EventText = _objSearchData.IsBigMoment == "1" ? "BM" : _objSearchData.IsFunnyMoment == "1" ? "FM" : _objSearchData.IsAudioPiece == "1" ? "AP" : "";
                        _objSearchData.SportId = SportId;
                        _objLstData.Add(_objSearchData);
                    }
                }
            }

            catch (Exception ex)
            {
                //ErrorLog.LogServiceError("SearchDAO.cs", "GetAllSearchS2Data", ex.ToString());
            }
            return _objLstData;
        }

        public string GetSavedSearches(string DBConnectionString, SaveSearchesRequestData objSavedSearchData)
        {
            string response = string.Empty;
            try
            {
                DataSet _objDsSearch = new DataSet();
                _objDsSearch = DBTask.ExecuteDataset(DBConnectionString, "dh_cl_get_search", objSavedSearchData.SportId, objSavedSearchData.UserId == 0 ? null : objSavedSearchData.UserId, objSavedSearchData.SearchTypeId);
                if (_objDsSearch.Tables != null && _objDsSearch.Tables.Count > 0)
                {
                    response = JSONHelper.FromDataTable(_objDsSearch.Tables[0]);
                }
            }
            catch (Exception ex)
            {
                response = ex.ToString();
            }
            return response;
        }

        public List<KabaddiS1Data> GetSearchDataS1ForKabaddi(string DBConnectionString,bool isFullDownload = true)
        {
            int FullDownloadFlag = isFullDownload ? 1 : 0;

            List<KabaddiS1Data> _objLstData = new List<KabaddiS1Data>();
            try
            {
                DataSet _objDsSearch = new DataSet();
                _objDsSearch = DBTask.ExecuteDataset(DBConnectionString, "dh_s1_data_search", 3, FullDownloadFlag);
                if (_objDsSearch.Tables != null && _objDsSearch.Tables.Count > 0)
                {
                    foreach (DataRow rowitem in _objDsSearch.Tables[0].Rows)
                    {
                        KabaddiS1Data _objSearchData = new KabaddiS1Data();
                        _objSearchData.Id = Convert.ToString(rowitem["s_id"]);
                        _objSearchData.RId = Convert.ToString(rowitem["Row_Id"]);
                        _objSearchData.MatchId = Convert.ToString(rowitem["match_id"]);
                        _objSearchData.MarkIn = Convert.ToString(rowitem["markin_timecode"]);
                        _objSearchData.MarkOut = Convert.ToString(rowitem["markout_timecode"]);
                        _objSearchData.ShortMarkIn = Convert.ToString(rowitem["markin_short"]);
                        _objSearchData.ShortMarkOut = Convert.ToString(rowitem["markout_short"]);
                        _objSearchData.ClearId = Convert.ToString(rowitem["clear_id"]).Trim();
                        _objSearchData.QClearId = Convert.ToString(rowitem["clear_id"]).Replace("-", string.Empty);
                        _objSearchData.ClearId2 = Convert.ToString(rowitem["clear_id_2"]);
                        _objSearchData.QClearId2 = Convert.ToString(rowitem["clear_id_2"]).Replace("-", string.Empty);
                        _objSearchData.ClearId3 = Convert.ToString(rowitem["clear_id_3"]);
                        _objSearchData.QClearId3 = Convert.ToString(rowitem["clear_id_3"]).Replace("-", string.Empty);

                        _objSearchData.ClearId4 = Convert.ToString(rowitem["clear_id_4"]).Trim();
                        _objSearchData.QClearId4 = Convert.ToString(rowitem["clear_id_4"]).Replace("-", string.Empty);
                        _objSearchData.ClearId5 = Convert.ToString(rowitem["clear_id_5"]);
                        _objSearchData.QClearId5 = Convert.ToString(rowitem["clear_id_5"]).Replace("-", string.Empty);
                        _objSearchData.ClearId6 = Convert.ToString(rowitem["clear_id_6"]);
                        _objSearchData.QClearId6 = Convert.ToString(rowitem["clear_id_6"]).Replace("-", string.Empty);

                        _objSearchData.ClearId7 = Convert.ToString(rowitem["clear_id_7"]);
                        _objSearchData.QClearId7 = Convert.ToString(rowitem["clear_id_7"]).Replace("-", string.Empty);

                        _objSearchData.MediaId = Convert.ToString(rowitem["media_id"]);
                        _objSearchData.MediaId2 = Convert.ToString(rowitem["media_id_2"]);
                        _objSearchData.MediaId3 = Convert.ToString(rowitem["media_id_3"]);

                        _objSearchData.MediaId4 = Convert.ToString(rowitem["media_id_4"]);
                        _objSearchData.MediaId5 = Convert.ToString(rowitem["media_id_5"]);
                        _objSearchData.MediaId6 = Convert.ToString(rowitem["media_id_6"]);
                        _objSearchData.MediaId7 = Convert.ToString(rowitem["media_id_7"]);

                        _objSearchData.CompTypeId = Convert.ToString(rowitem["comp_type_id"]);
                        _objSearchData.CompType = Convert.ToString(rowitem["comp_type"]);
                        _objSearchData.VenueId = Convert.ToString(rowitem["venue_id"]);
                        _objSearchData.Venue = Convert.ToString(rowitem["venue_name"]);
                        _objSearchData.SeriesId = Convert.ToString(rowitem["series_id"]);
                        _objSearchData.Series = Convert.ToString(rowitem["series_name"]);
                        _objSearchData.MatchDate = Convert.ToString(rowitem["matchdate"]) != "" ? int.Parse(Convert.ToDateTime(rowitem["matchdate"]).ToString("yyyyMMdd", CultureInfo.InvariantCulture)) : 0;
                        _objSearchData.Team1Id = Convert.ToString(rowitem["team1_id"]);
                        _objSearchData.Team1 = Convert.ToString(rowitem["team1"]);
                        _objSearchData.Team2Id = Convert.ToString(rowitem["team2_id"]);
                        _objSearchData.Team2 = Convert.ToString(rowitem["team2"]);
                        _objSearchData.Match = Convert.ToString(rowitem["game_event_name"]);
                        _objSearchData.MatchStage = Convert.ToString(rowitem["match_stage"]);
                        _objSearchData.MatchStageId = Convert.ToString(rowitem["match_stage_id"]);
                        _objSearchData.EventId = Convert.ToString(rowitem["event_id"]);
                        _objSearchData.EventName = Convert.ToString(rowitem["event_name"]);
                        _objSearchData.EventText = Convert.ToString(rowitem["event_text"]);
                        _objSearchData.EventGroup = Convert.ToString(rowitem["event_group"]);
                        _objSearchData.AssetTypeId = Convert.ToString(rowitem["asset_type_id"]);
                        _objSearchData.SType = Convert.ToString(rowitem["s_type"]);
                        _objSearchData.Title = Convert.ToString(rowitem["title"]);
                        _objSearchData.Description = Convert.ToString(rowitem["description"]);
                        _objSearchData.Duration = Convert.ToString(rowitem["duration"]);
                        _objSearchData.ParentSeriesId = Convert.ToString(rowitem["parent_series_id"]);
                        _objSearchData.ParentSeriesName = Convert.ToString(rowitem["parent_series_name"]);
                        _objSearchData.IsAsset = Convert.ToString(rowitem["is_asset"]);
                        _objSearchData.HasShortClip = Convert.ToString(rowitem["has_short_clip"]);
                        _objSearchData.IsTagged = Convert.ToString(Convert.ToInt32(rowitem["is_tagged"]));

                        _objSearchData.OffensivePlayerId = Convert.ToString(rowitem["offensive_player_id"]);
                        _objSearchData.OffensivePlayerName = Convert.ToString(rowitem["offensive_player_name"]);
                        _objSearchData.OffensivePlayerTeam = Convert.ToString(rowitem["offensive_player_team"]);
                        _objSearchData.DefensivePlayerId = Convert.ToString(rowitem["defensive_player_id"]);
                        _objSearchData.DefensivePlayerName = Convert.ToString(rowitem["defensive_player_name"]);
                        _objSearchData.ReplacedPlayerId = Convert.ToString(rowitem["replaced_player_id"]);
                        _objSearchData.ReplacedPlayerName = Convert.ToString(rowitem["replaced_player_name"]);
                        _objSearchData.TouchTypeId = Convert.ToString(rowitem["touch_type_id"]);
                        _objSearchData.TouchType = Convert.ToString(rowitem["touch_type"]);
                        _objSearchData.TackleTypeId = Convert.ToString(rowitem["tackle_type_id"]);
                        _objSearchData.TackleType = Convert.ToString(rowitem["tackle_type"]);
                        _objSearchData.IsSuccessfulRaid = Convert.ToString(rowitem["is_successful_raid"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_successful_raid"])) : "0";
                        _objSearchData.IsEmptyRaid = Convert.ToString(rowitem["is_empty_raid"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_empty_raid"])) : "0";
                        _objSearchData.IsFailedRaid = Convert.ToString(rowitem["is_failed_raid"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_failed_raid"])) : "0";
                        _objSearchData.IsSuccessfulTackle = Convert.ToString(rowitem["is_successful_tackle"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_successful_tackle"])) : "0";
                        _objSearchData.IsFailedTackle = Convert.ToString(rowitem["is_failed_tackle"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_failed_tackle"])) : "0";
                        _objSearchData.IsSuperTackle = Convert.ToString(rowitem["is_super_tackle"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_super_tackle"])) : "0";
                        _objSearchData.IsAllOut = Convert.ToString(rowitem["is_all_out"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_all_out"])) : "0";
                        _objSearchData.IsDeclaration = Convert.ToString(rowitem["is_declaration"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_declaration"])) : "0";
                        _objSearchData.IsTimeOut = Convert.ToString(rowitem["is_time_out"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_time_out"])) : "0";
                        _objSearchData.IsSubstitution = Convert.ToString(rowitem["is_substitution"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_substitution"])) : "0";
                        _objSearchData.IsPursuit = Convert.ToString(rowitem["is_pursuit"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_pursuit"])) : "0";
                        _objSearchData.IsTechnicalPoint = Convert.ToString(rowitem["is_technical_point"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_technical_point"])) : "0";
                        _objSearchData.IsBonusPoint = Convert.ToString(rowitem["is_bonus_point"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_bonus_point"])) : "0";
                        _objSearchData.IsDoOrDieRaid = Convert.ToString(rowitem["is_do_or_die_raid"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_do_or_die_raid"])) : "0";
                        _objSearchData.IsMultiPointRaid = Convert.ToString(rowitem["is_double_point_raid"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_double_point_raid"])) : "0";
                        _objSearchData.IsSuperRaid = Convert.ToString(rowitem["is_super_raid"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_super_raid"])) : "0";
                        _objSearchData.IsTouchPoint = Convert.ToString(rowitem["is_touch_point"]) != string.Empty ? Convert.ToString(Convert.ToInt32(rowitem["is_touch_point"])) : "0";

                        _objSearchData.AssistPlayer1Id = Convert.ToString(rowitem["assist_player_id_1"]);
                        _objSearchData.AssistPlayer1 = Convert.ToString(rowitem["assist_player_name_1"]);
                        _objSearchData.AssistPlayer2Id = Convert.ToString(rowitem["assist_player_id_2"]);
                        _objSearchData.AssistPlayer2 = Convert.ToString(rowitem["assist_player_name_2"]);
                        _objSearchData.AssistType1Id = Convert.ToString(rowitem["assist_type_id_1"]);
                        _objSearchData.AssistType1 = Convert.ToString(rowitem["assist_type_1"]);
                        _objSearchData.AssistType2Id = Convert.ToString(rowitem["assist_type_id_2"]);
                        _objSearchData.AssistType2 = Convert.ToString(rowitem["assist_type_2"]);
                        _objSearchData.NoOfDefenders = Convert.ToString(rowitem["no_of_antis"]);
                        _objSearchData.LanguageId = Convert.ToString(rowitem["language_id"]);

                        _objLstData.Add(_objSearchData);
                    }
                }
            }

            catch (Exception ex)
            {
               // ErrorLog.LogServiceError("SearchDAO.cs", "GetSearchS1DataForKabaddi", ex.ToString());
            }
            return _objLstData;
        }

    }
}
