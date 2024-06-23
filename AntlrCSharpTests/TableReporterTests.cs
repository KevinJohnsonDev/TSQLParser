﻿using TSQLParserLib.analysis;
using TSQLParserLib.listeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSQLParserLibTests {
    [TestClass]
    public class TableReporterTests {

        private string _sampleStatements = @"
        SELECT A.ID FROM dbo.A JOIN dbo.B ON A.ID = B.AID;
        SELECT B.AID FROM dbo.B;
        SELECT B.ID FROM dbo.B INNER JOIN dbo.C ON B.ID = C.BID;
        ";



        [TestMethod]
        public void TableUsageReporterCountsEachDistinctTable() {

            SqlListener listener = TestMethods.Init(_sampleStatements);
            TableUsageReporter tur = new(listener.Statements);
            Assert.IsTrue(tur.Tables.Count == 3);
        }

        [TestMethod]
        public void TableUsageReporterTiesFullyQualifiedNamesToStatementsUsed() {
            SqlListener listener = TestMethods.Init(_sampleStatements);
            TableUsageReporter tur = new(listener.Statements);
            var dboA = tur.Tables["dbo.A"];

            Assert.IsTrue(dboA.Count == 1);
            Assert.IsTrue(dboA[0].TokenText == "SELECT A.ID FROM dbo.A JOIN dbo.B ON A.ID = B.AID;");

            var dboB = tur.Tables["dbo.B"];
            Assert.IsTrue(dboB.Count == 3);
            Assert.IsTrue(dboB[0].TokenText == "SELECT A.ID FROM dbo.A JOIN dbo.B ON A.ID = B.AID;");
            Assert.IsTrue(dboB[1].TokenText == "SELECT B.AID FROM dbo.B;");
            Assert.IsTrue(dboB[2].TokenText == "SELECT B.ID FROM dbo.B INNER JOIN dbo.C ON B.ID = C.BID;");

            var dboC = tur.Tables["dbo.C"];
            Assert.IsTrue(dboC.Count == 1);
            Assert.IsTrue(dboC[0].TokenText == "SELECT B.ID FROM dbo.B INNER JOIN dbo.C ON B.ID = C.BID;");


        }

        [TestMethod]

        public void TableUsageReporterUsesFullyQualifiedNameForKeys() {
        var fullyQualifiedWithImplicitSameName = @"
        SELECT A.ID FROM dbo.A AS A JOIN SampleDB.dbo.A AS SA ON A.ID = SA.ID;
        ";
        SqlListener listener = TestMethods.Init(fullyQualifiedWithImplicitSameName);
        TableUsageReporter tur = new(listener.Statements);
        Assert.IsTrue(tur.Tables.Count == 2);
            Assert.IsTrue(tur.Tables.ContainsKey("dbo.A"));
            Assert.IsTrue(tur.Tables.ContainsKey("SampleDB.dbo.A"));
        }


    }
}
