using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlashParcsLite.Data.Models;
using FlashParcsLite.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlashParcsLite.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LocationController : ControllerBase
    {       
        private readonly ILocationRepository _repo;
        private readonly ILogger<LocationController> _logger;

        public LocationController(ILogger<LocationController> logger, ILocationRepository repo)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Location> Get()
        {
            return _repo.GetLocations();
        }
    }
}
