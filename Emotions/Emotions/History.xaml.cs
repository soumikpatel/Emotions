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

        async void Handle_ClickedAsync(object sender, System.EventArgs e)
        {
            List<NotHotDogModel> notHotDogInformation = await AzureManager.AzureManagerInstance.GetHotDogInformation();

            HotDogList.ItemsSource = notHotDogInformation;
        }
    }
}