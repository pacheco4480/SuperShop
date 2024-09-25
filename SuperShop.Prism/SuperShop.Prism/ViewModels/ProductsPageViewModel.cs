using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SuperShop.Prism.Helpers;
using SuperShop.Prism.ItemViewModels;
using SuperShop.Prism.Models;
using SuperShop.Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SuperShop.Prism.ViewModels
{
    public class ProductsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        //Atributos
        private readonly IApiService _apiService;
        // Atributo para Produtos
        //No mobile nao se costuma usar LIst na vez disso usamos ObservableCollection
        private ObservableCollection<ProductItemViewModel> _products;
        //Roda dentada que aperece quando está a fazer loading da pagina
        private bool _isRunning;
        //Barra de pesquisa
        private string _search;
        private List<ProductResponse> _myProducts;
        private DelegateCommand _searchCommand;

        //Construtor
        public ProductsPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.Products;
            LoadProductsAsync();
        }

        //Propriedade para Produtos
        //Isto serve para pudermos usar a lista de Produtos (Temos entao que conjugar o Atributo + a Propriedade + o Método)
        public ObservableCollection<ProductItemViewModel> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowProducts));

        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowProducts();
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }



        //Método para criar os Produtos
        private async void LoadProductsAsync()
        {   
            //Se nao tiver ligação à internet
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {   //Mostra o Alert
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                });
                return; 
            }

            IsRunning = true;

            //Esta URL está no ficheiro App.xaml
            string url = App.Current.Resources["UrlAPI"].ToString();

            //Ir buscar a API (aqui estamos a aceder ao ApiService.cs)
            Response response = await _apiService.GetListAsync<ProductResponse>(url, "/api", "/Products");

            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _myProducts = (List<ProductResponse>)response.Result;
            ShowProducts();
        }

        private void ShowProducts()
        {
            if (string.IsNullOrEmpty(Search))
            {   //Aqui estamos a passar o valor dos produtos _myProducts e a inserir estes no ViewModel ProductItemViewModel
                //ProductItemViewModel aqui ja temos o comando de seleção
                Products = new ObservableCollection<ProductItemViewModel>(_myProducts.Select(p =>
                new ProductItemViewModel(_navigationService)
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    LastPurchase = p.LastPurchase,
                    LastSale = p.LastSale,
                    IsAvailable = p.IsAvailable,
                    Stock = p.Stock,
                    User = p.User,
                    ImageFullPath = p.ImageFullPath
                }).ToList());
            }
            else
            {
                Products = new ObservableCollection<ProductItemViewModel>(
                    _myProducts.Select(p =>
                    new ProductItemViewModel(_navigationService)
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        LastPurchase = p.LastPurchase,
                        LastSale = p.LastSale,
                        IsAvailable = p.IsAvailable,
                        Stock = p.Stock,
                        User = p.User,
                        ImageFullPath = p.ImageFullPath
                    })
                    .Where(p => p.Name.ToLower().Contains(Search.ToLower()))
                    .ToList());
            }
        }
    }
}
