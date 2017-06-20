using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrateDataLib.Constants;
using MigrateDataLib.Schema.DefCopyItems;
using MigrateDataLib.Schema.DefInfoItems;

namespace MigrateDataLib.Schema.Transformation
{
    public class TargetSchemaTransformation
    {
        public const string COLUMN_NAME_AUTOID = DatabaseDef.COLUMN_NAME_AUTOID;

        public const string NAMEAUTO_REF_ID = DatabaseDef.NAMEAUTO_REF_ID;

        public static void ConvertTablesAutoIdFieldToId(IList<TableDefCopy> tableList, UInt32 buildVersion)
        {
            foreach (var tableDef in tableList)
            {
                ConvertTableAutoId2Id(tableDef, tableList);
            }
        }

        public static void ConvertTablesRelationsMxToId(IList<TableDefCopy> tableList, UInt32 buildVersion)
        {
            foreach (var tableDef in tableList)
            {
                ConvertTableRelsMx2Id(tableDef, tableList, buildVersion);
            }
        }

        private static void ConvertTableAutoId2Id(TableDefCopy tableInfo, IList<TableDefCopy> tableList)
        {
            TableFieldCopy m_XID = tableInfo.GetAutoIncrementColumn();
            IndexDefCopy m_XPK = tableInfo.IndexPK();
            IList<RelationDefCopy> tableToRelation = tableInfo.ForeignRelations(tableList);

            if (m_XID == null)
            {
                tableInfo.CreateTargetFAUTO(COLUMN_NAME_AUTOID, DatabaseDef.DB_LONG);

                tableInfo.CreateTargetIndexFromXPK(m_XPK);

                m_XPK = tableInfo.CreatePKAutoConstraint("XPK", COLUMN_NAME_AUTOID);
            }
            else
            {
                string oldAuroName = m_XID.TargetColumnName();

                m_XID.ReNameTargetColumn(COLUMN_NAME_AUTOID);

                foreach (var index in tableInfo.IndexesNonPK())
                {
                    index.ReNameTargetColumn(oldAuroName, COLUMN_NAME_AUTOID);
                }

                foreach (var relationTable in tableToRelation)
                {
                    relationTable.ReNameTableColumn(oldAuroName, COLUMN_NAME_AUTOID);
                }

                tableInfo.CreateTargetIndexFromXPK(m_XPK, oldAuroName, COLUMN_NAME_AUTOID);

                m_XPK = tableInfo.CreatePKAutoConstraint("XPK", COLUMN_NAME_AUTOID);
            }
        }

        private static void ConvertTableRelsMx2Id(TableDefCopy tableInfo, IList<TableDefCopy> tableList, UInt32 buildVersion)
        {
            IList<RelationDefCopy> foreignRelations = ForeignRelations(tableInfo, tableList);

            IList<string> foreignNamesOfRelations = foreignRelations.Select((r) => (r.TargetForeignNamestAllUnique())).ToList();

            foreach (var relation in foreignRelations)
            {
                ConvertRelation(tableInfo, relation, foreignNamesOfRelations, tableList, buildVersion);
            }
        }

        public static void ConvertQueriesAutoIdFieldToId(IList<QueryDefCopy> queryList, UInt32 buildVersion)
        {
            foreach (var queryDef in queryList)
            {
                ConvertQueryAutoId2Id(queryDef);
            }
        }

