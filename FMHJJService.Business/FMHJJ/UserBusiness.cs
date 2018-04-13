using FMHJJService.BLL.Container;
using FMHJJService.BLL.IBLL.FMHJJ;
using FMHJJService.Common;
using FMHJJService.Common.Enum;
using FMHJJService.Model.CustomModel.FMHJJ;
using FMHJJService.Model.FMHJJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMHJJService.Business.FMHJJ
{
    public class UserBusiness
    {
        private static List<UserInfoCacheModel> listUser = null;

        public static List<UserInfoCacheModel> UserInfoList
        {
            get {
                if(listUser == null) Init();
                return listUser;
            }
        }

        public static void Init()
        {
            IBase_UserInfoService userInfoService = BLLContainer.Resolve<IBase_UserInfoService>();
            List<Base_UserInfo> list = userInfoService.GetModels(p => p.State == (int)UserState.Normal).ToList();
            listUser = Utils.GetCopyList<UserInfoCacheModel, Base_UserInfo>(list);
        }

        public static bool IsEffectiveUser(int id)
        {
            return UserInfoList.Any<UserInfoCacheModel>(p => p.ID == id);
        }

        public static bool IsEffectiveUser(string userName)
        {
            return UserInfoList.Any<UserInfoCacheModel>(p => p.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }

        public static bool IsEffectiveUser(string userName, string password)
        {
            password = Utils.MD5(password);
            return UserInfoList.Any<UserInfoCacheModel>(p => p.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) && p.Password.Equals(password, StringComparison.OrdinalIgnoreCase));
        }

        public static UserInfoCacheModel GetUser(string userName)
        {
            return UserInfoList.Where<UserInfoCacheModel>(p => p.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public static UserInfoCacheModel GetUser(int? id)
        {
            return UserInfoList.Where<UserInfoCacheModel>(p => p.ID == id).FirstOrDefault();
        }

        public static string GetUserName(int? id)
        {
            var userInfo = GetUser(id);
            return userInfo != null ? userInfo.UserName : string.Empty;
        }

        public static string GetUserLvl(int? id)
        {
            var userInfo = GetUser(id);
            return userInfo != null ? DictSystemBusiness.GetDictKey("客户等级", userInfo.UserLvl + "") : string.Empty;
        }
    }
}
