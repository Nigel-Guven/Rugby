using Rugby.Domain;

namespace Rugby.Application
{
    public class MatchCreator
    {
        

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
            double homeBaseScore = homeTeam.Coefficient * AlgorithmConstants.BaseCoefficientMultiplier; // Adjust the coefficient for a base score
            double awayBaseScore = awayTeam.Coefficient * AlgorithmConstants.BaseCoefficientMultiplier; // Adjust the coefficient for a base score

            // Step 2: Apply home advantage bonus to the home team
            homeBaseScore += homeBaseScore * (AlgorithmConstants.HomeAdvantageBonus + homeTeam.HomeAdvantageFactor);
            awayBaseScore += awayBaseScore * (awayTeam.HomeAdvantageFactor);

            // Step 3: Add luck factor (random) to both teams
            double homeLuckFactor = GetLuckFactor(random, homeTeam.TeamForm);
            double awayLuckFactor = GetLuckFactor(random, awayTeam.TeamForm);

            homeBaseScore *= homeLuckFactor;
            awayBaseScore *= awayLuckFactor;

            // Step 4: Check for yellow cards
            var (homeTeamYellowCard, 
                awayTeamYellowCard,
                secondHomeYellowCard,
                secondAwayYellowCard,
                thirdHomeYellowCard,
                thirdAwayYellowCard) = GetYellowCards(random);

            // Step 5: Adjust scoring chances based on yellow cards
            double homeTeamYellowCardBoost = homeTeamYellowCard ? AlgorithmConstants.YellowCardChanceToScore : AlgorithmConstants.DefaultChanceToScore;  // 1.5x chance for away team to score
            double awayTeamYellowCardBoost = awayTeamYellowCard ? AlgorithmConstants.YellowCardChanceToScore : AlgorithmConstants.DefaultChanceToScore;  // 1.5x chance for home team to score
            double secondHomeTeamYellowCardBoost = secondHomeYellowCard ? AlgorithmConstants.YellowCardChanceToScore : AlgorithmConstants.DefaultChanceToScore;  // 1.5x chance for away team to score
            double secondAwayTeamYellowCardBoost = secondAwayYellowCard ? AlgorithmConstants.YellowCardChanceToScore : AlgorithmConstants.DefaultChanceToScore;  // 1.5x chance for home team to score
            double thirdHomeTeamYellowCardBoost = thirdHomeYellowCard ? AlgorithmConstants.YellowCardChanceToScore : AlgorithmConstants.DefaultChanceToScore;  // 1.5x chance for away team to score
            double thirdAwayTeamYellowCardBoost = thirdAwayYellowCard ? AlgorithmConstants.YellowCardChanceToScore : AlgorithmConstants.DefaultChanceToScore;  // 1.5x chance for home team to score

            // Step 6: Implement trashing logic for teams with high coefficients
            double trashingThresholdHigh = AlgorithmConstants.TrashingThresholdHigh;  // Coefficient threshold for high-performing teams
            double trashingThresholdLow = AlgorithmConstants.TrashingThresholdLow;  // Coefficient threshold for low-performing teams
            double trashingFactor = AlgorithmConstants.TrashMultiplier;  // Boost multiplier for a trashing scenario

            bool isHomeTeamTrash = homeTeam.Coefficient > trashingThresholdHigh && homeBaseScore > awayBaseScore * AlgorithmConstants.HomeTeamTrashBoost;  // Significant advantage for home team
            bool isAwayTeamTrash = awayTeam.Coefficient > trashingThresholdHigh && awayBaseScore > homeBaseScore * AlgorithmConstants.AwayTeamTrashBoost;  // Significant advantage for away team

            bool isLowHomeTeam = homeTeam.Coefficient < trashingThresholdLow;  // Low coefficient for home team
            bool isLowAwayTeam = awayTeam.Coefficient < trashingThresholdLow;  // Low coefficient for away team

