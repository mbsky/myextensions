CREATE TABLE [dbo].[Users]
(
    [UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Enabled] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateLastLogin] [datetime] NULL,
	[DateLastActivity] [datetime] NULL
);
