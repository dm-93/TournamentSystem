namespace TournamentSystemDataSource.DTO.Person.Response
{
    public record GetPersonSimpleResponse(int Id,
                                          string Name,
                                          string LastName,
                                          string Email,
                                          int Age,
                                          double Weight,
                                          bool Gender,
                                          string Phone);
}
