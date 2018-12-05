/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.06.22
   Description:	Gets parent organization data from salesorder
   Usage:		Select top 100* FROM [vwSalesOrderOrganization] where salesOrderID = 100007 and versionID = 2

   Revision History:

   Return Codes:
   ============================================= */
CREATE VIEW [dbo].[vwSalesOrderOrganization]
AS
	
	SELECT A.OrganizationID TopOrganizationID
		, SO.SalesOrderID
		, SO.VersionID
		, O.Name OrganizationName
		, L.Name
		, CO.FirstName
		, CO.LastName
		, CO.OfficePhone
		, CO.MobilePhone
		, CO.Fax
		, CO.Email
		, L.Address1
		, L.Address2
		, L.Address4
		, L.HouseNumber
		, L.Street
		, L.City
		, L.StateID
		, L.LocationTypeID
		, S.StateName
		, S.StateCode
		, L.CountryID
		, C.CountryName
		, L.PostalCode
		, L.Created
		, L.CreatedBy
		, L.IsDeleted
		, A.CompanyTypeID
		, A.AccountID
		, O.BankName
		, O.BranchName
		, O.USDAccount
		, O.EURAccount
		, O.SwiftAccount
		, O.RoutingNumber
	FROM SalesOrders SO
	INNER JOIN Accounts A ON A.OrganizationID = dbo.fnGetRecursiveParentOrgID(SO.OrganizationID) AND A.IsSourceability = 1
	INNER JOIN Organizations O on O.OrganizationID = A.OrganizationID
	INNER JOIN Locations L on L.AccountID = A.AccountID
	LEFT OUTER JOIN Contacts CO on CO.AccountID = A.AccountID AND CO.IsActive = 1
	LEFT OUTER JOIN States S on S.StateID = L.StateID
	INNER JOIN Countries C on C.CountryID = L.CountryID
	AND L.IsDeleted = 0
GO


