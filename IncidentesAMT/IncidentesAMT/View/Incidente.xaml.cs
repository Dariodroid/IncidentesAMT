using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IncidentesAMT.Helpers;
using Xamarin.Forms.GoogleMaps;
using IncidentesAMT.Modelo;
using IncidentesAMT.VistaModelo;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using IncidentesAMT.ViewModel;
using Xamarin.Essentials;

namespace IncidentesAMT.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Incidente : ContentPage
    {
        string _idPersona;
        string _idIncidente;
        IncidenteViewModel incidenteViewModel;
        public Incidente(string idPersona, string idIncidente)
        {
            _idIncidente = idIncidente; 
            _idPersona = idPersona;
            InitializeComponent();
            incidenteViewModel = new IncidenteViewModel(Navigation, _idPersona, _idIncidente, MapView);
            BindingContext = incidenteViewModel;
        }

        private void SlideToActView_SlideCompleted(object sender, EventArgs e)
        {
            incidenteViewModel.Incidente();
        }
       
    }
}