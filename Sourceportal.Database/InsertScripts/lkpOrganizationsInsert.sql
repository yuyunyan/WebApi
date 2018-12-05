--DELETE FROM Organizations

SET IDENTITY_INSERT Organizations ON;
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY, BranchName, USDAccount, SwiftAccount, BankName, RoutingNumber) VALUES (1, NULL, 'Sourceability North America, LLC', '7200', 0, 'New York', '23071314501', 'HYVEUS33', 'UniCredit Bank AG', '0260-0880-8') 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (2, 1, 'Sales NA Trading (Reporting Only)', 'S7200', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (3, 2, 'Sales Unit CA Trading', 'S7202', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (4, 2, 'Sales Unit TX Trading', 'S7203', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (5, 2, 'Sales Unit NY Trading', 'S7204', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (6, 2, 'Sales Unit FL Trading', 'S7201', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (7, 1, 'Sales NA E-Commerce (Reporting Only)', 'S7205', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (8, 7, 'Sales Unit CA E-Commerce', 'S7210', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (9, 7, 'Sales Unit NY E-Commerce', 'S7211', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (10, 7, 'Sales Unit TX E-Commerce', 'S7212', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (11, 7, 'Sales Unit FL E-Commerce', 'S7206', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (12, 1, 'Sales NA Service (Reporting Only)', 'S7207', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (13, 12, 'Sales Unit NY Service', 'S7213', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (14, 12, 'Sales Unit TX Service', 'S7214', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (15, 12, 'Sales Unit FL Service', 'S7208', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (16, 12, 'Sales Unit CA Service', 'S7209', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY, BranchName, USDAccount, SwiftAccount, BankName) VALUES (17, NULL, 'Sourceability HK Ltd.', '5200_N', 0, 'Hong Kong Branch','1167720101','BVBEHKHH','UniCredit Bank AG') 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (18, 17, 'Sales HK E-Commerce (Reporting Only)', 'S5202', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (19, 18, 'Sales Unit HK E-Commerce', 'S5203', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (20, 17, 'Sales HK Trading (Reporting Only)', 'S5200_N', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (21, 20, 'Sales Unit HK Trading', 'S5201', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY, BranchName, USDAccount, EURAccount, SwiftAccount, BankName) VALUES (22, NULL, 'Sourceability SG PTE. Ltd.', '5300_N', 0, 'Singapore Branch', '1031923301', '1031929901', 'BVBESGSG', 'UniCredit Bank AG') 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (23, 22, 'Sales SG Trading (Reporting Only)', 'S5300_N', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (24, 23, 'Sales Unit SG Trading', 'S5301_N', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (25, 22, 'Sales SG Service (Reporting only)', 'S5304_N', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (26, 25, 'Sales Unit SG Service', 'S5305_N', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (27, 22, 'Sales SG E-Commerce (Reporting Only)', 'S5302_N', 0) 
INSERT INTO Organizations (OrganizationID, ParentOrgID, [Name], ExternalID, CreatedBY) VALUES (28, 27, 'Sales Unit SG E-Commerce', 'S5303_N', 0) 
SET IDENTITY_INSERT Organizations OFF;

--Sales Orders 16
INSERT INTO mapOrgObjectTypes (OrganizationID, ObjectTypeID)
VALUES	(3 , 16), --Sales Unit CA Trading
		(4 , 16), --Sales Unit TX Trading
		(5 , 16), --Sales Unit NY Trading
		(6 , 16), --Sales Unit FL Trading
		(8 , 16), --Sales Unit CA E-Commerce
		(9 , 16), --Sales Unit NY E-Commerce
		(10, 16), --Sales Unit TX E-Commerce
		(11, 16), --Sales Unit FL E-Commerce
		(13, 16), --Sales Unit NY Service
		(14, 16), --Sales Unit TX Service
		(15, 16), --Sales Unit FL Service
		(16, 16), --Sales Unit CA Service
		(19, 16), --Sales Unit HK E-Commerce
		(21, 16), --Sales Unit HK Trading
		(24, 16), --Sales Unit SG Trading
		(26, 16), --Sales Unit SG Service
		(28, 16)  --Sales Unit SG E-Commerce

--Quotes 19
INSERT INTO mapOrgObjectTypes (OrganizationID, ObjectTypeID)
VALUES	(3 , 19), --Sales Unit CA Trading
		(4 , 19), --Sales Unit TX Trading
		(5 , 19), --Sales Unit NY Trading
		(6 , 19), --Sales Unit FL Trading
		(8 , 19), --Sales Unit CA E-Commerce
		(9 , 19), --Sales Unit NY E-Commerce
		(10, 19), --Sales Unit TX E-Commerce
		(11, 19), --Sales Unit FL E-Commerce
		(13, 19), --Sales Unit NY Service
		(14, 19), --Sales Unit TX Service
		(15, 19), --Sales Unit FL Service
		(16, 19), --Sales Unit CA Service
		(19, 19), --Sales Unit HK E-Commerce
		(21, 19), --Sales Unit HK Trading
		(24, 19), --Sales Unit SG Trading
		(26, 19), --Sales Unit SG Service
		(28, 19)  --Sales Unit SG E-Commerce

--Purchase Orders 22
INSERT INTO mapOrgObjectTypes (OrganizationID, ObjectTypeID)
VALUES	(1 , 22), --Sourceability North America, LLC
		(17, 22), --Sourceability HK Ltd.
		(22, 22)  --Sourceability SG PTE. Ltd.

--Vendor RFQs 27
INSERT INTO mapOrgObjectTypes (OrganizationID, ObjectTypeID)
VALUES	(1 , 27), --Sourceability North America, LLC
		(17, 27), --Sourceability HK Ltd.
		(22, 27)  --Sourceability SG PTE. Ltd.

--Accounts 1
INSERT INTO mapOrgObjectTypes (OrganizationID, ObjectTypeID)
VALUES	(1 , 1), --Sourceability North America, LLC
		(17, 1), --Sourceability HK Ltd.
		(22, 1)  --Sourceability SG PTE. Ltd.

--OPTIONAL REFERENTIAL INTEGRITY UPDATES
/*
UPDATE SalesOrders SET OrganizationID = 6
UPDATE Quotes SET OrganizationID = 6
UPDATE PurchaseOrders SET OrganizationID = 1
UPDATE VendorRFQs SET OrganizationID = 1
*/