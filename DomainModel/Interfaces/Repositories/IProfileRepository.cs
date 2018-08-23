using DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Interfaces.Repositories
{
    public interface IProfileRepository
    {
        void Create(Profile profile);
        Profile Get(Guid id);
    }
}
 