using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceRequests.Models;
using ServiceRequests.Service;

namespace ServiceRequests.Controllers
{
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly DatabaseContext _context;

        //Injected logger to use swagger and context to use db
        public ServicesController(DatabaseContext context, ILogger<ServicesController> logger)
        {
            _logger = logger;
            _context = context;
        }

        private readonly ILogger<ServicesController> _logger;

        // GET: ServiceModels
        [HttpGet]
        [Route("api/[controller]/[action]")]
        public ActionResult<ServiceModel> Index()
        {
            var serviceModels = _context.Services.ToArrayAsync();
            if (serviceModels == null)
                return StatusCode(204, "Empty content");
            return Ok(serviceModels);
        }

        // GET: ServiceModels/Details/5
        [HttpGet]
        [Route("api/[controller]/[action]")]
        public async Task<ActionResult> Details(Guid? id)
        {
            var serviceModel = await _context.Services.Where(m => m.Id == id).ToArrayAsync();
            if (serviceModel == null)
                return NotFound();
            return Ok(serviceModel);
        }


        // POST: ServiceModels/Create
        [HttpPost]
        [Route("api/[controller]/[action]")]
        public async Task<ActionResult> Create([Bind("Id,BuildingCode,Description,CurrentStatus,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate")] ServiceModel _serviceModel)
        {
            if (ModelState.IsValid)
            {
                //Following line creates a new GUID on DEBUG mode
#if DEBUG
                _serviceModel.Id = Guid.NewGuid();
#endif
                _context.Add(_serviceModel);
                await _context.SaveChangesAsync();
                return Ok(_serviceModel);
            }
            return BadRequest(_serviceModel);

        }

        [HttpPut]
        [Route("api/[controller]/[action]")]
        public async Task<ActionResult> Update([Bind("Id,BuildingCode,Description,CurrentStatus,CreatedBy,CreatedDate,LastModifiedBy,LastModifiedDate")] ServiceModel _serviceModel)
        {
            var serviceModel = _context.Services.Where(m => m.Id == _serviceModel.Id).ToArray().FirstOrDefault();

            if (serviceModel == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(serviceModel.Copy(_serviceModel));
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return Ok();
        }

        // DELETE: ServiceModels/Delete/5
        [HttpDelete]
        [Route("api/[controller]/[action]")]
        public async Task<ActionResult> Delete(Guid? id)
        {
            var serviceModel = _context.Services.Where(m => m.Id == id).FirstOrDefault();
            if (serviceModel == null)
            {
                return NotFound(id);
            }
            else
            {
                _context.Services.Remove(serviceModel);
                await _context.SaveChangesAsync();
            }
            return StatusCode(201, "Successfully deleted");
        }
    }
}
