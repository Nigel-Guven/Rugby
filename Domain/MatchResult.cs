namespace Rugby.Domain
{
    public class MatchResult
    {
        public Team FirstTeam { get; set; }
        public int FirstTeamTotalScore { get; set; }
        public int FirstTeamTries { get; set; }
        public int FirstTeamConversions { get; set; }
        public int FirstTeamPenalties { get; set; }
        public Team SecondTeam { get; set; }
        public int SecondTeamTotalScore { get; set; }
        public int SecondTeamTries { get; set; }
        public int SecondTeamConversions { get; set; }
        public int SecondTeamPenalties { get; set; }


        public MatchResult(
            Team firstTeam, 
            int firstTeamTotalScore,
            int firstTeamTries, 
            int firstTeamConversions, 
            int firstTeamPenalties, 
            Team secondTeam, 
            int secondTeamTotalScore,
            int secondTeamTries, 
            int secondTeamConversions, 
            int secondTeamPenalties)
        {
            FirstTeam = firstTeam;
            FirstTeamTotalScore = firstTeamTotalScore;
            FirstTeamTries = firstTeamTries;
            FirstTeamConversions = firstTeamConversions;
            FirstTeamPenalties = firstTeamPenalties;
            SecondTeam = secondTeam;
            SecondTeamTotalScore = secondTeamTotalScore;
            SecondTeamTries = secondTeamTries;
            SecondTeamConversions = secondTeamConversions;
            SecondTeamPenalties = secondTeamPenalties;
        }
    }
}
