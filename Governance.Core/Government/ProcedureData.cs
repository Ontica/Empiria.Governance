/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Management System        *
*  Assembly : Empiria.Governance.dll                           Pattern : Data Service                        *
*  Type     : ProcedureData                                    License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Process definition data read and write methods.                                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

namespace Empiria.Governance.Government {

  static internal class ProcedureData {

    static internal FixedList<Procedure> GetProcedureList(string filter = "", string keywords = "") {
      if (keywords.Length != 0) {
        keywords = SearchExpression.ParseAndLike("Keywords", keywords);
      }
      filter = GeneralDataOperations.BuildSqlAndFilter(filter, keywords);

      if (filter.Length == 0) {
        filter = GeneralDataOperations.AllRecordsFilter;
      }

      var op = DataOperation.Parse("@qryGRCProcedures", filter);

      return DataReader.GetFixedList<Procedure>(op);
    }

    static internal void WriteProcedure(Procedure o) {
      var op = DataOperation.Parse("writeGRCProcedure",
                                    o.Id, o.UID, o.Keywords, o.BpmnDiagram.Id);

      DataWriter.Execute(op);
    }

  }  // class ProcedureData

}  // namespace namespace Empiria.Governance.Government
