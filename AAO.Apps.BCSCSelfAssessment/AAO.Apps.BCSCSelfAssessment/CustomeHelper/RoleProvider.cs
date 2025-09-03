namespace AAO.Apps.BCSCSelfAssessment.CustomeHelper
{
    using System;
    using System.Web.Security;
    using AAO.Apps.BCSCSelfAssessment.Proxy;
    using AAO.Common.BCSCSelfAssessment;
    using Newtonsoft.Json;

    public class AAORoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] s;
            string[] words = username.Split('|');
            UserVM uName = new UserVM
            {
                UserName = words[0],
            };

            string serviceJson = JsonConvert.SerializeObject(uName);

            string urlGetUserRole = AAOGlobalConstants.SiteWebAPIUrl + "login/GetUserByRole";
            string resultUserRole = HttpProxy.HttpPost(urlGetUserRole, serviceJson, "application/json; charset=utf-8", "POST");
            string role = JsonConvert.DeserializeObject<string>(resultUserRole);
            s = new string[] { role };
            return s;

            // throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

    }
}