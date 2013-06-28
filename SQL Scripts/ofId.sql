USE [cs496database]
GO
/****** Object:  StoredProcedure [dbo].[ofId]    Script Date: 04/30/2013 17:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		qtc
-- Create date: 
-- Description:	return on id(for VIEW button)
-- =============================================
ALTER PROCEDURE [dbo].[ofId] 
	-- Add the parameters for the stored procedure here
	@id int = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * from qtc where id = @id;
END

