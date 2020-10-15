/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Web API                  *
*  Assembly : Empiria.Governance.WebApi.dll                    Pattern : Web Api Controller                  *
*  Type     : CataloguesController                             License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Get and set apis for very basic entities or value objects.                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.DataTypes.Time;
using Empiria.WebApi;

using Empiria.Governance.Government;

namespace Empiria.Governance.WebApi {

  /// <summary>Get and set apis for very basic entities or value objects.</summary>
  public class CataloguesController : WebApiController {

    #region GET methods

    [HttpGet]
    [Route("v1/catalogues/procedure-starts-when")]
    public CollectionModel GetProcedureStartsWhenList() {
      try {
        var list = Procedure.StartsWhenList;

        return new CollectionModel(this.Request, list, typeof(StartsWhen).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/catalogues/procedure-themes")]
    public CollectionModel GetProcedureThemesList() {
      try {
        var list = Procedure.ThemesList;

        return new CollectionModel(this.Request, list, "Governance.Procedure.Theme");

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/catalogues/term-time-units")]
    public CollectionModel GetTermTimeUnitsList() {
      try {
        var list = Procedure.TermTimeUnitsList;

        return new CollectionModel(this.Request, list, typeof(TimeUnit).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion GET methods

  }  // class CataloguesController

}  // namespace Empiria.Governance.WebApi
