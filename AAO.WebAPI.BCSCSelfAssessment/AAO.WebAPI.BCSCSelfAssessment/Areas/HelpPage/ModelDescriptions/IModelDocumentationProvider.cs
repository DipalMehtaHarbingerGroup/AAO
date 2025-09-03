using System;
using System.Reflection;

namespace AAO.WebAPI.BCSCSelfAssessment.Areas.HelpPage.ModelDescriptions
{
    public interface IModelDocumentationProvider
    {
        string GetDocumentation(MemberInfo member);

        string GetDocumentation(Type type);
    }
}