using System;
using System.Collections.Generic;
using System.Linq;
using FootballWorldCup.Services;

namespace FootballWorldCup.Tests
{
    public class ScoreboardTests
    {
        [Theory]
        [InlineData("Team A", "Team B")]
        [InlineData("Team C", "Team D")]
        public void StartGame_AddsGameToScoreboardTest(string firstTeam, string secondTeam)
        {
            // Arrange
            var scoreboard = new ScoreboardService();                      
            
            // Act
            scoreboard.StartGame(firstTeam, secondTeam);
            
            // Assert
            Assert.Equal(1, scoreboard.GetSortedGamesList().Count);
        }

        [Theory]
        [InlineData("Team A", "Team B")]
        [InlineData("Team C", "Team D")]
        public void FinishGame_RemovesGameFromScoreboardTest(string homeTeam, string awayTeam)
        {
            // Arrange
            var scoreboard = new ScoreboardService();
            scoreboard.StartGame(homeTeam, awayTeam);

            // Act
            scoreboard.FinishGame(homeTeam, awayTeam);
            
            // Assert
            Assert.Equal(0, scoreboard.GetSortedGamesList().Count);
        }

        [Theory]
        [InlineData("Team A", "Team B", 3, 2)]
        [InlineData("Team C", "Team D", 1, 1)]
        [InlineData("Team E", "Team F", 5, 0)]
        public void UpdateScoreTest(string homeTeam, string awayTeam, int homeScore, int awayScore)
        {
            // Arrange
            var scoreboard = new ScoreboardService();
            scoreboard.StartGame(homeTeam, awayTeam);

            // Act
            scoreboard.UpdateScore(homeTeam, awayTeam, homeScore, awayScore);

            // Assert
            var game = scoreboard.GetSortedGamesList().FirstOrDefault(g => g.HomeTeam == homeTeam && g.AwayTeam == awayTeam);
            Assert.NotNull(game);
            Assert.Equal(homeScore, game.HomeScore);
            Assert.Equal(awayScore, game.AwayScore);
        }

        [Fact]
        public void GetSortedGamesList_ReturnsGamesOrderedByTotalScoreAndRecencyTest()
        {
            // Arrange
            var scoreboard = new ScoreboardService();
            scoreboard.StartGame("Mexico", "Canada");
            scoreboard.UpdateScore("Mexico", "Canada", 0, 5);

            scoreboard.StartGame("Spain", "Brazil");
            scoreboard.UpdateScore("Spain", "Brazil", 10, 2);

            scoreboard.StartGame("Germany", "France");
            scoreboard.UpdateScore("Germany", "France", 2, 2);

            scoreboard.StartGame("Uruguay", "Italy");
            scoreboard.UpdateScore("Uruguay", "Italy", 6, 6);

            scoreboard.StartGame("Argentina", "Australia");
            scoreboard.UpdateScore("Argentina", "Australia", 3, 1);

            // Act
            var summary = scoreboard.GetSortedGamesList();

            // Assert
            Assert.Collection(summary,
                game => Assert.Equal("Uruguay 6 - Italy 6", game.ToString()),
                game => Assert.Equal("Spain 10 - Brazil 2", game.ToString()),
                game => Assert.Equal("Mexico 0 - Canada 5", game.ToString()),
                game => Assert.Equal("Argentina 3 - Australia 1", game.ToString()),
                game => Assert.Equal("Germany 2 - France 2", game.ToString()));
        }

        [Fact]
        public void GetSummary_ReturnsGamesOrderedByTotalScoreAndRecencyTest()
        {
            // Arrange
            var scoreboard = new ScoreboardService();
            scoreboard.StartGame("Mexico", "Canada");
            scoreboard.UpdateScore("Mexico", "Canada", 0, 5);

            scoreboard.StartGame("Spain", "Brazil");
            scoreboard.UpdateScore("Spain", "Brazil", 10, 2);

            scoreboard.StartGame("Germany", "France");
            scoreboard.UpdateScore("Germany", "France", 2, 2);

            scoreboard.StartGame("Uruguay", "Italy");
            scoreboard.UpdateScore("Uruguay", "Italy", 6, 6);

            scoreboard.StartGame("Argentina", "Australia");
            scoreboard.UpdateScore("Argentina", "Australia", 3, 1);

            // Act
            var summary = scoreboard.GetSummary();

            var expected = string.Join("\n", "Uruguay 6 - Italy 6", "Spain 10 - Brazil 2", "Mexico 0 - Canada 5", "Argentina 3 - Australia 1", "Germany 2 - France 2");

            // Assert
            Assert.Equal(expected, summary);
        }

        [Theory]
        [InlineData(4, 2)]
        [InlineData(9, 6)]
        [InlineData(0, 0)]
        public void TotalScoreValidation_ValidScores_DoesNotThrowExceptionTest(int homeScore, int awayScore)
        {
            // Arrange
            var team1 = "Team A";
            var team2 = "Team B";
            var scoreboard = new ScoreboardService();
            scoreboard.StartGame(team1, team2);

            // Act
            var exception = Record.Exception(() => scoreboard.UpdateScore(team1, team2, homeScore, awayScore));

            // Assert
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(-3, 2)]
        [InlineData(4, -1)]
        [InlineData(-1, -1)]
        public void TotalScoreValidation_NegativeScores_ThrowsArgumentExceptionTest(int homeScore, int awayScore)
        {
            // Arrange
            var team1 = "Team A";
            var team2 = "Team B";
            var scoreboard = new ScoreboardService();
            scoreboard.StartGame(team1, team2);

            // Act
            var exception = Record.Exception(() => scoreboard.UpdateScore(team1, team2, homeScore, awayScore));

            // Assert
            Assert.Contains("Score Values cannot be negative!", exception.Message);
        }

        [Theory]
        [InlineData("Team A", "Team B")]
        [InlineData("Home", "Away")]
        [InlineData("Team 1", "Team 2")]
        public void TeamNameValidation_ValidNames_DoesNotThrowExceptionTest(string homeTeam, string awayTeam)
        {
            // Arrange
            var scoreboard = new ScoreboardService();
            
            // Act
            var exception = Record.Exception(() => scoreboard.StartGame(homeTeam, awayTeam));
            
            // Assert
            Assert.Null(exception);
        }

        [Theory]
        [InlineData(null, "Team B")]
        [InlineData("", "Team B")]
        [InlineData("   ", "Team B")]
        [InlineData("Team A", null)]
        [InlineData("Team A", "")]
        [InlineData("Team A", "   ")]
        public void TeamNameValidation_InvalidNames_ThrowsArgumentExceptionTest(string homeTeam, string awayTeam)
        {
            // Arrange
            var scoreboard = new ScoreboardService();

            // Act 
            var exception = Assert.Throws<ArgumentException>(() => scoreboard.StartGame(homeTeam, awayTeam));

            if (string.IsNullOrWhiteSpace(homeTeam))
            {
                Assert.Contains("Home team name cannot be empty or null.", exception.Message);
                Assert.Equal("homeTeam", exception.ParamName);
            }
            else
            {
                Assert.Contains("Away team name cannot be empty or null.", exception.Message);
                Assert.Equal("awayTeam", exception.ParamName);
            }
        }
    }
}
    