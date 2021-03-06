﻿/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Web API                  *
*  Assembly : Empiria.Governance.WebApi.dll                    Pattern : Web Api Controller                  *
*  Type     : RequirementsController                           License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Gets and sets procedure requirements data.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Governance.Government;

namespace Empiria.Governance.WebApi {

  /// <summary>Gets and sets procedure requirements data.</summary>
  public class RequirementsController : WebApiController {

    #region GET methods

    [HttpGet]
    [Route("v1/procedures/{procedureUID}/requirements")]
    public CollectionModel GetProcedureRequirements([FromUri] string procedureUID) {
      try {
        var procedure = Procedure.Parse(procedureUID);

        var requirements = procedure.Requirements;

        return new CollectionModel(this.Request, requirements.ToResponse(),
                                   typeof(Requirement).FullName);
      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion GET methods

    #region UPDATE methods

    [HttpPost]
    [Route("v1/procedure-requirements/update-all")]
    public void UpdateAllDocuments() {
      try {
        Requirement.UpdateAll();

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion UPDATE methods

  }  // class RequirementsController

}  // namespace Empiria.Governance.WebApi
