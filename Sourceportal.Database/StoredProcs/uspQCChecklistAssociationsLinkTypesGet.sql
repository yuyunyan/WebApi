
/* =============================================
   Author:		Corey Tyrrell
   Create date: 2017.08.24
   Description:	Deletes Checklist Associations from mapQCChecklistsJoin tbl using ChecklistID to discern
   Usage: EXEC uspQCChecklistAssociationsLinkTypesGet

   Revision History: 
		2017.10.06	JT	Delete vendor from lkpAccountTypes
		2018.11.01	NA	Added CompanyType
   Return Codes:
   ============================================= */


CREATE PROCEDURE [dbo].[uspQCChecklistAssociationsLinkTypesGet]

AS
BEGIN
	SET NOCOUNT ON;
	  SELECT ObjectName AS ObjectName
	  , ObjectTypeID AS ObjectTypeID
	  , 0 AS AccountTypeID
	  FROM [SourcePortal2_TEST].[dbo].lkpObjectTypes
	  WHERE ObjectTypeID in (101, 102, 103, 110)

	  UNION 

	  SELECT Name AS ObjectName
	  , 1 AS ObjectTypeID
	  , AccountTypeID AS AccountTypeID
	  FROM [SourcePortal2_TEST].[dbo].lkpAccountTypes	 
END