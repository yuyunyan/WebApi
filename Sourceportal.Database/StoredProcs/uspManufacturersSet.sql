/* =============================================
   Author: Hrag Sarkissian
   Create date: 2018.06.01
   Description:	Creating new manufacturer
   Usage: EXEC uspCreateMfr
   ============================================= */

CREATE PROCEDURE [dbo].[uspManufacturersSet]
	--@MfrID INT = 11,
	@MfrName NVARCHAR(50),
	@Code INT = 0,
	@MfrUrl NVARCHAR(50),
	@CreatedBy INT = NULL

AS
BEGIN
/*
declare 
	--@_mfrId int = 51,
	@_mfrName nvarchar(50),
	@_code int,
	@_mfrUrl nvarchar(50),
	@_createdBy int;
	
	set @_mfrName = 'hasdfarryeweasdfdwa';
	set @_code = 0;
	set @_mfrUrl = 'www.source123.com';
	set @_createdBy = 12;
	
DECLARE @output INT 
EXEC @output =  uspManufacturersSet
	--@_mfrId,
	@_mfrName,
	@_code,
	@_mfrUrl,
	@_createdBy;

	--SELECT @output
*/
	DECLARE @MfrID INT = 0
	IF NOT EXISTS (SELECT 1 FROM Manufacturers WHERE MfrName = @MfrName)
	BEGIN
	   INSERT INTO Manufacturers(MfrName,Code,MfrURL,CreatedBy)
	   VALUES
	   (
	   @MfrName,
	   @Code,
	   @MfrUrl,
	   @CreatedBy
	   )
		SET @MfrID = SCOPE_IDENTITY()

		SELECT @MfrID 

	END
	ELSE
	BEGIN
		SELECT @MfrID 
	END
END