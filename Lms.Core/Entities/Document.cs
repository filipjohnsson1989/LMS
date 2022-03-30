namespace Lms.Core.Entities;
public class Document : BaseEntity
{
    public string Name { get; set; } = default!;
    public byte[] Data { get; set; } = default!;
    public string ContentType { get; set; } = default!;
    public DateTime UploadDate { get; set; }
    public Course? Course { get; set; }
    public Module? Module { get; set; }
    public Activity? Activity { get; set; }

    public ApplicationUser? User { get; set; }

}
