/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.06.18
   Description:	Retrieves oragization address informatiom for saless order
   Usage:		EXEC uspSalesOrderOrganizationGet 100007, 2
   Return Codes:
				
   Revision History:
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspSalesOrderOrganizationGet]
	@SalesOrderID INT = NULL,
	@VersionID INT = NULL,
	@UserID INT = NULL
AS
BEGIN
	SELECT top 1 OrganizationName
		, ISNULL(O.Address1,'') Address1
		, ISNULL(O.Address2,'') Address2
		, ISNULL(O.Address4,'') Address4
		, ISNULL(O.HouseNumber,'') HouseNumber
		, ISNULL(O.Street,'') Street
		, ISNULL(O.City,'') City
		, ISNULL(O.StateName,'') StateName
		, ISNULL(O.StateCode,'') StateCode
		, ISNULL(O.CountryName,'') CountryName
		, ISNULL(O.PostalCode,'') PostalCode
		, ISNULL(O.BranchName,'') BranchName
		, ISNULL(O.BankName,'') BankName
		, ISNULL(O.USDAccount,'') USDAccount
		, ISNULL(O.EURAccount,'') EURAccount
		, ISNULL(O.SwiftAccount,'') SwiftAccount
		, ISNULL(O.RoutingNumber,'') RoutingNumber
		, ISNULL(O.OfficePhone,'') OfficePhone
		, ISNULL(O.MobilePhone,'') MobilePhone
		, ISNULL(O.Fax,'') Fax
		, ISNULL(O.Email,'') Email
	FROM vwSalesOrderOrganization O
	WHERE SalesOrderID = @SalesOrderID
	AND VersionID = @VersionID
	AND LocationTypeID & 1 = 1
	ORDER BY Created DESC
END