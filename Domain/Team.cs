
namespace Rugby.Domain
{
    public class Team
    {
        public string Name { get; set; }
        public double Coefficient { get; set; }
        public double HomeAdvantageFactor { get; set; }
        public double TeamForm { get; set; }
        public int LeaguePoints { get; set; }
        public int Score { get; set; }
        public int Tries { get; set; }
        
        public Team(string name, double coefficient, double homeAdvantageFactor, double teamForm)
        {
            Name = name;
            Coefficient = coefficient;
            HomeAdvantageFactor = homeAdvantageFactor;
            TeamForm = teamForm;
            LeaguePoints = 0;
            Score = 0;
            Tries = 0;
        }
    }
}
