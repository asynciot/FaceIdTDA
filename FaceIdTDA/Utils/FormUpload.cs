using FaceIdTDA.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace FaceIdTDA.Utils
{
    public static class FormUpload
    {
        private static readonly Encoding encoding = Encoding.UTF8;

        public static string MultipartFormPost(string postUrl, Image image, IDictionary<string, string> headers)
        {
            string formDataBoundary = string.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(image, formDataBoundary);

            using (HttpWebResponse response = PostForm(postUrl, contentType, formData, headers))
            {
                StreamReader responseReader = new StreamReader(response.GetResponseStream());
                string responseText = responseReader.ReadToEnd();
                return responseText;
            }
        }

        private static HttpWebResponse PostForm(string postUrl, string contentType,
            byte[] formData, IDictionary<string, string> headers)
        {
            if (!(WebRequest.Create(postUrl) is HttpWebRequest request))
            {
                throw new NullReferenceException("request is not a http request");
            }

            // Set up the request properties.  
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = @"Mozilla / 4.0(compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 1.1.4322; CIBA; .NET CLR 2.0.50727)";
            request.Timeout = 5000;
            request.ContentLength = formData.Length;
            request.KeepAlive = false;
            //request.CookieContainer = new CookieContainer();

            // You could add authentication here as well if needed:  
            // request.PreAuthenticate = true;  
            // request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;  

            //Add header if needed
            if (headers != null)
            {
                foreach(string key in headers.Keys)
                {
                    request.Headers.Add(key, headers[key]);
                }
            }

            // Send the form data to the request.  
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            return request.GetResponse() as HttpWebResponse;
        }

        private static byte[] GetMultipartFormData(Image image, string boundary)
        {
            using (Stream formDataStream = new MemoryStream())
            {
                string header = string.Format("--{0}\r\nContent-Disposition: form-data;name=\"userfile\";pictype=0&isurl=0&piclen={1};filename=\"{2}\"\r\nContent-Type: image/pjpeg\r\n\r\n",
                        boundary, image.jpgFile.Length, image.jpgFile.Name);

                formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                var buffer = new byte[1024];
                int bytesRead = 0;
                using (FileStream fileStream = new FileStream(image.jpgFile.FullName, FileMode.Open))
                {
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        formDataStream.Write(buffer, 0, bytesRead);
                    }
                }

                formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                header = string.Format("--{0}\r\nContent-Disposition: form-data;name=\"userfile\";pictype=1&isurl=0&piclen={1};filename=\"{2}\"\r\nContent-Type: image/pjpeg\r\n\r\n",
                        boundary, image.jpegFile.Length, image.jpegFile.Name);

                formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                using (FileStream fileStream = new FileStream(image.jpegFile.FullName, FileMode.Open))
                {
                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        formDataStream.Write(buffer, 0, bytesRead);
                    }
                }

                // Add the end of the request.  Start with a newline  
                string footer = "\r\n--" + boundary + "--\r\n";
                formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

                // Dump the Stream into a byte[]
                formDataStream.Position = 0;
                byte[] formData = new byte[formDataStream.Length];
                formDataStream.Read(formData, 0, formData.Length);
                return formData;
            }
        }
    }
}