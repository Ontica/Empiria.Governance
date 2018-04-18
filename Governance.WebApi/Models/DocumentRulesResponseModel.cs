/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Web API                  *
*  Assembly : Empiria.Governance.WebApi.dll                    Pattern : Response methods                    *
*  Type     : DocumentRulesResponseModel                       License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Response static methods for document rules entities.                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Workflow.Definition;

using Empiria.Governance.Contracts;

namespace Empiria.Governance.WebApi {

  /// <summary>Response static methods for document rules entities.</summary>
  static internal class DocumentRulesResponseModel {

    static internal object ToResponse(this IList<ContractRule> rulesList, Clause clause) {
      return new {
        uid = clause.UID,
        section = clause.Section,
        clauseNo = clause.Number,
        title = clause.Title,
        contractUID = clause.Contract.UID,
        rules = rulesList.ToResponse(),
      };
    }


    static internal ICollection ToResponse(this IList<ContractRule> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var rule in list) {
        var item = new {
          uid = rule.UID,
          name = rule.Name,
          description = rule.Description,
          workflowObjectUID = WorkflowObject.Parse(rule.WorkflowObjectId).UID,
          procedures = rule.Procedures.ToResponse()
        };

        array.Add(item);
      }
      return array;
    }

  }  // class DocumentRulesResponseModel

}  // namespace Empiria.Governance.WebApi
