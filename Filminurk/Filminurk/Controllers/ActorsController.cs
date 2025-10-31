using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.Actors;
using Filminurk.Models.UserComments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Filminurk.Controllers
{
    public class ActorsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IActorServices _actorServices;

        public ActorsController(FilminurkTARpe24Context context, IActorServices actorServices)
        {
            _context = context;
            _actorServices = actorServices;
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
                    Gender = (Genders)a.Gender
                }
            );
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ActorsCreateViewModel result = new ActorsCreateViewModel();
            return View(result);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create(ActorsCreateViewModel vm)
        {
            if (!ModelState.IsValid) { return NotFound(); }

            var dto = new ActorDTO()
            {
                ActorID = vm.ActorID,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                NickName = vm.NickName,
                MovieKnownFor = vm.MovieKnownFor,
                Gender = (Core.Domain.Genders)vm.Gender,
                ActorRating = vm.ActorRating,
                MoviesActedFor = vm.MoviesActedFor,
                PortraitID = vm.PortraitID,
                EntryCreatedAt = vm.EntryCreatedAt,
                EntryModifiedAt = vm.EntryModifiedAt,
            };

            var result = await _actorServices.Create(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id) 
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(x => x.ActorID == id);

            var vm = new ActorsDeleteViewModel();
            vm.ActorID = actor.ActorID;
            vm.FirstName = actor.FirstName;
            vm.LastName = actor.LastName;
            vm.NickName = actor.NickName;
            vm.MoviesActedFor = actor.MoviesActedFor;
            vm.PortraitID = actor.PortraitID;
            vm.Gender = (Genders)actor.Gender;
            vm.MovieKnownFor = actor.MovieKnownFor;
            vm.ActorRating = actor.ActorRating;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            var actor = await _actorServices.Delete(id);
            if (actor == null) { return NotFound(); }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(x => x.ActorID == id);

            var vm = new ActorsDetailsViewModel();
            vm.ActorID = actor.ActorID;
            vm.FirstName = actor.FirstName;
            vm.LastName = actor.LastName;
            vm.NickName = actor.NickName;
            vm.MoviesActedFor = actor.MoviesActedFor;
            vm.PortraitID = actor.PortraitID;
            vm.Gender = (Genders)actor.Gender;
            vm.ActorRating = actor.ActorRating;
            vm.MovieKnownFor = actor.MovieKnownFor;

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var actor = await _context.Actors.FirstOrDefaultAsync(x => x.ActorID == id);

            var vm = new ActorsUpdateViewModel();
            vm.ActorID = actor.ActorID;
            vm.FirstName = actor.FirstName;
            vm.LastName = actor.LastName;
            vm.NickName = actor.NickName;
            vm.MoviesActedFor = actor.MoviesActedFor;
            vm.PortraitID = actor.PortraitID;
            vm.Gender = (Genders)actor.Gender;
            vm.ActorRating = actor.ActorRating;
            vm.MovieKnownFor = actor.MovieKnownFor;
            vm.EntryCreatedAt = actor.EntryCreatedAt;
            vm.EntryModifiedAt = actor.EntryModifiedAt;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ActorsUpdateViewModel vm)
        {
            if (!ModelState.IsValid) { return NotFound(); }

            var dto = new ActorDTO()
            {
                ActorID = vm.ActorID,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                NickName = vm.NickName,
                MovieKnownFor = vm.MovieKnownFor,
                Gender = (Core.Domain.Genders)vm.Gender,
                ActorRating = vm.ActorRating,
                MoviesActedFor = vm.MoviesActedFor,
                PortraitID = vm.PortraitID,
                EntryCreatedAt = vm.EntryCreatedAt,
                EntryModifiedAt = DateTime.Now,
            };

            var result = await _actorServices.Update(dto);
            return RedirectToAction(nameof(Index));
        }
    }
}
