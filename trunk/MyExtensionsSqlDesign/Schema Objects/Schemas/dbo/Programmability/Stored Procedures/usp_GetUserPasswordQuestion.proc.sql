CREATE PROCEDURE [dbo].[usp_GetUserPasswordQuestion]
	@UserId int , 
	@Question NVARCHAR(100) OUTPUT
AS
	SELECT @Question = Question
	FROM [dbo].[UserSecurity]
	WHERE UserId = @UserId