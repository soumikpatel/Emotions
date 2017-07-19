using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emotions.Models
{
    public class EmotionsHistoryModel
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "anger")]
        public float anger { get; set; }

        [JsonProperty(PropertyName = "contempt")]
        public float contempt { get; set; }

        [JsonProperty(PropertyName = "disgust")]
        public float disgust { get; set; }

        [JsonProperty(PropertyName = "fear")]
        public float fear { get; set; }

        [JsonProperty(PropertyName = "happiness")]
        public float happiness { get; set; }

        [JsonProperty(PropertyName = "neutral")]
        public float neutral { get; set; }

        [JsonProperty(PropertyName = "sadness")]
        public float sadness { get; set; }

        [JsonProperty(PropertyName = "surprise")]
        public float surprise { get; set; }
    }
}
