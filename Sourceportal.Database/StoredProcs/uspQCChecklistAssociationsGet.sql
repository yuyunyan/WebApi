/* =============================================
   Author:		Corey Tyrrell
   Create date: 2017.08.24
   Description:	Gets Checklist Associations from mapQCChecklistsJoin tbl using ChecklistID to narrow
   Usage: EXEC uspQCChecklistAssociationsGet @ChecklistID = 4

   Revision History:
   2017.11.30	AR	Added UNION for "Any" type links, which have ObjectID/ObjectTypeID = 0
   2018.11.01   NA  Added lkpCompanyTypes Join
   Return Codes:
   ============================================= */


CREATE PROCEDURE [dbo].[uspQCChecklistAssociationsGet]
(
	@ChecklistID INT = NULL
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
	o.ObjectName AS LinkType --ISNULL(t.Name, o.ObjectName) AS LinkType
	, COALESCE(a.AccountName, i.PartNumber, c.CommodityName, m.MfrName, ct.[Name]) 'Value'
	, qc.ObjectID
	, qc.ObjectTypeID 
	FROM dbo.mapQCChecklistJoins qc
	INNER JOIN [dbo].lkpObjectTypes o on o.ObjectTypeId = qc.ObjectTypeId
	LEFT JOIN [dbo].Accounts a on a.AccountID = qc.ObjectID AND qc.ObjectTypeID = 1
	LEFT JOIN lkpCompanyTypes ct ON qc.ObjectID = ct.CompanyTypeID AND qc.ObjectTypeID = 110
	--LEFT JOIN [dbo].lkpAccountTypes t on t.AccountTypeID = a.AccountTypeID
	LEFT JOIN [dbo].lkpItemCommodities c on c.CommodityID = qc.ObjectID AND qc.ObjectTypeID = 101
	LEFT JOIN [dbo].Manufacturers m on m.MfrID = qc.ObjectID AND qc.ObjectTypeID = 102
	LEFT JOIN [dbo].Items i on i.ItemID = qc.ObjectID AND qc.ObjectTypeID = 103
	WHERE qc.ChecklistID = @ChecklistID AND qc.IsDeleted = 0

	UNION ALL SELECT
		'Any',
		NULL Value,
		0,
		0
	FROM mapQCChecklistJoins QC
	WHERE QC.CheckListID = @CheckListID 
	AND QC.ObjectID = 0 AND QC.ObjectTypeID = 0AND QC.IsDeleted = 0
END
