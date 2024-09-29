﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSQLParserLib.analysis {
    public class TableUsageReporter {

        public Dictionary<string, List<SqlStatement>> Tables { get; init; } = new();
        public Dictionary<string, List<SqlStatement>> Unresolved { get; init; } = new();

        /*
         * The idea of this reporter is to report the statements queries that a particular table is used in
         * Equivilent to find all references although it should work cross database 
         * 
         */
        public TableUsageReporter(IEnumerable<SqlStatement> statements) {
            foreach (SqlStatement statement in statements) {
                foreach (var table in statement.Tables) {
                    if (!Tables.ContainsKey(table.FQN)) {
                        Tables.Add(table.FQN, new List<SqlStatement>());
                    }
                    if (!Tables[table.FQN].Contains(statement)) {
                        Tables[table.FQN].Add(statement);
                    }
                    if (table.ResolvedTable != null) { continue; }
                    if (!Unresolved.ContainsKey(table.FQN)) { 
                        Unresolved.Add(table.FQN, new List<SqlStatement>());
                        Unresolved[table.FQN].Add(statement);
                    }
                    if (!Unresolved[table.FQN].Contains(statement)) {
                        Unresolved[table.FQN].Add(statement);
                    }

                }
            }
        }
    }
}
