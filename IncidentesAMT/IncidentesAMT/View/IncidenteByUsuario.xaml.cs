using IncidentesAMT.Modelo;
using IncidentesAMT.ViewModel;
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

namespace IncidentesAMT.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IncidenteByUsuario : ContentPage
    {
        string _idUser;
        public IncidenteByUsuario(string idUser)
        {
            _idUser = idUser;
            InitializeComponent();
            BindingContext = new IncidenteByUsuarioViewModel(Navigation,_idUser);
        }
    }
}