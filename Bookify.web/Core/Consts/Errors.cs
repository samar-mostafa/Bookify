namespace Bookify.web.Core.Consts
{
    public static class Errors
    {
        public const string MaxLength = "Length cannot be more than {1} characters! ";
        public const string Dublicated = "value with the same name is allready Exist!";
        public const string AllowedImageExtensions = "only extensions of .jpg , .jpeg ,.png allowed !";
        public const string AllowedImageSize = "file cannot be more than 2 MB!";
        public const string AllowedDate = "Date can not be in future";
        public const string DublicatedBook = "book with this title is allready exist with this author";
    }
}
