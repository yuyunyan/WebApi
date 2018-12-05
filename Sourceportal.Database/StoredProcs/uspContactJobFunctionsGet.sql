/* =============================================
   Author:		Berry Zhong
   Create date: 2017.11.30
   Description:	Return list of Contact job functions
   Usage:	EXEC [uspContactJobFunctionsGet
   Revision History:
		2018.10.10	NA	Added ExternalID
   ============================================= */
   
CREATE OR ALTER PROCEDURE [dbo].[uspContactJobFunctionsGet]
	@IsDeleted BIT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		JobFunctionID,
		JobFunctionName,
		ExternalID
	From ContactJobFunctions
	WHERE IsDeleted = @IsDeleted
END
GO
