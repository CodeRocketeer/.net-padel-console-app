using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Padel.Domain.Models
{
    public  class User
    {

        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required string Sex { get; set; } 

    }
}
