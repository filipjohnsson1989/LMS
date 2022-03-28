using AutoMapper;
using Lms.Core.ViewModels.Documents;

namespace Lms.Data.AutoMapper;

public class DocumentProfile:Profile
{
    public DocumentProfile()
    {
        CreateMap<Document, DocumentViewModel>();
    }
}
