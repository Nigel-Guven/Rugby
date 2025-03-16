namespace Rugby.Domain
{
    public class Team
    {
        public string Name { get; set; }
        public double Coefficient { get; set; }
        public int LeaguePoints { get; set; }
        public int Score { get; set; }
        public int Tries { get; set; }

        public Team(string name, double coefficient)
        {
            Name = name;
            Coefficient = coefficient;
            LeaguePoints = 0;
            Score = 0;
            Tries = 0;
        }
    }
}
