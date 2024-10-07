namespace Padel.Domain.Models
{
    public class Team
    {
        public User Player1 { get; set; }
        public User Player2 { get; set; }

        public Team(User player1, User player2)
        {
            Player1 = player1;
            Player2 = player2;
        }

        public override string ToString()
        {
            return $"{Player1.Name}({Player1.Sex}) & {Player2.Name}({Player2.Sex})";
        }
    }
}
