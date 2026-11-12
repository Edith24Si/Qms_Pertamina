using Microsoft.AspNetCore.Mvc;
using QMS.Core.Models;

namespace QMS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QualityDocumentsController : ControllerBase
{
    // Mock Database sederhana
    private static readonly List<QualityDocument> Documents = new()
    {
        new QualityDocument { Id = 1, DocumentNumber = "SOP-001", Title = "Prosedur Kontrol Dokumen", Category = "SOP", Version = "1.0", IsApproved = true },
        new QualityDocument { Id = 2, DocumentNumber = "WI-005", Title = "Instruksi Kerja Kalibrasi", Category = "Work Instruction", Version = "2.1", IsApproved = false }
    };

    [HttpGet]
    public ActionResult<IEnumerable<QualityDocument>> GetAll()
    {
        return Ok(Documents);
    }

    [HttpGet("{id}")]
    public ActionResult<QualityDocument> GetById(int id)
    {
        var doc = Documents.FirstOrDefault(d => d.Id == id);
        if (doc == null) return NotFound();
        return Ok(doc);
    }

    [HttpPost]
    public ActionResult<QualityDocument> Create(QualityDocument newDoc)
    {
        newDoc.Id = Documents.Count > 0 ? Documents.Max(d => d.Id) + 1 : 1;
        Documents.Add(newDoc);
        return CreatedAtAction(nameof(GetById), new { id = newDoc.Id }, newDoc);
    }
}