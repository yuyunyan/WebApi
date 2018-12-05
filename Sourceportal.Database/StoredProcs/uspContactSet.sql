/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.06.02
   Description:	Inserts or updates contact data from Contacts tbl
   Usage:	EXEC [uspContactSet] @AccountID = 1, @FirstName = 'Aaron', @LastName = 'Rodecker', @OfficePhone = 7141112222, @Email = 'aaronrcustomer@sourceability.com', @Details = 'test', @CreatedBy = 1
			SELECT * FROM Contacts
			DELETE Contacts
   Return Codes:
		-5 Insert contact failed
		-6 Update contact failed, check ContactID

   Revision History:
		2017.06.06	AR	Changed EndRow default to max
		2017.06.12	AR	Added support for Office/Mobile/Fax Phone, PreferredContact, ContactStatusID, LocationID, Title columns
		2017.06.21	AR	Changed Phone fields to varchar
		2017.10.18  ML  Added ExternalID
		2017.11.14	NA	Added ownership copy from Account
		2017.11.27	BZ	Removed ContactStatus column, added IsActive in UpdateContact
		2017.11.30	BZ	Added Department, JobFunctionID, Birthdate, Gender, Salutation, MaritalStatus, KidsNames and ReportsTo
		2018.02.19  BZ  Add creator of contact as owner when copy from account owner failed 
		2018.02.27	BZ	Update IsDeleted to support deleting contact
		2018.04.02	CT  Update Row if External ID is matching
   ============================================= */
CREATE PROCEDURE [dbo].[uspContactSet]
(
	@ContactID INT = NULL OUTPUT
	, @AccountID INT = NULL
	, @FirstName VARCHAR(64) = NULL
	, @LastName VARCHAR(64) = NULL
	, @OfficePhone VARCHAR(32) = NULL
	, @MobilePhone VARCHAR(32) = NULL
	, @Fax VARCHAR(32) = NULL
	, @PreferredContactMethodID INT = 0
	, @LocationID INT = NULL
	, @Title VARCHAR(256) = NULL
	, @Email VARCHAR(256) = NULL
	, @Details VARCHAR(1024) = NULL
	, @CreatedBy INT = NULL
	, @IsActive BIT = 1
	, @Department VARCHAR(256) = NULL
	, @JobFunctionID INT = NULL
	, @Birthdate DATE = NULL
	, @Gender VARCHAR(50) = NULL
	, @Salutation VARCHAR(50) = NULL
	, @MaritalStatus VARCHAR(50) = NULL
	, @KidsNames VARCHAR(MAX) = NULL
	, @ReportsTo VARCHAR(256) = NULL
	, @ExternalID VARCHAR(50) = NULL
	, @IsDeleted BIT = 0
)
AS
BEGIN
	IF ISNULL(@ContactID, 0) = 0
		GOTO InsertContact
	ELSE
		GOTO UpdateContact

InsertContact:
	INSERT INTO Contacts (AccountID, FirstName, LastName, OfficePhone, MobilePhone, Fax,
		PreferredContactMethodID, LocationID, Title,  Email, Details, IsActive, CreatedBy, ExternalID,
		Department, JobFunctionID, Birthdate, Gender, Salutation, MaritalStatus, KidsNames, ReportsTo)
	VALUES (@AccountID, @FirstName, @LastName, @OfficePhone, @MobilePhone, @Fax,
		@PreferredContactMethodID, @LocationID, @Title, @Email, @Details, @IsActive, @CreatedBy, @ExternalID,
		@Department, @JobFunctionID, @Birthdate, @Gender, @Salutation, @MaritalStatus, @KidsNames, @ReportsTo)	
	
	SET @ContactID = @@identity
	IF (@@ROWCOUNT=0)
		RETURN -5
	
	--Copy Ownership from the Account
	INSERT INTO mapOwnership (OwnerID, ObjectTypeID, ObjectID, IsGroup, [Percent], CreatedBy)
	SELECT OwnerID, 2, @ContactID, IsGroup, [Percent], @CreatedBy
	FROM mapOwnership
	WHERE ObjectID = @AccountID AND ObjectTypeID = 1 AND IsDeleted = 0

	IF (@@ROWCOUNT > 0)
		GOTO ReturnSelect
	ELSE
		BEGIN
			INSERT INTO mapOwnership (OwnerID, ObjectTypeID, ObjectID, IsGroup, [Percent], CreatedBy)
			VALUES (@CreatedBy, 2, @ContactID, 0, 100, @CreatedBy)

			GOTO ReturnSelect
		END

UpdateContact:
	UPDATE Contacts
		SET AccountID = ISNULL(@AccountID, AccountID)
		, FirstName = ISNULL(@FirstName, FirstName)
		, LastName = ISNULL(@LastName, LastName)
		, OfficePhone = ISNULL(@OfficePhone, OfficePhone)
		, MobilePhone = ISNULL(@MobilePhone, MobilePhone)
		, Fax = ISNULL(@Fax, Fax)
		, PreferredContactMethodID = ISNULL(@PreferredContactMethodID, PreferredContactMethodID)
		, LocationID = ISNULL(@LocationID, LocationID)
		, Title = ISNULL(@Title, Title)
		, Email = ISNULL(@Email, Email)
		, Details = ISNULL(@Details, Details)
		, IsActive = ISNULL(@IsActive, IsActive)
		, ModifiedBy = ISNULL(@CreatedBy, CreatedBy)
		, Modified = GETUTCDATE()
		, ExternalID = @ExternalID
		, Department = ISNULL(@Department, Department)
		, JobFunctionID = ISNULL(@JobFunctionID, JobFunctionID)
		, Birthdate = ISNULL(@Birthdate, Birthdate)
		, Gender = ISNULL(@Gender, Gender)
		, Salutation = ISNULL(@Salutation, Salutation)
		, MaritalStatus = ISNULL(@MaritalStatus, MaritalStatus)
		, KidsNames = ISNULL(@KidsNames, KidsNames)
		, ReportsTo = ISNULL(@ReportsTo, ReportsTo)
		, IsDeleted = ISNULL(@IsDeleted, IsDeleted)
		WHERE ContactID = @ContactID  OR ExternalID = @ExternalID --null = null ? equals false

		IF ISNULL(@ContactID,0) = 0
			RETURN -6
		GOTO ReturnSelect

ReturnSelect:
	SELECT ContactID
		, AccountID
		, FirstName
		, LastName
		, OfficePhone
		, MobilePhone
		, Fax
		, PreferredContactMethodID
		, ContactStatusID
		, LocationID
		, Title
		, Email
		, Details
		, IsActive
		, CreatedBy
		, ExternalID
		, Department
		, Birthdate
		, Gender
		, Salutation
		, MaritalStatus
		, KidsNames
		, ReportsTo
	FROM Contacts
	WHERE ContactID = @ContactID
END
