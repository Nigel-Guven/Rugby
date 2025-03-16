using Rugby.Domain;

namespace Rugby.Application
{
    public class TablePointsCreator
    {
        public static (int homePoints, int awayPoints) TablePointsCalculator(MatchResult matchResult)
        {
            var homeTeamScore = matchResult.FirstTeamPenalties * 3 + matchResult.FirstTeamTries * 5 + matchResult.FirstTeamConversions * 2;
            var awayTeamScore = matchResult.SecondTeamPenalties * 3 + matchResult.SecondTeamTries * 5 + matchResult.SecondTeamConversions * 2;

            int homePoints = 0;
            int awayPoints = 0;

            // Determine if there's a winner or a draw
            if (homeTeamScore > awayTeamScore)
            {
                homePoints = 4; // Home team wins
            }
            else if (awayTeamScore > homeTeamScore)
            {
                awayPoints = 4; // Away team wins
            }
            else
            {
                homePoints = 2; // It's a draw
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
