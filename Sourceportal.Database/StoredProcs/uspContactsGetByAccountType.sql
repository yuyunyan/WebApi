/* =============================================
   Author:		Remya Varriem
   Create date: 2018.02.02
   Description:	Retrieves all Contacts from Contacts tbl based on Account type
   Usage:		EXEC uspContactsGetByAccountType @UserID=76 , @AccountTypeID = 4 , @AccountIsActive = 0
				EXEC uspContactsGetByAccountType @UserID=76 , @AccountTypeID = 4, @AccountIsActive = 1
				EXEC uspContactsGetByAccountType @UserID=76 , @AccountTypeID = 1 , @AccountIsActive = 0
				EXEC uspContactsGetByAccountType @UserID=76 , @AccountId = 24
   Revision History:
		2018.02.02	RV	Added AccountTypeId for getting contacts
		2018.02.26	BZ	Added IsDeleted to WHERE clause 
		2018.02.27	BZ	Added IsDeleted to join condition with mapAccountTypes table
		2018.02.28	BZ	Added phone number strip search to column filter
		2018.03.01	BZ	Added phone number strip search to grid level
		2018.10.16	BZ	Include inactive contacts as well
   ============================================= */
CREATE PROCEDURE [dbo].[uspContactsGetByAccountType]
(
	@AccountTypeID INT = 0,
	@ContactID INT = NULL,
	@AccountID INT = NULL,
	@SearchText VARCHAR(256) = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 500,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@FilterBy NVARCHAR(25) = NULL,
	@FilterText VARCHAR(256) = NULL,
	@UserID INT = NULL,
	@AccountIsActive BIT = 1
)
AS
BEGIN
	
	DECLARE @Sec TABLE (ContactID INT, RoleID INT)
	INSERT @Sec EXECUTE uspContactSecurityGet @UserID = @UserID;
	
	SET NOCOUNT ON;
	IF ISNULL(@RowLimit,0) = 0
		SET @RowLimit = 500;
	WITH Main_Temp AS (
	SELECT C.ContactID,
		C.AccountID,
		SUM(mat.AccountTypeID) AccountTypeBitwise , --A.AccountTypeID AccountTypeBitwise
		A.AccountName,
		C.FirstName,
		C.LastName,
		C.OfficePhone,
		C.MobilePhone,
		C.Fax,
		C.Email,
		Title,
		Details 'Note',
		C.PreferredContactMethodID,
		C.LocationID,
		C.IsActive,
		C.ExternalID,
		C.Department,
		C.JobFunctionID,
		C.Birthdate,
		C.Gender,
		C.Salutation,
		C.MaritalStatus,
		C.KidsNames,
		C.ReportsTo,
		dbo.fnGetContactOwners(C.ContactID) Owners,
		(s.Name) AccountStatus,
		s.AccountIsActive ,
		COUNT(*) OVER() AS 'TotalRows'
	FROM Contacts C
		INNER JOIN (SELECT DISTINCT ContactID FROM @Sec) sec ON C.ContactID = sec.ContactID AND C.IsDeleted = 0
		LEFT OUTER JOIN Accounts A ON A.AccountID = C.AccountID
		INNER JOIN mapAccountTypes mat on mat.AccountID = A.AccountID and mat.IsDeleted = 0
		INNER JOIN lkpAccountTypes T on T.AccountTypeID & mat.AccountTypeID = T.AccountTypeID 
		LEFT OUTER JOIN lkpAccountStatuses s on mat.AccountStatusID = s.AccountStatusID 
	WHERE C.ContactID = ISNULL(@ContactID,C.ContactID)
	AND A.IsDeleted = 0
	AND C.isDeleted = 0
    AND A.AccountID = ISNULL(NULLIF(@AccountID,0), A.AccountID)
	AND (@AccountTypeID = 0 OR T.AccountTypeID =ISNULL(@AccountTypeID, T.AccountTypeID)  )
	AND ((@AccountIsActive = 1 AND s.AccountIsActive = ISNULL(@AccountIsActive,s.AccountIsActive)
				OR
		(@AccountIsActive = 0 AND s.AccountIsActive IN (1 , 0 )) ) ) --return that are in either of the status
    AND (ISNULL(FirstName,'') + ISNULL(LastName,'') + CONVERT(VARCHAR(16),ISNULL(C.OfficePhone,0)) + ISNULL(C.Email,'') 
        + ISNULL(A.AccountName,'')  + dbo.fnStripNonNumeric(ISNULL(C.OfficePhone, ''))
            LIKE '%' + ISNULL(@SearchText,'') + '%'  ) 
    AND (@FilterBy IS NULL OR (
            ((@FilterBy = 'FirstName' AND C.FirstName LIKE '%' + ISNULL(@FilterText , '') + '%')
            OR
            (@FilterBy = 'LastName' AND C.LastName LIKE '%' + ISNULL(@FilterText , '') + '%')
            OR 
			(@FilterBy = 'AccountName' AND A.AccountName LIKE '%' + ISNULL(@FilterText , '') + '%')

			OR 
			(@FilterBy = 'Email' AND C.Email LIKE '%' + ISNULL(@FilterText , '') + '%')
			OR
				(@FilterBy = 'Phone' AND dbo.fnStripNonNumeric(C.OfficePhone) LIKE '%' + ISNULL(@FilterText , '') + '%')
			OR
				(@FilterBy = 'AccountStatus' AND s.Name LIKE '%' + ISNULL(@FilterText , '') + '%')
				OR
				(@FilterBy = 'Owners' AND dbo.fnGetContactOwners(C.ContactID) LIKE '%' + ISNULL(@FilterText , '') + '%')
	
    )))

	 GROUP BY
		C.ContactID,
		C.AccountID,
		A.AccountName,
		C.FirstName,
		C.LastName,
		C.OfficePhone,
		C.MobilePhone,
		C.Fax,
		C.Email,
		C.ExternalID,
		Title,
		Details,
		C.PreferredContactMethodID,
		C.LocationID,
		C.IsActive,
		C.ExternalID,
		C.Department,
		C.JobFunctionID,
		C.Birthdate,
		C.Gender,
		C.Salutation,
		C.MaritalStatus,
		C.KidsNames,
		C.ReportsTo,
		dbo.fnGetContactOwners(C.ContactID),
		(s.Name),
		s.AccountIsActive
	 	),
	Count_Temp AS(
	    SELECT COUNT(*) AS TotalRowCount
		From Main_Temp
	     )


	SELECT *
	FROM Main_Temp,Count_Temp
       ORDER BY
              CASE WHEN @DescSort = 0 THEN
                     CASE 
                           WHEN ISNULL(@SortBy, '') = '' THEN Main_Temp.ContactID
                     END
              END ASC,
              CASE WHEN @DescSort = 0 THEN
                     CASE 
                           WHEN @SortBy = 'AccountName' THEN Main_Temp.AccountName
                           WHEN @SortBy = 'FirstName' THEN Main_Temp.FirstName
                           WHEN @SortBy = 'LastName' THEN Main_Temp.LastName
                           WHEN @SortBy = 'Email' THEN Main_Temp.Email 
                           WHEN @SortBy = 'Phone' THEN Main_Temp.OfficePhone 
						   WHEN @SortBy = 'AccountStatus' THEN Main_Temp.AccountStatus 
						   WHEN @SortBy = 'Owners' THEN dbo.fnGetContactOwners(Main_Temp.ContactID)
                     END
              END ASC,
              CASE WHEN @DescSort = 1 THEN
                     CASE 
                           WHEN ISNULL(@SortBy, '') = '' THEN Main_Temp.ContactID
                     END
              END DESC,
              CASE WHEN @DescSort = 1 THEN
                     CASE 
                           WHEN @SortBy = 'AccountName' THEN Main_Temp.AccountName
                           WHEN @SortBy = 'FirstName' THEN Main_Temp.FirstName
                           WHEN @SortBy = 'LastName' THEN Main_Temp.LastName
                           WHEN @SortBy = 'Email' THEN Main_Temp.Email 
                           WHEN @SortBy = 'Phone' THEN Main_Temp.OfficePhone 
						   WHEN @SortBy = 'AccountStatus' THEN Main_Temp.AccountStatus 
						   WHEN @SortBy = 'Owners' THEN dbo.fnGetContactOwners(Main_Temp.ContactID)
                     END
              END DESC
              OFFSET ISNULL(@RowOffset,0) ROWS
              FETCH NEXT @RowLimit ROWS ONLY
END
