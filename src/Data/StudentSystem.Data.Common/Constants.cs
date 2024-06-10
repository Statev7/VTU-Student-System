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

            public const int TeaserDescriptionMaxLength = 200;

            public const int DescriptionMaxLength = 50000;

            public const int ImageFolderMaxLength = 512;
        }

        public static class ImageFile
        {
            public const int FolderMaxLength = 1024;
        }

        public static class Payment
        {
            public const int SessionMaxLength = 1024;
        }

        public static class Lesson
        {
            public const int NameMaxLength = 256;

            public const int DescriptionMaxLength = 2000;
        }

        public static class Resources
        {
            public const int NameMaxLength = 64;

            public const int FolderPathMaxLength = 2048;

            public const int ExtansionMaxLength = 16;
        }
    }
}
