namespace Shared.RequestFeatures;

public class UsersRequestParameter
{
    public int NumberOfRecord { get; set; }
    public int NumberOfRecordsToSkip { get; set; }
    public string RoleName { get; set; }
    public string @Username { get; set; } = string.Empty;
}
