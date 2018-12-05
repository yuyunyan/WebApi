-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.01.24
-- Description:			Insert or Update AccountGroup
-- Usage:				EXEC uspAccountGroupSet @AccountGroupID = NULL, @GroupName = 'Empty Group', @UserID = 68, @GroupLinesData = '[]'
--						EXEC uspAccountGroupSet @AccountGroupID = NULL, @GroupName = 'One Account Group', @UserID = 68, @GroupLinesData = '[{"AccountID": 26, "ContactID": 65}]'
-- =============================================
CREATE PROCEDURE [dbo].[uspAccountGroupSet]
	@AccountGroupID INT = NULL,
	@GroupName VARCHAR(255) = '',
	@UserID INT = 0,
	@IsDeleted BIT = 0,
	@GroupLinesData VARCHAR(MAX) = ''
AS
BEGIN
	SET NOCOUNT ON;
	IF ISNULL(@AccountGroupID, 0) = 0 
		GOTO INSERT_NEW_ACCOUNT_GROUP
	ELSE
		GOTO UPDATE_ACCOUNT_GROUP

INSERT_NEW_ACCOUNT_GROUP:
	INSERT INTO 
		UserAccountGroups (UserID, GroupName, Created, CreatedBy) 
	VALUES(@UserID, @GroupName, getdate(), @UserID)
	SET @AccountGroupID = @@IDENTITY

	INSERT INTO mapUserAccountGroupAccounts 
		(AccountGroupID, AccountID, ContactID, Created, CreatedBy, IsDeleted) 
	SELECT
		@AccountGroupID,
		GL.accountId,
		GL.contactId,
		getdate(),
		@UserID,
		0
	FROM
	OPENJSON(@GroupLinesData)
		WITH (
		 groupLineId INT,
		 accountId INT,
		 contactId INT,
		 isDeleted BIT
	) GL

	GOTO RETURN_SELECT 

UPDATE_ACCOUNT_GROUP:
	UPDATE UserAccountGroups
	SET GroupName = @GroupName, IsDeleted = @IsDeleted
	WHERE AccountGroupID = @AccountGroupID

	INSERT INTO mapUserAccountGroupAccounts
		(AccountGroupID, AccountID, ContactID, Created, CreatedBy, IsDeleted) 
	SELECT
		@AccountGroupID,
		GL.accountId,
		GL.contactId,
		getdate(),
		@UserID,
		GL.isDeleted
	FROM
	OPENJSON(@GroupLinesData)
		WITH (
		 groupLineId INT,
		 accountId INT,
		 contactId INT,
		 isDeleted BIT
	) GL
	WHERE ISNULL(GL.groupLineId, 0) = 0

	UPDATE mapUserAccountGroupAccounts
	SET 
		AccountID = GL.accountId,
		IsDeleted = GL.isDeleted,
		ContactID = GL.contactId
	FROM OPENJSON(@GroupLinesData)
		WITH (
		 groupLineId INT,
		 accountId INT,
		 contactId INT,
		 isDeleted BIT
	) GL
	WHERE mapUserAccountGroupAccounts.GroupLineID = GL.groupLineId

	GOTO RETURN_SELECT
	
RETURN_SELECT:
	SELECT @AccountGroupID 'AccountGroupID'
END
GO
