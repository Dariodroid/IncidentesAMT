using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using IncidentesAMT.Modelo;
using IncidentesAMT.ViewModel;

namespace IncidentesAMT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        LoginViewModel loginViewModel;
        public Login()
        {
            InitializeComponent();
            BindingContext  = new LoginViewModel(Navigation);
        }
      
        protected override void OnAppearing()
        {
            loginViewModel = new LoginViewModel(Navigation);
            loginViewModel.Navegar();
        }
    }
}