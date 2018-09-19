using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }
    }
}
