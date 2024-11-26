namespace ManufacturingManager.Core.Services
{
    public static class DropDownCategories
    {
        public static List<Decision> GetDecisions()
        {
            var decisionList = new List<Decision>
            {
                new() { DecisionId = 1, DecisionName = "OK" },
                new() { DecisionId = 2, DecisionName = "NOK" }
            };
            return decisionList;
        }
        
    }
}
