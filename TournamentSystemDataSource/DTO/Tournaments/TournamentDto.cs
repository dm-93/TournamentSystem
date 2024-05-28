using TournamentSystemModels;

public class TournamentDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal EntryFee { get; set; }
    public string TournamentPictureBase64 { get; set; }

    public TournamentDto()
    {

    }

    public TournamentDto(Tournament tournament) : this()
    {
        Id = tournament.Id;
        Name = tournament.Name;
        StartDate = tournament.StartDate;
        EndDate = tournament.EndDate;
        EntryFee = tournament.EntryFee;
        TournamentPictureBase64 = string.Empty;
    }

    public TournamentDto(Tournament tournament, string tournamentPictureBase64) : this(tournament)
    {
        TournamentPictureBase64 = tournamentPictureBase64;
    }
}
