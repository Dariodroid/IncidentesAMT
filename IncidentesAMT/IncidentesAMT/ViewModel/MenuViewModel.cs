using IncidentesAMT.Model;
using IncidentesAMT.Modelo;
using IncidentesAMT.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace IncidentesAMT.VistaModelo
{
    public class MenuViewModel : BaseViewModel
    {
        //public ICommand GetIncidentePersonaCommand { get; set; }
        public MenuViewModel(string idUser)
        {
            GetIncidentePersonaById(idUser);
            FrmFalsos = false;
            FrmActivos = false;
        }
        #region VARIABLES
            private ObservableCollection<IncidenteByPersonaModel> _incidenteByPersonaModel;
            string _lblIncActivos;
            string _lblfalso;
            bool _frmActivos;
            bool _frmFalsos;
        #endregion
        #region OBJETOS
        public bool FrmFalsos
        {
            get { return _frmFalsos; }
            set { _frmFalsos = value; OnPropertyChanged(); }
        }
        public bool FrmActivos
        {
            get { return _frmActivos; }
            set { _frmActivos = value; OnPropertyChanged(); }
        }
        public string Lblfalso
        {
            get { return _lblfalso; }
            set { _lblfalso = value; OnPropertyChanged(); }
        }
        public string LblIncActivos
        {
            get { return _lblIncActivos; }
            set { _lblIncActivos = value; OnPropertyChanged(); }
        }
        public ObservableCollection<IncidenteByPersonaModel> IncidenteByPersonaModel
        {
            get { return _incidenteByPersonaModel; }
            set { _incidenteByPersonaModel = value; OnPropertyChanged(); }
        }

        

        #endregion



        public async void GetIncidentePersonaById(string _idUser)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://incidentes-amt.herokuapp.com/incidentes/findByIdPersona/" + _idUser);
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accpet", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                IncidenteByPersonaModel = JsonConvert.DeserializeObject<ObservableCollection<IncidenteByPersonaModel>>(content);
                VerificarNotif();
            }

        }
        private void VerificarNotif()
        {
            int contgen = 0;
            int contfal = 0;
            if (IncidenteByPersonaModel != null)
            {
                for (int i = 0; i < IncidenteByPersonaModel.Count; i++)
                {
                    if (IncidenteByPersonaModel[i].estado.ToString() == "GEN")
                    {
                        FrmActivos = true;
                        contgen += 1;
                        LblIncActivos = $"{contgen} Incidente{(contgen > 1 ? "s" : "")} activo{(contgen > 1 ? "s" : "")}";
                    }

                    if (IncidenteByPersonaModel[i].estado.ToString() == "FAL")
                    {
                        FrmFalsos = true;
                        contfal += 1;
                        Lblfalso = $"{contfal} Incidente{(contfal > 1 ? "s" : "")} falso{(contfal > 1 ? "s" : "")}";
                    }

                }
            }
        }
    }
}
