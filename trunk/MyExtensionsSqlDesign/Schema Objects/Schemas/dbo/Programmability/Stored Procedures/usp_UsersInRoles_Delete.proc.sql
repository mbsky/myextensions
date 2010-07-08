CREATE PROCEDURE [dbo].[usp_UsersInRoles_Delete]
	@RoleName nvarchar(100) , 
	@UserName  nvarchar(100) 
AS
    DECLARE @UserId INT
    SELECT @UserId = UserId FROM [Users] WHERE UserName = @UserName
    IF @UserId IS NOT NULL
	DELETE FROM UsersInRoles
	WHERE UserId=@UserId AND RoleName=@RoleName;