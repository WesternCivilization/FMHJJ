using FMHJJService.BLL.CommonBLL;
using FMHJJService.BLL.Container;
using FMHJJService.BLL.IBLL.FMHJJ;
using FMHJJService.Business.FMHJJ;
using FMHJJService.Common;
using FMHJJService.Common.Enum;
using FMHJJService.DAL;
using FMHJJService.Model.CustomModel.FMHJJ;
using FMHJJService.Model.FMHJJ;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace FMHJJService.Controllers
{
    [AuthorizeUser]
    public class UserManageController : Controller
    {
        private void GetUserLvls(string userLvl = "0")
        {            
            var userLvlList = DictSystemBusiness.GetDicts("客户等级");
            var selectItemList = new List<SelectListItem>() {
                new SelectListItem(){ Value="0", Text="请选择" }
            };

            bool hasSelected = false;
            if (userLvlList != null && userLvlList.Count > 0)
            {
                foreach (var item in userLvlList)
                {
                    if (item.DictValue.Equals(userLvl, StringComparison.OrdinalIgnoreCase))
                    {
                        hasSelected = true;
                        selectItemList.Add(new SelectListItem { Value = item.DictValue, Text = item.DictKey, Selected = true });
                    }
                    else
                    {
                        selectItemList.Add(new SelectListItem { Value = item.DictValue, Text = item.DictKey });
                    }
                }
            }

            if (!hasSelected)
            {
                selectItemList[0].Selected = true;
            }

            ViewBag.database_userlvls = selectItemList;
        }

        private void GetProductTypes(string productType = "0")
        {
            var productInfoList = ProductTypeBusiness.ProductInfoList;
            var selectItemList = new List<SelectListItem>() {
                new SelectListItem(){ Value = "0", Text = "请选择" }
            };

            bool hasSelected = false;
            if (productInfoList != null && productInfoList.Count > 0)
            {
                foreach (var item in productInfoList)
                {
                    if (item.ID.ToString().Equals(productType, StringComparison.OrdinalIgnoreCase))
                    {
                        hasSelected = true;
                        selectItemList.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.ProductName, Selected = true });
                    }
                    else
                    {
                        selectItemList.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.ProductName });
                    }
                }
            }

            if (!hasSelected)
            {
                selectItemList[0].Selected = true;
            }

            ViewBag.database_producttypes = selectItemList;
        }

        // GET: UserManage
        public ActionResult Index(string usertype = "1", string producttype = "0", string userlvl = "0")
        {
            ViewData["usertype"] = usertype;
            GetProductTypes(producttype);
            GetUserLvls(userlvl);

            SqlParameter[] parameters = new SqlParameter[0];
            string sql = "select * from Base_UserInfo where [State]=0 ";
            sql = sql + " and UserType=@userType";
            Array.Resize(ref parameters, parameters.Length + 1);
            parameters[parameters.Length - 1] = new SqlParameter("@userType", usertype);

            if (producttype != "0")
            {
                sql = sql + " and ProductType=@producttype";
                Array.Resize(ref parameters, parameters.Length + 1);
                parameters[parameters.Length - 1] = new SqlParameter("@producttype", producttype);
            }
            if (userlvl != "0")
            {
                sql = sql + " and UserLvl=@userlvl";
                Array.Resize(ref parameters, parameters.Length + 1);
                parameters[parameters.Length - 1] = new SqlParameter("@userlvl", userlvl);
            }

            var dbContext = DbContextFactory.CreateByModelNamespace(typeof(Base_UserInfo).Namespace);
            var base_UserInfoList = dbContext.Database.SqlQuery<Base_UserInfo>(sql, parameters).ToList();
            return View(base_UserInfoList);
        }
        
        [AllowAnonymous]
        public ActionResult UserRegister()
        {
            GetProductTypes();
            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult UserRegister(Base_UserInfo base_UserInfo)
        {
            var json = new JsonHelper() { Msg = "录入成功", Status = "n" };
            try
            {
                if (!string.IsNullOrWhiteSpace(base_UserInfo.Mobile) && !Utils.CheckMobile(base_UserInfo.Mobile))
                {
                    json.Msg = "非法的手机号码";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrWhiteSpace(base_UserInfo.Mail) && !Utils.IsValidEmail(base_UserInfo.Mail))
                {
                    json.Msg = "非法的邮箱地址";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (UserBusiness.IsEffectiveUser(base_UserInfo.UserName))
                {
                    json.Msg = "用户名已存在，请更换一个名称";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                base_UserInfo.UserType = (int)UserType.Customer;
                base_UserInfo.State = (int)UserState.Normal;
                base_UserInfo.Password = Utils.MD5(base_UserInfo.Password);
                IBase_UserInfoService base_UserInfoService = BLLContainer.Resolve<IBase_UserInfoService>();
                if (!base_UserInfoService.Add(base_UserInfo))
                {
                    json.Msg = "数据库更新失败";
                }
                else
                {
                    json.Status = "y";
                    json.Msg = "注册成功";
                    json.ReUrl = "../home/index";
                    UserBusiness.Init();

                    Utils.FormSignOut();
                    var authTicket = new FormsAuthenticationTicket(1, base_UserInfo.UserName, DateTime.Now, DateTime.Now.AddDays(1),
                        base_UserInfo.REMEMBER_ME, base_UserInfo.ID.ToString());
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));
                    if (authTicket.IsPersistent) cookie.Expires = authTicket.Expiration;
                    Response.Cookies.Add(cookie);

                    Session["loginUser"] = UserBusiness.GetUser(base_UserInfo.UserName);
                    Session.Timeout = 300;
                }
            }
            catch (Exception e)
            {
                json.Msg = e.Message;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifyPassword()
        {
            string errMsg = "修改成功";
            
            try
            {
                string oldPassword = Utils.MD5(Request.Form["oldpass"]);
                var userInfoCacheModel = Session["loginUser"] as UserInfoCacheModel;
                if (userInfoCacheModel.Password != oldPassword)
                {
                    errMsg = "旧密码输入错误";
                    return RedirectToAction("ChangePassword", "UserManage", new { errMsg = errMsg });
                }

                int id = userInfoCacheModel.ID;
                string newPassword = Utils.MD5(Request.Form["newpass"]);                
                IBase_UserInfoService base_UserInfoService = BLLContainer.Resolve<IBase_UserInfoService>();
                var base_UserInfoModel = base_UserInfoService.GetModels(p => p.ID == id).FirstOrDefault();
                if (base_UserInfoModel == null)
                {
                    errMsg = "该用户不存在";
                    return RedirectToAction("ChangePassword", "UserManage", new { errMsg = errMsg });
                }

                base_UserInfoModel.Password = newPassword;
                if (base_UserInfoService.Update(base_UserInfoModel))
                {                    
                    userInfoCacheModel.Password = newPassword;
                    UserBusiness.Init();
                }
                else
                {
                    errMsg = "修改失败";
                }

            }
            catch (Exception e)
            {
                errMsg = e.Message;
            }

            return RedirectToAction("ChangePassword", "UserManage", new { errMsg = errMsg });
        }

        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUser(Base_UserInfo base_UserInfo)
        {
            var json = new JsonHelper() { Msg = "录入成功", Status = "n" };
            try
            {
                if (!string.IsNullOrWhiteSpace(base_UserInfo.Mobile) && !Utils.CheckMobile(base_UserInfo.Mobile))
                {
                    json.Msg = "非法的手机号码";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrWhiteSpace(base_UserInfo.Mail) && !Utils.IsValidEmail(base_UserInfo.Mail))
                {
                    json.Msg = "非法的邮箱地址";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (UserBusiness.IsEffectiveUser(base_UserInfo.UserName))
                {
                    json.Msg = "用户名已存在，请更换一个名称";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                base_UserInfo.UserType = (int)UserType.User;
                base_UserInfo.Password = Utils.MD5(base_UserInfo.Password);
                base_UserInfo.State = (int)UserState.Normal;
                IBase_UserInfoService base_UserInfoService = BLLContainer.Resolve<IBase_UserInfoService>();
                if (!base_UserInfoService.Add(base_UserInfo))
                {
                    json.Msg = "数据库更新失败";
                }
                else
                {                    
                    json.Status = "y";
                    UserBusiness.Init();               
                }
            }
            catch (Exception e)
            {
                json.Msg = e.Message;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditUser(int id)
        {            
            IBase_UserInfoService base_UserInfoService = BLLContainer.Resolve<IBase_UserInfoService>();
            var base_UserInfoModel = base_UserInfoService.GetModels(p => p.ID == id).FirstOrDefault();
            return View(base_UserInfoModel);
        }

        [HttpPost]
        public ActionResult EditUser(Base_UserInfo base_UserInfo)
        {
            var json = new JsonHelper() { Msg = "录入成功", Status = "n" };
            try
            {
                if (!string.IsNullOrWhiteSpace(base_UserInfo.Mobile) && !Utils.CheckMobile(base_UserInfo.Mobile))
                {
                    json.Msg = "非法的手机号码";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrWhiteSpace(base_UserInfo.Mail) && !Utils.IsValidEmail(base_UserInfo.Mail))
                {
                    json.Msg = "非法的邮箱地址";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (UserBusiness.UserInfoList.Any(p => p.ID != base_UserInfo.ID && p.UserName.Equals(base_UserInfo.UserName, StringComparison.OrdinalIgnoreCase)))
                {
                    json.Msg = "用户名已存在，请更换一个名称";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(base_UserInfo.Password))
                {
                    base_UserInfo.Password = UserBusiness.GetUser(base_UserInfo.ID).Password;
                }
                else
                {
                    base_UserInfo.Password = Utils.MD5(base_UserInfo.Password);
                }
                
                IBase_UserInfoService base_UserInfoService = BLLContainer.Resolve<IBase_UserInfoService>();
                if (!base_UserInfoService.Update(base_UserInfo))
                {
                    json.Msg = "数据库更新失败";                    
                }
                else
                {
                    json.Status = "y";
                    UserBusiness.Init();
                }                
            }
            catch (Exception e)
            {
                json.Msg = e.Message;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditCustomer(int id)
        {            
            IBase_UserInfoService base_UserInfoService = BLLContainer.Resolve<IBase_UserInfoService>();
            var base_UserInfoModel = base_UserInfoService.GetModels(p => p.ID == id).FirstOrDefault();
            GetUserLvls(base_UserInfoModel.UserLvl.HasValue ? base_UserInfoModel.UserLvl.ToString() : null);
            GetProductTypes(base_UserInfoModel.ProductType.HasValue ? base_UserInfoModel.ProductType.ToString() : null);
            return View(base_UserInfoModel);
        }

        [HttpPost]
        public ActionResult EditCustomer(Base_UserInfo base_UserInfo)
        {
            var json = new JsonHelper() { Msg = "录入成功", Status = "n" };
            try
            {
                if (!string.IsNullOrWhiteSpace(base_UserInfo.Mobile) && !Utils.CheckMobile(base_UserInfo.Mobile))
                {
                    json.Msg = "非法的手机号码";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (!string.IsNullOrWhiteSpace(base_UserInfo.Mail) && !Utils.IsValidEmail(base_UserInfo.Mail))
                {
                    json.Msg = "非法的邮箱地址";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (UserBusiness.UserInfoList.Any(p => p.ID != base_UserInfo.ID && p.UserName.Equals(base_UserInfo.UserName, StringComparison.OrdinalIgnoreCase)))
                {
                    json.Msg = "用户名已存在，请更换一个名称";
                    return Json(json, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(base_UserInfo.Password))
                {
                    base_UserInfo.Password = UserBusiness.GetUser(base_UserInfo.ID).Password;
                }
                else
                {
                    base_UserInfo.Password = Utils.MD5(base_UserInfo.Password);
                }

                IBase_UserInfoService base_UserInfoService = BLLContainer.Resolve<IBase_UserInfoService>();
                if (!base_UserInfoService.Update(base_UserInfo))
                {
                    json.Msg = "数据库更新失败";
                }
                else
                {
                    json.Status = "y";
                    UserBusiness.Init();
                }              
            }
            catch (Exception e)
            {
                json.Msg = e.Message;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        
        public string GetJson(string userId)
        {
            var json = new JavaScriptSerializer();

            try
            {
                //获取公司用户菜单列表
                string sql = " exec  SelectUserFunction @UserID";
                SqlParameter[] parameters = new SqlParameter[1];
                parameters[0] = new SqlParameter("@UserID", userId);
                System.Data.DataSet ds = FMHJJBLL.ExecuteQuerySentence(sql, parameters);
                if (ds.Tables.Count != 1)
                {
                    return string.Empty;
                }

                var list = Utils.GetModelList<UserFunctionsModel>(ds.Tables[0]);

                //将对象转换为JSON字符串      
                return json.Serialize(list);
            }
            catch
            {
                return string.Empty;
            }                        
        }

        public ActionResult UserLvl()
        {            
            return View(DictSystemBusiness.GetDicts("客户等级"));
        }

        public ActionResult Create(string dictKey)
        {
            string errMsg = string.Empty;

            if (string.IsNullOrEmpty(dictKey))
            {
                errMsg = "客户等级不能为空";
                return RedirectToAction("UserLvl", "UserManage", new { errMsg = errMsg });
            }            

            if (!DictSystemBusiness.AddModel("客户等级", dictKey, out errMsg))
            {
                return RedirectToAction("UserLvl", "UserManage", new { errMsg = errMsg });
            }

            return RedirectToAction("UserLvl", "UserManage", new { errMsg = "添加成功" });
        }

        public ActionResult Update(int id, string dictKey)
        {
            string errMsg = string.Empty;
            
            if (!DictSystemBusiness.UpdateModel(id, dictKey, out errMsg))
            {
                return RedirectToAction("UserLvl", "UserManage", new { errMsg = errMsg });
            }

            return RedirectToAction("UserLvl", "UserManage", new { errMsg = "更新成功" });
        }

        public ActionResult Delete(int id)
        {
            string errMsg = string.Empty;
            
            if (!DictSystemBusiness.DelModel(id, out errMsg))
            {
                return RedirectToAction("UserLvl", "UserManage", new { errMsg = errMsg });
            }

            return RedirectToAction("UserLvl", "UserManage", new { errMsg = "删除成功" });
        }

        public ActionResult GrantUser(int id)
        {
            ViewData["UserID"] = id;
            ViewData["UserName"] = UserBusiness.GetUser(id).UserName;
            return View();
        }

        [HttpPost]
        public string AddUserGrant(string userId, string ids)
        {
            //开启EF事务
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    List<string> idList = ids.Split(',').ToList();
                    IBase_UserInfo_GrantService base_UserInfo_GrantService = BLLContainer.Resolve<IBase_UserInfo_GrantService>();
                    var base_UserInfo_GrantList = base_UserInfo_GrantService.GetModels(p => p.UserID.ToString().Equals(userId, StringComparison.OrdinalIgnoreCase)).ToList();
                    foreach (var base_UserInfo_Grant in base_UserInfo_GrantList)
                    {
                        if (!base_UserInfo_GrantService.Delete(base_UserInfo_Grant))
                        {
                            return "权限添加失败";
                        }
                    }

                    foreach (var id in idList)
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            var base_UserInfo_Grant = new Base_UserInfo_Grant();
                            base_UserInfo_Grant.FunctionID = Convert.ToInt32(id);
                            base_UserInfo_Grant.UserID = Convert.ToInt32(userId);
                            base_UserInfo_Grant.IsAllow = 1;
                            if (!base_UserInfo_GrantService.Add(base_UserInfo_Grant))
                            {
                                return "权限添加失败";
                            }
                        }
                    }

                    ts.Complete();
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            
            return "权限添加成功";
        }

        [HttpPost]
        public string DelUser(int id)
        {
            IBase_UserInfoService base_UserInfoService = BLLContainer.Resolve<IBase_UserInfoService>();
            var base_UserInfo = base_UserInfoService.GetModels(p => p.ID == id).FirstOrDefault();
            if (base_UserInfo == null)
            {
                return "该用户已不存在";
            }
            base_UserInfo.State = (int)UserState.Canceled;
            if (!base_UserInfoService.Update(base_UserInfo))
            {
                return "注销用户失败";
            }

            //更新缓存
            UserBusiness.Init();

            return "操作成功";            
        }
    }
}