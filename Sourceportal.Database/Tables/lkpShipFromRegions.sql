CREATE TABLE [dbo].[lkpShipFromRegions]
(
	[ShipFromRegionID] INT           NOT NULL IDENTITY PRIMARY KEY, 
    [RegionName]	  NVARCHAR(200)  NOT NULL,
	[OrganizationID]  INT		     NOT NULL,
	[CountryID]		  INT			 NOT NULL,
	[Created]         DATETIME       DEFAULT GETUTCDATE() NOT NULL,
    [CreatedBy]       INT            NOT NULL,
    [Modified]        DATETIME       NULL,
    [ModifiedBy]      INT            NULL,
    [IsDeleted]       BIT            DEFAULT 0 NOT NULL,
)
