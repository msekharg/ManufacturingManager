namespace ManufacturingManager.Web.Data
{
    public class UserLogged 

    {
        public int UserId { get; set; }
        public string LoginName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Role { get; set; } = null!; //Define role for user when login

    }
}
