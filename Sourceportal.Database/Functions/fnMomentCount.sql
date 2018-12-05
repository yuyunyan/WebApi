/*	=============================================
	Author: Aaron Rodecker
	Create Date: 2018.03.09
	Usage: SELECT dbo.fnMomentCount('12:12:12', '12:12:16')
	Description: A "moment" in time is a time that uses two or less digits (not counting zeroes) to display a time in HH:MM:SS format.
				For example, a "moment" could be 12:21:21 because it uses only two digits (2 and 1).
				12:20:01 could also be a "moment" because it only uses 1 and 2, excluding 0.
				However 12:22:23 would not be a "moment" because it uses 1, 2, and 3.
				This function returns the moment Count

	============================================*/

CREATE FUNCTION dbo.fnMomentCount(
	@StartTime VARCHAR(10)	--"HH:MM:SS"
	, @EndTime VARCHAR(10)	--"HH:MM:SS"
)
RETURNS INT

AS
BEGIN
	DECLARE @Start TIME = CONVERT(TIME, @StartTime)
	DECLARE @End TIME = CONVERT(TIME, @EndTime) 
	DECLARE @CurrentSecond INT = 0
	DECLARE @SecondsBetween INT = DATEDIFF(SECOND, @Start, @End)

	DECLARE @MomentCount INT = 0
	WHILE (@CurrentSecond < @SecondsBetween)
	BEGIN

		--Declare characters table and time to loop from (excluding colons)
		DECLARE @CharTbl TABLE ([Char] VARCHAR(100))
		DECLARE @CurrentTime VARCHAR(12) = REPLACE(DATEADD(SECOND, @CurrentSecond, @Start ), ':', '')

		--Insert numbers into temp table, excluding colons
		INSERT INTO @CharTbl ([Char])
		VALUES (SUBSTRING(@CurrentTime,1,1)), (SUBSTRING(@CurrentTime,2,1)), (SUBSTRING(@CurrentTime,3,1)), (SUBSTRING(@CurrentTime,4,1)), (SUBSTRING(@CurrentTime,5,1)), (SUBSTRING(@CurrentTime,6,1))

		--Select unique characters from temp table, excluding 0's
		DECLARE @CharCount INT = (SELECT COUNT(DISTINCT [Char]) FROM @CharTbl WHERE [Char] != '0')
		
		--Check count of unique characters/append total moment
		IF (@CharCount <= 2)
			SET @MomentCount = @MomentCount + 1 --Moment Found

		SET @CurrentSecond  = @CurrentSecond + 1
	END
	RETURN @MomentCount
END