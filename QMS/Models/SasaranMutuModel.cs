namespace QMS.Core.Models;

public class KpiModel
{
    public int Id { get; set; }
    public string Indikator { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string PenanggungJawab { get; set; } = string.Empty;
    public double Target { get; set; }
    public double Capaian { get; set; }
    public string Satuan { get; set; } = "%";
}

public class GugusMutuModel
{
    public int Id { get; set; }
    public string NamaGugus { get; set; } = string.Empty;
    public string Kategori { get; set; } = "CIP";
    public string Judul { get; set; } = string.Empty;
    public string Fasilitator { get; set; } = string.Empty;
    public string Ketua { get; set; } = string.Empty;
}