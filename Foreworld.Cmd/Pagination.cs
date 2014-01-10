using System;
using System.Collections.Generic;
using System.Text;

namespace Foreworld.Cmd
{
    public class Pagination
    {
        private UInt32 _current = 1;
        private UInt32 _pageSize = 20;

        public UInt32 Current
        {
            get { return _current; }
            set { _current = (0 == value ? 1 : value); }
        }

        public UInt32 PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
    }
}
