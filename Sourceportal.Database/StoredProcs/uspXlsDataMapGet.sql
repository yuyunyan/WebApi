/* =============================================
	 Author:			Berry, Zhong
	 Create date:		2017.09.08
	 Description:		Retrieves XlsDataMaps for given XlsType
	 Usage:				EXEC uspXlsDataMapGet @XlsType = 'ItemListLines'
						SELECT * FROM lkpXlsDataMap
						UPDATE lkpXlsDataMap SET ItemListTypeID = 1
	 Revision:
	 2018.01.12		AR	Added support for @ItemListTypeID
   =============================================*/
CREATE PROCEDURE [dbo].[uspXlsDataMapGet]
	@XlsType NVARCHAR(25) = NULL,
	@ItemListTypeID INT = 1
AS
BEGIN
	SELECT
		XlsDataMapID,
		FieldLabel,
		IsRequired
	FROM lkpXlsDataMap xdm
	WHERE xdm.XlsType = @XlsType
	AND ItemListTypeID = ISNULL(@ItemListTypeID,ItemListTypeID)
END