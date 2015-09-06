using Media.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Xamarin.Forms;
using System.Net.Http.Headers;
using System.IO;
using Refractored.Xam.TTS;
using System.Diagnostics;
using XamFormsImageResize;

namespace Blindist
{
   public partial class ImageProcess
    {
       public string error { get; set; }
       public  byte[] imgraw { get; set; }
        public ImageSource img { get; set; }

            private readonly string url = "https://gateway.watsonplatform.net/visual-recognition-beta/api";
            //Create Bluemix account for Username/password
            private readonly string username = "474e89c2-53e4-477d-8570-fdb495288a09";
            private readonly string password = "QBxEanvs9YOQ";

            public async Task<string> ProcessImage(byte[] img)
            {
            CrossTextToSpeech.Current.Speak("Please Wait while we process your image!");
            try
                {

                    byte[] CompressedImage = ImageResizer.ResizeImage(img, 400, 400);
                    var multiPartContent = new MultipartFormDataContent();
                    var byteArrayContent = new ByteArrayContent(img);
                    byteArrayContent.Headers.Add("Content-Type", "image/jpeg");
                    multiPartContent.Add(byteArrayContent, "img_File", "temp.jpg");
                    using (var client = new HttpClient())
                    {
                        CrossTextToSpeech.Current.Speak("Please Wait while we process your image!");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GetBase64CredentialString());
                    
                    var response = await client.PostAsync("https://gateway.watsonplatform.net/visual-recognition-beta/api/v1/tag/recognize", multiPartContent);
                    CrossTextToSpeech.Current.Speak("Please Wait while we process your image!");
                    if (response.IsSuccessStatusCode == true)
                    {
                        return await HandleResponseAsync(response);
                    }
                    else
                    {
                        return null;
                    }
                    
                    
                  
                   
                    }
                }
                catch (Exception ex)
                {
                  return null;
                }
            }
            private async Task<string> HandleResponseAsync(HttpResponseMessage response)
            {
                string bb = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return bb;
            }
            private string GetBase64CredentialString()
            {

                var auth = string.Format("{0}:{1}", username,password);
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(auth));
            }

        public async Task<ImageProcess> GetImage()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
            {
                ImageProcess ob = new ImageProcess()
                {
                    error = "Oh No ! No Camera.. I can't access your camera",
                    imgraw = null,
                    img = null
                };
                return ob;
            }


            var file = await CrossMedia.Current.TakePhotoAsync(new Media.Plugin.Abstractions.StoreCameraMediaOptions
            {

                Directory = "Sample",
                Name = "test.jpg"
            });

            if (file == null)
                return null;


           var  imgrawtemp = new BinaryReader(file.GetStream()).ReadBytes((int)file.GetStream().Length);
            var image = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
            ImageProcess temp = new ImageProcess()
            {   imgraw = imgrawtemp, 
                error = "Success",
                img = image
            };
            return temp;
        }
    }
}
