using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SuperShop.Prism.Models;
using SuperShop.Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperShop.Prism.ViewModels
{
    public class ProductsPageViewModel : ViewModelBase
    {
        //Atributos
        private readonly IApiService _apiService;
        // Atributo para Produtos
        private List<ProductResponse> _products;

        //Construtor
        public ProductsPageViewModel(
            INavigationService navigationService,
            IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            Title = "Products Page";
            LoadProductsAsync();
        }

        //Propriedade para Produtos
        //Isto serve para pudermos usar a lista de Produtos (Temos entao que conjugar o Atributo + a Propriedade + o Método)
        public List<ProductResponse> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }


        //Método para criar os Produtos
        private async void LoadProductsAsync()
        {   //Esta URL está no ficheiro App.xaml
            string url = App.Current.Resources["UrlAPI"].ToString();

            //Ir buscar a API (aqui estamos a aceder ao ApiService.cs)
            Response response = await _apiService.GetListAsync<ProductResponse>(url, "/api", "/Products");

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            Products = (List<ProductResponse>)response.Result;
        }
    }
}
