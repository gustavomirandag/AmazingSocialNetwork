using DomainModel.ObjectValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Entities
{
    public class PhotoGallery
    {
        public Guid Id { get; set; }
        public virtual ICollection<Photo> PhotosUrls { get; set; }
    }
}
