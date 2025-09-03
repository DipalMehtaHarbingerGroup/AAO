namespace AAO.Apps.BCSCSelfAssessment.CustomeHelper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using DTO.BCSCSelfAssessment;

    public static class CustomeHelper
    {
        public static IHtmlString CheckBoxList(this HtmlHelper helper, IEnumerable<SelectListItem> items, string name, string[] marks = null)
        {
            var output = new StringBuilder();
            int j = 1;
            int k = 1;
            if (name == "TypeofQuestionList")
            {
                int i = 0;
                output.AppendFormat("<div class='col-lg-3 col-md-6 col-sm-6 padleft0'>");
                foreach (var item in items.ToList())
                {
                    output.AppendFormat("<label class='container-checkbox'>{0} (<span id='spanValue_" + item.Value + "'>" + marks[i] + "</span>)", item.Text);
                    output.AppendFormat("<input id='questionfilter_" + item.Value + "' name=" + name + " {1}  type='checkbox' value=" + item.Value + " class='" + name + "' totalCount='" + marks[i] + "'/>", item.Value, item.Selected ? "Checked" : "");
                    output.AppendFormat("<span class='checkmark'></span></label>");
                    if (k % 2 == 0 && k != 1)
                    {
                        output.AppendFormat("</div><div class='col-lg-3 col-md-6 col-sm-6 padleft0'>");
                    }
                    i++;
                    k++;
                }
                output.AppendFormat("</div>");
            }
            else
            {
                output.AppendFormat("<div class='col-lg-4 padleft0'><div class=''>");
                int counter = 0;
                foreach (var item in items.ToList())
                {
                    counter++;
                    string[] dataValue = item.Value.Split(':');
                    output.AppendFormat("<label class='container-checkbox'>");
                    output.AppendFormat("Section " + counter + ": " + item.Text);
                    output.AppendFormat("<input name=" + name + " {1}  type='checkbox' value=" + dataValue[0]
                         + " TotalCount='" + dataValue[1] + "'"
                           + " MarkQuestionCount='" + dataValue[2] +"'" +
                            " IncorrectAsnwerCount='" + dataValue[3] +"'" 
                            + " ExamSkipQuestionCount='" + dataValue[4] + "'" +
                        " class='" + name + "'>", dataValue[0], item.Selected ? "Checked" : "");
                    output.AppendFormat("<span class='checkmark martop2'></span>");
                    output.AppendFormat("</label>");

                    if (j % 5 == 0 && j != 1)
                    {
                        output.AppendFormat("</div></div><div class='col-lg-4 padleft0'><div class=''>");
                    }
                    j++;
                }
                output.AppendFormat("</div></div>");
            }
            
            return new HtmlString(output.ToString());
        }
    }
}