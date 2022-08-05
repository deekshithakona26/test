using System;
using Microsoft.AspNetCore.Mvc;
using GetECINo.Filters;
using GETECINo.Models;
using GetECINo.Repository;
using System.Threading.Tasks;
using GETECINo.Helpers;
using Newtonsoft.Json;
using StackExchange.Profiling;
using Serilog;

namespace GetECINo.Controller
{
    //[OpenApi Tag("MongoDB", Description = "Get ECINO")]
    [Route("api/mongodb")]
    [ApiController]
    public class MongoDBController : ControllerBase
    {
        public readonly DataRepository _dataRepository;
        //public readonly object ModelState;

        public MongoDBController(DataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("getECINo")]
        [ValidateModel]



        public ActionResult Result([FromBody] EcinoRequest ecinoRequest)
        {
            return _dataRepository.GetEcino(ecinoRequest);
        }

        

    }
}