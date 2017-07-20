using Emotions.Models;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emotions
{
    class AzureManager
    {
        private static AzureManager instance;
		private MobileServiceClient client;
		private IMobileServiceTable<EmotionsHistoryModel> emotionTable;

		private AzureManager()
		{
            
                this.client = new MobileServiceClient("http://emotions-soumik.azurewebsites.net");
                this.emotionTable = this.client.GetTable<EmotionsHistoryModel>();
            
		}

		public MobileServiceClient AzureClient
		{
			get { return client; }
		}

		public static AzureManager AzureManagerInstance
		{
			get
			{
				if (instance == null)
				{
					instance = new AzureManager();
				}

				return instance;
			}
		}

		public async Task<List<EmotionsHistoryModel>> GetEmotionInformation()
		{
			return await this.emotionTable.ToListAsync();
		}
    }
}
