﻿using Dapper;
using ManufacturingManager.ADO;
using ManufacturingManager.Core.Models;
using Microsoft.Data.SqlClient;

namespace ManufacturingManager.Core.Repositories
{
    public class DataEntryRepository : IDataEntryRepository
    {
        //private readonly IConfigurationService Configuration;
        //private readonly CMRSAppSettings _appSettings;

        //public DataEntryRepository(IConfigurationService configuration, IOptions<CMRSAppSettings> appSettings)
        //{
        //    Configuration = configuration;
        //    _appSettings = appSettings.Value;
        //}

        //Lookups
        public IList<CurrentFTCConfiguration> GetCurrentFTCConfiguration(bool isActive = true)
        {
            IList<CurrentFTCConfiguration> currentFtcConfigurations = new List<CurrentFTCConfiguration>();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                if (AppCache.CurrentFTCConfigurations is { Count: > 0 })
                {
                    currentFtcConfigurations = AppCache.CurrentFTCConfigurations;
                }
                //string connString = Configuration.ChangeManagementConnectionString();
                using SqlConnection conn = new SqlConnection(connString);

                string strSelectCmd =
                    $"SELECT TOP 1000 CurrentFTCConfigurationId,PartNumber,AssemblyConfiguration,Height,Length,Thickness,IsActive,StartDateTime,EndDateTime, CreatedBy, CreatedDate FROM dbo.CurrentFTCConfiguration" + 
                    " WHERE IsActive=@IsActive";

                conn.Open();
                
                currentFtcConfigurations =  conn.QueryAsync<CurrentFTCConfiguration>(strSelectCmd, new{IsActive = isActive}).Result.ToList();
                AppCache.CurrentFTCConfigurations = currentFtcConfigurations;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return currentFtcConfigurations;
        }
        
        public bool UpdateCurrentFTCConfiguration(CurrentFTCConfiguration currentFtcConfiguration)
        {
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            string sql =
                "UPDATE dbo.CurrentFTCConfiguration SET " +
                "IsActive = 0, EndDateTime = @EndDateTime" +
                " WHERE CurrentFTCConfigurationId = @CurrentFTCConfigurationId";

            using SqlConnection conn = new(connString);
            conn.Open();
            var recordUpdated = conn.ExecuteAsync(sql, currentFtcConfiguration).Result;
            
            return recordUpdated > 0;
        }

        public int InvalidateAllFTCConfigurations()
        {
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            string sql =
                "UPDATE dbo.CurrentFTCConfiguration SET " +
                "IsActive = 0, EndDateTime = @EndDateTime" +
                " WHERE IsActive = 1";

            using SqlConnection conn = new(connString);
            conn.Open();
            var recordUpdated = conn.ExecuteAsync(sql, new{EndDateTime = DateTime.Now}).Result;

            return recordUpdated;
        }
        
        public CurrentFTCConfiguration InsertCurrentFTCConfiguration(CurrentFTCConfiguration currentFtcConfiguration)
        {
            currentFtcConfiguration.CreatedBy = "system";
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            var insertQuery =
                "INSERT INTO dbo.CurrentFTCConfiguration(PartNumber, AssemblyConfiguration, Height,Length, Thickness, IsActive, EndDateTime, CreatedBy)" +
                "VALUES (@PartNumber, @AssemblyConfiguration, @Height, @Length, @Thickness, @IsActive, @EndDateTime, @CreatedBy); SELECT SCOPE_IDENTITY();";
                

            using var conn = new SqlConnection(connString);
            conn.Open();
            var currentFtcConfigurationId = conn.ExecuteScalar<int>(insertQuery, currentFtcConfiguration);
            
            if (currentFtcConfigurationId > 0)
                AppCache.CurrentFTCConfigurations.Add(currentFtcConfiguration);
            
            return currentFtcConfiguration;
        }
        
        public IEnumerable<ClampsPositioning> GetClampsPositioning()
        {
            IEnumerable<ClampsPositioning> clampsPositionings = new List<ClampsPositioning>();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                //string connString = Configuration.ChangeManagementConnectionString();
                using SqlConnection conn = new SqlConnection(connString);

                string strSelectCmd =
                    $"SELECT TOP 1000 ClampsPositioningId,CXPX,CX,Clamp1,Clamp2,PX,Clamp3,Clamp4,HoleSetDrilled FROM dbo.ClampsPositioning";

                conn.Open();
                
                clampsPositionings =  conn.QueryAsync<ClampsPositioning>(strSelectCmd).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return clampsPositionings;
        }
        
        public ClampsPositioning GetClampsPositioningById(int clampsPositioningId)
        {
            ClampsPositioning clampsPositioning = new ClampsPositioning();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                //string connString = Configuration.ChangeManagementConnectionString();
                using SqlConnection conn = new SqlConnection(connString);

                string strSelectCmd =
                    $"SELECT TOP 1 ClampsPositioningId,CXPX,CX,Clamp1,Clamp2,PX,Clamp3,Clamp4,HoleSetDrilled FROM dbo.ClampsPositioning WHERE ClampsPositioningId=@clampsPositioningId";
    
                conn.Open();
                clampsPositioning =  conn.QueryAsync<ClampsPositioning>(strSelectCmd, new{clampsPositioningId = clampsPositioningId}).Result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return clampsPositioning;
        }

