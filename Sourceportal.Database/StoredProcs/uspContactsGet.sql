/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.06.02
   Description:	Retrieves all Contacts from Contacts tbl
   Usage:		EXEC uspContactsGet @RowOffset = NULL, @RowLimit = 0
				EXEC uspContactsGet @SearchText='juan' , @UserID=76
				SELECT Top 100* FROM Contacts
				SELECT * FROM Locations
   Revision History:
		2017.06.06	AR	Changed EndRow default to max
		2016.06.07	AR	Added support to pass AccountID paramter
		2017.06.12	AR	Added Mobile/Fax, Title, LocationID, ContactStatusID and PreferredContactMethodID columns
		2017.06.14	AR	Added NULLIF to AccountID condition
		2017.06.27  NA  Replaced Accounts.Name with Accounts.AccountName
		2017.07.10  NA  Implemented pagination and remote sorting
		2017.10.16  CT  Added ExternalID to SELECT
		2017.10.30 ML Added ExternalID
		2017.11.27	BZ	Added alias 'Note' to Details column, Removed ContactStatus, Removed IsActive from WHERE clause
		2017.11.28	BZ	Added IsDeleted in join clause
		2017.11.30	BZ	Added Department, JobFunctionID, Birthdate, Gender, Salutation, MaritalStatus, KidsNames and ReportsTo
		2018.01.22	CT  Removed Accounts.AccountTypeID, and account status
		2018.02.19	BZ	Added AccountTypeBitWise
   ============================================= */
CREATE PROCEDURE [dbo].[uspContactsGet]
(
	@ContactID INT = NULL,
	@AccountID INT = NULL,
	@SearchText VARCHAR(256) = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 500,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@FilterBy NVARCHAR(25) = NULL,
	@FilterText VARCHAR(256) = NULL,
	@UserID INT = NULL
)
AS
BEGIN
	DECLARE @Sec TABLE (ContactID INT, RoleID INT)
	INSERT @Sec EXECUTE uspContactSecurityGet @UserID = @UserID;

	SET NOCOUNT ON;
	IF ISNULL(@RowLimit,0) = 0
		SET @RowLimit = 500

	SELECT C.ContactID,
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
		dbo.fnGetAccountTypes(A.AccountID) AccountTypes,
		(SELECT 
			SUM(mat.AccountTypeID)
		 FROM mapAccountTypes mat
		 WHERE mat.AccountID = C.AccountID AND mat.IsDeleted = 0) AccountTypeBitwise,
		COUNT(*) OVER() AS 'TotalRows'
	FROM Contacts C
		INNER JOIN (SELECT DISTINCT ContactID FROM @Sec) sec ON C.ContactID = sec.ContactID AND C.IsDeleted = 0
		LEFT OUTER JOIN Accounts A ON A.AccountID = C.AccountID
	WHERE C.ContactID = ISNULL(@ContactID,C.ContactID)
              AND A.AccountID = ISNULL(NULLIF(@AccountID,0), A.AccountID)
              AND (ISNULL(FirstName,'') + ISNULL(LastName,'') + CONVERT(VARCHAR(16),ISNULL(C.OfficePhone,0)) + CONVERT(VARCHAR(16),ISNULL(Fax,0)) + CONVERT(VARCHAR(16),ISNULL(C.Fax,0)) + ISNULL(C.Email,'') --+ ISNULL(Details,'') 
                  + ISNULL(A.AccountName,'') + ISNULL(dbo.fnGetContactOwners(C.ContactID),'') + ISNULL(dbo.fnGetAccountTypes(A.AccountID),'') --+ ISNULL(S.Name,'')
                     LIKE '%' + ISNULL(@SearchText,'') + '%'  ) --This may have to be replace with an indexed CONTAINS with more records (unless memory optimized table)
              AND (@FilterBy IS NULL OR (
                     ((@FilterBy = 'FirstName' AND FirstName LIKE '%' + ISNULL(@FilterText , '') + '%')
                     OR
                     (@FilterBy = 'LastName' AND LastName LIKE '%' + ISNULL(@FilterText , '') + '%')
                     OR 
					  (@FilterBy = 'AccountName' AND AccountName LIKE '%' + ISNULL(@FilterText , '') + '%')
					 --OR
						--(@FilterBy = 'AccountStatus' AND s.[Name] LIKE '%' + ISNULL(@FilterText , '') + '%')
					 OR
						(@FilterBy = 'AccountTypes' AND dbo.fnGetAccountTypes(A.AccountID) LIKE '%' + ISNULL(@FilterText , '') + '%')
					 OR 
						(@FilterBy = 'Email' AND C.Email LIKE '%' + ISNULL(@FilterText , '') + '%')
					 OR
						 (@FilterBy = 'Phone' AND C.OfficePhone LIKE '%' + ISNULL(@FilterText , '') + '%')
					OR
						(@FilterBy = 'Owners' AND dbo.fnGetContactOwners(C.ContactID) LIKE '%' + ISNULL(@FilterText , '') + '%')
              )))
       ORDER BY
              CASE WHEN @DescSort = 0 THEN
                     CASE 
                           WHEN ISNULL(@SortBy, '') = '' THEN C.ContactID
                     END
              END ASC,
              CASE WHEN @DescSort = 0 THEN
                     CASE 
                           WHEN @SortBy = 'AccountName' THEN A.AccountName
                           WHEN @SortBy = 'FirstName' THEN c.FirstName
                           WHEN @SortBy = 'LastName' THEN c.LastName
                           --WHEN @SortBy = 'AccountStatus' THEN s.[Name]
                           WHEN @SortBy = 'AccountTypes' THEN dbo.fnGetAccountTypes(A.AccountID)
                           WHEN @SortBy = 'Email' THEN C.Email 
                           WHEN @SortBy = 'Phone' THEN C.OfficePhone 
                           WHEN @SortBy = 'Owners' THEN dbo.fnGetContactOwners(C.ContactID)
                     END
              END ASC,
              CASE WHEN @DescSort = 1 THEN
                     CASE 
                           WHEN ISNULL(@SortBy, '') = '' THEN C.ContactID
                     END
              END DESC,
              CASE WHEN @DescSort = 1 THEN
                     CASE 
                           WHEN @SortBy = 'AccountName' THEN A.AccountName
                           WHEN @SortBy = 'FirstName' THEN c.FirstName
                           WHEN @SortBy = 'LastName' THEN c.LastName
                           --WHEN @SortBy = 'AccountStatus' THEN s.[Name]
                           WHEN @SortBy = 'AccountTypes' THEN dbo.fnGetAccountTypes(A.AccountID)
                           WHEN @SortBy = 'Email' THEN C.Email 
                           WHEN @SortBy = 'Phone' THEN C.OfficePhone 
                           WHEN @SortBy = 'Owners' THEN dbo.fnGetContactOwners(C.ContactID)
                     END
              END DESC
              OFFSET ISNULL(@RowOffset,0) ROWS
              FETCH NEXT @RowLimit ROWS ONLY
END
