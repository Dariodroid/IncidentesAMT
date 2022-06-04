using Acr.UserDialogs;
using IncidentesAMT.Helpers;
using IncidentesAMT.Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IncidentesAMT.ViewModel
{
    public class IncidenteByUsuarioViewModel : BaseViewModel
    {
        #region VARIABLES

        private ObservableCollection<IncidenteByPersonaModel> _incidenteByUsuarioModel;

        #endregion

        #region PROPIEDADES
        public ObservableCollection<IncidenteByPersonaModel> IncidenteByUsuarioModel
        {
            get { return _incidenteByUsuarioModel; }
            set { _incidenteByUsuarioModel = value; OnPropertyChanged(); }
        }
        #endregion

        public IncidenteByUsuarioViewModel(string idUser)
        {
            GetIncidentePersonaById(idUser);
        }
        public async void GetIncidentePersonaById(string idUser)
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Cargando...");
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://incidentes-amt.herokuapp.com/incidentes/findByIdPersona/" + idUser);
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accpet", "application/json");
                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    IncidenteByUsuarioModel = JsonConvert.DeserializeObject<ObservableCollection<IncidenteByPersonaModel>>(content);
                }
                    UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", ex.Message, "Cerrar");
            }
        }
    }
}
