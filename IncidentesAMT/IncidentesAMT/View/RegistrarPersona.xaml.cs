using IncidentesAMT.Helpers;
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
            try
            {
                //if (await VerifyData())
                //{
                //    if (!Verify_Ci.VerificaIdentificacion(txtCedula.Text))
                //    {
                //        await DisplayAlert("Ocurrió un error", "La cédula es incorrecta", "Cerrar");
                //        txtCedula.Text = string.Empty;
                //        txtCedula.Focus();
                //        return;
                //    }
                    await Navigation.PushAsync(new Registro(txtCedula.Text,txtNombres.Text,txtApellidos.Text,txtCorreo.Text.Trim(), "62883ca68d0ce7cb7d438059",txtCelular.Text));
                //}
            }
            catch (Exception)
            {
               
            }
        }

        private async Task<bool> VerifyData()
        {
            if (string.IsNullOrEmpty(txtCedula.Text))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                txtCedula.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtNombres.Text))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                txtNombres.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtApellidos.Text))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                txtApellidos.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtCorreo.Text))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                txtCorreo.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(txtCelular.Text))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                txtCelular.Focus();
                return false;
            }
            return true;
        }
    }
}