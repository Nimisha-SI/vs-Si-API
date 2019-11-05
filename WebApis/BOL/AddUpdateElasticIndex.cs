using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApis.elastic;
using WebApis.Model;

namespace WebApis.BOL
{
    public class AddUpdateElasticIndex : IAddUpdateIndex
    {
        CommonFunction _objCf =new  CommonFunction();
        private ESInterface _oLayer;
        ElasticClient EsClient_obj;
        public AddUpdateElasticIndex(ESInterface oLayer) {
            _oLayer = oLayer;
        }

        private void Add_UpdateCricketS1Data(dynamic searchData)
        {
            try
            {
                EsClient_obj = _oLayer.CreateConnection();
                QueryContainer qMust = new QueryContainer();
                var resultS1Cricketdata = new SearchCricketData();
                resultS1Cricketdata = searchData;
                QueryContainer query = new TermQuery { Field = "RId", Value = Convert.ToString(searchData.RId) };
                qMust &= query;
                var resultDelete = EsClient_obj.DeleteByQuery<SearchCricketData>(a => a.Index("cricket").Query(q => qMust));
                EsClient_obj.Index(resultS1Cricketdata, i => i
                       .Index("cricket")
                       );
            }
            catch (Exception ex) {

            }
            
        }
        private void Add_UpdateCricketS2Data(dynamic searchData)
        {
            try
            {
                EsClient_obj = _oLayer.CreateConnection();
                QueryContainer qMust = new QueryContainer();
                var resultS2data = new SearchS2Data();
                resultS2data = searchData;
                QueryContainer query = new TermQuery { Field = "Id", Value = Convert.ToString(searchData.Id) };
                qMust &= query;
                var resultDelete = EsClient_obj.DeleteByQuery<SearchS2Data>(a => a.Index("crickets2data").Query(q => qMust));
                EsClient_obj.Index(resultS2data, i => i
                       .Index("crickets2data")
                       );
            }
            catch (Exception ex) {

            }
            
        }
        private void Add_UpdateKabaddiS1Data(dynamic searchData)
        {
            try {
                EsClient_obj = _oLayer.CreateConnection();
                QueryContainer qMust = new QueryContainer();
                var resultS1kabaddidata = new KabaddiS1Data();
                resultS1kabaddidata = searchData;
                QueryContainer query = new TermQuery { Field = "RId", Value = Convert.ToString(searchData.RId) };
                qMust &= query;
                var resultDelete = EsClient_obj.DeleteByQuery<KabaddiS1Data>(a => a.Index("cricket").Query(q => qMust));
                EsClient_obj.Index(resultS1kabaddidata, i => i
                       .Index("kabaddi")
                       );
            } catch (Exception ex) {
          
            }
            
        }

        bool IAddUpdateIndex.AddUpdateElasticIndex(dynamic sampleDatas, int sportid, bool isfull, bool isS2 = false)
        {
            bool IsSuccess = false;
            try {
                switch (sportid)
                {
                    case 1:
                        if (isS2)
                        {
                            foreach (var sampleData in sampleDatas)
                            {
                                Add_UpdateCricketS2Data(sampleData);
                            }
                            
                        }
                        else {
                            foreach (var sampleData in sampleDatas)
                            {
                                Add_UpdateCricketS1Data(sampleData);
                            }
                        }
                        break;
                    case 3:
                        foreach (var sampleData in sampleDatas)
                        {
                            Add_UpdateKabaddiS1Data(sampleData);
                        }
                        break;
                    default:
                        break;
                }
                IsSuccess = true;
            }
            catch (Exception ex) {
                IsSuccess = false;
            }
            return IsSuccess;
        }

        
    }
}
