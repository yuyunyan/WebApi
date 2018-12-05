INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (19, 'Quote Total', 'The total Revenue for a Quote', 
'SELECT SUM(Qty * Price) ''Result''
FROM vwQuoteLines
WHERE QuoteID = @ObjectID', 
'N', 0)

INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (19, 'TEST Quote Customer Name Contains F', 'TEST The name of the customer on the quote contains the letter F', 
'SELECT q.QuoteID ''ObjectID'', a.AccountName ''Result''
FROM vwQuotes q
INNER JOIN Accounts a ON q.AccountID = a.AccountID
WHERE a.AccountName LIKE ''%f%''
AND q.QuoteID = @ObjectID', 
'B', 0)

INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (19, 'Quote Type', 'Quote Type', 
'SELECT q.QuoteID ''ObjectID'', t.TypeName ''Result''
FROM vwQuotes q
INNER JOIN lkpQuoteTypes t ON q.QuoteTypeID = t.QuoteTypeID
WHERE q.QuoteID = @ObjectID', 
'C', 0)

INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (19, 'Quote Total', 'The total Revenue for a Quote', 
'SELECT SUM(Qty * Price) ''Result''
FROM vwQuoteLines
WHERE QuoteID = @ObjectID', 
'N', 0)


--TEST Quote lines with odd qty
INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (20, 'TEST Quote Line has odd Qty', 'TEST Condition: Qty value on Quote Line is an odd number',
'SELECT QuoteLineID ''ObjectID'', Qty ''Result''
	FROM vwQuoteLines
	WHERE Qty % 2 = 1
	AND QuoteID = @ObjectID',
'B', 0)

--TEST Quote lines Qty (Numeric)
INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (20, 'TEST Quote Line Qty', 'TEST Condition: Qty value on Quote Line for numerical comparison',
'SELECT QuoteLineID ''ObjectID'', Qty ''Result''
	FROM vwQuoteLines	
	AND QuoteID = @ObjectID',
'N', 0)

--TEST Quote lines Qty (Contains)
INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (20, 'TEST Quote Line Qty', 'TEST Condition: Qty value on Quote Line for numerical comparison',
'SELECT QuoteLineID ''ObjectID'', Qty ''Result''
	FROM vwQuoteLines
	AND QuoteID = @ObjectID',
'C', 0)

--Quote lines not matched to Sources
INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (19, 'Lines not matched to sources', 'Quote has lines that are not matched to at least one Source', 
'SELECT CASE WHEN COUNT(QuoteLineID) > 0 THEN ''True'' ELSE ''False'' END AS ''Result'' FROM (
	SELECT ql.QuoteLineID, COUNT(s.SourceID) ''Matches''
	FROM vwQuoteLines ql
	LEFT OUTER JOIN mapSourcesJoin s ON ql.QuoteLineID = s.ObjectID AND s.ObjectTypeID = 20 AND s.IsDeleted = 0 AND s.IsMatch = 1
	WHERE ql.QuoteID = @ObjectID
	GROUP BY ql.QuoteLineID
	HAVING COUNT(s.SourceID) = 0) z', 
'B', 0)

--Net due days on a PO, based on the payment terms.
INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (22, 'PO Net Due Days', 'Number of days payment is due for the PO', 
'SELECT NetDueDays ''Result''
	FROM codes.lkpPaymentTerms t
	INNER JOIN PurchaseOrders p ON t.PaymentTermID = p.PaymentTermID 
	AND p.PurchaseOrderID = @ObjectID', 
'N', 0)

--PO Vendor is Approved
INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (22, 'Vendor is Approved', 'Vendor on the PO is active, approved by Mgmt and Accounting, and not blacklisted',
'SELECT CASE WHEN (s.AccountIsActive = 1 AND s.AccountIsFinanceApproved = 1 AND s.AccountIsMgmtApproved = 1 AND s.AccountIsBlacklisted = 0) THEN ''True'' ELSE ''False'' END ''Result''
	FROM vwPurchaseOrders p
	INNER JOIN Accounts a ON p.AccountID = a.AccountID
	INNER JOIN mapAccountTypes t ON a.AccountID = t.AccountID AND t.IsDeleted = 0 AND t.AccountTypeID = 1
	INNER JOIN lkpAccountStatuses s ON t.AccountStatusID = s.AccountStatusID
	WHERE p.PurchaseOrderID = @ObjectID',
'B', 0)

