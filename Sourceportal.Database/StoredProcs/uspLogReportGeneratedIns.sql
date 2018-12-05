/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.11.02
   Description:	Insert srecord into logReportsGenerated
   Usage:		EXEC uspLogReportGeneratedIns @ReportTitle = 'Proforma Invoice', @UserID = 1
	
   Revision History:
		2018.11.02	AR	Intitial Deployment
   Return Codes:
	
   ============================================= */

CREATE PROCEDURE [dbo].[uspLogReportGeneratedIns]
	@ReportTitle VARCHAR(256) = NULL,
	@UserID INT = NULL
AS
BEGIN
	DECLARE @out TABLE (ID INT)

	INSERT INTO logReportsGenerated(ReportTitle, UserID)
	OUTPUT INSERTED.LogID INTO @out(ID)
	VALUES (@ReportTitle, @UserID)

	RETURN ( SELECT
			TOP 1 ID
			FROM @out )
END