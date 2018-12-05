/****** Object:  StoredProcedure [dbo].[uspConfigVariablesGet]    Script Date: 12/04/17 9:21:07 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[uspConfigVariablesGet]
	@configName varchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ConfigValue FROM lkpConfigVariables WHERE ConfigName LIKE '%' +@configName + '%'
END
