namespace Filminurk.Models.Movies
{
    public class MoviesIndexViewModel
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public DateOnly FirstPublished { get; set; }
        public double? CurrentRating { get; set; }

        /* 3 andmetüüpi */
        public string? CountryFilmedIn { get; set; }
        public string? Genre { get; set; }
    }
}
