using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
//using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;
using WebApis.BOL;
using WebApis.DAL;
using WebApis.elastic;
using WebApis.Model;
using static WebApis.Model.ELModels;


namespace WebApis.Controllers
{
    public class KabaddiController : ControllerBase
    {
        private AppConfig _settings;
        static ElasticClient EsClient;
        private ICon _con;
        EsLayer oLayer;
        GetSearchS1DataForKabaddi obj = new GetSearchS1DataForKabaddi();

        public KabaddiController(ICon con)
        {
            _con = con;
            oLayer = new EsLayer(_con);
        }

        //[System.Web.Http.HttpGet]
        //[Route("api/getData")]
        //public IActionResult Get()
        //{
        //    EsClient = oLayer.CreateConnection();
        //    List<KabaddiS1Data> obj2 = new List<KabaddiS1Data>();
        //    string connection = obj.GetConnectionString("ConnectionStrings", "DefaultConnection");
        //    try
        //    {
        //        //dynamic DyObj = new List<SearchCricketData>();
        //        for (int i = 1; i <= 120; i++)
        //        {
        //            obj2 = obj.GetSearchDataS1ForKabaddi(connection, true);
        //            if (obj2.Count > 0)
        //            {
        //                oLayer.BulkInsert(EsClient, obj2);
        //            }

        //        }
        //        //  return Request.CreateResponse(HttpStatusCode.Created, "Index Created successfully.");
        //        return Ok("Success");
        //    }
        //    catch (Exception ex)
        //    {

        //        // return Request.CreateResponse(HttpStatusCode.BadRequest, ex.ToString());
        //        return BadRequest(ex.ToString());
        //        // return request.CreateResponse(HttpStatusCode.BadRequest, "Invalid operation!");
        //    }
        //}
    }
}