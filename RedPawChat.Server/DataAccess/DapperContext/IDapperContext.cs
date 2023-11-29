using System.Data;

namespace RedPawChat.Server.DataAccess.DapperContext
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
    }
}