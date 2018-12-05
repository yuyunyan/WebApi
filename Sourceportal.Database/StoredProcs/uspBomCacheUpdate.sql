/* =============================================
	 Author:		Aaron Rodecker
	 Create date: 2017.09.14
	 Description:	Based from [uspBomCacheUpdate] - populates cache data table with data
	 Usage: EXEC [uspBomCacheUpdate]
			SELECT * FROM BOMCachedResults
			SELECT * FROM logBOMCacheDataPre
   ============================================= */
CREATE PROCEDURE [dbo].[uspBomCacheUpdate] 	
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE		@TmpVal int = (SELECT TOP (1) COALESCE(CurrentVersion, 0) + 1 FROM logBOMCacheVersion)

	--BEGIN TRANSACTION
		
	--	BEGIN TRY

			DROP TABLE logBOMCacheDataPre
	
			CREATE TABLE logBOMCacheDataPre(
				[DatabaseID] [int] NULL,
				[RecordID] [int] NULL,
				[LineID] [int] NULL,
				[TypeID] [char](1) NULL,
				[PartNumberStrip] [varchar](250) NULL,
				[Manufacturer] [varchar](250) NULL,
				[AccountName] [varchar](250) NULL,
				[Price] [float] NULL,
				[RecDate] [date] NULL
			) ON [PRIMARY]

			INSERT INTO		logBOMCacheDataPre WITH (TABLOCK)
			SELECT			D.*
			FROM            [SourcePortal_DEV].bom.viewCacheData D
			
			INSERT INTO		BOMCachedResults (DatabaseID, RecordID, LineID, TypeID, PartNumberStrip, Manufacturer, AccountName, Price, RecDate, [Version]) 
			SELECT			DatabaseID, RecordID, LineID, TypeID, PartNumberStrip, Manufacturer, AccountName, Price, RecDate, @TmpVal
			FROM            logBOMCacheDataPre

			UPDATE  logBOMCacheVersion
			SET		CurrentVersion = @TmpVal,
					UpdateDate = GETDATE()
			
			DELETE FROM BOMCachedResults WHERE [Version] = @TmpVal - 1
						
		--	COMMIT TRANSACTION
		--END TRY

		--BEGIN CATCH
		--	ROLLBACK TRANSACTION
		--END CATCH	
END