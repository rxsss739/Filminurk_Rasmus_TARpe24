using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Filminurk.Controllers
{
    public class MoviesController : Controller
    {
        private readonly FilminurkTARpe24Context _context; 
        private readonly IMovieServices _movieServices;

        public MoviesController(FilminurkTARpe24Context context, IMovieServices movieServices) 
        {
            _context = context;
            _movieServices = movieServices;
        }

        public IActionResult Index()
        {
            var result = _context.Movies.Select(x => new MoviesIndexViewModel
            {
                ID = x.ID,
                Title = x.Title,
                FirstPublished = x.FirstPublished,
                CurrentRating = x.CurrentRating,
                CountryFilmedIn = x.CountryFilmedIn,
                Genre = x.Genre,
            });
            return View(result);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            MoviesCreateUpdateViewModel result = new();
            return View("Create", result);
        }

        [HttpPost, ActionName("Create")]
        public async Task<IActionResult> Create(MoviesCreateUpdateViewModel vm)
        {
            if (vm == null) { return NotFound(); } 

            var dto = new MoviesDTO()
            {
                ID = vm.ID,
                Title = vm.Title,
                Description = vm.Description,
                FirstPublished = vm.FirstPublished,
                CurrentRating = vm.CurrentRating,
                Director = vm.Director,
                Actors = vm.Actors,
                TimesShown = vm.TimesShown,
                CountryFilmedIn = vm.CountryFilmedIn,
                Genre = vm.Genre,
                EntryCreatedAt = vm.EntryCreatedAt,
                EntryModifiedAt = vm.EntryModifiedAt,
            };

            var result = await _movieServices.Create(dto);
            if (result == null) { return RedirectToAction(nameof(Index)); }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var movie = await _movieServices.DetailsAsync(id);

            if (movie == null) { return NotFound(); }

            var vm = new MoviesCreateUpdateViewModel();
            vm.ID = movie.ID;
            vm.Title = movie.Title;
            vm.Description = movie.Description;
            vm.Genre = movie.Genre;
            vm.FirstPublished = movie.FirstPublished;
            vm.CountryFilmedIn = movie.CountryFilmedIn;
            vm.EntryCreatedAt = movie.EntryCreatedAt;
            vm.EntryModifiedAt = movie.EntryModifiedAt;
            vm.CurrentRating = movie.CurrentRating;
            vm.Director = movie.Director;
            vm.Actors = movie.Actors;
            vm.TimesShown = movie.TimesShown;

            return View("CreateUpdate", vm);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var movie = await _movieServices.DetailsAsync(id);

            if (movie == null) { return NotFound(); }

            var vm = new MoviesDeleteViewModel();
            vm.ID = movie.ID;
            vm.Title = movie.Title;
            vm.Description = movie.Description;
            vm.Genre = movie.Genre;
            vm.FirstPublished = movie.FirstPublished;
            vm.CountryFilmedIn = movie.CountryFilmedIn;
            vm.EntryCreatedAt = movie.EntryCreatedAt;
            vm.EntryModifiedAt = movie.EntryModifiedAt;
            vm.CurrentRating = movie.CurrentRating;
            vm.Director = movie.Director;
            vm.Actors = movie.Actors;
            vm.TimesShown = movie.TimesShown;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var movie = await _movieServices.Delete(id);
            if (movie == null) { return NotFound(); }
            return RedirectToAction(nameof(Index));
        }
    }
}
