/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Web API                  *
*  Assembly : Empiria.Governance.WebApi.dll                    Pattern : Response methods                    *
*  Type     : DocumentResponseModel                            License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Response static methods for documents.                                                         *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Empiria.Governance.WebApi {

  /// <summary>Response static methods for documents.</summary>
  static internal class DocumentResponseModel {

    static internal ICollection ToResponse(this IList<Document> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var document in list) {
        var item = new {
          uid = document.UID,
          type = document.DocumentType,
          name = document.Name,
          code = document.Code,
          authority = document.Authority,
          stage = document.Stage,
          version = document.Version,
          lastUpdated = document.LastUpdated,
          fromDate = document.FromDate,
          toDate = document.ToDate,
          url = document.Url,
          officialUrl = document.OfficialURL
        };
        array.Add(item);
      }
      return array;
    }

  }  // class DocumentResponseModel

}  // namespace Empiria.Governance.WebApi
