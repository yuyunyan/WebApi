

--Update and insert from SAP data.  Insert the data from SAP into a temp @Binz table
DECLARE @Binz TABLE (BinID VARCHAR(20), BinUUID VARCHAR(50), WarehouseID VARCHAR(20))
		--Insert records here

MERGE WarehouseBins  AS wb
USING (SELECT binz.BinID, binz.BinUUID, w.WarehouseID
		FROM @Binz binz
		INNER JOIN Warehouses w ON binz.WarehouseID = w.ExternalID) AS b ON (wb.ExternalUUID = b.BinUUID)
WHEN NOT MATCHED
	THEN INSERT (WarehouseID, BinName, ExternalID, ExternalUUID, CreatedBy)
	VALUES (b.WarehouseID, b.BinID, b.BinID, b.BinUUID, 1)
WHEN MATCHED
	THEN UPDATE SET wb.BinName = b.BinID, wb.ExternalID = b.BinID, wb.Modified = GETUTCDATE(), wb.ModifiedBy = 0;


--Insert In-Transit bins
INSERT INTO WarehouseBins (WarehouseID, BinName, CreatedBy)
SELECT w.WarehouseID, 'In Transit', 0 
FROM Warehouses w
LEFT OUTER JOIN WarehouseBins b ON w.WarehouseID = b.WarehouseID AND b.ExternalID IS NULL
WHERE w.IsDeleted = 0 AND b.WarehouseID IS NULL

--Set IsSelectable
UPDATE WarehouseBins SET IsSelectable = 1 WHERE BinName IN ('QA', 'RTV', 'QC', 'SCRAP', 'PUT_AWAY')

--Set Accepted Bins
UPDATE w
SET w.AcceptedBinID = wb.WarehouseBinID
FROM Warehouses w
INNER JOIN WarehouseBins wb ON w.WarehouseID = wb.WarehouseID AND wb.BinName LIKE 'Put%'

--Set Rejected Bins
UPDATE w
SET w.RejectedBinID = wb.WarehouseBinID
FROM Warehouses w
INNER JOIN WarehouseBins wb ON w.WarehouseID = wb.WarehouseID AND wb.BinName = 'RTV'