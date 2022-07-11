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
        public BaseController(IRepository<T> repository)
        {
            Repository = repository;
        }

        //Returns a Specific Model
        [HttpGet]
        public virtual ActionResult<T> Get(Guid Id)
        {
            if(!Repository.Exists(Id))
            {
                return NotFound(Id);
            }
            return Repository.GetByID(Id);
        }

        //Updates a Model
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual ActionResult<T> Post(T model)
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

                return CreatedAtAction(nameof(Post), nameof(this.GetType), model, model);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        //Deletes a Model.
        [HttpDelete]
        public virtual IActionResult Delete(Guid Id)
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
