﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TSQLParserLib.analysis
{
    /*
     * The idea of this reporter is to report queries 
     * whose subqueries go deep enough to potentially severely impact performance
     * across various SQL Batches and Procedures
     */
    public class SubqueryDepthReporter
    {
        public List<ISargable> Errors { get; init; }
        public SubqueryDepthReporter(IEnumerable<SqlStatement> statements,int maxDepth = 3)
        {
            Errors = new List<ISargable>();

            foreach (SqlStatement statement in statements)
            {
                int depth = Depth(statement, 0);
                if(depth > maxDepth) { Errors.Add(statement); }
            }
        }

        private int Depth(SqlStatement statement, int currentDepth)
        {
            int maxDepth = currentDepth;
            foreach(SqlStatement s in statement.Subqueries)
            {
                maxDepth = Math.Max(maxDepth,Depth(s,currentDepth+1));
            }
            return maxDepth;
        }
    }


}
