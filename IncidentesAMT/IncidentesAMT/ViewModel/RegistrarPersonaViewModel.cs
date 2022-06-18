using IncidentesAMT.Helpers;
using IncidentesAMT.Modelo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IncidentesAMT.ViewModel
{
    public class RegistrarPersonaViewModel : BaseViewModel
    {
        #region PROPIEDADES
        INavigation Navigation { get; set; }
        public Command NextPageCommand { get; set; }

        private string identificacion;
        private string nombres;
        private string apellidos;
        private string correo;
        private string celular;
        public string Identificacion
        {
            get { return identificacion; }
            set
            {
                identificacion = value;
                OnPropertyChanged();
            }
        }
        public string Nombres
        {
            get { return nombres; }
            set
            {
                nombres = value;
                OnPropertyChanged();
            }
        }
        public string Apellidos
        {
            get { return apellidos; }
            set
            {
                apellidos = value;
                OnPropertyChanged();
            }
        }
        public string Correo
        {
            get { return correo; }
            set
            {
                correo = value;
                OnPropertyChanged();
            }
        }
        public string Celular
        {
            get { return celular; }
            set
            {
                celular = value;
                OnPropertyChanged();
            }
        }
        #endregion

      

        public RegistrarPersonaViewModel(INavigation navigation)
        {
            Navigation = navigation;
            NextPageCommand = new Command(() => NextPage());
        }

        private async void NextPage()
        {
            if (await VerifyData() /*&& await VerifyCI()*/)
            {
                PersonaModel persona = new PersonaModel
                {
                    //identificacion = identificacion,
                    nombres = nombres,
                    apellidos = apellidos,
                    correo = correo,
                    celular = celular
                };
                await Navigation.PushAsync(new Registro(persona));
            }
        }

        private async Task<bool> VerifyData()
        {
            //if (string.IsNullOrEmpty(Identificacion))
            //{
            //    await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
            //    return false;
            //}
            if (string.IsNullOrEmpty(Nombres))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                return false;
            }
            if (string.IsNullOrEmpty(Apellidos))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                return false;
            }
            if (string.IsNullOrEmpty(Correo))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                return false;
            }
            if (string.IsNullOrEmpty(Celular))
            {
                await DisplayAlert("Error", "Debe llenar todos los campos", "Ok");
                return false;
            }
            return true;
        }

        //private async Task<bool> VerifyCI()
        //{
        //    if (!Verify_Ci.VerificaIdentificacion(Identificacion))
        //    {
        //        await DisplayAlert("Ocurrió un error", "La cédula es incorrecta", "Cerrar");
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}
    }
}
