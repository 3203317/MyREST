using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace Foreworld.Cmd
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string @description)
        {
            Description = @description;
        }

        public string Name { get; set; }

        private int _length = 255;
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        private bool _unique = false;
        /// <summary>
        /// 唯一性
        /// </summary>
        public bool Unique
        {
            get { return _unique; }
            set { _unique = value; }
        }

        private bool _nullable = false;
        /// <summary>
        /// 可空的
        /// </summary>
        public bool Nullable
        {
            get { return _nullable; }
            set { _nullable = value; }
        }

        private bool _id = false;
        /// <summary>
        /// 主键
        /// </summary>
        public bool Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public SqlDbType SqlDbType { get; set; }
        public OleDbType OleDbType { get; set; }

        public String DefaultValue { get; set; }
    }
}
