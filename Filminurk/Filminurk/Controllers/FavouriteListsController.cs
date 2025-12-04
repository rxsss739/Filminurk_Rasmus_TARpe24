using Filminurk.ApplicationServices.Services;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.FavouriteLists;
using Filminurk.Models.Movies;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class FavouriteListsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IFavouriteListsServices _favouriteListsServices;
        //fileservice add later

        public FavouriteListsController(FilminurkTARpe24Context context, IFavouriteListsServices favouriteListsServices)
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
                    ListDeletedAt = x.ListDeletedAt,
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
            newListDto.ListDeletedAt = vm.ListDeletedAt;
            newListDto.ListBelongsToUser = "00000000-0000-0000-0000-000000000001";

            var listofmoviestoadd = new List<Movie>();
            foreach (var movieId in tempParse)
            {
                Movie thismovie = (Movie)_context.Movies.Where(tm => tm.ID == movieId).ToArray().Take(1);
                listofmoviestoadd.Add(thismovie);
            }

            //List<Guid> convertedIDs = new List<Guid>();
            //if (newListDto.ListOfMovies != null)
            //{
            //    convertedIDs = MovieToId(newListDto.ListOfMovies);
            //}
            var newList = await _favouriteListsServices.Create(newListDto/*, convertedIDs*/);
            if (newList == null)
            {
                return BadRequest();
            }
            return RedirectToAction("Index", vm);
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(Guid id, Guid thisuserid)
        {
            if (id == null || thisuserid == null)
            {
                return BadRequest();
            }
            var thisList = _context.FavouriteLists
                .Where(tl => tl.FavouriteListID == id && tl.ListBelongsToUser == thisuserid.ToString())
                .Select
                (
                stl => new FavouriteListUserDetailsViewModel
                {
                    FavouriteListID = stl.FavouriteListID,
                    ListBelongsToUser = stl.ListBelongsToUser,
                    IsMovieOrActor = stl.IsMovieOrActor,
                    ListName = stl.ListName,
                    ListDescription = stl.ListDescription,
                    IsPrivate = stl.IsPrivate,
                    ListOfMovies = stl.ListOfMovies,
                    IsReported = stl.IsReported,
                    ListDeletedAt = stl.ListDeletedAt
                    //Image = _context.FilesToDatabase
                    //    .Where(i => i.ListID == stl.FavouriteListID)
                    //    .Select(si => new FavouriteListIndexImageViewModel
                    //    {
                    //        ImageID = si.ImageID,
                    //        ListID = si.ListID,
                    //        ImageData = si.ImageData,
                    //        ImageTitle = si.ImageTitle,
                    //        Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(si.ImageData))
                    //    }).ToList()
                }).First();
            //add vd atr here later, for checking if user&admin

            if (thisList == null)
            {
                return NotFound();
            }

            return View("Details", thisList);
        }

        [HttpPost]
        public async Task<IActionResult> UserTogglePrivacy(Guid id)
        {
            FavouriteList thisList = await _favouriteListsServices.DetailsAsync(id);

            if (thisList == null) { return NotFound(); }

            thisList.IsPrivate = !thisList.IsPrivate;
            thisList.ListModifiedAt = DateTime.Now;

            await _favouriteListsServices.Update(thisList);
            return RedirectToAction("UserDetails", new { id = thisList.FavouriteListID, thisuserid = Guid.Parse(thisList.ListBelongsToUser) });
        }

        [HttpPost]
        public async Task<IActionResult> UserDelete(Guid id)
        {
            FavouriteList thisList = await _favouriteListsServices.DetailsAsync(id);

            if (thisList == null) { return NotFound(); }
            if (thisList.ListDeletedAt != null) { return BadRequest(); }

            thisList.ListModifiedAt = DateTime.Now;
            thisList.ListDeletedAt = DateTime.Now;

            await _favouriteListsServices.Update(thisList);
            return RedirectToAction("UserDetails", new { id = thisList.FavouriteListID, thisuserid = Guid.Parse(thisList.ListBelongsToUser) });
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