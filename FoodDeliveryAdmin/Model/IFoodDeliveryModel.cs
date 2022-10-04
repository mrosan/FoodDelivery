using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDeliveryData;

namespace FoodDeliveryAdmin.Model
{
    public interface IFoodDeliveryModel
    {

        IReadOnlyList<OrderDTO> Orders { get; }
        IReadOnlyList<ProductDTO> Products { get; }
        IReadOnlyList<ProductDTO> SelectedProducts { get; }
        Boolean IsUserLoggedIn { get; }

        //void CreateOrder(OrderDTO order);
        void UpdateOrder(OrderDTO order);
        //void DeleteOrder(OrderDTO order);
        void CreateProduct(ProductDTO product);
        //void UpdateProduct(ProductDTO product);
        void DeleteProduct(ProductDTO product);

        Task LoadAsync();
        Task SaveAsync();
        Task AssembleProductListAsync(int OrderGroup);

        Task<Boolean> LoginAsync(String userName, String userPassword);
        Task<Boolean> LogoutAsync();

    }
}
