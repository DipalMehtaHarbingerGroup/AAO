GO
ALTER PROCEDURE [dbo].[AAO_CreateExam] @ExamName         VARCHAR(200), 
                                       @ExamCreateDate   DATETIME, 
                                       @ExamType         INT, 
                                       @ExamMode         BIT, 
                                       @UserId           INT, 
                                       @NoofQuestions    INT, 
                                       @ExamTimeType     BIT, 
                                       @IsDeleted        BIT, 
                                       @ExamAnswerToShow BIT, 
                                       @TypeofQuestion   VARCHAR(200), 
                                       @TypeofCategory   VARCHAR(200), 
                                       @id               INT output 
AS 
    /* **********************************************************************  
    Author            : HARBINGER  
    Creation Date    : 05/09/2018  
    Desc              : This store procedure will be use to Create and save the exam of the user.
    Table Use         : exam   
    Modify Date       :  
    *************************************************************************      
    */ 
    SET nocount ON 

  BEGIN 
   DECLARE @count INT 

      SET @count = (SELECT Count(*) 
                    FROM   exam 
                    WHERE  userid = @UserId 
                           AND ExamName = @ExamName 
                           ) 
 IF( @count < =0 ) 
        BEGIN 
      INSERT INTO exam 
                  (examname, 
                   examcreatedate, 
                   examtype, 
                   exammode, 
                   userid, 
                   noofquestions, 
                   examtimetype, 
                   isdeleted, 
                   examanswertoshow, 
                   typeofquestion, 
                   typeofcategory) 
      VALUES     (@ExamName, 
                  @ExamCreateDate, 
                  @ExamType, 
                  @ExamMode, 
                  @UserId, 
                  @NoofQuestions, 
                  @ExamTimeType, 
                  @IsDeleted, 
                  @ExamAnswerToShow, 
                  @TypeofQuestion, 
                  @TypeofCategory) 

      SET @id=Scope_identity() 

      RETURN @id 
	  End
	  else
	  begin
	  SET @id=0; 

      RETURN @id 
	  end
  END 
