using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyResumeAPI.Models;
using MyResumeAPI.Models.Response;
using ResumeCore.Interface;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyResumeAPI.Controllers {
    /// <summary>
    /// Controller for the My Resume API w/ Simple Read Operations.  This acts as a Gateway API, as well as an Application API for the Resume Services.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ResumeController: ControllerBase {
        #region Members
        /// <summary>
        /// Resume Repository derived from Generic Cosmos Repository defined in ResumeInfastructure PCL
        /// </summary>
        private readonly IGenericRepository<ResumeEntity> _resumeRepo;

        /// <summary>
        /// Automapper to ease mappings from data models to request/response models
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Logging
        /// </summary>
        private readonly ILogger _logger;
        #endregion

        #region Ctor
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="resumeRepo"></param>
        /// <param name="mapper"></param>
        public ResumeController(IGenericRepository<ResumeEntity> resumeRepo, IMapper mapper, ILogger<ResumeController> logger) {
            _resumeRepo = resumeRepo;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Create
        //TODO: HttpCreate
        #endregion

        #region Read
        /// <summary>
        /// Get a single existing Resume Entity Record from the system.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An existing Resume Entity record or an Error.</returns>
        [HttpGet("{id:guid}")]
        [SwaggerResponse(200, "Success", typeof(ResumeResponse))]
        [SwaggerResponse(404, "Record Not Found", typeof(NotFoundResult))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> GetResumeById(Guid id) {
            _logger.LogInformation("Begin : Get Institution By Id", id);
            try {
                var result = await _resumeRepo.GetItemAsync(id.ToString());
                if ( result is null ) {
                    var msg = $"The resume with id({id}) was not found.";
                    _logger.LogWarning($"NotFound - {msg}");
                    return NotFound(msg);
                }
                var ret = _mapper.Map<ResumeResponse>(result);
                _logger.LogInformation("End : Get Institution By Id - Success", ret);
                return Ok(ret);
            } catch ( ArgumentException ae ) {
                _logger.LogError(ae, "BadRequest");
                return BadRequest(ae.Message);
            } catch ( Exception e ) {
                _logger.LogError(e, "Error (500)");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Gets a List of all Resume Entities currently stored in the system.
        /// </summary>
        /// <returns>A Listing of all existing Resume Entity records or an Error.</returns>
        [HttpGet("all")]
        [SwaggerResponse(200, "Success", typeof(List<ResumeResponse>))]
        [SwaggerResponse(204, "Record Not Found", typeof(NoContentResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> GetAllResumesDetailed() {
            _logger.LogInformation("Begin : Get All Institutions");
            try {
                string query = @$"SELECT * FROM c";
                var results = await _resumeRepo.GetItemsAsync(query);
                if ( ! results.Any() ) {
                    var msg = $"No Resume Records Found.";
                    _logger.LogWarning($"NoContent - {msg}");
                    return NoContent();
                }
                ResumesResponse ret = new();
                foreach ( var result in results ) {  //TODO:: improve with another automapper profile.. make automapper do this
                    ret.Resumes.Add(_mapper.Map<ResumeResponse>(result));
                }
                _logger.LogInformation("End : Get All Institutions - Success", ret);
                return Ok(ret);
            } catch ( Exception e ) {
                _logger.LogError(e, "Error (500)");
                return StatusCode(500, e.Message);
            }
        }
        #endregion

        #region Update
        //TODO: HttpPost
        #endregion

        #region Delete
        //TODO: HttpDelete
        #endregion

        #region Audit
        // TODO:: implement an audit endpoint to get the history of any record by id
        #endregion
    }
}
