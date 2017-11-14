/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Management System        *
*  Assembly : Empiria.Governance.dll                           Pattern : Domain class                        *
*  Type     : Office                                           License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Describes a government agency or office.                                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;

namespace Empiria.Governance.Government {

  public class Office : Organization {

    #region Constructors and parsers

    private Office() {
      // Required by Empiria Framework.
    }

    static public new Office Parse(string uid) {
      return BaseObject.ParseKey<Office>(uid);
    }

    static public new Office Empty {
      get {
        return BaseObject.ParseEmpty<Office>();
      }
    }

    #endregion Constructors and parsers

    #region Properties

    private Position _headPosition = null;
    public Position HeadPosition {
      get {
        if (_headPosition == null) {
          _headPosition = base.GetLink<Position>("Office->HeadPosition", Position.Empty);
        }
        return _headPosition;
      }
    }

    #endregion Properties

  }  // class Office

}  // namespace Empiria.Governance.Government
