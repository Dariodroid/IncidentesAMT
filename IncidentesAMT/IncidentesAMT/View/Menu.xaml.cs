using IncidentesAMT.Model;
using IncidentesAMT.Modelo;
using IncidentesAMT.Vista;
using IncidentesAMT.VistaModelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncidentesAMT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        private string _idUser;

        public Menu( string idUser)
        {
            InitializeComponent(); 
            _idUser = idUser;
            BindingContext = new MenuViewModel(Navigation, _idUser);
        }
    }
}