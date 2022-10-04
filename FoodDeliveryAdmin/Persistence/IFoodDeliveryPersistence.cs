using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDeliveryData;

namespace FoodDeliveryAdmin.Persistence
{
    public interface IFoodDeliveryPersistence
    {
        Task<IEnumerable<OrderDTO>> ReadOrdersAsync();
        //Task<Boolean> CreateOrderAsync(OrderDTO order);
        Task<Boolean> UpdateOrderAsync(OrderDTO order);
        //Task<Boolean> DeleteOrderAsync(OrderDTO order);

        Task<IEnumerable<ProductDTO>> ReadProductListAsync();
        Task<IEnumerable<ProductDTO>> ReadProductsAsync(Int32 GroupID);
        Task<Boolean> CreateProductAsync(ProductDTO product);
        //Task<Boolean> UpdateProductAsync(ProductDTO product);
        Task<Boolean> DeleteProductAsync(ProductDTO product);

        Task<Boolean> LoginAsync(String userName, String userPassword);
        Task<Boolean> LogoutAsync();
    }
}
