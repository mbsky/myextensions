CREATE PROCEDURE [dbo].[usp_UsersInRoles_Insert]
	@UserName nvarchar(100), 
	@RoleName nvarchar(100)
AS
    DECLARE @UserId INT
    SELECT @UserId = UserId FROM [Users] WHERE UserName = @UserName
    IF @UserId IS NOT NULL
	INSERT INTO UsersInRoles (UserId, RoleName) VALUES (@UserId, @RoleName)