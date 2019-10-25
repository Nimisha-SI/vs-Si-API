using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using static WebApis.Model.ELModels;

namespace WebApis.Model
{
    public class SearchKabaddiData
    {
        public string Id { get; set; }
        public string RId { get; set; }
        public string MatchId { get; set; }
        public string MarkIn { get; set; }
        public string MarkOut { get; set; }
        public string ShortMarkIn { get; set; }
        public string ShortMarkOut { get; set; }
        public string ClearId { get; set; }
        public string QClearId { get; set; }
        public string ClearId2 { get; set; }
        public string QClearId2 { get; set; }
        public string ClearId3 { get; set; }
        public string QClearId3 { get; set; }

        public string ClearId4 { get; set; }
        public string QClearId4 { get; set; }
        public string ClearId5 { get; set; }
        public string QClearId5 { get; set; }
        public string ClearId6 { get; set; }
        public string QClearId6 { get; set; }

        public string ClearId7 { get; set; }
        public string QClearId7 { get; set; }

        public string MediaId { get; set; }
        public string MediaId2 { get; set; }
        public string MediaId3 { get; set; }

        public string MediaId4 { get; set; }
        public string MediaId5 { get; set; }
        public string MediaId6 { get; set; }
        public string MediaId7 { get; set; }

        public string CompTypeId { get; set; }
        public string CompType { get; set; }
        public string VenueId { get; set; }
        public string Venue { get; set; }
        public string ParentSeriesId { get; set; }
        public string ParentSeriesName { get; set; }
        public string SeriesId { get; set; }
        public string Series { get; set; }
        public int MatchDate { get; set; }
        public string Match { get; set; }
        public string Team1Id { get; set; }
        public string Team1 { get; set; }
        public string Team2Id { get; set; }
        public string Team2 { get; set; }
        public string MatchStage { get; set; }
        public string MatchStageId { get; set; }
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string EventText { get; set; }
        public string EventGroup { get; set; }
        public string OffensivePlayerId { get; set; }
        public string OffensivePlayerName { get; set; }
        public string DefensivePlayerId { get; set; }
        public string DefensivePlayerName { get; set; }
        public string ReplacedPlayerId { get; set; }
        public string ReplacedPlayerName { get; set; }
        public string OffensivePlayerTeam { get; set; }
        public string TouchTypeId { get; set; }
        public string TouchType { get; set; }
        public string TackleTypeId { get; set; }
        public string TackleType { get; set; }
        public string IsSuccessfulRaid { get; set; }
        public string IsEmptyRaid { get; set; }
        public string IsFailedRaid { get; set; }
        public string IsDoOrDieRaid { get; set; }
        public string IsSuccessfulTackle { get; set; }
        public string IsFailedTackle { get; set; }
        public string IsSuperTackle { get; set; }
        public string IsAllOut { get; set; }
        public string IsDeclaration { get; set; }
        public string IsTimeOut { get; set; }
        public string IsSubstitution { get; set; }
        public string IsPursuit { get; set; }
        public string IsTechnicalPoint { get; set; }
        public string IsBonusPoint { get; set; }
        public string IsTouchPoint { get; set; }
        public string IsMultiPointRaid { get; set; }
        public string IsSuperRaid { get; set; }
        public string AssistType1Id { get; set; }
        public string AssistType1 { get; set; }
        public string AssistType2Id { get; set; }
        public string AssistType2 { get; set; }
        public string AssistPlayer1Id { get; set; }
        public string AssistPlayer1 { get; set; }
        public string AssistPlayer2Id { get; set; }
        public string AssistPlayer2 { get; set; }
        public string SType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string IsAsset { get; set; }
        public string HasShortClip { get; set; }
        public string IsTagged { get; set; }
        public string AssetTypeId { get; set; }
        public string Language { get; set; }
        public string LanguageId { get; set; }
        public string NoOfDefenders { get; set; }

    }

