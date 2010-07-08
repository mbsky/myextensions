CREATE PROCEDURE [dbo].[usp_UsersInRoles_GetUsersInRole]
   @RoleName nvarchar(100) 
AS
	SELECT U.UserName
	FROM UsersInRoles AS UR 
	INNER JOIN [Users] U
	ON UR.UserId = U.UserId
	WHERE UR.RoleName=@RoleName;