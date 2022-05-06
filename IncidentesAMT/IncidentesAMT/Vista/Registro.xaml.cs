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
        public Registro(string identificacion, string nombres, string apellidos, string correo, string nacionalidad)
        {
            InitializeComponent();
            _identificacion = identificacion;
            _nombres = nombres;
            _apellidos = apellidos;
            _correo = correo;
            _nacionalidad = nacionalidad;
            BindingContext = new FotoViewModel();
        }

        private async void btnRegistrar_Clicked(object sender, EventArgs e)
        {
            try
            {
                _path = ConvertImgToBase64(lblPath.Text);
                _archivo = _path;

                PersonaModel persona = new PersonaModel
                {

                    identificacion = _identificacion,
                    nombres = _nombres,
                    apellidos = _apellidos,
                    nacionalidad = _nacionalidad,
                    correo = _correo,
                    password = txtConfirmarPassword.Text,
                    fotoCedula = _archivo

                };

                Uri RequestUri = new Uri("http://192.168.16.35:3000/personas/createPersona");
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(persona);
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(RequestUri, contentJson);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    await DisplayAlert("Mensaje", "Registrado correctamente", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", response.StatusCode.ToString(), "Ok");
                }
            }
            catch (Exception ex)
            {
               await DisplayAlert("Error",ex.Message, "Cerrar");
            }
           
        }

        private string ConvertImgToBase64(string path)
        {
            byte[] ImageData = File.ReadAllBytes(path);
            string base64String = Convert.ToBase64String(ImageData);
            return base64String;
        }

        private void btnAddFoto_Clicked(object sender, EventArgs e)
        { 

        }
    }
}