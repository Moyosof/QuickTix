namespace QuickTix.API.Entities.Auth
{
    public class PasswordReset
    {
        public string UserEmail { get; set; }
        public string UserOTP { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; } 
    }
}
