using Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Base
{
    [ApiController]
    [Route("[controller]")]
    public abstract class BaseController<T> where T : BaseModel
    {
        protected abstract IRepository<T> Repository { get; init; }

        //Returns a Specific Model
        [HttpGet]
        public IActionResult Get(Guid Id)
        {
            var model = Repository.Read(Id);
            return new OkObjectResult(Repository.Read(Id)); ;
        }

        //Returns a generic list of Models
        [HttpGet]
        public IActionResult Get()
        {
            var models = Repository.ReadMultiple();
            return new OkObjectResult(models);
        }

        //Updates a Model
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post(T model)
        {
            try
            {
                if (Repository.Exists(model))
                {
                    Repository.Update(model);
                }
                else
                {
                    Repository.Create(model);
                }

                return new CreatedAtActionResult(nameof(Post), nameof(this.GetType), model, model);
            }
            catch(Exception ex)
            {
                return new BadRequestResult();
            }
        }

        //Deletes a Model.
        [HttpDelete]
        public void Delete(Guid Id)
        {

        }
    }
}
