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
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        List<UserEnt> GetAllUsers();
        /// <summary>
        /// 根据姓名获取用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        List<UserEnt> GetUsers(string name);
        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserEnt GetUserById(int id);

        bool AddUser(UserEnt user);

        bool DeleteUser(int id);

        bool UpdateUser(UserEnt user);
        /// <summary>
        /// 是否存在用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool IsExist(UserEnt user);
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        bool Login(string userName, string passWord);
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="newPassWord"></param>
        /// <returns></returns>
        bool ChangePassWord(string userName, string oldPassWord, string newPassWord);
    }
}
