namespace StudentSystem.Common.Constants
{
    public static class GlobalConstants
    {
        //Roles

        public const string AdminRole = "Admin";
        public const string StudentRole = "Student";
        public const string TeacherRole = "Teacher";
        public const string GuestRole = "Guest";

        //Users

        public const string AdminPassword = "admin@admin.admin";

        //Email

        public const string EmailSender = "petiostatev2703@gmail.com";
        public const string EmailSenderName = "StudentSystem";

        public const string ConfirmSubject = "Confirm your email";
        public const string ConfirmMessage = "Please confirm your account by <a href='{0}'>clicking here</a>.";

        public const string ApplicationResultSubject = "Result of your application - StudentSystem";
        public const string StudentApprovedMessage = "Congratulations, you have been approved as a student";
        public const string StudentNotApprovedMessage = "Unfortunately, your application has been rejected. But you can send a new one!";
    }
}
