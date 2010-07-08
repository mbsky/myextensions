CREATE PROCEDURE [dbo].[usp_UsersInRoles_FindUsersInRole]
	@RoleName nvarchar(100) , 
	@UserName  nvarchar(100) 
AS
	SELECT U.UserName 
	FROM UsersInRoles AS UR 
	INNER JOIN [Users] U
	ON UR.UserId = U.UserId
	WHERE UR.RoleName=@RoleName 
	AND U.UserName LIKE @UserName