SET IDENTITY_INSERT lkpSourceTypes ON;
INSERT INTO lkpSourceTypes (SourceTypeID, TypeName, TypeRank, IsConfirmed, CreatedBy) VALUES (6, 'Inventory', 1, 1, 0)
INSERT INTO lkpSourceTypes (SourceTypeID, TypeName, TypeRank, IsConfirmed, CreatedBy) VALUES (7, 'Vendor Quote', 3, 1, 0)
INSERT INTO lkpSourceTypes (SourceTypeID, TypeName, TypeRank, IsConfirmed, CreatedBy) VALUES (8, 'Consignment', 4, 1, 0)
INSERT INTO lkpSourceTypes (SourceTypeID, TypeName, TypeRank, IsConfirmed, CreatedBy) VALUES (9, 'Outside Offer', 5, 0, 0)
INSERT INTO lkpSourceTypes (SourceTypeID, TypeName, TypeRank, IsConfirmed, CreatedBy) VALUES (10, 'Excess', 6, 0, 0)
SET IDENTITY_INSERT lkpSourceTypes OFF;