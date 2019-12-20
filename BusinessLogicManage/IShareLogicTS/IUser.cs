using EntitysTS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IShareLogicTS
{
    public interface IUser
    {
        List<UserEnt> GetAllUsers();

        UserEnt GetUserById(int id);

        bool AddUser(UserEnt user);

        bool DeleteUser(int id);

        bool UpdateUser(UserEnt user);

        bool IsExist(UserEnt user);

        bool Login(string userName, string passWord);
    }
}