--PO Total
INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (22, 'Purchase Order Total', 'The total cost of the PO', 
'SELECT ISNULL(SUM(Qty * Cost), 0) ''Result''
	FROM vwPurchaseOrderLines pol
	INNER JOIN lkpStatuses s ON pol.StatusID = s.StatusID
	WHERE PurchaseOrderID = @ObjectID
	AND s.IsCanceled = 0',
'N', 0)

--PO contains Spec Buy lines
INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (22, 'Contains spec buy line(s)', 'The PO contains at least one line flagged as a Spec Buy',
'SELECT CASE WHEN MAX(CAST(p.IsSpecBuy AS INT)) = 1 THEN ''True'' ELSE ''False'' END ''Result''
	FROM vwPurchaseOrderLines p
	WHERE PurchaseOrderID = @ObjectID',
'B', 0)

--SO Revenue Total
INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (16, 'Sales Order Total', 'The total revenue of the sales order',
'SELECT ISNULL(SUM(Qty * Price), 0) ''Result''
	FROM vwSalesOrderLines sol
	INNER JOIN lkpStatuses s ON sol.StatusID = s.StatusID
	WHERE sol.SalesOrderID = @ObjectID
	AND s.IsCanceled = 0',
'N', 0)

--SO Gross Profit
INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (16, 'Sales Gross Profit', 'The total gross profit of the sales order',
'SELECT ISNULL(SUM(Qty * (Price - Cost)), 0) ''Result''
	FROM vwSalesOrderLines sol
	INNER JOIN lkpStatuses s ON sol.StatusID = s.StatusID
	WHERE sol.SalesOrderID = @ObjectID
	AND s.IsCanceled = 0',
'N', 0)

--SO Margin Percentage
INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (16, 'Sales Margin Percent', 'The total GPM % of the sales order',
'SELECT ISNULL((SUM(Qty * Price) - SUM(Qty * Cost)) / SUM(Qty * Price) * 100, 0) ''Result''
	FROM vwSalesOrderLines sol
	INNER JOIN lkpStatuses s ON sol.StatusID = s.StatusID
	WHERE sol.SalesOrderID = @ObjectID
	AND s.IsCanceled = 0',
'N', 0)

--Sales Order Customer is Approved
INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (16, 'Customer is Approved', 'Customer on the SO is active, approved by Mgmt and Accounting, and not blacklisted',
'SELECT CASE WHEN (s.AccountIsActive = 1 AND s.AccountIsFinanceApproved = 1 AND s.AccountIsMgmtApproved = 1 AND s.AccountIsBlacklisted = 0) THEN ''True'' ELSE ''False'' END ''Result''
	FROM vwSalesOrders so
	INNER JOIN Accounts a ON so.AccountID = a.AccountID
	INNER JOIN mapAccountTypes t ON a.AccountID = t.AccountID AND t.IsDeleted = 0 AND t.AccountTypeID = 1
	INNER JOIN lkpAccountStatuses s ON t.AccountStatusID = s.AccountStatusID
	WHERE so.SalesOrderID = @ObjectID',
'B', 0)

--PO Allocated to SO with mismatched ItemID
INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (22, 'SO Allocation MPN Mismatch', 'One or more PO lines allocated to an SO with mismatched part numbers',
'SELECT CASE WHEN SUM(CASE WHEN (sol.ItemID != pol.ItemID) THEN 1 ELSE 0 END) > 0 THEN ''True'' ELSE ''False'' END ''Result''
	FROM vwPurchaseOrderLines pol
	INNER JOIN mapSOPOAllocation sopo ON pol.POLineID = sopo.POLineID AND sopo.IsDeleted = 0
	INNER JOIN vwSalesOrderLines sol ON sopo.SOLineID = sol.SOLineID
	WHERE pol.PurchaseOrderID = @ObjectID',
'B', 0)


--PO created with existing unallocated inventory
INSERT INTO lkpStateEngineConditions (ObjectTypeID, ConditionName, ConditionDescription, SQLQuery, ComparisonType, CreatedBy)
VALUES (22, 'Items have Unallocated Stock', 'One or more items on the PO have unallocated stock in inventory',
'SELECT CASE WHEN ISNULL(SUM(Results),0) = 0 THEN ''False'' ELSE ''True'' END ''Result'' 
	FROM (
	SELECT COUNT(inv.ItemID) ''Results''
	FROM vwPurchaseOrderLines pol
	INNER JOIN vwItemInventoryWithFulfillment inv ON pol.ItemID = inv.ItemID
	WHERE pol.PurchaseOrderID = @ObjectID
	GROUP BY inv.ItemID
	HAVING SUM(inv.InventoryQty) - SUM(ISNULL(inv.FulfilledQty,0)) > 0) z',
'B', 0)