    public class KabaddiResultData
    {
        public string Id { get; set; }
        public string ClearId { get; set; }
        public string MatchId { get; set; }
        public string MarkIn { get; set; }
        public string MarkOut { get; set; }
        public string ShortMarkIn { get; set; }
        public string ShortMarkOut { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string IsAsset { get; set; }
        public int MatchDate { get; set; }
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string EventText { get; set; }
        public string MediaId { get; set; }
        public string TouchTypeId { get; set; }
        public string TouchType { get; set; }
        public string TackleTypeId { get; set; }
        public string TackleType { get; set; }
        public string AssistType1Id { get; set; }
        public string AssistType1 { get; set; }
        public string AssistType2Id { get; set; }
        public string AssistType2 { get; set; }
        public string NoOfDefenders { get; set; }
        public string LanguageId { get; set; }
        public string SType { get; set; }

    }

    public class KabaddiEntityRequestData
    {
        public int SportId { get; set; }
        public int EntityId { get; set; }
        public int EntityTypeId { get; set; }
        public int EntityRoleId { get; set; }
        public string EntityText { get; set; }
        public MatchDetail MatchDetails { get; set; }
        public KabaddiPlayerDetail PlayerDetails { get; set; }
    }

    public class SearchResultFilterDataHIndi
    {
        public string Id { get; set; }
        public string ClearId2 { get; set; }
        public string MatchId { get; set; }
        public string MarkIn { get; set; }
        public string MarkOut { get; set; }
        public string ShortMarkIn { get; set; }
        public string ShortMarkOut { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string IsAsset { get; set; }
        public int MatchDate { get; set; }
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string NoOfDefenders { get; set; }

    }

    public class SearchResultFilterDataTamil
    {
        public string Id { get; set; }
        public string ClearId3 { get; set; }
        public string MatchId { get; set; }
        public string MarkIn { get; set; }
        public string MarkOut { get; set; }
        public string ShortMarkIn { get; set; }
        public string ShortMarkOut { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string IsAsset { get; set; }
        public int MatchDate { get; set; }
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string NoOfDefenders { get; set; }

    }

    public class KabaddiPlayerDetail
    {
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string EventText { get; set; }
        public string EventGroup { get; set; }
        public string OffensivePlayerId { get; set; }
        public string OffensivePlayerName { get; set; }
        public string DefensivePlayerId { get; set; }
        public string DefensivePlayerName { get; set; }
        public string ReplacedPlayerId { get; set; }
        public string ReplacedPlayerName { get; set; }
        public string OffensivePlayerTeam { get; set; }
        public string TouchTypeId { get; set; }
        public string TouchType { get; set; }
        public string TackleTypeId { get; set; }
        public string TackleType { get; set; }
        public bool IsSuccessfulRaid { get; set; }
        public bool IsEmptyRaid { get; set; }
        public bool IsFailedRaid { get; set; }
        public bool IsDoOrDieRaid { get; set; }
        public bool IsMultiPointRaid { get; set; }
        public bool IsSuccessfulTackle { get; set; }
        public bool IsFailedTackle { get; set; }
        public bool IsSuperTackle { get; set; }
        public bool IsAllOut { get; set; }
        public bool IsDeclaration { get; set; }
        public bool IsTimeOut { get; set; }
        public bool IsSubstitution { get; set; }
        public bool IsPursuit { get; set; }
        public bool IsTechnicalPoint { get; set; }
        public bool IsBonusPoint { get; set; }
        public bool IsSuperRaid { get; set; }
        public bool IsTouchPoint { get; set; }
        public bool IsDefault { get; set; }
        public bool IsMasterData { get; set; }
        public string AssistPlayerId { get; set; }
        public string AssistPlayer { get; set; }
        public string AssistPlayer2Id { get; set; }
        public string AssistPlayer2 { get; set; }
        public string AssistTypeId { get; set; }
        public string AssistType { get; set; }
        public string AssistType2Id { get; set; }
        public string AssistType2 { get; set; }
        public string NoOfDefenders { get; set; }

    }

