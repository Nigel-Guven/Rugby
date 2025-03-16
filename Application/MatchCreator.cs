﻿using Rugby.Domain;

namespace Rugby.Application
{
    public class MatchCreator
    {
        private const double HomeAdvantageBonus = 0.1; // 10% bonus for home advantage
        private const double LuckFactorMin = 0.85; // Bad day
        private const double LuckFactorMax = 1.15; // Good day

        public static void ScheduleMatches(League league)
        {
            var teams = league.Teams;

            // Ensure there is an even number of teams
            if (teams.Count % 2 != 0)
            {
                throw new InvalidOperationException("Number of teams should be even for a round-robin schedule.");
            }

            Console.WriteLine("Scheduling matches:");

            // Create a round-robin schedule (pairing teams)
            int numRounds = teams.Count - 1;

            // Rotate through the teams and generate matches for each round
            for (int round = 0; round < numRounds; round++)
            {
                Console.WriteLine($"-----------------------");
                Console.WriteLine();
                Console.WriteLine($"Round {round + 1}:");

                // Create pairings for the current round
                for (int i = 0; i < teams.Count / 2; i++)
                {
                    Team homeTeam = teams[i];
                    Team awayTeam = teams[teams.Count - 1 - i];

                    Console.WriteLine();
                    Console.WriteLine($"Match: {homeTeam.Name} vs {awayTeam.Name}");
                    Console.WriteLine();

                    var result = GenerateMatchScore(homeTeam, awayTeam);
                    

                    var (homePoints, awayPoints) = TablePointsCreator.TablePointsCalculator(result);

                    homeTeam.LeaguePoints += homePoints;
                    awayTeam.LeaguePoints += awayPoints;

                    Console.WriteLine($"{result.FirstTeam.Name} {result.FirstTeamTotalScore} - {result.SecondTeamTotalScore} {result.SecondTeam.Name}");

                    
                }

                // Rotate the teams (keeping the first team fixed, rotating the others)
                Team temp = teams[1];
                for (int i = 1; i < teams.Count - 1; i++)
                {
                    teams[i] = teams[i + 1];
                }
                teams[teams.Count - 1] = temp;


                GroupCreator.DisplaySingleGroup(league);
                Console.WriteLine(); // Break Line between rounds
            }
        }

        private static MatchResult GenerateMatchScore(Team homeTeam, Team awayTeam)
        {
            Random random = new();

            // Step 1: Calculate base scores based on coefficients
            double homeBaseScore = homeTeam.Coefficient * 0.5; // Adjust the coefficient for a base score
            double awayBaseScore = awayTeam.Coefficient * 0.5; // Adjust the coefficient for a base score

            // Step 2: Apply home advantage bonus to the home team
            homeBaseScore += homeBaseScore * (HomeAdvantageBonus + homeTeam.HomeAdvantageFactor);
            awayBaseScore += awayBaseScore * (awayTeam.HomeAdvantageFactor);

            // Step 3: Add luck factor (random) to both teams
            double homeLuckFactor = GetLuckFactor(random, homeTeam.TeamForm);
            double awayLuckFactor = GetLuckFactor(random, awayTeam.TeamForm);

            homeBaseScore *= homeLuckFactor;
            awayBaseScore *= awayLuckFactor;

            // Step 4: Check for yellow cards
            var (homeTeamYellowCard, awayTeamYellowCard) = GetYellowCards(random);

            // Step 5: Adjust scoring chances based on yellow cards
            double homeTeamYellowCardBoost = homeTeamYellowCard ? 1.5 : 1.0;  // 1.5x chance for away team to score
            double awayTeamYellowCardBoost = awayTeamYellowCard ? 1.5 : 1.0;  // 1.5x chance for home team to score

            // Step 6: Implement trashing logic for teams with high coefficients
            double trashingThresholdHigh = 81.00;  // Coefficient threshold for high-performing teams
            double trashingThresholdLow = 57.87;  // Coefficient threshold for low-performing teams
            double trashingFactor = 3.0;  // Boost multiplier for a trashing scenario

            bool isHomeTeamTrash = homeTeam.Coefficient > trashingThresholdHigh && homeBaseScore > awayBaseScore * 1.5;  // Significant advantage for home team
            bool isAwayTeamTrash = awayTeam.Coefficient > trashingThresholdHigh && awayBaseScore > homeBaseScore * 1.5;  // Significant advantage for away team

            // Trashing conditions: if a stronger team faces a weaker team, they dominate
            if (isHomeTeamTrash)
            {
                // Home team is trashing away team
                homeBaseScore *= trashingFactor;  // Significantly boost home team's score
                awayBaseScore = Math.Min(awayBaseScore, 5);  // Away team scores very little or nothing
            }
            else if (isAwayTeamTrash)
            {
                // Away team is trashing home team
                awayBaseScore *= trashingFactor;  // Significantly boost away team's score
                homeBaseScore = Math.Min(homeBaseScore, 5);  // Home team scores very little or nothing
            }

            // Limit the maximum score for the stronger team in a trashing scenario to a reasonable cap
            homeBaseScore = Math.Min(homeBaseScore, 85);  // Cap the stronger team's score to 50 points
            awayBaseScore = Math.Min(awayBaseScore, 12);   // Cap the weaker team's score to 5 points (or 0 if you prefer)

            // Step 7: Generate tries, conversions, and penalties for each team
            int homeTries = GenerateTries(random, homeBaseScore * awayTeamYellowCardBoost);
            int awayTries = GenerateTries(random, awayBaseScore * homeTeamYellowCardBoost);

            int homeConversions = homeTries; // Assuming 1 conversion for each try
            int awayConversions = awayTries; // Assuming 1 conversion for each try

            int homePenalties = GeneratePenalties(random, homeBaseScore);
            int awayPenalties = GeneratePenalties(random, awayBaseScore);

            // Step 7: Calculate the total score for each team (if needed for debugging or output)
            int homeTeamScore = (homeTries * 5) + (homeConversions * 2) + (homePenalties * 3);
            int awayTeamScore = (awayTries * 5) + (awayConversions * 2) + (awayPenalties * 3);

            // Return a MatchResult object
            return new MatchResult(
                homeTeam,
                homeTeamScore,
                homeTries,
                homeConversions,
                homePenalties,
                awayTeam,
                awayTeamScore,
                awayTries,
                awayConversions,
                awayPenalties);
        }

        // Generate random luck factor for a team (Good or Bad day)
        private static double GetLuckFactor(Random random, double teamForm)
        {
            double luck = random.NextDouble() * (LuckFactorMax - LuckFactorMin) + LuckFactorMin;
            return luck * teamForm;
        }

        // Generate number of tries based on team's base strength
        private static int GenerateTries(Random random, double baseScore)
        {
            int maxTries = (int)(baseScore / 10); // More tries for stronger teams
            return random.Next(0, maxTries + 1); // Random between 0-3 tries
        }

        // Generate number of penalties based on team's base strength
        private static int GeneratePenalties(Random random, double baseScore)
        {
            int maxPenalties = (int)(baseScore / 15);
            return random.Next(0, maxPenalties + 1); // Random between 0-3 penalties
        }

        private static (bool homeTeamYellowCard, bool awayTeamYellowCard) GetYellowCards(Random random)
        {
            // Define chance of a yellow card for each team
            double yellowCardChance = 0.08; // 10% chance of getting a yellow card

            bool homeTeamYellowCard = random.NextDouble() < yellowCardChance;
            bool awayTeamYellowCard = random.NextDouble() < yellowCardChance;

            return (homeTeamYellowCard, awayTeamYellowCard);
        }
    }
}
