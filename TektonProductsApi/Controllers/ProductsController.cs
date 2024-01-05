using Application.Products.Create;
using Application.Products.GetById;
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

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetById(Guid productId)
        {
            var result = await _mediator.Send(
                new GetProductByIdQuery(productId)
            );

            return result.Match(
                response => Ok(response),
                errors => errors.First().ToActionResult()
            );
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductsCommand command)
        {
            if (command is null)
                return BadRequest(new ArgumentNullException());

            var result = await _mediator.Send(command);

            return result.Match(
                response => Created($"{Request.Path}/{response}", response),
                errors => errors.First().ToActionResult()
            );
        }
    }
}
