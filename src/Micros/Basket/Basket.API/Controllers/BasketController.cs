using AutoMapper;
using System.Net;
using MassTransit;
using Basket.API.Entities;
using System.Threading.Tasks;
using Basket.API.Repositories;
using Basket.API.GrpcServices;
using Microsoft.AspNetCore.Mvc;
using EventBus.Messages.Events;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IDiscountGrpcService _discountGrpcService;
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publish;

        public BasketController(IDiscountGrpcService discountGrpcService, IBasketRepository basketRepository, IMapper mapper, IPublishEndpoint publish)
        {
            _discountGrpcService = discountGrpcService;
            _basketRepository = basketRepository;
            _mapper = mapper;
            _publish = publish;
        }

        [HttpGet("{userName}", Name = "GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetBasketAsync(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            foreach (var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscountAsync(item.ProductName);

                if (coupon != null)
                    item.Price -= coupon.Amount;
            }

            return Ok(await _basketRepository.UpdateBasketAsync(basket));
        }

        [HttpDelete("{userName}", Name = "DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _basketRepository.DeleteBasketAsync(userName);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _basketRepository.GetBasketAsync(basketCheckout.UserName);

            if (basket == null) return BadRequest();

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.TotalPrice = basket.TotalPrice;
            await _publish.Publish(eventMessage);

            await _basketRepository.DeleteBasketAsync(basket.UserName);

            return Accepted();
        }
    }
}