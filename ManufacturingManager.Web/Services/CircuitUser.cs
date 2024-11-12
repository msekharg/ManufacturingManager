namespace ManufacturingManager.Web.Services
{
    public class CircuitUser
    {
        //    public string UserId { get; set; }
        //     public string CircuitId { get; set; }
        public int UserId { get; set; } = 0;
        public string UserName { get; set; } = "";
        public string? Role { get; set; } = "";
        public string CircuitId { get; set; }
    }
}
