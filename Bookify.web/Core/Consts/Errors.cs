namespace Bookify.web.Core.Consts
{
    public static class Errors
    {
        public const string MaxLength = "Length cannot be more than {1} characters! ";
        public const string Dublicated = "Another record with the same {0} is allready Exist!";
        public const string AllowedImageExtensions = "only extensions of .jpg , .jpeg ,.png allowed !";
        public const string AllowedImageSize = "file cannot be more than 2 MB!";
        public const string AllowedDate = "Date can not be in future";
        public const string DublicatedBook = "book with this title is allready exist with this author";
        public const string InvalidRange = "{0} shoud be between {1} and{2}";
        public const string MaxMinLength = "The {0} must be at least {2} and at max {1} characters long.";
        public const string ConfirmPassword = "The password and confirmation password do not match.";
        public const string WeekPassword = "The  password must contains an uppercase character, lowercase character, a digit, and a non-alphanumeric character. Password must be at least 8 characters long.";
        public const string InvalidUsername = "Username is invalid, can only contain letters or digits.";
        public const string OnlyEnglishLetters = "Only English letters are allowed.";
        public const string OnlyArabicLetters = "Only Arabic letters are allowed.";
        public const string OnlyNumbersAndLetters = "Only Arabic/English letters or digits are allowed.";
        public const string DenySpecialCharacters = "Special characters are not allowed.";
        public const string InvaildCheckPassword = "This password is not correct";
    }
}
