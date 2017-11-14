/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Management System        *
*  Assembly : Empiria.Governance.dll                           Pattern : Data Service                        *
*  Type     : DocumentsData                                    License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Data read and write methods for the documents library.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

namespace Empiria.Governance {

  /// <summary>Data read and write methods for the documents library.</summary>
  static internal class DocumentsData {

    static internal void WriteDocument(Document o) {
      var op = DataOperation.Parse("writeGRCDocument", o.Id, o.UID, o.Keywords);

      DataWriter.Execute(op);
    }

  }  // class DocumentsData

}  // namespace Empiria.Governance