       public async Task<int> UpdateClampsPositioning(ClampsPositioning clampsPositioning)
        {
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            // colorCodeMatrix.UpdatedBy = dimension.InspectorName;
            // dimension.UpdatedDate = DateTime.Now;
            string sql =
                "UPDATE dbo.ClampsPositioning SET " +
                "CXPX = @CXPX, CX = @CX, Clamp1 = @Clamp1, Clamp2 = @Clamp2, PX = @PX, Clamp3 = @Clamp3,Clamp4 = @Clamp4, HoleSetDrilled = @HoleSetDrilled " +
                " WHERE ClampsPositioningId = @ClampsPositioningId";

            using SqlConnection conn = new(connString);
            conn.Open();
            int recordsUpdated = await conn.ExecuteAsync(sql, clampsPositioning);
            
            return recordsUpdated;
        }

        public async Task<int> InsertClampsPositioning(ClampsPositioning clampsPositioning)
        {
            // dimension.CreatedBy = dimension.InspectorName;
            // dimension.CreatedDate = DateTime.Now;
            // dimension.UpdatedBy = dimension.InspectorName; dimension.UpdatedDate = DateTime.Now;
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            var insertQuery =
                "INSERT INTO dbo.ClampsPositioning(ClampsPositioningId,CXPX,CX,Clamp1,Clamp2,PX,Clamp3,Clamp4,HoleSetDrilled) VALUES (@ClampsPositioningId,@CXPX,@CX,@Clamp1,@Clamp2,@PX,@Clamp3,@Clamp4,@HoleSetDrilled)";

            using var conn = new Microsoft.Data.SqlClient.SqlConnection(connString);
            conn.Open();
            var clampsPositioningId = conn.ExecuteScalar<int>(insertQuery, clampsPositioning);

            return await Task.FromResult(clampsPositioningId);
        }
        
        public async Task<int> DeleteClampsPositioning(ClampsPositioning clampsPositioning)
        {
            // user.UpdatedBy = dimension.InspectorName;
            // dimension.UpdatedDate = DateTime.Now;
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            const string query = @"DELETE from dbo.[ClampsPositioning] WHERE ClampsPositioningId = @ClampsPositioningId";

            await using SqlConnection conn = new(connString);
            conn.Open();
            int recordDeleted = await conn.ExecuteAsync(query, new
            {
                clampsPositioning.ClampsPositioningId
            });

            // if(recordDeleted > 0)
            //     UpdateCache(user, "DELETE");
            //
            return recordDeleted; 
        }

        public IEnumerable<ColorCodeMatrix> GetColorCodeMatrix()
        {
            IEnumerable<ColorCodeMatrix> colorCodeMatrices = new List<ColorCodeMatrix>();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                //string connString = Configuration.ChangeManagementConnectionString();
                using SqlConnection conn = new SqlConnection(connString);

                string strSelectCmd =
                    $"SELECT TOP 1000 ColorCodeMatrixId,Color,HexColorCode,PantoneColor,RALColorCode FROM dbo.ColorCodeMatrix";

                conn.Open();
                
                colorCodeMatrices =  conn.QueryAsync<ColorCodeMatrix>(strSelectCmd).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return colorCodeMatrices;
        }
        
        public ColorCodeMatrix GetColorCodeMatrixById(int colorCodeMatrixId)
        {
            ColorCodeMatrix colorCodeMatrix = new ColorCodeMatrix();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                //string connString = Configuration.ChangeManagementConnectionString();
                using SqlConnection conn = new SqlConnection(connString);

                string strSelectCmd =
                    $"SELECT TOP 1 ColorCodeMatrixId,Color,HexColorCode,PantoneColor,RALColorCode FROM dbo.ColorCodeMatrix WHERE ColorCodeMatrixId=@ColorCodeMatrixId";
    
                conn.Open();
                colorCodeMatrix =  conn.QueryAsync<ColorCodeMatrix>(strSelectCmd, new{colorCodeMatrixId = colorCodeMatrixId}).Result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return colorCodeMatrix;
        }
        
