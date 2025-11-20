using Filminurk.ApplicationServices.Services;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Data;
using Filminurk.Models.FavouriteLists;
using Filminurk.Models.Movies;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class FavouriteListsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly FavouriteListsServices _favouriteListsServices;
        //fileservice add later

        public FavouriteListsController(FilminurkTARpe24Context context, FavouriteListsServices favouriteListsServices)
        {
            _context = context;
            _favouriteListsServices = favouriteListsServices;
        }

        public IActionResult Index()
        {
            var resultingLists = _context.FavouriteLists
                .OrderByDescending(y => y.ListCreatedAt)
                .Select(x => new FavouriteListsIndexViewModel
                {
                    FavouriteListID = x.FavouriteListID,
                    ListBelongsToUser = x.ListBelongsToUser,
                    IsMovieOrActor = x.IsMovieOrActor,
                    ListName = x.ListName,
                    ListDescription = x.ListDescription,
                    ListCreatedAt = x.ListCreatedAt,
                    Image = (List<FavouriteListIndexImageViewModel>)_context.FilesToDatabase
                        .Where(ml => ml.ListID == x.FavouriteListID)
                        .Select(li => new FavouriteListIndexImageViewModel
                        {
                            ImageID = li.ImageID,
                            ListID = li.ListID,
                            ImageData = li.ImageData,
                            ImageTitle = li.ImageTitle,
                            Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(li.ImageData))
                        })

                });
            return View(resultingLists);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var movies = _context.Movies
                .ToList()
                .OrderBy(m => m.Title)
                .Select(mo => new MoviesIndexViewModel
                {
                    ID = mo.ID,
                    Title = mo.Title,
                    FirstPublished = mo.FirstPublished,
                    Genre = mo.Genre,
                });

            ViewData["allMovies"] = movies;
            ViewData["userHasSelected"] = new List<string>();
            FavouriteListsUserCreateViewModel vm = new();
            return View("UserCreate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> UserCreate(
            FavouriteListsUserCreateViewModel vm, 
            List<string> userHasSelected, 
            List<MoviesIndexViewModel> movies
            )
        {
            List<Guid> tempParse = new();
            foreach (var stringID in userHasSelected)
            {
                tempParse.Add(Guid.Parse(stringID));
            }

            var newListDto = new FavouriteListDTO() { };
            newListDto.ListName = vm.ListName;
            newListDto.ListDescription = vm.ListDescription;
            newListDto.IsMovieOrActor = vm.IsMovieOrActor;
            newListDto.IsPrivate = vm.IsPrivate;
            newListDto.ListCreatedAt = DateTime.UtcNow;
            newListDto.ListModifiedAt = DateTime.UtcNow;
            newListDto.ListDeletedAt = (DateTime)vm.ListDeletedAt;
            newListDto.ListBelongsToUser = "00000000-0000-0000-0000-000000000001";
            List<Guid> convertedIDs = new List<Guid>();
            if (newListDto.ListOfMovies != null)
            {
                convertedIDs = MovieToId(newListDto.ListOfMovies);
            }
            var newList = await _favouriteListsServices.Create(newListDto, convertedIDs);
            if (newList != null)
            {
                return BadRequest();
            }
            return RedirectToAction("Index", vm);
        }

        private List<Guid> MovieToId(List<Movie> listOfMovies)
        {
            var result = new List<Guid>();
            foreach (var movie in listOfMovies)
            {
                result.Add(movie.ID);
            }
            return result;
        }
    }
}