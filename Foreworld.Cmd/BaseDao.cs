#define DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

using log4net;
using MySql.Data.MySqlClient;

using Foreworld.Log;

namespace Foreworld.Cmd
{
    public abstract class BaseDao<T, S> where T : new()
    {
        private string _connectionString = ConfigurationSettings.AppSettings["connectionString"];
        public virtual string ConnectionString
        {
            get { return _connectionString; }
        }

        private String _querySql = null;
        /* 字段名逗号分割 */
        private String _fieldSql = null;
        private Type _type = null;
        private List<PropertyInfo> _propInfos = null;
        /* 数据库表名 */
        private String _dbTableName = null;

        /// <summary>
        /// 设置查询SQL
        /// </summary>
        private void SetQuerySql()
        {
            foreach (PropertyInfo __propInfo_3 in _propInfos)
            {
                _querySql += "," + __propInfo_3.Name;
            }

            _fieldSql = _querySql.Substring(1);
            _querySql = "SELECT " + _fieldSql;

            TableAttribute __tabAttr = (TableAttribute)(_type.GetCustomAttributes(typeof(TableAttribute), false)[0]);
            _querySql += " FROM " + __tabAttr.Name;
            _dbTableName = __tabAttr.Name;
        }

        /// <summary>
        /// 设置字段属性
        /// </summary>
        private void SetPropertyInfo()
        {
            _propInfos = new List<PropertyInfo>();

            foreach (PropertyInfo __propInfo_3 in _type.GetProperties())
            {
                object[] __obj_4 = __propInfo_3.GetCustomAttributes(typeof(ColumnAttribute), false);

                if (1 == __obj_4.Length)
                {
                    _propInfos.Add(__propInfo_3);
                }
            }
        }

        private static readonly ILog _log = LogManager.GetLogger(typeof(BaseDao<T, S>));

