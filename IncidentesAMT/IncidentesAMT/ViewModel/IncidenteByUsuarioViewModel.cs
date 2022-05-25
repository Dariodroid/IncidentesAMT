using IncidentesAMT.Modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IncidentesAMT.ViewModel
{
    public class IncidenteByUsuarioViewModel
    {
        ObservableCollection<IncidenteByPersonaModel> incidenteByUsuarioModel;
        public IncidenteByUsuarioViewModel()
        {

        }

        public async Task<ObservableCollection<IncidenteByPersonaModel>> GetIncidentePersonaById(string idUser)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://incidentes-amt.herokuapp.com/incidentes/findByIdPersona/" + idUser);
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accpet", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                incidenteByUsuarioModel = JsonConvert.DeserializeObject<ObservableCollection<IncidenteByPersonaModel>>(content);
                
            }
            return incidenteByUsuarioModel;

        }
    }
}
