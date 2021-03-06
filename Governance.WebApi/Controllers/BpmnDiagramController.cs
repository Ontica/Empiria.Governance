﻿/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Web API                  *
*  Assembly : Empiria.Governance.WebApi.dll                    Pattern : Web Api Controller                  *
*  Type     : BpmnDiagramController                            License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Provides services for read and write BPMN diagrams.                                            *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.Json;
using Empiria.WebApi;

using Empiria.Workflow.Definition;

namespace Empiria.Governance.WebApi {

  /// <summary>Provides services for read and write BPMN diagrams.</summary>
  public class BpmnDiagramController : WebApiController {

    #region GET methods

    [HttpGet]
    [Route("v1/process-definitions")]
    public CollectionModel GetProcessDefinitionList([FromUri] string keywords = "") {
      try {
        var list = BpmnDiagram.Search(keywords);

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(BpmnDiagram).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/process-definitions/{processDefinitionUID}")]
    public SingleObjectModel GetProcessDefinition(string processDefinitionUID) {
      try {
        base.RequireResource(processDefinitionUID, "processDefinitionUID");

        var processDefinition = BpmnDiagram.Parse(processDefinitionUID);

        return new SingleObjectModel(this.Request, processDefinition.ToResponse(),
                                     typeof(BpmnDiagram).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion GET methods

    #region UPDATE methods

    [HttpPost]
    [Route("v1/process-definitions")]
    public SingleObjectModel CreateProcessDefinition([FromBody] object body) {
      try {
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var processDefinition = new BpmnDiagram(bodyAsJson);

        processDefinition.Save();

        return new SingleObjectModel(this.Request, processDefinition.ToResponse(),
                                     typeof(BpmnDiagram).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPut, HttpPatch]
    [Route("v1/process-definitions/{processDefinitionUID}")]
    public SingleObjectModel UpdateProcessDefinition(string processDefinitionUID,
                                                    [FromBody] object body) {
      try {
        base.RequireResource(processDefinitionUID, "processDefinitionUID");
        base.RequireBody(body);

        var bodyAsJson = JsonObject.Parse(body);

        var processDefinition = BpmnDiagram.Parse(processDefinitionUID);

        processDefinition.Update(bodyAsJson);

        processDefinition.Save();

        return new SingleObjectModel(this.Request, processDefinition.ToResponse(),
                                     typeof(BpmnDiagram).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/process-definitions/load-from-files")]
    public void LoadFromFiles() {
      try {
        Empiria.Governance.Government.Procedure.UpdateBpmnDiagrams();

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpPost]
    [Route("v1/process-definitions/export-to-files")]
    public void ExportToFiles() {
      try {
        Empiria.Governance.Government.Procedure.ExportBpmnDiagrams();

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion UPDATE methods

  }  // class BpmnDiagramController

}  // namespace Empiria.Governance.WebApi
