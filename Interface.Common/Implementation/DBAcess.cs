using Dapper;
 using Interface.Misc.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Misc.Implementation
{
    public class DBAccess : IDBAccess
    {
        private readonly string DefaultConnectionstringName = "DefaultConnection";

        private readonly IConfiguration _configuration;

        public DBAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Dispose()
        {
            //TODO: Do something here please
        }
        public async Task<int> Execute(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            return await db.ExecuteAsync(sp, parms, commandType: commandType, commandTimeout: 180);
        }

        public async Task<T> Get<T>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            return (await db.QueryAsync<T>(sp, parms, commandType: commandType, commandTimeout: 180)).FirstOrDefault()!;
        }

        public async Task<List<T>> GetAll<T>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            return (await db.QueryAsync<T>(sp, parms, commandType: commandType, commandTimeout: 180)).ToList();
        }

        public async Task<T> Insert<T>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(ConnectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                try
                {
                    result = (await db.QueryAsync<T>(sp, parms, commandType: commandType, commandTimeout: 180)).FirstOrDefault()!;
                }
                catch (Exception) { throw; }
            }
            catch (Exception) { throw; }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return result;
        }

        public async Task<T> Update<T>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(ConnectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                try
                {
                    result = (await db.QueryAsync<T>(sp, parms, commandType: commandType, commandTimeout: 180)).FirstOrDefault()!;
                }
                catch (Exception) { throw; }
            }
            catch (Exception) { throw; }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return result;
        }

        public async Task<T> Delete<T>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(ConnectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                try
                {
                    result = (await db.QueryAsync<T>(sp, parms, commandType: commandType, commandTimeout: 180)).FirstOrDefault()!;
                }
                catch (Exception) { throw; }
            }
            catch (Exception) { throw; }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }
            return result;
        }

        public async Task<Tuple<IEnumerable<T>, IEnumerable<T1>>> GetAllMultiple<T, T1>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            var reader = await db.QueryMultipleAsync(sp, parms, commandType: commandType, commandTimeout: 180);
            var list1 = await reader.ReadAsync<T>();
            var list2 = await reader.ReadAsync<T1>();
            return new Tuple<IEnumerable<T>, IEnumerable<T1>>(list1, list2);
        }

        public async Task<Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>>> GetAllMultiple1<T, T1, T2>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            var reader = await db.QueryMultipleAsync(sp, parms, commandType: commandType, commandTimeout: 180);
            var list1 = await reader.ReadAsync<T>();
            var list2 = await reader.ReadAsync<T1>();
            var list3 = await reader.ReadAsync<T2>();
            return new Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>>(list1, list2, list3);
        }

        public async Task<Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>> GetAllMultiple2<T, T1, T2, T3>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            var reader = await db.QueryMultipleAsync(sp, parms, commandType: commandType, commandTimeout: 180);
            var list1 = await reader.ReadAsync<T>();
            var list2 = await reader.ReadAsync<T1>();
            var list3 = await reader.ReadAsync<T2>();
            var list4 = await reader.ReadAsync<T3>();
            return new Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>>(list1, list2, list3, list4);
        }

        public async Task<Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>> GetAllMultiple3<T, T1, T2, T3, T4>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            var reader = await db.QueryMultipleAsync(sp, parms, commandType: commandType, commandTimeout: 180);
            var list1 = await reader.ReadAsync<T>();
            var list2 = await reader.ReadAsync<T1>();
            var list3 = await reader.ReadAsync<T2>();
            var list4 = await reader.ReadAsync<T3>();
            var list5 = await reader.ReadAsync<T4>();
            return new Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>>(list1, list2, list3, list4, list5);
        }

        public async Task<Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>>> GetAllMultiple4<T, T1, T2, T3, T4, T5>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            var reader = await db.QueryMultipleAsync(sp, parms, commandType: commandType, commandTimeout: 180);
            var list1 = await reader.ReadAsync<T>();
            var list2 = await reader.ReadAsync<T1>();
            var list3 = await reader.ReadAsync<T2>();
            var list4 = await reader.ReadAsync<T3>();
            var list5 = await reader.ReadAsync<T4>();
            var list6 = await reader.ReadAsync<T5>();
            return new Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>>(list1, list2, list3, list4, list5, list6);
        }

        public async Task<Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>>> GetAllMultiple5<T, T1, T2, T3, T4, T5, T6>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            var reader = await db.QueryMultipleAsync(sp, parms, commandType: commandType, commandTimeout: 180);
            var list1 = await reader.ReadAsync<T>();
            var list2 = await reader.ReadAsync<T1>();
            var list3 = await reader.ReadAsync<T2>();
            var list4 = await reader.ReadAsync<T3>();
            var list5 = await reader.ReadAsync<T4>();
            var list6 = await reader.ReadAsync<T5>();
            var list7 = await reader.ReadAsync<T6>();
            return new Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>>(list1, list2, list3, list4, list5, list6, list7);
        }

        public async Task<Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>>> GetAllMultiple6<T, T1, T2, T3, T4, T5, T6, T7>(string sp, DynamicParameters parms, string ConnectionString, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(ConnectionString);
            var reader = await db.QueryMultipleAsync(sp, parms, commandType: commandType, commandTimeout: 180);
            var list1 = await reader.ReadAsync<T>();
            var list2 = await reader.ReadAsync<T1>();
            var list3 = await reader.ReadAsync<T2>();
            var list4 = await reader.ReadAsync<T3>();
            var list5 = await reader.ReadAsync<T4>();
            var list6 = await reader.ReadAsync<T5>();
            var list7 = await reader.ReadAsync<T6>();
            var list8 = await reader.ReadAsync<T7>();
            return new Tuple<IEnumerable<T>, IEnumerable<T1>, IEnumerable<T2>, IEnumerable<T3>, IEnumerable<T4>, IEnumerable<T5>, IEnumerable<T6>, IEnumerable<T7>>(list1, list2, list3, list4, list5, list6, list7, list8);
        }

        public string GetConnectionString(string? Name = null)
        {
            if (string.IsNullOrEmpty(Name))
                Name = DefaultConnectionstringName;
            return _configuration.GetConnectionString(Name)!;
        }
    }
}


