using IncidentesAMT.Modelo;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using IncidentesAMT.Helpers;

namespace IncidentesAMT.VistaModelo
{
    public class FotoViewModel:FotoModel
    {
        public Command CapturarCommand { get; set; }

        public FotoViewModel()
        {
            CapturarCommand = new Command(TomarFoto);
        }

        private async void TomarFoto()
        {
            try
            {
                var camera = new StoreCameraMediaOptions();
                camera.PhotoSize = PhotoSize.Full;
                camera.SaveToAlbum = true;
                var foto = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(camera);
                if (foto != null)
                {
                    PathFoto = foto.AlbumPath;
                    Foto = ImageSource.FromStream(() =>
                    {
                        return foto.GetStream();
                    });
                }
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
            

        }
    }
}
