using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class Profile : EntityBase
    {
        public string Name { get; set; }
        public string Email { get; set; } //AccountId
        public DateTime Birthday { get; set; }
        public string Photo { get; set; }
        public virtual ICollection<Profile> Follows { get; set; }
    }
}
