
namespace Rugby.Domain
{
    public class League
    {
        public List<Team> Teams { get; set; }

        public League()
        {
            Teams = new List<Team>();
        }

        public void AddTeam(Team team)
        {
            Teams.Add(team);
        }
    }
}
