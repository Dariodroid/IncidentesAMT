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
        ObservableCollection<IncidenteByPersonaModel> incidenteByUsuarioModel;
        IncidenteByUsuarioViewModel IncidenteByUsuarioViewModel;
        public IncidenteByUsuario(string idUser)
        {
            _idUser = idUser;
            InitializeComponent();
            getIncidntes();

        }
        private async void getIncidntes()
        {
            IncidenteByUsuarioViewModel = new IncidenteByUsuarioViewModel();
            var ls = await IncidenteByUsuarioViewModel.GetIncidentePersonaById(_idUser);
            cwIncidentes.ItemsSource = ls;
        }

        public async void GetIncidentePersonaById()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://192.168.16.33:3000/incidentes/findByIdPersona/" + _idUser);
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accpet", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                incidenteByUsuarioModel = JsonConvert.DeserializeObject<ObservableCollection<IncidenteByPersonaModel>>(content);
                cwIncidentes.ItemsSource = incidenteByUsuarioModel;
            }

        }
    }
}