        public async Task<bool> UpdateColorCode(ColorCodeMatrix colorCodeMatrix)
        {
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            // colorCodeMatrix.UpdatedBy = dimension.InspectorName;
            // dimension.UpdatedDate = DateTime.Now;
            string sql =
                "UPDATE dbo.ColorCodeMatrix SET " +
                "Color = @Color, HexColorCode = @HexColorCode, PantoneColor = @PantoneColor, RALColorCode = @RALColorCode" +
                " WHERE ColorCodeMatrixId = @ColorCodeMatrixId";
            try
            {
                using SqlConnection conn = new(connString);
                conn.Open();
                conn.ExecuteAsync(sql, colorCodeMatrix);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return await Task.FromResult(true);
        }
        
        public async Task<int> InsertColorCode(ColorCodeMatrix colorCodeMatrix)
        {
            int colorCodeMatrixId = 0;
            // dimension.CreatedBy = dimension.InspectorName;
            // dimension.CreatedDate = DateTime.Now;
            // dimension.UpdatedBy = dimension.InspectorName; dimension.UpdatedDate = DateTime.Now;
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            var insertQuery =
                "INSERT INTO dbo.ColorCodeMatrix(Color, HexColorCode, PantoneColor,RALColorCode)  OUTPUT INSERTED.ColorCodeMatrixId VALUES (@Color,@HexColorCode,@PantoneColor,@RALColorCode)";
            try
            {
                using var conn = new Microsoft.Data.SqlClient.SqlConnection(connString);
                conn.Open();
                colorCodeMatrixId = conn.QuerySingle<int>(insertQuery, colorCodeMatrix);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return await Task.FromResult(colorCodeMatrixId);
        }
        
        public async Task<int> DeleteColorCode(ColorCodeMatrix colorCodeMatrix)
        {
            // user.UpdatedBy = dimension.InspectorName;
            // dimension.UpdatedDate = DateTime.Now;
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            const string query = @"Delete from dbo.[ColorCodeMatrix] WHERE ColorCodeMatrixId = @ColorCodeMatrixId";

            await using SqlConnection conn = new(connString);
            conn.Open();
            int recordDeleted = await conn.ExecuteAsync(query, new
            {
                colorCodeMatrix.ColorCodeMatrixId,
            });

            // if(recordDeleted > 0)
            //     UpdateCache(user, "DELETE");
            
            return recordDeleted; 
        }

        public IEnumerable<MidRailConfiguration> GetMidRailConfiguration()
        {
            IEnumerable<MidRailConfiguration> midRailConfigurations = new List<MidRailConfiguration>();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                //string connString = Configuration.ChangeManagementConnectionString();
                using SqlConnection conn = new SqlConnection(connString);

                string strSelectCmd =
                    $"SELECT TOP 1000 MidRailConfigurationId,PartNumber,Height,Thickness,Length,RailWeight FROM dbo.MidRailConfiguration";

                conn.Open();
                
                midRailConfigurations =  conn.QueryAsync<MidRailConfiguration>(strSelectCmd).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return midRailConfigurations;
        }
        
        public MidRailConfiguration GetMidRailConfigurationById(int midRailConfigurationId)
        {
            MidRailConfiguration midRailConfiguration = new MidRailConfiguration();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                //string connString = Configuration.ChangeManagementConnectionString();
                using SqlConnection conn = new SqlConnection(connString);

                string strSelectCmd =
                    $"SELECT TOP 1 MidRailConfigurationId,PartNumber,Height,Thickness,Length FROM dbo.MidRailConfiguration WHERE MidRailConfigurationId=@MidRailConfigurationId";
    
                conn.Open();
                midRailConfiguration =  conn.QueryAsync<MidRailConfiguration>(strSelectCmd, new{MidRailConfigurationId = midRailConfigurationId}).Result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return midRailConfiguration;
        }
        
        public async Task<int> UpdateMidRailConfiguration(MidRailConfiguration midRailConfiguration)
        {
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            // colorCodeMatrix.UpdatedBy = dimension.InspectorName;
            // dimension.UpdatedDate = DateTime.Now;
            string sql =
                "UPDATE dbo.MidRailConfiguration SET " +
                "PartNumber = @PartNumber, Height = @Height, Thickness = @Thickness, Length = @Length, RailWeight=@RailWeight" +
                " WHERE MidRailConfigurationId = @MidRailConfigurationId";

            using SqlConnection conn = new(connString);
            conn.Open();
            int recordsUpdated = await conn.ExecuteAsync(sql, midRailConfiguration);
            return recordsUpdated;
        }

        public async Task<int> InsertMidRailConfiguration(MidRailConfiguration midRailConfiguration)
        {
            int midRailConfigurationId = 0;
            // dimension.CreatedBy = dimension.InspectorName;
            // dimension.CreatedDate = DateTime.Now;
            // dimension.UpdatedBy = dimension.InspectorName; dimension.UpdatedDate = DateTime.Now;
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            var insertQuery =
                "INSERT INTO dbo.MidRailConfiguration(PartNumber, Height, Thickness, Length, RailWeight) OUTPUT INSERTED.MidRailConfigurationId VALUES (@PartNumber,@Height,@Thickness,@Length, @RailWeight)";
            try
            {
                using var conn = new SqlConnection(connString);
                conn.Open();
                midRailConfigurationId = conn.QuerySingle<int>(insertQuery, midRailConfiguration);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return await Task.FromResult(midRailConfigurationId);
        }
        
        public async Task<int> DeleteMidRailConfiguration(MidRailConfiguration midRailConfiguration)
        {
            // user.UpdatedBy = dimension.InspectorName;
            // dimension.UpdatedDate = DateTime.Now;
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            const string query = @"DELETE from dbo.[MidRailConfiguration] WHERE MidRailConfigurationId = @MidRailConfigurationId";

            await using SqlConnection conn = new(connString);
            conn.Open();
            int recordDeleted = await conn.ExecuteAsync(query, new
            {
                midRailConfiguration.MidRailConfigurationId
            });

            // if(recordDeleted > 0)
            //     UpdateCache(user, "DELETE");
            //
            return recordDeleted; 
        }
        
        public async Task<IEnumerable<Dimension>> GetDimensionsDataView()
        {
            IEnumerable<Dimension> changeLogs = new List<Dimension>();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                //string connString = Configuration.ChangeManagementConnectionString();
                await using SqlConnection conn = new SqlConnection(connString);

                string strSelectCmd =
                    $"SELECT TOP 1000 DimensionId, InspectorName, TubeThickness26, ClampThickness, RailTubeHeight23, RailTubeWidth22, TubeWeldSeamLocation24, TubeLength2, PaintIdentification, TubeCornerRadius2Clock25,TubeCornerRadius4Clock25,TubeCornerRadius8Clock25,TubeCornerRadius10Clock25, EndOfTubeCLDistance, TorquePerNote2, TorquePerNote3, TorqueMarking, TubePreGalvCoatingThickness, ClampPreCoatingThickness, TotalNoOfHolesBottom3, PartMarking, RivetPresence, Appearance, CreatedDate FROM dbo.Dimension";

                conn.Open();

                changeLogs =  conn.QueryAsync<Dimension>(strSelectCmd).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return await Task.FromResult(changeLogs);
        }

        public async Task<IEnumerable<SolarInspection>> GetSolarInspectionsDataView()
        {
            IEnumerable<SolarInspection> solarInspectionRecords = new List<SolarInspection>();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                //string connString = Configuration.ChangeManagementConnectionString();
                await using SqlConnection conn = new SqlConnection(connString);

                string strSelectCmd =
                    $"SELECT TOP 1000 [SolarInspectionId], [InspectorName], [TubeThickness26], [ClampThickness], [RailTubeHeight23], [RailTubeWidth22], [TubeWeldSeamLocation24], [TubeLength2_Red], [PaintIdentification_2900Red], [TubeLength2_Yellow], [PaintIdentification_2700Yellow], [TubeCornerRadius4Places25], [EndOfTubeCLDistance], [TorqueEndHangerPlateVoyager_1_4_2PlacesNote2], [TorqueEndHangerPlateVoyager_2_3_2PlacesNote3], [TorqueMarking], [TubePreGalvCoatingThickness], [ClampPreCoatingThickness], [TotalNoOfHolesBottom3], [PartMarking], [RivetPresence], [Appearance], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate] FROM [dbo].[SolarInspection]";

                conn.Open();

                solarInspectionRecords =  conn.QueryAsync<SolarInspection>(strSelectCmd).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return await Task.FromResult(solarInspectionRecords);
        }
        
        public async Task<Dimension> GetDimensionById(int id)
        {
            IEnumerable<Dimension> dimensions = new List<Dimension>();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = $"SELECT TOP 100 [DimensionId], InspectorName, TubeThickness26, ClampThickness, RailTubeHeight23, RailTubeWidth22, TubeWeldSeamLocation24, TubeLength2, PaintIdentification, TubeCornerRadius2Clock25,TubeCornerRadius4Clock25,TubeCornerRadius8Clock25,TubeCornerRadius10Clock25, EndOfTubeCLDistance, TorquePerNote2, TorquePerNote3, TorqueMarking, TubePreGalvCoatingThickness, ClampPreCoatingThickness, TotalNoOfHolesBottom3, PartMarking, RivetPresence, Appearance FROM dbo.Dimension WHERE DimensionId = @dimensionId";

                    conn.Open();

                    dimensions = conn.Query<Dimension>(query, new { dimensionId = id });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message} ");
            }
            return await Task.FromResult(dimensions.FirstOrDefault());

        }
        
        public async Task<SolarInspection> GetSolarInspectionById(int id)
        {
            IEnumerable<SolarInspection> solarInspections = new List<SolarInspection>();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = $"SELECT TOP 100 [SolarInspectionId], [InspectorName], [TubeThickness26], [ClampThickness], [RailTubeHeight23], [RailTubeWidth22], [TubeWeldSeamLocation24], [TubeLength2_Red], [PaintIdentification_2900Red], [TubeLength2_Yellow], [PaintIdentification_2700Yellow], [TubeCornerRadius4Places25], [EndOfTubeCLDistance], [TorqueEndHangerPlateVoyager_1_4_2PlacesNote2], [TorqueEndHangerPlateVoyager_2_3_2PlacesNote3], [TorqueMarking], [TubePreGalvCoatingThickness], [ClampPreCoatingThickness], [TotalNoOfHolesBottom3], [PartMarking], [RivetPresence], [Appearance], [CreatedBy], [CreatedDate], [UpdatedBy], [UpdatedDate] [dbo].[SolarInspection] WHERE SolarInspectionId = @solarInspectionId";

                    conn.Open();

                    solarInspections = conn.Query<SolarInspection>(query, new { solarInspectionId = id });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message} ");
            }
            
