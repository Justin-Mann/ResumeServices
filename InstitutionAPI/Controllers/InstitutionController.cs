using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using InstitutionAPI.Models;
using ResumeCore.Interface;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstitutionAPI.Models.Response;
using Microsoft.Extensions.Logging;

namespace InstitutionAPI.Controllers {
    /// <summary>
    /// Controller for the Institution Service API w/ CRUD Operations.  This acts as an API for the Institution Microservice.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class InstitutionController: ControllerBase {
        #region Members
        /// <summary>
        /// Institution Repository derived from Generic Cosmos Repository defined in ResumeInfastructure PCL
        /// </summary>
        private readonly IGenericRepository<InstitutionEntity> _institutionRepo;

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
        /// <param name="institutionRepo"></param>
        /// <param name="mapper"></param>
        public InstitutionController(IGenericRepository<InstitutionEntity> institutionRepo, IMapper mapper, ILogger<InstitutionEntity> logger) {
            _institutionRepo = institutionRepo;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Create
        /// <summary>
        /// Creates a new Institution Entity using the Person Data supplied. 
        /// </summary>
        /// <param name="institution"></param>
        /// <returns>The Id of the new record as a string.</returns>
        [HttpPost("create")]
        [SwaggerResponse(200, "Success", typeof(string))]
        [SwaggerResponse(404, "Record Not Found", typeof(NotFoundResult))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> Create([FromBody] Institution institution) {
            _logger.LogInformation("Begin : Create Institution", institution);
            if ( institution is null ) {
                var msg = "The institution parameter cannot be null.";
                _logger.LogError($"Bad Request - {msg}");
                return BadRequest(msg);
            }
            try {
                var institutionEntity = new InstitutionEntity();
                institutionEntity.Institution = institution;
                var ret = await _institutionRepo.AddItemAsync(institutionEntity);
                _logger.LogInformation("End : Create Institution - Success", ret);
                return Ok(ret);
            } catch ( ArgumentException ae ) {
                _logger.LogError(ae, "ArgumentException");
                return BadRequest(ae.Message);
            } catch ( Exception e ) {
                _logger.LogError(e, "Error (500)");
                return StatusCode(500, e.Message);
            }
        }
        #endregion

        #region Read
        /// <summary>
        /// Get a single existing Institution Entity Record from the system.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An existing Institution Entity record or an Error.</returns>
        [HttpGet("{id:guid}")]
        [SwaggerResponse(200, "Success", typeof(InstitutionResponse))]
        [SwaggerResponse(404, "Record Not Found", typeof(NotFoundResult))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            _logger.LogInformation("Begin : Get Institution By Id", id);
            try {
                var result = await _institutionRepo.GetItemAsync(id.ToString());
                if ( result is null ) {
                    var msg = $"The institution with id({id}) was not found.";
                    _logger.LogWarning($"NotFound - {msg}");
                    return NotFound(msg);
                }
                var ret = _mapper.Map<InstitutionResponse>(result);
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
        /// Gets a List of all Institution Entities currently stored in the system.
        /// </summary>
        /// <returns>A Listing of all existing Institution Entity records or an Error.</returns>
        [HttpGet("all")]
        [SwaggerResponse(200, "Success", typeof(List<InstitutionResponse>))]
        [SwaggerResponse(204, "Record Not Found", typeof(NoContentResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> GetAll() {
            _logger.LogInformation("Begin : Get All Institutions");
            try {
                string query = @$"SELECT * FROM c";
                var results = await _institutionRepo.GetItemsAsync(query);
                if ( !results.Any() ) {
                    var msg = $"No Institution Records Found.";
                    _logger.LogWarning($"NoContent - {msg}");
                    return NoContent();
                }
                InstitutionsResponse ret = new();
                foreach ( var result in results ) { //TODO:: improve with another automapper profile.. make automapper do this
                    ret.Institutions.Add(_mapper.Map<InstitutionResponse>(result));
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
        /// Update an existing Institution Entity in the system using HttpPut to perform an update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="institution"></param>
        /// <returns>The updated Institution Entity record or an Error.</returns>
        [HttpPut("put/{id:guid}")]
        [SwaggerResponse(200, "Success", typeof(InstitutionResponse))]
        [SwaggerResponse(404, "Record Not Found", typeof(NotFoundResult))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] Institution institution) {
            _logger.LogInformation("Begin : Update Institution w/ PUT", new { id, institution });
            if ( institution is null ) {
                var msg = "The institution parameter cannot be null.";
                _logger.LogError($"Bad Request - {msg}");
                return BadRequest(msg);
            }
            try {
                var item = await _institutionRepo.GetItemAsync(id.ToString());
                if ( item is null ) {
                    var msg = $"The institution with id({id}) was not found.";
                    _logger.LogWarning($"NotFound - {msg}");
                    return NotFound(msg);
                }
                item.Institution = institution;
                await _institutionRepo.UpdateItemAsync(id.ToString(), item);
                var result = await _institutionRepo.GetItemAsync(id.ToString());
                var ret = _mapper.Map<InstitutionResponse>(result);
                _logger.LogInformation("End : Update Institution w/ PUT - Success", ret);
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
        /// Update an existing Institution Entity in the system using HttpPatch to perform a soft update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="institution"></param>
        /// <returns>The updated Institution Entity record or an Error.</returns>
        [HttpPatch("patch/{id:guid}")]
        [SwaggerResponse(200, "Success", typeof(InstitutionResponse))]
        [SwaggerResponse(404, "Record Not Found", typeof(NotFoundResult))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> SoftUpdate([FromRoute] Guid id, [FromBody] JsonPatchDocument<Institution> institution) {
            _logger.LogInformation("Begin : Update Institution w/ PATCH", new { id, institution });
            if ( institution is null ) {
                var msg = "The institution parameter cannot be null.";
                _logger.LogError($"Bad Request - {msg}");
                return BadRequest(msg);
            }
            try {
                var item = await _institutionRepo.GetItemAsync(id.ToString());
                if ( item is null ) {
                    var msg = $"The institution with id({id}) was not found.";
                    _logger.LogWarning($"NotFound - {msg}");
                    return NotFound(msg);
                }
                institution.ApplyTo(item.Institution, ModelState);
                await _institutionRepo.UpdateItemAsync(id.ToString(), item);
                var result = await _institutionRepo.GetItemAsync(id.ToString());
                var ret = _mapper.Map<InstitutionResponse>(result);
                _logger.LogInformation("End : Update Institution w/ PATCH - Success", ret);
                return Ok(ret);
            } catch ( ArgumentException ae ) {
                _logger.LogError(ae, "BadRequest");
                return BadRequest(ae.Message);
            } catch ( Exception e ) {
                _logger.LogError(e, "Error (500)");
                return StatusCode(500, e.Message);
            }
        }
        #endregion

        #region Delete
        /// <summary>
        /// Removes an existing Institution Entity from the system.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ok Response or Error.</returns>
        [HttpDelete("remove/{id:guid}")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(404, "Record Not Found", typeof(NotFoundResult))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> Remove([FromRoute] Guid id) {
            _logger.LogInformation("Begin : Remove Institution", id);
            try {
                if ( ! await _institutionRepo.DeleteItemAsync(id.ToString()) ) {
                    var msg = $"The institution with id({id}) was not found.";
                    _logger.LogWarning($"NotFound - {msg}");
                    return NotFound(msg);
                }
                _logger.LogInformation("End : Remove Institution - Success", id);
                return Ok();
            } catch ( ArgumentException ae ) {
                _logger.LogError(ae, "BadRequest");
                return BadRequest(ae.Message);
            } catch ( Exception e ) {
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
