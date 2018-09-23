using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class Friendship : EntityBase
    {
        //Essa classe poderia ter uma chave composta
        //mas coloquei cada registro contendo uma chave única
        //public virtual Profile ProfileIdA { get; set; }
        //public virtual Profile ProfileIdB { get; set; }
        public virtual Profile ProfileA { get; set; }
        public virtual Profile ProfileB { get; set; }
    }
}