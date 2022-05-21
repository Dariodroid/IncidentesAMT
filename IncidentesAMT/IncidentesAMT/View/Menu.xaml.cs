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
        private ObservableCollection<IncidenteByPersonaModel> incidenteByPersonaModel;
        MenuViewModel MenuViewModel;
        public Menu( string idUser)
        {
            
            InitializeComponent(); 
            _idUser = idUser;
           //  MenuViewModel = new MenuViewModel(_idUser);

            BindingContext = new MenuViewModel(_idUser);//esta mal estructurado tu MVVM
            //GetIncidentePersonaById();
            //GetPersonaById();
            GetCatalogoXId();
            //verf();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PerfilUsuario());
        }

        private async void GetPersonaById()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://192.168.16.33:3000/personas/" + _idUser);
            request.Method = HttpMethod.Get;
            request.Headers.Add("Accpet", "application/json");
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                InfoUserModel infoUserModel = JsonConvert.DeserializeObject<InfoUserModel>(content);
            }
        }//metodo para obtener una persona x su id

        private void verf()
        {
            //incidenteByPersonaModel = MenuViewModel.GetIncidentePersonaById(_idUser);

            if (incidenteByPersonaModel != null)
            {
                for (int i = 0; i < incidenteByPersonaModel.Count; i++)
                {
                    if (incidenteByPersonaModel[i].estado.ToString() == "GEN")
                    {
                        DisplayAlert("", incidenteByPersonaModel[i].estado.ToString(), "");
                        frmActivos.IsVisible = true;
                        int cont = 0;
                        cont += 1;
                        lblIncActivos.Text = $"{cont} Incidente{(cont > 1 ? "s" : "")} activo{(cont > 1 ? "s" : "")}";
                        //return 1;
                    }

                    if (incidenteByPersonaModel[i].estado.ToString() == "FAL")
                    {
                        frmFalsos.IsVisible = true;
                        int cont = 0;
                        cont += 1;
                        lblIncActivos.Text = $"{cont} Incidente{(cont > 1 ? "s" : "")} falso{(cont > 1 ? "s" : "")}";
                        //return 1;
                    }

                }
                //return 0;
            }
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