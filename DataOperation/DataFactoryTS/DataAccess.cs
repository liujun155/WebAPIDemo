using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFactoryTS
{
    /// <summary>
    /// 接口工厂
    /// </summary>
    public class DataAccess
    {
        private static readonly string manage = ConfigurationManager.AppSettings["Manager"] + "TS";

        /// <summary>
        /// 基类接口
        /// </summary>
        /// <returns></returns>
        public static IBase CreateTSBase()
        {
            string manageName = manage + ".BaseMgr";
            Assembly assem = Assembly.Load(manage);
            return (IBase)assem.CreateInstance(manageName);
        }
    }
}
