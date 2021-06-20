using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace RuleMaker.Models
{
    public class ScoringGame
    {
        public string Author { get; set; }
        public string BggId { get; set; }
        public string Description { get; set; }
        public int MaxPlayers { get; set; }
        public int MinPlayers { get; set; }
        public string Name { get; set; }
        public int PlayerCount { get; set; }
        public List<Row> Rows { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ScoringType ScoringType { get; set; }

        public double Version { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public Winner Winner { get; set; }
    }
}