
namespace Rugby.Domain
{
    public class AlgorithmConstants
    {
        public const double HomeAdvantageBonus = 0.05;
        public const double LuckFactorMin = 0.85;
        public const double LuckFactorMax = 1.15;

        public const double BaseCoefficientMultiplier = 0.5;

        public const double YellowCardChanceToScore = 1.5;
        public const double SecondYellowCardChanceToScore = 2.2;
        public const double ThirdYellowCardChanceToScore = 3.0;
        public const double DefaultChanceToScore = 0.0;
        public const double ChanceOfYellowCard = 0.05;
        public const double ChanceOfSecondYellowCard = 0.018;
        public const double ChanceOfThirdYellowCard = 0.0005;

        public const double TrashingThresholdHigh = 80.00;
        public const double TrashingThresholdLow = 60.00;
        public const double TrashMultiplier = 1.5;
        public const double HomeTeamTrashBoost = 1.3;
        public const double AwayTeamTrashBoost = 1.2;

        public const double TrashHomeLoserMaxScore = 20;
        public const double TrashAwayLoserMaxScore = 15;

        public const double LimitLowPerformingHomeTeamScore = 12;
        public const double LimitLowPerformingAwayTeamScore = 10;

        public const double ShockWinChance = 0.1;
        public const double HighCoefficientDifferenceShockWinChance = 0.5;
        public const double CoefficientThresholdDifference = 20;

        public const double LimitMaximumScoreOnTrashHome = 50;
        public const double LimitMaximumScoreOnTrashAway = 15;
    }
}
