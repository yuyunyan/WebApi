SET IDENTITY_INSERT QCChecklists ON;
INSERT INTO QCChecklists (ChecklistID, ParentChecklistID, ChecklistName, ChecklistDescription, SortOrder, CreatedBy, ChecklistTypeID) VALUES (1, 4, 'Level 01', 'Level 1 Inspection is defined as source of purchased product is fully traceable to the manufacturer.', 1, 0, 1) 
INSERT INTO QCChecklists (ChecklistID, ParentChecklistID, ChecklistName, ChecklistDescription, SortOrder, CreatedBy, ChecklistTypeID) VALUES (2, 4, 'Level 02', 'Level 2 Inspection is defined as source of purchased product is deemed "secure" from a verified Franchise Distributor for the component manufacturer.', 2, 0, 1) 
INSERT INTO QCChecklists (ChecklistID, ParentChecklistID, ChecklistName, ChecklistDescription, SortOrder, CreatedBy, ChecklistTypeID) VALUES (3, 4, 'Level 03', 'Level 3 Inspection is defined as source of purchased product is procured from all other sources including, Independent Distributors, Contract manufacturers, Brokers.', 3, 0, 1) 
INSERT INTO QCChecklists (ChecklistID, ParentChecklistID, ChecklistName, ChecklistDescription, SortOrder, CreatedBy, ChecklistTypeID) VALUES (4, NULL, 'Level 3 Checklists', 'The complete 3 level package', 1, 0, 1) 
SET IDENTITY_INSERT QCChecklists OFF;

INSERT INTO QCChecklists (ChecklistTypeID, ChecklistName, ChecklistDescription, SortOrder, CreatedBy) VALUES (1, 'Customer Special', 'A special checklist for a specific customer', 5, 0)
INSERT INTO QCChecklists (ChecklistTypeID, ChecklistName, ChecklistDescription, SortOrder, CreatedBy) VALUES (1, 'Supplier Special', 'A special checklist for a specific supplier', 6, 0)
INSERT INTO QCChecklists (ChecklistTypeID, ChecklistName, ChecklistDescription, SortOrder, CreatedBy) VALUES (1, 'Commodity Special', 'A special checklist for a specific commodity', 7, 0)
INSERT INTO QCChecklists (ChecklistTypeID, ChecklistName, ChecklistDescription, SortOrder, CreatedBy) VALUES (1, 'Item Special', 'A special checklist for a specific item', 8, 0)