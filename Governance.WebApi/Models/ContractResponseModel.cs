﻿/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Web API                  *
*  Assembly : Empiria.Governance.WebApi.dll                    Pattern : Response methods                    *
*  Type     : ContractResponseModels                           License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Response static methods for contract entities.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections;
using System.Collections.Generic;

using Empiria.Governance.Contracts;

namespace Empiria.Governance.WebApi {

  /// <summary>Response static methods for contract entities.</summary>
  static internal class ContractResponseModel {

    static internal ICollection ToResponse(this IList<Contract> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var legalDocument in list) {
        var item = new {
          uid = legalDocument.UID,
          name = legalDocument.Name,
          url = legalDocument.Url
        };
        array.Add(item);
      }
      return array;
    }

    static internal object ToResponse(this Contract contract,
                                      IList<Clause> clausesList) {
      return new {
        uid = contract.UID,
        name = contract.Name,
        url = contract.Url,
        clauses = clausesList.ToResponse(),
      };
    }


    static internal ICollection ToResponse(this IList<Clause> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var documentItem in list) {
        var item = documentItem.ToShortResponse();

        array.Add(item);
      }
      return array;
    }


    static internal object ToShortResponse(this Clause clause) {
      return new {
        uid = clause.UID,
        section = clause.Section,
        clauseNo = clause.Number,
        title = clause.Title,
        contractUID = clause.Contract.UID,
      };
    }

    static internal object ToResponse(this Clause clause) {
      return new {
        uid = clause.UID,
        section = clause.Section,
        clauseNo = clause.Number,
        title = clause.Title,
        contractUID = clause.Contract.UID,

        text = clause.Text,
        sourcePageNo = clause.DocumentPageNo,
        hypertext = clause.AsHypertext,
        notes = clause.Notes,
        status = clause.Status,
        relatedProcedures = clause.RelatedProcedures.ToResponse(),
        contract = new {
          uid = clause.Contract.UID,
          name = clause.Contract.Name,
          url = clause.Contract.Url
        }
      };
    }


    static internal ICollection ToResponse(this IList<RelatedProcedure> list) {
      ArrayList array = new ArrayList(list.Count);

      foreach (var relatedProcedure in list) {
        var item = relatedProcedure.ToResponse();

        array.Add(item);
      }
      return array;
    }

    static internal object ToResponse(this RelatedProcedure relatedProcedure) {
      return new {
        uid = relatedProcedure.UID,
        procedure = relatedProcedure.Procedure.ToShortResponse(),
        //maxFilingTerm = relatedProcedure.MaxFilingTerm,
        //maxFilingTermType = relatedProcedure.MaxFilingTermType,
        //startsWhen = relatedProcedure.StartsWhen,
        //startsWhenTrigger = relatedProcedure.StartsWhenTrigger,
        notes = relatedProcedure.Notes
      };
    }

  }  // class ContractResponseModels

}  // namespace Empiria.Governance.WebApi
