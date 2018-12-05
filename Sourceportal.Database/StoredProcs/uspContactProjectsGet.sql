-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2017.12.01
-- Description:			Return list of projects for a contact
-- =============================================
CREATE PROCEDURE [dbo].[uspContactProjectsGet]
	@ContactID INT = NULL,
	@AccountID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

   SELECT
		P.ProjectID,
		P.[Name],
		AP.AccountID,
		CP.ContactID,
		(CASE WHEN ISNULL(CP.ContactID, 0) != @ContactID THEN 'True' ELSE 'False' END) 'IsOption'
	FROM Projects P
		INNER JOIN mapAccountProjects AP ON AP.ProjectID = P.ProjectID
		LEFT OUTER JOIN (SELECT C.AccountID, mcp.ContactID, mcp.ProjectID, mcp.IsDeleted FROM mapContactProjects mcp LEFT JOIN Contacts C ON C.ContactID = mcp.ContactID) CP ON CP.ProjectID = AP.ProjectID AND CP.IsDeleted = 0 AND AP.AccountID = CP.AccountID
	WHERE AP.AccountID = @AccountID
END
GO
