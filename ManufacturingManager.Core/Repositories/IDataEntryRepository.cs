using ManufacturingManager.Core.Models;

namespace ManufacturingManager.Core.Repositories
{
    public interface IDataEntryRepository
    {
        //Lookups
        IEnumerable<ClampsPositioning> GetClampsPositioning();
        IEnumerable<ColorCodeMatrix> GetColorCodeMatrix();
        IEnumerable<MidRailConfiguration> GetMidRailConfiguration();
        
        Task<IEnumerable<Dimension>> GetDimensionsDataView();
        Task<Dimension> GetDimensionById(int id);
        Task<bool> UpdateDimension(Dimension dimension);
        Task<int> InsertDimension(Dimension dimension);
        void DeleteDataEntry(int dimension);
        
        Task<IEnumerable<SolarInspection>> GetSolarInspectionsDataView();
        Task<SolarInspection> GetSolarInspectionById(int id);
        Task<bool> UpdateSolarInspection(SolarInspection dimension);
        Task<int> InsertSolarInspection(SolarInspection dimension);
        void DeleteSolarInspection(int solarInspectionId);
        
        Task<IEnumerable<InProcessCheck>> GetInProcessCheckDataView();
        Task<InProcessCheck> GetInProcessCheckById(int id);
        Task<bool> UpdateInProcessCheck(InProcessCheck dimension);
        Task<int> InsertInProcessCheck(InProcessCheck dimension);
        void DeleteInProcessCheck(int inProcessCheckId);
        Task<FinalInspection> GetFinalInspectionRecordById(int finalInspectionId);
        Task<IEnumerable<FinalInspection>> GetFinalInspectionsDataView();
        Task<bool> UpdateFinalInspection(FinalInspection dimension);
        Task<int> InsertFinalInspection(FinalInspection dimension);
        void DeleteFinalInspection(int finalInspectionId);
    }
}
