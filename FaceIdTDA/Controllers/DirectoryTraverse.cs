﻿using FaceIdTDA.Entities;
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
        private readonly IDictionary<string, Image> imageDictionary;
        private readonly IDictionary<string, string> headerDictionary;

        private int faceid = 0;

        public DirectoryTraverse()
        {
            imageDictionary = new Dictionary<string, Image>();
            headerDictionary = new Dictionary<string, string>();
            headerDictionary.Add("Cache-Control", "no-cache");
            headerDictionary.Add("Accept", "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, */*");
        }

        public void Retrieve()
        {
            lock (imageDictionary)
            {
                DirectoryInfo ftpDirectory = new DirectoryInfo(Program.FTP_DIRECTORY);

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
                            imageDictionary.Remove(filename);
                            Console.WriteLine(string.Format("{0}: {1}", image.faceid, image.jpegFile.FullName));
                            FormUpload.MultipartFormPost(Program.REMOTE_SERVER, image, headerDictionary);
                        }
                    }
                    //File.Delete(nextFile.FullName);
                }

                imageDictionary.Clear();
            }
        }
    }
}
