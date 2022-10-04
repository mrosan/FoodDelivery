using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDeliveryAdmin.Persistence;
using FoodDeliveryData;

namespace FoodDeliveryAdmin.Model
{
    public class FoodDeliveryModel : IFoodDeliveryModel
    {
        private enum DataFlag
        {
            Create,
            Update,
            Delete
        }

        private IFoodDeliveryPersistence _persistence;
        private List<OrderDTO> _orders;
        private Dictionary<OrderDTO, DataFlag> _orderFlags;
        private List<ProductDTO> _products;
        private Dictionary<ProductDTO, DataFlag> _productFlags;
        private List<ProductDTO> _selectedProducts;

        public FoodDeliveryModel(IFoodDeliveryPersistence persistence)
        {
            if (persistence == null)
                throw new ArgumentNullException(nameof(persistence));

            IsUserLoggedIn = false;
            _persistence = persistence;
        }

        public Boolean IsUserLoggedIn { get; private set; }

        public IReadOnlyList<OrderDTO> Orders
        {
            get { return _orders; }
        }

        public IReadOnlyList<ProductDTO> Products
        {
            get { return _products; }
        }

        public IReadOnlyList<ProductDTO> SelectedProducts
        {
            get { return _selectedProducts; }
        }

        // UPDATE an order: set from "Not Completed" to "Completed"
        public void UpdateOrder(OrderDTO order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            //note: in this case List is unnecessary
            //OrderDTO orderToModify = _orders.FirstOrDefault(o => o.OrderGroup == order.OrderGroup);
            List<OrderDTO> ordersToModify = _orders.Where(o => o.OrderGroup == order.OrderGroup).ToList();

            if (ordersToModify == null)
                throw new ArgumentException("The order does not exist.", nameof(order));

            foreach(OrderDTO o in ordersToModify)
            {
                if (!o.Completed)
                {
                    o.Completed = true;
                    o.DateCompleted = DateTime.Now;
                }

                _orderFlags[o] = DataFlag.Update;
            }
            
        }


        // CREATE a new product
        public void CreateProduct(ProductDTO product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            if (_products.Contains(product))
            {
                throw new ArgumentException("The product is already in the collection.", nameof(product));
            }

            product.Id = (_products.Count > 0 ? _products.Max(b => b.Id) : 0) + 1; // generate new, temporary ID (won't go through to server)
            _productFlags.Add(product, DataFlag.Create);
            _products.Add(product);
        }


        // DELETE an existing product
        public void DeleteProduct(ProductDTO product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            ProductDTO productToDelete = _products.FirstOrDefault(b => b.Id == product.Id);

            if (productToDelete == null)
                throw new ArgumentException("The order does not exist.", nameof(product));

            // if the deleted data is new, we don't have to delete it from the server
            if (_productFlags.ContainsKey(productToDelete) && _productFlags[productToDelete] == DataFlag.Create)
            {
                _productFlags.Remove(productToDelete);
            } else
            {
                _productFlags[productToDelete] = DataFlag.Delete;
            }

            _products.Remove(productToDelete);
        }

        public async Task AssembleProductListAsync(int OrderGroup)
        {
            _selectedProducts = (await _persistence.ReadProductsAsync(OrderGroup)).ToList();
        }

        public async Task LoadAsync()
        {
            _orders = (await _persistence.ReadOrdersAsync()).ToList();
            _orderFlags = new Dictionary<OrderDTO, DataFlag>();
            _products = (await _persistence.ReadProductListAsync()).ToList();
            _productFlags = new Dictionary<ProductDTO, DataFlag>();
        }

        public async Task SaveAsync()
        {
            List<OrderDTO> ordersToSave = _orderFlags.Keys.ToList();
            foreach (OrderDTO order in ordersToSave)
            {
                Boolean result = true;
                switch (_orderFlags[order])
                {
                    case DataFlag.Create:
                        //result = await _persistence.CreateOrderAsync(order);
                        break;
                    case DataFlag.Delete:
                        //result = await _persistence.DeleteOrderAsync(order);
                        break;
                    case DataFlag.Update:
                        result = await _persistence.UpdateOrderAsync(order);
                        break;
                }
                if (!result)
                {
                    throw new InvalidOperationException("Operation " + _orderFlags[order] + " failed on building " + order.OrderGroup);
                }
                _orderFlags.Remove(order);
            }

            List<ProductDTO> productsToSave = _productFlags.Keys.ToList();
            foreach (ProductDTO product in productsToSave)
            {
                Boolean result = true;
                switch (_productFlags[product])
                {
                    case DataFlag.Create:
                        result = await _persistence.CreateProductAsync(product);
                        break;
                    case DataFlag.Delete:
                        result = await _persistence.DeleteProductAsync(product);
                        break;
                    case DataFlag.Update:
                        //result = await _persistence.UpdateProductAsync(product);
                        break;
                }
                if (!result)
                {
                    throw new InvalidOperationException("Operation " + _productFlags[product] + " failed on building " + product.Id);
                }
                _productFlags.Remove(product);
            }
        }

        public async Task<Boolean> LoginAsync(String userName, String userPassword)
        {
            IsUserLoggedIn = await _persistence.LoginAsync(userName, userPassword);
            return IsUserLoggedIn;
        }

        public async Task<Boolean> LogoutAsync()
        {
            if (!IsUserLoggedIn)
                return true;

            IsUserLoggedIn = !(await _persistence.LogoutAsync());

            return IsUserLoggedIn;
        }

    }
}
