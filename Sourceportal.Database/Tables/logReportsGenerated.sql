﻿CREATE TABLE logReportsGenerated(LogID INT IDENTITY(1,1), Created DATETIME DEFAULT GETUTCDATE(), UserId INT DEFAULT NULL, ReportTitle VARCHAR(256) DEFAULT NULL)