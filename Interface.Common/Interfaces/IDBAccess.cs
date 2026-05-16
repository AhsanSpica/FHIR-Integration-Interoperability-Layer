using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Misc.Interfaces
{
    public interface IDBAccess : IDisposable
    {
        Task<T> Get<T>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure);
        Task<List<T>> GetAll<T>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure);
        Task<Tuple<IEnumerable<T>, IEnumerable<T1>>> GetAllMultiple<T, T1>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure);
        Task<Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>>> GetAllMultiple1<T, T1, T2>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure);
        Task<Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>> GetAllMultiple2<T, T1, T2, T3>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure);
        Task<Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>> GetAllMultiple3<T, T1, T2, T3, T4>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure);
        Task<Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>>> GetAllMultiple4<T, T1, T2, T3, T4, T5>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure);
        Task<Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>>> GetAllMultiple5<T, T1, T2, T3, T4, T5, T6>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure);
        Task<Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>>> GetAllMultiple6<T, T1, T2, T3, T4, T5, T6, T7>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure);
        Task<int> Execute(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure);
        Task<T> Insert<T>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure);
        Task<T> Update<T>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure);
        Task<T> Delete<T>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure);
        string GetConnectionString(string? Name = null);
    }
}


