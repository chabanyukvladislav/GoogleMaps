using System.Threading.Tasks;

namespace MapsApiStandardLibrary
{
    public interface IService<out T, TQ>
    {
        T Parameters { get; }

        Task<TQ> GetResultAsync();
    }
}
