using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace IncidentesAMT.Helpers
{
    public static class ConvertImgBase64
    {
        //public ConvertImgBase64()
        //{

        //}
        public static string ConvertImgToBase64(string path)
        {
            byte[] ImageData = File.ReadAllBytes(path);
            string base64String = Convert.ToBase64String(ImageData);
            return base64String;
        }

        public static ImageSource GetImageSourceFromBase64String(string base64)
        {
            if (base64 == null)
            {
                return null;
            }
            string result = Regex.Replace(base64, @"^data:image\/[a-zA-Z]+;base64,", string.Empty);

            byte[] Base64Stream = Convert.FromBase64String(result);
            return ImageSource.FromStream(() => new MemoryStream(Base64Stream));
        }
    }
}
