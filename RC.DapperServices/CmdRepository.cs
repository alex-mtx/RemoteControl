﻿using Dapper;
using RC.Implementation.Commands;
using RC.Implementation.Commands.Storages;
using RC.Interfaces.Factories;
using RC.Interfaces.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace RC.DapperServices
{
    public class CmdRepository : AbstractRepository, ICmdRepository<CmdParametersSet>
    {
        public CmdRepository(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory)
        {
        }

        public virtual IEnumerable<CmdParametersSet> PendingCommands()
        {
            return base.Query((IDbConnection conn) => GetAllPendingCmds(conn));
        }

        protected IEnumerable<CmdParametersSet> GetAllPendingCmds(IDbConnection conn)
        {
            //https://github.com/StackExchange/Dapper#type-switching-per-row
            var cmdParams = new List<CmdParametersSet>();
            using (var reader = conn.ExecuteReader("SELECT * from command_request WHERE [Finished] = 0"))
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

    }
}