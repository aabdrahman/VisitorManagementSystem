namespace Shared.RequestFeatures;

public class UsersRequestParameter
{
    public int NumberOfRecord { get; set; } = 1;
    public int NumberOfRecordsToSkip { get; set; } = 1;
    public string RoleName { get; set; }
    public string? Username { get; set; } = string.Empty;
}
