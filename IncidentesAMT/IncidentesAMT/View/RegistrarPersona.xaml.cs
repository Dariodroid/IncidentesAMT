using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncidentesAMT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatosPersona : ContentPage
    {
        public DatosPersona()
        {
            InitializeComponent();
        }

        private async void btnSiguiente_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Registro(txtCedula.Text,txtNombres.Text,txtApellidos.Text,txtCorreo.Text, "62883ca68d0ce7cb7d438059"));
        }
    }
}