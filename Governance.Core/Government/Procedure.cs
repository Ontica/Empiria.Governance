﻿/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Management System        *
*  Assembly : Empiria.Governance.dll                           Pattern : Domain class                        *
*  Type     : Procedure                                        License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Provides services that gets process definition models.                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Empiria.DataTypes;
using Empiria.Json;

using Empiria.Governance.Contracts;
using Empiria.Governance.Presentation;

using Empiria.Workflow.Definition;

namespace Empiria.Governance.Government {

  public class Procedure : BaseObject {

    #region Fields

    private const string BPMN_DIAGRAMS_PATH = @"E:\empiria.files\covar.steps\bpmn.diagrams\";

    private Lazy<List<Requirement>> requirementsList = null;

    #endregion Fields

    #region Constructors and parsers

    private Procedure() {
      // Required by Empiria Framework.
    }


    public Procedure(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.Load(data);
    }


    static public Procedure Parse(int id) {
      return BaseObject.ParseId<Procedure>(id);
    }


    static public Procedure Parse(string uid) {
      return BaseObject.ParseKey<Procedure>(uid);
    }


    static public Procedure Empty {
      get {
        return BaseObject.ParseEmpty<Procedure>();
      }
    }


    static public FixedList<Procedure> GetList(string filter = "", string keywords = "") {
      return ProcedureData.GetProcedureList(filter, keywords);
    }


    static public FixedList<KeyValue> StartsWhenList {
      get {
        var list = KeyValueList.Parse("Governance.ProcedureStartsWhen.List");

        return list.GetItems();
      }
    }


    static public FixedList<string> ThemesList {
      get {
        var list = GeneralList.Parse("Governance.ProcedureThemes.List");

        return list.GetItems<string>();
      }
    }


    static public FixedList<KeyValue> TermTimeUnitsList {
      get {
        var list = KeyValueList.Parse("Governance.TermTimeUnits.List");

        return list.GetItems();
      }
    }


    protected override void OnInitialize() {
      requirementsList = new Lazy<List<Requirement>>(() => Requirement.GetList(this));
    }


    static public void ExportBpmnDiagrams() {
      var rootDirectory = new DirectoryInfo(BPMN_DIAGRAMS_PATH);

      var targetDirectory = rootDirectory.CreateSubdirectory("export." + DateTime.Now.ToString("yyyy-MM-dd.HHmm"));

      FixedList<BpmnDiagram> list = BpmnDiagram.GetList<BpmnDiagram>()
                                               .ToFixedList();

      list.Sort((x, y) => x.Name.CompareTo(y.Name));

      foreach (var diagram in list) {
        var temp = CleanForDiagramFileName(diagram.Name);

        var fileName = Path.Combine(targetDirectory.FullName, temp + ".bpmn");

        try {
          if (diagram.Xml.Length != 0) {
            File.WriteAllText(fileName, diagram.Xml);
          }
        } catch (Exception e) {
          throw new Exception($"Error processing file {fileName}", e);
        }

      }  // foreach
    }


    static public void UpdateBpmnDiagrams() {
      var directory = new DirectoryInfo(BPMN_DIAGRAMS_PATH);

      var files = directory.GetFiles("*.bpmn");
      var tags = String.Empty;

      foreach (var file in files) {
        var diagramName = file.Name.Replace(".bpmn", String.Empty);
        var procedureId = -1;

        if (EmpiriaString.IsInteger(diagramName)) {
          procedureId = EmpiriaString.ToInteger(diagramName);
          var procedure = Procedure.Parse(procedureId);
          if (procedure.Code == "No disponible") {
            diagramName = $"[{procedure.Id.ToString("000")}] {procedure.ShortName}";
          } else {
            diagramName = $"[{procedure.Id.ToString("000")}] {procedure.Code} {procedure.ShortName}";
          }
          if (procedure.Modality.Length != 0) {
            diagramName += $" (Modalidad: {procedure.Modality})";
         }

          diagramName = EmpiriaString.TrimAll(diagramName);
          tags = procedure.Keywords;
        }

        var xml = File.ReadAllText(file.FullName);

        var diagram = new BpmnDiagram(diagramName, xml, tags);

        diagram.Save();

        if (procedureId != -1) {
          var procedure = Procedure.Parse(procedureId);
          procedure.SetBpmnDiagram(diagram);
        }
      }
    }

    static public void UpdateAll() {
      var procedures = BaseObject.GetList<Procedure>();

      foreach (var procedure in procedures) {
        procedure.Save();

        UpdateAllRelatedClauses(procedure, Contract.Parse(565), procedure.LegalInfo.Ronda13Consorcio);
        UpdateAllRelatedClauses(procedure, Contract.Parse(566), procedure.LegalInfo.Ronda13Individual);
        UpdateAllRelatedClauses(procedure, Contract.Parse(567), procedure.LegalInfo.Ronda14Consorcio);
        UpdateAllRelatedClauses(procedure, Contract.Parse(568), procedure.LegalInfo.Ronda14Individual);
        UpdateAllRelatedClauses(procedure, Contract.Parse(569), procedure.LegalInfo.Ronda21Consorcio);
        UpdateAllRelatedClauses(procedure, Contract.Parse(570), procedure.LegalInfo.Ronda21Individual);
        UpdateAllRelatedClauses(procedure, Contract.Parse(575), procedure.LegalInfo.Ronda24Consorcio);
        UpdateAllRelatedClauses(procedure, Contract.Parse(576), procedure.LegalInfo.Ronda24Individual);
        UpdateAllRelatedClauses(procedure, Contract.Parse(577), procedure.LegalInfo.Santuario);
      }
    }


