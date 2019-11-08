using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nest;
using WebApis.DAL;
using WebApis.elastic;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.Controllers
{
    public class IndexDocumentController : Controller
    {
        ElasticClient EsClient_obj;
        GetSearchS1DataForCricket objCricket = new GetSearchS1DataForCricket();
        GetSearchS1DataForKabaddi objKabaddi = new GetSearchS1DataForKabaddi();
        private ESInterface _oLayer;
        private ICon _con;
        public IndexDocumentController(ICon con, ESInterface oLayer)
        {
            _con = con;
            _oLayer = oLayer;
        }
        [System.Web.Http.HttpPost]
        [Route("api/CricketS1DataIndex")]
        public IActionResult CricketS1DataIndex()
        {

            EsClient_obj = _oLayer.CreateConnection();
            List<SearchCricketData> obj2 = new List<SearchCricketData>();
            string connection = _con.GetKeyValueAppSetting("ConnectionStrings", "DefaultConnection");
            try
            {
                //dynamic DyObj = new List<SearchCricketData>();
                for (int i = 1; i <= 120; i++)
                {
                    obj2 = objCricket.getCricketData(connection, true, i, 10000);
                    if (obj2.Count > 0)
                    {
                        _oLayer.BulkInsert<SearchCricketData>(EsClient_obj, obj2, "cricketkeytags");
                    }

                }
                //  return Request.CreateResponse(HttpStatusCode.Created, "Index Created successfully.");
                return Ok("Success");
            }
            catch (Exception ex)
            {

                // return Request.CreateResponse(HttpStatusCode.BadRequest, ex.ToString());
                return BadRequest(ex.ToString());
                // return request.CreateResponse(HttpStatusCode.BadRequest, "Invalid operation!");
            }
        }

        [System.Web.Http.HttpPost]
        [Route("api/CricketS2DataIndex")]
        public IActionResult CricketS2DataIndex()
        {


            EsClient_obj = _oLayer.CreateConnection();
            List<SearchS2Data> obj2 = new List<SearchS2Data>();
            List<SearchS2Data> obj3 = new List<SearchS2Data>();
            string connection = _con.GetKeyValueAppSetting("ConnectionStrings", "DefaultConnection");
            Dictionary<string, object> column = new Dictionary<string, object>();
            obj2 = objCricket.GetAllSearchS2Data(connection, 1, true);
            obj3 = obj2.ToList();
            //column.Add("S2Data", obj2);
            try
            {
                //  dynamic DyObj = new List<SearchS2Data>();
                //obj3.RemoveRange(1, 10000);
                for (int i = 1; i <= 45; i++)
                {
                    obj3 = obj3.Take(20000).ToList();
                    if (obj3.Count > 0)
                    {
                        _oLayer.BulkInsert<SearchS2Data>(EsClient_obj, obj3, "crickets2data");
                    }
                    if (obj3.Count > 20000)
                    {
                        obj3.RemoveRange(1, 20000);
                    }
                }
                //    var listOfObj = obj3.Take(10000);
                //    column.Add("S2Data", listOfObj);
                //    column.Remove("S2Data");
                //    column.
                //    //listOfObj.r
                //    //var deleteList = listOfObj.RemoveRange(3, 2);



                //}
                //return Request.CreateResponse(HttpStatusCode.Created, "Index Created successfully.");
                return Ok(new { Result = obj2 });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message.ToString());
                //return BadRequest(ex.ToString());
                // return request.CreateResponse(HttpStatusCode.BadRequest, "Invalid operation!");
            }
            //}

        }


        [System.Web.Http.HttpGet]
        [Route("api/KabaddiS1DataIndex")]
        public IActionResult KabaddiS1DataIndex()
        {
            EsClient_obj = _oLayer.CreateConnection();
            List<KabaddiS1Data> obj2 = new List<KabaddiS1Data>();
            string connection = _con.GetKeyValueAppSetting("ConnectionStrings", "DefaultConnection");
            try
            {
                //dynamic DyObj = new List<SearchCricketData>();
                for (int i = 1; i <= 120; i++)
                {
                    obj2 = objKabaddi.GetSearchDataS1ForKabaddi(connection, true);
                    if (obj2.Count > 0)
                    {
                        _oLayer.BulkInsert(EsClient_obj, obj2,"kabaddi");
                    }

                }
                //  return Request.CreateResponse(HttpStatusCode.Created, "Index Created successfully.");
                return Ok("Success");
            }
            catch (Exception ex)
            {

                // return Request.CreateResponse(HttpStatusCode.BadRequest, ex.ToString());
                return BadRequest(ex.ToString());
                // return request.CreateResponse(HttpStatusCode.BadRequest, "Invalid operation!");
            }
        }

        [System.Web.Http.HttpPost]
        [Route("api/KabaddiS2DataIndex")]
        public IActionResult KabaddiS2DataIndex()
        {
            EsClient_obj = _oLayer.CreateConnection();
            List<SearchS2Data> obj2 = new List<SearchS2Data>();
            List<SearchS2Data> obj3 = new List<SearchS2Data>();
            string connection = _con.GetKeyValueAppSetting("ConnectionStrings", "DefaultConnection");
            Dictionary<string, object> column = new Dictionary<string, object>();
            obj2 = objCricket.GetAllSearchS2Data(connection, 3, true);
            obj3 = obj2.ToList();
            try
            {
                for (int i = 1; i <= 45; i++)
                {
                    obj3 = obj3.Take(20000).ToList();
                    if (obj3.Count > 0)
                    {
                        _oLayer.BulkInsert<SearchS2Data>(EsClient_obj, obj3, "kabaddis2data");
                    }
                    if (obj3.Count > 20000)
                    {
                        obj3.RemoveRange(1, 20000);
                    }
                }
                return Ok(new { Result = obj2 });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message.ToString());
            }
        }



        [System.Web.Http.HttpPost]
        [Route("api/KeyTagsforFTS")]
        public IActionResult GetAllKeyTagsforFTS()
        {
            EsClient_obj = _oLayer.CreateConnection();
            List<KTData> obj2 = new List<KTData>();
           
            string connection = _con.GetKeyValueAppSetting("ConnectionStrings", "DefaultConnection");
            Dictionary<string, object> column = new Dictionary<string, object>();
            obj2 = objCricket.GetAllKeyTagsforFTS(connection, 1, true);
           
            try
            {
                _oLayer.BulkInsert<KTData>(EsClient_obj, obj2, "keytags");
                return Ok(new { Result = obj2 });
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message.ToString());
            }
        }
    }
}