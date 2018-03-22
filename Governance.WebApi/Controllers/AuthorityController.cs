/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Web API                  *
*  Assembly : Empiria.Governance.WebApi.dll                    Pattern : Web Api Controller                  *
*  Type     : AuthorityController                              License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Gets and sets authority and entities data.                                                     *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Web.Http;

using Empiria.WebApi;

using Empiria.Governance.Government;

namespace Empiria.Governance.WebApi {

  /// <summary>Gets and sets authority and entities data.</summary>
  public class AuthorityController : WebApiController {

    #region Public APIs

    [HttpGet]
    [Route("v1/modeling/entities")]
    public CollectionModel GetEntitiesList([FromUri] string filter = "") {
      try {
        var list = Entity.GetList(filter ?? String.Empty);

        return new CollectionModel(this.Request, list.ToResponse(),
                                   typeof(Entity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/modeling/entities/{entityUID}")]
    public SingleObjectModel GetEntity([FromUri] string entityUID) {
      try {
        base.RequireResource(entityUID, "entityUID");

        var entity = Entity.Parse(entityUID);

        return new SingleObjectModel(this.Request, entity.ToResponse(),
                                     typeof(Entity).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }


    [HttpGet]
    [Route("v1/modeling/offices/{officeUID}")]
    public SingleObjectModel GetOffice([FromUri] string officeUID) {
      try {
        base.RequireResource(officeUID, "officeUID");

        var authority = Office.Parse(officeUID);

        return new SingleObjectModel(this.Request, authority.ToResponse(),
                                     typeof(Office).FullName);

      } catch (Exception e) {
        throw base.CreateHttpException(e);
      }
    }

    #endregion Public APIs

  }  // class AuthorityController

}  // namespace Empiria.Governance.WebApi
