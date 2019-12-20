using IShareLogicTS;
using EntitysTS;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace TestWebApi.Controllers
{
    public class UserController : ApiController, IUser
    {
        /// <summary>
        /// 业务工厂
        /// </summary>
        public BLFactoryTS.BLFactory FactoryTS { get { return new BLFactoryTS.BLFactory(); } }

        [HttpPost]
        public bool AddUser(UserEnt user)
        {
            return FactoryTS.User.AddUser(user);
        }

        [HttpPost]
        public bool DeleteUser(int id)
        {
            return FactoryTS.User.DeleteUser(id);
        }

        [HttpPost]
        public List<UserEnt> GetAllUsers()
        {
            return FactoryTS.User.GetAllUsers();
        }

        [HttpPost]
        public UserEnt GetUserById(int id)
        {
            return FactoryTS.User.GetUserById(id);
        }

        [HttpPost]
        public bool IsExist(UserEnt user)
        {
            return FactoryTS.User.IsExist(user);
        }

        [HttpPost]
        public bool Login(string userName, string passWord)
        {
            return FactoryTS.User.Login(userName, passWord);
        }

        [HttpPost]
        public bool UpdateUser(UserEnt user)
        {
            return FactoryTS.User.UpdateUser(user);
        }
    }
}
