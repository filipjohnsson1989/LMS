namespace Lms.Data.Repositories;

public class DocumentRepositoryG : GenericRepository<Document>
{
    public DocumentRepositoryG(ApplicationDbContext context) : base(context)
    {
    }
}
