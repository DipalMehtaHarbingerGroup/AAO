namespace AAO.Common.BCSCSelfAssessment
{
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class QuestionQAS
    {
        [JsonProperty("question", NullValueHandling = NullValueHandling.Ignore)]
        public List<QuestionElement> QuestionQuestion { get; set; }
    }

    public partial class QuestionElement
    {
        [JsonProperty("active", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Active { get; set; }

        [JsonProperty("answer", NullValueHandling = NullValueHandling.Ignore)]
        public List<Answer> Answer { get; set; }

        [JsonProperty("deleted", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Deleted { get; set; }

        [JsonProperty("discussion", NullValueHandling = NullValueHandling.Ignore)]
        public Discussion Discussion { get; set; }

        [JsonProperty("friendlyid", NullValueHandling = NullValueHandling.Ignore)]
        public string Friendlyid { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public List<Image> Image { get; set; }

        [JsonProperty("references", NullValueHandling = NullValueHandling.Ignore)]
        public string References { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public long? Status { get; set; }

        [JsonProperty("stem", NullValueHandling = NullValueHandling.Ignore)]
        public string Stem { get; set; }

        [JsonProperty("createdOn", NullValueHandling = NullValueHandling.Ignore)]
        public System.DateTime? CreatedOn { get; set; }

        [JsonProperty("modifiedOn", NullValueHandling = NullValueHandling.Ignore)]
        public System.DateTime? ModifiedOn { get; set; }

        [JsonProperty("topicMapping", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> TopicMapping { get; set; }

        [JsonProperty("notRandomizeAnswers", NullValueHandling = NullValueHandling.Ignore)]
        public bool? NotRandomizeAnswers { get; set; }

        [JsonProperty("relatedContent", NullValueHandling = NullValueHandling.Ignore)]
        public string RelatedContents { get; set; }
    }

    public partial class Answer
    {
        [JsonProperty("correct", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Correct { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("sequence", NullValueHandling = NullValueHandling.Ignore)]
        public long? Sequence { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public List<Image> Image { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("caption", NullValueHandling = NullValueHandling.Ignore)]
        public string Caption { get; set; }

        [JsonProperty("comments", NullValueHandling = NullValueHandling.Ignore)]
        public string Comments { get; set; }

        [JsonProperty("credit", NullValueHandling = NullValueHandling.Ignore)]
        public string Credit { get; set; }

        [JsonProperty("figure", NullValueHandling = NullValueHandling.Ignore)]
        public string Figure { get; set; }

        [JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        public string Path { get; set; }

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }
    }

    public partial class Discussion
    {
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public List<Image> Image { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }
    }

    public partial class QuestionQAS
    {
        public static QuestionQAS FromJson(string json)
        {
            return JsonConvert.DeserializeObject<QuestionQAS>(json, AAO.Common.BCSCSelfAssessment.Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this QuestionQAS self)
        {
            return JsonConvert.SerializeObject(self, AAO.Common.BCSCSelfAssessment.Converter.Settings);
        }
    }

    internal class Converter
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