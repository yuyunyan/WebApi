CREATE TABLE [dbo].[mapAccountProjects]
(
	[AccountID] INT  NOT NULL , 
    [ProjectID] INT NOT NULL, 
    PRIMARY KEY ([ProjectID], [AccountID]), 
    CONSTRAINT [FK_mapAccountProjects_Accounts] FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID), 
    CONSTRAINT [FK_mapAccountProjects_Projects] FOREIGN KEY (ProjectID) REFERENCES Projects(ProjectID)
)
