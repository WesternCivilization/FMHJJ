using FMHJJService.Business.FMHJJ;
using FMHJJService.Common;
using FMHJJService.Common.Enum;
using FMHJJService.DAL;
using FMHJJService.Model.CustomModel.FMHJJ;
using FMHJJService.Model.FMHJJ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace FMHJJService.Controllers
{
    [AuthorizeUser]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Utils.FormSignOut();            
            return RedirectToAction("Login", "Home");
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        [HttpPost, AllowAnonymous]
        public ActionResult UserLogin(Base_UserInfo item)
        {
            var json = new JsonHelper() { Msg = "登录成功", Status = "n" };
            try
            {
                if (UserBusiness.IsEffectiveUser(item.UserName, item.Password))
                {
                    json.Status = "y";
                    json.Msg = "用户名密码正确";
                    json.ReUrl = "../home/index";

                    Utils.FormSignOut();
                    var userInfoCacheModel = UserBusiness.GetUser(item.UserName);
                    var authTicket = new FormsAuthenticationTicket(1, userInfoCacheModel.UserName, DateTime.Now, DateTime.Now.AddDays(1),
                        item.REMEMBER_ME, userInfoCacheModel.ID.ToString());
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    if (authTicket.IsPersistent) cookie.Expires = authTicket.Expiration;                   
                    Response.Cookies.Add(cookie);

                    Session["loginUser"] = userInfoCacheModel;
                    Session.Timeout = 300;
                }
                else
                {
                    json.Msg = "用户名或密码不正确";
                }

            }
            catch (Exception e)
            {
                json.Msg = e.Message;
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
 
        [AllowAnonymous]
        public ActionResult Index()
        {
            var userInfoCacheMode = (Session["loginUser"] as UserInfoCacheModel) ?? new UserInfoCacheModel();

            var menuId = 0;
            ViewData["Menus"] = "<script type='text/javascript'>getMenu(" + getMenu(userInfoCacheMode.ID, CommonConfig.SuperName, userInfoCacheMode.UserType ?? (int)UserType.Anonym, "新竞价公告", out menuId) + ");</script>";
            ViewData["MenuId"] = "tab_" + menuId;
            return View();
        }

        [HttpPost]
        public string getMenu(int userId, string superName, int userType, string menuText, out int menuId)
        {
            menuId = 0;
            SqlParameter[] parameters = new SqlParameter[3];
            parameters[0] = new SqlParameter("@UserID", userId);
            parameters[1] = new SqlParameter("@SuperName", superName);
            parameters[2] = new SqlParameter("@UserType", userType);
            string sql = "exec GetMenus @UserID,@SuperName,@UserType";            
            var dbContext = DbContextFactory.CreateByModelNamespace(typeof(Base_UserInfo).Namespace);
            var menuList = dbContext.Database.SqlQuery<DictFunctionsModel>(sql, parameters).ToList();            
            if (menuList == null) return "";

            //用于获取主界面菜单ID
            var menu = menuList.Where(p => p.FunctionName.Equals(menuText, StringComparison.OrdinalIgnoreCase)).FirstOrDefault() ?? new DictFunctionsModel();
            menuId = menu.ID;

            JavaScriptSerializer jss = new JavaScriptSerializer();
            var js = jss.Serialize(menuList);

            return js;
        }

    }
}