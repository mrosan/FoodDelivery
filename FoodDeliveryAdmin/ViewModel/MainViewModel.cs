using FoodDeliveryAdmin.Model;
using FoodDeliveryAdmin.Persistence;
using FoodDeliveryData;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.ObjectModel;

namespace FoodDeliveryAdmin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IFoodDeliveryModel _model;
        private ObservableCollection<OrderDTO> _orders;
        private ObservableCollection<ProductDTO> _products;
        private ObservableCollection<ProductDTO> _selectedProducts;
        private OrderDTO _currentOrder;
        private Boolean _isLoaded;
        private Boolean _isPListLoaded;
        private Int32 _selectedIndex;

        public ProductDTO EditedProduct { get; private set; }
        public String NameFilter { get; set; }
        public String AddressFilter { get; set; }
        public bool CompletedFilter { get; set; }
        public bool NotCompletedFilter { get; set; }

        public ObservableCollection<OrderDTO> Orders
        {
            get { return _orders; }
            private set
            {
                if (_orders != value)
                {
                    _orders = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ProductDTO> Products
        {
            get { return _products; }
            private set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ProductDTO> SelectedProducts
        {
            get { return _selectedProducts; }
            private set
            {
                if (_selectedProducts != value)
                {
                    _selectedProducts = value;
                    OnPropertyChanged();
                }
            }
        }

        public OrderDTO CurrentOrder
        {
            get { return _currentOrder; }
            private set
            {
                if (_currentOrder != value)
                {
                    _currentOrder = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean IsLoaded
        {
            get { return _isLoaded; }
            private set
            {
                if (_isLoaded != value)
                {
                    _isLoaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean IsPListLoaded
        {
            get { return _isPListLoaded; }
            private set
            {
                if (_isPListLoaded != value)
                {
                    _isPListLoaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;
                    OnPropertyChanged();

                    //for the new selected index, create a tmp Order and the list of Products belonging to it
                    if (_selectedIndex >= 0 && _selectedIndex < _orders.Count)
                    {
                        CurrentOrder = new OrderDTO
                        {
                            Completed = _orders[_selectedIndex].Completed,
                            Name = _orders[_selectedIndex].Name,
                            Address = _orders[_selectedIndex].Address,
                            TelephoneNr = _orders[_selectedIndex].TelephoneNr,
                            DateSubmitted = _orders[_selectedIndex].DateSubmitted,
                            DateCompleted = _orders[_selectedIndex].DateCompleted,
                            OrderGroup = _orders[_selectedIndex].OrderGroup,
                            Sum = _orders[_selectedIndex].Sum
                        };

                        LoadSelectedProducts(_orders[_selectedIndex].OrderGroup);
                    }
                        
                }
            }
        }

        //Product management
        public DelegateCommand ManageProductsCommand { get; private set; }
        public DelegateCommand CreateProductCommand { get; private set; }
        public DelegateCommand RefreshProductsCommand { get; private set; }
        public DelegateCommand CloseEditorCommand { get; private set; }

        //Order management
        public DelegateCommand LoadCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand UpdateOrderCommand { get; private set; }
        public DelegateCommand FilterOrdersCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }

        //Events
        public event EventHandler ExitApplication;
        public event EventHandler ProductEditingStarted;
        public event EventHandler ProductEditingFinished;


        public MainViewModel(IFoodDeliveryModel model)
        {
            _model = model;
            _isLoaded = false;
            _isPListLoaded = false;
            _selectedIndex = -1;

            ManageProductsCommand = new DelegateCommand(param =>
            {
                EditedProduct = new ProductDTO();
                OnProductEditingStarted();
            });

            UpdateOrderCommand = new DelegateCommand(param =>
            {
                Int32 index = SelectedIndex;
                if (index != -1)
                {
                    Orders.RemoveAt(index);
                    _model.UpdateOrder(CurrentOrder);
                    Orders.Insert(index, _model.Orders[index]);
                }
            });

            CreateProductCommand = new DelegateCommand(param => CreateProduct());
            CloseEditorCommand = new DelegateCommand(param => CloseEditor());
            LoadCommand = new DelegateCommand(param => LoadAsync());
            RefreshProductsCommand = new DelegateCommand(param => RefreshProducts());
            FilterOrdersCommand = new DelegateCommand(param => ApplyFilters());
            SaveCommand = new DelegateCommand(param => SaveAsync());
            ExitCommand = new DelegateCommand(param => OnExitApplication());

            NameFilter = null;
            AddressFilter = null;
            CompletedFilter = true;
            NotCompletedFilter = true;
        }

        //Loads both the orders and the products
        private async void LoadAsync()
        {
            try
            {
                await _model.LoadAsync();
                Orders = new ObservableCollection<OrderDTO>(_model.Orders);
                Products = new ObservableCollection<ProductDTO>(_model.Products);
                SelectedIndex = -1;
                IsLoaded = true;
                IsPListLoaded = true;
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("Error occurred while loading the data! No connection to the service.");
            }
        }

        //Refreshes the list of products from the model
        private void RefreshProducts()
        {
            try
            {
                Products = new ObservableCollection<ProductDTO>(_model.Products);
                IsPListLoaded = true;
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("Error occurred while loading the products! No connection to the service.");
            }
        }

        private void ApplyFilters()
        {
            //take order list from the model, in case it was already filtered previously
            List<OrderDTO> filteredOrders = _model.Orders.ToList();

            //apply filters
            if (!CompletedFilter)
            {
                filteredOrders = filteredOrders.Where(x => !x.Completed).ToList();
            }
            if (!NotCompletedFilter)
            {
                filteredOrders = filteredOrders.Where(x => x.Completed).ToList();
            }
            if (NameFilter != null)
            {
                filteredOrders = filteredOrders.Where(x => x.Name.Contains(NameFilter)).ToList();
            }
            if (AddressFilter != null)
            {
                filteredOrders = filteredOrders.Where(x => x.Address.Contains(AddressFilter)).ToList();
            }

            //set order list
            Orders = new ObservableCollection<OrderDTO>(filteredOrders);
        }

        //Loads the products for a selected Order
        private async void LoadSelectedProducts(int OrderGroup)
        {
            try
            {
                await _model.AssembleProductListAsync(OrderGroup);
                SelectedProducts = new ObservableCollection<ProductDTO>(_model.SelectedProducts);
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("Error occurred while loading the data! No connection to the service.");
            }
        }

        private async void SaveAsync()
        {
            try
            {
                await _model.SaveAsync();
                OnMessageApplication("Saving was successful!");
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("Saving was unsuccessful! No connection to the service.");
            }
        }

        private void CreateProduct()
        {
            if (String.IsNullOrEmpty(EditedProduct.Name))
            {
                OnMessageApplication("No name!");
                return;
            }
            foreach(ProductDTO p in Products)
            {
                if (p.Name == EditedProduct.Name)
                {
                    OnMessageApplication("This name already exists!");
                    return;
                }
            }
            if (EditedProduct.Price == 0)
            {
                OnMessageApplication("No price!");
                return;
            }
            if(EditedProduct.CategoryId < 1 || EditedProduct.CategoryId > 7)
            {
                OnMessageApplication("Incorrect category!");
                return;
            }
            

            // save only if the product is new (_model.Createproduct will set the Id)
            if (EditedProduct.Id == 0)
            {
                _model.CreateProduct(EditedProduct);
                Products.Add(EditedProduct);
                OnMessageApplication("Product added!");
            }

            EditedProduct = null;
        }

        private void CloseEditor()
        {
            EditedProduct = null;
            OnProductEditingFinished();
        }

        private void OnProductEditingStarted()
        {
            if (ProductEditingStarted != null)
                ProductEditingStarted(this, EventArgs.Empty);
        }

        private void OnProductEditingFinished()
        {
            if (ProductEditingFinished != null)
                ProductEditingFinished(this, EventArgs.Empty);
        }

        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }
    }
}
