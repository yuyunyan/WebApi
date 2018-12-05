/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.06.06
   Description:	Inserts or updates record in Accounts tbl
   Usage: EXEC uspAccountSet
		  SELECT * FROM Accounts
   Revision History:
		2017.06.08	AR	Added support for AccountNum column
		2017.06.27  NA  Replaced Accounts.Name with Accounts.AccountName and updated CurrencyID to be CHAR(3)
		2017.10.06 ML Added externalID
		2017.10.19 ML Remove ParentID
		2017.10.27 ML Return externalID
		2017.12.06	BZ	Added AccountHierarchyID
		2018.01.10	RV	Added mapOwnership for account
		2018.01.16	CT	Added mapAccountTypes for account
		2018.02.06 RV Added QCNotes and PONotes for account basic details
		2018.02.20	BZ	Added IsDeleted = 0 for new inserted mapAccountTypes
		2018.02.23	CT	Added ApprovedVendor field
		2018.04.02	CT  Update Row if External ID is matching
		2018.05.09	BZ	Added ApprovedVendor to return
		2018.08.06	CT	Added IncotermID
   Return Codes:
		-1	Insert Failed
		-2	Update Failed, check AccountID

   ============================================= */
CREATE PROCEDURE [dbo].[uspAccountSet]
(
	@AccountID INT = NULL OUTPUT
	, @AccountTypeBitwise INT = NULL
	, @CompanyTypeID INT = NULL
	, @OrganizationID INT = NULL
	, @AccountNum VARCHAR(128) = NULL
	, @CurrencyID CHAR(3) = NULL
	, @AccountName VARCHAR(128)
	, @CreatedBy INT = NULL
	, @IsDeleted BIT = 0
	, @ExternalID VARCHAR(50)
	, @AccountHierarchyID INT = NULL
	, @Email VARCHAR(250) = NULL
	, @Website VARCHAR(MAX) = NULL
	, @YearEstablished date = NULL
	, @NumOfEmployees INT = NULL
	, @EndProductFocus VARCHAR(250) = NULL
	, @CarryStock BIT = NULL
	, @MinimumPO Money = NULL
	, @ShippingInstructions VARCHAR(MAX) = NULL
	, @VendorNum VARCHAR(50) = NULL
	, @SupplierRating VARCHAR(50) = NULL
	, @QCNotes VARCHAR(MAX) = NULL
	, @PONotes VARCHAR(MAX) = NULL
	, @ApprovedVendor BIT = 0
	, @IncotermID INT = NULL

)
AS
BEGIN

	SET NOCOUNT ON;
	IF (ISNULL(@AccountID, 0) = 0)
		GOTO InsertAccount
	ELSE
		GOTO UpdateAccount

InsertAccount:
	INSERT INTO Accounts (CompanyTypeID, OrganizationID, AccountNum, 
		CurrencyID, AccountName, CreatedBy, ExternalID, AccountHierarchyID, Email, Website, YearEstablished,
		NumOfEmployees, EndProductFocus, CarryStock, MinimumPO, ShippingInstructions, VendorNum, SupplierRating, QCNotes, PONotes, ApprovedVendor, IncotermID)
	VALUES ( @CompanyTypeID
		, @OrganizationID
		, @AccountNum
		, @CurrencyID
		, @AccountName
		, @CreatedBy
		, @ExternalID 
		, @AccountHierarchyID
		, @Email
		, @Website
		, @YearEstablished
		, @NumOfEmployees
		, @EndProductFocus
		, @CarryStock
		, @MinimumPO
		, @ShippingInstructions
		, @VendorNum
		, @SupplierRating
		, @QCNotes
		, @PONotes
		, @ApprovedVendor
		, @IncotermID)
	SET @AccountID = @@IDENTITY

	IF (ISNULL(@AccountID,0) = 0)
		RETURN -1

		-- Set Ownership for the newly created Account
	INSERT INTO mapOwnership (OwnerID, ObjectTypeID, ObjectID, IsGroup, [Percent], CreatedBy, IsDeleted)
	VALUES (@CreatedBy, 1, @AccountID , 0 , 100 , @CreatedBy, 0) 
	GOTO InsertMapAccountTypes

