GO
ALTER TABLE [dbo].[ExamAttemptAsnwer] DROP CONSTRAINT [ExamAttemptAsnwer_IsYourAnswer]
GO

ALTER TABLE [dbo].[ExamAttemptAsnwer]   ALTER COLUMN IsYourAnswer INT

ALTER TABLE [dbo].[ExamAttemptAsnwer] ADD  CONSTRAINT [ExamAttemptAsnwer_IsYourAnswer]  DEFAULT ((0)) FOR [IsYourAnswer]
GO

 GO
ALTER TABLE [dbo].[AAOUser] ALTER COLUMN ResidencyStart datetime null
GO

GO
ALTER TABLE [dbo].[AAOUser] ALTER COLUMN ResidencyEnd datetime null
GO



INSERT INTO [dbo].[BCSCQASMapping] ([QASSubspecialty],[BCSCSectionNumber],[BCSCSectionTitle]) VALUES('Cataract',11,'Lens and Cataract')
INSERT INTO [dbo].[BCSCQASMapping] ([QASSubspecialty],[BCSCSectionNumber],[BCSCSectionTitle]) VALUES('Cornea',8,'External Disease and Cornea')
INSERT INTO [dbo].[BCSCQASMapping] ([QASSubspecialty],[BCSCSectionNumber],[BCSCSectionTitle]) VALUES('Fundamentals',2,'Fundamentals and Principles of Ophthalmology')
INSERT INTO [dbo].[BCSCQASMapping] ([QASSubspecialty],[BCSCSectionNumber],[BCSCSectionTitle]) VALUES('General Medicine',1,'Update on General Medicine')
INSERT INTO [dbo].[BCSCQASMapping] ([QASSubspecialty],[BCSCSectionNumber],[BCSCSectionTitle]) VALUES('Glaucoma',10,'Glaucoma')
INSERT INTO [dbo].[BCSCQASMapping] ([QASSubspecialty],[BCSCSectionNumber],[BCSCSectionTitle]) VALUES('Neuro-Ophthalmology',5,'Neuro-Ophthalmology')
INSERT INTO [dbo].[BCSCQASMapping] ([QASSubspecialty],[BCSCSectionNumber],[BCSCSectionTitle]) VALUES('Oculoplastics',7,'Orbit, Eyelids, and Lacrimal System')
INSERT INTO [dbo].[BCSCQASMapping] ([QASSubspecialty],[BCSCSectionNumber],[BCSCSectionTitle]) VALUES('Optics',3,'Clinical Optics')
INSERT INTO [dbo].[BCSCQASMapping] ([QASSubspecialty],[BCSCSectionNumber],[BCSCSectionTitle]) VALUES('Pathology/Oncology',4,'Ophthalmic Pathology and Intraocular Tumors')
INSERT INTO [dbo].[BCSCQASMapping] ([QASSubspecialty],[BCSCSectionNumber],[BCSCSectionTitle]) VALUES('Pediatric',6,'Pediatric Ophthalmology and Strabismus')
INSERT INTO [dbo].[BCSCQASMapping] ([QASSubspecialty],[BCSCSectionNumber],[BCSCSectionTitle]) VALUES('Refractive',13,'Refractive Surgery')
INSERT INTO [dbo].[BCSCQASMapping] ([QASSubspecialty],[BCSCSectionNumber],[BCSCSectionTitle]) VALUES('Retina',12,'Retina and Vitreous')
INSERT INTO [dbo].[BCSCQASMapping] ([QASSubspecialty],[BCSCSectionNumber],[BCSCSectionTitle]) VALUES('Uveitis',9,'Intraocular Inflammation and Uveitis')


--- SP changes

