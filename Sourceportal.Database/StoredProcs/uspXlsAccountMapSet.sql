/*  =============================================
	Author:			Berry, Zhong
	Create date:	2017.09.06
	Description:	Insert XlsDataMaps for Account
	Usage:			EXEC uspXlsAccountMapSet @XlsAccountMapsJSON = '[{"XlsDataMapID": 1, "ColumnIndex": 4 }, {"XlsDataMapID": 2, "ColumnIndex": 3 }]', @AccountID = 5, @UserID = 68, @XlsType = 'ItemListLines'
	Return Codes:	
					-1 Missing JSON List of XlsDataMap 
					-2 Missing AccountID
					-3 Missing UserID
					-4 Missing XlsType
	=============================================*/
CREATE PROCEDURE [dbo].[uspXlsAccountMapSet]
	@XlsAccountMapsJSON VARCHAR(MAX) = NULL,
	@AccountID INT = NULL,
	@UserID INT = NULL,
	@XlsType NVARCHAR(25) = ''
AS
BEGIN
	SET NOCOUNT ON;

    IF ISNULL(@XlsAccountMapsJSON, '') = ''
		RETURN -1 

	IF ISNULL(@AccountID, 0) = 0
		RETURN -2

	IF ISNULL(@UserID, 0) = 0
		RETURN -3

	DELETE mapXlsAccount
	FROM mapXlsAccount
		INNER JOIN lkpXlsDataMap lxd ON lxd.XlsDataMapID = mapXlsAccount.XlsDataMapID
	WHERE mapXlsAccount.AccountID = @AccountID  AND lxd.XlsType = @XlsType

	MERGE mapXlsAccount AS mxa
	USING (
		SELECT 
			mj.XlsDataMapID 'XlsDataMapID',
			mj.ColumnIndex 'ColumnIndex',
			CASE WHEN EXISTS (SELECT 1 
				FROM mapXlsAccount mxa
				WHERE mxa.XlsDataMapID = mj.XlsDataMapID AND mxa.AccountID = @AccountID)
				THEN 1
				ELSE 2
			END AS HasRecord
		FROM lkpXlsDataMap dm
		INNER JOIN OPENJSON(@XlsAccountMapsJSON)
			WITH (
				XlsDataMapID INT '$.XlsDataMapID',
				ColumnIndex INT '$.ColumnIndex'
			) 
			AS mj
			ON mj.XlsDataMapID = dm.XlsDataMapID
	) AS joinedMap
	ON (joinedMap.HasRecord = 1 AND mxa.XlsDataMapID = joinedMap.XlsDataMapID)
	WHEN NOT MATCHED
		THEN INSERT (XlsDataMapID, AccountID, ColumnIndex, CreatedBy)
			 VALUES(joinedMap.XlsDataMapID, @AccountID, joinedMap.ColumnIndex, @UserID)
	WHEN MATCHED
		THEN UPDATE SET mxa.XlsDataMapID = joinedMap.XlsDataMapID,
						mxa.AccountID = @AccountID,
						mxa.ColumnIndex = joinedMap.ColumnIndex,
						mxa.ModifiedBy = @UserID;

	SELECT @@ROWCOUNT 'RowCount'
END
