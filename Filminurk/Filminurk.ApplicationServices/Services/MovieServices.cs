﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.ServiceInterface;
using Filminurk.Core.Dto;
using Filminurk.Data;
using Microsoft.EntityFrameworkCore;

namespace Filminurk.ApplicationServices.Services
{
    public class MovieServices : IMovieServices
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IFilesServices _filesServices;

        public MovieServices(FilminurkTARpe24Context context, IFilesServices filesServices)
        {
            _context = context;
            _filesServices = filesServices;
        }

        public async Task<Movie> Create(MoviesDTO dto)
        {
            Movie movie = new Movie();
            movie.ID = Guid.NewGuid();
            movie.Title = dto.Title;
            movie.Description = dto.Description;
            movie.CurrentRating = dto.CurrentRating;
            movie.Actors = dto.Actors;
            movie.Director = dto.Director;
            movie.CountryFilmedIn = dto.CountryFilmedIn;
            movie.FirstPublished = (DateOnly)dto.FirstPublished;
            movie.Genre = dto.Genre;
            movie.TimesShown = dto.TimesShown;
            movie.EntryCreatedAt = DateTime.Now;
            movie.EntryModifiedAt = DateTime.Now;
            _filesServices.FilesToApi(dto, movie);

            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        public async Task<Movie> DetailsAsync(Guid id)
        {
            var result = await _context.Movies.FirstOrDefaultAsync(x => x.ID == id);
            return result;
        }

        public async Task<Movie> Update(MoviesDTO dto)
        {
            Movie movie = new Movie();
            movie.ID = (Guid)dto.ID;
            movie.Title = dto.Title;
            movie.Description = dto.Description;
            movie.CurrentRating = dto.CurrentRating;
            movie.Actors = dto.Actors;
            movie.Director = dto.Director;
            movie.CountryFilmedIn = dto.CountryFilmedIn;
            movie.FirstPublished = (DateOnly)dto.FirstPublished;
            movie.Genre = dto.Genre;
            movie.TimesShown = dto.TimesShown;
            movie.EntryCreatedAt = DateTime.Now;
            movie.EntryModifiedAt = DateTime.Now;
            _filesServices.FilesToApi(dto, movie);

            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<Movie> Delete(Guid id)
        {
            var result = await _context.Movies.FirstOrDefaultAsync(x => x.ID == id);

            var images = await _context.FilesToApi
                .Where(x => x.MovieID == id)
                .Select(y => new FileToApiDTO
                {
                    ImageID = y.ImageID,
                    MovieID = y.MovieID,
                    FilePath = y.ExistingFilePath
                }).ToArrayAsync();

            _context.Movies.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
