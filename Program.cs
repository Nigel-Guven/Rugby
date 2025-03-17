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


            foreach(League group in groups)
            {
                MatchCreator.ScheduleMatches(group);
            }

            foreach(League group in groups)
            {
                GroupCreator.DisplayTopTwo(group);
            }
        }
    }
}

