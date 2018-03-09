using RC.Domain.Commands;
using System;
using System.Collections.Generic;

namespace RC.Data
{
    public static class TableNameMapping
    {
        private static Dictionary<Type, string> _map = new Dictionary<Type, string>();

        static TableNameMapping()
        {
            _map.Add(typeof(CmdParametersSet), "[CmdParametersSets]");
        }
        public static string TableName(this Type type)
        {
            if(_map.ContainsKey(type))
                return _map[type];

            if (_map.ContainsKey(type.BaseType))
                return _map[type.BaseType];

            throw new ArgumentException($"Type {type} is not mapped", nameof(type));
        }
    }
}
