USE [cs496database]
GO
/****** Object:  StoredProcedure [dbo].[ofStatus]    Script Date: 04/30/2013 17:46:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		qtc
-- Create date: 
-- Description:	return requests of stats=@status(for displaying data on each tab)
-- =============================================
ALTER PROCEDURE [dbo].[ofStatus] 
	-- Add the parameters for the stored procedure here
	@status varchar(20) = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    IF @status = 'incoming' or @status = 'triage' or @status = 'followup'
		SELECT * from qtc where status = @status;
	ELSE
		PRINT 'INVALID Status'
END

