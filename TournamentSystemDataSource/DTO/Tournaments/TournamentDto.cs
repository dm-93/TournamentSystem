using TournamentSystemModels;

public class TournamentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal EntryFee { get; set; }
    public string TournamentPictureBase64 { get; set; }

    public Tournament MapToDomainModel()
    {
        return new Tournament
        {
            Id = Id,
            Name = Name,
            StartDate = StartDate,
            EndDate = EndDate,
            EntryFee = EntryFee,
            TournamentPicture = null,
        };
    }
}
