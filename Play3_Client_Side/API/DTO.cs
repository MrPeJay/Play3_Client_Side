using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Play3_Client_Side.API
{
    public class DTO
    {
        [Serializable]
        public class PlayerDTO
        {
            [JsonProperty(PropertyName = "uuid")] 
            public string Uuid { get; set; }
            [JsonProperty(PropertyName = "xCoord")]
            public int XCoord { get; set; }
            [JsonProperty(PropertyName = "yCoord")]
            public int YCoord { get; set; }
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "health")]
            public float Health { get; set; }
        }

        [Serializable]
        public class FoodDTO
        {
            [JsonProperty(PropertyName = "uuid")]
            public string Uuid { get; set; }
            [JsonProperty(PropertyName = "xCoord")]
            public int XCoord { get; set; }
            [JsonProperty(PropertyName = "yCoord")]
            public int YCoord { get; set; }
            [JsonProperty(PropertyName = "healthPoints")]
            public float HealthPoints { get; set; }
        }

        [Serializable]
        public class ObstacleDTO
        {
            [JsonProperty(PropertyName = "uuid")]
            public string Uuid { get; set; }
            [JsonProperty(PropertyName = "xCoord")]
            public int XCoord { get; set; }
            [JsonProperty(PropertyName = "yCoord")]
            public int YCoord { get; set; }
            [JsonProperty(PropertyName = "damagePoints")]
            public float DamagePoints { get; set; }
        }

        [Serializable]
        public class GameDTO
        {
            [JsonProperty(PropertyName = "players")]
            public List<PlayerDTO> Players { get; set; }

            [JsonProperty(PropertyName = "foods")]
            public List<FoodDTO> Foods { get; set; }

            [JsonProperty(PropertyName = "obstacles")]
            public List<ObstacleDTO> Obstacles { get; set; }
        }
    }
}
