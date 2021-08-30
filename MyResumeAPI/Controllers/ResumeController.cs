using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web.Resource;
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
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ResumeController: ControllerBase {
        static readonly string[] readUser = new string[] { "Reader", "Contributor" };
        static readonly string[] readWriteUser = new string[] { "Contributor" };

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
        /// <summary>
        /// Creates a new Person Entity using the Person Data supplied. 
        /// </summary>
        /// <param name="person"></param>
        /// <returns>The Id of the new record as a string.</returns>
        [HttpPost("Create")]
        [SwaggerResponse(200, "Success", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> Create([FromBody] Resume resume) {
            HttpContext.VerifyUserHasAnyAcceptedScope(readWriteUser);
            _logger.LogInformation("Begin : Create Resume", resume);
            if (resume is null) {
                var msg = "The resume parameter cannot be null.";
                _logger.LogError($"Bad Request - {msg}");
                return BadRequest(msg);
            } try {
                var resumeEntity = new ResumeEntity();
                resumeEntity.Resume = resume;
                var ret = await _resumeRepo.AddItemAsync(resumeEntity);
                _logger.LogInformation("End : Create Institution - Success", ret);
                return Ok(new { Id = ret });
            } catch (ArgumentException ae) {
                _logger.LogError(ae, "ArgumentException");
                return BadRequest(ae.Message);
            } catch (Exception e) {
                _logger.LogError(e, "Error (500)");
                return StatusCode(500, e.Message);
            }
        }
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
            HttpContext.VerifyUserHasAnyAcceptedScope(readUser);
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
        [HttpGet("All")]
        [SwaggerResponse(200, "Success", typeof(List<ResumeResponse>))]
        [SwaggerResponse(204, "Record Not Found", typeof(NoContentResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> GetAllResumesDetailed() {
            HttpContext.VerifyUserHasAnyAcceptedScope(readUser);
            _logger.LogInformation("Begin : Get All Resumes");
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
        /// <summary>
        /// Update an existing Resume Entity in the system using HttpPut to perform an update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resume"></param>
        /// <returns>The updated Resume Entity record or an Error.</returns>
        [HttpPut("{id:guid}")]
        [SwaggerResponse(200, "Success", typeof(ResumeResponse))]
        [SwaggerResponse(404, "Record Not Found", typeof(NotFoundResult))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] Resume resume) {
            HttpContext.VerifyUserHasAnyAcceptedScope(readWriteUser);
            _logger.LogInformation("Begin : Update Person w/ PUT", new { id, resume });
            if (resume is null) {
                var msg = "The resume parameter cannot be null.";
                _logger.LogError($"Bad Request - {msg}");
                return BadRequest(msg);
            } try {
                var item = await _resumeRepo.GetItemAsync(id.ToString());
                if (item is null) {
                    var msg = $"The resume with id({id}) was not found.";
                    _logger.LogWarning($"NotFound - {msg}");
                    return NotFound(msg);
                }
                item.Resume = resume;
                await _resumeRepo.UpdateItemAsync(id.ToString(), item);
                var result = await _resumeRepo.GetItemAsync(id.ToString());
                var ret = _mapper.Map<ResumeResponse>(result);
                _logger.LogInformation("End : Update Resume w/ PUT - Success", ret);
                return Ok(ret);
            } catch (ArgumentException ae) {
                _logger.LogError(ae, "BadRequest");
                return BadRequest(ae.Message);
            } catch (Exception e) {
                _logger.LogError(e, "Error (500)");
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Update an existing Resume Entity in the system using HttpPatch to perform a soft update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="resume"></param>
        /// <returns>The updated Resume Entity record or an Error.</returns>
        [HttpPatch("{id:guid}")]
        [SwaggerResponse(200, "Success", typeof(ResumeResponse))]
        [SwaggerResponse(404, "Record Not Found", typeof(NotFoundResult))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> SoftUpdate([FromRoute] Guid id, [FromBody] JsonPatchDocument<Resume> resume) {
            HttpContext.VerifyUserHasAnyAcceptedScope(readWriteUser);
            _logger.LogInformation("Begin : Update Person w/ PATCH", new { id, resume });
            if (resume is null) {
                var msg = "The resume parameter cannot be null.";
                _logger.LogError($"Bad Request - {msg}");
                return BadRequest(msg);
            } try {
                var item = await _resumeRepo.GetItemAsync(id.ToString());
                if (item is null) {
                    var msg = $"The resume with id({id}) was not found.";
                    _logger.LogWarning($"NotFound - {msg}");
                    return NotFound(msg);
                }
                resume.ApplyTo(item.Resume, ModelState);
                await _resumeRepo.UpdateItemAsync(id.ToString(), item);
                var result = await _resumeRepo.GetItemAsync(id.ToString());
                var ret = _mapper.Map<ResumeResponse>(result);
                _logger.LogInformation("End : Update Resume w/ PATCH - Success", ret);
                return Ok(ret);
            } catch (ArgumentException ae) {
                _logger.LogError(ae, "BadRequest");
                return BadRequest(ae.Message);
            } catch (Exception e) {
                _logger.LogError(e, "Error (500)");
                return StatusCode(500, e.Message);
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Removes an existing Resume Entity from the system.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ok Response or Error.</returns>
        [HttpDelete("{id:guid}")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(404, "Record Not Found", typeof(NotFoundResult))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> Remove([FromRoute] Guid id) {
            HttpContext.VerifyUserHasAnyAcceptedScope(readWriteUser);
            _logger.LogInformation("Begin : Remove Resume", id);
            try {
                if (!await _resumeRepo.DeleteItemAsync(id.ToString())) {
                    var msg = $"The resume with id({id}) was not found.";
                    _logger.LogWarning($"NotFound - {msg}");
                    return NotFound(msg);
                }
                _logger.LogInformation("End : Remove Resume - Success", id);
                return Ok();
            } catch (ArgumentException ae) {
                _logger.LogError(ae, "BadRequest");
                return BadRequest(ae.Message);
            } catch (Exception e) {
                _logger.LogError(e, "Error (500)");
                return StatusCode(500, e.Message);
            }
        }
        #endregion

        #region Audit
        // TODO:: implement an audit endpoint to get the history of any record by id
        #endregion
    }
}
