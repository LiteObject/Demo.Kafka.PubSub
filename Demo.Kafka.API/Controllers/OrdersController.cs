namespace Demo.Kafka.API.Controllers
{
    using Demo.Kafka.API.Data.Repositories;
    using Demo.Kafka.API.Domain.Entities;
    using Demo.Kafka.API.Extensions;
    using Humanizer;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /* If we don't want versioning here, we can implement as a header value */
    [Route("api/[controller]")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiVersion("1.1")]
    [ApiVersion("2.0")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IRepository<Order> orderRepo;

        public OrdersController(ILogger<OrdersController> logger, IRepository<Order> orderRepo)
        {
            this.orderRepo = orderRepo;
            this._logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // ToDo: Use repo
            if (id == 123)
            {
                return this.Ok(await Task.FromResult(new Order { Id = 123 }));
            }
            else
            {
                return this.NotFound($"No record (with id {id}) found in the system.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > 100)
            {
                // ToDo: Future requirment - we want to notify caller if page size is greater than 100.
                // return BadRequest(...);

                pageSize = 100;
            }

            var orders = await orderRepo.GetAllAsync(pageNumber, pageSize);

            if (orders is null || !orders.Any())
            {
                // ToDo: NotFound object without a message fails unit text "Assert.IsType<NotFoundObjectResult>(...)"
                return NotFound($"We didn't find any records. Page Number: {pageNumber}, Page Size: {pageSize}");
            }

            return Ok(orders);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] DTO.Order order)
        {
            // We can do this validation globally as well.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest($"Route id ({id}) doesn't match with object id ({order.Id}) ");
            }

            var orderEntity = await orderRepo.GetAsync(id);

            if (orderEntity is null)
            {
                return NotFound("No record found in the system");
            }

            // 200 (OK) or 204 (No Content)
            return Ok(await Task.FromResult(order));
        }

        /// <summary>
        /// Payload example: 
        /// [
        ///     {"op":"add", "path":"/note", "value":"some new value here"}
        /// ]
        /// 
        /// More on Patch: https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> Patch([FromRoute] int id, [FromBody] JsonPatchDocument<DTO.Order> patchDocument)
        {
            if (patchDocument is null)
            {
                return BadRequest($"{nameof(patchDocument)} cannot be null.");
            }

            // Does a record exist?
            var order = await this.orderRepo.GetAsync(id);

            if (order is null)
            {
                return NotFound($"No order with id# {id} found in the system.");
            }

            // ToDo: Use AutoMapper. Currently using an ext method.
            var orderToPatch = order.ToDto();
            patchDocument.ApplyTo(orderToPatch, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState);

            // Validate request object. This is a very important step.
            // We need to have unit test for this.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderEntity = orderToPatch.ToEntity();

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DTO.Order newOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // From DTO to Domain Entity/Model
            var orderEntity = newOrder.ToEntity();

            await this.orderRepo.Add(orderEntity);
            await this.orderRepo.SaveChangesAsync();

            // 202 (Accepted) - Request has been accepted for processing, but the processing
            // has not been completed. The request might or might not be eventually acted upon.
            // return this.Accepted(new Uri($"/api/orders/{newOrder.Id}"));
            return this.CreatedAtAction(nameof(GetById), new { id = orderEntity.Id }, orderEntity.ToDto());
        }

        [NonAction]
        public async Task<IActionResult> Get(string message)
        {
            throw new InvalidOperationException(message);
        }
    }
}
