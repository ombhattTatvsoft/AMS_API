using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Constants
{
    public class Constant
    {
        public const string WRONG_CRED = "Login Failed, Check Credentials!";
        public const string LOGIN_FAIL = "Login Failed, Please Try Again!";
        public const string LOGIN_SUCCESS = "Login Successful";
        public const string EMAIL_SUCCESS = "Email sent successfully";
        public const string EMAIL_FAIL = "Email sent failed";
        public const string USER_NOT_FOUND = "User Not Found.";
        public const string INVALID_RESET_LINK = "Invalid reset link.";
        public const string EXPIRED_RESET_LINK = "Reset Link Expired.";

        // Catch Exception fields
        public const string AUTH_USER_FAIL = "Error authenticating user with email: {Email}";
        public const string TOKEN_CREATE_FAIL = "Error creating JWT token for user: {UserId}";
        public const string USER_BY_EMAIL_FAIL = "Error finding user by email: {Email}";
        public const string EMAIL_LINK_FAIL = "Email reset link preparation failed";
        public const string EMAIL_CATCH_FAIL = "Email sent failed for {Subject}";

    }
}