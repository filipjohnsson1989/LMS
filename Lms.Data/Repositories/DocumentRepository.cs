﻿using Lms.Core.Entities;
using Lms.Core.Interfaces;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
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
    }
}
