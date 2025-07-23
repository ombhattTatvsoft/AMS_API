
namespace BLL.Constants
{
    public class Constant
    {
        // path
        public const string RESET_PASS_PATH = "auth/reset-password/";

        // Auth
        public const string WRONG_CRED = "Login Failed, Check Credentials!";
        public const string LOGIN_FAIL = "Login Failed, Please Try Again!";
        public const string LOGIN_SUCCESS = "Login Successful";
        public const string INVALID_RESET_LINK = "Invalid reset link";
        public const string EXPIRED_RESET_LINK = "Reset Link Expired";
        public const string RESET_PASSWORD_SUCCESS = "Reset Password Successful";

        // Email
        public const string EMAIL_SUCCESS = "Email sent successfully";
        public const string EMAIL_FAIL = "Email sent failed";

        // User
        public const string USER_NOT_FOUND = "User Not Found";
        public const string USER_EXISTS = "User with same email already exists";
        public const string USER_ADDED = "User added successfully";
        public const string USER_UPDATED = "User updated successfully";
        public const string USER_DELETED = "User deleted successfully";
        public const string ASSOCIATED_MANAGER = "Cannot delete user who is a manager of other users";
        public const string SAVE_USER_EMAIL_FAIL = "User created but email sending failed";
        public const string SAVE_USER_SUCCESS = "User created successfully and email sent";
        public const string PROFILE_UPDATED = "Profile updated successfully";

        // Department
        public const string DEPT_EXISTS = "Department with same name already exists";
        public const string DEPT_NOT_FOUND = "Department Not Found";
        public const string DEPT_ADDED = "Department added successfully";
        public const string DEPT_UPDATED = "Department updated successfully";
        public const string DEPT_WITH_USERS = "Cannot delete department with associated users";
        public const string DEPT_DELETED = "Department deleted successfully";

        // Dashboard 
        public const string PASSWORD_CHANGE_SUCCESS = "Password Changed Successfully";
        public const string PASSWORD_CHANGE_FAIL = "Password Change Failed, Please Check your Current Password";

        // Catch Exception fields

        // Auth
        public const string AUTH_USER_CATCH = "Error authenticating user with email: {Email}";
        public const string TOKEN_CREATE_CATCH = "Error creating JWT token for user: {UserId}";
        public const string EMAIL_LINK_CATCH = "Email reset link preparation failed";
        public const string EMAIL_SEND_CATCH = "Email sent failed for {Subject}";
        public const string CHECK_RESET_LINK_CATCH = "Error checking reset link for ID: {Id}";
        public const string USER_BY_RESET_CODE_CATCH = "Error finding user by reset code: {ResetCode}";

        // Department
        public const string GET_ALL_DEPT_CATCH = "Error occurred while getting all departments";
        public const string GET_DEPT_CATCH = "Error occurred while getting department with ID {Id}";
        public const string SAVE_DEPT_CATCH = "Error occurred while saving department";
        public const string DELETE_DEPT_CATCH = "Error deleting department with ID {DepartmentId}";

        //  User
        public const string GET_ALL_USERS_CATCH = "Error occurred while getting all users";
        public const string UPDATE_PASSWORD_CATCH = "Error updating password for user {UserId}";
        public const string USER_BY_EMAIL_CATCH = "Error finding user by email: {Email}";
        public const string GET_ALL_ROLES_CATCH = "Error occurred while getting all roles";
        public const string GET_USER_CATCH = "Error occurred while getting user with ID {Id}";
        public const string SAVE_USER_CATCH = "Error saving user with email {Email}";
        public const string DELETE_USER_CATCH = "Error deleting user with ID {UserId}";

    }
}