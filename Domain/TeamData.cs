using System.Text.Json.Serialization;

namespace Rugby.Domain
{
    #pragma warning disable CS8618
    public class TeamData
    {
        [JsonPropertyName("nation")]
        public string Nation { get; set; }

        [JsonPropertyName("coefficient")]
        public double Coefficient { get; set; }

        [JsonPropertyName("homeAdvantageFactor")]
        public double HomeAdvantageFactor { get; set; }

        [JsonPropertyName("teamForm")]
        public double TeamForm { get; set; }
    }
}
