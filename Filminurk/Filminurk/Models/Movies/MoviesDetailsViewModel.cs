﻿namespace Filminurk.Models.Movies
{
    public class MoviesDetailsViewModel
    {
        public Guid? ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly FirstPublished { get; set; }
        public string Director { get; set; }
        public List<string>? Actors { get; set; }
        public double? CurrentRating { get; set; }
        //public List<UserComment>? Reviews { get; set; }
        /* Kaasasolevate piltide andmeomadused */
        public List<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();

        /* 3 andmetüüpi */
        public int? TimesShown { get; set; }
        public string? CountryFilmedIn { get; set; }
        public string? Genre { get; set; }

        /* */
        public DateTime? EntryCreatedAt { get; set; }
        public DateTime? EntryModifiedAt { get; set; }
    }
}
