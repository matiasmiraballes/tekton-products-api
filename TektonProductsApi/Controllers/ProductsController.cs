using Application.Products.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TektonProductsApi.Contracts;

namespace TektonProductsApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ISender _mediator;

        public ProductsController(ISender mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductsCommand command)
        {
            if (command is null)
                return BadRequest(new ArgumentNullException());

            var result = await _mediator.Send(command);

            return result.Match(
                response => CreatedAtAction("Get", new { id = response }),
                errors => errors.First().ToActionResult()
            );
        }
    }
}