            return await Task.FromResult(solarInspections.FirstOrDefault());
        }

        public async Task<bool> UpdateDimension(Dimension dimension)
        {
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            dimension.UpdatedBy = dimension.InspectorName;
            dimension.UpdatedDate = DateTime.Now;
            string sql =
                "UPDATE dbo.Dimension SET " +
                "InspectorName = @InspectorName, TubeThickness26 = @TubeThickness26, ClampThickness = @ClampThickness, RailTubeHeight23 = @RailTubeHeight23, RailTubeWidth22 = @RailTubeWidth22, TubeWeldSeamLocation24 = @TubeWeldSeamLocation24, TubeLength2 = @TubeLength2, PaintIdentification = @PaintIdentification, TubeCornerRadius2Clock25 = @TubeCornerRadius2Clock25,TubeCornerRadius4Clock25 = @TubeCornerRadius4Clock25,TubeCornerRadius8Clock25 = @TubeCornerRadius8Clock25,TubeCornerRadius10Clock25 = @TubeCornerRadius10Clock25, EndOfTubeCLDistance = @EndOfTubeToCLDistance, TorquePerNote2 = @TorquePerNote2, TorquePerNote3 = @TorquePerNote3, TorqueMarking = @TorqueMarking, TubePreGalvCoatingThickness = @TubePreGalvCoatingThickness, ClampPreCoatingThickness = @ClampPreCoatingThickness, TotalNoOfHolesBottom3 = @TotalNoOfHolesBottom3, PartMarking = @PartMarking, RivetPresence = @RivetPresence, Appearance = @Appearance,  UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate" +
                " WHERE DimensionId = @DimensionId";

            using SqlConnection conn = new(connString);
            conn.Open();
             conn.ExecuteAsync(sql, dimension);
            return await Task.FromResult(true);
        }
        
