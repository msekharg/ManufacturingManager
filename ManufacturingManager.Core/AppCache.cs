using ManufacturingManager.Core.Models;

namespace ManufacturingManager.Core
{
    public class AppCache
    {

        public static IList<ColorCodeMatrix> ColorCodeMatrices { get; set; }
        public static IList<ClampsPositioning> ClampsPositionings { get; set; }
        public static IList<MidRailConfiguration> ClampsPositioningConfigurations { get; set; }
        public static IList<UserRole> UserRoles { get; set; }
        public static IList<User> Users { get; set; }
        public static IList<CurrentFTCConfiguration> CurrentFTCConfigurations { get; set; }
    }

}
