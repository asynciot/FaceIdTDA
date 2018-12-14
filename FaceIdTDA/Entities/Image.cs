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

        public string longitude { get; set; }

        public string latitude { get; set; }

        public string remark { get; set; }

        public string jppFile { get; set; }

        public string jpegFile { get; set; }
    }
}
