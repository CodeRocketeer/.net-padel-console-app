using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Domain.Models
{
    public class Season(string title, DateTime startDate, DayOfWeek dayOfWeek)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; } = title;

        public DateTime StartDate { get; set; } = startDate;

        public DayOfWeek DayOfWeek { get; set; } = dayOfWeek;

        public DateTime EndDate { get; set; }

        public List<Match> Matches { get; set; } = new List<Match>();

        public void SetEndDate()
        {
            if (Matches.Any())
            {
            EndDate = Matches.Last(
            ).Date;
            }
        }


    }

}
