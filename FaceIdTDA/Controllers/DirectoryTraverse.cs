using FaceIdTDA.Entities;
using FaceIdTDA.Utils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FaceIdTDA.Controllers
{
    class DirectoryTraverse
    {
        private static readonly string FTP_DIRECTORY = @"D:\Cloud\FTPDir\";

        private int faceid = 0;

        private readonly IDictionary<string, Image> imageDictionary;

        public DirectoryTraverse()
        {
            imageDictionary = new Dictionary<string, Image>();
        }

        public void Retrieve()
        {
            lock (FTP_DIRECTORY)
            {
                DirectoryInfo ftpDirectory = new DirectoryInfo(FTP_DIRECTORY);

                foreach (FileInfo nextFile in ftpDirectory.GetFiles())
                {
                    string[] fileNameItems = nextFile.Name.Split(".");
                    if (fileNameItems.Length == 2)
                    {
                        string filename = fileNameItems[0];

                        Image image = null;
                        if (!imageDictionary.TryGetValue(filename, out image))
                        {
                            IServiceProvider serviceProvider = DIFaceIdTDA.GetServiceProvider();
                            image = serviceProvider.GetRequiredService<Image>();
                            imageDictionary[filename] = image;

                            string[] items = filename.Split("_");
                            if (items.Length == 6)
                            {
                                image.zptime = items[0];
                                image.puid = items[1];
                                image.faceid = faceid++;
                                image.remark = items[4] + "_" + items[5];
                                image.latitude = 29.466893;
                                image.longitude = 121.882763;
                            }
                        }

                        switch (nextFile.Extension.ToLower())
                        {
                            case (".jpg"):
                                image.jpgFile = nextFile;
                                break;
                            case (".jpeg"):
                                image.jpegFile = nextFile;
                                break;
                        }

                        if (image.jpegFile != null && image.jpgFile != null)
                        {
                            Console.WriteLine(image.jpegFile.FullName);
                        }
                    }
                    //File.Delete(nextFile.FullName);
                }

                imageDictionary.Clear();
            }
        }
    }
}