        public async Task<bool> UpdateSolarInspection(SolarInspection solarInspection)
        {
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            solarInspection.UpdatedBy = solarInspection.InspectorName;
            solarInspection.UpdatedDate = DateTime.Now;
            string sql =
                "UPDATE dbo.SolarInspection SET " +
                "InspectorName = @InspectorName, TubeThickness26 = @TubeThickness26, ClampThickness = @ClampThickness, RailTubeHeight23 = @RailTubeHeight23, RailTubeWidth22 = @RailTubeWidth22, TubeWeldSeamLocation24 = @TubeWeldSeamLocation24, TubeLength2 = @TubeLength2, PaintIdentification = @PaintIdentification, TubeCornerRadius2Clock25 = @TubeCornerRadius2Clock25,TubeCornerRadius4Clock25 = @TubeCornerRadius4Clock25,TubeCornerRadius8Clock25 = @TubeCornerRadius8Clock25,TubeCornerRadius10Clock25 = @TubeCornerRadius10Clock25, EndOfTubeCLDistance = @EndOfTubeToCLDistance, TorquePerNote2 = @TorquePerNote2, TorquePerNote3 = @TorquePerNote3, TorqueMarking = @TorqueMarking, TubePreGalvCoatingThickness = @TubePreGalvCoatingThickness, ClampPreCoatingThickness = @ClampPreCoatingThickness, TotalNoOfHolesBottom3 = @TotalNoOfHolesBottom3, PartMarking = @PartMarking, RivetPresence = @RivetPresence, Appearance = @Appearance,  UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate " +
                " WHERE SolarInspectionId = @SolarInspectionId";

            using SqlConnection conn = new(connString);
            conn.Open();
            conn.ExecuteAsync(sql, solarInspection);
            
            return await Task.FromResult(true);
        }

        public async Task<int> InsertDimension(Dimension dimension)
        {
            dimension.CreatedBy = dimension.InspectorName;
            dimension.CreatedDate = DateTime.Now;
            dimension.UpdatedBy = dimension.InspectorName; dimension.UpdatedDate = DateTime.Now;
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            var insertQuery =
                "INSERT INTO dbo.Dimension(InspectorName, PartName, TubeThickness26, ClampThickness, RailTubeHeight23, RailTubeWidth22, TubeWeldSeamLocation24, TubeLength2, PaintIdentification, TubeCornerRadius2Clock25,TubeCornerRadius4Clock25,TubeCornerRadius8Clock25,TubeCornerRadius10Clock25, EndOfTubeCLDistance, TorquePerNote2, TorquePerNote3, TorqueMarking, TubePreGalvCoatingThickness, ClampPreCoatingThickness, TotalNoOfHolesBottom3, PartMarking, RivetPresence, Appearance, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate) VALUES (@InspectorName, @PartName, @TubeThickness26,@ClampThickness,@RailTubeHeight23,@RailTubeWidth22,@TubeWeldSeamLocation24,@TubeLength2,@PaintIdentification,@TubeCornerRadius2Clock25,@TubeCornerRadius4Clock25,@TubeCornerRadius8Clock25,@TubeCornerRadius10Clock25,@EndOfTubeToCLDistance,@TorquePerNote2,@TorquePerNote3,@TorqueMarking,@TubePreGalvCoatingThickness,@ClampPreCoatingThickness,@TotalNoOfHolesBottom3,@PartMarking,@RivetPresence,@Appearance, @CreatedBy, @CreatedDate, @UpdatedBy,@UpdatedDate)";

            using var conn = new Microsoft.Data.SqlClient.SqlConnection(connString);
            conn.Open();
            var changeLogId = conn.ExecuteScalar<int>(insertQuery, dimension);

            return await Task.FromResult(changeLogId);
        }
        
        public async Task<int> InsertSolarInspection(SolarInspection solarInspection)
        {
            solarInspection.CreatedBy = solarInspection.InspectorName;
            solarInspection.CreatedDate = DateTime.Now;
            solarInspection.UpdatedBy = solarInspection.InspectorName; solarInspection.UpdatedDate = DateTime.Now;
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            var insertQuery =
                "INSERT INTO dbo.SolarInspection(InspectorName, TubeThickness26, ClampThickness, RailTubeHeight23, RailTubeWidth22, TubeWeldSeamLocation24, TubeLength2_Red, PaintIdentification_2900Red,TubeLength2_Yellow, PaintIdentification_2700Yellow, TubeCornerRadius4Places25, EndOfTubeCLDistance, TorqueEndHangerPlateVoyager_1_4_2PlacesNote2, TorqueEndHangerPlateVoyager_2_3_2PlacesNote3, TorqueMarking, TubePreGalvCoatingThickness, ClampPreCoatingThickness, TotalNoOfHolesBottom3, PartMarking, RivetPresence, Appearance, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate) VALUES (@InspectorName,@TubeThickness26,@ClampThickness,@RailTubeHeight23,@RailTubeWidth22,@TubeWeldSeamLocation24,@TubeLength2Red,@PaintIdentification2900Red,@TubeLength2Yellow,@PaintIdentification2700Yellow,@TubeCornerRadius4Places25,@EndOfTubeToCLDistance,@TorqueEndHangerPlateVoyager_1_4_2PlacesNote2,@TorqueEndHangerPlateVoyager_2_3_2PlacesNote2,@TorqueMarking,@TubePreGalvCoatingThickness,@ClampPreCoatingThickness,@TotalNoOfHolesBottom3,@PartMarking,@RivetPresence,@Appearance, @CreatedBy, @CreatedDate, @UpdatedBy,@UpdatedDate)";

            using var conn = new Microsoft.Data.SqlClient.SqlConnection(connString);
            conn.Open();
            var changeLogId = conn.ExecuteScalar<int>(insertQuery, solarInspection);

            return await Task.FromResult(changeLogId);
        }

