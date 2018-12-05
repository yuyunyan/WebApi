--Buyer's PO Limit
INSERT INTO lkpStateEngineDynamicValues (ValueName, ValueDescription, SQLQuery, CreatedBy)
VALUES ('Owner''s Purchasing Limit', 'The highest purchasing limit of all owners on a Purchase Order', 'SELECT MAX(b.POLimit) ''Result''
FROM mapOwnership o
INNER JOIN mapUserBuyers b ON o.OwnerID = b.UserID
WHERE ObjectID = @ObjectID
AND ObjectTypeID = 22
AND o.IsDeleted = 0
AND b.IsDeleted = 0', 0)

