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

    public IEnumerable<Document> GetDocumentsBy_UserId(string id)
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
    public async Task<IEnumerable<Document>> GetDocumentsBy_CourseIdAsync(int id)
    {
        var document = await db.Documents
                             .Include(d => d.Course)
                             .Where(d => d.Course!.Id == id)
                              .ToListAsync();
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }
        return document;
    }
}
