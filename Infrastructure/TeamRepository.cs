using Rugby.Domain;
using System.Text.Json;

namespace Rugby.Infrastructure
{
    #pragma warning disable CS8600
    public class TeamRepository
    {
        public static List<Team>? LoadTeamsFromJson()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "../../../Teams.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found at: {filePath}");
                return null;
            }

            try
            {
                string jsonString = File.ReadAllText(filePath);

                List<TeamData> teamDataList = JsonSerializer.Deserialize<List<TeamData>>(jsonString);

                if (teamDataList == null)
                {
                    Console.WriteLine("Failed to deserialize JSON.");
                    return null;
                }

                List<Team> teams = new();
                foreach (var data in teamDataList)
                {
                    teams.Add(new Team(data.Nation, data.Coefficient));
                }

                return teams;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading or deserializing JSON: {ex.Message}");
                return null;
            }
        }
    }
}
