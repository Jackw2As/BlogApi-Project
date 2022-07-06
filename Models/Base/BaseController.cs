﻿using Domain.ActionResults;
using Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Base
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseController<T> : ControllerBase where T : BaseObject
    {
        protected abstract IRepository<T> Repository { get; init; }

        //Returns a Specific Model
        [HttpGet]
        public virtual IActionResult Get(Guid Id)
        {
            var model = Repository.GetByID(Id);
            if(model == null)
            {
                return NotFound(Id);
            }
            return Ok(Repository.GetByID(Id));
        }

        //Updates a Model
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public virtual IActionResult Post(T model)
        {
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
                return BadRequest(ex);
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
