using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Play3_Client_Side.API
{
    public class DTO
    {
        [Serializable]
        public class PlayerDTO
        {
            [JsonProperty(PropertyName = "Uuid")] 
            public string Uuid { get; set; }
            [JsonProperty(PropertyName = "XCoord")]
            public int XCoord { get; set; }
            [JsonProperty(PropertyName = "YCoord")]
            public int YCoord { get; set; }
            [JsonProperty(PropertyName = "Name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "Health")]
            public float Health { get; set; }
        }
    }
}
