using BusinessLogicTS;
using IShareLogicTS;

namespace BLFactoryTS
{
    public class BLFactory
    {
        private IUser _user;
        /// <summary>
        /// 用户接口
        /// </summary>
        public IUser User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserBL();
                }
                return _user;
            }
        }
    }
}
