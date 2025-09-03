// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using QuickType;
//
//    var userJson = UserJson.FromJson(jsonString);

namespace AAO.Common.BCSCSelfAssessment
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class UserJson
    {
        [JsonProperty("MasterCustomerId", NullValueHandling = NullValueHandling.Ignore)]
        public string MasterCustomerId { get; set; }

        [JsonProperty("FirstName", NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty("LastName", NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty("PrimaryEmail", NullValueHandling = NullValueHandling.Ignore)]
        public string PrimaryEmail { get; set; }

        [JsonProperty("CommunicationsEmail", NullValueHandling = NullValueHandling.Ignore)]
        public string CommunicationsEmail { get; set; }

        [JsonProperty("ResidencyStart", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? ResidencyStart { get; set; }

        [JsonProperty("ResidencyEnd", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? ResidencyEnd { get; set; }

        [JsonProperty("ResidencyProgramName", NullValueHandling = NullValueHandling.Ignore)]
        public string ResidencyProgramName { get; set; }

        [JsonProperty("ResidencyProgramId", NullValueHandling = NullValueHandling.Ignore)]
        public string ResidencyProgramId { get; set; }

        [JsonProperty("ProgramDirectorProgramName")]
        public string ProgramDirectorProgramName { get; set; }

        [JsonProperty("ProgramDirectorProgramId")]

        public string ProgramDirectorProgramId { get; set; }

        [JsonProperty("KnownMasterCustomerIds")]
        public string[] KnownMasterCustomerIds { get; set; }

        [JsonProperty("RenewalMessage")]

        public string RenewalMessage { get; set; }

        [JsonProperty("RenewalLink")]

        public string RenewalLink { get; set; }
    }

    public partial class UserJson
    {
        // public static UserJson FromJson(string json) => JsonConvert.DeserializeObject<UserJson>(json, AAO.Common.BCSCSelfAssessment.UserConverter.Settings);
        public static UserJson FromJson(string json)
        {
            return JsonConvert.DeserializeObject<UserJson>(json, AAO.Common.BCSCSelfAssessment.UserConverter.Settings);
        }
    }

    public static class UserSerialize
    {
        public static string ToJson(this UserJson self)
        {
            return JsonConvert.SerializeObject(self, AAO.Common.BCSCSelfAssessment.UserConverter.Settings);
        }
    }

    internal static class UserConverter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal },
            },
        };
    }
}
