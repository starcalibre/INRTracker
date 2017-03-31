using System.Collections.Generic;
using System.Threading.Tasks;
using INRTracker.Models;

namespace INRTracker.Services
{
    /// <summary>
    /// All repositories for INREntry rows should implement this interface.
    /// </summary>
    public interface IINREntryRepository
    {
        Task<List<INREntry>> GetINREntriesAsync();
        Task<INREntry> GetINREntryByIDAsync(long id);
        Task<INREntry> AddINREntryAsync(INREntry entry);
        Task<int> UpdateINREntryAsync(INREntry entry);
        Task<int> DeleteINREntryAsync(INREntry entry);
    }
}
