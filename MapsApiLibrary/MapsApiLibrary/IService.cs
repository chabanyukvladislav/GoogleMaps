using System.Threading.Tasks;

namespace MapsApiLibrary
{
    public interface IService<out T, TQ>
    {
        T Parameters { get; }

        Task<TQ> GetResultAsync();
    }
}
