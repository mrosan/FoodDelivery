using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FoodDeliveryData;

namespace FoodDeliveryAdmin.Persistence
{
    public class FoodDeliveryServicePersistence : IFoodDeliveryPersistence
    {
        private HttpClient _client;

        public FoodDeliveryServicePersistence(String baseAddress)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseAddress);
        }
        
        public async Task<IEnumerable<OrderDTO>> ReadOrdersAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/orders/");
                if (response.IsSuccessStatusCode)
                {
                    //for this to work: PM> install-package Microsoft.AspNet.WebApi.Client
                    IEnumerable<OrderDTO> orders = await response.Content.ReadAsAsync<IEnumerable<OrderDTO>>();
                    return orders;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }

        }

        //get every product
        public async Task<IEnumerable<ProductDTO>> ReadProductListAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/products/");
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<ProductDTO> products = await response.Content.ReadAsAsync<IEnumerable<ProductDTO>>();
                    return products;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }

        }

        //get products belonging to an orger group
        public async Task<IEnumerable<ProductDTO>> ReadProductsAsync(Int32 GroupID)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/products/" + GroupID);
                if (response.IsSuccessStatusCode)
                {
                    IEnumerable<ProductDTO> products = await response.Content.ReadAsAsync<IEnumerable<ProductDTO>>();
                    return products;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }

        }

        public async Task<Boolean> UpdateOrderAsync(OrderDTO order)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/orders/", order);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<Boolean> CreateProductAsync(ProductDTO product)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/products/", product);
                product.Id = (await response.Content.ReadAsAsync<ProductDTO>()).Id;
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<Boolean> DeleteProductAsync(ProductDTO product)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("api/products/" + product.Id);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<Boolean> LoginAsync(String userName, String userPassword)
        {
            try
            {
                Console.WriteLine("API-val kapcsolat teremtés . . .");
                HttpResponseMessage response = await _client.GetAsync("api/account/login/" + userName + "/" + userPassword);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Nem sikerült az API-val kapcsolatot teremteni. :(");
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<Boolean> LogoutAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/account/logout");
                return !response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
    }
}
