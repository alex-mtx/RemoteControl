using Dapper;
using RC.Client.Interfaces.Repositories;
using RC.Data;
using RC.Domain.Commands;
using RC.Interfaces.Factories;
using System.Data;
using System.Threading.Tasks;
using RC.Client.DapperServices.Dapper.Contrib;
using RC.Client.DapperServices.Dapper.Contrib.Extensions;

namespace RC.Client.DapperServices
{
    public class DapperCmdRepository : AbstractRepository, ICmdRepositoryAsync
    {
        public DapperCmdRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
        }

        public async Task<CmdResult<TResult, TCmdParamsSet>> RetrieveResultAsync<TResult, TCmdParamsSet>(TCmdParamsSet parametersSet)
            where TCmdParamsSet : CmdParametersSet
        {
            return await Task.Run(() => new CmdResult<TResult, TCmdParamsSet>(default(TResult), parametersSet));

        }

        public async Task SendToBackendAsync<TCmdParams>(TCmdParams cmdParams)
            where TCmdParams : CmdParametersSet
        {

            await base.ExecuteAsync((IDbConnection conn, IDbTransaction tx) =>
               conn.InsertAsync(cmdParams, tx));
        }


    }
}
