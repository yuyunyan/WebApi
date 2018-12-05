/*	=============================================
	Author:			Nathan Ayers
	Create date:	2018.09.11
	Description:	Insert a record, or update by approving or deleting.
	Usage:			EXEC uspStateEngineApprovalsSet @RuleConditionID = 1, @ApprovalValue = 'Test', @UserID = 0
	Return Codes:
			-1	Either ApprovalID must be NULL to create a new record, or the existing record must be getting Approved (@ApprovedBy) or Deleted (@IsDeleted)
	Revision History:

	============================================*/

CREATE OR ALTER PROCEDURE [dbo].[uspStateEngineApprovalsSet]
	@ApprovalID INT = NULL,
	@RuleConditionID INT = NULL,
	@ApprovalObjectID INT = NULL,
	@ApprovedBy INT = NULL,
	@ApprovalValue VARCHAR(250) = NULL,
	@ApprovedDate DATETIME = NULL,
	@IsDeleted BIT = 0,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@ApprovalID, 0) = 0 
		GOTO InsertApproval
	IF @ApprovedBy IS NOT NULL
		GOTO UpdateApproval
	IF @IsDeleted = 1
		GOTO DeleteApproval
	ELSE
		RETURN -1

InsertApproval:
	INSERT INTO StateEngineApprovals (RuleConditionID, ApprovalObjectID, ApprovalValue, CreatedBy, Created, IsDeleted)
	VALUES (@RuleConditionID, @ApprovalObjectID, @ApprovalValue, @UserID, GETUTCDATE(), 0)

	SET @ApprovalID = SCOPE_IDENTITY()

	GOTO ReturnSelect

UpdateApproval:
	UPDATE StateEngineApprovals
	SET	ApprovedBy = @ApprovedBy,		
		ApprovedDate = GETUTCDATE()
	WHERE ApprovalID = @ApprovalID

	GOTO ReturnSelect

DeleteApproval:
	UPDATE StateEngineApprovals
	SET	IsDeleted = 1,
		DeletedDate = GETUTCDATE()
	WHERE ApprovalID = @ApprovalID

	GOTO ReturnSelect

ReturnSelect:
	SELECT @ApprovalID 'ApprovalID'
END