CREATE TABLE [dbo].[mapOrgObjectTypes]
(
	[OrganizationID] INT NOT NULL , 
    [ObjectTypeID] INT NOT NULL, 
    PRIMARY KEY ([ObjectTypeID], [OrganizationID])
)
