using IncidentesAMT.Model;
using IncidentesAMT.Modelo;
using IncidentesAMT.Vista;
using IncidentesAMT.VistaModelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncidentesAMT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        private string _idUser;
        private List<CatalogoXIdModel> catalogoXIdModelo;

        public Menu( string idUser)
        {
            InitializeComponent(); 
            _idUser = idUser;

            BindingContext = new MenuViewModel(_idUser);
            GetCatalogoXId();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PerfilUsuario(_idUser));
        }
       
        private async void GetCatalogoXId()
        {
            try
            {
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://192.168.16.33:3000/catalogo/findIdPadre/0");
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accpet", "application/json");
                var client = new HttpClient();
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    catalogoXIdModelo = JsonConvert.DeserializeObject<List<CatalogoXIdModel>>(content);
                    cwIncidentes.ItemsSource = catalogoXIdModelo;

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "ok");
            }
        }

        private async void cwIncidentes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itemselectd = e.CurrentSelection[0] as CatalogoXIdModel;
            if(itemselectd != null)
            {
                await Navigation.PushAsync(new Incidente(_idUser, itemselectd._id));
            }
        }
    }
}