using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IncidentesAMT.Helpers
{
    public static class ConvertImgBase64
    {
        public static string ConvertImgToBase64(string path)
        {
            byte[] ImageData = File.ReadAllBytes(path);
            string base64String = Convert.ToBase64String(ImageData);
            return base64String;
        }
    }
}