USE [Aao_Bcsc_app]
GO
/****** Object:  StoredProcedure [dbo].[AAO_GetQuestionTypeCountOnCategory]    Script Date: 22-05-2018 11:41:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[AAO_GetQuestionTypeCountOnCategory] @UserId INT 
AS 
    /* **********************************************************************  
    Author            : HARBINGER  
    Creation Date    : 05/09/2018  
    Desc              : This store procedure will be use to return question type count on catefory.
    Table Use         : QuestionTopicXwalk,SubspecialtyTopicXWalk  ,Subspecialty,MarkQuestion,Question,SubspecialtyTopicXWalk,IncorrectQuestion
    Modify Date       :  
    *************************************************************************      
    */ 
    SET nocount ON 

  BEGIN 
      SELECT B.BCSCSectionTitle  AS  Subspecialty,            
             A.subspecialtyid 
             AS 
             SubspecialtyId, 
             (SELECT Count(DISTINCT question.questionid) 
              FROM   question question 
                     INNER JOIN questiontopicxwalk questionTopicXWalk 
                             ON question.questionid = 
                                questiontopicxwalk.questionid 
                     INNER JOIN subspecialtytopicxwalk subspecialtyTopicXWalk 
                             ON questiontopicxwalk.topicid = 
                                subspecialtytopicxwalk.topicid 
                     INNER JOIN subspecialty subSpeciality 
                             ON subspecialtytopicxwalk.subspecialtyid = 
                                subSpeciality.subspecialtyid 
                     INNER JOIN markquestion markQuestion 
                             ON markquestion.questionid = question.questionid 
                                AND markquestion.userid = @UserId 
              WHERE  subSpeciality.subspecialtyid IN ( A.subspecialtyid ) 
                     AND question.status = 7 
                     AND question.isactive = 1 
                     AND markquestion.isdeleted = 0) 
             AS 
             MarkQuestionCounts, 
             (SELECT Count(DISTINCT question.questionid) 
              FROM   question question 
                     INNER JOIN questiontopicxwalk questionTopicXWalk 
                             ON question.questionid = 
                                questiontopicxwalk.questionid 
                     INNER JOIN subspecialtytopicxwalk subspecialtyTopicXWalk 
                             ON questiontopicxwalk.topicid = 
                                subspecialtytopicxwalk.topicid 
                     INNER JOIN subspecialty subSpeciality 
                             ON subspecialtytopicxwalk.subspecialtyid = 
                                subSpeciality.subspecialtyid 
                     INNER JOIN incorrectquestion incorrectQuestion 
                             ON incorrectquestion.questionid = 
                                question.questionid 
                                AND incorrectquestion.questionid = 
                                    question.questionid 
                                AND incorrectquestion.userid = @UserId 
              WHERE  subSpeciality.subspecialtyid IN ( A.subspecialtyid ) 
                     AND question.status = 7 
                     AND question.isactive = 1 
                     AND incorrectquestion.isdeleted = 0) 
             AS 
             IncorrectAsnwerCounts, 
             (SELECT Count(DISTINCT QU.questionid) QuestionIdCount 
              FROM   question QU 
                     JOIN questiontopicxwalk QTX 
                       ON QU.questionid = QTX.questionid 
                     JOIN subspecialtytopicxwalk SPTX 
                       ON QTX.topicid = SPTX.topicid 
                     JOIN subspecialty SP 
                       ON SP.subspecialtyid = sptx.subspecialtyid 
              WHERE  QU.status = 7 
                     AND QU.isactive = 1 
                     AND QU.isdeleted = 0 
                     AND QU.questionid NOT IN (SELECT questionid 
                                               FROM   examattemptasnwer EAA 
                                               WHERE  userid = @UserId) 
                     AND SP.subspecialtyid IN ( A.subspecialtyid )) 
             AS 
             ExamSkipQuestionCounts, 
             (SELECT Count(DISTINCT question.questionid) 
              FROM   question question 
                     INNER JOIN questiontopicxwalk questionTopicXWalk 
                             ON question.questionid = 
                                questiontopicxwalk.questionid 
                     INNER JOIN subspecialtytopicxwalk subspecialtyTopicXWalk 
                             ON questiontopicxwalk.topicid = 
                                subspecialtytopicxwalk.topicid 
                     INNER JOIN subspecialty subSpeciality 
                             ON subspecialtytopicxwalk.subspecialtyid = 
                                subSpeciality.subspecialtyid 
              WHERE  question.status = 7 
                     AND question.isactive = 1 
                     AND subSpeciality.subspecialtyid IN ( A.subspecialtyid )) 
             AS 
             TotalCount 
      FROM   subspecialty A join BCSCQASMapping B on A.Subspecialty=B.QASSubspecialty and  A.isactive = 1  AND A.isactive = 1 order by B.BCSCSectionNumber asc

  /* 
  Declare @TotalCount int 
  Declare @MarkQuestionCount int 
  Declare @IncorrectAsnwerCount int 
  Declare @ExamSkipQuestionCount int 
   
   select @TotalCount=count(distinct question.QuestionId) from Question question 
                                          inner join QuestionTopicXwalk questionTopicXWalk on question.QuestionId = questionTopicXWalk.QuestionId
                                          inner join SubspecialtyTopicXWalk subspecialtyTopicXWalk on questionTopicXWalk.TopicId = subspecialtyTopicXWalk.TopicId
                                          inner join Subspecialty subSpeciality on subspecialtyTopicXWalk.SubspecialtyId = subSpeciality.SubspecialtyId 
                                          where  question.Status = 7 and question.IsActive=1 
   
   select @ExamSkipQuestionCount=Count(distinct question.QuestionId)  from Question question  
                                          inner join QuestionTopicXwalk questionTopicXWalk on question.QuestionId = questionTopicXWalk.QuestionId 
                                          inner join SubspecialtyTopicXWalk subspecialtyTopicXWalk on questionTopicXWalk.TopicId = subspecialtyTopicXWalk.TopicId 
                                          inner join Subspecialty subSpeciality on subspecialtyTopicXWalk.SubspecialtyId = subSpeciality.SubspecialtyId
                                          inner join ExamSkipQuestion examSkipQuestion  on examSkipQuestion.QuestionId = question.QuestionId 
                                          AND examSkipQuestion.UserId = @UserId where subSpeciality.SubspecialtyId in (@SubSpecialityId) and question.Status=7 and question.IsActive=1  and examSkipQuestion.IsDeleted=0
   
   select @IncorrectAsnwerCount=count(distinct question.QuestionId) from Question question  
                                      inner join QuestionTopicXwalk questionTopicXWalk on question.QuestionId = questionTopicXWalk.QuestionId 
                                      inner join SubspecialtyTopicXWalk subspecialtyTopicXWalk on questionTopicXWalk.TopicId = subspecialtyTopicXWalk.TopicId 
                                      inner join Subspecialty subSpeciality on subspecialtyTopicXWalk.SubspecialtyId = subSpeciality.SubspecialtyId
                                      inner join ExamAttemptAsnwer examAttemptAsnwer on examAttemptAsnwer.QuestionId= question.QuestionId
                                      AND examAttemptAsnwer.QuestionId = question.QuestionId and examAttemptAsnwer.IsRight = 0 
                                      and examAttemptAsnwer.UserId = @UserId where subSpeciality.SubspecialtyId in (@SubSpecialityId) and question.Status=7 and question.IsActive=1 and examAttemptAsnwer.IsDeleted=0
   
  select @MarkQuestionCount=count(distinct question.QuestionId) from Question question  
                                   inner join QuestionTopicXwalk questionTopicXWalk on question.QuestionId = questionTopicXWalk.QuestionId 
                                   inner join SubspecialtyTopicXWalk subspecialtyTopicXWalk on questionTopicXWalk.TopicId = subspecialtyTopicXWalk.TopicId 
                                   inner join Subspecialty subSpeciality on subspecialtyTopicXWalk.SubspecialtyId = subSpeciality.SubspecialtyId
                                   inner join MarkQuestion markQuestion on markQuestion.QuestionId=question.QuestionId 
                                   and markQuestion.UserId= @UserId where subSpeciality.SubspecialtyId in (@SubSpecialityId) and question.Status=7 and question.IsActive=1 and markQuestion.IsDeleted=0
   
   create table #question_Count (TotalCounts int, MarkQuestionCounts int, IncorrectAsnwerCounts int,ExamSkipQuestionCounts int)  
   
   insert into #question_Count(TotalCounts,MarkQuestionCounts,IncorrectAsnwerCounts,ExamSkipQuestionCounts)values(@TotalCount,@MarkQuestionCount,@IncorrectAsnwerCount,@ExamSkipQuestionCount)
    
    select * from #question_Count 
  */ 
  END 





