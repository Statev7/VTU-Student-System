namespace StudentSystem.Data.Common
{
    public static class Constants
    {
        public static class ApplicationUser
        {
            public const int FirstNameMaxLength = 64;
            public const int FirstNameMinLength = 2;

            public const int LastNameMaxLength = 64;
            public const int LastNameMinLength = 2;

            public const int UsernameMaxLength = 64;
            public const int UsernameMinLength = 2;
        }

        public static class Teacher
        {
            public const int AboutMeMaxLength = 20000;
        }

        public static class City
        {
            public const int NameMaxLength = 128;
        }

        public static class Course
        {
            public const int NameMaxLength = 128;
            public const int NameMinLength = 2;

            public const int DescriptionMaxLength = 50000;
        }
    }
}
