﻿CREATE FUNCTION [dbo].[fnIsValidBitWiseInt]
(
	@BitValue INT = NULL
)
RETURNS BIT
AS
BEGIN
	DECLARE @bitwise BIGINT =  4294967295

	IF EXISTS (SELECT 1 WHERE @bitwise & @BitValue = @BitValue)
		RETURN 1
	RETURN 0
END