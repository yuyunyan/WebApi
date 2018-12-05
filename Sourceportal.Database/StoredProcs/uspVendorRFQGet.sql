/* =============================================
   Author:		Manuka Leelaratne
   Create date: 2017.09.08
   Description:	Retrieves rfq details
   Usage:		exec uspVendorRfqGet @RfqId=0,@SearchString=NULL,@RowOffset=0,@RowLimit=25,@SortBy=NULL,@DescSort=0
				exec uspVendorRfqGet @RfqId=0,@SearchString=NULL,@RowOffset=0,@RowLimit=25,@SortBy=NULL,@DescSort=0 
				exec uspVendorRfqGet @UserId =  76 , @SearchString = '#'
   Revision History:
   09-14-17: Added columns, and function to retrieve owners.  Passing in 0 as RfqId returns all RFQs
   09-15-17: Added server-side pagination with new parameters (rowoffset, rowlimit, sortby, descsort) for SelectAll
   2017.10.10	AR	Renamed sorting columns to match front end , moved datesort out of string section
   ============================================= */

CREATE PROCEDURE [dbo].[uspVendorRFQGet]
(
	@RfqId INT = NULL,
	@SearchString NVARCHAR(100) = '',
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@UserID INT = NULL
)
AS
BEGIN

SET NOCOUNT ON;
	DECLARE @Sec TABLE (VendorRFQID INT, RoleID INT)
	INSERT @Sec EXECUTE uspVendorRFQSecurityGet @UserID = @UserID;

	IF ISNULL(@RfqId, 0) = 0						
		GOTO SelectAll			
	ELSE
		GOTO SelectOne
	
SelectOne:

	SELECT 
	V.VendorRFQID,
	V.AccountID,
	A.AccountName,
	V.ContactID,
	C.FirstName + ' ' + C.LastName Contact,
	V.StatusID,
	S.StatusName,
	V.SentDate,
	V.CurrencyID,
	dbo.fnGetRfqOwners(@RfqId) Buyer	
	FROM VendorRFQs V
	INNER JOIN Accounts A ON V.AccountID = A.AccountID
	INNER JOIN Contacts C ON V.ContactID = C.ContactID
	INNER JOIN lkpStatuses S ON V.StatusID = S.StatusID
	INNER JOIN (SELECT DISTINCT VendorRFQID FROM @Sec) sec ON V.VendorRFQID = sec.VendorRFQID
	WHERE V.VendorRFQID =  @RfqId

	IF (@@rowcount = 0)
		RETURN -1

SelectAll:
	WITH Main_CTE AS( SELECT 
	V.VendorRFQID,
	V.AccountID,
	A.AccountName,
	V.ContactID,
	C.FirstName + ' ' + C.LastName Contact,
	V.StatusID,
	S.StatusName,
	SentDate,
	dbo.fnGetRfqOwners(V.VendorRFQID) Buyer	
	FROM VendorRFQs V
	INNER JOIN Accounts A ON V.AccountID = A.AccountID
	INNER JOIN Contacts C ON V.ContactID = C.ContactID
	INNER JOIN lkpStatuses S ON V.StatusID = S.StatusID
	INNER JOIN (SELECT DISTINCT VendorRFQID FROM @Sec) sec ON V.VendorRFQID = sec.VendorRFQID
	WHERE (CAST(V.VendorRFQID AS NVARCHAR(16)) + ISNULL(A.AccountName, '') + ISNULL(C.FirstName, '') + ISNULL(C.LastName, '') + ISNULL(C.FirstName, '') + ' '+ ISNULL(C.LastName, '') + ISNULL(dbo.fnGetRfqOwners(V.VendorRFQID), '') LIKE '%' + ISNULL(@SearchString,'') + '%')
	),
	
	Count_CTE AS (
	SELECT COUNT(*) AS [RowCount]
	FROM Main_CTE
	)

	SELECT *
FROM Main_CTE, Count_CTE
	
	ORDER BY 
		
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_CTE.VendorRFQID
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'supplierName' THEN Main_CTE.AccountName 
				WHEN @SortBy = 'contactName' THEN Main_CTE.Contact 
				WHEN @SortBy = 'StatusName' THEN Main_CTE.StatusName 				
				WHEN @SortBy = 'Buyer' THEN Main_CTE.Buyer 
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'SentDate' THEN Main_CTE.SentDate 
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'VendorRFQID' THEN Main_CTE.VendorRFQID 
			END
		END ASC,
		
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_CTE.VendorRFQID
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'supplierName' THEN Main_CTE.AccountName 
				WHEN @SortBy = 'contactName' THEN Main_CTE.Contact 
				WHEN @SortBy = 'StatusName' THEN Main_CTE.StatusName 
				WHEN @SortBy = 'Buyer' THEN Main_CTE.Buyer 
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'SentDate' THEN Main_CTE.SentDate 
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'VendorRFQID' THEN Main_CTE.VendorRFQID 
			END
		END DESC

		OFFSET @RowOffset ROWS
		FETCH NEXT @RowLimit ROWS ONLY


	IF (@@rowcount = 0)
		RETURN -1

END