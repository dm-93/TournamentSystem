namespace TournamentSystemDataSource.DTO.Person.Request
{
    public class GetPersonByConditionRequest(string PropertyName, object PropertyValue,string PropertyValueType)
    {
        public required string PropertyName { get; set; } = PropertyName;
        public required object PropertyValue { get; set;  } = PropertyValue;
        public required string PropertyValueType { get; set; } = PropertyValueType;
    }
}
