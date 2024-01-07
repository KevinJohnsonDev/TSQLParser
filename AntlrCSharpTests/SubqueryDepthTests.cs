﻿using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using AntlrCSharp.listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntlrCSharp.analysis;

namespace AntlrCSharpTests
{
    [TestClass]
    public class SubqueryDepthTests
    {

        private String _sampleQuery =  @"
            SELECT 1 AS ID 
            FROM (
                SELECT 1 AS ID 
                FROM (
                    SELECT 1 AS ID 
                    FROM (
                        SELECT 1 AS ID
                    ) AS L3
                ) AS L2
            ) AS L1 ";
        [TestMethod]
        public void DepthReporterMarksQueriesDeeperThanMaxAsError()
        {

            SqlListener listener = TestMethods.Init(_sampleQuery);
            SubqueryDepthReporter r = new(listener.Statements,2);
            Assert.IsTrue(r.Errors.Count==1);
            Assert.IsTrue(r.Errors[0].TokenText == listener.Statements[0].TokenText);

        }
        [TestMethod]
        public void DepthReporterIgnoresLessThanEqualToMax()
        {
            SqlListener listener = TestMethods.Init(_sampleQuery);
            SubqueryDepthReporter r = new(listener.Statements, 3);
            Assert.IsTrue(r.Errors.Count == 0);
        }
    }




}
