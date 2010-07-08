CREATE PROCEDURE [dbo].[usp_Users_Delete]
	@UserName nvarchar(100)
AS
    DECLARE @UserId INT
    SELECT @UserId = UserId FROM [Users] 
    WHERE UserName = @UserName
    
    BEGIN TRAN
    
    DELETE FROM UsersInRoles
	WHERE UserId=@UserId;
	
	DELETE FROM UserSecurity
	WHERE UserId=@UserId;
    
	DELETE FROM Users 
	WHERE UserId=@UserId;
	
	COMMIT TRAN