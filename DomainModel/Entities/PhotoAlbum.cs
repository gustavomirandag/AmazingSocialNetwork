using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class PhotoAlbum : EntityBase
    {
        public virtual Profile Owner { get; set; } //Dono de um determinado album
        public string Title { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public PhotoAlbum()
        {
            Photos = new HashSet<Photo>();
        }
    }
}
