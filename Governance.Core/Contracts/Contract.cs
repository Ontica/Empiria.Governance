﻿/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Management System        *
*  Assembly : Empiria.Governance.dll                           Pattern : Domain class                        *
*  Type     : Contract                                         License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Characterizes a contract (legal document) with a set of clauses and annexes.                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Linq;

using Empiria.Json;
using Empiria.Ontology;

namespace Empiria.Governance.Contracts {

  /// <summary>Characterizes a contract (legal document) with a set of clauses and annexes.</summary>
  [PartitionedType(typeof(LegalDocumentType))]
  public class Contract : BaseObject {

    #region Fields

    private Lazy<List<Clause>> clausesList = null;

    #endregion Fields

    #region Constructors and parsers

    private Contract() {
      // Required by Empiria Framework.
    }

    protected Contract(LegalDocumentType powertype) : base(powertype) {
      // Required by Empiria Framework for all partitioned types.
    }


    static internal Contract Parse(int id) {
      return BaseObject.ParseId<Contract>(id);
    }


    static public Contract Parse(string uid) {
      return BaseObject.ParseKey<Contract>(uid);
    }

    static public Contract Empty {
      get {
        return BaseObject.ParseEmpty<Contract>();
      }
    }

    static public FixedList<Contract> GetList(LegalDocumentType powertype) {
      string filter = $"ObjectStatus <> 'X'";

      return Contract.GetList<Contract>(filter)
                     .ToFixedList();
    }


    protected override void OnInitialize() {
      clausesList = new Lazy<List<Clause>>(() => Clause.GetList(this));
    }

    #endregion Constructors and parsers

    #region Public properties

    public LegalDocumentType LegalDocumentType {
      get {
        return (LegalDocumentType) base.GetEmpiriaType();
      }
    }


    [DataField("ObjectName")]
    public string Name {
      get;
      private set;
    }


    [DataField("ObjectExtData.url")]
    public string Url {
      get;
      private set;
    }


    public FixedList<Clause> Clauses {
      get {
        return clausesList.Value.ToFixedList();
      }
    }

    #endregion Public properties

    #region Public methods

    public Clause AddClause(JsonObject data) {
      var clause = new Clause(this, data);

      clause.Save();

      clausesList.Value.Add(clause);

      return clause;
    }


    public Clause GetClause(string clauseUID) {
      Clause item = clausesList.Value.Find((x) => x.UID == clauseUID);

      Assertion.AssertObject(item, $"A clause with uid = '{clauseUID}' " +
                                   $"was not found in contract with uid = '{this.UID}'");

      return item;
    }

    public FixedList<Clause> GetClauses(string keywords) {
      if (keywords.Length == 0) {
        return this.Clauses;
      }

      return Clause.GetList(this, keywords)
                   .ToFixedList();
    }


    internal FixedList<Clause> GetClausesFromText(string clausesAsText) {
      if (clausesAsText.Contains("Anexo")) {
        return new FixedList<Clause>();
      }

      clausesAsText = clausesAsText.Replace(',', ' ')
                                   .Replace(';', ' ');

      var clauseTextParts = clausesAsText.Split(' ')
                                         .Where((x) => EmpiriaString.IsQuantity(x));

      if (clauseTextParts == null) {
        return new FixedList<Clause>();
      }

      var clauses = this.Clauses.FindAll((x) => clauseTextParts.Contains(x.Number) && x.Section == "Cláusulas");

      if (clauses != null) {
        return clauses;
      } else {
        return new FixedList<Clause>();
      }
    }


    public FixedList<Clause> GetSectionClauses(string sectionName) {
      return this.Clauses.FindAll((x) => x.Section == sectionName);
    }


    public FixedList<Clause> MatchClauses(string identificationText) {
      if (identificationText.Contains("Anexo")) {
        return GetAnnexesFromText(identificationText);

      } else {
        return GetClausesFromText(identificationText);
      }
    }


    public Clause TryGetClause(Predicate<Clause> predicate) {
      return clausesList.Value.Find(predicate);
    }

    #endregion Public methods

    #region Private methods

    private FixedList<Clause> GetAnnexesFromText(string annexesAsText) {
      annexesAsText = annexesAsText.Replace(',', ' ')
                                   .Replace(';', ' ');

      annexesAsText = EmpiriaString.TrimAll(annexesAsText);

      var clauseTextParts = annexesAsText.Split(' ');

      if (clauseTextParts == null) {
        return new FixedList<Clause>();
      }

      if (clauseTextParts.Length < 2 || !clauseTextParts[0].StartsWith("Anexo")) {
        return new FixedList<Clause>();
      }

      var clauses = this.Clauses.FindAll((x) => x.Section == "Anexo" && x.Number == $"Anexo {clauseTextParts[1]}");

      if (clauses != null) {
        return clauses;
      } else {
        return new FixedList<Clause>();
      }
    }

    #endregion Private methods

  }  // class Contract

}  // namespace Empiria.Governance.Contracts
