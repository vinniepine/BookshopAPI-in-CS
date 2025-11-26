namespace BookshopAPI.Domain.Enums
{
    public class Genres
    {
        public static readonly HashSet<string> ValidGenres = new HashSet<string>
        {
            "ficção",
            "romance",
            "mistério",
            "terror",
            "fantasia",
            "não-ficção",
            "biografia",   
        };

        public static bool IsValid(string genre)
        {
            return ValidGenres.Contains(genre.Trim().ToLower());
        }
    }
}
