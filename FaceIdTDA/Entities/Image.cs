using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FaceIdTDA.Entities
{
    public class Image
    {
        public string puid { get; set; }

        public string zptime { get; set; }

        public int faceid { get; set; }

        public double longitude { get; set; }

        public double latitude { get; set; }

        public string remark { get; set; }

        public FileInfo jpgFile { get; set; }

        public FileInfo jpegFile { get; set; }
    }
}
