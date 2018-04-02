/* Empiria Governance ****************************************************************************************
*                                                                                                            *
*  Solution : Empiria Governance                               System  : Governance Management System        *
*  Assembly : Empiria.Governance.dll                           Pattern : Enumeration                         *
*  Type     : HypertextEngine                                  License : Please read LICENSE.txt file        *
*                                                                                                            *
*  Summary  : Represents a time unit used to describe activity due terms.                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Empiria.DataTypes;
using Empiria.Governance.Contracts;

namespace Empiria.Governance.Presentation {

  public class Hypertext {

    static public string ToAcronymHypertext(string source) {
      var template = TextTemplate.Parse("Acronym.Link");

      var documentsList = Document.GetList();

      var hypertext = source;

      foreach (var document in documentsList) {

        // Begin dictionary work

        var textReplacementRules = new Dictionary<string, string>(2);

        textReplacementRules.Add("{{URL}}", document.Url);
        textReplacementRules.Add("{{ACRONYM}}", document.Code);
        textReplacementRules.Add("{{BUBBLE-TEXT}}", document.Name);

        var content = template.ParseContent(textReplacementRules);

        // End dictionary work

        string textToFind = String.Format(@"\b{0}\b", document.Code);

        hypertext = Regex.Replace(hypertext, textToFind, content);
      }
      return hypertext;
    }

    static public string ToTermDefinitionHypertext(string source, Contract contract) {
      var template = TextTemplate.Parse("Term.Definition");

      var clausesList = Clause.GetList(contract);
      clausesList = clausesList.FindAll((x) => x.Section == "Definiciones" && x.Number == "1.1");
      clausesList.Sort((x, y) => x.Title.Length.CompareTo(y.Title.Length));
      clausesList.Reverse();

      var hypertext = source;

      foreach (var clause in clausesList) {

        // Begin dictionary work

        var textReplacementRules = new Dictionary<string, string>(2);

        var clauseTitle = CleanClauseTitle(clause.Title);

        textReplacementRules.Add("{{TERM}}", clauseTitle);
        textReplacementRules.Add("{{DEFINITION}}", clause.Text);

        var content = template.ParseContent(textReplacementRules);

        // End dictionary work

        var regex = new Regex(String.Format(@"(?<!<[^>]*)\b{0}\b", clauseTitle));

        hypertext = regex.Replace(hypertext, content, 10);
      }

      return hypertext;
    }

    private static string CleanClauseTitle(string title) {
      title = title.Replace("“", "");
      title = title.Replace("”", "");
      title = title.Replace("\"", "");

      return EmpiriaString.TrimAll(title);
    }
  }

  //}  // class Hypertext

}  // namespace Empiria.Governance.Presentation
