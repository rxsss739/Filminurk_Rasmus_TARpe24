using Filminurk.Data;
using Filminurk.Models.Actors;
using Filminurk.Models.UserComments;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class ActorsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;

        public ActorsController(FilminurkTARpe24Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var result = _context.Actors
                .Select(a => new ActorsIndexViewModel
                {
                    ActorID = a.ActorID,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    NickName = a.NickName,
                    MoviesActedFor = a.MoviesActedFor,
                    PortraitID = a.PortraitID,
                    ActorRating = a.ActorRating,
                    MovieKnownFor = a.MovieKnownFor,
                    Gender = a.Gender
                }
            );
            return View(result);
        }
    }
}
