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
            loadingCircle.IsVisible = true;
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
                loadingCircle.IsVisible = false;

                if (x.Count != 0)
                {
                    foreach (var emotion in x)
                    {
                        var anger = Math.Round((x[x.IndexOf(emotion)].scores.anger * 100), 2);
                        var contempt = Math.Round((x[x.IndexOf(emotion)].scores.contempt * 100), 2);
                        var disgust = Math.Round((x[x.IndexOf(emotion)].scores.disgust * 100), 2);
                        var fear = Math.Round((x[x.IndexOf(emotion)].scores.fear * 100), 2);
                        var happiness = Math.Round((x[x.IndexOf(emotion)].scores.hapiness * 100), 2);
                        var neutral = Math.Round((x[x.IndexOf(emotion)].scores.neutral * 100), 2);
                        var sadness = Math.Round((x[x.IndexOf(emotion)].scores.sadness * 100), 2);
                        var surprise = Math.Round((x[x.IndexOf(emotion)].scores.surprise * 100), 2);

                        Heading.Text = "Detected Emotions";

                        TagLabel.Text = "Anger:  " + anger.ToString() + " %\n";
                        TagLabel.Text += "Contempt:  " + contempt.ToString() + " %\n";
                        TagLabel.Text += "Disgust:  " + disgust.ToString() + " %\n";
                        TagLabel.Text += "Fear:  " + fear.ToString() + " %\n";
                        TagLabel.Text += "Happiness:  " + happiness.ToString() + " %\n";
                        TagLabel.Text += "Neutral:  " + neutral.ToString() + " %\n";
                        TagLabel.Text += "Sadness:  " + sadness.ToString() + " %\n";
                        TagLabel.Text += "Surprise:  " + surprise.ToString() + " %\n";

                        EmotionsHistoryModel model = new EmotionsHistoryModel()
                        {
                            anger = (float)anger,
                            contempt = (float)contempt,
                            disgust = (float)disgust,
                            fear = (float)fear,
                            happiness = (float)happiness,
                            neutral = (float)neutral,
                            sadness = (float)sadness,
                            surprise = (float)surprise
                        };

                        await AzureManager.AzureManagerInstance.PostEmotions(model);
                    }


                }

                else
                {
                    Heading.Text = "\nNo Faces Detected!";
                    TagLabel.Text = "";
                }
                                
            }
            file.Dispose();
        }
    }
}