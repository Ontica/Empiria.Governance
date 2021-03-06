﻿/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Management System        *
*  Assembly : Empiria.Governance.dll                           Pattern : Data Service                        *
*  Type     : LegalDocument                                    License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Characterizes a legal document, such as a legislation, regulation or a contract.               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.DataTypes.Time;
using Empiria.Json;
using Empiria.StateEnums;

using Empiria.Governance.Government;

namespace Empiria.Governance.Contracts {

  public class RelatedProcedure : BaseObject {

    #region Constructors and parsers

    protected RelatedProcedure() {
      // Required by Empiria Framework.
    }


    internal RelatedProcedure(Clause legalDocumentItem, JsonObject data) {
      Assertion.AssertObject(legalDocumentItem, "legalDocumentItem");
      Assertion.AssertObject(data, "data");

      this.LegalDocumentItemId = legalDocumentItem.Id;

      this.AssertIsValid(data);

      this.Load(data);
    }

    internal RelatedProcedure(Clause legalDocumentItem, Procedure procedure) {
      Assertion.AssertObject(legalDocumentItem, "legalDocumentItem");
      Assertion.AssertObject(procedure, "procedure");

      this.LegalDocumentItemId = legalDocumentItem.Id;
      this.ProcedureId = procedure.Id;
    }

    static public RelatedProcedure Parse(int id) {
      return BaseObject.ParseId<RelatedProcedure>(id);
    }

    static public RelatedProcedure Parse(string uid) {
      return BaseObject.ParseKey<RelatedProcedure>(uid);
    }

    static public RelatedProcedure Empty {
      get {
        return BaseObject.ParseEmpty<RelatedProcedure>();
      }
    }

    static internal List<RelatedProcedure> GetList(Clause documentItemId) {
      string filter = $"LegalDocumentItemId = {documentItemId.Id}";

      return BaseObject.GetList<RelatedProcedure>(filter, "ItemPosition, RelatedProcedureId");
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("LegalDocumentItemId")]
    internal int LegalDocumentItemId {
      get;
      private set;
    }


    internal Clause Clause {
      get {
        return Clause.Parse(this.LegalDocumentItemId);
      }
    }

    [DataField("ProcedureId")]
    private int ProcedureId {
      get;
      set;
    }


    public Procedure Procedure {
      get {
        return Procedure.Parse(this.ProcedureId);
      }
    }


    [DataField("MaxFilingTerm")]
    public int MaxFilingTerm {
      get;
      private set;
    } = -1;


    [DataField("MaxFilingTermType", Default = TimeUnit.Undefined)]
    public TimeUnit MaxFilingTermType {
      get;
      private set;
    }


    [DataField("StartsWhen", Default = StartsWhen.Undefined)]
    public StartsWhen StartsWhen {
      get;
      private set;
    }


    [DataField("StartsWhenTrigger")]
    public string StartsWhenTrigger {
      get;
      private set;
    }


    public int ItemPosition {
      get;
      private set;
    }


    public JsonObject ExtensionData {
      get;
      internal set;
    } = new JsonObject();


    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    }


    [DataField("Status", Default = EntityStatus.Active)]
    public EntityStatus Status {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods

    protected override void OnSave() {
      ContractsData.WriteRelatedProcedure(this);
    }


    public void Update(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.AssertIsValid(data);
      this.Load(data);

      this.Save();
    }

    #endregion Public methods

    #region Private methods

    private void AssertIsValid(JsonObject data) {
      Assertion.AssertObject(data, "data");

      Validate.HasValue(data, "procedure/uid",
                        "Requiero conocer el uid del trámite relacionado a esta cláusula.");

      var procedureUID = data.Get<string>("procedure/uid");

      if (this.IsNew) {
        var relatedProcedure = this.Clause.TryGetRelatedProcedure((x) => x.Procedure.UID.Equals(procedureUID));

        Validate.AlreadyExists(relatedProcedure, $"Esta claúsula ya tiene asociado ese mismo trámite.");
      } else {
        var relatedProcedure = this.Clause.TryGetRelatedProcedure((x) => x.Procedure.UID.Equals(procedureUID) &&
                                                                         x.Id != this.Id);
        Validate.AlreadyExists(relatedProcedure, $"La claúsula ya tiene relacionado ese mismo trámite.");
      }
    }

    private void Load(JsonObject data) {
      this.ProcedureId = Procedure.Parse(data.Get<string>("procedure/uid")).Id;

      this.MaxFilingTerm = data.Get<int>("maxFilingTerm", this.MaxFilingTerm);
      this.MaxFilingTermType = data.Get<TimeUnit>("maxFilingTermType", this.MaxFilingTermType);
      this.StartsWhen = data.Get<StartsWhen>("startsWhen", this.StartsWhen);
      this.StartsWhenTrigger = data.Get<string>("startsWhenTrigger", this.StartsWhenTrigger);

      this.Notes = data.Get<string>("notes", this.Notes);
      this.Status = data.Get<EntityStatus>("status", this.Status);
    }

    #endregion Private methods

  }  // class RelatedProcedure

}  // namespace Empiria.Governance.Contracts
