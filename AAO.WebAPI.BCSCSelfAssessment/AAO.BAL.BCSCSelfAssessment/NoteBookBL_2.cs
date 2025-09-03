namespace AAO.BAL.BCSCSelfAssessment
{
    using System;
    using System.Collections.Generic;
    using Common.BCSCSelfAssessment;
    using DAL.BCSCSelfAssessment;

    public static class NoteBookBL
    {
        public static SectionsList GetNoteBookDetails(NoteBookRequest noteBookRequest)
        {
            SectionsList sectionList = new SectionsList();
            List<NoteSections> notesSectionList = new List<NoteSections>();
            List<ChapterDetails> chapterList = new List<ChapterDetails>();
            List<ChapterDetails> chapterList_custom = new List<ChapterDetails>();
            List<QuestionDetail> questionDetailList = new List<QuestionDetail>();

            if ((noteBookRequest.SubSpecialtyId == 0 || noteBookRequest.SubSpecialtyId == null) && noteBookRequest.ChapterName == null)
            {
                notesSectionList = NoteBookDAL.GetNotesSection();
                if (notesSectionList.Count > 0)
                {
                    List<NoteSections> notesSectionLists = new List<NoteSections>();
                    foreach (NoteSections section in notesSectionList)
                    {
                        NoteSections noteSections = new NoteSections();
                        noteSections.SubSpecialtyId = section.SubSpecialtyId;
                        noteSections.SubSpecialty = section.SubSpecialty;
                        noteSections.BCSCSectionNumber = section.BCSCSectionNumber;
                        noteSections.BCSCSectionTitle = section.BCSCSectionTitle;

                        chapterList = NoteBookDAL.GetChapter(section.SubSpecialtyId, noteBookRequest);

                        List<ChapterDetails> chapterLists = new List<ChapterDetails>();
                        foreach (ChapterDetails chapter in chapterList)
                        {
                            ChapterDetails chapterDetails = new ChapterDetails();
                            chapterDetails.Chapter = chapter.Chapter;
                            chapterDetails.ChapterName = chapter.ChapterName;
                            chapterDetails.ChapterCount = chapter.ChapterCount;
                            chapterDetails.ChapterSerialNumber = chapter.ChapterSerialNumber;
                            questionDetailList = NoteBookDAL.GetNotesQuestion(section.SubSpecialtyId, chapter.ChapterName, noteBookRequest.UserId);
                            List<QuestionDetail> questionDetailLists = new List<QuestionDetail>();
                            foreach (QuestionDetail questionDetails in questionDetailList)
                            {
                                QuestionDetail questionDetail = new QuestionDetail();
                                questionDetail.QuestionId = questionDetails.QuestionId;
                                questionDetail.Stem = questionDetails.Stem;
                                questionDetail.NotesList = NoteBookDAL.GetNotesList(questionDetails.QuestionId, noteBookRequest.UserId);
                                questionDetail.ChoiceList = NoteBookDAL.GetChoiceList(questionDetails.QuestionId);
                                questionDetail.RightChoiceId = AssessmentDAL.GetRightChoiceByQuestionId(Convert.ToInt32(questionDetails.QuestionId));
                                questionDetail.UserChoiceId = questionDetails.UserChoiceId;
                                questionDetail.TopicName = questionDetails.TopicName;
                                questionDetail.Topic = questionDetails.Topic;
                                questionDetailLists.Add(questionDetail);
                            }

                            chapterDetails.QuestionList = questionDetailLists;

                            if (chapterDetails.QuestionList.Count > 0)
                            {
                                chapterLists.Add(chapterDetails);
                            }
                        }

                        noteSections.ChapterList = chapterLists;

                        notesSectionLists.Add(noteSections);
                    }

                    sectionList.NoteSectionsLists = notesSectionLists;
                }
            }
            else if (noteBookRequest.SubSpecialtyId != null && noteBookRequest.ChapterName == null)
            {
                NoteSections noteSections = new NoteSections();
                noteSections = NoteBookDAL.GetSectionOnSubspecilty(Convert.ToInt32(noteBookRequest.SubSpecialtyId));
                List<NoteSections> notesSectionLists = new List<NoteSections>();

                chapterList = NoteBookDAL.GetChapter(noteBookRequest.SubSpecialtyId, noteBookRequest);

                chapterList_custom = NoteBookDAL.GetChapter_withoutPagination(noteBookRequest.SubSpecialtyId, noteBookRequest);

                List<ChapterDetails> chapterLists = new List<ChapterDetails>();
                if (chapterList.Count > 0)
                {
                    foreach (ChapterDetails chapter in chapterList)
                    {
                        ChapterDetails chapterDetails = new ChapterDetails();
                        chapterDetails.Chapter = chapter.Chapter;
                        chapterDetails.ChapterName = chapter.ChapterName;
                        chapterDetails.ChapterCount = chapter.ChapterCount;
                        chapterDetails.ChapterSerialNumber = chapter.ChapterSerialNumber;
                        questionDetailList = NoteBookDAL.GetNotesQuestion(noteBookRequest.SubSpecialtyId, chapter.ChapterName, noteBookRequest.UserId);
                        List<QuestionDetail> questionDetailLists = new List<QuestionDetail>();
                        foreach (QuestionDetail questionDetails in questionDetailList)
                        {
                            QuestionDetail questionDetail = new QuestionDetail();
                            questionDetail.QuestionId = questionDetails.QuestionId;
                            questionDetail.Stem = questionDetails.Stem;
                            questionDetail.NotesList = NoteBookDAL.GetNotesList(questionDetails.QuestionId, noteBookRequest.UserId);
                            questionDetail.ChoiceList = NoteBookDAL.GetChoiceList(questionDetails.QuestionId);
                            questionDetail.RightChoiceId = AssessmentDAL.GetRightChoiceByQuestionId(Convert.ToInt32(questionDetails.QuestionId));
                            questionDetail.UserChoiceId = questionDetails.UserChoiceId;
                            questionDetail.TopicName = questionDetails.TopicName;
                            questionDetail.Topic = questionDetails.Topic;
                            questionDetailLists.Add(questionDetail);
                        }

                        chapterDetails.QuestionList = questionDetailLists;

                        if (chapterDetails.QuestionList.Count > 0)
                        {
                            chapterLists.Add(chapterDetails);
                        }
                    }
                    noteSections.ChapterList = chapterLists;
                    notesSectionLists.Add(noteSections);
                    sectionList.NoteSectionsLists = notesSectionLists;
                }
                else
                {
                    ChapterDetails chapterDetails = new ChapterDetails();

                    foreach (ChapterDetails chapter in chapterList_custom)
                    {
                        chapterDetails.ChapterCount = chapter.ChapterCount;
                        chapterDetails.Chapter = null;
                        chapterDetails.ChapterName = null;
                        chapterDetails.ChapterSerialNumber = 0;
                        chapterDetails.QuestionList = new List<QuestionDetail>();
                    }
                    chapterLists.Add(chapterDetails);
                    noteSections.ChapterList = chapterLists;
                    notesSectionLists.Add(noteSections);
                    sectionList.NoteSectionsLists = notesSectionLists;
                }


            }
            else if (noteBookRequest.SubSpecialtyId != null && noteBookRequest.ChapterName != null)
            {
                NoteSections noteSections = new NoteSections();
                noteSections = NoteBookDAL.GetSectionOnSubspecilty(Convert.ToInt32(noteBookRequest.SubSpecialtyId));
                List<ChapterDetails> chapterLists = new List<ChapterDetails>();
                List<NoteSections> notesSectionLists = new List<NoteSections>();

                ChapterDetails chapterDetails = new ChapterDetails();
                chapterDetails = NoteBookDAL.GetChapterByName(noteBookRequest);
                chapterDetails.ChapterCount = 1;
                chapterDetails.ChapterSerialNumber = 1;

                questionDetailList = NoteBookDAL.GetNotesQuestion(noteBookRequest.SubSpecialtyId, noteBookRequest.ChapterName, noteBookRequest.UserId);
                List<QuestionDetail> questionDetailLists = new List<QuestionDetail>();
                foreach (QuestionDetail questionDetails in questionDetailList)
                {
                    QuestionDetail questionDetail = new QuestionDetail();
                    questionDetail.QuestionId = questionDetails.QuestionId;
                    questionDetail.Stem = questionDetails.Stem;
                    questionDetail.NotesList = NoteBookDAL.GetNotesList(questionDetails.QuestionId, noteBookRequest.UserId);
                    questionDetail.ChoiceList = NoteBookDAL.GetChoiceList(questionDetails.QuestionId);
                    questionDetail.RightChoiceId = AssessmentDAL.GetRightChoiceByQuestionId(Convert.ToInt32(questionDetails.QuestionId));
                    questionDetail.QuestionImageList = AssessmentDAL.GetQuestionImageList(questionDetails.QuestionId);
                    questionDetail.UserChoiceId = questionDetails.UserChoiceId;
                    questionDetail.TopicName = questionDetails.TopicName;
                    questionDetail.Topic = questionDetails.Topic;
                    questionDetailLists.Add(questionDetail);
                }

                chapterDetails.QuestionList = questionDetailLists;
                if (chapterDetails.QuestionList.Count > 0)
                {
                    chapterLists.Add(chapterDetails);
                }
                else
                {
                    ChapterDetails chapterDetails1 = new ChapterDetails();
                    chapterLists.Add(chapterDetails1);
                }

                noteSections.ChapterList = chapterLists;
                notesSectionLists.Add(noteSections);
                sectionList.NoteSectionsLists = notesSectionLists;
            }

            return sectionList;
        }

        public static List<NoteSections> GetSectionList(NoteBookRequest noteBookRequest)
        {
            return NoteBookDAL.GetNotesSection();
        }

        public static List<ChapterDetails> GetChapterList(NoteBookRequest noteBookRequest)
        {
            return NoteBookDAL.GetChapterList(noteBookRequest.SubSpecialtyId);
        }

        public static List<Notes> AddNotes(NotesRequest notesRequest)
        {
            return NoteBookDAL.AddNotes(notesRequest);
        }

        public static List<Notes> UpdateNotes(NotesRequest notesRequest)
        {
            return NoteBookDAL.UpdateNotes(notesRequest);
        }

        public static List<Notes> DeleteNotes(NotesRequest notesRequest)
        {
            return NoteBookDAL.DeleteNotes(notesRequest);
        }
    }
}
