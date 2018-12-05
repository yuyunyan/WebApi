/* =============================================
   Author:		Julia Thomas
   Create date: 2017.08.22
   Description:	Inserts or updates QCChecklists
   Usage:	EXEC uspQCChecklistSet @ChecklistID=13,@ParentChecklistID=8,@ChecklistTypeID=2,@ChecklistName='CNN',
   @ChecklistDescription='CNN Description',@SortOrder=8,@EffectiveStartDate='2017-08-22',@UserID=3
			
   Return Codes:
			-1 UserID is required
			-2 Error in checklist insert
			-3 Error in checklist update
			
   Revision History:
			
   ============================================= */
CREATE PROCEDURE [dbo].[uspQCChecklistSet]
	@ChecklistID INT = NULL OUTPUT,
	@ParentChecklistID INT = NULL OUTPUT,
	@ChecklistTypeID INT = NULL,
	@ChecklistName NVARCHAR(50) = NULL,
	@ChecklistDescription NVARCHAR(500) = NULL,
	@SortOrder INT= NULL,
	@EffectiveStartDate DATE = NULL,
	@IsDeleted BIT = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF @UserID IS NULL
		RETURN -1
	IF ISNULL(@ChecklistID, 0) = 0
		GOTO InsertCheckList
	ELSE
		GOTO UpdateCheckList

InsertCheckList:
	
	INSERT INTO QCChecklists (ParentChecklistID, ChecklistTypeID, ChecklistName, ChecklistDescription, SortOrder,EffectiveStartDate, CreatedBy)
	VALUES (		
		NULLIf(@ParentChecklistID,0), -- if it is 0, convert it to Null 
		@ChecklistTypeID,
		@ChecklistName,
		@ChecklistDescription,
		@SortOrder,
		@EffectiveStartDate,
		@UserID
	)

	SET @ChecklistID = SCOPE_IDENTITY()

	IF ISNULL(@ChecklistID, 0) = 0
		RETURN -2

	GOTO ReturnSelect

UpdateCheckList:
	
	UPDATE QCChecklists
	SET
		ParentChecklistID = @ParentChecklistID,
		ChecklistTypeID = @ChecklistTypeID,
		ChecklistName = @ChecklistName,
		ChecklistDescription = @ChecklistDescription,
		SortOrder = @SortOrder,
		EffectiveStartDate = @EffectiveStartDate,
		IsDeleted = ISNULL(@IsDeleted, IsDeleted),
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE ChecklistID = @ChecklistID

	IF (@@ROWCOUNT=0)
		RETURN -3

	GOTO ReturnSelect

ReturnSelect:
	SELECT @ChecklistID 'ChecklistID'
END