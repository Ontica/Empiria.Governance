/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Management System        *
*  Assembly : Empiria.Governance.dll                           Pattern : Domain class                        *
*  Type     : DocumentRule                                     License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Characterizes an obligation or right enforced by law or by a legal agreement.                  *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;

using Empiria.Data;
using Empiria.Json;

using Empiria.Governance.Government;
using System.Linq;

namespace Empiria.Governance.Contracts {

  /// <summary>Characterizes an obligation or right enforced by law or by a legal agreement.</summary>
  public class ContractRule : BaseObject {

    #region Constructors and parsers

    protected ContractRule() {
      // Required by Empiria Framework.
    }


    internal ContractRule(Contract legalDocument, JsonObject data) {
      Assertion.AssertObject(legalDocument, "legalDocument");
      Assertion.AssertObject(data, "data");

      this.DocumentId = legalDocument.Id;

      this.AssertIsValid(data);

      this.Load(data);
    }


    static internal ContractRule Parse(int id) {
      return BaseObject.ParseId<ContractRule>(id);
    }


    static public ContractRule Parse(string uid) {
      return BaseObject.ParseKey<ContractRule>(uid);
    }



    static internal List<ContractRule> GetList(Clause clause) {
      string filter = $"DocumentItemId = {clause.Id}";

      return BaseObject.GetList<ContractRule>(filter, "ItemPosition, RuleName");
    }

    static internal List<ContractRule> GetList(Contract legalDocument, string keywords) {
      string filter = $"DocumentId = {legalDocument.Id}";

      keywords = keywords.Replace("'", "");

      if (keywords.Length != 0) {
        var keywordsFilter = SearchExpression.ParseAndLike("Keywords", keywords);

        filter = GeneralDataOperations.BuildSqlAndFilter(filter, keywordsFilter);
      }

      return BaseObject.GetList<ContractRule>(filter, "RuleName");
    }

    static public void UpdateAll() {
      var rules = BaseObject.GetList<ContractRule>();

      foreach (var rule in rules) {
        var contract = Contract.Parse(rule.DocumentId);

        FixedList<Clause> clauses = contract.GetClausesFromText(rule.DocumentItems);

        if (clauses.Count == 0) {
          rule.Save();
        }
        for (int i = 0; i < clauses.Count; i++) {
          var clause = clauses[i];
          if (i == 0) {
            rule.DocumentItemId = clause.Id;
            rule.Save();
          } else {
            var newRule = rule.Clone();

            newRule.DocumentItemId = clause.Id;
            newRule.ReferenceRuleId = rule.Id;
            newRule.Save();
          }
        }
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("DocumentId")]
    internal int DocumentId {
      get;
      private set;
    }


    public Contract Document {
      get {
        return Contract.Parse(this.DocumentId);
      }
    }


    [DataField("DocumentItemId")]
    public int DocumentItemId {
      get;
      private set;
    }


    [DataField("ReferenceRuleId")]
    public int ReferenceRuleId {
      get;
      private set;
    } = -1;


    [DataField("RuleType", Default = ContractRuleType.Undefined)]
    public ContractRuleType RuleType {
      get;
      private set;
    }


    [DataField("RuleName")]
    public string Name {
      get;
      private set;
    }


    [DataField("Description")]
    public string Description {
      get;
      private set;
    }


    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    }


    [DataField("ItemPosition")]
    public int Position {
      get;
      private set;
    }


    [DataField("AppliesTo")]
    public string AppliesTo {
      get;
      private set;
    }


    [DataField("Verb")]
    public string Verb {
      get;
      private set;
    }


    [DataField("Action")]
    public string Action {
      get;
      private set;
    }


    [DataField("WhenPredicate")]
    public string WhenPredicate {
      get;
      private set;
    }


    [DataField("ActionTimeCondition")]
    public string ActionTimeCondition {
      get;
      private set;
    }


    [DataField("Tags")]
    public string Tags {
      get;
      private set;
    }


    public JsonObject ExtensionData {
      get;
      internal set;
    } = new JsonObject();


    [DataField("WorkflowObjectId")]
    public int WorkflowObjectId {
      get;
      private set;
    } = -1;


    [DataField("DocumentItems")]
    internal string DocumentItems {
      get;
      private set;
    }


    private FixedList<Procedure> procedures = null;
    public FixedList<Procedure> Procedures {
      get {
        if (procedures == null) {
          procedures = this.GetProceduresFromText();
        }
        return procedures;
      }
    }


    [DataField("Procedures")]
    internal string ProceduresAsText {
      get;
      private set;
    }


    private string _asHypertext = null;
    public string AsHypertext {
      get {
        if (_asHypertext == null) {
          _asHypertext = Presentation.Hypertext.ToTermDefinitionHypertext(this.Name, this.Document);
        }
        return _asHypertext;
      }
    }


    internal string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Name, this.Tags, this.Description);
      }
    }

    #endregion Public properties

    #region Public methods

    protected override void OnSave() {
      ContractsData.WriteDocumentRule(this);
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


    private ContractRule Clone() {
      var clone = new ContractRule();

      clone.Name = this.Name;
      clone.RuleType = this.RuleType;
      clone.DocumentId = this.DocumentId;
      clone.Description = this.Description;
      clone.DocumentItems = this.DocumentItems;
      clone.WorkflowObjectId = this.WorkflowObjectId;
      clone.ProceduresAsText = this.ProceduresAsText;

      return clone;
    }


    private FixedList<Procedure> GetProceduresFromText() {
      var temp = this.ProceduresAsText.Replace(',', ' ')
                                      .Replace(';', ' ');

      var textParts = temp.Split(' ')
                          .Where((x) => EmpiriaString.IsQuantity(x));

      if (textParts == null) {
        return new FixedList<Procedure>();
      }

      List<Procedure> list = new List<Procedure>();
      foreach (var part in textParts) {
        list.Add(Procedure.Parse(int.Parse(part)));
      }
      return list.ToFixedList();
    }

    private void Load(JsonObject data) {
      // no-op
    }

    #endregion Private methods

  }  // class ContractRule

}  // namespace Empiria.Governance.Contracts
