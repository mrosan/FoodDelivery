using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryService.Models
{
    public class Cart
    {
        //List containing the products and their quantities
        private List<CartItem> itemCollection = new List<CartItem>();

        //simpler version: if we dont need to bother with quantity
        //private List<Product> itemCollection = new List<Product>;

        //Adds a product to the cart.
        public void AddItem(Product product, int quantity)
        {
            //check whether the new Product is already in the collection (doesn't necessary for simpler version)
            //FirstOrDefault: either the item we are looking for, or null
            CartItem item = itemCollection.Where(x => x.Item.ProductID == product.ProductID).FirstOrDefault();
            if (item == null)
            {
                itemCollection.Add(new CartItem { Item = product, Quantity = quantity });
            } else
            {
                item.Quantity += quantity;
            }
        }

        //Removes all instances of a specific product.
        public void RemoveItems(Product product)
        {
            //removes all instances, technically
            //itemCollection.Remove(p => p.Item.ProductID == product.ProductID);

            CartItem cartItem = itemCollection.First(p => p.Item.ProductID == product.ProductID);
            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            } else
            {
                itemCollection.Remove(cartItem);
            }

        }

        //Returns how much all the products within the cart would cost
        public int CartValue()
        {
            return itemCollection.Sum(p => p.Item.Price * p.Quantity);
        }

        //Getter for the cart's items
        public IEnumerable<CartItem> getItems
        {
            get { return itemCollection; }
        }

        //Empties the whole cart
        public void ClearCart()
        {
            itemCollection.Clear();
        }
    }
}
