using Rugby.Domain;

namespace Rugby.Application
{
    public static class GroupCreator
    {
        public static List<League> DivideTeamsIntoGroups(List<Team> teams, bool shouldPrint)
        {
            var sortedTeams = teams.OrderByDescending(t => t.Coefficient).ToList();

            List<League> leagues = new List<League>
            {
                new League(), // Group A
                new League(), // Group B
                new League(), // Group C
                new League(), // Group D
                new League(), // Group E
                new League(), // Group F
                new League(), // Group G
                new League()  // Group H
            };

            for (int i = 0; i < sortedTeams.Count; i++)
            {
                int groupIndex = i % 8; 
                leagues[groupIndex].AddTeam(sortedTeams[i]);
            }

            if(shouldPrint) 
                DisplayGroups(leagues);

            return leagues;
        }

        public static void DisplayGroups(List<League> leagues)
        {
            char groupLabel = 'A';
            foreach (var league in leagues)
            {
                Console.WriteLine($"Group {groupLabel}:");
                foreach (var team in league.Teams)
                {
                    Console.WriteLine($"  {team.Name}, {team.LeaguePoints}");
                }
                groupLabel++;
                Console.WriteLine();
            }
        }

        public static void DisplaySingleGroup(League league)
        {
            Console.WriteLine();
            Console.WriteLine("Current League Standings:");
            foreach (var team in league.Teams.OrderByDescending(t => t.LeaguePoints))
            {
                Console.WriteLine($"{team.Name}: {team.LeaguePoints} points");
            }
            Console.WriteLine();

        }

        public static void DisplayTopTwo(League league)
        {
            var topTwoTeams = league.Teams.OrderByDescending(t => t.Score).Take(2);

            Console.WriteLine();
            foreach (var team in topTwoTeams)
            {
                Console.WriteLine($"{team.Name}");
            }
        }
    }
}
