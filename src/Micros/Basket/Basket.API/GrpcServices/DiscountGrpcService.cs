using System;
using Discount.Grpc.Protos;
using System.Threading.Tasks;

namespace Basket.API.GrpcServices
{
    public interface IDiscountGrpcService
    {
        public Task<CouponModel> GetDiscountAsync(string productName);
    }

    public class DiscountGrpcService : IDiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountServiceClient;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountServiceClient)
            => _discountServiceClient = discountServiceClient ?? throw new ArgumentNullException(nameof(discountServiceClient));

        public async Task<CouponModel> GetDiscountAsync(string productName)
            => await _discountServiceClient.GetDiscountAsync(new GetDiscountRequest { ProductName = productName });
    }
}