UpdateAccount:
	UPDATE Accounts
	SET CompanyTypeID = ISNULL(@CompanyTypeID, CompanyTypeID)
		, OrganizationID = ISNULL(@OrganizationID, OrganizationID)
		, AccountNum = ISNULL(@AccountNum, AccountNum)
		, CurrencyID = ISNULL(@CurrencyID, CurrencyID)
		, AccountName = ISNULL(@AccountName, AccountName)
		, ModifiedBy = ISNULL(@CreatedBy, CreatedBy)
		, IsDeleted = ISNULL(@IsDeleted, IsDeleted)
		, ExternalID = ISNULL(@ExternalID, ExternalID )
		, AccountHierarchyID = ISNULL(@AccountHierarchyID, AccountHierarchyID)
		, Email = ISNULL(@Email, Email)
		, Website = ISNULL(@Website, Website)
		, YearEstablished = @YearEstablished
		, NumOfEmployees = ISNULL(@NumOfEmployees, NumOfEmployees)
		, EndProductFocus = ISNULL(@EndProductFocus, EndProductFocus)
		, CarryStock = ISNULL(@CarryStock, CarryStock)
		, MinimumPO = ISNULL(@MinimumPO, MinimumPO)
		, ShippingInstructions = ISNULL(@ShippingInstructions, ShippingInstructions)
		, VendorNum = ISNULL(@VendorNum, VendorNum)
		, SupplierRating = ISNULL(@SupplierRating, SupplierRating)
		, QCNotes = ISNULL(@QCNotes, QCNotes)
		, PONotes = ISNULL(@PONotes, PONotes)
		, ApprovedVendor =ISNULL(@ApprovedVendor, ApprovedVendor)
		, IncotermID = ISNULL(@IncotermID, IncotermID)
	WHERE AccountID = @AccountID OR ExternalID = @ExternalID --null = null ? equals false

		IF (@@rowcount = 0)
			RETURN -2
		GOTO UpdateMapAccountTypes

InsertMapAccountTypes:	
	IF((@AccountTypeBitwise & 1) = 1) --customer type
		INSERT INTO mapAccountTypes (AccountID, AccountTypeID, AccountStatusID, CreatedBy, IsDeleted)
		VALUES (@AccountID, 1, 5, @CreatedBy, 0)
	IF((@AccountTypeBitwise & 4) = 4) --supplier type
		INSERT INTO mapAccountTypes (AccountID, AccountTypeID, AccountStatusID, CreatedBy, IsDeleted)
		VALUES (@AccountID, 4, 5, @CreatedBy, 0)

	GOTO SelectOutput

UpdateMapAccountTypes:
--Finds out which roles are in the bit value and insert them into @temp
	DECLARE @temp TABLE (AccountTypeID INT)
	DECLARE @tempUpdated TABLE (AccountTypeID INT)
	INSERT INTO @temp 
		SELECT AccountTypeID
		FROM lkpAccountTypes WHERE @AccountTypeBitwise & AccountTypeID = AccountTypeID

	--Updates roles in @temp to (deleted = false) and outputs effected roles into @tempUpdated
	UPDATE mapAccountTypes SET IsDeleted = 0, Modified = GETUTCDATE()
	OUTPUT INSERTED.AccountTypeID INTO @tempUpdated
	WHERE AccountID = @AccountID AND AccountTypeID IN ( SELECT AccountTypeID FROM @temp )

	--Updates roles NOT in @temp to (deleted = true) and outputs effected roles into @tempUpdated
	UPDATE mapAccountTypes SET IsDeleted = 1, Modified = GETUTCDATE()
	OUTPUT INSERTED.AccountTypeID INTO @tempUpdated
	WHERE AccountID = @AccountID AND AccountTypeID NOT IN ( SELECT AccountTypeID FROM @temp )

	--Inserts remaining roles that arent in @tempUpdated
	INSERT INTO mapAccountTypes (AccountID, AccountTypeID, AccountStatusID, CreatedBy, IsDeleted)
	SELECT @AccountID, AccountTypeID, 5, @CreatedBy, 0 --AccountStatusID
	FROM @temp T
	WHERE T.AccountTypeID NOT IN (SELECT AccountTypeID FROM @tempUpdated) 

	GOTO SelectOutput
	
SelectOutput:
	SELECT AccountID
		, CompanyTypeID
		, OrganizationID
		, AccountNum
		, CurrencyID
		, AccountName
		, ApprovedVendor
		, CreatedBy
		, ExternalID
	FROM Accounts
	WHERE AccountID = @AccountID
END