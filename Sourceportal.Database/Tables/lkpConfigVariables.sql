CREATE TABLE [dbo].[lkpConfigVariables]
(
	[ConfigName] VARCHAR(50) NOT NULL PRIMARY KEY, 
    [ConfigDescription] VARCHAR(500) NOT NULL, 
    [ConfigValue] VARCHAR(50) NOT NULL
)