        public void DeleteDataEntry(int dimensionId)
        {
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            const string query = @"DELETE FROM dbo.Dimension WHERE DimensionId = @dimensionId";

            using SqlConnection conn = new(connString);
            conn.Open();
            conn.Execute(query, new
            {
                dimensionId
            });
        }
        
        public void DeleteSolarInspection(int solarInspectionId)
        {
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            const string query = @"DELETE FROM dbo.SolarInspection WHERE SolarInspectionId = @solarInspectionId";

            using SqlConnection conn = new(connString);
            conn.Open();
            conn.Execute(query, new
            {
                solarInspectionId
            });
        }

        public async Task<IEnumerable<InProcessCheck>> GetInProcessCheckDataView()
        {
            IEnumerable<InProcessCheck> changeLogs = new List<InProcessCheck>();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                //string connString = Configuration.ChangeManagementConnectionString();
                 using SqlConnection conn = new SqlConnection(connString);

                string strSelectCmd =
                    $"SELECT [InProcessCheckId],[InspectorName],[PrimarySawLengthNominal],[PrimarySawLengthTolPlus],[PrimarySawLengthTolMinus],[PrimarySawLengthValue],[BendingLengthNominal],[BendingLengthTolPlus],[BendingLengthTolMinus],[BendingLengthValue],[BendingODNominal],[BendingODTolPlus],[BendingODTolMinus],[BendingODValue],[BendingWallThicknessNominal],[BendingWallThicknessTolPlus],[BendingWallThicknessTolMinus],[BendingWallThicknessValue],[SecondarySawLengthNominal],[SecondarySawLengthTolPlus],[SecondarySawLengthTolMinus],[SecondarySawLengthValue],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate]\r\n  FROM [QualityAssuranceManager].[dbo].[InProcessCheck]";

                conn.Open();

                changeLogs =  conn.QueryAsync<InProcessCheck>(strSelectCmd).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return await Task.FromResult(changeLogs);
        }

