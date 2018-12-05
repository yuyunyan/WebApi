/* =============================================
   Author:		Berry Zhong
   Create date: 2018.01.17
   Description:	Return  all suppliers that match the line cards
   Usage:		EXEC uspSuppliersLineCardMatch @CommoditiesJSON = '[1, 2, 3, 4, 19]', @ManufacturersJSON = '[ {"mfrName":"2E Mechatronic Gmbh & Co Kg"}, {"mfrName": "3L Electronic Corp"}]'
   Return Codes:

   Revision History:
				2018.06.13  NA  Added IsActive check on AccountStatus
   ============================================= */
CREATE PROCEDURE [dbo].[uspSuppliersLineCardMatch]
	@CommoditiesJSON VARCHAR(MAX) = '',
	@ManufacturersJSON VARCHAR(MAX) = ''
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		af.FocusID,
		af.AccountID,
		af.ObjectID,
		aft.FocusTypeID,
		aft.FocusName,
		afot.ObjectTypeID,
		a.AccountName,
		c.FirstName,
		c.LastName,
		c.OfficePhone,
		c.Email,
		c.ContactID
	FROM mapAccountFocuses af
		INNER JOIN lkpAccountFocusTypes aft ON aft.FocusTypeID = af.FocusTypeID AND aft.IsBlacklisted = 0
		INNER JOIN lkpAccountFocusObjectTypes afot ON afot.FocusObjectTypeID = af.FocusObjectTypeID AND afot.IsDeleted = 0
		INNER JOIN lkpItemCommodities ic ON ic.CommodityID = af.ObjectID AND afot.ObjectTypeID = 101
		INNER JOIN Accounts a ON a.AccountID = af.AccountID AND a.IsDeleted = 0
		INNER JOIN mapAccountTypes mat ON a.AccountID = mat.AccountID AND mat.IsDeleted = 0
		INNER JOIN lkpAccountStatuses ast ON mat.AccountStatusID = ast.AccountStatusID AND ast.AccountIsActive = 1 AND ast.IsDeleted = 0
		LEFT JOIN mapContactFocuses mcf ON mcf.FocusID = af.FocusID AND mcf.IsDeleted = 0
		LEFT JOIN Contacts c ON mcf.ContactID = c.ContactID AND c.IsDeleted = 0
		INNER JOIN OPENJSON(@CommoditiesJSON) AS cj ON af.ObjectID = cj.value
	UNION
	SELECT
		af.FocusID,
		af.AccountID,
		af.ObjectID,
		aft.FocusTypeID,
		aft.FocusName,
		afot.ObjectTypeID,
		a.AccountName,
		c.FirstName,
		c.LastName,
		c.OfficePhone,
		c.Email,
		c.ContactID
	FROM mapAccountFocuses af
		INNER JOIN lkpAccountFocusTypes aft ON aft.FocusTypeID = af.FocusTypeID AND aft.IsBlacklisted = 0
		INNER JOIN lkpAccountFocusObjectTypes afot ON afot.FocusObjectTypeID = af.FocusObjectTypeID AND afot.IsDeleted = 0
		INNER JOIN Manufacturers m ON m.MfrID = af.ObjectID AND afot.ObjectTypeID = 102
		INNER JOIN Accounts a ON a.AccountID = af.AccountID AND a.IsDeleted = 0
		INNER JOIN mapAccountTypes mat ON a.AccountID = mat.AccountID AND mat.IsDeleted = 0
		INNER JOIN lkpAccountStatuses ast ON mat.AccountStatusID = ast.AccountStatusID AND ast.AccountIsActive = 1 AND ast.IsDeleted = 0
		LEFT JOIN mapContactFocuses mcf ON mcf.FocusID = af.FocusID AND mcf.IsDeleted = 0
		LEFT JOIN Contacts c ON mcf.ContactID = c.ContactID AND c.IsDeleted = 0
		INNER JOIN OPENJSON(@ManufacturersJSON) WITH (mfrName varchar(200) '$.mfrName')AS mj ON m.MfrName LIKE '%' + ISNULL(mj.mfrName ,'')+ '%'
END