    private static void UpdateAllRelatedClauses(Procedure procedure,
                                                Contract contract, string clausesAsText) {
      if (clausesAsText.Contains("Anexo")) {
        return;
      }

      clausesAsText = clausesAsText.Replace(',', ' ')
                                   .Replace(';', ' ');

      var clauseTextParts = clausesAsText.Split(' ')
                                         .Where((x) => EmpiriaString.IsQuantity(x));

      if (clauseTextParts == null) {
        return;
      }
      var clauses = contract.Clauses.FindAll((x) => clauseTextParts.Contains(x.Number) && x.Section == "Cláusulas");

      if (clauses == null || clauses.Count == 0) {
        return;
      }
      foreach (var clause in clauses) {
        if (clause.RelatedProcedures.Contains((x) => x.Procedure.Equals(procedure))) {
          continue;
        }
        clause.AddRelatedProcedure(procedure);
      }
    }

    #endregion Constructors and parsers

    #region Public properties

    [DataField("ProcedureName")]
    public string Name {
      get;
      private set;
    }

    [DataField("ShortName")]
    public string ShortName {
      get;
      private set;
    }


    [DataField("GovName")]
    public string GovName {
      get;
      private set;
    }


    [DataField("Modality")]
    public string Modality {
      get;
      private set;
    }

    [DataField("Code")]
    public string Code {
      get;
      private set;
    }

    [DataField("Notes")]
    public string Notes {
      get;
      private set;
    }


    private ProcedureHypertext _hypertextFields = null;
    public ProcedureHypertext HypertextFields {
      get {
        if (_hypertextFields == null) {
          _hypertextFields = new ProcedureHypertext(this);
        }
        return _hypertextFields;
      }
    }

    public string NotesHypertext {
      get {
        return Presentation.Hypertext.ToAcronymHypertext(this.Notes);
      }
    }

    public string Keywords {
      get {
        return EmpiriaString.BuildKeywords(this.Code, this.ShortName, this.Modality, this.Name, this.GovName,
                                           this.EntityName, this.AuthorityContact, this.ProjectType,
                                           this.Tags, this.Theme, this.Subtheme,
                                           this.FilingCondition.StartsWhenTrigger, this.FilingCondition.HowToFileAddress,
                                           this.LegalInfo.LegalBasis, this.LegalInfo.Obligation);
      }
    }

    [DataField("OfficialURL")]
    public string OfficialURL {
      get;
      private set;
    }


    [DataField("RegulationURL")]
    public string RegulationURL {
      get;
      private set;
    }


    [DataField("Theme")]
    public string Theme {
      get;
      private set;
    }


    [DataField("Subtheme")]
    public string Subtheme {
      get;
      private set;
    }


    [DataField("Tags")]
    public string Tags {
      get;
      private set;
    }


    [DataField("ExecutionMode")]
    public string ExecutionMode {
      get;
      private set;
    }

    [DataField("ProjectType")]
    public string ProjectType {
      get;
      private set;
    }


    [DataField("ProjectTypeFlags")]
    public int ProjectTypeFlags {
      get;
      private set;
    }


    [DataField("StageFlags")]
    public int StageFlags {
      get;
      private set;
    }


    [DataField("Entity")]
    public string EntityName {
      get;
      private set;
    }


    [DataField("Authority")]
    public string AuthorityName {
      get;
      private set;
    }


    [DataField("AuthorityTitle")]
    public string AuthorityTitle {
      get;
      private set;
    }


    [DataField("AuthorityContact")]
    public string AuthorityContact {
      get;
      private set;
    }


    [DataObject]
    public Authority Authority {
      get;
      private set;
    } = Authority.Empty;


    [DataObject]
    public LegalInfo LegalInfo {
      get;
      private set;
    } = LegalInfo.Empty;


    [DataObject]
    public FilingCondition FilingCondition {
      get;
      private set;
    } = FilingCondition.Empty;


    public FixedList<Requirement> Requirements {
      get {
        return requirementsList.Value.ToFixedList();
      }
    }


    [DataObject]
    public FilingFee FilingFee {
      get;
      private set;
    } = FilingFee.Empty;


    [DataField("BpmnDiagramId")]
    public BpmnDiagram BpmnDiagram {
      get;
      private set;
    }

    #endregion Public properties

    #region Public methods


    protected override void OnSave() {
      ProcedureData.WriteProcedure(this);
    }


    internal void SetBpmnDiagram(BpmnDiagram diagram) {
      this.BpmnDiagram = diagram;
      Save();
    }


    public void Update(JsonObject data) {
      Assertion.AssertObject(data, "data");

      this.Load(data);
    }

    #endregion Public methods

    #region Private methods


    static private string CleanForDiagramFileName(string name) {
      string temp = name.Replace("%", "pc");

      temp = temp.Replace(":", "-");
      temp = temp.Replace("\\", " ");
      temp = temp.Replace("/", " ");

      if (temp.Length > 176) {
        temp = temp.Substring(0, 176);
      }

      return temp;
    }


    private void Load(JsonObject data) {
      this.Name = data.Get<string>("name", this.Name);
      this.ShortName = data.Get<string>("shortName", this.Name);
      this.Code = data.Get<string>("code", this.Code);
      this.Notes = data.Get<string>("notes", this.Notes);
      this.OfficialURL = data.Get<string>("officialUrl", this.OfficialURL);
      this.RegulationURL = data.Get<string>("regulationUrl", this.OfficialURL);

      this.Authority = Authority.Parse(data.Slice("authority"));
      this.LegalInfo = LegalInfo.Parse(data.Slice("legalInfo"));
      this.FilingCondition = FilingCondition.Parse(data.Slice("filingCondition"));
      this.FilingFee = FilingFee.Parse(data.Slice("filingFee"));
    }

    #endregion Private methods

  }  // class Procedure

}  // namespace Empiria.Governance.Government
