﻿namespace ManufacturingManager.Web.Services
{
    public class CurrentUser
    {
        public int UserId { get;  set; } = 0;
        public string UserName { get;  set; } = "";
        public string? Role { get;  set; } = "";
        public string CircuitId { get; set; }
    }
}
