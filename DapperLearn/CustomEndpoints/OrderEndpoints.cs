using Dapper;

using DapperLearn.Models;
using DapperLearn.Services;

namespace DapperLearn.CustomEndpoints
{
    public static class OrderEndpoints
    {
        public static void MapOrderEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("orders");

            group.MapGet("withProduct", async (SqlConnectionFactory factory) =>
            {
                const string sql = @"SELECT o.Id, o.DateOfOrder, p.Id as ProductId, p.Name, p.Price
                                     FROM Orders o
                                     JOIN Products p
                                     on o.ProductId = p.Id";

                using var connection = factory.Create();

                var ordersWithProduct = await connection.QueryAsync<Order, Product, Order>(sql, (order, product) =>
                {
                    order.Product = product;
                    return order;
                }, splitOn: "ProductId");

                return ordersWithProduct;
            });
        }
    }
}
