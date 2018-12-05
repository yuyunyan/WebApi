SET IDENTITY_INSERT lkpObjectTypes ON

INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (1, NULL, 'Account', 'A customer, vendor or supplier')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (2, 1, 'Contact', 'A business contact person for an Account')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (3, NULL, 'Region', 'A region of the world for Sales purposes')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (4, NULL, 'Account Hierarchy', 'The SAP Hierarchy representing a Company; Accounts belong to Hierarchies.  Has a Region')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (8, NULL, 'Navigation', 'Navigation')

INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (16, NULL, 'Sales Order', 'Sales Orders')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (17, 16, 'Sales Order Line', 'A line item on a Sales Order')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (18, 16, 'Sales Order Extra', 'An extra line item on a Sales Order')

INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (19, NULL, 'Quote', 'Quote to a customer')
INSERT INTO lkpObjectTypes (ObjectTypeID,ParentID,  ObjectName, [Description])
VALUES (20, 19, 'Quote Line', 'A line item on a Quote')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (21, 19, 'Quote Extra', 'An extra line item on a Quote')

INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (22, NULL, 'Purchase Order', 'Purchase Order (PO) for our Vendors')
INSERT INTO lkpObjectTypes (ObjectTypeID,ParentID,  ObjectName, [Description])
VALUES (23, 22, 'Purchase Order Line', 'A line item on a Purchase Order')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (24, 22, 'Purchase Order Extra', 'An extra line item on a Purhcase Order')

INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (25, NULL, 'Item List', 'Lists of Items like a BOM')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (26, 25, 'Item List Line', 'One line on an Item List')

INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (27, NULL, 'Vendor RFQ', 'A Request For Quote we send to our Vendors')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (28, 27, 'Vendor RFQ Line', 'A line on a Vendor RFQ')

INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (32, NULL, 'User', 'An application user')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (64, NULL, 'User Group', 'A collection of users')

INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (100, NULL, 'Item Group', 'A high-level grouping of commodities')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (101, 100, 'Commodity', 'A type of electronic component')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (102, NULL, 'Manufacturer', 'A company that manufactures components')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (103, NULL, 'Item', 'A specific electronic part')

INSERT INTO lkpObjectTypes (ObjectTypeID, ObjectName, [Description])
VALUES (104,'Inspection', 'QC Inspection')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (105, 104, 'Answer', 'QC Answer that pertains to an inspection')

INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (106, NULL, 'Source', 'A potential source for parts (Vendor Quote, Outside offers, etc)')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (107, NULL, 'Item Stock', 'A part stored in inventory')
INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (108, NULL, 'Source Join', 'The Relationship between a Source and another object')

INSERT INTO lkpObjectTypes (ObjectTypeID, ParentID, ObjectName, [Description])
VALUES (110, NULL, 'Company Type', 'Company Type of an Account')

SET IDENTITY_INSERT lkpObjectTypes OFF

