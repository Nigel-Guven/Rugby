using Rugby.Domain;

namespace Rugby.Application
{
    public static class GroupCreator
    {
        public static List<League> DivideTeamsIntoGroups(List<Team> teams, bool shouldPrint)
        {
            var sortedTeams = teams.OrderByDescending(t => t.Coefficient).ToList();

            League league = new League();

            for (int i = 0; i < 12; i ++)
            {
                league.AddTeam(sortedTeams.ElementAt(i));
            }

            league.RemoveTeamByName("South Africa");
            league.RemoveTeamByName("New Zealand");
            league.RemoveTeamByName("Argentina");
            league.RemoveTeamByName("Australia");
            league.RemoveTeamByName("Fiji");
            league.RemoveTeamByName("Georgia");

            List<League> leagues = new()
            {
                league, // Group A
                //new League(), // Group B
                //new League(), // Group C
                //new League(), // Group D
                //new League(), // Group E
                //new League(), // Group F
                //new League(), // Group G
                //new League()  // Group H
            };

            /*
            List<List<Team>> rankBands = new List<List<Team>>();
            for (int i = 0; i < sortedTeams.Count; i += 8)
            {
                var rankBand = sortedTeams.Skip(i).Take(8).ToList();
                rankBands.Add(rankBand);
            }

            // Step 4: Randomly shuffle each rank band to ensure randomness
            Random random = new Random();
            for (int i = 0; i < rankBands.Count; i++)
            {
                rankBands[i] = rankBands[i].OrderBy(x => random.Next()).ToList(); // Shuffle each rank band
            }

            int groupIndex = 0;
            foreach (var rankBand in rankBands)
            {
                foreach (var team in rankBand)
                {
                    // Distribute teams from each rank band to each group
                    leagues[groupIndex].AddTeam(team);
                    groupIndex = (groupIndex + 1) % 8;  // Wrap around after the 8th group
                }
            }*/



            if (shouldPrint) 
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
