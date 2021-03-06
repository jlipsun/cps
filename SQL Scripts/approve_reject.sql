USE [cs496database]
GO
/****** Object:  StoredProcedure [dbo].[approve_reject]    Script Date: 04/30/2013 17:45:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		qtc
-- Create date: 
-- Description:	change status(for clicking approve/reject on right column of incomming request tab)
-- =============================================
ALTER PROCEDURE [dbo].[approve_reject] 
	-- Add the parameters for the stored procedure here
	@id int = 0, 
	@triage_followup VarChar(50) = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	PRINT 'set status to: ' + @triage_followup 
	IF @triage_followup = 'triage' 
	BEGIN
		UPDATE qtc
		SET status=@triage_followup
		WHERE id = @id	
	END
	ELSE IF @triage_followup = 'followup'
	BEGIN	
		UPDATE qtc
		SET status=@triage_followup
		WHERE id = @id
	END
	ELSE
	BEGIN
		PRINT 'INVALID status'
	END

END

