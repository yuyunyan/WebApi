/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.08.22
   Description:	Creates an Inspection off of a new ItemStock
   Usage: EXEC uspQCInspectionCreate @StockID = 0000
   Revision History:
	   2018.04.23	ML	Added @ExternalID and @InspectionQty and removed selecting InspectionQty from ItemInventory table
	   2018.05.10	CT	Added @InspectionStatusID
	   2018.06.22	NA	Converted to ItemStock schema
	   2018.10.16	NA	Added InspectionTypeExternalID
	   2018.10.22	NA	Added Company Type to checklist finding logic
	   2018.10.29	NA	Added InspectionType to matching logic

   Return Codes:
			-1 StockID is required
			-2 Given StockID already has an Inspection
			-3 No default status is configured for QC Inspections
   ============================================= */
   

CREATE OR ALTER PROCEDURE [dbo].[uspQCInspectionCreate]
	@StockID INT = 0,
	@UserID INT = 0,
	@ExternalID VARCHAR(50),
	@InspectionQty INT = 0,
	@InspectionStatusID INT = NULL,
	@InspectionTypeExternalID VARCHAR(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF ISNULL(@StockID, 0) = 0
		RETURN -1

	--Ensure the StockID doesn't already have an Inspection set up for it
	IF EXISTS (SELECT 1 FROM QCInspections WHERE StockID = @StockID AND IsDeleted = 0)
		RETURN -2
	
	--Get a default status ID for new QC Inspections
	DECLARE @StatusID INT = 0
	SET @StatusID = (SELECT TOP 1 InspectionStatusID FROM lkpQCInspectionStatuses WHERE IsDefault = 1 AND IsDeleted = 0)
	IF ISNULL(@StatusID, 0) = 0
		RETURN -3

	--Find the Inspection Type by ExternalID
	DECLARE @InspectionTypeID INT = 0
	SET @InspectionTypeID = (SELECT InspectionTypeID FROM lkpQCInspectionTypes WHERE ExternalID = @InspectionTypeExternalID)

	--Find which Checklists fit the matching criteria for the given StockID
	DECLARE @Checklists TABLE (ChecklistID INT)
	INSERT INTO @Checklists	
		SELECT j.ChecklistID
		FROM mapQCChecklistJoins j
		  INNER JOIN vwStockWithFulfillment swf ON j.ObjectID = swf.CustomerAccountID AND j.ObjectTypeID = 1 --Customer Account
		  INNER JOIN QCChecklists c ON j.ChecklistID = c.ChecklistID AND c.ChecklistTypeID = @InspectionTypeID
		WHERE swf.StockID = @StockID AND j.IsDeleted = 0 AND c.IsDeleted = 0
		UNION
		SELECT j.ChecklistID
		FROM mapQCChecklistJoins j
		  INNER JOIN vwStockWithFulfillment swf ON j.ObjectID = swf.VendorAccountID AND j.ObjectTypeID = 1 --Vendor Account
		  INNER JOIN QCChecklists c ON j.ChecklistID = c.ChecklistID AND c.ChecklistTypeID = @InspectionTypeID
		WHERE swf.StockID = @StockID AND j.IsDeleted = 0 AND c.IsDeleted = 0
		UNION
		SELECT j.ChecklistID
		FROM mapQCChecklistJoins j
		  INNER JOIN Accounts a ON j.ObjectID = a.CompanyTypeID AND j.ObjectTypeID = 110 --Vendor Company Type
		  INNER JOIN vwStockWithFulfillment swf ON a.AccountID = swf.VendorAccountID
		  INNER JOIN QCChecklists c ON j.ChecklistID = c.ChecklistID AND c.ChecklistTypeID = @InspectionTypeID
		WHERE swf.StockID = @StockID AND j.IsDeleted = 0 AND c.IsDeleted = 0
		UNION
		SELECT j.ChecklistID
		FROM mapQCChecklistJoins j
		  INNER JOIN vwStockWithFulfillment swf ON j.ObjectID = swf.CommodityID AND j.ObjectTypeID = 101 --Commodity
		  INNER JOIN QCChecklists c ON j.ChecklistID = c.ChecklistID AND c.ChecklistTypeID = @InspectionTypeID
		WHERE swf.StockID = @StockID AND j.IsDeleted = 0 AND c.IsDeleted = 0
		UNION
		SELECT j.ChecklistID
		FROM mapQCChecklistJoins j
		  INNER JOIN vwStockWithFulfillment swf ON j.ObjectID = swf.ItemID AND j.ObjectTypeID = 103 --Item
		  INNER JOIN QCChecklists c ON j.ChecklistID = c.ChecklistID AND c.ChecklistTypeID = @InspectionTypeID
		WHERE swf.StockID = @StockID AND j.IsDeleted = 0 AND c.IsDeleted = 0
		UNION
		SELECT j.ChecklistID 
		FROM mapQCChecklistJoins j
		  INNER JOIN QCChecklists c ON j.ChecklistID = c.ChecklistID AND c.ChecklistTypeID = @InspectionTypeID
		WHERE j.ObjectID = 0 AND j.ObjectTypeID = 0  --Always
	
	--Include the children of the matched Checklists.  Does not handle more than 1 level of parentage.
	INSERT INTO @Checklists
	SELECT c.ChecklistID
	FROM QCChecklists c
	  INNER JOIN @Checklists vc ON c.ParentChecklistID = vc.ChecklistID AND c.IsDeleted = 0

	--Create an Inspection
	DECLARE @InspectionID INT
	
	INSERT INTO QCInspections (StockID, InspectionStatusID, InspectionTypeID, InspectionQty, CreatedBy, ExternalID)
	VALUES (@StockID, ISNULL(@InspectionStatusID, @StatusID), @InspectionTypeID, @InspectionQty, @UserID, @ExternalID)

	SET @InspectionID = SCOPE_IDENTITY()

	--Create the Answers for the Inspection
	INSERT INTO QCAnswers (InspectionID, QuestionID, QuestionVersionID, CreatedBy)
	SELECT @InspectionID, QuestionID, VersionID, @UserID
	FROM vwQCQuestions q
	  INNER JOIN @Checklists vc ON q.ChecklistID = vc.ChecklistID
	  INNER JOIN QCChecklists c ON vc.ChecklistID = c.ChecklistID
	ORDER BY c.SortOrder, q.SortOrder

	--Join the inspection to the stock that spawned it
	INSERT INTO mapQCInspectionStock (InspectionID, StockID, CreatedBy)
	VALUES (@InspectionID, @StockID, @UserID)

	SELECT @InspectionID 'InspectionID'
END
