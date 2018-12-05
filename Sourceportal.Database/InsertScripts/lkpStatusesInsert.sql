--Quotes
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (19, 'Not Quoted', 1, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (19, 'Quoted', 0, 0, 1, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (19, 'Won', 0, 0, 0, 1, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (19, 'Lost', 0, 0, 0, 1, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (19, 'Canceled', 0, 0, 0, 0, 1, 0)

--Quote Lines
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (20, 'Not-Routed', 1, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (20, 'Routed', 0, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (20, 'Won', 0, 0, 0, 1, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (20, 'Lost', 0, 0, 0, 1, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (20, 'Canceled', 0, 0, 0, 0, 1, 0)

--Quote Extras
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (21, 'Open', 1, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (21, 'Closed', 0, 0, 0, 1, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (21, 'Canceled', 0, 0, 0, 0, 1, 0)

--Sales Orders
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (16, 'In Preparation', 1, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (16, 'Not Shipped', 0, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (16, 'Partially Shipped', 0, 1, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (16, 'Shipped', 0, 0, 1, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (16, 'Complete', 0, 0, 0, 1, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (16, 'Canceled', 0, 0, 0, 0, 1, 0)

--Sales Order Lines
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (17, 'Not Shipped', 1, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (17, 'Partially Shipped', 0, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (17, 'Shipped', 0, 0, 0, 1, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (17, 'Canceled', 0, 0, 0, 0, 1, 0)

--Sales Order Extras
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (18, 'Open', 1, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (18, 'Closed', 0, 0, 0, 1, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (18, 'Canceled', 0, 0, 0, 0, 1, 0)

--Purchase Orders
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy, ExternalID)
VALUES (22, 'Open', 1, 0, 0, 0, 0, 0, '6')
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy, ExternalID)
VALUES (22, 'Hold', 0, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy, ExternalID)
VALUES (22, 'Received', 0, 0, 0, 1, 0, 0, '9')
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy, ExternalID)
VALUES (22, 'Finished', 0, 0, 0, 1, 0, 0, '10')
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy, ExternalID)
VALUES (22, 'Canceled', 0, 0, 0, 0, 1, 0, '8')

--Purchase Order Lines
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (23, 'Open', 1, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (23, 'Closed', 0, 0, 0, 1, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (23, 'Canceled', 0, 0, 0, 0, 1, 0)

--Purchase Order Extras
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (24, 'Open', 1, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (24, 'Closed', 0, 0, 0, 1, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (24, 'Canceled', 0, 0, 0, 0, 1, 0)


--Item Lists
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (25, 'Active', 1, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (25, 'Quoted', 0, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (25, 'Archived', 0, 0, 0, 1, 0, 0)

--Item List Lines
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (26, 'Open', 1, 0, 0, 0, 0, 0)

--Vendor RFQs
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (27, 'New', 1, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (27, 'Submitted', 0, 0, 1, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (27, 'Closed', 0, 0, 0, 1, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (27, 'Canceled', 0, 0, 0, 0, 1, 0)

--Vendor RFQ Lines
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (28, 'Waiting', 1, 0, 0, 0, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (28, 'Responded', 0, 0, 0, 1, 0, 0)
INSERT INTO lkpStatuses (ObjectTypeID, StatusName, isDefault, isAwaitingApproval, IsApproved, IsComplete, IsCanceled, CreatedBy)
VALUES (28, 'Canceled', 0, 0, 0, 0, 1, 0)