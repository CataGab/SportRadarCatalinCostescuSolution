# Football World Cup Score Board

## Introduction

The program provides functionality to manage a Football World Cup Score Board, allowing you to start a game, finish a game, update the score, and get a summary of games ordered by total score and recency created by Catalin Gabriel Costescu.

## Requirements

For this program you will require: .Net 8.0 and XUnit package to run the project.

## Usage

1. **Start a game**: Adds a game to the scoreboard with an initial score of 0-0.
2. **Finish a game**: Removes a game from the scoreboard.
3. **Update score**: Updates the score of a specified game.
4. **Get summary**: Returns a summary of games ordered by total score and recency.

## Testing

I have covered testing for each of the functions used to build this application with multiple test cases, including edge cases as requested:
- Adding a game to score board;
- Removing a game from score board;
- Updating the score;
- Check that a list of given games are returned ordered by total score descending (if 2 or more games have the same score will be returned ordered by the most recently added to our system.)
- Checks for score validations. (if the numbers are positive)
- Was thinking of including a case of "perhaps the score shouldn't be more than a really big number, for example, 9999", as this is realistically never going to happen, but in theory can be made as a human error. For the given requirements, I have added the most common human error to be tested: negative scoring.
- Finally, checks for team names.
