using System.Collections.Generic;

namespace RuleMaker.Models
{
    public class ScoringPlayer
    {
        public string Name { get; set; }
        public List<Row> Rows { get; set; }
        public double Score { get; set; }
    }
}