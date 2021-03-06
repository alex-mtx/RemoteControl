﻿using Dapper;
using Dapper.Contrib.Extensions;
using RC.Data;
using RC.Domain.Commands;
using RC.Domain.Commands.Storages;
using RC.Interfaces.Factories;
using RC.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;

namespace RC.DapperServices
{
    public class CmdRepository : AbstractDataBaseRepository, ICmdRepository<CmdParametersSet, CmdParametersSet>
    {
        private static string _cmdParametersSetTableName = typeof(CmdParametersSet).TableName();
        public CmdRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
            SqlMapperExtensions.TableNameMapper += (type) => { return type.TableName(); };

        }

        public virtual IEnumerable<CmdParametersSet> PendingCommands()
        {
            return base.Query((IDbConnection conn) => GetAllPendingCmds(conn));
        }

        protected IEnumerable<CmdParametersSet> GetAllPendingCmds(IDbConnection conn)
        {
            //https://github.com/StackExchange/Dapper#type-switching-per-row
            var cmdParams = new List<CmdParametersSet>();
            using (var reader = conn.ExecuteReader($"SELECT * from {_cmdParametersSetTableName} WHERE [Status] = @Status",new {Status = CmdStatus.AwaitingForExecution}))
            {
                var storageListingParser = reader.GetRowParser<CmdParametersSet>(typeof(StorageCmdParamSet));

                CmdParametersSet param;
                CmdType type = 0;
                while(reader.Read())
                {
                    type = (CmdType)reader.GetInt32(3);
                    switch (type)
                    {
                        case CmdType.StorageContentsListing:
                            param = storageListingParser(reader);
                            break;
                        default:
                            throw new NotImplementedException($"Parameter type {type} is not implemented");
                    }

                    cmdParams.Add(param);
                }

                return cmdParams;

            }
        }
        public void Update(CmdParametersSet cmdParameters)
        {
            base.Execute((IDbConnection conn, IDbTransaction tx) => conn.Update(cmdParameters, tx));

        }
        public void Update<TCmdReturn>(CmdResult<TCmdReturn, CmdParametersSet> cmdResult)
        {
            
            //intentionally doubling the round trip so the code get's much simpler.
            
            //1st: the "automatic" update of CmdParametersSet
            base.Execute((IDbConnection conn, IDbTransaction tx) => 
                conn.Update(cmdResult.CmdParamsSet, tx));

            //2nd is to have the CmdResult and CmdResult.Result serialized and persisted into the columns CmdResultJson and Result respectively
            var cmdResultJson = JsonServices.Json.Serialize(cmdResult);
            var resultJson = JsonServices.Json.Serialize(cmdResult.Result);

            string sql = $"UPDATE {_cmdParametersSetTableName} SET CmdResultJson = @CmdResult, Result = @Result WHERE ID = @Id";
            base.Execute((IDbConnection conn, IDbTransaction tx) =>
                conn.Execute(sql,new { CmdResult = cmdResultJson, Result = resultJson,  cmdResult.CmdParamsSet.Id },tx));

        }
    }
}
