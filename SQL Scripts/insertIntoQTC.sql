USE [cs496database]
GO
/****** Object:  StoredProcedure [dbo].[insertIntoQTC]    Script Date: 05/06/2013 22:54:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		qtc
-- Create date: 
-- Description:	
-- =============================================
ALTER PROCEDURE [dbo].[insertIntoQTC] 
	-- Add the parameters for the stored procedure here
	@DocumentID varchar(20) = null,
	@ClaimNumber varchar(20) = null,
	@ContractNumber varchar(20) = null,
	@DateOfExamRequest varchar(20) = null,
	@RequestType varchar(20) = null,
	@LOB  varchar(20) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    INSERT into qtc values(@DocumentID, @ClaimNumber, @ContractNumber, @DateOfExamRequest, @RequestType, null, null, null, null, null, @LOB);
END
