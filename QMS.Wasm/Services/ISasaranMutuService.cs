using QMS.Core.Models;

namespace QMS.Wasm.Services;

public interface ISasaranMutuService
{
    Task<List<KpiModel>> GetKpisAsync();
    Task<bool> AddKpiAsync(KpiModel kpi);
    Task<bool> UpdateKpiAsync(KpiModel kpi);
    Task<bool> DeleteKpiAsync(int id);

    Task<List<GugusMutuModel>> GetGugusListAsync();
    Task<bool> AddGugusAsync(GugusMutuModel gugus);
    Task<bool> UpdateGugusAsync(GugusMutuModel gugus);
    Task<bool> DeleteGugusAsync(int id);
}