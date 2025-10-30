using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filminurk.Core.Domain
{
    public class Actor
    {
        [Key]
        public Guid ActorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public List<string> MoviesActedFor { get; set; }
        public Guid PortraitID { get; set; }
        /* 3 enda mõeldud andmetüüpi */
        public decimal ActorRating { get; set; }
        public string MovieKnownFor { get; set; }
        public Gender Gender { get; set; }
    }
}
