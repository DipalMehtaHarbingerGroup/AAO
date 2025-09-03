namespace AAO.Common.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Topic
    {
        [JsonProperty("topic", NullValueHandling = NullValueHandling.Ignore)]
        public TopicContent TopicContent { get; set; }
    }

    public partial class TopicContent
    {
        [JsonProperty("active", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Active { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("deleted", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Deleted { get; set; }

        [JsonProperty("modifiedOn")]
        public object ModifiedOn { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("mappings", NullValueHandling = NullValueHandling.Ignore)]
        public List<Mapping> Mappings { get; set; }
    }

    public partial class Mapping
    {
        [JsonProperty("program", NullValueHandling = NullValueHandling.Ignore)]
        public string Program { get; set; }

        [JsonProperty("programId", NullValueHandling = NullValueHandling.Ignore)]
        public string ProgramId { get; set; }

        [JsonProperty("subProgram", NullValueHandling = NullValueHandling.Ignore)]
        public string SubProgram { get; set; }

        [JsonProperty("subProgramId", NullValueHandling = NullValueHandling.Ignore)]
        public string SubProgramId { get; set; }

        [JsonProperty("subSpecialty", NullValueHandling = NullValueHandling.Ignore)]
        public string SubSpecialty { get; set; }

        [JsonProperty("subSpecialtyId", NullValueHandling = NullValueHandling.Ignore)]
        public string SubSpecialtyId { get; set; }
    }

    public partial class Topic
    {
        public static Topic FromJson(string json)
        {
            return JsonConvert.DeserializeObject<Topic>(json, AAO.Common.BCSCSelfAssessment.ConverterTopic.Settings);
        }
    }

    public static class SerializeTopic
    {
        public static string ToJson(this Topic self)
        {
            return JsonConvert.SerializeObject(self, AAO.Common.BCSCSelfAssessment.ConverterTopic.Settings);
        }
    }

    internal class ConverterTopic
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