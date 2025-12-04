using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Microsoft.EntityFrameworkCore;

namespace Filminurk.ApplicationServices.Services
{
    public class FavouriteListsServices : IFavouriteListsServices
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IFilesServices _filesServices;

        public FavouriteListsServices(FilminurkTARpe24Context context, IFilesServices filesServices)
        {
            _context = context;
            _filesServices = filesServices;
        }

        public async Task<FavouriteList> DetailsAsync(Guid id)
        {
            var result = await _context.FavouriteLists
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.FavouriteListID == id);
            return result;
        }

        public async Task<FavouriteList> Create(FavouriteListDTO dto/*, List<Movie> selectedMovies*/)
        {
            FavouriteList newList = new();
            newList.FavouriteListID = Guid.NewGuid();
            newList.ListName = dto.ListName;
            newList.ListDescription = dto.ListDescription;
            newList.ListCreatedAt = dto.ListCreatedAt;
            newList.ListModifiedAt = dto.ListModifiedAt;
            newList.ListDeletedAt = dto.ListDeletedAt;
            newList.ListOfMovies = dto.ListOfMovies;
            newList.ListBelongsToUser = dto.ListBelongsToUser;
            await _context.FavouriteLists.AddAsync(newList);
            await _context.SaveChangesAsync();
            //foreach (var movieId in selectedMovies)
            //{
            //    _context.FavouriteLists.Entry
            //}
            return newList;
        }

        public async Task<FavouriteList> Update(FavouriteList updatedList)
        {
            FavouriteList modifiedList = new();

            modifiedList.FavouriteListID = updatedList.FavouriteListID;
            modifiedList.ListBelongsToUser = updatedList.ListBelongsToUser;
            modifiedList.IsMovieOrActor = updatedList.IsMovieOrActor;
            modifiedList.ListName = updatedList.ListName;
            modifiedList.ListDescription= updatedList.ListDescription;
            modifiedList.IsPrivate = updatedList.IsPrivate;
            modifiedList.ListOfMovies= updatedList.ListOfMovies;
            modifiedList.ListOfActors= updatedList.ListOfActors;
            modifiedList.ListCreatedAt = updatedList.ListCreatedAt;
            modifiedList.ListModifiedAt = updatedList.ListModifiedAt;
            modifiedList.ListDeletedAt = updatedList.ListDeletedAt;
            modifiedList.IsReported = updatedList.IsReported;

            _context.FavouriteLists.Update(modifiedList);
            await _context.SaveChangesAsync();

            return modifiedList;
        }
    }
}