            // Trashing conditions: if a stronger team faces a weaker team, they dominate
            if (isHomeTeamTrash)
            {
                // Home team is trashing away team
                homeBaseScore *= trashingFactor;  // Significantly boost home team's score
                awayBaseScore = Math.Min(awayBaseScore, AlgorithmConstants.TrashAwayLoserMaxScore);  // Away team scores very little or nothing
            }
            else if (isAwayTeamTrash)
            {
                // Away team is trashing home team
                awayBaseScore *= trashingFactor;  // Significantly boost away team's score
                homeBaseScore = Math.Min(homeBaseScore, AlgorithmConstants.TrashHomeLoserMaxScore);  // Home team scores very little or nothing
            }
            else if (isLowHomeTeam)
            {
                // Low-performing home team scenario
                homeBaseScore = Math.Min(homeBaseScore, AlgorithmConstants.LimitLowPerformingHomeTeamScore);  // Limit home team's score if they're low-performing
                awayBaseScore *= AlgorithmConstants.LimitLowPerformingHomeTeamScore;  // Boost away team's chances of scoring if the home team is low-performing
            }
            else if (isLowAwayTeam)
            {
                // Low-performing away team scenario
                awayBaseScore = Math.Min(awayBaseScore, AlgorithmConstants.LimitLowPerformingAwayTeamScore);  // Limit away team's score if they're low-performing
                homeBaseScore *= AlgorithmConstants.LimitLowPerformingAwayTeamScore;  // Boost home team's chances of scoring if the away team is low-performing
            }

            // Step 7: Add shock win scenario for underdog team
            // Calculate the coefficient difference
            double coefficientDifference = Math.Abs(homeTeam.Coefficient - awayTeam.Coefficient);

            // Randomly determine if the underdog wins
            bool shockWin = coefficientDifference > AlgorithmConstants.CoefficientThresholdDifference 
                ? 
                random.NextDouble() < AlgorithmConstants.HighCoefficientDifferenceShockWinChance  : random.NextDouble() < AlgorithmConstants.ShockWinChance;

            // Apply shock win effect to tries, conversions, and penalties
            double shockFactor = shockWin ? 3.0 : 1.0;

            // Step 8: Limit the maximum score for the stronger team in a trashing scenario to a reasonable cap
            //homeBaseScore = Math.Min(homeBaseScore, AlgorithmConstants.LimitMaximumScoreOnTrashHome);  // Cap the stronger team's score to X points
            //awayBaseScore = Math.Min(awayBaseScore, AlgorithmConstants.LimitMaximumScoreOnTrashAway);   // Cap the weaker team's score to X points

            // Step 9: Generate tries, conversions, and penalties for each team
            int homeTries = GenerateTries(random, homeBaseScore * awayTeamYellowCardBoost + secondAwayTeamYellowCardBoost + thirdAwayTeamYellowCardBoost) * (int)shockFactor;
            int awayTries = GenerateTries(random, awayBaseScore * homeTeamYellowCardBoost + secondHomeTeamYellowCardBoost + thirdHomeTeamYellowCardBoost) * (int)shockFactor;

            int homeConversions = homeTries * (int)shockFactor; // Assuming 1 conversion for each try
            int awayConversions = awayTries * (int)shockFactor; // Assuming 1 conversion for each try

            int homePenalties = GeneratePenalties(random, homeBaseScore) * (int)shockFactor;
            int awayPenalties = GeneratePenalties(random, awayBaseScore) * (int)shockFactor;

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
            double luck = random.NextDouble() * (AlgorithmConstants.LuckFactorMax - AlgorithmConstants.LuckFactorMin) + AlgorithmConstants.LuckFactorMin;
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

        private static (
            bool homeTeamYellowCard,  bool awayTeamYellowCard,
            bool secondHomeTeamYellowCard,  bool secondAwayTeamYellowCard,
            bool thirdHomeTeamYellowCard, bool thirdAwayTeamYellowCard) 
            GetYellowCards(Random random)
        {
            double firstYellowCardChance = AlgorithmConstants.ChanceOfYellowCard; 
            double secondYellowCardChance = AlgorithmConstants.ChanceOfSecondYellowCard;
            double thirdYellowCardChance = AlgorithmConstants.ChanceOfThirdYellowCard;


            bool homeTeamYellowCard = random.NextDouble() < firstYellowCardChance;
            bool awayTeamYellowCard = random.NextDouble() < firstYellowCardChance;

            bool secondHomeTeamYellowCard = random.NextDouble() < secondYellowCardChance;
            bool secondAwayTeamYellowCard = random.NextDouble() < secondYellowCardChance;

            bool thirdHomeTeamYellowCard = random.NextDouble() < thirdYellowCardChance;
            bool thirdAwayTeamYellowCard = random.NextDouble() < thirdYellowCardChance;



            return (homeTeamYellowCard, awayTeamYellowCard, secondHomeTeamYellowCard, secondAwayTeamYellowCard, thirdHomeTeamYellowCard, thirdAwayTeamYellowCard);
        }
    }
}