        public BaseDao()
        {
            _type = typeof(T);

            /*  */
            SetPropertyInfo();
            /* */
            SetQuerySql();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public T query(T search)
        {
            string __sql = string.Empty;

            List<MySqlParameter> __sps = new List<MySqlParameter>();
            MySqlParameter __sp = null;

            foreach (PropertyInfo __propInfo_3 in _propInfos)
            {
                var __objVal_4 = _type.GetProperty(__propInfo_3.Name).GetValue(search, null);

                if (null != __objVal_4)
                {
                    __sql += " AND " + __propInfo_3.Name + "=?" + __propInfo_3.Name;
                    object[] __obj_5 = __propInfo_3.GetCustomAttributes(typeof(ColumnAttribute), false);
                    ColumnAttribute __colAttr_5 = (ColumnAttribute)__obj_5[0];

                    __sp = new MySqlParameter("?" + __propInfo_3.Name, __colAttr_5.MySqlDbType, __colAttr_5.Length);
                    __sp.Value = __objVal_4;
                    __sps.Add(__sp);
                }
            }

            if (!string.Empty.Equals(__sql))
            {
                __sql = " WHERE 1=1" + __sql;
            }

            __sql = _querySql + __sql;

            try
            {
                DataRow dr_3 = MySqlHelper.ExecuteDataRow(ConnectionString, __sql, __sps.ToArray());
                if (null != dr_3)
                {
                    foreach (PropertyInfo __propInfo_5 in _propInfos)
                    {
                        object __propVal_6 = dr_3[__propInfo_5.Name];

                        if (!(__propVal_6 is System.DBNull))
                        {
                            _type.GetProperty(__propInfo_5.Name).SetValue(search, __propVal_6 is System.DateTime ? __propVal_6.ToString() : __propVal_6, null);
                        }
                    }
                }
                else
                {
                    search = default(T);
                }
            }
            catch (Exception @ex)
            {
                _log.Error(@ex.Message);
                search = default(T);
            }

            return search;
        }

        public T query(string @id)
        {

            object o = new object();
            return (T)o;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topNum"></param>
        /// <param name="sort"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<T> queryAll(uint @topNum, Dictionary<string, string> @sort, S @search)
        {
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="sort"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<T> queryAll(Pagination @pagination, Dictionary<string, string> @sort, S @search)
        {
            LogInfo __logInfo = new LogInfo();
            string __sql = string.Empty;

            List<MySqlParameter> __sps = new List<MySqlParameter>();
            MySqlParameter __sp = null;

            if (null != @search)
            {
                foreach (PropertyInfo __propInfo_3 in _propInfos)
                {
                    var __objVal_4 = _type.GetProperty(__propInfo_3.Name).GetValue(@search, null);

                    if (null != __objVal_4)
                    {
                        __sql += " AND " + __propInfo_3.Name + "=?" + __propInfo_3.Name;

                        object[] __obj_5 = __propInfo_3.GetCustomAttributes(typeof(ColumnAttribute), false);
                        ColumnAttribute __colAttr_5 = (ColumnAttribute)__obj_5[0];
                        __sp = new MySqlParameter("?" + __propInfo_3.Name, __colAttr_5.MySqlDbType, __colAttr_5.Length);
                        __sp.Value = __objVal_4;
                        __sps.Add(__sp);
#if DEBUG
                        __logInfo.Msg = __sp + ": " + __sp.Value;
                        __logInfo.Code = "SQLParam";
                        _log.Debug(__logInfo);
#endif
                    }
                }
            }

            if (!string.Empty.Equals(__sql))
            {
                __sql = " WHERE 1=1" + __sql;
            }

            __sql = _querySql + __sql;

            /* 排序 */
            if (null != @sort)
            {
                __sql += " ORDER BY";
                foreach (string __key_3 in @sort.Keys)
                {
                    __sql += " " + __key_3 + " " + @sort[__key_3] + ",";
                }
                __sql = __sql.Substring(0, __sql.Length - 1);
            }

            if (null != @pagination)
            {
                __sql += " limit " + (@pagination.Current - 1) * @pagination.PageSize + "," + @pagination.PageSize;
            }

#if DEBUG
            __logInfo.Msg = __sql;
            __logInfo.Code = "SQL";
            _log.Debug(__logInfo);
#endif

            DataSet __ds = null;
            List<T> __list = null;
            try
            {
                __ds = MySqlHelper.ExecuteDataset(ConnectionString, __sql, __sps.ToArray());

                if (null != __ds)
                {
                    __list = new List<T>();

                    DataTable __dt_3 = __ds.Tables[0];
                    DataRowCollection __rows_3 = __dt_3.Rows;
                    DataColumnCollection __columns_3 = __dt_3.Columns;

                    for (int __i_3 = 0, __j_3 = __rows_3.Count, __k_3 = __columns_3.Count; __i_3 < __j_3; __i_3++)
                    {
                        DataRow __row_4 = __rows_3[__i_3];

                        T __t = new T();

                        foreach (PropertyInfo __propInfo_5 in _propInfos)
                        {
                            object __propVal_6 = __row_4[__propInfo_5.Name];

                            if (!(__propVal_6 is System.DBNull))
                            {
                                _type.GetProperty(__propInfo_5.Name).SetValue(__t, __propVal_6 is System.DateTime ? __propVal_6.ToString() : __propVal_6, null);
                            }
                        }
                        __list.Add(__t);
                    }
                }
            }
            catch (Exception @ex)
            {
                _log.Error(@ex.Message);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="querySql"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<T> queryAll(string @querySql, S @search)
        {
            LogInfo __logInfo = new LogInfo();

            string __sql = @querySql;
            __sql = __sql.Replace("*", _fieldSql);

            List<MySqlParameter> __sps = new List<MySqlParameter>();
            MySqlParameter __sp = null;

            if (null != @search)
            {
                foreach (PropertyInfo __propInfo_3 in _propInfos)
                {
                    var __objVal_4 = _type.GetProperty(__propInfo_3.Name).GetValue(@search, null);

                    if (null != __objVal_4)
                    {
                        object[] __obj_5 = __propInfo_3.GetCustomAttributes(typeof(ColumnAttribute), false);
                        ColumnAttribute __colAttr_5 = (ColumnAttribute)__obj_5[0];
                        __sp = new MySqlParameter("?" + __propInfo_3.Name, __colAttr_5.MySqlDbType, __colAttr_5.Length);
                        __sp.Value = __objVal_4;
                        __sps.Add(__sp);
#if DEBUG
                        __logInfo.Msg = __sp + ": " + __sp.Value;
                        __logInfo.Code = "SQLParam";
                        _log.Debug(__logInfo);
#endif
                    }
                }
            }

#if DEBUG
            __logInfo.Msg = __sql;
            __logInfo.Code = "SQL";
            _log.Debug(__logInfo);
#endif

            DataSet __ds = null;
            List<T> __list = null;
            try
            {
                __ds = MySqlHelper.ExecuteDataset(ConnectionString, __sql, __sps.ToArray());

                if (null != __ds)
                {
                    __list = new List<T>();

                    DataTable __dt_3 = __ds.Tables[0];
                    DataRowCollection __rows_3 = __dt_3.Rows;
                    DataColumnCollection __columns_3 = __dt_3.Columns;

                    for (int __i_3 = 0, __j_3 = __rows_3.Count, __k_3 = __columns_3.Count; __i_3 < __j_3; __i_3++)
                    {
                        DataRow __row_4 = __rows_3[__i_3];

                        T __t = new T();

                        foreach (PropertyInfo __propInfo_5 in _propInfos)
                        {
                            object __propVal_6 = __row_4[__propInfo_5.Name];

                            if (!(__propVal_6 is System.DBNull))
                            {
                                _type.GetProperty(__propInfo_5.Name).SetValue(__t, __propVal_6 is System.DateTime ? __propVal_6.ToString() : __propVal_6, null);
                            }
                        }
                        __list.Add(__t);
                    }
                }
            }
            catch (Exception @ex)
            {
                _log.Error(@ex.Message);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public long queryAllCount(S @search)
        {
            string __sql = string.Empty;

            List<MySqlParameter> __sps = new List<MySqlParameter>();

            if (null != search)
            {
                MySqlParameter __sp = null;

                foreach (PropertyInfo __propInfo_3 in _propInfos)
                {
                    var __objVal_4 = _type.GetProperty(__propInfo_3.Name).GetValue(@search, null);

                    if (null != __objVal_4)
                    {
                        __sql += " AND " + __propInfo_3.Name + "=?" + __propInfo_3.Name;

                        object[] __obj_5 = __propInfo_3.GetCustomAttributes(typeof(ColumnAttribute), false);
                        ColumnAttribute __colAttr_5 = (ColumnAttribute)__obj_5[0];
                        __sp = new MySqlParameter("?" + __propInfo_3.Name, __colAttr_5.MySqlDbType, __colAttr_5.Length);
                        __sp.Value = __objVal_4;
                        __sps.Add(__sp);
                    }
                }

                if (!string.Empty.Equals(__sql))
                {
                    __sql = " WHERE 1=1" + __sql;
                }
            }

            __sql = "SELECT COUNT(1) FROM " + _dbTableName + __sql;

            string __count = string.Empty;

            try
            {
                __count = MySqlHelper.ExecuteScalar(ConnectionString, __sql, __sps.ToArray()).ToString().Trim();
            }
            catch (Exception @ex)
            {
                _log.Error(@ex.Message);
            }
            finally
            {
                if (string.Empty == __count)
                {
                    __count = "0";
                }
            }

            return Convert.ToInt64(__count);
        }

        public void insert(T @entity)
        {
        }

        public void update(T @entity)
        {
        }

        public void delete(T @entity)
        {
        }

        public void delete(string @id)
        {
        }

        public void delete(string[] @ids)
        {
        }
    }
}
