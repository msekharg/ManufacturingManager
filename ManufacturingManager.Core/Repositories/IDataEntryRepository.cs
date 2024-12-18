﻿using ManufacturingManager.Core.Models;

namespace ManufacturingManager.Core.Repositories
{
    public interface IDataEntryRepository
    {
        //Lookups
        IList<CurrentFTCConfiguration> GetCurrentFTCConfiguration(bool isActive);
        bool UpdateCurrentFTCConfiguration(CurrentFTCConfiguration currentFtcConfiguration);
        int InvalidateAllFTCConfigurations();
        CurrentFTCConfiguration InsertCurrentFTCConfiguration(CurrentFTCConfiguration currentFtcConfiguration);
        IEnumerable<ClampsPositioning> GetClampsPositioning();
        ClampsPositioning GetClampsPositioningById(int clampsPositioningId);
        Task<int> UpdateClampsPositioning(ClampsPositioning clampsPositioning);
        Task<int> InsertClampsPositioning(ClampsPositioning clampsPositioning);
        Task<int> DeleteClampsPositioning(ClampsPositioning clampsPositioning);
        IEnumerable<ColorCodeMatrix> GetColorCodeMatrix();
        ColorCodeMatrix GetColorCodeMatrixById(int colorCodeMatrixId);
        Task<bool> UpdateColorCode(ColorCodeMatrix colorCodeMatrix);
        Task<int> InsertColorCode(ColorCodeMatrix colorCodeMatrix);
        Task<int> DeleteColorCode(ColorCodeMatrix colorCodeMatrix);
        IEnumerable<MidRailConfiguration> GetMidRailConfiguration();
        MidRailConfiguration GetMidRailConfigurationById(int midRailConfigurationId);
        Task<int> UpdateMidRailConfiguration(MidRailConfiguration midRailConfiguration);
        Task<int> InsertMidRailConfiguration(MidRailConfiguration midRailConfiguration);
        Task<int> DeleteMidRailConfiguration(MidRailConfiguration midRailConfiguration);
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
