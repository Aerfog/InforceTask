namespace InforceTask.Data.Entity;

public class UrlsItem
{
   public UrlsItem(string fullUrl, string shortUrl, string createdBy, DateTime createdDate, string descriptions)
   {
      FullUrl = fullUrl;
      ShortUrl = shortUrl;
      CreatedBy = createdBy;
      CreatedDate = createdDate;
      Descriptions = descriptions;
   }

   public int Id { get; set; }
   public string FullUrl { get; }
   public string ShortUrl { get; }
   public string CreatedBy { get; }
   public DateTime CreatedDate { get; }
   public string? Descriptions { get; }
}