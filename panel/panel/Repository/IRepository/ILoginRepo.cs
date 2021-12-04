using panel.Models;
using System.Threading.Tasks;

namespace panel.Repository.IRepository
{
    public interface ILoginRepo:IBaseRepo<User>
    {
        Task<User> Login(string url, User user);
    }
}