        public async Task<InProcessCheck> GetInProcessCheckById(int id)
        {
            IEnumerable<InProcessCheck> inProcessChecks = new List<InProcessCheck>();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = $"SELECT [InProcessCheckId],[InspectorName],[PrimarySawLengthNominal],[PrimarySawLengthTolPlus],[PrimarySawLengthTolMinus],[PrimarySawLengthValue],[BendingLengthNominal],[BendingLengthTolPlus],[BendingLengthTolMinus],[BendingLengthValue],[BendingODNominal],[BendingODTolPlus],[BendingODTolMinus],[BendingODValue],[BendingWallThicknessNominal],[BendingWallThicknessTolPlus],[BendingWallThicknessTolMinus],[BendingWallThicknessValue],[SecondarySawLengthNominal],[SecondarySawLengthTolPlus],[SecondarySawLengthTolMinus],[SecondarySawLengthValue],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate] FROM [QualityAssuranceManager].[dbo].[InProcessCheck] WHERE InProcessCheckId = @inProcessCheckId";

                    conn.Open();

                    inProcessChecks = conn.Query<InProcessCheck>(query, new { inProcessCheckId = id });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message} ");
            }
            return await Task.FromResult(inProcessChecks.FirstOrDefault());

        }

        public async Task<bool> UpdateInProcessCheck(InProcessCheck inProcessCheck)
        {
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            inProcessCheck.UpdatedBy = inProcessCheck.InspectorName;
            inProcessCheck.UpdatedDate = DateTime.Now;
            string sql =
                "UPDATE dbo.InProcessCheck SET " +
                "[InspectorName] = @InspectorName,[PrimarySawLengthNominal] = @PrimarySawLengthNominal,[PrimarySawLengthTolPlus] = @PrimarySawLengthTolPlus,[PrimarySawLengthTolMinus] = @PrimarySawLengthTolMinus,[PrimarySawLengthValue] = @PrimarySawLengthValue,[BendingLengthNominal] = @BendingLengthNominal,[BendingLengthTolPlus] = @BendingLengthTolPlus,[BendingLengthTolMinus] = @BendingLengthTolMinus,[BendingLengthValue] = BendingLengthValue,[BendingODNominal] = @BendingODNominal,[BendingODTolPlus] = BendingODTolPlus,[BendingODTolMinus] = @BendingODTolMinus,[BendingODValue] = @BendingODValue,[BendingWallThicknessNominal] = @BendingWallThicknessNominal,[BendingWallThicknessTolPlus] = @BendingWallThicknessTolPlus,[BendingWallThicknessTolMinus] = @BendingWallThicknessTolMinus,[BendingWallThicknessValue] = @BendingWallThicknessValue,[SecondarySawLengthNominal] = @SecondarySawLengthNominal,[SecondarySawLengthTolPlus] = @SecondarySawLengthTolPlus,[SecondarySawLengthTolMinus] = @SecondarySawLengthTolMinus,[SecondarySawLengthValue] = @SecondarySawLengthValue,UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate" +
                " WHERE InProcessCheckId = @InProcessCheckId";

            await using SqlConnection conn = new(connString);
            conn.Open();
            await conn.ExecuteAsync(sql, inProcessCheck);
            
            return await Task.FromResult(true);
        }

        public async Task<int> InsertInProcessCheck(InProcessCheck inProcessCheck)
        {
            inProcessCheck.CreatedBy = inProcessCheck.InspectorName;
            inProcessCheck.CreatedDate = DateTime.Now;
            inProcessCheck.UpdatedBy = inProcessCheck.InspectorName; inProcessCheck.UpdatedDate = DateTime.Now;
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            var insertQuery =
                "INSERT INTO dbo.InProcessCheck(InspectorName,PrimarySawLengthNominal,PrimarySawLengthTolPlus,PrimarySawLengthTolMinus,PrimarySawLengthValue,BendingLengthNominal,BendingLengthTolPlus,BendingLengthTolMinus,BendingLengthValue,BendingODNominal,BendingODTolPlus,BendingODTolMinus,BendingODValue,BendingWallThicknessNominal,BendingWallThicknessTolPlus,BendingWallThicknessTolMinus,BendingWallThicknessValue,SecondarySawLengthNominal,SecondarySawLengthTolPlus,SecondarySawLengthTolMinus,SecondarySawLengthValue,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate)\nVALUES(@InspectorName, @PrimarySawLengthNominal, @PrimarySawLengthTolPlus, @PrimarySawLengthTolMinus, @PrimarySawLengthValue, @BendingLengthNominal, @BendingLengthTolPlus, @BendingLengthTolMinus, @BendingLengthValue, @BendingODNominal, @BendingODTolPlus, @BendingODTolMinus, @BendingODValue, @BendingWallThicknessNominal, @BendingWallThicknessTolPlus, @BendingWallThicknessTolMinus, @BendingWallThicknessValue, @SecondarySawLengthNominal, @SecondarySawLengthTolPlus, @SecondarySawLengthTolMinus, @SecondarySawLengthValue, @CreatedBy, @CreatedDate, @UpdatedBy, @UpdatedDate)";

            using var conn = new Microsoft.Data.SqlClient.SqlConnection(connString);
            conn.Open();
            var changeLogId = conn.ExecuteScalar<int>(insertQuery, inProcessCheck);

            return await Task.FromResult(changeLogId);
        }

        public void DeleteInProcessCheck(int inProcessCheckId)
        {
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            const string query = @"DELETE FROM dbo.InProcessCheck WHERE InProcessCheckId = @inProcessCheckId";

            using SqlConnection conn = new(connString);
            conn.Open();
            conn.Execute(query, new
            {
                inProcessCheckId
            });
        }
        public async Task<IEnumerable<FinalInspection>> GetFinalInspectionsDataView()
        {
            IEnumerable<FinalInspection> finalInspectionRecords = new List<FinalInspection>();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                await using SqlConnection conn = new SqlConnection(connString);
                string strSelectCmd =
                    $"SELECT TOP (1000) [FinalInspectionId],[InspectorName],[PartName],[TubeThickness],[MidHangerClampThickness],[RailTubeHeight],[RailTubeWidth],[TubeWeldSeamLocation],[TubeLength],[TubeCornerRadius4Places],[EndOfTubeToCLDistance],[EndOfTubeToClamp1Bolt_2Places],[Clamp1BoltToClamp2BoltDistance_2Places],[EndOfTubeToClamp3Bolt_2Places],[Clamp3BoltToClamp4BoltDistance_2Places],[TorqueClamp1_2Places],[TorqueClamp2_2Places],[TorqueClamp3_2Places],[TorqueClamp4_2Places],[TorqueMarking],[PartMarking],[RivetPresence],[MidHangerHolesAlign],[WasherPlatePresence6Locations],[ColorCode],[Appearance],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate]FROM [QualityAssuranceManager].[dbo].[FinalInspection]";
                conn.Open();

                finalInspectionRecords =  conn.QueryAsync<FinalInspection>(strSelectCmd).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return await Task.FromResult(finalInspectionRecords);
        }
        
        public async Task<FinalInspection> GetFinalInspectionRecordById(int finalInspectionId)
        {
            IEnumerable<FinalInspection> finalInspections = new List<FinalInspection>();
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = $"SELECT TOP (1000) [FinalInspectionId],[InspectorName],[PartName],[TubeThickness],[MidHangerClampThickness],[RailTubeHeight],[RailTubeWidth],[TubeWeldSeamLocation],[TubeLength],[TubeCornerRadius4Places],[EndOfTubeToCLDistance],[EndOfTubeToClamp1Bolt_2Places],[Clamp1BoltToClamp2BoltDistance_2Places],[EndOfTubeToClamp3Bolt_2Places],[Clamp3BoltToClamp4BoltDistance_2Places],[TorqueClamp1_2Places],[TorqueClamp2_2Places],[TorqueClamp3_2Places],[TorqueClamp4_2Places],[TorqueMarking],[PartMarking],[RivetPresence],[MidHangerHolesAlign],[WasherPlatePresence6Locations],[ColorCode],[Appearance],[CreatedBy],[CreatedDate],[UpdatedBy],[UpdatedDate]FROM [QualityAssuranceManager].[dbo].[FinalInspection] WHERE FinalInspectionId = @finalInspectionId";

                    conn.Open();

                    finalInspections = conn.Query<FinalInspection>(query, new { finalInspectionId = finalInspectionId });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception : {ex.Message} ");
            }
            
            return await Task.FromResult(finalInspections.FirstOrDefault());
        }
        
        public async Task<bool> UpdateFinalInspection(FinalInspection finalInspection)
        {
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            finalInspection.UpdatedBy = finalInspection.InspectorName;
            finalInspection.UpdatedDate = DateTime.Now;
            string sql =
                "UPDATE [QualityAssuranceManager].[dbo].[FinalInspection] SET" +
                " InspectorName = @InspectorName, TubeThickness = @TubeThickness, MidHangerClampThickness = @MidHangerClampThickness, RailTubeHeight = @RailTubeHeight, RailTubeWidth = @RailTubeWidth, TubeWeldSeamLocation = @TubeWeldSeamLocation, TubeLength = @TubeLength, TubeCornerRadius4Places = @TubeCornerRadius4Places, EndOfTubeToCLDistance = @EndOfTubeToCLDistance,EndOfTubeToClamp1Bolt_2Places = @EndOfTubeToClamp1Bolt_2Places,Clamp1BoltToClamp2BoltDistance_2Places = @Clamp1BoltToClamp2BoltDistance_2Places,EndOfTubeToClamp3Bolt_2Places = @EndOfTubeToClamp3Bolt_2Places, Clamp3BoltToClamp4BoltDistance_2Places = @Clamp3BoltToClamp4BoltDistance_2Places, TorqueClamp1_2Places = @TorqueClamp1_2Places, TorqueClamp2_2Places = @TorqueClamp2_2Places,TorqueClamp3_2Places=@TorqueClamp3_2Places,TorqueClamp4_2Places=@TorqueClamp4_2Places, TorqueMarking = @TorqueMarking, PartMarking = @PartMarking, RivetPresence = @RivetPresence, MidHangerHolesAlign = @MidHangerHolesAlign, WasherPlatePresence6Locations = @WasherPlatePresence6Locations, ColorCode = @ColorCode, Appearance = @Appearance,  UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate" +
                " WHERE FinalInspectionId = @FinalInspectionId";

            using SqlConnection conn = new(connString);
            // conn.Open();
            //conn.ExecuteAsync(sql, finalInspection);
            
            int rowsAffected = await conn.ExecuteAsync(sql, finalInspection);
        
            if (rowsAffected == 0)
            {
                // Log the case where no rows were updated
                Console.WriteLine("No rows were updated. Check if FinalInspectionId exists.");
            }
        
            return rowsAffected > 0;
        }
        
        public async Task<int> InsertFinalInspection(FinalInspection finalInspection)
        {
            finalInspection.CreatedBy = finalInspection.InspectorName;
            finalInspection.CreatedDate = DateTime.Now;
            finalInspection.UpdatedBy = finalInspection.InspectorName; 
            finalInspection.UpdatedDate = DateTime.Now;
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            var insertQuery =
                "INSERT INTO [QualityAssuranceManager].[dbo].[FinalInspection](InspectorName,PartName,TubeThickness,MidHangerClampThickness,RailTubeHeight,RailTubeWidth,TubeWeldSeamLocation,TubeLength,TubeCornerRadius4Places,EndOfTubeToCLDistance,EndOfTubeToClamp1Bolt_2Places,Clamp1BoltToClamp2BoltDistance_2Places,EndOfTubeToClamp3Bolt_2Places,Clamp3BoltToClamp4BoltDistance_2Places,TorqueClamp1_2Places,TorqueClamp2_2Places,TorqueClamp3_2Places,TorqueClamp4_2Places,TorqueMarking,PartMarking,RivetPresence,MidHangerHolesAlign,WasherPlatePresence6Locations,ColorCode,Appearance,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate) VALUES (@InspectorName,@PartName,@TubeThickness,@MidHangerClampThickness,@RailTubeHeight,@RailTubeWidth,@TubeWeldSeamLocation,@TubeLength,@TubeCornerRadius4Places,@EndOfTubeToCLDistance,@EndOfTubeToClamp1Bolt_2Places,@Clamp1BoltToClamp2BoltDistance_2Places,@EndOfTubeToClamp3Bolt_2Places,@Clamp3BoltToClamp4BoltDistance_2Places,@TorqueClamp1_2Places,@TorqueClamp2_2Places,@TorqueClamp3_2Places,@TorqueClamp4_2Places,@TorqueMarking,@PartMarking,@RivetPresence,@MidHangerHolesAlign,@WasherPlatePresence6Locations,@ColorCode,@Appearance,@CreatedBy,@CreatedDate,@UpdatedBy,@UpdatedDate)";

            using var conn = new Microsoft.Data.SqlClient.SqlConnection(connString);
            conn.Open();
            var changeLogId = conn.ExecuteScalar<int>(insertQuery, finalInspection);

            return await Task.FromResult(changeLogId);
        }
        
        public void DeleteFinalInspection(int finalInspectionId)
        {
            var connString = DatabaseFactory.GetDbConnString("CMRS");
            const string query = @"DELETE FROM dbo.FinalInspection WHERE FinalInspectionId = @finalInspectionId";

            using SqlConnection conn = new(connString);
            conn.Open();
            conn.Execute(query, new
            {
                finalInspectionId
            });
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
        public override string ToString() => "IsPrimaryKey";
    }
}
