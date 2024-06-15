namespace StudentSystem.Common.Constants
{
    public static class NotificationConstants
    {
        public const string SuccessNotification = "Success";

        public const string ErrorNotification = "Error";

        public const string ErrorMesage = "An error occurred!";

        // Users

        public const string InvalidUserErrorMessage = "Such a user not exist!";

        public const string AlreadyATeacherErrorMessage = "This user is already a teacher!";

        // Students

        public const string SuccessfullyAppliedMessage = "You have successfully applied!";

        public const string AlreadyAppliedErrorMessage = "You are already applied!";

        public const string InvalidStudentErrorMessage = "Such a student not exist!";

        public const string SuccesfullyAprovedOperationMessage = "Response successfully sent!";

        // Courses

        public const string UnsuccessfullyCourseCreationErrorMessage = "Unsuccessfully creation!";

        public const string InvalidCourseErrorMessage = "Such a course not exist!";

        public const string StudentAlreadyInCourseMessage = "You are already in this course!";

        public const string NotActiveCourseErrorMessage = "This course is already started or is not active!";

        // Payments 

        public const string SuccessfullyPaymentMessage = "The payment was successful!";

        public const string UnsuccessfullyPaymentMessage = "The payment was unsuccessful!";

        // Lessons 

        public const string InvalidDatesErrorMessage = "The lesson start time cannot be earlier than the course start date, and the lesson end time cannot be later than the course end date";

        public const string InvalidLessonErrorMessage = "Such a lesson not exist!";

        // StudentCourses

        public const string EmailNotificationMessage = "The email was sent to {0} students.";

        public const string StudentCourseRelationNotFoundErrorMessage = "The relationship between the student and the course was not found!";

        // Files

        public const string EmptyFileErrorMessage = "The file cannot be empty!";

        public const string PotentialVirusInFileErrorMessage = "File contains potential viruses. Cannot continue with this file!";

        // Resources 

        public const string InvalidResourceErrorMessage = "Such a resource not exist!";

        public const string ResourceNotAccessErrorMessage = "You do not have access to this resource!";

        // Teacher

        public const string SuccessfullyCreateTeacher = "Successfully operation!";

        // Exam

        public const string SuccessfullyAssignedGradeMessage = "Grade successfully assigned";

        public const string StudentAlreadyHadGradeErrorMessage = "This student already has a grade!";

        public const string TeacherNotPermissionToAssignGradeErrorMessage = "You do not have permission to assign a grade!";

        // CRUD

        public const string SuccessfullyCreatedMessage = $"Successfully created!";

        public const string SuccessfullyUpdatedMessage = $"Successfully updated!";

        public const string SuccessfullyDeletedMessage = $"Successfully deleted!";

    }
}
