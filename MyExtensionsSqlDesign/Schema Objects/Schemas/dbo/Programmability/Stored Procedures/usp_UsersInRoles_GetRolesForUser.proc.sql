CREATE PROCEDURE [dbo].[usp_UsersInRoles_GetRolesForUser]
   @UserName nvarchar(100) 
AS
	SELECT UR.RoleName
	FROM UsersInRoles AS UR 
	INNER JOIN [Users] U
	ON UR.UserId = U.UserId
	WHERE U.UserName=@UserName;