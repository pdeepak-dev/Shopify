using Dapper;
using Npgsql;
using System;
using Discount.API.Entities;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Discount.API.Repositories
{
    public interface IDiscountRepository
    {
        public Task<Coupon> GetDiscountAsync(string productName);
        public Task<bool> CreateDiscountAsync(Coupon coupon);
        public Task<bool> UpdateDiscountAsync(Coupon coupon);
        public Task<bool> DeleteDiscountAsync(string productName);
    }

    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;
        private NpgsqlConnection GetConn() => new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

        public DiscountRepository(IConfiguration configuration)
            => _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

        public async Task<bool> CreateDiscountAsync(Coupon coupon)
        {
            using var conn = GetConn();
            var affected = await conn.ExecuteAsync
                ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount });

            return affected != 0;
        }

        public async Task<bool> DeleteDiscountAsync(string productName)
        {
            using var connection = GetConn();

            var affected = await connection.ExecuteAsync
                ("DELETE FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            return affected != 0;
        }

        public async Task<Coupon> GetDiscountAsync(string productName)
        {
            using var conn = GetConn();

            var coupon = await conn.QueryFirstOrDefaultAsync<Coupon>
                ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            return coupon ?? new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" }; ;
        }

        public async Task<bool> UpdateDiscountAsync(Coupon coupon)
        {
            using var connection = GetConn();

            var affected = await connection.ExecuteAsync
                    ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
                            new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            return affected != 0;
        }
    }
}