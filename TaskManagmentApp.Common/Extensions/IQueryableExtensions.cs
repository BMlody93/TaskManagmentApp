using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class IQueryableExtensions
{
    public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> source)
    {
        // Simulate async behavior
        return await Task.Run(() => source.ToList());
    }

    public static async Task<T> FirstOrDefaultAsync<T>(this IQueryable<T> source, Func<T, bool> predicate)
    {
        return await Task.Run(() => source.FirstOrDefault(predicate));
    }
}