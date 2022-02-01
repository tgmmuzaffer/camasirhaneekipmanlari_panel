using panel.Models;
using panel.Models.Dtos;
using System.Threading.Tasks;

namespace panel.Repository.IRepository
{
    public interface ILoginRepo:IBaseRepo<User>
    {
        Task<UserDto> Login(string url, UserDto user);
        Task<UserDto> GetUserDataByName(string url, string name);
        Task<bool> GetByResetPass(string url, User user);
    }
}
