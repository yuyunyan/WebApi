-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2017.12.01
-- Description:			Return insert or delete line in mapContactProject
-- =============================================
CREATE PROCEDURE [dbo].[uspContactProjectsSet]
	@ContactID INT = NULL,
	@ProjectID INT = NULL,
	@IsDeleted BIT = 0
AS
BEGIN
	SET NOCOUNT ON;

	DELETE mapContactProjects
	WHERE ContactID = @ContactID 
	AND ProjectID = @ProjectID
	
	INSERT INTO mapContactProjects (ContactID, ProjectID, IsDeleted)
	VALUES (@ContactID, @ProjectID, @IsDeleted)

	SELECT @@ROWCOUNT 'RowCount'

END
GO
