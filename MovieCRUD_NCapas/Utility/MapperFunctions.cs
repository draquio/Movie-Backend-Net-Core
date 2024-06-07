using AutoMapper;
using MovieCRUD_NCapas.Models;
using System.Globalization;

namespace MovieCRUD_NCapas.Utility
{
    public class MapperFunctions
    {
        public string FormatDuration(int TotalMinutes)
        {
            int hours = TotalMinutes / 60;
            int minutes = TotalMinutes % 60;
            return $"{hours}h {minutes}m";
        }
        public string CalculateRating(ICollection<Review> reviews)
        {
            if (reviews == null || !reviews.Any())
            {
                return "No data";
            }
            double ratingAvarage = reviews.Average(review => review.Rating);
            return $"{ratingAvarage}/5";
        }

        public DateTime DateTimeFormat(string date)
        {
            return DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
    }
}
