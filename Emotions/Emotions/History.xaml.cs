using Emotions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Emotions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class History : ContentPage
    {
        public History()
        {
            InitializeComponent();
        }

        async void GetData(object sender, System.EventArgs e)
        {
            loadingCircle.IsVisible = true;

            List<EmotionsHistoryModel> emotionInformation = await AzureManager.AzureManagerInstance.GetEmotionInformation();
            EmotionsList.ItemsSource = emotionInformation;

            loadingCircle.IsVisible = false;
        }
    }
}