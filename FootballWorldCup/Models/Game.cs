using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FootballWorldCup.Services;

namespace FootballWorldCup.Models
{
    public class Game 
    {
        public required string HomeTeam { get; init; }
        public string AwayTeam { get; init; }
        public int HomeScore { get; set; } = 0;
        public int AwayScore { get; set; } = 0;
        public DateTime StartTime { get; init; } = DateTime.Now;
        public override string ToString()
        {
            return $"{this.HomeTeam} {this.HomeScore} - {this.AwayTeam} {this.AwayScore}";
        }
    }
}

