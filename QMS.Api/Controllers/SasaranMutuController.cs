using Microsoft.AspNetCore.Mvc;
using QMS.Core.Models;

namespace QMS.Api.Controllers;

[ApiController]
[Route("api/sasaran-mutu")]
public class SasaranMutuController : ControllerBase
{
    private static readonly List<KpiModel> KpiList = new()
    {
        new KpiModel { Id = 1, Indikator = "Reduksi Defect Line A", Department = "Quality Assurance", PenanggungJawab = "Budi Santoso", Target = 95, Capaian = 92.5, Satuan = "%" }
    };

    private static readonly List<GugusMutuModel> GugusList = new()
    {
        new GugusMutuModel { Id = 1, NamaGugus = "Gugus Eagle", Kategori = "CIP", Judul = "Efisiensi Waktu Setup Mesin", Fasilitator = "Ahmad Hidayat", Ketua = "Rian Pratama" }
    };

    [HttpGet("kpi")]
    public ActionResult<IEnumerable<KpiModel>> GetKpis() => Ok(KpiList);

    [HttpPost("kpi")]
    public ActionResult<KpiModel> AddKpi([FromBody] KpiModel newKpi)
    {
        newKpi.Id = KpiList.Count > 0 ? KpiList.Max(k => k.Id) + 1 : 1;
        KpiList.Add(newKpi);
        return Ok(newKpi);
    }

    [HttpPut("kpi/{id}")]
    public IActionResult UpdateKpi(int id, [FromBody] KpiModel updatedKpi)
    {
        var existing = KpiList.FirstOrDefault(k => k.Id == id);
        if (existing == null) return NotFound();
        existing.Indikator = updatedKpi.Indikator;
        existing.Department = updatedKpi.Department;
        existing.PenanggungJawab = updatedKpi.PenanggungJawab;
        existing.Target = updatedKpi.Target;
        existing.Capaian = updatedKpi.Capaian;
        existing.Satuan = updatedKpi.Satuan;
        return NoContent();
    }

    [HttpDelete("kpi/{id}")]
    public IActionResult DeleteKpi(int id)
    {
        var existing = KpiList.FirstOrDefault(k => k.Id == id);
        if (existing == null) return NotFound();
        KpiList.Remove(existing);
        return NoContent();
    }

    [HttpGet("gugus")]
    public ActionResult<IEnumerable<GugusMutuModel>> GetGugusList() => Ok(GugusList);

    [HttpPost("gugus")]
    public ActionResult<GugusMutuModel> AddGugus([FromBody] GugusMutuModel newGugus)
    {
        newGugus.Id = GugusList.Count > 0 ? GugusList.Max(g => g.Id) + 1 : 1;
        GugusList.Add(newGugus);
        return Ok(newGugus);
    }

    [HttpPut("gugus/{id}")]
    public IActionResult UpdateGugus(int id, [FromBody] GugusMutuModel updatedGugus)
    {
        var existing = GugusList.FirstOrDefault(g => g.Id == id);
        if (existing == null) return NotFound();
        existing.NamaGugus = updatedGugus.NamaGugus;
        existing.Kategori = updatedGugus.Kategori;
        existing.Judul = updatedGugus.Judul;
        existing.Fasilitator = updatedGugus.Fasilitator;
        existing.Ketua = updatedGugus.Ketua;
        return NoContent();
    }

    [HttpDelete("gugus/{id}")]
    public IActionResult DeleteGugus(int id)
    {
        var existing = GugusList.FirstOrDefault(g => g.Id == id);
        if (existing == null) return NotFound();
        GugusList.Remove(existing);
        return NoContent();
    }
}