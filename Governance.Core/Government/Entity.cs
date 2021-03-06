﻿/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Management System        *
*  Assembly : Empiria.Governance.dll                           Pattern : Domain class                        *
*  Type     : Entity                                           License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Describes a government entity such as an agency or ministry.                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Contacts;

namespace Empiria.Governance.Government {

  public class Entity : Organization {

    #region Constructors and parsers

    private Entity() {
      // Required by Empiria Framework.
    }

    static public new Entity Parse(string uid) {
      return BaseObject.ParseKey<Entity>(uid);
    }

    static public new Entity Empty {
      get {
        return BaseObject.ParseEmpty<Entity>();
      }
    }

    static public new FixedList<Entity> GetList(string filter) {
      var list = BaseObject.GetList<Entity>(filter, "Nickname");
      return list.ToFixedList();
    }

    #endregion Constructors and parsers

    #region Properties

    private FixedList<Office> _authoritiesList = null;
    public FixedList<Office> Authorities {
      get {
        if (_authoritiesList == null) {
          _authoritiesList = base.GetLinks<Office>("Entity->Offices");

          _authoritiesList.Sort((x, y) => x.FullName.CompareTo(y.FullName));
        }
        return _authoritiesList;
      }
    }

    private FixedList<Position> _positionsList = null;
    public FixedList<Position> Positions {
      get {
        if (_positionsList == null) {
          _positionsList = base.GetLinks<Position>("Entity->Positions");

          _positionsList.Sort((x, y) => x.FullName.CompareTo(y.FullName));
        }
        return _positionsList;
      }
    }

    #endregion Properties

  }  // class Entity

}  // namespace Empiria.Governance.Government
