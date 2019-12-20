using CommonLibrary;
using DataFactoryTS;
using EntitysTS;
using IShareLogicTS;
using ModelsTS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicTS
{
    public class UserBL : IUser
    {
        private static IBase ibase = DataAccess.CreateTSBase();

        List<UserEnt> IUser.GetAllUsers()
        {
            List<user> users = ibase.GetModelList<user>(x => true);
            if (users?.Count > 0)
            {
                List<UserEnt> ents = new List<UserEnt>();
                users.ForEach(m => ents.Add(ConvertModelToEnt(m)));
                return ents;
            }
            return null;
        }

        UserEnt IUser.GetUserById(int id)
        {
            user user = ibase.GetModel<user>(x => x.Id == id);
            if (user != null)
                return ConvertModelToEnt(user);
            return null;
        }

        bool IUser.AddUser(UserEnt user)
        {
            if (user != null)
            {
                return ibase.Add(ConvertEntToModel(user));
            }
            return false;
        }

        bool IUser.DeleteUser(int id)
        {
            user user = ibase.GetModel<user>(x => x.Id == id);
            if (user != null)
                return ibase.Delete(user);
            return false;
        }

        bool IUser.UpdateUser(UserEnt ent)
        {
            if (ent == null) return false;
            user user = ibase.GetModel<user>(x => x.Id == ent.Id);
            if (user != null)
            {
                user model = ConvertEntToModel(ent);
                return ibase.Update(model);
            }
            return false;
        }

        bool IUser.IsExist(UserEnt user)
        {
            user model = ibase.GetModel<user>(x => x.UserName == user.UserName && x.Id != user.Id);
            if (model != null)
                return true;
            return false;
        }

        bool IUser.Login(string userName, string passWord)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord)) return false;
            List<user> users = ibase.GetModelList<user>(x => true);
            if (users?.Count > 0)
            {
                user logUser = users.Find(x => x.UserName == userName);
                if (logUser == null) return false;
                if (logUser.PassWord == passWord)
                    return true;
                else
                    return false;
            }
            return false;
        }


        private UserEnt ConvertModelToEnt(user user)
        {
            UserEnt ent = new UserEnt();
            ent.Id = user.Id;
            ent.Name = user.Name;
            ent.IdCardNum = user.IdCardNum;
            ent.Sex = (int)user.Sex;
            ent.UserName = user.UserName;
            ent.PassWord = user.PassWord;
            ent.Phone = user.Phone;
            return ent;
        }

        private user ConvertEntToModel(UserEnt ent)
        {
            user model = new user();
            model.Id = ent.Id;
            model.Name = ent.Name;
            model.IdCardNum = ent.IdCardNum;
            model.Sex = ent.Sex;
            model.UserName = ent.UserName;
            model.PassWord = ent.PassWord;
            model.Phone = ent.Phone;
            return model;
        }
    }
}
