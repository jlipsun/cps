USE [cs496database]
GO
/****** Object:  StoredProcedure [dbo].[viewRequest]    Script Date: 05/04/2013 13:18:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		QTC
-- Create date: 
-- Description:	
-- =============================================
ALTER PROCEDURE [dbo].[viewRequest] 
	-- Add the parameters for the stored procedure here
	@id int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT xml_data FROM qtc WHERE id = @id;
END
