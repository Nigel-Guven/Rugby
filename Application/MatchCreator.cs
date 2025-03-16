using Rugby.Domain;

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

                    Console.WriteLine($"Match: {homeTeam.Name} vs {awayTeam.Name}");
                    var result = GenerateMatchScore(homeTeam, awayTeam);
                    

                    var (homePoints, awayPoints) = TablePointsCreator.TablePointsCalculator(result);

                    homeTeam.LeaguePoints += homePoints;
                    awayTeam.LeaguePoints += awayPoints;

                    Console.WriteLine($"{result.FirstTeam.Name} {result.FirstTeamTotalScore} - {result.SecondTeamTotalScore} {result.SecondTeam.Name}");

                    GroupCreator.DisplaySingleGroup(league);
                }

                // Rotate the teams (keeping the first team fixed, rotating the others)
                Team temp = teams[1];
                for (int i = 1; i < teams.Count - 1; i++)
                {
                    teams[i] = teams[i + 1];
                }
                teams[teams.Count - 1] = temp;

                Console.WriteLine(); // Break Line between rounds
            }
        }

        private static MatchResult GenerateMatchScore(Team homeTeam, Team awayTeam)
        {
            // Step 1: Calculate base scores based on coefficients
            double homeBaseScore = homeTeam.Coefficient * 0.5; // Adjust the coefficient for a base score
            double awayBaseScore = awayTeam.Coefficient * 0.5; // Adjust the coefficient for a base score

            // Step 2: Apply home advantage bonus to the home team
            homeBaseScore += homeBaseScore * HomeAdvantageBonus;

            // Step 3: Add luck factor (random) to both teams
            double homeLuckFactor = GetLuckFactor();
            double awayLuckFactor = GetLuckFactor();

            homeBaseScore *= homeLuckFactor;
            awayBaseScore *= awayLuckFactor;

            // Step 4: Generate tries, conversions, and penalties for each team
            int homeTries = GenerateTries(homeBaseScore);
            int awayTries = GenerateTries(awayBaseScore);

            int homeConversions = homeTries; // Assuming 1 conversion for each try
            int awayConversions = awayTries; // Assuming 1 conversion for each try

            int homePenalties = GeneratePenalties(homeBaseScore);
            int awayPenalties = GeneratePenalties(awayBaseScore);

            // Step 5: Calculate the total score for each team (if needed for debugging or output)
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
        private static double GetLuckFactor()
        {
            Random random = new();
            return random.NextDouble() * (LuckFactorMax - LuckFactorMin) + LuckFactorMin;
        }

        // Generate number of tries based on team's base strength
        private static int GenerateTries(double baseScore)
        {
            Random random = new();
            return (int)(baseScore / 10) + random.Next(0, 3); // Random between 0-3 tries
        }

        // Generate number of penalties based on team's base strength
        private static int GeneratePenalties(double baseScore)
        {
            Random random = new();
            return (int)(baseScore / 15) + random.Next(0, 3); // Random between 0-3 penalties
        }
    }
}
