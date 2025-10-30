using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Data;

namespace Filminurk.ApplicationServices.Services
{
    public class ActorServices
    {
        FilminurkTARpe24Context _context;
        public ActorServices(FilminurkTARpe24Context context)
        {
            _context = context;
        }

        public async Task<Actor> Create(ActorDTO dto)
        {
            Actor actor = new Actor();
            actor.ActorID = Guid.NewGuid();
            actor.FirstName = dto.FirstName;
            actor.LastName = dto.LastName;
            actor.NickName = dto.NickName;
            actor.PortraitID = dto.PortraitID;
            actor.MoviesActedFor = dto.MoviesActedFor;
            actor.ActorRating = dto.ActorRating;
            actor.Gender = dto.Gender;
            actor.MovieKnownFor = dto.MovieKnownFor;

            await _context.Actors.AddAsync(actor);
            await _context.SaveChangesAsync();

            return actor;
        }
    }
}
