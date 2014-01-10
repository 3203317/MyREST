using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Web;

using log4net;

using Foreworld.Cmd;
using Foreworld.Log;

namespace Foreworld.Rest
{
    public class CmdManager
    {
        private class MethodObject
        {
            public bool Public { get; set; }
            public MethodInfo MethodInfo { get; set; }
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(CmdManager));
        private static LogInfo _logInfo = new LogInfo();

        private Dictionary<string, object> _rests = null;
        private Dictionary<string, MethodObject> _resources = null;
        private Dictionary<string, IValidator> _validators = null;

        private CmdManager()
        {
            _logInfo.Msg = "CmdManager初始化...";
            _log.Info(_logInfo);
            _rests = new Dictionary<string, object>();
            _resources = new Dictionary<string, MethodObject>();
            _validators = new Dictionary<string, IValidator>();
            LoadAll();
        }

        private static volatile CmdManager _instance;
        private static object _syncObj = new Object();

        public static CmdManager INSTANCE
        {
            get
            {
                if (null == _instance)
                {
                    lock (_syncObj)
                    {
                        if (null == _instance) _instance = new CmdManager();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 加载全部的Dll
        /// </summary>
        private void LoadAll()
        {
            HttpContext context = HttpContext.Current;

            if (null != context)
            {
                string[] files_3 = Directory.GetFiles(context.Server.MapPath("./") + "App_Data", "*.Cmd.*.dll", SearchOption.AllDirectories);

                foreach (string file_4 in files_3)
                {
                    LoadDll(file_4);
                }
            }
        }

        /// <summary>
        /// 加载Dll
        /// </summary>
        /// <param name="file"></param>
        private void LoadDll(string @file)
        {
            ///* 独立域 */
            //FileStream __fileStream = File.Open(@file, FileMode.Open, FileAccess.Read);
            //long __fileLen = __fileStream.Length;
            //byte[] __bytes = new byte[__fileLen];
            //__fileStream.Read(__bytes, 0, (int)__fileLen);
            //__fileStream.Close();
            //Assembly __assembly = AppDomain.CurrentDomain.Load(__bytes);
            //Type[] __types = __assembly.GetTypes();

            /* 独占程序集 */
            Assembly assembly = Assembly.LoadFrom(@file);
            Type[] types = assembly.GetTypes();

            /* Foreworld.Cmd.Sysmanage.Validator.CommonValidator */
            object instance = assembly.CreateInstance(assembly.GetName().Name + ".Validator.CommonValidator");
            /* /sysmanage */
            _validators.Add("/" + assembly.GetName().Name.Split('.')[2].ToLower(), (IValidator)instance);

            foreach (Type type_3 in types)
            {
                if (null != type_3.GetInterface("IRest"))
                {
                    object instance_4 = assembly.CreateInstance(type_3.FullName);
                    string restName_4 = type_3.Name.Replace("Rest", string.Empty);
                    /* /Sysmanage/User */
                    string restFullName_4 = "/" + type_3.FullName.Split('.')[2] + "/" + restName_4;
                    restFullName_4 = restFullName_4.ToLower();
                    _rests.Add(restFullName_4, instance_4);

                    foreach (MethodInfo methodInfo_5 in type_3.GetMethods(BindingFlags.Instance | BindingFlags.Public))
                    {
                        object[] attrObjs_6 = (methodInfo_5.GetCustomAttributes(typeof(ResourceAttribute), false));

                        if (null != attrObjs_6 && 1 == attrObjs_6.Length)
                        {
                            MethodObject methodObject_7 = new MethodObject();
                            methodObject_7.Public = ((ResourceAttribute)attrObjs_6[0]).Public;
                            methodObject_7.MethodInfo = methodInfo_5;

                            string methodName_7 = methodInfo_5.Name;
                            /* /sysmanage/user/loginUI */
                            methodName_7 = restFullName_4 + "/" + methodName_7.ToLower();
                            _resources.Add(methodName_7, methodObject_7);

                            _logInfo.Msg = "加载资源：" + methodName_7;
                            _log.Info(_logInfo);
                        }
                    }
                }
            }
            _logInfo.Msg = "加载DLL：" + @file;
            _log.Info(_logInfo);
        }

        /// <summary>
        /// 重新加载
        /// </summary>
        public void Reload()
        {
            Destory();
            LoadAll();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Destory()
        {
            foreach (IRest rest_3 in _rests.Values)
            {
                rest_3.Destroy();
            }
            _resources.Clear();
            _rests.Clear();
            foreach (IValidator validator_3 in _validators.Values)
            {
                validator_3.Destroy();
            }
            _validators.Clear();
        }

        /// <summary>
        /// 加载一个或多个
        /// </summary>
        public void Load() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdName">/sysmanage/user/loginUI</param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ResultMapper Exec(string @cmdName, Parameter @parameter)
        {
            LogInfo logInfo = @parameter.LogInfo;
            logInfo.Msg = "请求资源：" + @cmdName;
            _log.Info(logInfo);

            /* 获取方法 */
            MethodObject methodObject = _resources[@cmdName];
            /* /sysmanage/user */
            string restName = @cmdName.Substring(0, @cmdName.LastIndexOf('/'));

            if (!methodObject.Public)
            {
                /* /sysmanage */
                string dllName_3 = restName.Substring(0, restName.LastIndexOf('/'));
                /* 验证器 */
                IValidator validator_3 = _validators[dllName_3];

                ResultMapper mapper_3 = validator_3.Validate(@parameter);
                if (!mapper_3.Success)
                {
                    return mapper_3;
                }
            }

            /* 获取实例 */
            object instance = _rests[restName];
            object[] paramObjs = { @parameter };

            try
            {
                ResultMapper mapper_3 = methodObject.MethodInfo.Invoke(instance, paramObjs) as ResultMapper;
                return mapper_3 == null ? new ResultMapper() : mapper_3;
            }
            catch (Exception @ex)
            {
                logInfo.Msg = @ex.Message;
                _log.Error(logInfo);
                return new ResultMapper();
            }
        }
    }
}
