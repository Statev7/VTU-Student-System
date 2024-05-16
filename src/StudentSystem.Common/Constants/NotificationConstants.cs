namespace StudentSystem.Common.Constants
{
    public static class NotificationConstants
    {
        public const string SuccessNotification = "Success";

        public const string ErrorNotification = "Error";

        public const string ErrorMesage = "An error occurred!";

        // Users

        public const string InvalidUserErrorMessage = "Such a user not exist!";

        public const string SuccessfullyCreatedTeacherMessage = $"Successfully created a new teacher!";

        public const string AlreadyATeacherErrorMessage = "This user is already a teacher!";

        // Students

        public const string SuccessfullyAppliedMessage = "You have successfully applied!";

        public const string AlreadyAppliedErrorMessage = "You are already applied!";

        public const string InvalidStudentErrorMessage = "Such a student not exist!";

        public const string SuccesfullyAprovedOperationMessage = "Response send!";

        // Courses

        public const string UnsuccessfullyCourseCreationErrorMessage = "Unsuccessfully creation!";

        public const string SuccessfullyCreatedCourseMessage = $"Successfully created a new course!";

        public const string SuccessfillyUpdatedCourseMessage = $"Successfully updated!";

        public const string SuccessfillyDeletedCourseMessage = $"Successfully deleted!";

        public const string InvalidCourseErrorMessage = "Such a course not exist!";

        public const string StudentAlreadyInCourseMessage = "You are already in this course!";

        // Payments 

        public const string SuccessfullyPaymentMessage = "The payment was successful!";

        public const string UnsuccessfullyPaymentMessage = "The payment was unsuccessful!";
    }
}
