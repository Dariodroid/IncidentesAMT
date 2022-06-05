using IncidentesAMT.Helpers;
using IncidentesAMT.Modelo;
using IncidentesAMT.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace IncidentesAMT.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Detalle_incidente : ContentPage
    {
        public Detalle_incidente(IncidenteByPersonaModel incidenteByPersonaModel)
        {
            InitializeComponent();
            BindingContext = new Detalle_incidenteViewModel(Navigation,incidenteByPersonaModel, MapView).Incidente;
        }
    }
}