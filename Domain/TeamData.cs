using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Rugby.Domain
{
    public class TeamData
    {
        [JsonPropertyName("nation")]
        public string Nation { get; set; }

        [JsonPropertyName("coefficient")]
        public double Coefficient { get; set; }
    }
}
