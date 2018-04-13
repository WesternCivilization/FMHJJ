using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMHJJService.DAL.Base;

namespace FMHJJService.BLL.Container
{
    public class BLLContainer
    {
        public static Dictionary<string, IContainer> containerDict = new Dictionary<string, IContainer>();

        /// <summary>
        /// 获取 IDal 的实例化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>(string xcGameDBName = "")
        {
            try
            {
                string containerName = GetContainerName(typeof(T), xcGameDBName);
                if (!string.IsNullOrEmpty(containerName))
                {
                    if (!containerDict.ContainsKey(containerName))
                    {
                        Initialise(containerName);
                    }
                    return containerDict[containerName].Resolve<T>(new NamedParameter("containerName", containerName));
                }
                else
                {
                    return default(T);
                }
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("IOC实例化出错!" + ex.Message);
            }       
        }

        private static string GetContainerName(Type type, string xcGameDBName)
        {
            if (type.FullName.ToLower().IndexOf("fmhjj.") > 0)
            {
                return ContainerConstant.FMHJJIOCContainer;
            }            
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialise(string containerName)
        {
            var builder = new ContainerBuilder();
            if (containerName.Equals(ContainerConstant.FMHJJIOCContainer))
            {
                //FMHJJ注册
                builder.RegisterType<FMHJJService.BLL.FMHJJ.Base_UserInfoService>().As<FMHJJService.BLL.IBLL.FMHJJ.IBase_UserInfoService>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.BLL.FMHJJ.Base_ProductInfoService>().As<FMHJJService.BLL.IBLL.FMHJJ.IBase_ProductInfoService>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.BLL.FMHJJ.Base_ProductInfo_DetailService>().As<FMHJJService.BLL.IBLL.FMHJJ.IBase_ProductInfo_DetailService>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.BLL.FMHJJ.Dict_SystemService>().As<FMHJJService.BLL.IBLL.FMHJJ.IDict_SystemService>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.BLL.FMHJJ.Base_UserInfo_GrantService>().As<FMHJJService.BLL.IBLL.FMHJJ.IBase_UserInfo_GrantService>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.BLL.FMHJJ.Data_BidManageService>().As<FMHJJService.BLL.IBLL.FMHJJ.IData_BidManageService>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.BLL.FMHJJ.Data_ParametersService>().As<FMHJJService.BLL.IBLL.FMHJJ.IData_ParametersService>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.BLL.FMHJJ.Data_SmsManageService>().As<FMHJJService.BLL.IBLL.FMHJJ.IData_SmsManageService>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.BLL.FMHJJ.Dict_FunctionMenuService>().As<FMHJJService.BLL.IBLL.FMHJJ.IDict_FunctionMenuService>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.BLL.FMHJJ.Flw_BiddingInfoService>().As<FMHJJService.BLL.IBLL.FMHJJ.IFlw_BiddingInfoService>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.BLL.FMHJJ.Flw_BiddingNoticeService>().As<FMHJJService.BLL.IBLL.FMHJJ.IFlw_BiddingNoticeService>().InstancePerLifetimeScope();
            }
            
            IContainer container = builder.Build();
            containerDict[containerName] = container;
        }
    }
}
