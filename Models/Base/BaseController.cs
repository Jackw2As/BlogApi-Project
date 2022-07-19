using Domain.ActionResults;
using Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Base
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseController<T> : ControllerBase where T : BaseObject
    {
        protected IRepository<T> Repository { get; init; }
        internal protected BaseController(IRepository<T> repository)
        {
            Repository = repository;
        }

        //Returns a Specific Model
        [HttpGet(Order = 100)]
        internal protected virtual ActionResult<T> GetById(Guid Id)
        {
            if(!Repository.Exists(Id))
            {
                return NotFound(Id);
            }
            return new OkObjectResult(Repository.GetByID(Id));
        }

        //Updates a Model
        [HttpPost(Order = 100)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        internal protected virtual ActionResult<T> Post(T model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                if (Repository.Exists(model.ID))
                {
                    Repository.Modify(model);
                }
                else
                {
                    Repository.Save(model);
                }

                return CreatedAtAction("GetById", new { id = model.ID }, model);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        //Deletes a Model.
        [HttpDelete(Order = 100)]
        internal protected virtual IActionResult Delete(Guid Id)
        {
            if(!Repository.Exists(Id))
            {
                return NotFound(Id);
            }
            var result = Repository.Delete(Id);
            if(result)
            {
                return Ok();
            }
            return new ServerError("Could not be deleted by Storage for some reason");
        }
    }
}