        private static IList<RelationDefCopy> ForeignRelations(TableDefCopy tableInfo, IList<TableDefCopy> tableList)
        {
            return tableList.SelectMany((m) => (m.Relations().Where((r) => (r.SourceTableName.CompareTo(tableInfo.TableName()) == 0)))).ToList();
        }
        public static void ConvertRelation(TableDefCopy tableInfo, RelationDefCopy relation, IList<string> namesOfRelations, IList<TableDefCopy> tableList, UInt32 buildVersion)
        {
            var relationTable = TableInfoByName(tableList, relation.ForeignTableName);

            if (relationTable != null)
            {
                string constraintName = CreateRelationNameMxToId(tableInfo, relation, relationTable, namesOfRelations);

                var targetColumn = relationTable.SourceFieldByName(constraintName, buildVersion);

                if (targetColumn == null)
                {
                    relationTable.CreateTargetField(constraintName, DatabaseDef.DB_LONG, DatabaseDef.dbNullFieldOption);
                }

                relation.MakeTargetRelationOrmReady(constraintName, COLUMN_NAME_AUTOID);
            }
        }
        private static TableDefCopy TableInfoByName(IList<TableDefCopy> tableList, string name)
        {
            return tableList.SingleOrDefault((t) => (t.TableName().CompareTo(name) == 0));
        }
        private static string CreateRelationNameMxToId(TableDefCopy tableInfo, RelationDefCopy relation, TableDefCopy relationTable, IList<string> relationNames)
        {
            string uniqueXPkName = tableInfo.TargetPKUniqueAllNames();

            string uniqueRPkName = relationTable.TargetPKUniqueAllNames();

            string uniqueForName = relation.TargetForeignNamestAllUnique();

            string uniqueRelName = relation.TargetNamestAllUnique();

            string[] columnsNames = uniqueForName.Split(new char[] { '.' });
            int columnsNamesCount = columnsNames.Length;
            string[] colRelsNames = uniqueRelName.Split(new char[] { '.' });
            int colRelsNamesCount = colRelsNames.Length;
            string uniqueRelXEnds = colRelsNames[colRelsNamesCount - 1];

            int realtionNameCount = relationNames.Count((rn) => (rn.CompareTo(uniqueForName) == 0));

            string constraintName = relation.RelationName.ToLower() + NAMEAUTO_REF_ID;

            if (uniqueXPkName.CompareTo(uniqueRelName) != 0)
            {
                if (uniqueRelXEnds.CompareTo("id") == 0)
                {
                    constraintName = columnsNames[columnsNamesCount - 1];
                }
                else if (uniqueRelXEnds.CompareTo("firma_id") == 0)
                {
                    constraintName = relation.SourceTableName.ToLower() + NAMEAUTO_REF_ID;
                }
                else if (uniqueRelXEnds.EndsWith("kod"))
                {
                    string constraintColl = columnsNames[columnsNamesCount - 1];
                    constraintName = CreateConstraintName(constraintColl, "kod", NAMEAUTO_REF_ID);
                }
                else if (uniqueRelXEnds.Contains("kod"))
                {
                    string constraintColl = columnsNames[columnsNamesCount - 1];
                    constraintName = CreateConstraintName(constraintColl, "kod", NAMEAUTO_REF_ID);
                }
                else if (uniqueRelXEnds.EndsWith("cislo"))
                {
                    string constraintColl = columnsNames[columnsNamesCount - 1];
                    constraintName = CreateConstraintName(constraintColl, "cislo", NAMEAUTO_REF_ID);
                }
                else if (uniqueRelXEnds.Contains("cislo"))
                {
                    string constraintColl = columnsNames[columnsNamesCount - 1];
                    constraintName = CreateConstraintName(constraintColl, "cislo", NAMEAUTO_REF_ID);
                }
                else if (uniqueRelXEnds.EndsWith("id"))
                {
                    string constraintColl = columnsNames[columnsNamesCount - 1];
                    constraintName = CreateConstraintName(constraintColl, "id", NAMEAUTO_REF_ID);
                }
                else if (uniqueRelXEnds.Contains("id"))
                {
                    string constraintColl = columnsNames[columnsNamesCount - 1];
                    constraintName = CreateConstraintName(constraintColl, "id", NAMEAUTO_REF_ID);
                }
                else if (uniqueRelXEnds.CompareTo("mesic") == 0)
                {
                    constraintName = relation.SourceTableName.ToLower() + NAMEAUTO_REF_ID;
                }
                else
                {
                    string constraintColl = columnsNames[columnsNamesCount - 1];
                    constraintName = constraintColl + NAMEAUTO_REF_ID;
                }
            }
            else
            {
                constraintName = columnsNames[columnsNamesCount - 1];
            }

            return constraintName;
        }
        private static string CreateConstraintName(string constraintColl, string keyName, string autoRefId)
        {
            string constraintName;
            string keyNameEx1 = "_" + keyName;
            string keyNameEx2 = keyName + "_";

            if (constraintColl.EndsWith(keyNameEx1))
            {
                constraintName = constraintColl.Replace(keyNameEx1, autoRefId);
            }
            else if (constraintColl.Contains(keyNameEx2))
            {
                constraintName = constraintColl.Replace(keyNameEx2, "") + autoRefId;
            }
            else
            {
                constraintName = constraintColl + autoRefId;
            }

            return constraintName;
        }
        private static void ConvertQueryAutoId2Id(QueryDefCopy queryInfo)
        {
            IList<QueryTableCopy> queryTables = queryInfo.QueryTableInfo();

            foreach (var queryTable in queryTables)
            {
                ConvertQueryTableAutoId2Id(queryTable, queryInfo);
            }
        }