    public class KabaddiS1Data
    {
        public string Id { get; set; }
        public string RId { get; set; }
        public string MatchId { get; set; }
        public string MarkIn { get; set; }
        public string MarkOut { get; set; }
        public string ShortMarkIn { get; set; }
        public string ShortMarkOut { get; set; }
        public string ClearId { get; set; }
        public string QClearId { get; set; }
        public string ClearId2 { get; set; }
        public string QClearId2 { get; set; }
        public string ClearId3 { get; set; }
        public string QClearId3 { get; set; }

        public string ClearId4 { get; set; }
        public string QClearId4 { get; set; }
        public string ClearId5 { get; set; }
        public string QClearId5 { get; set; }
        public string ClearId6 { get; set; }
        public string QClearId6 { get; set; }

        public string ClearId7 { get; set; }
        public string QClearId7 { get; set; }

        public string MediaId { get; set; }
        public string MediaId2 { get; set; }
        public string MediaId3 { get; set; }

        public string MediaId4 { get; set; }
        public string MediaId5 { get; set; }
        public string MediaId6 { get; set; }
        public string MediaId7 { get; set; }

        public string CompTypeId { get; set; }
        public string CompType { get; set; }
        public string VenueId { get; set; }
        public string Venue { get; set; }
        public string ParentSeriesId { get; set; }
        public string ParentSeriesName { get; set; }
        public string SeriesId { get; set; }
        public string Series { get; set; }
        public int MatchDate { get; set; }
        public string Match { get; set; }
        public string Team1Id { get; set; }
        public string Team1 { get; set; }
        public string Team2Id { get; set; }
        public string Team2 { get; set; }
        public string MatchStage { get; set; }
        public string MatchStageId { get; set; }
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string EventText { get; set; }
        public string EventGroup { get; set; }
        public string OffensivePlayerId { get; set; }
        public string OffensivePlayerName { get; set; }
        public string DefensivePlayerId { get; set; }
        public string DefensivePlayerName { get; set; }
        public string ReplacedPlayerId { get; set; }
        public string ReplacedPlayerName { get; set; }
        public string OffensivePlayerTeam { get; set; }
        public string TouchTypeId { get; set; }
        public string TouchType { get; set; }
        public string TackleTypeId { get; set; }
        public string TackleType { get; set; }
        public string IsSuccessfulRaid { get; set; }
        public string IsEmptyRaid { get; set; }
        public string IsFailedRaid { get; set; }
        public string IsDoOrDieRaid { get; set; }
        public string IsSuccessfulTackle { get; set; }
        public string IsFailedTackle { get; set; }
        public string IsSuperTackle { get; set; }
        public string IsAllOut { get; set; }
        public string IsDeclaration { get; set; }
        public string IsTimeOut { get; set; }
        public string IsSubstitution { get; set; }
        public string IsPursuit { get; set; }
        public string IsTechnicalPoint { get; set; }
        public string IsBonusPoint { get; set; }
        public string IsTouchPoint { get; set; }
        public string IsMultiPointRaid { get; set; }
        public string IsSuperRaid { get; set; }
        public string AssistType1Id { get; set; }
        public string AssistType1 { get; set; }
        public string AssistType2Id { get; set; }
        public string AssistType2 { get; set; }
        public string AssistPlayer1Id { get; set; }
        public string AssistPlayer1 { get; set; }
        public string AssistPlayer2Id { get; set; }
        public string AssistPlayer2 { get; set; }
        public string SType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string IsAsset { get; set; }
        public string HasShortClip { get; set; }
        public string IsTagged { get; set; }
        public string AssetTypeId { get; set; }
        public string Language { get; set; }
        public string LanguageId { get; set; }
        public string NoOfDefenders { get; set; }

    }

}
