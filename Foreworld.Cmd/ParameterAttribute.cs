using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd
{
    public enum RepositoryType
    {
        NONE,
        SESSION
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ParameterAttribute : Attribute
    {
        public ParameterAttribute(string name, string description)
        {
            _name = name;
            _description = description;
        }

        private string _name = string.Empty;
        private string _description = string.Empty;
        private bool _required = false;

        private ParameterType _type = ParameterType.OBJECT;
        private ParameterType _collectionType = ParameterType.OBJECT;

        private bool _expose = true;
        private bool _includeInApiDoc = true;

        private string _regexp = string.Empty;
        private string _regexpInfo = string.Empty;

        private RepositoryType _repository = RepositoryType.NONE;

        private string _defaultValue = string.Empty;


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public bool Required
        {
            get { return _required; }
            set { _required = value; }
        }

        public ParameterType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public ParameterType CollectionType
        {
            get { return _collectionType; }
            set { _collectionType = value; }
        }

        public bool Expose
        {
            get { return _expose; }
            set { _expose = value; }
        }

        public bool IncludeInApiDoc
        {
            get { return _includeInApiDoc; }
            set { _includeInApiDoc = value; }
        }

        public string Regexp
        {
            get { return _regexp; }
            set { _regexp = value; }
        }

        public string RegexpInfo
        {
            get { return _regexpInfo; }
            set { _regexpInfo = value; }
        }

        public RepositoryType Repository
        {
            get { return _repository; }
            set { _repository = value; }
        }

        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }
    }
}
