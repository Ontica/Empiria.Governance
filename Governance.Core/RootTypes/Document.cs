﻿/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Management System        *
*  Assembly : Empiria.Governance.dll                           Pattern : Domain class                        *
*  Type     : Document                                         License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Handles information about a document.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;

using Empiria.Json;
using Empiria.Contacts;

namespace Empiria.Governance {

  /// <summary>Handles information about a document.</summary>
  public class Document : BaseObject {

    #region Fields

    private readonly string libraryBaseAddress = ConfigurationData.GetString("DocumentsLibrary.BaseAddress");

    #endregion Fields

    #region Constructors and parsers

    protected Document() {
      // Required by Empiria Framework.
    }


    static internal Document Parse(int id) {
      return BaseObject.ParseId<Document>(id);
    }


    static public Document Parse(string uid) {
      return BaseObject.ParseKey<Document>(uid);
    }


    static public FixedList<Document> GetList(string type = "",
                                              string keywords = "") {
      string filter = String.Empty;
      string orderBy = "DocumentType, DocumentName";

      if (!String.IsNullOrWhiteSpace(type)) {
        filter = SearchExpression.ParseEquals("DocumentType", type);
      }
      if (!String.IsNullOrWhiteSpace(keywords)) {
        filter += filter.Length != 0 ? " AND " : String.Empty;
        filter += SearchExpression.ParseAndLike("Keywords", keywords);
      }

      return BaseObject.GetList<Document>(filter, orderBy)
                       .FindAll((x) => !x.IsSpecialCase)
                       .ToFixedList();
    }

    static public void UpdateAll() {
      var documents = BaseObject.GetList<Document>();

      foreach (var document in documents) {
        document.Save();
     }
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("DocumentType")]
    public string DocumentType {
      get;
      private set;
    } = String.Empty;


    [DataField("DocumentName")]
    public string Name {
      get;
      private set;
    } = String.Empty;


    [DataField("Code")]
    public string Code {
      get;
      private set;
    } = String.Empty;


    [DataField("Authority")]
    public string Authority {
      get;
      private set;
    } = String.Empty;


    [DataField("Stage")]
    public string Stage {
      get;
      private set;
    } = String.Empty;



    [DataField("Version")]
    public string Version {
      get;
      private set;
    } = String.Empty;



    [DataField("LastUpdated")]
    public string LastUpdated {
      get;
      private set;
    } = String.Empty;


    [DataField("FromDate")]
    public string FromDate {
      get;
      private set;
    } = String.Empty;


    [DataField("ToDate")]
    public string ToDate {
      get;
      private set;
    } = String.Empty;


    [DataField("DocumentURL")]
    public string FilePath {
      get;
      private set;
    }

    public string Url {
      get {
        if (this.FilePath.StartsWith("~/")) {
          return this.FilePath.Replace("~/", libraryBaseAddress + "/");
        } else {
          return this.FilePath;
        }
      }
    }


    [DataField("OfficialURL")]
    public string OfficialURL {
      get;
      private set;
    }


    [DataField("OwnerId")]
    public Contact Owner {
      get;
      private set;
    }


    internal string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Code, this.Name, this.Authority,
                                           this.Stage, this.DocumentType);
      }
    }

    #endregion Public properties

    #region Public methods

    protected override void OnSave() {
      DocumentsData.WriteDocument(this);
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

    }


    private void Load(JsonObject data) {

    }

    #endregion Private methods

  }  // class Document

}  // namespace Empiria.Governance