        private static void ConvertQueryTableAutoId2Id(QueryTableCopy queryTableInfo, QueryDefCopy queryInfo)
        {
            TableFieldCopy m_XID = queryTableInfo.GetAutoIncrementColumn();

            IndexDefCopy m_XPK = queryTableInfo.IndexPK();

            IList<RelationDefCopy> tableToRelation = queryInfo.ForeignRelations();

            if (m_XID == null)
            {
                queryTableInfo.CreateTargetFAUTO(COLUMN_NAME_AUTOID, DatabaseDef.DB_LONG);

                queryTableInfo.CreateTargetIndexFromXPK(m_XPK);

                m_XPK = queryTableInfo.CreatePKAutoConstraint("XPK", COLUMN_NAME_AUTOID);

            }
            else
            {
                string oldAuroName = m_XID.TargetColumnName();

                m_XID.ReNameTargetColumn(COLUMN_NAME_AUTOID);

                foreach (var index in queryTableInfo.IndexesNonPK())
                {
                    index.ReNameTargetColumn(oldAuroName, COLUMN_NAME_AUTOID);
                }

                foreach (var relationTable in tableToRelation)
                {
                    relationTable.ReNameTableColumn(oldAuroName, COLUMN_NAME_AUTOID);
                }

                queryTableInfo.CreateTargetIndexFromXPK(m_XPK, oldAuroName, COLUMN_NAME_AUTOID);

                m_XPK = queryTableInfo.CreatePKAutoConstraint("XPK", COLUMN_NAME_AUTOID);

                QueryFieldCopy m_QID = queryTableInfo.QueryTargetAutoIncrementColumn();

                if (m_QID != null)
                {
                    m_QID.ReNameTargetColumn(COLUMN_NAME_AUTOID);
                }

                IList<JoinsFieldCopy> tableLeftJoinInfo = queryInfo.LeftQueryJoinFieldInfo(queryTableInfo.AliasName, oldAuroName);

                foreach (var leftJoinFields in tableLeftJoinInfo)
                {
                    leftJoinFields.ReNameTargetLeftColumn(oldAuroName, COLUMN_NAME_AUTOID);
                }

                IList<JoinsFieldCopy> tableRightJoinInfo = queryInfo.RightQueryJoinFieldInfo(queryTableInfo.AliasName, oldAuroName);

                foreach (var rightJoinFields in tableRightJoinInfo)
                {
                    rightJoinFields.ReNameTargetRightColumn(oldAuroName, COLUMN_NAME_AUTOID);
                }

                IList<QueryCloseCopy> tableEndClausesInfo = queryInfo.QueryEndClausesInfo(queryTableInfo.AliasName, oldAuroName);

                foreach (var endClausesInfo in tableEndClausesInfo)
                {
                    endClausesInfo.ReNameTargetColumn(queryTableInfo.AliasName, oldAuroName, COLUMN_NAME_AUTOID);
                }

            }
        }
    }

}
