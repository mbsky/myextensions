CREATE PROCEDURE [dbo].[usp_Users_Insert]
	@UserName nvarchar(100), 
	@Email varchar(100), 
	@Enabled bit,
	@PasswordHash char(86),
	@PasswordSalt char(5),
	@Question nvarchar(100),
	@Answer nvarchar(100)
AS

    DECLARE @UserId int

	INSERT INTO [dbo].[Users]
	(UserName, Email, Comment, Enabled, DateCreated, DateLastLogin, DateLastActivity) 
	VALUES (@UserName, @Email, NULL, @Enabled, GETDATE(), NULL, NULL)
	
	SET @UserId = @@IDENTITY
	
    INSERT INTO [dbo].[UserSecurity] (UserId,PasswordHash, PasswordSalt, Question, Answer, DateLastPasswordChange) VALUES (@UserId, @PasswordHash, @PasswordSalt, @Question, @Answer, GETDATE())