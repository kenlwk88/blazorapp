using System.Data.SqlClient;


namespace HLA.CSSMS.Server.Data
{
    public class ApplicationDbContext
    {
        protected readonly IConfiguration _configuration;
        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("SqlConnection"));
        }
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql)
        {
            using var conn = CreateConnection();
            return await conn.QueryAsync<T>(sql);
        }
        public async Task ExecuteAsync(string sql, object? para = null)
        {
            using var conn = CreateConnection();
            await conn.ExecuteAsync(sql, para);
        }
        public async Task<int> ExecuteResultAsync(string sql, object? para = null)
        {
            using var conn = CreateConnection();
            return await conn.ExecuteAsync(sql, para);
        }
        public async Task<IEnumerable<T>> QueryStoredProcedureAsync<T>(string sp, object? para = null)
        {
            using var conn = CreateConnection();
            return await conn.QueryAsync<T>(sp, para, commandType: CommandType.StoredProcedure);
        }
        public async Task<int> ExecuteStoredProcedureAsync(string sp, object? para = null)
        {
            using var conn = CreateConnection();
            return await conn.QueryFirstAsync<int>(sp, para, commandType: CommandType.StoredProcedure);
        }
        public async Task ExecuteSPAsync(string sp, object? para = null)
        {
            using var conn = CreateConnection();
            await conn.ExecuteAsync(sp, para, commandType: CommandType.StoredProcedure);
        }
    }
}
