namespace InforceTask.Models;

public class UrlModelWithAccess
{
    public int Id { get; set; }
    public string FullUrl { get; set; }
    public string? Description { get; set; }
    public bool HaveDeleteButton { get; set; }
}