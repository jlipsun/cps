USE [cs496database]
GO
/****** Object:  StoredProcedure [dbo].[insertMetadata]    Script Date: 05/06/2013 22:55:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		insertMetadata
-- Create date: 
-- Description:	
-- =============================================
ALTER PROCEDURE [dbo].[insertMetadata]
	-- Add the parameters for the stored procedure here
	@status varchar(20), 
	@author varchar(20),
	@time_created varchar(20), 
	@xml_string text
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    if (@status = 'incoming')
    begin
		UPDATE qtc SET xml_data = @xml_string WHERE status IS NULL;		
	end
	else if (@status = 'followup')
	begin	
		UPDATE qtc SET xml_data_malformed = @xml_string WHERE status IS NULL;
	end
	
	UPDATE qtc SET status = @status, created_by = @author, created_date = @time_created WHERE status IS NULL;
END

