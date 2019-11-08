using Nest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WebApis.DAL;
using WebApis.elastic;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
    public class KeyTags : IKeyTags
    {
        CommonFunction objCF = new CommonFunction();
        private ESInterface _oLayer;
        private ICon _con;
        ICricketS2 _cricketS2;
        public KeyTags(ESInterface oLayer, ICon con, ICricketS2 cricketS2) {
            _oLayer = oLayer;
        }
        public  IEnumerable<FTSResultData> FetchSearchFTSResultData(List<SearchQueryModel> lstSearchFields, Dictionary<string, string> str, int sportid = 1, string stype = "s1")
        {
            List<FTSResultData> lstsearchresults = new List<FTSResultData>();
            QueryContainer qc = new QueryContainer();
            string Indexname = stype == "s2" ? "crickets2data" : "cricket";
            if (lstSearchFields.Count > 0)
            {
                try
                {
                    var data = lstSearchFields.FirstOrDefault();
                        
                        
                        foreach (SearchQueryModel sqitem in lstSearchFields)
                        {
                            //QueryParser qp = new QueryParser(Version.LUCENE_30, sqitem.FieldName, analyzer);
                            if (sqitem.FieldType == ElasticQueryType.FieldType_Text)
                            {
                            QueryContainer query = new WildcardQuery { Field = sqitem.FieldName, Value = sqitem.SearchText.Trim() + "*" };

                            if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                            {
                                qc &= query;
                            }
                            else if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                            {
                                qc |= query;
                            }
                        }
                            else if (sqitem.FieldType == ElasticQueryType.FieldType_Number)
                            {
                            QueryContainer query = new TermQuery { Field = sqitem.FieldName, Value = sqitem.SearchText };
                            if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                            {
                                qc &= query;
                            }
                            else if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                            {
                                qc &= query;
                            }
                            }
                            else if (sqitem.FieldType == ElasticQueryType.FieldType_TextMultiple)
                            {
                                if (sqitem.SearchText.Contains(","))
                                {
                                    string[] ArrSearchText = sqitem.SearchText.Split(',').ToArray();
                                if (ArrSearchText.Length > 0)
                                {
                                    List<string> _objLstItems = ArrSearchText.Distinct().ToList();
                                    QueryContainer _objNestedBoolQuery = new QueryContainer();

                                    foreach (string listitem in _objLstItems)
                                    {
                                        if (listitem != string.Empty)
                                        {
                                            QueryContainer query = new TermQuery { Field = sqitem.FieldName, Value = listitem };
                                            _objNestedBoolQuery |= query;
                                        }
                                    }
                                    if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                                    {
                                        qc &= _objNestedBoolQuery;
                                    }
                                    else if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                                    {
                                        qc |= _objNestedBoolQuery;
                                    }
                                }
                            }
                                else
                                {
                                QueryContainer query = new TermQuery { Field = sqitem.FieldName, Value = sqitem.SearchText };
                                qc &= query;
                            }
                            }
                            else if (sqitem.FieldType == ElasticQueryType.FieldType_Nested)
                            {
                                Dictionary<string, string> _objNestedFields = sqitem.NestedFields;
                            if (_objNestedFields.Count > 0)
                            {
                                QueryContainer _objNestedBoolQuery = new QueryContainer();
                                foreach (var item in _objNestedFields)
                                {
                                    QueryContainer query = new TermQuery { Field = item.Key, Value = item.Value };
                                    if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                                    {
                                        _objNestedBoolQuery |= query;
                                    }
                                    else if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                                    {
                                        _objNestedBoolQuery &= query;
                                    }
                                }
                                qc &= _objNestedBoolQuery;
                            }
                        }
                            else if (sqitem.FieldType == ElasticQueryType.FieldType_NestedTextMultiple)
                            {
                                Dictionary<string, string> _objNestedFields = sqitem.NestedFields;
                            if (_objNestedFields.Count > 0)
                            {
                                QueryContainer _objBoolQuery = new QueryContainer();
                                foreach (var item in _objNestedFields)
                                {
                                    if (item.Value.Contains(","))
                                    {
                                        string[] ArrSearchText = item.Value.Split(',').ToArray();
                                        if (ArrSearchText.Length > 0)
                                        {
                                            QueryContainer _objInnerNestedBoolQuery = new QueryContainer();
                                            for (int iSrchCtr = 0; iSrchCtr < ArrSearchText.Length; iSrchCtr++)
                                            {
                                                QueryContainer query = new TermQuery { Field = item.Key, Value = ArrSearchText[iSrchCtr] };
                                                _objInnerNestedBoolQuery &= query;
                                            }
                                            if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                                            {
                                                qc |= _objInnerNestedBoolQuery;
                                            }
                                            else if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                                            {
                                                qc &= _objInnerNestedBoolQuery;
                                            }
                                        }
                                    }
                                    else
                                    {

                                        QueryContainer query = new TermQuery { Field = item.Key, Value = item.Value };

                                        if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                                        {
                                            _objBoolQuery |= _objBoolQuery;
                                        }
                                        else if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                                        {
                                            _objBoolQuery &= _objBoolQuery;
                                        }
                                    }
                                }
                                qc &= _objBoolQuery;
                            }
                        }
                            else if (sqitem.FieldType == ElasticQueryType.FieldType_Range) //not work
                            {
                            string[] ArrFieldRange = sqitem.SearchText.Split('-').ToArray();
                            int LowRange = Convert.ToInt32(ArrFieldRange[0]);
                            int HighRange = Convert.ToInt32(ArrFieldRange[1]);
                            QueryContainer query = new NumericRangeQuery { Field = sqitem.FieldName, LessThan = LowRange, GreaterThan = HighRange };
                            qc &= query;
                        }
                            else if (sqitem.FieldType == ElasticQueryType.FieldType_DateRange)
                            {
                            string[] ArrFieldRange = sqitem.SearchText.Split('-').ToArray();
                            int LowRange = int.Parse(DateTime.ParseExact(ArrFieldRange[0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            int HighRange = int.Parse(DateTime.ParseExact(ArrFieldRange[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            QueryContainer query = new DateRangeQuery { Field = sqitem.FieldName, LessThan = Convert.ToString(LowRange), GreaterThan = Convert.ToString(HighRange) };
                            qc &= query;
                        }
                            else if (sqitem.FieldType == ElasticQueryType.FieldType_TextWithWildCard)
                            {
                            QueryContainer query = new WildcardQuery { Field = sqitem.FieldName, Value = sqitem.SearchText.Trim() };
                            qc &= query;
                        }
                            else if (sqitem.FieldType == ElasticQueryType.FieldType_NestedTextWithWildcard)
                            {
                                Dictionary<string, string> _objNestedFields = sqitem.NestedFields;
                            if (_objNestedFields.Count > 0)
                            {
                                QueryContainer _objNestedBoolQuery = new QueryContainer();
                                foreach (var item in _objNestedFields)
                                {
                                    QueryContainer query = new TermQuery { Field = item.Key, Value = item.Value.Trim() };
                                    if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                                    {
                                        _objNestedBoolQuery |= query;
                                    }
                                    else if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                                    {
                                        _objNestedBoolQuery &= query;
                                    }
                                }
                                qc &= _objNestedBoolQuery;
                            }
                        }
                            else if (sqitem.FieldType == ElasticQueryType.FieldType_Date)
                            {
                            int val = int.Parse(DateTime.ParseExact(sqitem.SearchText, "ddMMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            QueryContainer query = new TermQuery { Field = sqitem.FieldName, Value = val.ToString() };

                            if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                            {
                                qc &= query;
                            }
                            else if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                            {
                                qc |= query;
                            }
                        }
                            else if (sqitem.FieldType == ElasticQueryType.FieldType_DateString)
                            {
                            int val = int.Parse(DateTime.ParseExact(sqitem.SearchText, "ddMMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            QueryContainer query = new TermQuery { Field = sqitem.FieldName, Value = val.ToString() };

                            if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                            {
                                //bq.Add(query, Occur.MUST);
                            }
                            else if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                            {
                                //bq.Add(query, Occur.SHOULD);
                            }
                        }
                        }
                        if (str.Count > 0)
                        {
                        
                            foreach (KeyValuePair<string, string> pair in str)
                            {
                                string key = pair.Key;
                                string value = pair.Value;
                            QueryContainer query = new TermQuery { Field = key, Value = value };
                            qc &= query;
                            }

                        }
                      
                        if (sportid == 3)
                        {
                        //    lstsearchresults = FTSresultdataMap(topDocs, searcher, sportid).OrderBy(x => x.MarkIn).ToList(); //_mapLuceneToFTSResultDataList(hits, searcher, stype).OrderBy(x => x.MarkIn).ToList();
                        lstsearchresults = FTSresultdataMap(_oLayer.CreateConnection(), qc, Indexname).OrderBy(x => x.MarkIn).ToList();
                        ///Grouping done for kabaddi as multiple records can exist for the same s_id
                            lstsearchresults = lstsearchresults.GroupBy(x => x.Id).Select(z => z.First()).ToList();
                        }
                        else
                        {
                        lstsearchresults = FTSresultdataMap(_oLayer.CreateConnection(), qc, Indexname);
                       
                        }
                        if (lstsearchresults.Count > 0)
                    {
                        foreach (SearchQueryModel sqitem in lstSearchFields)
                        {
                            if (sqitem.FieldType == ElasticQueryType.FieldType_DateString)
                            {
                                int val = int.Parse(DateTime.ParseExact(sqitem.SearchText, "ddMMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                                lstsearchresults = lstsearchresults.Where(t => t.MatchDate == val).ToList();
                            }
                        }
                    }


                }
                catch (Exception ex)
                {

                }
            }
            return lstsearchresults;
        }
        public List<KTData> FreeTextMapping(QueryContainer _objQuery, ElasticClient EsClient, string IndexName)
        {
            int Count = Convert.ToInt32(objCF.getIndexCount(IndexName, EsClient, 1));
            List<KTData> KTData = new List<KTData>();
            var FreeTextResult = EsClient.Search<KTData>(s => s.Index(IndexName).Query(q => _objQuery).Size(Count)
           );
            foreach (var items in FreeTextResult.Hits)
            {
                KTData.Add(new KTData
                {
                    SportId = items.Source.SportId.ToString(),
                    Id = items.Source.Id,
                    KeyTags = items.Source.KeyTags.ToString(),
                    LookUpFields = items.Source.LookUpFields.ToString(),
                    DataType = items.Source.DataType.ToString(),
                    SearchPosition = items.Source.SearchPosition.ToString(),
                    IsPhrase = items.Source.IsPhrase,
                    GlobalId = items.Source.GlobalId.ToString(),
                    SkillId = items.Source.SkillId.ToString(),
                    //identifier = items.Source.identifier.ToString(),
                    SType = items.Source.SType.ToString(),

                });
            }
            return KTData;
        }
        //IEnumerable<SearchCricketResultDataFreeText>
        public QueryContainer SearchResultData(List<SearchQueryModel> lstSearchFields, int sportid = 1, string languageid = "1")
        {
            
            QueryContainer qc = new QueryContainer();
            if (lstSearchFields.Count > 0)
            {
                try
                {
                    var data = lstSearchFields.FirstOrDefault();
                    foreach (SearchQueryModel sqitem in lstSearchFields)
                    {
                        if (sqitem.FieldType == ElasticQueryType.FieldType_Text)
                        {
                            QueryContainer query = new WildcardQuery { Field = sqitem.FieldName, Value = sqitem.SearchText.Trim() + "*" };

                            if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                            {
                                qc &= query;
                            }
                            else if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                            {
                                qc |= query;
                            }
                        }
                        else if (sqitem.FieldType == ElasticQueryType.FieldType_Number)
                        {
                            QueryContainer query = new TermQuery { Field = sqitem.FieldName, Value = sqitem.SearchText };
                            if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                            {
                                qc &= query;
                            }
                            else if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                            {
                                qc &= query;
                            }
                        }
                        else if (sqitem.FieldType == ElasticQueryType.FieldType_TextMultiple)
                        {
                            if (sqitem.SearchText.Contains(","))
                            {
                                string[] ArrSearchText = sqitem.SearchText.Split(',').ToArray();
                                if (ArrSearchText.Length > 0)
                                {
                                    List<string> _objLstItems = ArrSearchText.Distinct().ToList();
                                    QueryContainer _objNestedBoolQuery = new QueryContainer();

                                    foreach (string listitem in _objLstItems)
                                    {
                                        if (listitem != string.Empty)
                                        {
                                            QueryContainer query = new TermQuery { Field = sqitem.FieldName, Value = listitem };
                                            _objNestedBoolQuery |= query;
                                        }
                                    }
                                    if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                                    {
                                        qc &= _objNestedBoolQuery;
                                    }
                                    else if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                                    {
                                        qc |= _objNestedBoolQuery;
                                    }
                                }
                            }
                            else
                            {
                                QueryContainer query = new TermQuery { Field = sqitem.FieldName, Value = sqitem.SearchText };
                                qc &= query;
                            }
                        }
                        else if (sqitem.FieldType == ElasticQueryType.FieldType_Nested)
                        {
                            Dictionary<string, string> _objNestedFields = sqitem.NestedFields;
                            if (_objNestedFields.Count > 0)
                            {
                                QueryContainer _objNestedBoolQuery = new QueryContainer();
                                foreach (var item in _objNestedFields)
                                {
                                    QueryContainer query = new TermQuery { Field = item.Key, Value = item.Value };
                                    if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                                    {
                                        _objNestedBoolQuery |= query;
                                    }
                                    else if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                                    {
                                        _objNestedBoolQuery &= query;
                                    }
                                }
                                qc &= _objNestedBoolQuery;
                            }
                        }
                        else if (sqitem.FieldType == ElasticQueryType.FieldType_NestedTextMultiple)
                        {
                            Dictionary<string, string> _objNestedFields = sqitem.NestedFields;
                            if (_objNestedFields.Count > 0)
                            {
                                QueryContainer _objBoolQuery = new QueryContainer();
                                foreach (var item in _objNestedFields)
                                {
                                    if (item.Value.Contains(","))
                                    {
                                        string[] ArrSearchText = item.Value.Split(',').ToArray();
                                        if (ArrSearchText.Length > 0)
                                        {
                                            QueryContainer _objInnerNestedBoolQuery = new QueryContainer();
                                            for (int iSrchCtr = 0; iSrchCtr < ArrSearchText.Length; iSrchCtr++)
                                            {
                                                QueryContainer query = new TermQuery { Field = item.Key, Value = ArrSearchText[iSrchCtr] };
                                                _objInnerNestedBoolQuery &= query;
                                            }
                                            if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                                            {
                                                qc |= _objInnerNestedBoolQuery;
                                            }
                                            else if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                                            {
                                                qc &= _objInnerNestedBoolQuery;
                                            }
                                        }
                                    }
                                    else
                                    {

                                        QueryContainer query = new TermQuery { Field = item.Key, Value = item.Value };

                                        if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                                        {
                                            _objBoolQuery |= _objBoolQuery;
                                        }
                                        else if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                                        {
                                            _objBoolQuery &= _objBoolQuery;
                                        }
                                    }
                                }
                                qc &= _objBoolQuery;
                            }
                        }
                        else if (sqitem.FieldType == ElasticQueryType.FieldType_Range)
                        {
                            string[] ArrFieldRange = sqitem.SearchText.Split('-').ToArray();
                            int LowRange = Convert.ToInt32(ArrFieldRange[0]);
                            int HighRange = Convert.ToInt32(ArrFieldRange[1]);
                            QueryContainer query = new NumericRangeQuery { Field = sqitem.FieldName, LessThan = LowRange, GreaterThan = HighRange };
                            qc &= query;
                        }
                        else if (sqitem.FieldType == ElasticQueryType.FieldType_DateRange)
                        {
                            string[] ArrFieldRange = sqitem.SearchText.Split('-').ToArray();
                            int LowRange = int.Parse(DateTime.ParseExact(ArrFieldRange[0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            int HighRange = int.Parse(DateTime.ParseExact(ArrFieldRange[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            QueryContainer query = new DateRangeQuery { Field = sqitem.FieldName, LessThan = Convert.ToString(LowRange), GreaterThan = Convert.ToString(HighRange) };
                            qc &= query;
                        }
                        else if (sqitem.FieldType == ElasticQueryType.FieldType_TextWithWildCard)
                        {
                            QueryContainer query = new WildcardQuery { Field = sqitem.FieldName, Value = sqitem.SearchText.Trim() };
                            qc &= query;
                        }
                        else if (sqitem.FieldType == ElasticQueryType.FieldType_NestedTextWithWildcard)
                        {
                            Dictionary<string, string> _objNestedFields = sqitem.NestedFields;
                            if (_objNestedFields.Count > 0)
                            {
                                QueryContainer _objNestedBoolQuery = new QueryContainer();
                                foreach (var item in _objNestedFields)
                                {
                                    QueryContainer query = new TermQuery { Field = item.Key, Value = item.Value.Trim() };
                                    if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                                    {
                                        _objNestedBoolQuery |= query;
                                    }
                                    else if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                                    {
                                        _objNestedBoolQuery &= query;
                                    }
                                }
                                qc &= _objNestedBoolQuery;
                            }
                        }
                        else if (sqitem.FieldType == ElasticQueryType.FieldType_Date)
                        {
                            //Query query = qp.Parse(DateTime.ParseExact(sqitem.SearchText, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            //int val = int.Parse(DateTime.ParseExact(sqitem.SearchText, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            //Query query = NumericRangeQuery.NewIntRange(sqitem.FieldName, val, val, true, true);
                            QueryContainer query = new TermQuery { Field = sqitem.FieldName, Value = Convert.ToString(sqitem.SearchText) };

                            if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                            {
                                qc &= query;
                            }
                            else if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                            {
                                qc |= query;
                            }
                        }
                        else if (sqitem.FieldType == ElasticQueryType.FieldType_DateString)
                        {
                            int val = int.Parse(DateTime.ParseExact(sqitem.SearchText, "ddMMMyyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                            QueryContainer query = new TermQuery { Field = sqitem.FieldName, Value = val.ToString() };

                            if (sqitem.Operator == ElasticQueryType.Field_Operator_AND)
                            {
                                //bq.Add(query, Occur.MUST);
                            }
                            else if (sqitem.Operator == ElasticQueryType.Field_Operator_OR)
                            {
                                //bq.Add(query, Occur.SHOULD);
                            }
                        }
                    }
                   
                }
                catch (Exception)
                {

                }
            }
         
            return qc;
        }
        public IEnumerable<SearchCricketResultDataFreeText> FetchSearchResultData(QueryContainer qc, ElasticClient EsClient) {
            List<SearchCricketResultDataFreeText> _objResultData = new List<SearchCricketResultDataFreeText>();
             _objResultData = FetchSearchResultDataKeyTags(_oLayer.CreateConnection(), qc);
            return _objResultData;
        }
        public List<SearchCricketResultDataFreeText> FetchSearchResultDataKeyTags(ElasticClient elasticClient, QueryContainer _objNestedQuery)
        {
            List<SearchCricketResultDataFreeText> obj = new List<SearchCricketResultDataFreeText>();
            int Count = Convert.ToInt32(objCF.getIndexCount("cricket", elasticClient, 1));

            var resultKeyTags = elasticClient.Search<SearchCricketResultDataFreeText>(s => s.Index("cricket").Query(q => _objNestedQuery));
            foreach (var items in resultKeyTags.Hits)
            {
                obj.Add(new SearchCricketResultDataFreeText
                {
                    Id = items.Source.Id.ToString(),
                    ClearId = items.Source.ClearId.ToString(),
                    MatchId = items.Source.MatchId.ToString(),
                    MatchDate = items.Source.MatchDate.ToString(),
                    MarkIn = items.Source.MarkIn.ToString(),
                    MarkOut = items.Source.MarkOut.ToString(),
                    Title = items.Source.Title.ToString(),
                    Description = items.Source.Description.ToString(),
                    Duration = items.Source.Duration.ToString(),
                    IsAsset = items.Source.IsAsset.ToString(),
                    Event = items.Source.Event.ToString(),
                    EventText = items.Source.EventText.ToString(),
                    SType = items.Source.SType.ToString(),
                    BatsmanId = items.Source.BatsmanId.ToString(),
                    BowlerId = items.Source.BowlerId.ToString(),
                    FielderId = items.Source.FielderId.ToString(),

                });
            }
            return obj;


        }
        public List<KTData> GetSelectedFTData(List<SearchQueryModel> objlstSearchQueryModel, int sportid = 1, int stype = 1, bool isAutoComplete = true)
        {
            string _response = string.Empty;
            List<FTData> _objFTData = new List<FTData>();
            List<KTData> _objktdata = new List<KTData>();
            try
            {
                QueryContainer qc = new QueryContainer();
                foreach (SearchQueryModel objSearchQueryModel in objlstSearchQueryModel)
                {

                    if (objSearchQueryModel.FieldType == ElasticQueryType.FieldType_Text)
                    {
                        QueryContainer query = new WildcardQuery { Field = objSearchQueryModel.FieldName, Value = objSearchQueryModel.SearchText.Trim() + "*" };
                        qc &= query;
                        //_objktdata = FreeTextMapping(qc, _oLayer.CreateConnection(), "cricketkeytags", objSearchQueryModel.SearchText.Trim());

                    }
                    else if (objSearchQueryModel.FieldType == ElasticQueryType.FieldType_Number)
                    {
                        QueryContainer query = new TermQuery { Field = objSearchQueryModel.FieldName, Value = objSearchQueryModel.SearchText.Trim() };

                        if (objSearchQueryModel.Operator == ElasticQueryType.Field_Operator_AND)
                        {
                            qc &= query;
                        }
                        //_objktdata = FreeTextMapping(qc, _oLayer.CreateConnection(), "cricketkeytags", objSearchQueryModel.SearchText.Trim());
                    }

                    else if (objSearchQueryModel.FieldType == ElasticQueryType.FieldType_TextMultiple)
                    {
                        if (objSearchQueryModel.SearchText.Contains(","))
                        {
                            string[] ArrSearchText = objSearchQueryModel.SearchText.Split(',').ToArray();
                            if (ArrSearchText.Length > 0)
                            {
                                List<string> _objLstItems = ArrSearchText.Distinct().ToList();
                                QueryContainer _objNestedBoolQuery = new QueryContainer();
                                foreach (string listitem in _objLstItems)
                                {
                                    if (listitem != string.Empty)
                                    {
                                        QueryContainer query = new TermQuery { Field = objSearchQueryModel.FieldName, Value = listitem };
                                        _objNestedBoolQuery |= query;
                                    }
                                }
                                if (objSearchQueryModel.Operator == ElasticQueryType.Field_Operator_AND)
                                {
                                    qc &= _objNestedBoolQuery;
                                }
                                else if (objSearchQueryModel.Operator == ElasticQueryType.Field_Operator_OR)
                                {
                                    qc |= _objNestedBoolQuery;
                                }
                            }
                        }
                        else
                        {

                            QueryContainer query = new TermQuery { Field = objSearchQueryModel.FieldName, Value = objSearchQueryModel.SearchText };
                            qc &= query;
                        }
                    }
                    else if (objSearchQueryModel.FieldType == ElasticQueryType.FieldType_Nested)
                    {
                        Dictionary<string, string> _objNestedFields = objSearchQueryModel.NestedFields;
                        if (_objNestedFields.Count > 0)
                        {
                            QueryContainer _objNestedBoolQuery = new QueryContainer();
                            foreach (var item in _objNestedFields)
                            {
                                QueryContainer query = new TermQuery { Field = objSearchQueryModel.FieldName, Value = item.Value };

                                if (objSearchQueryModel.Operator == ElasticQueryType.Field_Operator_OR)
                                {
                                    _objNestedBoolQuery |= query;
                                }
                                else if (objSearchQueryModel.Operator == ElasticQueryType.Field_Operator_AND)
                                {
                                    _objNestedBoolQuery &= query;
                                }
                            }
                            qc &= _objNestedBoolQuery;
                        }
                    }
                    else if (objSearchQueryModel.FieldType == ElasticQueryType.FieldType_TextWithWildCard)
                    {
                        //Term term = new Term(sqitem.FieldName, sqitem.SearchText);
                        QueryContainer query = new WildcardQuery { Field = objSearchQueryModel.FieldName, Value = objSearchQueryModel.SearchText.Trim() + "*" };
                        qc &= query;
                    }
                    else if (objSearchQueryModel.FieldType == ElasticQueryType.FieldType_NestedTextWithWildcard)
                    {
                        Dictionary<string, string> _objNestedFields = objSearchQueryModel.NestedFields;
                        if (_objNestedFields.Count > 0)
                        {
                            QueryContainer _objNestedBoolQuery = new QueryContainer();
                            foreach (var item in _objNestedFields)
                            {
                                QueryContainer _objNestedquery = new TermQuery { Field = item.Key, Value = item.Value.Trim() };

                                if (objSearchQueryModel.Operator == ElasticQueryType.Field_Operator_OR)
                                {

                                    _objNestedBoolQuery |= _objNestedquery;
                                }
                                else if (objSearchQueryModel.Operator == ElasticQueryType.Field_Operator_AND)
                                {
                                    _objNestedBoolQuery &= _objNestedquery;
                                }
                            }
                            qc &= _objNestedBoolQuery;
                        }
                    }
                }

                _objktdata = FreeTextMapping(qc, _oLayer.CreateConnection(), "keytags");
            }
            catch (Exception ex)
            {

            }
            return _objktdata;
        }

        public List<FTSResultData> FTSresultdataMap(ElasticClient elasticClient, QueryContainer _objNestedQuery, string IndexName)
        {
            List<FTSResultData> obj = new List<FTSResultData>();
            try
            {
               
                int Count = Convert.ToInt32(objCF.getIndexCount(IndexName, elasticClient, 1));
                QueryContainer qc = new TermQuery { Field = "venueId", Value = "329" };
                QueryContainer qc1 = new TermQuery { Field = "isAsset", Value = "1" };
                var resultKeyTags = elasticClient.Search<FTSResultData>(s => s.Index(IndexName).Query(q => _objNestedQuery));
                foreach (var items in resultKeyTags.Hits)
                {
                    obj.Add(new FTSResultData
                    {
                        //Id = items.Source.Id.ToString(),
                        ClearId = items.Source.ClearId.ToString(),
                        //MatchId = items.Source.MatchId.ToString(),
                        //MatchDate = items.Source.MatchDate.ToString(),
                        MarkIn = items.Source.MarkIn.ToString(),
                        MarkOut = items.Source.MarkOut.ToString(),
                        ShortMarkIn = items.Source.ShortMarkIn.ToString(),
                        ShortMarkOut = items.Source.ShortMarkOut.ToString(),
                        Title = items.Source.Title.ToString(),
                        Description = items.Source.Description.ToString(),
                        Duration = items.Source.Duration.ToString(),
                        //IsAsset = items.Source.IsAsset.ToString(),
                        //Event = items.Source.Event.ToString(),
                        EventText = items.Source.EventText.ToString(),
                        SType = items.Source.SType.ToString(),
                    });
                }
            }
            catch (Exception ex) {

            }
            
            return obj;


        }

        
    }
}
