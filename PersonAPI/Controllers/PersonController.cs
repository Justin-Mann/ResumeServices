using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonAPI.Models;
using PersonAPI.Models.Response;
using ResumeCore.Entity.Models;
using ResumeCore.Interface;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonAPI.Controllers {
    /// <summary>
    /// Controller for the Person Service API w/ CRUD Operations.  This acts as an API for the Person Microservice.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PersonController: ControllerBase {
        #region Members
        /// <summary>
        /// Person Repository derived from Generic Cosmos Repository defined in ResumeInfastructure PCL
        /// </summary>
        private readonly IGenericRepository<PersonEntity> _personRepo;

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
        /// <param name="personRepo"></param>
        /// <param name="mapper"></param>
        public PersonController(IGenericRepository<PersonEntity> personRepo, IMapper mapper, ILogger<PersonController> logger) {
            _personRepo = personRepo;
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
        [HttpPost("create")]
        [SwaggerResponse(200, "Success", typeof(string))]
        [SwaggerResponse(404, "Record Not Found", typeof(NotFoundResult))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> Create([FromBody] Person person) {
            _logger.LogInformation("Begin : Create Person", person);
            if ( person is null ) {
                var msg = "The person parameter cannot be null.";
                _logger.LogError($"Bad Request - {msg}");
                return BadRequest(msg);
            }
            try {
                var personEntity = new PersonEntity();
                personEntity.Person = person;
                var ret = await _personRepo.AddItemAsync(personEntity);
                _logger.LogInformation("End : Create Institution - Success", ret);
                return Ok(ret);
            } catch( ArgumentException ae ) {
                _logger.LogError(ae, "ArgumentException");
                return BadRequest( ae.Message );
            } catch( Exception e ) {
                _logger.LogError(e, "Error (500)");
                return StatusCode( 500, e.Message );
            }
        }
        #endregion

        #region Read
        /// <summary>
        /// Get a single existing Person Entity Record from the system.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>An existing Person Entity record or an Error.</returns>
        [HttpGet("{id:guid}")]
        [SwaggerResponse(200, "Success", typeof(PersonResponse))]
        [SwaggerResponse(404, "Record Not Found", typeof(NotFoundResult))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> GetById([FromRoute] Guid id) {
            _logger.LogInformation("Begin : Get Person By Id", id);
            try {
                var result = await _personRepo.GetItemAsync(id.ToString());
                if ( result is null ) {
                    var msg = $"The person with id({id}) was not found.";
                    _logger.LogWarning($"NotFound - {msg}");
                    return NotFound(msg);
                }
                var ret = _mapper.Map<PersonResponse>(result);
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
        /// Gets a List of all Person Entities currently stored in the system.
        /// </summary>
        /// <returns>A Listing of all existing Person Entity records or an Error.</returns>
        [HttpGet("all")]
        [SwaggerResponse(200, "Success", typeof(List<PersonResponse>))]
        [SwaggerResponse(204, "Record Not Found", typeof(NoContentResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> GetAll() {
            _logger.LogInformation("Begin : Get All People");
            try {
                string query = @$"SELECT * FROM c";
                var results = await _personRepo.GetItemsAsync(query);
                if ( ! results.Any() ) {
                    var msg = $"No Person Records Found.";
                    _logger.LogWarning($"NoContent - {msg}");
                    return NoContent();
                }
                PeopleResponse ret = new();
                foreach ( var result in results ) { //TODO:: improve with another automapper profile.. make automapper do this
                    ret.People.Add(_mapper.Map<PersonResponse>(result));
                }
                _logger.LogInformation("End : Get All People - Success", ret);
                return Ok(ret);
            } catch ( Exception e ) {
                _logger.LogError(e, "Error (500)");
                return StatusCode(500, e.Message);
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Update an existing Person Entity in the system using HttpPut to perform an update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns>The updated Person Entity record or an Error.</returns>
        [HttpPut("put/{id:guid}")]
        [SwaggerResponse(200, "Success", typeof(PersonResponse))]
        [SwaggerResponse(404, "Record Not Found", typeof(NotFoundResult))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] Person person) {
            _logger.LogInformation("Begin : Update Person w/ PUT", new { id, person });
            if ( person is null ) {
                var msg = "The person parameter cannot be null.";
                _logger.LogError($"Bad Request - {msg}");
                return BadRequest(msg);
            }
            try {
                var item = await _personRepo.GetItemAsync(id.ToString());
                if ( item is null ) {
                    var msg = $"The person with id({id}) was not found.";
                    _logger.LogWarning($"NotFound - {msg}");
                    return NotFound(msg);
                }
                item.Person = person;
                await _personRepo.UpdateItemAsync(id.ToString(), item);
                var result = await _personRepo.GetItemAsync(id.ToString());
                var ret = _mapper.Map<PersonResponse>(result);
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
        /// Update an existing Person Entity in the system using HttpPatch to perform a soft update.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="person"></param>
        /// <returns>The updated Person Entity record or an Error.</returns>
        [HttpPatch("patch/{id:guid}")]
        [SwaggerResponse(200, "Success", typeof(PersonResponse))]
        [SwaggerResponse(404, "Record Not Found", typeof(NotFoundResult))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> SoftUpdate([FromRoute] Guid id, [FromBody] JsonPatchDocument<Person> person) {
            _logger.LogInformation("Begin : Update Person w/ PATCH", new { id, person }); 
            if ( person is null ) {
                var msg = "The person parameter cannot be null.";
                _logger.LogError($"Bad Request - {msg}");
                return BadRequest(msg);
            }
            try {
                var item = await _personRepo.GetItemAsync(id.ToString());
                if ( item is null ) {
                    var msg = $"The person with id({id}) was not found.";
                    _logger.LogWarning($"NotFound - {msg}");
                    return NotFound(msg);
                }
                person.ApplyTo(item.Person, ModelState);
                await _personRepo.UpdateItemAsync(id.ToString(), item);
                var result = await _personRepo.GetItemAsync(id.ToString());
                var ret = _mapper.Map<PersonResponse>(result);
                _logger.LogInformation("End : Update Person w/ PATCH - Success", ret);
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
        /// Removes an existing Person Entity from the system.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ok Response or Error.</returns>
        [HttpDelete("remove/{id:guid}")]
        [SwaggerResponse(200, "Success")]
        [SwaggerResponse(404, "Record Not Found", typeof(NotFoundResult))]
        [SwaggerResponse(400, "Bad Request", typeof(BadRequestResult))]
        [SwaggerResponse(500, "An Error Has Occured", typeof(StatusCodeResult))]
        public async Task<IActionResult> Remove([FromRoute] Guid id) {
            _logger.LogInformation("Begin : Remove Person", id);
            try {
                if ( ! await _personRepo.DeleteItemAsync(id.ToString()) ) {
                    var msg = $"The person with id({id}) was not found.";
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
