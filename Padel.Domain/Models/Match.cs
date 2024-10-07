namespace Padel.Domain.Models
{
    public class Match(string dayOfWeek, DateTime date, Team team1, Team team2)
    {
        public string DayOfWeek { get; set; } = dayOfWeek;
        public DateTime Date { get; set; } = date;
        public Team Team1 { get; set; } = team1;
        public Team Team2 { get; set; } = team2;

        public override string ToString()
        {
            return $"{Date.ToShortDateString()} ({DayOfWeek}): {Team1} vs {Team2}";
        }
    }
}
