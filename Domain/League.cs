
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

        public void DisplayStandings()
        {
            Console.WriteLine("League Standings:");
            foreach (var team in Teams.OrderByDescending(t => t.Score))
            {
                Console.WriteLine($"{team.Name}: {team.Score} points");
            }
        }
    }
}
