using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Interfaces
{
    public interface IDocumentRepository
    {
        Task AddDocument(Document document);
        void UpdateDocument(Document document);
        Task DeleteDocument(int id);
        bool DocumentExists(int id);
        Task<Document> GetDocumentById(int id);
        IEnumerable<Document> GetDocumentsBy_UserId(string id);
        Task<IEnumerable<Document>> GetAllDocuments();
        Task<IEnumerable<Document>> GetDocumentsBy_CourseIdAsync(int id);
    }
}
