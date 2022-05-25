using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using IncidentesAMT.Modelo;
using System.Net.Http;
using System.Net;
using System.IO;
using IncidentesAMT.VistaModelo;
using IncidentesAMT.Helpers;
using IncidentesAMT.Model;
using System.Collections.ObjectModel;

namespace IncidentesAMT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registro : ContentPage
    {
        private string _identificacion;
        private string _nombres;
        private string _apellidos;
        private string _correo;
        private string _archivo;
        private string _nacionalidad;
        private string _path;
        private string _celular;
        FotoViewModel fotoViewModel;
        string foto;
        public Registro(string identificacion, string nombres, string apellidos, string correo, string nacionalidad, string celular)
        {
            InitializeComponent();
            _identificacion = identificacion;
            _nombres = nombres;
            _apellidos = apellidos;
            _correo = correo;
            _nacionalidad = nacionalidad;
            _celular = celular;
            fotoViewModel = new FotoViewModel();
            BindingContext = fotoViewModel;


        }

        private async void btnRegistrar_Clicked(object sender, EventArgs e)
        {
            try
            {                
                PersonaModel persona = new PersonaModel
                {
                    identificacion = _identificacion,
                    nombres = _nombres,
                    apellidos = _apellidos,
                    nacionalidad = _nacionalidad,
                    correo = _correo,
                    password = txtConfirmarPassword.Text,
                    fotoCedula = ConvertImgBase64.ConvertImgToBase64(fotoViewModel.PathFoto),
                    celular = _celular
                };

                Uri RequestUri = new Uri("http://incidentes-amt.herokuapp.com/personas/createPersona");
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(persona);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    await DisplayAlert("Mensaje", "Registrado correctamente", "Ok");
                    await Navigation.PushAsync(new Login(), true);
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                    MessageModel obj = JsonConvert.DeserializeObject<MessageModel>(content);
                    var messageType = obj.message;

                    await DisplayAlert("Ocurrió un error", messageType[0].ToString(), "Cerrar");
                }
            }
            catch (Exception ex)
            {
               await DisplayAlert("Error",ex.Message, "Cerrar");
            }
           
        }        

        private void btnAddFoto_Clicked(object sender, EventArgs e)
        {
            takefoto();
        }
        private async void takefoto()
        {
            fotoViewModel = new FotoViewModel();
            await fotoViewModel.TomarFoto();
            foto = fotoViewModel.PathFoto;
            fotocedula.Source = foto;
        }
    }
}