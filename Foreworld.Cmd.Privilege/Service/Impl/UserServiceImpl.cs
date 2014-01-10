#define DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using log4net;

using Foreworld.Db;

using Foreworld.Cmd.Privilege.Model;
using Foreworld.Cmd.Privilege.Dao;
using Foreworld.Cmd.Privilege.Dao.Impl;

namespace Foreworld.Cmd.Privilege.Service.Impl
{
    public class UserServiceImpl : BaseService, UserService
    {
        private UserDao _userDao;

        public UserServiceImpl()
        {
            _userDao = new UserDaoImpl();
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(UserServiceImpl));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public User FindUserByName(string @name)
        {
            User __user = new User();
            __user.UserName = @name;
            __user.IsEnable = 1;
            __user.IsInvalid = 1;

            __user = _userDao.query(__user);
            return __user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Module> GetMenuTreeById(string @id)
        {
            //            string __sql = @"SELECT DISTINCT T.* FROM (
            //                                SELECT A.* FROM S_MODULE A, s_modopt B,s_role_modopt C
            //                                WHERE A.modtype=1
            //                                AND A.IsInvalid = 1 AND A.IsEnable=1
            //                                AND B.IsInvalid = 1 AND B.IsEnable=1
            //                                AND C.IsInvalid = 1 AND C.IsEnable=1
            //                                AND A.id = B.tab_s_module_id 
            //                                AND B.tab_p_code_moptype_id=1
            //                                AND C.tab_s_modopt_id=B.id
            //                                AND C.tab_s_role_id IN (SELECT tab_s_role_id  FROM s_user_role WHERE tab_s_user_id=@userid)
            //                            ) T
            //                            ORDER BY T.p_id,T.sort,T.id ASC";
            string __sql = @"SELECT DISTINCT T.* FROM (
                                SELECT A.* FROM S_MODULE A
                            ) T
                            ORDER BY T.PModuleId,T.Sort,T.ModuleId ASC";

            SqlParameter[] __sps = new SqlParameter[1];
            __sps[0] = new SqlParameter("@userid", SqlDbType.VarChar);
            __sps[0].Value = @id;

            List<Module> __list = new List<Module>();
            DataSet __ds = null;
            try
            {
                __ds = SqlHelper.ExecuteDataSet(ConnectionString, CommandType.Text, __sql, __sps);

                if (null != __ds)
                {
                    DataTable __dt = __ds.Tables[0];
                    DataRowCollection __rows = __dt.Rows;
                    DataColumnCollection __columns = __dt.Columns;

                    for (int __i_3 = 0, __j_3 = __rows.Count, __k_3 = __columns.Count; __i_3 < __j_3; __i_3++)
                    {
                        DataRow __row_4 = __rows[__i_3];

                        Module __module = new Module();
                        __module.ModuleId = __row_4["ModuleId"].ToString();
                        __module.PModuleId = __row_4["PModuleId"].ToString();
                        __module.ModuleName = __row_4["ModuleName"].ToString();
                        __module.ModuleUrl = __row_4["ModuleUrl"].ToString();

                        __list.Add(__module);
                    }
                }
            }
            catch (Exception @ex)
            {
                _log.Error(@ex);
            }
            finally
            {
                if (null != __ds)
                {
                    __ds.Clear();
                    __ds.Dispose();
                }
            }

            return __list;
        }
    }
}
