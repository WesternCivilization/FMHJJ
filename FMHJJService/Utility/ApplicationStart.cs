using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FMHJJService.Common;
using FMHJJService.Common.Enum;
using FMHJJService.Business.FMHJJ;
using System.Data.Entity;
using FMHJJService.Model.CustomModel.FMHJJ;

namespace FMHJJService.Utility
{
    public class ApplicationStart
    {
        public static void Init()
        {
            // 在应用程序启动时运行的代码
            try
            {
                //LogHelper.SaveLog(TxtLogType.SystemInit, "********************************Application Start********************************");                
                UserInit();
                DictSystemInit();
                ProductTypeInit();
                DataParameterInit();
            }
            catch
            {
                //LogHelper.SaveLog(TxtLogType.SystemInit, "Init..." + Utils.GetException(e));
            }
        }        

        public static void UserInit()
        {
            try
            {
                UserBusiness.Init();
                //LogHelper.SaveLog(TxtLogType.SystemInit, "UserInit Sucess");
            }
            catch
            {
                //LogHelper.SaveLog(TxtLogType.SystemInit, "UserInit..." + Utils.GetException(ex));
            }     
        }

        public static void ProductTypeInit()
        {
            try
            {
                ProductTypeBusiness.Init();
                //LogHelper.SaveLog(TxtLogType.SystemInit, "ProductTypeInit Sucess");
            }
            catch
            {
                //LogHelper.SaveLog(TxtLogType.SystemInit, "ProductTypeInit..." + Utils.GetException(ex));
            }
        }

        public static void DictSystemInit()
        {
            try
            {
                DictSystemBusiness.Init();
                //LogHelper.SaveLog(TxtLogType.SystemInit, "DictSystemInit Sucess");
            }
            catch
            {
                //LogHelper.SaveLog(TxtLogType.SystemInit, "DictSystemInit..." + Utils.GetException(ex));
            }
        }

        public static void DataParameterInit()
        {
            try
            {
                DataParameterBusiness.Init();
                //LogHelper.SaveLog(TxtLogType.SystemInit, "DataParameterInit Sucess");
            }
            catch
            {
                //LogHelper.SaveLog(TxtLogType.SystemInit, "DataParameterInit..." + Utils.GetException(ex));
            }
        }
    }
}
