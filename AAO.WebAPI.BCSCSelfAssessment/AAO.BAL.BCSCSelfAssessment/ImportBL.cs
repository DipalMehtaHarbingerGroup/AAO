namespace AAO.BAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Configuration;
    using AAO.Common.BCSCSelfAssessment;
    using AAO.DAL.BCSCSelfAssessment;
    using HtmlAgilityPack;
    using Serilog;
    using QRREADER.BCSCSelfAssessment;

    public static class ImportBL
    {
        private static string apiUrl = WebConfigurationManager.AppSettings["ApiUrl"];
        private static string sAttrLogPath = ConfigurationManager.AppSettings.Get("LogfilePathBAL");

        public static int GetQuestionCount(ServiceCallVM serviceCall)
        {
            return ImportDAL.GetQuestionCount(serviceCall);
        }

        public static async Task<ResponseStatusVM> ImportQuestion(ServiceCallVM serviceCall)
        {
            int count = ImportDAL.GetQuestionCount(serviceCall);
            if (count <= 0)
            {
                return await ImportBL.SubmitQASData(serviceCall);
            }

            return null;
        }

        public static async Task<ResponseStatusVM> SubmitQASData(ServiceCallVM serviceCall)
        {
            string url = apiUrl + "question/getquestions?status=" + serviceCall.Status + "&program=Resident&subProgram=Self-Assessment";
            string responseBody;
            string jsonFilePath = ConfigurationManager.AppSettings.Get("ResponseJsonfilePath");
            string tableName = string.Empty;
            var myUniqueFileName = string.Format(@"{0}.json", Guid.NewGuid());
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            responseBody = await response.Content.ReadAsStringAsync();
                            var data = QuestionQAS.FromJson(responseBody);
                            if (data.QuestionQuestion.Count > 0)
                            {
                                foreach (var questionDetails in data.QuestionQuestion)
                                {
                                    QuestionVM questioData = new QuestionVM();
                                    if (questionDetails.Image.Count > 0)
                                    {
                                        questioData.QuestionType = false;
                                    }
                                    else
                                    {
                                        questioData.QuestionType = true;
                                    }

                                    questioData.Id = questionDetails.Id;
                                    questioData.References = questionDetails.References;
                                    questioData.Status = Convert.ToInt32(questionDetails.Status);
                                    questioData.Stem = questionDetails.Stem;
                                    questioData.FriendlyId = questionDetails.Friendlyid.ToString();
                                    questioData.Active = Convert.ToBoolean(questionDetails.Status);
                                    questioData.Createdon = Convert.ToDateTime(questionDetails.CreatedOn);
                                    questioData.ModifiedOn = Convert.ToDateTime(questionDetails.ModifiedOn);
                                    questioData.Deleted = Convert.ToBoolean(questionDetails.Deleted);

                                    // insert into Question table and get Question Id
                                    int questionId = ImportDAL.SaveQuestionData(questioData);

                                    // LogHelper.Log(LogTarget.File, "File Question imported(Question Saved)" + DateTime.Now);
                                    if (questionId != 0)
                                    {
                                        // Insert to Xwalk Table and Other Tables Question Id to be passed and Topic Id will be passed  and will return the Topic ID
                                        List<TopicContentVM> topicVMList = new List<TopicContentVM>();
                                        foreach (var questionDetailsTopicMapping in questionDetails.TopicMapping)
                                        {
                                            TopicContentVM topicContent = new TopicContentVM();
                                            Topic d = await GetTopic(questionDetailsTopicMapping);
                                            topicContent.Active = d.TopicContent.Active;
                                            topicContent.Code = d.TopicContent.Code;
                                            topicContent.Deleted = d.TopicContent.Deleted;
                                            topicContent.ModifiedOn = d.TopicContent.ModifiedOn;
                                            string[] bcscMapperContent = d.TopicContent.Title.Split(';');
                                            string chapter = string.Empty;
                                            string chapterName = string.Empty;
                                            string topic = string.Empty;
                                            string topicName = string.Empty;
                                            if (bcscMapperContent.Length > 0 && bcscMapperContent[0] != null)
                                            {
                                                chapter = bcscMapperContent[0];
                                            }

                                            if (bcscMapperContent.Length > 1 && bcscMapperContent[1] != null)
                                            {
                                                chapterName = bcscMapperContent[1];
                                            }

                                            if (bcscMapperContent.Length > 2 && bcscMapperContent[2] != null)
                                            {
                                                topic = bcscMapperContent[2];
                                            }

                                            if (bcscMapperContent.Length > 3 && bcscMapperContent[3] != null)
                                            {
                                                topicName = bcscMapperContent[3];
                                            }

                                            topicContent.Title = d.TopicContent.Title;
                                            topicContent.TopicId = questionDetailsTopicMapping;
                                            topicContent.Chapter = chapter;
                                            topicContent.ChapterName = chapterName;
                                            topicContent.Topic = topic;
                                            topicContent.TopicName = topicName;
                                            topicVMList.Add(topicContent);

                                            // in serting into Topic Table And Get topic Id
                                            int topicId = ImportDAL.SaveTopicData(topicContent);

                                            // LogHelper.Log(LogTarget.File, "File Topic imported(Topic Saved)" + DateTime.Now);
                                            // Creating questionTopicXWalk and assigning value and calling function for Insert
                                            QuestionTopicXWalkVM questionTopicXWalk = new QuestionTopicXWalkVM();
                                            questionTopicXWalk.QuestionId = questionId;
                                            questionTopicXWalk.QASQuestionId = questionDetails.Id;
                                            questionTopicXWalk.QASTopicId = questionDetailsTopicMapping;
                                            questionTopicXWalk.TopicId = topicId;

                                            // calling SaveTopicData for inserting data into questionTopicXwalk Table
                                            int questionTopicXwalkId = ImportDAL.SaveQuestionTopicXWalkData(questionTopicXWalk);

                                            // LogHelper.Log(LogTarget.File, "File questionTopicXwalk imported(questionTopicXwalk Saved)" + DateTime.Now);
                                            // get The mapping details of each topic
                                            // List<MappingVM> topicMappingVMList = new List<MappingVM>();
                                            foreach (Mapping topicmapping in d.TopicContent.Mappings)
                                            {
                                                // Fill ProgramVM and Call DAL for insert into Program
                                                ProgramVM program = new ProgramVM();
                                                program.Program = topicmapping.Program;
                                                program.QASProgramid = topicmapping.ProgramId;
                                                int programId = ImportDAL.SaveProgramData(program);

                                                // LogHelper.Log(LogTarget.File, "File program imported(program Saved)" + DateTime.Now);
                                                // Fill SubProgramVM and Call DAL for insert into SubProgram
                                                SubProgramVM subProgram = new SubProgramVM();
                                                subProgram.Subprogram = topicmapping.SubProgram;
                                                subProgram.QASSubprogramid = topicmapping.SubProgramId;
                                                subProgram.ProgramId = programId;
                                                int subProgramId = ImportDAL.SaveSubProgramData(subProgram);

                                                // LogHelper.Log(LogTarget.File, "File subProgram imported(subProgram Saved)" + DateTime.Now);
                                                // Fill SubSpecialityVM and Call DAL for insert into SubSpeciality
                                                SubSpecialityVM subSpeciality = new SubSpecialityVM();
                                                subSpeciality.Subspecialty = topicmapping.SubSpecialty;
                                                subSpeciality.QASSubspecialtyid = topicmapping.SubSpecialtyId;
                                                subSpeciality.SubprogramId = subProgramId;
                                                int subSpecialityId = ImportDAL.SaveSubSpecialityData(subSpeciality);

                                                // LogHelper.Log(LogTarget.File, "File subSpeciality imported(subSpeciality Saved)" + DateTime.Now);
                                                // Fill SubspecialtyTopicXWalkVM and Call DAL for insert into SubspecialtyTopicXWalk
                                                SubspecialtyTopicXWalkVM subspecialtyTopicXWalk = new SubspecialtyTopicXWalkVM();
                                                subspecialtyTopicXWalk.SubSpecialityId = subSpecialityId;
                                                subspecialtyTopicXWalk.TopicId = topicId;
                                                int subspecialtyTopicXWalkId = ImportDAL.SaveSubspecialtyTopicXWalkData(subspecialtyTopicXWalk);

                                                // UpdateBCSCMapperContentOnUpdate(questionId);

                                                // LogHelper.Log(LogTarget.File, "File subspecialtyTopicXWalk imported(subspecialtyTopicXWalk Saved)" + DateTime.Now);
                                            }
                                        }

                                        // update operation on question by question Id
                                        // get the question Image
                                        if (questionDetails.Image.Count > 0)
                                        {
                                            List<ImageVM> imageVMList = new List<ImageVM>();
                                            foreach (Image questionImage in questionDetails.Image)
                                            {
                                                ImageVM imageVM = new ImageVM();
                                                imageVM.Caption = questionImage.Caption;
                                                imageVM.Comments = questionImage.Comments;
                                                imageVM.Credit = questionImage.Credit;
                                                imageVM.Figure = questionImage.Figure;
                                                imageVM.Path = Convert.ToString(HttpUtility.HtmlDecode(Convert.ToString(questionImage.Path)));
                                                imageVM.Source = questionImage.Source;
                                                imageVM.Title = questionImage.Title;
                                                imageVMList.Add(imageVM);

                                                // insert into QuestionImage and pass LIst And QuestionId get imageId
                                                int questionImageId = ImportDAL.InsertImageData(imageVM, questionId, tableName = "Question_Image");

                                                // LogHelper.Log(LogTarget.File, "File QuestionImage imported(QuestionImage Saved)" + DateTime.Now);
                                            }
                                        }

                                        DiscussionVM discussionVm = new DiscussionVM();

                                        // Discussion text insert
                                        discussionVm.DiscussionText = questionDetails.Discussion.Text;
                                        discussionVm.DiscussionType = false;
                                        discussionVm.QuestionId = questionId;

                                        // insert into Dissicion pass questionId and get dissicionid
                                        int discussionId = ImportDAL.SaveDiscussionData(discussionVm);

                                        // LogHelper.Log(LogTarget.File, "File QuestionDetails imported(QuestionDetails Saved)" + DateTime.Now);
                                        if (questionDetails.Discussion.Image.Count > 0)
                                        {
                                            List<ImageVM> imageDiscussionVMList = new List<ImageVM>();
                                            foreach (Image imageDiscusstionListData in questionDetails.Discussion.Image)
                                            {
                                                ImageVM imageVM = new ImageVM();
                                                imageVM.Caption = imageDiscusstionListData.Caption;
                                                imageVM.Comments = imageDiscusstionListData.Comments;
                                                imageVM.Credit = imageDiscusstionListData.Credit;
                                                imageVM.Figure = imageDiscusstionListData.Figure;
                                                imageVM.Path = imageDiscusstionListData.Path;
                                                imageVM.Source = imageDiscusstionListData.Source;
                                                imageVM.Title = imageDiscusstionListData.Title;
                                                imageDiscussionVMList.Add(imageVM);

                                                // insert into DissicionImage pass DissiconId and Get dissicionImageId
                                                int discussionImageId = ImportDAL.InsertImageData(imageVM, discussionId, tableName = "Discussion_Image");

                                                // LogHelper.Log(LogTarget.File, "File QuestionImage imported(QuestionImage Saved)" + DateTime.Now);
                                            }
                                        }

                                        // Get the answers
                                        if (questionDetails.Answer.Count > 0)
                                        {
                                            List<AnswerVM> answerVMList = new List<AnswerVM>();
                                            foreach (Answer answerVMListData in questionDetails.Answer)
                                            {
                                                AnswerVM answerVM = new AnswerVM();
                                                answerVM.Text = answerVMListData.Text;
                                                answerVM.Correct = answerVMListData.Correct;
                                                answerVM.Sequence = answerVMListData.Sequence;
                                                answerVM.Id = answerVMListData.Id;
                                                answerVM.QuestionId = questionId;
                                                answerVM.AnswerType = false;
                                                answerVMList.Add(answerVM);

                                                // Get the answer Image
                                                // insert into Choice Table pass QuestionId and choice object and get ChoiceId
                                                int answerId = ImportDAL.SaveAnswerData(answerVM);

                                                // LogHelper.Log(LogTarget.File, "File answer imported(answer Saved)" + DateTime.Now);
                                                if (answerVMListData.Image.Count > 0)
                                                {
                                                    List<ImageVM> imageAnswerVMList = new List<ImageVM>();
                                                    foreach (Image answerImageListData in answerVMListData.Image)
                                                    {
                                                        ImageVM imageVM = new ImageVM();
                                                        imageVM.Caption = answerImageListData.Caption;
                                                        imageVM.Comments = answerImageListData.Comments;
                                                        imageVM.Credit = answerImageListData.Credit;
                                                        imageVM.Figure = answerImageListData.Figure;
                                                        imageVM.Path = answerImageListData.Path;
                                                        imageVM.Source = answerImageListData.Source;
                                                        imageVM.Title = answerImageListData.Title;
                                                        imageAnswerVMList.Add(imageVM);

                                                        // insert into Choice Image pass choiceId and ChoiceImageObject and Get ChoiceImageId
                                                        int answerImageId = ImportDAL.InsertImageData(imageVM, answerId, tableName = "Answer_Choice_Image");

                                                        // LogHelper.Log(LogTarget.File, "File answerImage imported(answerImage Saved)" + DateTime.Now);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                Console.Write(e.InnerException);
            }

            return null;
        }

        public static async Task<ResponseStatusVM> RetiredQASData(ServiceCallVM serviceCall, bool defaultexcerpt)
        {
            // string url = apiUrl + "question/getquestions?status=" + serviceCall.Status + "&program=Resident&subProgram=Self-Assessment&modifiedafter=" + serviceCall.Modifiedafter.ToString("MM'-'dd'-'yyyy");
            string url = string.Empty;
            if (defaultexcerpt)
            {
                url = apiUrl + "question/getquestions?status=" + serviceCall.Status + "&program=Resident&subProgram=Self-Assessment";
            }
            else
            {
                url = apiUrl + "question/getquestions?status=" + serviceCall.Status + "&program=Resident&subProgram=Self-Assessment&modifiedafter=" + serviceCall.Modifiedafter.ToString("MM'-'dd'-'yyyy");
            }

            string responseBody;
            string jsonFilePath = ConfigurationManager.AppSettings.Get("ResponseJsonfilePath");
            string tableName = string.Empty;
            var myUniqueFileName = string.Format(@"{0}.json", Guid.NewGuid());
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);

                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                    SecurityProtocolType.Tls11 |
                                    SecurityProtocolType.Tls12;
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            responseBody = await response.Content.ReadAsStringAsync();
                            var data = QuestionQAS.FromJson(responseBody);
                            if (data.QuestionQuestion.Count > 0)
                            {
                                foreach (var questionDetails in data.QuestionQuestion)
                                {
                                    QuestionVM questioData = new QuestionVM();
                                    if (questionDetails.Image.Count > 0)
                                    {
                                        questioData.QuestionType = false;
                                    }
                                    else
                                    {
                                        questioData.QuestionType = true;
                                    }

                                    questioData.Id = questionDetails.Id;
                                    questioData.Status = Convert.ToInt32(questionDetails.Status);
                                    questioData.Active = Convert.ToBoolean(questionDetails.Status);

                                    // Retire Question into Question table and get Question Id
                                    int questionId = ImportDAL.RetiredQuestionData(questioData);
                                    if (questionId != 0)
                                    {
                                        ImportDAL.DeleteContentFromBCSCMapperOnRetired(questionId);
                                        ImportDAL.RetiredQuestionFromRelatedTable(questionId);
                                    }

                                    // LogHelper.Log(LogTarget.File, "File Question imported(Retired Updated)" + DateTime.Now);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                Console.Write(e.InnerException);
            }

            return null;
        }

        // UpdateQASData
        public static async Task<ResponseStatusVM> UpdateQASData(ServiceCallVM serviceCall, bool defaultexcerpt)
        {
            string url = string.Empty;
            var log = new Serilog.LoggerConfiguration().WriteTo.RollingFile(sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();

            if (defaultexcerpt)
            {
                url = apiUrl + "question/getquestions?status=" + serviceCall.Status + "&program=Resident&subProgram=Self-Assessment";
            }
            else
            {
                url = apiUrl + "question/getquestions?status=" + serviceCall.Status + "&program=Resident&subProgram=Self-Assessment&modifiedafter=" + serviceCall.Modifiedafter.ToString("MM'-'dd'-'yyyy");
            }

            string responseBody;
            string jsonFilePath = ConfigurationManager.AppSettings.Get("ResponseJsonfilePath");
            string tableName = string.Empty;
            var myUniqueFileName = string.Format(@"{0}.json", Guid.NewGuid());
            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                    client.Timeout = TimeSpan.FromMinutes(30);
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            responseBody = await response.Content.ReadAsStringAsync();

                            var questiondata = QuestionQAS.FromJson(responseBody);
                            log.Information("Question Data: " + questiondata);
                            if (questiondata.QuestionQuestion.Count > 0)
                            {
                                foreach (var questionDetails in questiondata.QuestionQuestion)
                                {
                                    QuestionVM questioData = new QuestionVM();
                                    if (questionDetails.Image.Count > 0)
                                    {
                                        questioData.QuestionType = false;
                                    }
                                    else
                                    {
                                        questioData.QuestionType = true;
                                    }

                                    questioData.Id = questionDetails.Id;
                                    questioData.References = questionDetails.References;
                                    questioData.Status = Convert.ToInt32(questionDetails.Status);
                                    questioData.Stem = questionDetails.Stem;
                                    questioData.FriendlyId = questionDetails.Friendlyid.ToString();
                                    questioData.Active = Convert.ToBoolean(questionDetails.Status);
                                    questioData.Createdon = Convert.ToDateTime(questionDetails.CreatedOn);
                                    questioData.ModifiedOn = Convert.ToDateTime(questionDetails.ModifiedOn);
                                    questioData.Deleted = Convert.ToBoolean(questionDetails.Deleted);
                                    questioData.NotRandomizeAnswers = Convert.ToBoolean(questionDetails.NotRandomizeAnswers);
                                    questioData.RelatedContents = Convert.ToString(questionDetails.RelatedContents);

                                    // Update into Question table and get Question Id
                                    int questionId = ImportDAL.UpdateQuestionData(questioData);

                                    // LogHelper.Log(LogTarget.File, "File Question imported(Question Updated)" + DateTime.Now);

                                    if (questionId != 0)
                                    {
                                        //Update RelatedContents if not null or empty
                                        if (questioData.RelatedContents != null && !string.IsNullOrEmpty(questioData.RelatedContents))
                                        {
                                            //call SP (AAO_UpdateorInsertRelatedContents) for update Contents
                                            int updatedQuestionid_RC = ImportDAL.UpdateorInsertRelatedContents(questioData, questionId);
                                        }

                                        // Insert to Xwalk Table and Other Tables Question Id to be passed and Topic Id will be passed  and will return the Topic ID
                                        List<TopicContentVM> topicVMList = new List<TopicContentVM>();

                                        List<int> beforeUpdateDb_TopicList = ImportDAL.GetQASTopicIdByQuestionId(questionId);

                                        ImportDAL.DeleteReferenceData(questionId, tableName = "QuestionTopicXwalk");
                                        foreach (var questionDetailsTopicMapping in questionDetails.TopicMapping)
                                        {
                                            TopicContentVM topicContent = new TopicContentVM();
                                            Topic d = await GetTopic(questionDetailsTopicMapping);
                                            topicContent.Active = d.TopicContent.Active;
                                            topicContent.Code = d.TopicContent.Code;
                                            topicContent.Deleted = d.TopicContent.Deleted;
                                            topicContent.ModifiedOn = d.TopicContent.ModifiedOn;
                                            string[] bcscMapperContent = d.TopicContent.Title.Split(';');
                                            string chapter = string.Empty;
                                            string chapterName = string.Empty;
                                            string topic = string.Empty;
                                            string topicName = string.Empty;
                                            if (bcscMapperContent.Length > 0 && bcscMapperContent[0] != null)
                                            {
                                                chapter = bcscMapperContent[0];
                                            }

                                            if (bcscMapperContent.Length > 1 && bcscMapperContent[1] != null)
                                            {
                                                chapterName = bcscMapperContent[1];
                                            }

                                            if (bcscMapperContent.Length > 2 && bcscMapperContent[2] != null)
                                            {
                                                topic = bcscMapperContent[2];
                                            }

                                            if (bcscMapperContent.Length > 3 && bcscMapperContent[3] != null)
                                            {
                                                topicName = bcscMapperContent[3];
                                            }

                                            topicContent.Title = d.TopicContent.Title;
                                            topicContent.TopicId = questionDetailsTopicMapping;
                                            topicContent.Chapter = chapter;
                                            topicContent.ChapterName = chapterName;
                                            topicContent.Topic = topic;
                                            topicContent.TopicName = topicName;
                                            topicVMList.Add(topicContent);

                                            // Update to the Topic Table And Get topic Id
                                            int topicId = ImportDAL.UpdateTopicData(topicContent);

                                            // LogHelper.Log(LogTarget.File, "File Topic imported(Topic Updated)" + DateTime.Now);
                                            // Creating questionTopicXWalk and assigning value and calling function for Insert
                                            QuestionTopicXWalkVM questionTopicXWalk = new QuestionTopicXWalkVM();
                                            questionTopicXWalk.QuestionId = questionId;
                                            questionTopicXWalk.QASQuestionId = questionDetails.Id;
                                            questionTopicXWalk.QASTopicId = questionDetailsTopicMapping;
                                            questionTopicXWalk.TopicId = topicId;

                                            // calling SaveTopicData for inserting data into questionTopicXwalk Table
                                            int questionTopicXwalkId = ImportDAL.UpdateQuestionTopicXWalkData(questionTopicXWalk);

                                            // LogHelper.Log(LogTarget.File, "File questionTopicXwalk imported(questionTopicXwalk Updated)" + DateTime.Now);
                                            // get The mapping details of each topic
                                            foreach (Mapping topicmapping in d.TopicContent.Mappings)
                                            {
                                                // Fill ProgramVM and Call DAL for insert into Program
                                                ProgramVM program = new ProgramVM();
                                                program.Program = topicmapping.Program;
                                                program.QASProgramid = topicmapping.ProgramId;
                                                int programId = ImportDAL.UpdateProgramData(program);

                                                // LogHelper.Log(LogTarget.File, "File program imported(program Updated)" + DateTime.Now);
                                                // Fill SubProgramVM and Call DAL for insert into SubProgram
                                                SubProgramVM subProgram = new SubProgramVM();
                                                subProgram.Subprogram = topicmapping.SubProgram;
                                                subProgram.QASSubprogramid = topicmapping.SubProgramId;
                                                subProgram.ProgramId = programId;
                                                int subProgramId = ImportDAL.UpdateSubProgramData(subProgram);

                                                // LogHelper.Log(LogTarget.File, "File subProgram imported(subProgram Updated)" + DateTime.Now);
                                                // Fill SubSpecialityVM and Call DAL for insert into SubSpeciality
                                                SubSpecialityVM subSpeciality = new SubSpecialityVM();
                                                subSpeciality.Subspecialty = topicmapping.SubSpecialty;
                                                subSpeciality.QASSubspecialtyid = topicmapping.SubSpecialtyId;
                                                subSpeciality.SubprogramId = subProgramId;
                                                int subSpecialityId = ImportDAL.UpdateSubSpecialityData(subSpeciality);

                                                // LogHelper.Log(LogTarget.File, "File subSpeciality imported(subSpeciality Updated)" + DateTime.Now);
                                                // Fill SubspecialtyTopicXWalkVM and Call DAL for insert into SubspecialtyTopicXWalk
                                                SubspecialtyTopicXWalkVM subspecialtyTopicXWalk = new SubspecialtyTopicXWalkVM();
                                                subspecialtyTopicXWalk.SubSpecialityId = subSpecialityId;
                                                subspecialtyTopicXWalk.TopicId = topicId;
                                                int subspecialtyTopicXWalkId = ImportDAL.UpdateSubspecialtyTopicXWalkData(subspecialtyTopicXWalk);

                                                // UpdateBCSCMapperContentOnUpdate(questionId, beforeUpdateDb_TopicList, afterUpdateDb_TopicList);
                                                // LogHelper.Log(LogTarget.File, "File subspecialtyTopicXWalk imported(subspecialtyTopicXWalk Updated)" + DateTime.Now);
                                            }
                                        }

                                        List<int> afterUpdateDb_TopicList = ImportDAL.GetQASTopicIdByQuestionId(questionId);
                                        if (defaultexcerpt)
                                        {
                                            log.Information("UpdateBCSCMapperContentForOnce,QuestionId :" + questionId);
                                            UpdateBCSCMapperContentForOnce(questionId);
                                        }
                                        else
                                        {
                                            log.Information("Update BCSC Mapper Content,QuestionId :" + questionId);
                                            UpdateBCSCMapperContentOnUpdate(questionId, beforeUpdateDb_TopicList, afterUpdateDb_TopicList);
                                        }

                                        // get the question Image
                                        ImportDAL.DeleteReferenceData(questionId, tableName = "Question_Image");
                                        if (questionDetails.Image.Count > 0)
                                        {
                                            List<ImageVM> imageVMList = new List<ImageVM>();

                                            // ImportDAL.DeleteReferenceData(questionId, tableName = "Question_Image");
                                            foreach (Image questionImage in questionDetails.Image)
                                            {
                                                ImageVM imageVM = new ImageVM();
                                                imageVM.Caption = questionImage.Caption;
                                                imageVM.Comments = questionImage.Comments;
                                                imageVM.Credit = questionImage.Credit;
                                                imageVM.Figure = questionImage.Figure;
                                                imageVM.Path = Convert.ToString(HttpUtility.HtmlDecode(Convert.ToString(questionImage.Path)));
                                                imageVM.Source = questionImage.Source;
                                                imageVM.Title = questionImage.Title;
                                                imageVMList.Add(imageVM);

                                                // insert into QuestionImage and pass LIst And QuestionId get imageId
                                                int questionImageId = ImportDAL.InsertImageData(imageVM, questionId, tableName = "Question_Image");

                                                // LogHelper.Log(LogTarget.File, "File QuestionImage imported(QuestionImage Updated)" + DateTime.Now);
                                            }
                                            log.Information("Question Image List Count: " + imageVMList.Count);
                                        }

                                        DiscussionVM discussionVm = new DiscussionVM();

                                        // Discussion text insert
                                        discussionVm.DiscussionText = questionDetails.Discussion.Text;
                                        discussionVm.DiscussionType = false;
                                        discussionVm.QuestionId = questionId;

                                        // insert into Dissicion pass questionId and get dissicionid
                                        int discussionId = ImportDAL.UpdateDiscussionData(discussionVm);

                                        // LogHelper.Log(LogTarget.File, "File QuestionDetails imported(QuestionDetails Update)" + DateTime.Now);
                                        if (discussionId != 0)
                                        {
                                            ImportDAL.DeleteReferenceData(discussionId, tableName = "Discussion_Image");
                                        }

                                        if (questionDetails.Discussion.Image.Count > 0)
                                        {
                                            List<ImageVM> imageDiscussionVMList = new List<ImageVM>();

                                            // ImportDAL.DeleteReferenceData(discussionId, tableName = "Discussion_Image");
                                            foreach (Image imageDiscusstionListData in questionDetails.Discussion.Image)
                                            {
                                                ImageVM imageVM = new ImageVM();
                                                imageVM.Caption = imageDiscusstionListData.Caption;
                                                imageVM.Comments = imageDiscusstionListData.Comments;
                                                imageVM.Credit = imageDiscusstionListData.Credit;
                                                imageVM.Figure = imageDiscusstionListData.Figure;
                                                imageVM.Path = imageDiscusstionListData.Path;
                                                imageVM.Source = imageDiscusstionListData.Source;
                                                imageVM.Title = imageDiscusstionListData.Title;
                                                imageDiscussionVMList.Add(imageVM);

                                                // insert into DissicionImage pass DissiconId and Get dissicionImageId
                                                int discussionImageId = ImportDAL.InsertImageData(imageVM, discussionId, tableName = "Discussion_Image");

                                                // LogHelper.Log(LogTarget.File, "File QuestionImage imported(QuestionImage Updated)" + DateTime.Now);
                                            }
                                        }

                                        // Get the answers fromm JSON data
                                        if (questionDetails.Answer.Count > 0)
                                        {
                                            List<AnswerVM> answerVMList = new List<AnswerVM>();
                                            foreach (Answer answerVMListData in questionDetails.Answer)
                                            {
                                                AnswerVM answerVM = new AnswerVM();
                                                answerVM.Text = answerVMListData.Text;
                                                answerVM.Correct = answerVMListData.Correct;
                                                answerVM.Sequence = answerVMListData.Sequence;
                                                answerVM.Id = answerVMListData.Id;
                                                answerVM.QuestionId = questionId;
                                                answerVM.AnswerType = false;
                                                answerVMList.Add(answerVM);

                                                // Get the answer Image
                                                // insert into Choice Table pass QuestionId and choice object and get ChoiceId
                                                int answerId = ImportDAL.UpdateAnswerData(answerVM);
                                                if (answerId != 0)
                                                {
                                                    ImportDAL.DeleteReferenceData(answerId, tableName = "Answer_Choice_Image");
                                                }

                                                // LogHelper.Log(LogTarget.File, "File answer imported(answer Saved)" + DateTime.Now);
                                                if (answerVMListData.Image.Count > 0)
                                                {
                                                    List<ImageVM> imageAnswerVMList = new List<ImageVM>();

                                                    // ImportDAL.DeleteReferenceData(answerId, tableName = "Answer_Choice_Image");
                                                    foreach (Image answerImageListData in answerVMListData.Image)
                                                    {
                                                        ImageVM imageVM = new ImageVM();
                                                        imageVM.Caption = answerImageListData.Caption;
                                                        imageVM.Comments = answerImageListData.Comments;
                                                        imageVM.Credit = answerImageListData.Credit;
                                                        imageVM.Figure = answerImageListData.Figure;
                                                        imageVM.Path = answerImageListData.Path;
                                                        imageVM.Source = answerImageListData.Source;
                                                        imageVM.Title = answerImageListData.Title;
                                                        imageAnswerVMList.Add(imageVM);

                                                        // insert into Choice Image pass choiceId and ChoiceImageObject and Get ChoiceImageId
                                                        int answerImageId = ImportDAL.InsertImageData(imageVM, answerId, tableName = "Answer_Choice_Image");

                                                        // LogHelper.Log(LogTarget.File, "File answerImage imported(answerImage Updated)" + DateTime.Now);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ImportDAL.DeleteOldTopicData();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                Console.Write(e.InnerException);
                log.Information("Error Update QAS" + e.Message);
                log.Information("Error :" + e.InnerException);
            }

            return null;
        }

        private static async Task<Topic> GetTopic(string topicId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);
                    string topicUrl = apiUrl + "topic/gettopic?id=" + topicId;
                    HttpResponseMessage response = await client.GetAsync(topicUrl);
                    response.EnsureSuccessStatusCode();
                    if (response.IsSuccessStatusCode)
                    {
                        using (HttpContent content = response.Content)
                        {
                            string jsonString = await response.Content.ReadAsStringAsync();
                            var topic = Topic.FromJson(jsonString);
                            return topic;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                Console.Write(e.InnerException.ToString());
                return null;
            }
        }

        // }
        //    public enum LogTarget
        //    {
        //        File, Database, EventLog
        //    }
        //    public static class LogHelper
        //    {
        //        private static LogBase logger = null;
        //        public static void Log(LogTarget target, string message)
        //        {
        //            switch (target)
        //            {
        //                case LogTarget.File:
        //                    logger = new FileLogger();
        //                    logger.Log(message);
        //                    break;
        //                case LogTarget.Database:
        //                    logger = new DBLogger();
        //                    logger.Log(message);
        //                    break;
        //                case LogTarget.EventLog:
        //                    logger = new EventLogger();
        //                    logger.Log(message);
        //                    break;
        //                default:
        //                    return;
        //            }
        //        }
        //    }
        //    public abstract class LogBase
        //    {
        //        protected readonly object lockObj = new object();
        //        public abstract void Log(string message);
        //    }

        // public class FileLogger : LogBase
        //    {
        //        public string filePath = @"C:\Logs\IDGLog.txt";
        //        public override void Log(string message)
        //        {
        //            lock (lockObj)
        //            {
        //                using (StreamWriter streamWriter = new StreamWriter(filePath, append: true))
        //                {
        //                    streamWriter.WriteLine(message);
        //                    streamWriter.Close();
        //                }
        //            }
        //        }
        //    }

        // public class EventLogger : LogBase
        //    {
        //        public override void Log(string message)
        //        {
        //            lock (lockObj)
        //            {
        //                EventLog m_EventLog = new EventLog("");
        //                m_EventLog.Source = "IDGEventLog";
        //                m_EventLog.WriteEntry(message);
        //            }
        //        }
        //    }

        // public class DBLogger : LogBase
        //    {
        //        string connectionString = string.Empty;
        //        public override void Log(string message)
        //        {
        //            lock (lockObj)
        //            {
        //                //Code to log data to the database
        //            }
        //        }
        //  }
        public static int UpdateBCSCMapperContent()
        {
            List<int> questionList = new List<int>();
            questionList = ImportDAL.GetQuestionList();
            foreach (int item in questionList)
            {
                List<BCSCContentVM> bCSCContentData = new List<BCSCContentVM>();
                bCSCContentData = GetBCSCSectionChapterTopicDetails(item);

                foreach (BCSCContentVM bCSCContent in bCSCContentData)
                {
                    var bcscBookdata = GetBCSCContentFromBook(bCSCContent);
                    bCSCContent.Content = bcscBookdata.BCSCContent;
                    bCSCContent.BCSCbookWordCount = bcscBookdata.TotalBcscWord;
                    ImportDAL.InsertContentInBCSCMapperIfNotInDb(bCSCContent);
                }
            }

            return 1;
        }

        public static int UpdateBCSCMapperContentOnUpdate(int question, List<int> beforeUpdateDb_TopicList, List<int> afterUpdateDb_TopicList)
        {
            /*if (question == 1662 || question == 3018 || question == 4197)
            {*/
            var log = new Serilog.LoggerConfiguration().WriteTo.RollingFile(sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();

            List<int> topicNotInDb = afterUpdateDb_TopicList.Except(beforeUpdateDb_TopicList).ToList();
            List<int> topicNotInQAS = beforeUpdateDb_TopicList.Except(afterUpdateDb_TopicList).ToList();
            log.Information("topicNotInDb count :" + beforeUpdateDb_TopicList.Count);
            log.Information("topicNotInQAS count :" + beforeUpdateDb_TopicList.Count);
            try
            {
                if (topicNotInDb.Count > 0)
                {
                    foreach (int topicId in topicNotInDb)
                    {
                        ImportDAL.DeleteContentFromBCSCMapper(question, topicId);
                        List<BCSCContentVM> bCSCContentData = new List<BCSCContentVM>();
                        bCSCContentData = GetBCSCSectionChapterTopicDetails(question);

                        foreach (BCSCContentVM bCSCContent in bCSCContentData)
                        {
                            if (bCSCContent.TopicId == topicId)
                            {
                                // bCSCContent.Content = GetBCSCContentFromBook(bCSCContent);
                                // ImportDAL.InsertContentInBCSCMapper(bCSCContent);
                                var bcscBookdata = GetBCSCContentFromBook(bCSCContent);
                                log.Information("GetBCSCContentFromBook Topic ID:" + topicId);
                                bCSCContent.Content = bcscBookdata.BCSCContent;
                                bCSCContent.BCSCbookWordCount = bcscBookdata.TotalBcscWord;
                                log.Information("GetBCSCContentFromBook Word count :" + bcscBookdata.TotalBcscWord);

                                ImportDAL.InsertContentInBCSCMapper(bCSCContent);
                            }
                        }
                    }
                }

                if (topicNotInQAS.Count > 0)
                {
                    foreach (int topicId in topicNotInQAS)
                    {
                        ImportDAL.DeleteContentFromBCSCMapper(question, topicId);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Information("UpdateBCSCMapperContentOnUpdate Error" + ex.Message);
            }

            /*return 1;
            }*/

            return 1;
        }

        public static List<BCSCContentVM> GetBCSCSectionChapterTopicDetails(int questionId)
        {
            return ImportDAL.GetBCSCSectionChapterTopicDetails(questionId);
        }

        public static string GetFileText(string name)
        {
            string fileContents = string.Empty;

            // If the file has been deleted since we took
            // the snapshot, ignore it and return the empty string.
            if (System.IO.File.Exists(name))
            {
                fileContents = System.IO.File.ReadAllText(name);
                fileContents = HtmlRemoval.StripTagsRegex(fileContents);
            }

            return fileContents;
        }

        public static BCSCContentWordCount GetBCSCContentFromBook(BCSCContentVM bCSCContent)
        {
            var fileName = string.Empty;
            var rData = string.Empty;
            var log = new Serilog.LoggerConfiguration().WriteTo.RollingFile(sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();

            //var path = @"~\BCSC Book\" + bCSCContent.BCSCSectionNumber + string.Empty;
            //DirectoryInfo directory = new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath(path));
            DirectoryInfo directory = new DirectoryInfo(WebConfigurationManager.AppSettings["BCSCBooksDirectory"] + bCSCContent.BCSCSectionNumber + string.Empty);
            log.Information("Books Directory : " + directory);
            IEnumerable<System.IO.FileInfo> fileList = directory.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            string chapterTerm = bCSCContent.Chapter.ToString();
            string chapterTerm_WithoutTralingZero = bCSCContent.Chapter;
            string[] chapterwords = chapterTerm.Split(' ');
            string chapterName = chapterwords[0];
            string chapterId = chapterwords[1];
            log.Information("Chapter Name : " + chapterTerm);

            chapterTerm = chapterId.TrimStart('0');
            chapterTerm = chapterName + ' ' + chapterTerm;
            string topicname = (bCSCContent.TopicName.ToString()).Trim();

            if (!string.IsNullOrEmpty(topicname))
            {
                char ctoreplace = '<';
                int indexBegin = topicname[0] == ctoreplace ? 1 : 0;
                int indexEnd = topicname[topicname.Length - 1] == ctoreplace ? 1 : 0;
                topicname = topicname.Substring(indexBegin, topicname.Length - (indexEnd + indexBegin));
            }

            var queryMatchingFiles =
                from file in fileList
                where file.Extension == ".html" && !file.Name.Contains("C00FM")
                let fileText = GetFileText(file.FullName)
                where fileText.Contains(StripHTML(bCSCContent.ChapterName.Replace((char)0xA0, ' ').Trim())) && fileText.Contains(StripHTML(topicname.Trim().Replace((char)0xA0, ' ')))
                select file.Name;

            List<string> found = new List<string>();

            // string line;
            string bcscText = string.Empty;
            string bcscContent = string.Empty;
            string bcscContentTopicoutline = string.Empty;
            int bcscWord = 0;
            BCSCContentWordCount content = new BCSCContentWordCount();
            try
            {
                foreach (string filename in queryMatchingFiles)
                {
                    // using (StreamReader file = new StreamReader(directory + "\\" + filename))
                    // {
                    //    int i = 0;
                    //    while ((line = file.ReadLine()) != null)
                    //    {
                    //        line = HtmlRemoval.StripTagsRegex(line);
                    //         if (line.Contains(topicname) && (line != String.Empty))
                    //        {
                    //            var line1 = File.ReadLines(directory + "\\" + filename).Skip(i + 1).Take(10);
                    //            found.Add(string.Join("\n", chapterTerm + ":" + bCSCContent.ChapterName, (string.Join("\n", topicname, (string.Join("\n", line1.ToArray()))))));
                    //        }
                    //        i++;
                    //    }
                    //    file.Close();
                    // }
                    string filedata = string.Empty;

                    // bcscContent += "<br/><b>Topics: </b><br/><ul>";
                    // bcscContent += topicname;
                    // Read the topics
                    using (StreamReader fileTopic = new StreamReader(directory + "\\" + filename))
                    {
                        filedata = fileTopic.ReadToEnd();
                        fileTopic.Close();
                    }

                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(filedata);

                    var images = htmlDocument.DocumentNode.SelectNodes("//img");
                    if (images != null)
                    {
                        foreach (var node in images)
                        {
                            var src = node.Attributes[@"src"].Value;

                            if (src.StartsWith("BCSC"))
                            {
                                if (src.Contains("qrcode"))
                                {
                                    src = src.Replace("/", @"\");
                                    var qrresult = QrReader.ReadQrCode(WebConfigurationManager.AppSettings["BCSCBooksDirectory"] + bCSCContent.BCSCSectionNumber + @"\" + src);
                                    if (qrresult != string.Empty && qrresult != null)
                                    {
                                        HtmlNode h2Node = HtmlNode.CreateNode("<br />");
                                        node.AppendChild(h2Node);
                                        HtmlNode link = HtmlNode.CreateNode("<a href=" + qrresult + " target='_blank'>Video Link</a>");
                                        node.AppendChild(link);
                                    }

                                    src = src.Replace(@"\", "/");
                                }

                                node.SetAttributeValue("src", "/BCSC Book/" + bCSCContent.BCSCSectionNumber + "/" + src);
                            }
                        }
                    }

                    // Load the Chapter Name
                    List<HtmlNode> chaptertitle = htmlDocument.DocumentNode.Descendants().Where(x => (x.Name == "p" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("chapter-title"))).ToList();

                    // if(toftitle1)
                    foreach (var chaptertermdata in chaptertitle)
                    {
                        // if(HtmlRemoval.StripTagsRegex(chaptertermdata.InnerText)== ChapterTerm) {
                        // Console.WriteLine("Topics");
                        List<HtmlNode> toftitle = htmlDocument.DocumentNode.Descendants().Where(x => (x.Name == "p" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("h1") && !x.Attributes["class"].Value.Contains("h2-h1") && !x.Attributes["class"].Value.Contains("sidebar-h1"))).ToList();
                        var figureReferenceDivContent = string.Empty;
                        var tableReferenceDivContent = string.Empty;
                        if (HtmlRemoval.StripTagsRegex(chaptertermdata.InnerText.Replace((char)0xA0, ' ')) == bCSCContent.ChapterName.Replace((char)0xA0, ' ').Trim() && toftitle.Count > 0)
                        {
                            // New code of implemenation for topic selection for BCSC outline
                            bcscContentTopicoutline += "<br/>";
                            bcscContentTopicoutline += "<b>Section " + bCSCContent.BCSCSectionNumber + ": " + bCSCContent.BCSCSectionTitle + " </b>";
                            bcscContentTopicoutline += "<br/>";
                            bcscContentTopicoutline += "<b>" + chapterTerm + ": " + bCSCContent.ChapterName + " </b>";
                            bcscContentTopicoutline += "<br/><br/><ul class=\"bcsc-excerpt-custom-ul\">";

                            List<string> topicList = new List<string>();
                            List<string> sortedTopicList = new List<string>();
                            foreach (var topicListInsert in toftitle)
                            {
                                topicList.Add(HtmlRemoval.StripTagsRegex(topicListInsert.InnerHtml.ToString().Trim()));
                            }

                            string currentString = HtmlRemoval.StripTagsRegex(topicname.Trim());
                            int topicCurrentindex = topicList.IndexOf(currentString);
                            Console.WriteLine(topicCurrentindex);

                            int prev = topicCurrentindex > 0 ? topicCurrentindex - 1 : -1;
                            int prev1 = topicCurrentindex > 1 ? topicCurrentindex - 2 : -2;

                            int next = topicCurrentindex > 0 ? topicCurrentindex + 1 : 1;
                            int next1 = topicCurrentindex > 0 ? topicCurrentindex + 2 : 2;

                            int prevItem, prevOneItem;

                            prevItem = prev != -1 ? topicList.IndexOf(currentString) : 0;
                            prevOneItem = prev1 != -2 ? topicList.IndexOf(currentString) : 0;
                            string previousStringone = string.Empty;
                            string previousStringtwo = string.Empty;

                            string nextStringone = string.Empty;
                            string nextStringtwo = string.Empty;
                            string currentselected = string.Empty;

                            if (prevItem > 0)
                            {
                                previousStringone = topicList[prevItem - 1];
                            }

                            if (prevOneItem > 0)
                            {
                                previousStringtwo = topicList[prevItem - 2];
                            }

                            if (topicList.ElementAtOrDefault(next) != null)
                            {
                                nextStringone = (topicList[next]);
                            }

                            if (topicList.ElementAtOrDefault(next1) != null)
                            {
                                nextStringtwo = (topicList[next1]);
                            }

                            if (topicCurrentindex >= 0)
                            {
                                currentselected = topicList[topicCurrentindex];
                            }

                            if (!string.IsNullOrEmpty(previousStringtwo))
                            {
                                sortedTopicList.Add(previousStringtwo);
                            }

                            if (!string.IsNullOrEmpty(previousStringone))
                            {
                                sortedTopicList.Add(previousStringone);
                            }

                            if (!string.IsNullOrEmpty(currentselected))
                            {
                                sortedTopicList.Add(currentselected);
                            }

                            if (!string.IsNullOrEmpty(nextStringone))
                            {
                                sortedTopicList.Add(nextStringone);
                            }

                            if (!string.IsNullOrEmpty(nextStringtwo))
                            {
                                sortedTopicList.Add(nextStringtwo.ToString());
                            }

                            int k1 = 0;

                            // sortedTopicList.RemoveAt(TopicCurrentindex);
                            // sortedTopicList[TopicCurrentindex] = "<li>" + topicname.ToString() + "<li>";
                            foreach (string topicName1 in sortedTopicList)
                            {
                                if (topicName1 == HtmlRemoval.StripTagsRegex(topicname.Trim()))
                                {
                                    // Console.WriteLine("\n List << " + k1 + ">>: <li> " + topicName1.ToString() + "</li>");
                                    bcscContentTopicoutline += "<li><span class=\"bcsc-excerpt-custom-content\" > " + topicName1.ToString() + "</span></li>";
                                }
                                else
                                {
                                    bcscContentTopicoutline += "<li>" + topicName1.ToString() + "</li>";
                                }

                                k1++;
                            }

                            bcscContentTopicoutline += "</ul>";

                            Console.WriteLine("\n");

                            // End of New code of implemenation for topic selection outline
                            int k = 1;
                            bcscContent += "<br/>";
                            bcscContent += "<b>Section " + bCSCContent.BCSCSectionNumber + ": " + bCSCContent.BCSCSectionTitle + " </b>";
                            bcscContent += "<br/>";
                            bcscContent += "<b>" + chapterTerm + ": " + bCSCContent.ChapterName + " </b>";
                            bcscContent += "<br/><br/><ul class=\"bcsc-excerpt-custom-ul\">";

                            foreach (var itemn in toftitle)
                            {
                                if (HtmlRemoval.StripTagsRegex(itemn.InnerHtml.Trim()) == HtmlRemoval.StripTagsRegex(topicname.Trim()))
                                {
                                    // Console.WriteLine(k + ": " + "<br /><span class=\"bcsc-excerpt-custom-content\">" + itemn.InnerHtml + "</span><br />" + "(Text Found)");
                                    bcscContent += "<li><span class=\"bcsc-excerpt-custom-content\" > " + itemn.InnerHtml + "</span></li>";

                                    var node = htmlDocument.DocumentNode.SelectSingleNode(itemn.XPath);
                                    HtmlNode sibling = node.NextSibling;
                                    while (sibling != null)
                                    {
                                        if (sibling.NodeType == HtmlNodeType.Element && sibling.OuterHtml.Contains("body-text--no-indent-"))
                                        {
                                            try
                                            {
                                                bcscText = sibling.InnerHtml;
                                                string myString = HtmlRemoval.StripTagsRegex(bcscText);

                                                // Count the number of words
                                                int count = 0;
                                                string str = myString;
                                                while (count <= str.Length - 1)
                                                {
                                                    if (str[count] == ' ' || str[count] == '\n' || str[count] == '\t')
                                                    {
                                                        bcscWord++;
                                                    }

                                                    count++;
                                                }





                                                List<HtmlNode> figureReferenceSpan = sibling.Descendants().Where(x => (x.Name == "span" && x.Attributes["class"] != null && x.Attributes["id"] != null && x.Attributes["class"].Value.Contains("xref-figure"))).DistinctBy(x => x.Attributes["id"].Value.ToString()).ToList();
                                                figureReferenceDivContent = string.Empty;

                                                if (figureReferenceSpan.Count > 0)
                                                {
                                                    foreach (var span in figureReferenceSpan)
                                                    {
                                                        List<HtmlNode> figureReferenceDiv = htmlDocument.DocumentNode.Descendants().Where(x => (x.Name == "div" && x.Attributes["id"] != null && x.Attributes["id"].Value.Equals(span.Attributes["id"].Value.ToString()))).DistinctBy(x => x.Attributes["id"].Value.ToString()).ToList();
                                                        if (figureReferenceDiv.Count > 0)
                                                        {
                                                            for (int i = 0; i < figureReferenceDiv.Count; i++)
                                                            {
                                                                figureReferenceDivContent += figureReferenceDiv[i].InnerHtml.ToString() + "<br />";
                                                            }
                                                        }
                                                    }
                                                }


                                                List<HtmlNode> tableReferenceSpan = sibling.Descendants().Where(x => (x.Name == "span" && x.Attributes["class"] != null && x.Attributes["id"] != null && x.Attributes["class"].Value.Contains("xref-table"))).DistinctBy(x => x.Attributes["id"].Value.ToString()).ToList();
                                                tableReferenceDivContent = string.Empty;

                                                if (tableReferenceSpan.Count > 0)
                                                {
                                                    foreach (var span in tableReferenceSpan)
                                                    {
                                                        List<HtmlNode> tableReferenceDiv = htmlDocument.DocumentNode.Descendants().Where(x => (x.Name == "div" && x.Attributes["id"] != null && x.Attributes["id"].Value.Equals(span.Attributes["id"].Value.ToString()))).DistinctBy(x => x.Attributes["id"].Value.ToString()).ToList();
                                                        if (tableReferenceDiv.Count > 0)
                                                        {
                                                            for (int i = 0; i < tableReferenceDiv.Count; i++)
                                                            {
                                                                tableReferenceDivContent += tableReferenceDiv[i].InnerHtml.ToString() + "<br />";
                                                            }
                                                        }
                                                    }
                                                }

                                                // myString = myString.Substring(0, 500);
                                                // myString = myString.Substring(0, Math.Min(myString.Length, 500));
                                                // int index = myString.LastIndexOf(' ');
                                                // bcscText = myString.Substring(0, index) + "...";
                                                string imageToBeReadsection = WebConfigurationManager.AppSettings["ImagesToBeShown"];
                                                List<int> imageToBeReadsectionList = imageToBeReadsection.Split(',').Select(int.Parse).ToList();
                                                if (imageToBeReadsectionList.Exists(x => x.Equals(bCSCContent.BCSCSectionNumber)))
                                                {
                                                    bcscText = bcscText;
                                                }
                                                else
                                                {
                                                    bcscText = Regex.Replace(bcscText, @"<img\s[^\>]*src\s*=\s*[""']([^/<]*/)*([^""']+)[^\>]*\s*/*>", string.Empty);
                                                }

                                                // bcscText = bcscText;

                                                // Console.WriteLine("Content:" + sibling.OuterHtml);
                                                break;
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine(e.InnerException);
                                                log.Information("GetBCSCContentFromBook Error " + e.Message);
                                            }

                                            break;
                                        }

                                        sibling = sibling.NextSibling;
                                    }
                                }
                                else
                                {
                                    // Console.WriteLine(k + ": " + "<br />" + itemn.InnerHtml);
                                    bcscContent += "<li>" + " " + itemn.InnerHtml + "</li>";
                                }

                                k++;
                            }

                            bcscContent += "</ul>";
                            if (!string.IsNullOrWhiteSpace(bcscContentTopicoutline))
                            {
                                // bcscContentTopicoutline += "<br />" + HtmlRemoval.StripTagsRegex(bcscText);
                                bcscContentTopicoutline += "<br />" + bcscText;
                                bcscText = string.Empty;
                            }

                            bcscContentTopicoutline += figureReferenceDivContent;
                            bcscContentTopicoutline += tableReferenceDivContent;
                            bcscContentTopicoutline += "<hr/>";
                        }
                        else
                        {
                            // bcscText = string.Empty;
                            // bcscContent = string.Empty;
                        }
                    }

                    // End of main loop
                }

                // foreach (var itembcsccontent in found)
                // {

                // bcscContent = HtmlRemoval.StripTagsRegex(itembcsccontent);
                // }
                // Keep the console window open in debug mode.

            }
            catch (Exception ex)
            {
                log.Information("GetBCSCContentFromBook Error :" + ex.Message);
            }
            content.BCSCContent = bcscContentTopicoutline;
            content.TotalBcscWord = bcscWord;
            return content;
        }

        public static string StripHTML(string input)
        {
            return Regex.Replace(input, "<.*?>", string.Empty);
        }

        public static int UpdateBCSCMapperContentField()
        {
            List<int> questionList = new List<int>();
            questionList = ImportDAL.GetQuestionList();
            foreach (int item in questionList)
            {
                List<BCSCContentVM> bCSCContentData = new List<BCSCContentVM>();
                bCSCContentData = GetBCSCSectionChapterTopicDetails(item);

                foreach (BCSCContentVM bCSCContent in bCSCContentData)
                {
                    // bCSCContent.Content = GetBCSCContentFromBook(bCSCContent);
                    // ImportDAL.UpdateBCSCMapperContentField(bCSCContent);
                    var bcscBookdata = GetBCSCContentFromBook(bCSCContent);
                    bCSCContent.Content = bcscBookdata.BCSCContent;
                    bCSCContent.BCSCbookWordCount = bcscBookdata.TotalBcscWord;
                    ImportDAL.UpdateBCSCMapperContentField(bCSCContent);
                }
            }

            return 1;
        }

        public static int UpdateBCSCMapperContentOnBookChange()
        {
            string section = WebConfigurationManager.AppSettings["Section"];
            List<int> sectionList = section.Split(',').Select(int.Parse).ToList();

            List<int> questionList = new List<int>();
            questionList = ImportDAL.GetQuestionList();
            foreach (int item in questionList)
            {
                List<BCSCContentVM> bCSCContentData = new List<BCSCContentVM>();
                bCSCContentData = GetBCSCSectionChapterTopicDetails(item);

                foreach (BCSCContentVM bCSCContent in bCSCContentData)
                {
                    if (sectionList.Exists(p => p.Equals(bCSCContent.BCSCSectionNumber)))
                    {
                        //ChapterAndTopicMatch() this function is used for comparing DB & Html chap name & topic name
                        bool chapterandtopicmatch = ChapterAndTopicMatch(bCSCContent);
                        if (chapterandtopicmatch)
                        {
                            var bcscBookdata = GetBCSCContentFromBook(bCSCContent);
                            bCSCContent.Content = bcscBookdata.BCSCContent;
                            bCSCContent.BCSCbookWordCount = bcscBookdata.TotalBcscWord;
                            ImportDAL.UpdateBCSCMapperContentOnBookChange(bCSCContent);
                        }
                        else
                        {
                            var log = new Serilog.LoggerConfiguration().WriteTo.RollingFile(sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();
                            log.Information("Chapter Name or Topic Name Not Found in HTML file: " + "Section No: " + bCSCContent.BCSCSectionNumber + " ; " + bCSCContent.ChapterName + " ; " + bCSCContent.TopicName);
                        }

                        // chpater and topic searchcheck needs to be add
                        // if topic and chapter matches 
                        // bCSCContent.Content = GetBCSCContentFromBook(bCSCContent);
                        // ImportDAL.UpdateBCSCMapperContentOnBookChange(bCSCContent);

                        /*var bcscBookdata = GetBCSCContentFromBook(bCSCContent);
                        bCSCContent.Content = bcscBookdata.BCSCContent;
                        bCSCContent.BCSCbookWordCount = bcscBookdata.TotalBcscWord;
                        ImportDAL.UpdateBCSCMapperContentOnBookChange(bCSCContent);*/
                    }
                }
            }
            return 1;
        }

        //Method added for chapter and topic match
        public static bool ChapterAndTopicMatch(BCSCContentVM bCSCContent)
        {
            //bCSCContent this data is comming from DataBase
            var valid = false;

            //Checking null condition for Chapt name & Topic Name of DB
            if (!string.IsNullOrEmpty(bCSCContent.ChapterName) && !string.IsNullOrEmpty(bCSCContent.TopicName))
            {
                var log = new Serilog.LoggerConfiguration().WriteTo.RollingFile(sAttrLogPath, shared: true, retainedFileCountLimit: 7).CreateLogger();

                //Reading data from HTML file
                DirectoryInfo directory = new DirectoryInfo(WebConfigurationManager.AppSettings["BCSCBooksDirectory"] + bCSCContent.BCSCSectionNumber + string.Empty);
                log.Information("Books Directory : " + directory);
                IEnumerable<System.IO.FileInfo> fileList = directory.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

                string topicname = (bCSCContent.TopicName.ToString()).Trim();

                var queryMatchingFiles =
                from file in fileList
                where file.Extension == ".html" && !file.Name.Contains("C00FM")
                let fileText = GetFileText(file.FullName)
                where fileText.Contains(StripHTML(bCSCContent.ChapterName.Replace((char)0xA0, ' ').Trim())) && fileText.Contains(StripHTML(topicname.Trim().Replace((char)0xA0, ' ')))
                select file.Name;

                try
                {
                    foreach (string filename in queryMatchingFiles)
                    {
                        string filedata = string.Empty;
                        using (StreamReader fileTopic = new StreamReader(directory + "\\" + filename))
                        {
                            filedata = fileTopic.ReadToEnd();
                            fileTopic.Close();
                        }

                        HtmlDocument htmlDocument = new HtmlDocument();
                        htmlDocument.LoadHtml(filedata);

                        // Load the Chapter Name
                        List<HtmlNode> chaptertitle = htmlDocument.DocumentNode.Descendants().Where(x => (x.Name == "p" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("chapter-title"))).ToList();

                        foreach (var chaptertermdata in chaptertitle)
                        {
                            // Load the Topic Name from HTML
                            List<HtmlNode> toftitle = htmlDocument.DocumentNode.Descendants().Where(x => (x.Name == "p" && x.Attributes["class"] != null && x.Attributes["class"].Value.Contains("h1") && !x.Attributes["class"].Value.Contains("h2-h1") && !x.Attributes["class"].Value.Contains("sidebar-h1"))).ToList();

                            //Chapter matching and topic > 0
                            if (HtmlRemoval.StripTagsRegex(chaptertermdata.InnerText.Replace((char)0xA0, ' ')) == bCSCContent.ChapterName.Replace((char)0xA0, ' ').Trim() && toftitle.Count > 0)
                            {
                                //topic list from HTML
                                foreach (var itemn in toftitle)
                                {
                                    //Topic name matching, comparing HTML & DB topic name
                                    if (HtmlRemoval.StripTagsRegex(itemn.InnerHtml.Trim()) == HtmlRemoval.StripTagsRegex(topicname.Trim()))
                                    {
                                        valid = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Information("ChapterAndTopicMatch() : " + ex.Message);
                    return valid;
                }
            }
            return valid;
        }

        public static int UpdateBCSCMapperContentForOnce(int questionId)
        {
            List<BCSCContentVM> bCSCContentData = new List<BCSCContentVM>();
            bCSCContentData = GetBCSCSectionChapterTopicDetails(questionId);
            ImportDAL.DeleteContentFromBCSCMapperOnRetired(questionId);
            foreach (BCSCContentVM bCSCContent in bCSCContentData)
            {
                var bcscBookdata = GetBCSCContentFromBook(bCSCContent);
                bCSCContent.Content = bcscBookdata.BCSCContent;
                bCSCContent.BCSCbookWordCount = bcscBookdata.TotalBcscWord;
                ImportDAL.InsertContentInBCSCMapper(bCSCContent);
            }

            return 1;
        }
    }
}
