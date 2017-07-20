using Emotions.Models;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Emotions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private async void LoadCamera(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Medium,
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            image.Source = ImageSource.FromStream(() =>
            {
                return file.GetStream();
            });

            //TagLabel.Text = "";

            await MakeRequest(file);

        }

        static byte[] GetImageAsByteArray(MediaFile file)
        {
            //FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            //BinaryReader binaryReader = new BinaryReader(fileStream);
            //return binaryReader.ReadBytes((int)fileStream.Length);

            var stream = file.GetStream();
            BinaryReader binaryReader = new BinaryReader(stream);
            return binaryReader.ReadBytes((int)stream.Length);
        }

        async Task MakeRequest(MediaFile file)
        {
            var client = new HttpClient();

            // Request headers - replace this example key with your valid key.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "0392263713f6403ea120be0bf26e5064");

            // NOTE: You must use the same region in your REST call as you used to obtain your subscription keys.
            //   For example, if you obtained your subscription keys from westcentralus, replace "westus" in the 
            //   URI below with "westcentralus".
            string uri = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?";
            HttpResponseMessage response;

            // Request body. Try this sample with a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(file);

            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response = await client.PostAsync(uri, content);
                var responseContent = response.Content.ReadAsStringAsync().Result;
                var x = JsonConvert.DeserializeObject<IList<EmotionModel>>(responseContent);

                foreach (var emotion in x)
                {
                    TagLabel.Text = "Anger: " + x[x.IndexOf(emotion)].scores.anger.ToString() + "\n";
                    TagLabel.Text += "Contempt: " + x[x.IndexOf(emotion)].scores.contempt.ToString() + "\n";
                    TagLabel.Text += "Disgust: " + x[x.IndexOf(emotion)].scores.disgust.ToString() + "\n";
                    TagLabel.Text += "Fear: " + x[x.IndexOf(emotion)].scores.fear.ToString() + "\n";
                    TagLabel.Text += "Hapiness: " + x[x.IndexOf(emotion)].scores.hapiness.ToString() + "\n";
                    TagLabel.Text += "Neutral: " + x[x.IndexOf(emotion)].scores.neutral.ToString() + "\n";
                    TagLabel.Text += "Sadness: " + x[x.IndexOf(emotion)].scores.sadness.ToString() + "\n";
                    TagLabel.Text += "Surprise: " + x[x.IndexOf(emotion)].scores.surprise.ToString() + "\n";
                }                
            }
            file.Dispose();
        }
    }
}