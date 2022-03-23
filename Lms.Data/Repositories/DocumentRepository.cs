using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Repositories;

public class DocumentRepository : IDocumentRepository
{

    private ApplicationDbContext db;
    public DocumentRepository(ApplicationDbContext db)
    {
        this.db = db ?? throw new ArgumentNullException(nameof(db));
    }
    public async Task<IEnumerable<Document>> GetAllDocuments()
    {
        return await db.Documents.ToListAsync();
    }
    public async Task AddDocument(Document document)
    {
        await db.Documents.AddAsync(document);
    }
    public void UpdateDocument(Document document)
    {
        db.Documents.Update(document);
    }
    public async Task DeleteDocument(int id)
    {
        var document = await db.Documents.FirstOrDefaultAsync(d => d.Id == id);
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }
        db.Documents.Remove(document);
    }

    public bool DocumentExists(int id)
    {
        return db.Documents.Any(e => e.Id == id);
    }
    public async Task<Document> GetDocumentById(int id)
    {
        var document = await db.Documents.FirstOrDefaultAsync(m => m.Id == id);
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }
        return document;
    }

    public  IEnumerable<Document> GetDocumentBy_UserId(string id)
    {
       var document =  db.Documents
                            .Include(u=>u.User)
                            .Where(d=>d.User.Id == id)
                             .ToList();
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }
        return document;
    }
}
