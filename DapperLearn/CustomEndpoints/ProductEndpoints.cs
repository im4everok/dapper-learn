using Dapper;

using DapperLearn.Models;
using DapperLearn.Services;

namespace DapperLearn.CustomEndpoints
{
    public static class ProductEndpoints
    {
        public static void MapProductEndpoints(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("product");

            group.MapGet("withOrders", async (SqlConnectionFactory factory) =>
            {
                const string sql = @"SELECT p.Id, p.Name, p.Price, o.Id, o.DateOfOrder, o.ProductId
                                     FROM Products p
                                     JOIN Orders o
                                     ON p.Id = o.ProductId";

                using var connection = factory.Create();

                IEnumerable<Product> productsWithOrdersUngrouped = await connection.QueryAsync<Product, Order, Product>(sql, (product, order) =>
                {
                   if(product.Orders == null) product.Orders = new List<Order>();

                   product.Orders.Add(order);
                   return product;
                }, splitOn: "Id");

                var groupedProductsWithOrders = productsWithOrdersUngrouped.GroupBy(b => b.Id)
                    .Select(g =>
                    {
                        var groupedBook = g.First();
                        groupedBook.Orders = g.Select(b => b.Orders.Single()).ToList();
                        return groupedBook;
                    });

                return groupedProductsWithOrders;
            });
        }
    }
}
