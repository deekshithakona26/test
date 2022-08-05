using GETECINo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GETECINo.Repository.Interfaces
{
    public interface IDataRepository
    {
        ActionResult GetEcino(EcinoRequest ecinoRequest);
        Task<bool> IsAliveAsync();
    }
}
