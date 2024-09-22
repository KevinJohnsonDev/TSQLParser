﻿using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using TSQLParserLib.listeners;
using TSQLParserLib.analysis;

namespace TSQLParserLibTests {
    public static class TestMethods
    {
        public static SqlListener Init(string input, List<DeclaredSqlTable>? CatalogItems = null)
        {
            AntlrInputStream inputStream = new(input);
            TSqlLexer tsqlLexer = new(inputStream);
            CommonTokenStream commonTokenStream = new(tsqlLexer);
            TSqlParser sqlParser = new(commonTokenStream);
            TokenLoggingSqlListener listener = new(sqlParser);
            if(CatalogItems != null) {
                foreach( var catalogItem in CatalogItems ) {
                    listener.DbCatalog.Add(catalogItem);
                }
            }
            ParseTreeWalker.Default.Walk(listener, sqlParser.tsql_file());

            return listener;
        }


    }
}
