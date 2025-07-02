namespace BLL.Helpers;

public static class EmailBodyHelper
{
    // forgot password email subject and body
    public static string ForgotPasswordSubject = "Password Reset Request";
    public static string ForgotPasswordBody(string link)
    {
        return $@"<div style='width:50%;'>
                <div style='display: flex;justify-content:center;align-items: center;background-color: #167BBC;padding:20px;gap:5px;'>
                    <h2 style='color: white;'>AMS</h2>
                </div>
                <div style='padding: 20px;'>
                <p>Attendance Management System,</p>
                <p>Please click <a href={link}>
                here</a> for reset your account Password.</p>
                <p>If you encounter any issues or have any question,Please do not hesitate to contact our support team</p>
                <p><span style='color:orange'>Important Note:</span> For security reasons,the link will expire in 24 hours. If you did not request a password reset, Please ignore this email or contact our support team immediately.</p>
                </div>
                </div>";
    }

    // new user created email subject and body
    public static string UpsertUserSubject = "Update Password Request";
    public static string UpsertUserBody(string link)
    {
        return $@"
                <div style='width:50%;'>
                <div style='display: flex;justify-content:center;align-items: center;background-color: #167BBC;padding:20px;gap:5px;'>
                <h2 style='color: white;'>AMS</h2>
                </div>
                <div style='padding: 20px;'>
                <p>Welcome to Attendance Management System</p>
                <p>Please find the details below for login into your account.</p>
                <div style='border:2px solid;padding:5px;'>
                <p>Please click <a href={link}>
                here</a> and reset your password.</p>
                </div>
                <p>If you encounter any issues or have any question,Please do not hesitate to contact our support team</p>
                </div>
                </div>";
    }
}
