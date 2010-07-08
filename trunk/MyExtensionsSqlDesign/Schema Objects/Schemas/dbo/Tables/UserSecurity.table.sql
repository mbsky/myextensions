CREATE TABLE [dbo].[UserSecurity]
(
    [UserId] [int] NOT NULL,
	[PasswordHash] [char](86) NOT NULL,
	[PasswordSalt] [char](5) NOT NULL,
	[DateLastPasswordChange] [datetime] NOT NULL,
	[Question] [nvarchar](100) NOT NULL,
	[Answer] [nvarchar](100) NOT NULL,
	[LogOnCount] int NOT NULL DEFAULT 0,
	[MD5PasswordFlag] [bit] NOT NULL DEFAULT 0
);
