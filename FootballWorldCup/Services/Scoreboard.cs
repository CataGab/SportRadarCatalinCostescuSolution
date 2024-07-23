using FootballWorldCup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballWorldCup.Services
{
    public class ScoreboardService
    {
        public List<Game> games;

        public ScoreboardService()
        {
            games = new List<Game>();
        }

        public void StartGame(string homeTeam, string awayTeam)
        {
            TeamNameValidation(homeTeam, awayTeam);
            games.Add(new Game()
            {
                HomeTeam = homeTeam,
                AwayTeam = awayTeam
            });
        }

        public void FinishGame(string homeTeam, string awayTeam)
        {
            TeamNameValidation(homeTeam, awayTeam);
            var game = games.FirstOrDefault(g => g.HomeTeam == homeTeam && g.AwayTeam == awayTeam);
            if (game != null)
            {
                games.Remove(game);
            }
        }

        public void UpdateScore(string homeTeam, string awayTeam, int homeScore, int awayScore)
        {
            TeamNameValidation(homeTeam, awayTeam);
            TotalScoreValidation(homeScore, awayScore);
            var game = games.FirstOrDefault(g => g.HomeTeam == homeTeam && g.AwayTeam == awayTeam);
            if (game != null)
            {
                game.HomeScore = homeScore;
                game.AwayScore = awayScore;
            }
        }
        
        public string GetSummary()
        {
            return string.Join("\n", GetSortedGamesList().Select(g => g.ToString()));
        }

        public List<Game> GetSortedGamesList()
        {
            return games.OrderByDescending(g => g.HomeScore + g.AwayScore)
                        .ThenByDescending(g => g.StartTime)
                        .ToList();
        }
        private static void TeamNameValidation(string homeTeam, string awayTeam)
        {
            if (string.IsNullOrWhiteSpace(homeTeam))
                throw new ArgumentException("Home team name cannot be empty or null.", nameof(homeTeam));
            if (string.IsNullOrWhiteSpace(awayTeam))
                throw new ArgumentException("Away team name cannot be empty or null.", nameof(awayTeam));
        }
        private static void TotalScoreValidation(int homeScore, int awayScore)
        {
            if (homeScore < 0 || awayScore < 0)
                throw new ArgumentException("Score Values cannot be negative!", (homeScore < 0) ? nameof(homeScore) : nameof(awayScore));
        }
    }
}

