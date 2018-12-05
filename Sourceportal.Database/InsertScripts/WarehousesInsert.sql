DECLARE @Singapore INT
DECLARE @HongKong INT
DECLARE @UnitedStates INT

SELECT @Singapore = ShipFromRegionID FROM lkpShipFromRegions WHERE RegionName = 'Singapore'
SELECT @HongKong = ShipFromRegionID FROM lkpShipFromRegions WHERE RegionName = 'Hong Kong'
SELECT @UnitedStates = ShipFromRegionID FROM lkpShipFromRegions WHERE RegionName = 'United States'

INSERT INTO Warehouses (OrganizationID, WarehouseName, ExternalID, ShipFromRegionID, CreatedBy) VALUES 
	(1,  'US externally located',			'USEXT',		NULL,			0) 
,	(1,  'US Owned in US WH (US7200US)',	'US7200US',		@UnitedStates,	0) 
,	(1,  'US Owned in SG WH (US7200SG)',	'US7200SG',		@Singapore,		0) 
,	(1,  'US Owned in HK WH (US7200HK)',	'US7200HK',		@HongKong,		0)
,	(17, 'HK externally located',			'HKEXT',		NULL,			0) 
,	(17, 'HK Owned in HK WH (HK5200HK)',	'HK5200HK',		@HongKong,		0) 
,	(17, 'HK Owned in US WH (HK5200US)',	'HK5200US',		@UnitedStates,	0) 
,	(17, 'HK Owned in SG WH (HK5200SG)',	'HK5200SG',		@Singapore,		0) 
,	(17, 'HK Owned in HK2 WH (HK5200WH)',	'HK5200WH',		@HongKong,		0) 
,	(22, 'SG externally located',			'SGEXT',		NULL,			0) 
,	(22, 'SG Owned in SG WH (SG5300SG)',	'SG5300SG',		@Singapore,		0) 
,	(22, 'SG Owned in HK WH (SG5300HK)',	'SG5300HK',		@HongKong,		0) 
,	(22, 'SG Owned in US WH (SG5300US)',	'SG5300US',		@UnitedStates,	0)