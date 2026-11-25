
using StatisticsStorage.Accumulators.Objects;

namespace BaseContract.Interfaces
{
    public interface IHittingSetSaver
    {
        string Save(List<HittingSetPerfomance> hittingSetPerfomances);
        Task<string?> DeleteAsync(string algorithm, int? numberOfSet = null, int? dimension = null, decimal? maxCount = null);
   }
}
