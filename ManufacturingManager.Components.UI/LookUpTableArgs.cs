namespace ManufacturingManager.Components.UI
{
    /// <summary>
    /// This structure is used to pass value arguments to Javascript used for Jquery datatable function in MyDataTables.js
    /// TableName is the id in <table></table> element to let jquery recognize it
    /// FileNameToExportExcel is the name to be used to create the excel file when Export to Excel is selected in Datatable
    /// TitleInExcelFile is used as title inside the file
    /// </summary>
    public struct LookUpTableArgs
    {
        public string? TableName { get; set; }
        public string? FilenameToExportExcel { get; set; }
        public string? TitleInExcelFile { get; set; }
    }
}
