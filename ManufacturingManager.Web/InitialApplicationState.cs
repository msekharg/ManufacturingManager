namespace ManufacturingManager.Web
{
    public class InitialApplicationState
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; } = null!;
        public string? IpAddress { get; set; }
    }
}
