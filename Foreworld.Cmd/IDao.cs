using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Foreworld.Cmd
{
    public interface IDao<T, S>
    {
        T query(S @search);

        T query(string @id);

        List<T> queryAll(uint @topNum, Dictionary<string, string> @sort, S @search);

        List<T> queryAll(Pagination @pagination, Dictionary<string, string> @sort, S @search);

        List<T> queryAll(string @querySql, S @search, Pagination @pagination);

        long queryAllCount(S @search);

        void insert(T @entity);

        void update(T @entity);

        void delete(T @entity);

        void delete(string @id);

        void delete(string[] @ids);
    }
}
