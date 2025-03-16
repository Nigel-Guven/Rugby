using Rugby.Domain;
namespace Rugby.Application
{
    public class TablePointsCreator
    {
        public static (int homePoints, int awayPoints) TablePointsCalculator(MatchResult matchResult)
        {
            var homeTeamScore = matchResult.FirstTeamPenalties * 3 + matchResult.FirstTeamTries * 5 + matchResult.FirstTeamConversions * 2;
            var awayTeamScore = matchResult.SecondTeamPenalties * 3 + matchResult.SecondTeamTries * 5 + matchResult.SecondTeamConversions * 2;

            var homePoints = 0;
            var awayPoints = 0;

            // Determine if there's a winner or a draw
            if (homeTeamScore > awayTeamScore)
            {
                // Team1 wins
                homePoints = 4;
                awayPoints = 0;
            }
            else if (awayTeamScore > homeTeamScore)
            {
                // Team2 wins
                homePoints = 0;
                awayPoints = 4;
            }
            else
            {
                // It's a draw
                homePoints = 2;
                awayPoints = 2;
            }

            // Bonus point for scoring 4 or more tries
            if (matchResult.FirstTeamTries >= 4)
            {
                homePoints += 1;
            }
            if (matchResult.SecondTeamTries >= 4)
            {
                awayPoints += 1;
            }

            // Bonus point for losing by less than 7 points
            if (Math.Abs(homeTeamScore - awayTeamScore) < 7)
            {
                if (homeTeamScore < awayTeamScore)
                {
                    homePoints += 1;
                }
                else if (awayTeamScore < homeTeamScore)
                {
                    awayPoints += 1;
                }
            }

            return (homePoints, awayPoints);
        }
    }
}
