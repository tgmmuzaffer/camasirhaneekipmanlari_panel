using panel.Models;
using panel.Models.Dtos;
using System.Threading.Tasks;

namespace panel.Repository.IRepository
{
    public interface ILoginRepo:IBaseRepo<User>
    {
        Task<UserDto> Login(string url, UserDto user);
    }
}
