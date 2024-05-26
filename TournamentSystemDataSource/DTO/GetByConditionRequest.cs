namespace TournamentSystemDataSource.DTO
{
    public record GetByConditionRequest(string PropertyName, string PropertyValue, string PropertyValueType)
    {
        public required string PropertyName { get; set; } = PropertyName;
        public string PropertyValue { get; set; } = PropertyValue;
        public required string PropertyValueType { get; set; } = PropertyValueType;
    }
}
