using Acr.UserDialogs;
using IncidentesAMT.Helpers;
using IncidentesAMT.Model;
using IncidentesAMT.View;
using IncidentesAMT.VistaModelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace IncidentesAMT.ViewModel
{
    public class DatosPersonalesViewModel : BaseViewModel
    {
        #region VARIABLES
        string tel = null; string dir = null;
        private string _idUser;
        bool verifcado = false;
        private InfoUserByIdModel infoUserModel;
        FotoViewModel fotoViewModel = new FotoViewModel();
        #endregion

        #region COMANDOS
        public Command Fotocommand { get; set; }

        public Command FotoPerfilcommand { get; set; }

        public Command Updatecommand { get; set; }

        public Command UpdatePssword { get; set; }
        #endregion

        #region PROPIEDADES
        public INavigation Navigation { get; set; }


        private string _direccion;

        public string Direccion
        {
            get { return _direccion; }
            set
            {
                _direccion = value;
                OnPropertyChanged();
            }
        }
        private string _telefono;

        public string Telefono
        {
            get { return _telefono; }
            set
            {
                _telefono = value;
                OnPropertyChanged();
            }
        }
        private ImageSource _fotoPerfil;

        public ImageSource FotoPerfil
        {
            get { return _fotoPerfil; }
            set
            {
                _fotoPerfil = value;
                OnPropertyChanged();
            }
        }
        private ImageSource _fotoCedula;

        public ImageSource FotoCedula
        {
            get { return _fotoCedula; }
            set
            {
                _fotoCedula = value;
                OnPropertyChanged();
            }
        }

        private string _nombre;

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; OnPropertyChanged(); }
        }

        private string _apellido;

        public string Apellido
        {
            get { return _apellido; }
            set { _apellido = value; OnPropertyChanged(); }
        }

        private string _correo;

        public string Correo
        {
            get { return _correo; }
            set { _correo = value; OnPropertyChanged(); }
        }


        #endregion

        public DatosPersonalesViewModel(INavigation navigation, string idUser)
        {
            Navigation = navigation;
            _idUser = idUser;            
            Fotocommand = new Command(() => takefoto());
            FotoPerfilcommand = new Command(() => takeFotoPerfil());
            Updatecommand = new Command(() => UpdatePersona());
            UpdatePssword = new Command(() => UpdatePassword());
            GetPersonaById();
        }

        private async void GetPersonaById()
        {
            try
            {
                if(infoUserModel == null)
                {
                    UserDialogs.Instance.ShowLoading("Cargando sus datos...");
                    var request = new HttpRequestMessage();
                    request.RequestUri = new Uri("https://incidentes-amt.herokuapp.com/personas/" + _idUser);
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("Accpet", "application/json");
                    var client = new HttpClient();
                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        infoUserModel = JsonConvert.DeserializeObject<InfoUserByIdModel>(content);
                        Nombre = infoUserModel.nombres;
                        Apellido = infoUserModel.apellidos;
                        Telefono = infoUserModel.telefono;
                        Direccion = infoUserModel.direccion;
                        Correo = infoUserModel.correo;
                        FotoCedula = ConvertImgBase64.GetImageSourceFromBase64String(infoUserModel.fotoCedula);
                        FotoPerfil = ConvertImgBase64.GetImageSourceFromBase64String(infoUserModel.fotoPerfil);
                        UserDialogs.Instance.HideLoading();
                        tel = Telefono;
                        dir = Direccion;
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", ex.Message.ToString(), "Cerrar");
            }

        }

        private async void UpdatePersona()
        {
            try
            {               
                UserDialogs.Instance.ShowLoading("Actualizando...");
                if (!string.IsNullOrEmpty(fotoViewModel.PathFoto) )
                {
                    if (!verifcado) 
                    {
                        UserDialogs.Instance.HideLoading();
                        await DisplayAlert("Mensaje", "Para continuar con la actualiación es necesario verificar su cédula", "Ok"); 
                        return; 
                    }
                    Fotocedula fotoCedula = new Fotocedula
                    {
                        fotoCedula = ConvertImgBase64.ConvertImgToBase64(fotoViewModel.PathFoto)
                    };
                    Uri RequestUri = new Uri("https://incidentes-amt.herokuapp.com/personas/" + _idUser);
                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(fotoCedula);
                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(RequestUri, contentJson);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        await DisplayAlert("Mensaje", "Ocurrió un error al actualizar la foto de su cédula", "Ok");
                        return;
                    }
                    await DisplayAlert("Mensaje", "Actualizado correctamente", "Ok");
                }
                if (!string.IsNullOrEmpty(fotoViewModel.PathFotoPerfil))
                {
                    Fotoperfil fotoPerfil = new Fotoperfil
                    {
                       fotoPerfil = ConvertImgBase64.ConvertImgToBase64(fotoViewModel.PathFotoPerfil.ToString())
                    };
                    Uri RequestUri = new Uri("https://incidentes-amt.herokuapp.com/personas/" + _idUser);
                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(fotoPerfil);
                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(RequestUri, contentJson);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        await DisplayAlert("Mensaje", "Ocurrió un error al actualizar la foto de perfil", "Ok");
                        return;
                    }
                    await DisplayAlert("Mensaje", "Actualizado correctamente", "Ok");
                }
                if(tel != Telefono || dir != Direccion)
                {
                    UpdatePersonaModel persona2 = new UpdatePersonaModel
                    {
                        telefono = Telefono,
                        direccion = Direccion                    
                    };                    
                    Uri RequestUri = new Uri("https://incidentes-amt.herokuapp.com/personas/" + _idUser);
                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(persona2);
                    var contentJson = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PutAsync(RequestUri, contentJson);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        await DisplayAlert("Mensaje", "Ocurrió un error al actualizar sus datos de contacto", "Ok");
                        return;
                    }
                    await DisplayAlert("Mensaje", "Actualizado correctamente", "Ok");
                }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await DisplayAlert("Error", ex.Message, "Cerrar");
            }
        }

        private async void takefoto()
        {
            await ScannCI();
        }

        private async void takeFotoPerfil()
        {
            fotoViewModel = new FotoViewModel();
            await fotoViewModel.TomarFotoPerfil();
            if (fotoViewModel.Foto != null)
            {
                FotoPerfil = fotoViewModel.PathFotoPerfil;
            }
        }

        public async Task ScannCI()
        {
            var options = new MobileBarcodeScanningOptions();

            var overlay = new ZXingDefaultOverlay
            {
                ShowFlashButton = true,
                TopText = "Coloca el código de barra de tu cédula frente al dispositivo",
                BottomText = "El escaneo es automático, luego de verificar debe añadir una foto de la cédula",
                Opacity = 0.90,
            };

            overlay.BindingContext = overlay;
            var page = new ZXingScannerPage(options, overlay)
            {
                Title = "Escaneo Cédula",
                DefaultOverlayShowFlashButton = true,
            };

            await Navigation.PushModalAsync(page);

            page.OnScanResult += (result) =>
            {
                page.IsScanning = false;
                page.IsAnalyzing = false;
                verifcado = Verify_Ci.VerificaIdentificacion(result.Text);
                if (verifcado)
                {  
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        Vibration.Vibrate();
                        var time = TimeSpan.FromMilliseconds(100);
                        Vibration.Vibrate(time);
                        await Navigation.PopModalAsync();
                        await DisplayAlert("Mensaje", "Cédula verificada correctamente", "Ok");

                        await fotoViewModel.TomarFoto();
                        if (!string.IsNullOrEmpty(fotoViewModel.PathFoto))
                        {
                            FotoCedula = fotoViewModel.PathFoto;
                        }
                    });
                }
                else
                {
                    overlay.TopText = "El documento es incorrecto".ToUpper();
                    overlay.BottomText = "Verifique la iluminación y distancia del documento";
                    overlay.BindingContext = overlay;
                } 
            };
        }

        private async void UpdatePassword()
        {            
            InfoUserByIdModel cambiarPasswordModel = new InfoUserByIdModel
            {
                telefono = Telefono,
                correo = Correo,
                _id = _idUser,
                nombres = Nombre,
                apellidos = Apellido,
            };
            await Navigation.PushAsync(new CambiarPassword(cambiarPasswordModel, FotoPerfil));
        }

    }
}
