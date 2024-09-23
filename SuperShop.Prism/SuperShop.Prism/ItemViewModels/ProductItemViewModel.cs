using Prism.Commands;
using Prism.Navigation;
using SuperShop.Prism.Models;
using SuperShop.Prism.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShop.Prism.ItemViewModels
{
    //Tudo o que tem haver com dados nao devemos misturar mais nada nem qualquer tipo de interaçao isso é o que
    //acontece no ProductResponse para implementarmos interaçao tivemos que criar esta classe ProductItemViewModel
    //Em suma a classe ProductResponse so tem o produto enquanto que a classe ProductItemViewModel alem de ter os
    //dados do produto vai ter a possiblidade de ser clicavel
    public class ProductItemViewModel : ProductResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectProductCommand;

        public ProductItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectProductCommand =>
            _selectProductCommand ??
            (_selectProductCommand = new DelegateCommand(SelectProductAsync));

        private async void SelectProductAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                {"product",this}
            };
            //Abre a página ProductDetailPage e passa os parametros product e o itemviewmodel
            //Quando chamarmos esta pagina ProductDetailPage ele vai receber os parametros em ProductDetailPageViewModel em OnNavigatedTo
            await _navigationService.NavigateAsync(nameof(ProductDetailPage), parameters);
        }
    }
    }
