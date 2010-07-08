ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [UK_Users_UserName]
UNIQUE ([UserName])