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
using ServiceRequests.Services;

namespace ServiceRequests.Controllers
{
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IEmailServices _emailServices;

        //Injected logger to use swagger and context to use db
        //Email injection is only being used to fulfill requested items from challenge. Other solutions might be possible.
        public ServicesController(DatabaseContext context, ILogger<ServicesController> logger
            , IEmailServices emailServices)
        {
            _logger = logger;
            _context = context;
            _emailServices = emailServices;
        }

        private readonly ILogger<ServicesController> _logger;

        // GET: ServiceModels
        /// <summary>
        /// Read all service requests
        /// </summary>
        /// <returns>200:list of service requests || 204: empty content</returns>
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
        /// <summary>
        /// Read service request by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>200: single service request || 404: not found</returns>
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
        /// <summary>
        /// Create new service request
        /// </summary>
        /// <param name="serviceModel"></param>
        /// <returns>201: created service request with id || 400: bad request</returns>
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

        /// <summary>
        /// Update service request based on id
        /// </summary>
        /// <param name="serviceModel"></param>
        /// <returns>200: updated service request || 400: bad service request || 404: not found</returns>
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
                    if(_serviceModel.CurrentStatus == CurrentStatus.Canceled || _serviceModel.CurrentStatus == CurrentStatus.Complete)
                        _emailServices.SendEmail(serviceModel);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return Ok();
        }

        // DELETE: ServiceModels/Delete/5
        /// <summary>
        /// Delete service request based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>201: successful || 404: not found </returns>
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
