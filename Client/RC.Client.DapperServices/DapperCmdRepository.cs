using RC.Client.Interfaces.Repositories;
using RC.Data;
using RC.Domain.Commands;
using RC.Interfaces.Factories;
using System.Data;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using Dapper;
using System.Threading;
using System;
using RC.Client.Domain;

namespace RC.Client.DapperServices
{
    public class DapperCmdRepository : AbstractDataBaseRepository, ICmdRepositoryAsync
    {
        private const string QueryByRequestIdAndStatus = "SELECT * FROM {0} WHERE [RequestId] = @RequestId AND [Status] != @Status";
        private const string QueryCmdResultJsonByRequestIdAndStatus = "SELECT [CmdResultJson] FROM {0} WHERE [RequestId] = @RequestId AND [Status] != @Status";
        public DapperCmdRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
            SqlMapperExtensions.TableNameMapper += (type) => { return type.TableName(); };
        }

        public async Task<TCmdParamsSet> RetrieveExecutedCmdAsync<TCmdParamsSet>(TCmdParamsSet parametersSet)
           where TCmdParamsSet : CmdParametersSet
        {
            var query = string.Format(QueryByRequestIdAndStatus, typeof(TCmdParamsSet).TableName());

            return await base.QueryAsync((IDbConnection conn) =>
              conn.QuerySingleOrDefaultAsync<TCmdParamsSet>(query, new { parametersSet.RequestId, Status = CmdStatus.AwaitingForExecution }));

        }
        public async Task SendToBackendAsync<TCmdParams>(TCmdParams cmdParams)
            where TCmdParams : CmdParametersSet
        {

            await base.ExecuteAsync((IDbConnection conn, IDbTransaction tx) =>
               conn.InsertAsync(cmdParams, tx));
        }

        public async Task<string> RetrieveExecutedCmdResultJsonAsync<TCmdParamsSet>(TCmdParamsSet parametersSet) where TCmdParamsSet : CmdParametersSet
        {
            var query = string.Format(QueryCmdResultJsonByRequestIdAndStatus, typeof(TCmdParamsSet).TableName());

            return await base.QueryAsync((IDbConnection conn) =>
             conn.ExecuteScalarAsync<string>(query, new { parametersSet.RequestId, Status = CmdStatus.AwaitingForExecution }));
        }

        public async  Task<CmdResult<TResult, TCmdParamsSet>> RetrieveResultAsync<TResult, TCmdParamsSet>(TCmdParamsSet parametersSet, TimeSpan timeout) where TCmdParamsSet : CmdParametersSet
        {
            if (timeout == null || timeout.Milliseconds == 0)
                timeout = new TimeSpan(0, 0, 0, 30);

            while (true)
            {
                TCmdParamsSet cmd = await ExecutedParameter<TCmdParamsSet>(parametersSet.RequestId);
                if (cmd == default(TCmdParamsSet))
                    Thread.Sleep(500);
                else
                {
                    var json = await CmdResultJson(parametersSet);

                    switch (cmd.Status)
                    {
                        case CmdStatus.Executed:
                            {
                                return JsonServices.Json.Deserialize<CmdResult<TResult, TCmdParamsSet>>(json);
                            }
                        case CmdStatus.ResultedInError:
                            {
                                var result =JsonServices.Json.Deserialize<CmdResult<Exception, TCmdParamsSet>>(json);
                                throw new CmdExecutionException("Command Execution With Errors", result.Result);
                            }

                        default:
                            throw new CommandResultRetrievalException();
                    }
                }
            }
        }

        private async Task<TCmdParamsSet> ExecutedParameter<TCmdParamsSet>(Guid requestId) where TCmdParamsSet : CmdParametersSet
        {
            var query = string.Format(QueryByRequestIdAndStatus, typeof(TCmdParamsSet).TableName());
            return await base.QueryAsync((IDbConnection conn) =>
                  conn.QuerySingleOrDefaultAsync<TCmdParamsSet>(query, new { requestId, Status = CmdStatus.AwaitingForExecution }));
        }

        private async Task<string> CmdResultJson(CmdParametersSet cmdParams)
        {
            return await base.QueryAsync((IDbConnection conn) =>
                                conn.ExecuteScalarAsync<string>($"SELECT CmdResultJson FROM {typeof(CmdParametersSet).TableName()} WHERE RequestId=@RequestId", new { cmdParams.RequestId })
                               );
        }
    }
}
