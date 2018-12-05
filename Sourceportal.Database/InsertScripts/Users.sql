
--Add Middleware user
SET IDENTITY_INSERT dbo.Users ON;  
GO

INSERT INTO dbo.Users(UserID, FirstName ) VALUES (999999, 'Middleware');  
GO