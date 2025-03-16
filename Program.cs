using Rugby.Application;
using Rugby.Domain;
using Rugby.Infrastructure;

namespace Rugby
{
#pragma warning disable CS8600
#pragma warning disable CS8604
    public class Program
    {
        static void Main(string[] args)
        {
    
            List<Team> teams = TeamRepository.LoadTeamsFromJson();

            var groups = GroupCreator.DivideTeamsIntoGroups(teams, false);

            League groupA = groups.FirstOrDefault();

            MatchCreator.ScheduleMatches(groupA);

            /*
            for (int i = 0; i < groupA.Teams.Count - 1; i += 2)
            {
                Team homeTeam = groupA.Teams.ElementAt(i);
                Team awayTeam = groupA.Teams.ElementAt(i + 1);

                var result = MatchCreator.GenerateMatchScore(homeTeam, awayTeam);

                

                
                Console.WriteLine($"{homeTeam.Name} vs {awayTeam.Name}");
                Console.WriteLine($"Home Team Score: {homePoints} points");
                Console.WriteLine($"Away Team Score: {awayPoints} points");
                Console.WriteLine($"{homeTeam.Name} Total Points: {homeTeam.LeaguePoints}");
                Console.WriteLine($"{awayTeam.Name} Total Points: {awayTeam.LeaguePoints}");
                Console.WriteLine();
            }*/

            /*
        foreach(League group in groups)
        {
            MatchCreator.ScheduleMatches(group);

            for (int i = 0; i < group.Teams.Count - 1; i += 2)
            {
                Team homeTeam = group.Teams.ElementAt(i);
                Team awayTeam = group.Teams.ElementAt(i + 1);

                var result = MatchCreator.GenerateMatchScore(homeTeam, awayTeam);

                var (homePoints, awayPoints) = Algorithms.TablePointsCalculator(result);

                homeTeam.LeaguePoints += homePoints;
                awayTeam.LeaguePoints += awayPoints;


                Console.WriteLine($"{homeTeam.Name} vs {awayTeam.Name}");
                Console.WriteLine($"Home Team Score: {homePoints} points");
                Console.WriteLine($"Away Team Score: {awayPoints} points");
                Console.WriteLine($"{homeTeam.Name} Total Points: {homeTeam.LeaguePoints}");
                Console.WriteLine($"{awayTeam.Name} Total Points: {awayTeam.LeaguePoints}");
                Console.WriteLine();
            }
        }*/
        }
    }
}

