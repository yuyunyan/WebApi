CREATE PROCEDURE [dbo].[uspQuoteTypesGet]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		QuoteTypeID
		, TypeName
	FROM lkpQuoteTypes
	where IsDeleted = 0
END