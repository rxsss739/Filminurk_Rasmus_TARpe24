using Filminurk.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.Actors
{
    public class ActorsCreateViewModel
    {
        [Key]
        public Guid ActorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public List<string>? MoviesActedFor { get; set; }
        public Guid? PortraitID { get; set; }
        /* 3 enda mõeldud andmetüüpi */
        public decimal ActorRating { get; set; }
        public string MovieKnownFor { get; set; }
        public Genders Gender { get; set; }
    }
}
