using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using GETECINo.Models;
using GetECINo.Middleware;
using GETECINo.Helpers;
using GetECINo.Repository.Interfaces;
using MongoDB.Driver;
using GetECINo.Models;
using Serilog;
using StackExchange.Profiling;
using System.Threading.Tasks;
using GETECINo;
using GETECINo.Repository.Interfaces;

namespace GetECINo.Repository
{
    /// <summary>
    /// DataRepository class is implemented from the IDatabaseRepository interface.
    /// In the DataRepository class, the connection to the database is established
    /// and the data is retrieved by performing the required operations. In the
    /// Getcino method, the transformations of the incoming request is done and pass
    /// the request data to the Get method. In the Get method, we use the Find method
    /// which is resided in IMongoCollection class which takes the request parameters
    /// and forms the query.
    /// </summary>
    public class DataRepository : IDataRepository
    {
        private readonly IMongoCollection<MT4073_Proj> _data;
        private readonly IOptions<DatabaseSettings> _settings;




        public DataRepository(IOptions<DatabaseSettings> settings)
        {
            _settings = settings;
            var connection = new MongoClient(_settings.Value.ConnectionString);
            var database = connection.GetDatabase(_settings.Value.DatabaseName);
            _data = database.GetCollection<MT4073_Proj>(_settings.Value.CollectionName);
        }





        public ActionResult GetEcino(EcinoRequest ecinoRequest)
        {
            var payload = JsonConvert.SerializeObject(ecinoRequest);
            var transId = Guid.NewGuid().ToString();
            if (AuditMiddleware.Logger != null)
            {
                AuditLogger.RequestInfo(
                transId, Constants.Post, Constants.Path, string.Empty, ecinoRequest.ToString());
            }
            if (AuditMiddleware.Logger != null)
            {
                AuditLogger.ResponseInfo(transId, Constants.Post, Constants.Path, string.Empty, _settings.Value.DatabaseName, _settings.Value.CollectionName, payload);
            }
            Log.Information("Request: {0}", JsonConvert.SerializeObject(ecinoRequest));
            DateTime Date = Convert.ToDateTime(ecinoRequest.Dob);
            int age = DateTime.Now.Year - Date.Year;
            //string gender = ecinoRequest.Gender.Equals("male") ? "M" : ecinoRequest.Gender.Equals("female") ? "F" : "";
            string gender = ecinoRequest.Gender;
            return Get(ecinoRequest.Name, age, gender);
        }





        public ActionResult Get(string name, int age, string gender)
        {
            Log.Information("Database Connected Successfully");
            List<MT4073_Proj> records = new List<MT4073_Proj>();
            List<ResponseDTO> list = new List<ResponseDTO>();
            using (MiniProfiler.Current.Step("Time taken to retrieve data from database:"))
            {
                if (gender.Length > 0)
                {
                    records = _data.Find(book => book.name == name && book.age == age && book.gender == gender).ToList();
                }
                else
                {
                    records = _data.Find(book => book.name == name && book.age == age).ToList();
                }



                foreach (var record in records)
                {
                    ResponseDTO res = new ResponseDTO();
                    res.Ecino = record.ecino;
                    list.Add(res);
                }
            }
            if (list.Count == 0)
            {
                Log.Information("No matches found");
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.ErrorMessage = "No matches found";
                errorResponse.StatusCode = 404;
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(errorResponse),
                    ContentType = Constants.JSON_CONTENT,
                    StatusCode = 404
                };
            }
            else if (list.Count >= 2)
            {
                Log.Information("More than one match found");
                ErrorResponse errorResponse = new ErrorResponse();
                errorResponse.ErrorMessage = "More than one match found";
                errorResponse.StatusCode = 400;
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(errorResponse),
                    ContentType = Constants.JSON_CONTENT,
                    StatusCode = 400
                };
            }
            else
            {
                Log.Information("The ECINo is {0}", list[0].Ecino);
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(list),
                    ContentType = Constants.JSON_CONTENT,
                    StatusCode = 200
                };
            }
        }
        ///Return health status of API
        public async Task<bool> IsAliveAsync()
        {
            try
            {
                using (MiniProfiler.Current.Step(Constants.HealthCommand))
                {
                    await Task.Delay(1);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
            return false;
        }
    }
}