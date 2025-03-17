
namespace Rugby.Domain
{
    #pragma warning disable CS8600
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

        public void RemoveTeamByName(string name)
        {
            // Find the team by its name
            Team teamToRemove = Teams.FirstOrDefault(t => t.Name == name);

            // If the team is found, remove it
            if (teamToRemove != null)
            {
                Teams.Remove(teamToRemove);
            }
        }
    }
}
