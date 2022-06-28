using IncidentesAMT.Modelo;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using IncidentesAMT.Helpers;
using IncidentesAMT.ViewModel;
using System.Threading.Tasks;

namespace IncidentesAMT.VistaModelo
{
    public class FotoViewModel:BaseViewModel
    {
        int cont = 0;
        public Command CapturarCommand { get; set; }
        public Command FotoPerfilCommand { get; set; }

        #region PROPIEDADES


        private ImageSource _foto;

        public ImageSource Foto
        {
            get { return _foto; }
            set { _foto = value; OnPropertyChanged(); }
        }

        private string _pathFotoPerfil;

        public string PathFotoPerfil
        {
            get { return _pathFotoPerfil; }
            set { _pathFotoPerfil = value; OnPropertyChanged(); }
        }

        private string _pathFoto;

        public string PathFoto
        {
            get { return _pathFoto; }
            set { _pathFoto = value; OnPropertyChanged(); }
        }

        private string _pathFoto2;

        public string PathFoto2
        {
            get { return _pathFoto2; }
            set { _pathFoto2 = value; OnPropertyChanged(); }
        }

        private string _txtBtnFotoRepInci;

        public string TxtBtnFotoRepInci
        {
            get { return _txtBtnFotoRepInci; }
            set { _txtBtnFotoRepInci = value; OnPropertyChanged(); }
        }
        #endregion

        public FotoViewModel()
        {
            CapturarCommand = new Command(async () => await TomarFoto());

            FotoPerfilCommand = new Command(async () => await TomarFotoPerfil());
        }

        public async Task TomarFoto()
        {
            try
            {                
                var camera = new StoreCameraMediaOptions();
                camera.PhotoSize = PhotoSize.Medium;
                camera.SaveToAlbum = true;                
                var foto = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(camera);
                if (foto != null)
                {
                    cont += 1;
                    if(cont == 1)
                    {
                        PathFoto = foto.Path;
                    }
                    else
                    {
                        PathFoto2 = foto.Path;
                        cont = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "Cerrar");
            }
        }

        public async Task TomarFotoPerfil()
        {
            try
            {
                var camera = new StoreCameraMediaOptions();
                camera.PhotoSize = PhotoSize.Medium;
                camera.DefaultCamera = CameraDevice.Front;
                camera.SaveToAlbum = true;
                var foto = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(camera);
                if (foto != null)
                {
                    PathFotoPerfil = foto.Path;
                    Foto = ImageSource.FromStream(() =>
                    {
                        return foto.GetStream();
                    });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message.ToString(), "Cerrar");
            }
        }

    }
}
