namespace ManufacturingManager.Core.Services
{
    public static class DropDownCategories
    {
        public static List<Decision> GetDecisions()
        {
            var decisionList = new List<Decision>
            {
                new() { DecisionId = 0, DecisionName = "OK" },
                new() { DecisionId = 1, DecisionName = "NOK" }
            };
            return decisionList;
        }
        
    }
}
