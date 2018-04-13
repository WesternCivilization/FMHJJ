using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMHJJService.DAL.IDAL;
using FMHJJService.DAL.Base;

namespace FMHJJService.DAL.Container
{
    public class DALContainer
    {
        public static Dictionary<string, IContainer> containerDict = new Dictionary<string, IContainer>();
        ///// <summary>
        ///// IOC 容器
        ///// </summary>
        //public static IContainer container = null;


        //public static IContainer Container
        //{
        //    set { container = value; }
        //    get { return container; }
        //}
 
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
                builder.RegisterType<FMHJJService.DAL.FMHJJ.Base_UserInfoDAL>().As<FMHJJService.DAL.IDAL.FMHJJ.IBase_UserInfoDAL>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.DAL.FMHJJ.Base_ProductInfoDAL>().As<FMHJJService.DAL.IDAL.FMHJJ.IBase_ProductInfoDAL>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.DAL.FMHJJ.Base_ProductInfo_DetailDAL>().As<FMHJJService.DAL.IDAL.FMHJJ.IBase_ProductInfo_DetailDAL>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.DAL.FMHJJ.Dict_SystemDAL>().As<FMHJJService.DAL.IDAL.FMHJJ.IDict_SystemDAL>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.DAL.FMHJJ.Base_UserInfo_GrantDAL>().As<FMHJJService.DAL.IDAL.FMHJJ.IBase_UserInfo_GrantDAL>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.DAL.FMHJJ.Data_BidManageDAL>().As<FMHJJService.DAL.IDAL.FMHJJ.IData_BidManageDAL>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.DAL.FMHJJ.Data_ParametersDAL>().As<FMHJJService.DAL.IDAL.FMHJJ.IData_ParametersDAL>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.DAL.FMHJJ.Data_SmsManageDAL>().As<FMHJJService.DAL.IDAL.FMHJJ.IData_SmsManageDAL>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.DAL.FMHJJ.Dict_FunctionMenuDAL>().As<FMHJJService.DAL.IDAL.FMHJJ.IDict_FunctionMenuDAL>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.DAL.FMHJJ.Flw_BiddingInfoDAL>().As<FMHJJService.DAL.IDAL.FMHJJ.IFlw_BiddingInfoDAL>().InstancePerLifetimeScope();
                builder.RegisterType<FMHJJService.DAL.FMHJJ.Flw_BiddingNoticeDAL>().As<FMHJJService.DAL.IDAL.FMHJJ.IFlw_BiddingNoticeDAL>().InstancePerLifetimeScope();
            }
            
            IContainer container = builder.Build();
            containerDict[containerName] = container;
        }
    }
}
