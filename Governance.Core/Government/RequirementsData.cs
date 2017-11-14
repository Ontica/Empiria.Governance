/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Management System        *
*  Assembly : Empiria.Governance.dll                           Pattern : Domain class                        *
*  Type     : RequirementsData                                 License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Data read and write methods for procedure requirements.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Data;

namespace Empiria.Governance.Government {

  /// <summary>Data read and write methods for procedure requirements.</summary>
  static internal class RequirementsData {

    static internal void WriteRequirement(Requirement o) {
      var op = DataOperation.Parse("writeGRCRequirement",
                                   o.Id, o.UID);

      DataWriter.Execute(op);
    }

  }  // class RequirementsData

}  // namespace Empiria.Governance.Government
