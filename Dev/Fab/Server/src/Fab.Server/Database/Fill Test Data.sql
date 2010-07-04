-- --------------------------------------------------
-- Date Created: 04/07/2010 12:26:00
-- Add test user with 4 default accounts for each of AssetTypes and 4 default categories
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
--USE [Database]
--GO

-- Create test user
INSERT INTO [dbo].[Users] ([Id], [Login], [Password], [IsDisabled]) VALUES ('7F06BFA6-B675-483C-9BF3-F59B88230382', 'default', 'test', 0)
GO

-- Create default user accounts for each of AssetType
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('UAH account', '7F06BFA6-B675-483C-9BF3-F59B88230382', 1)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('USD account', '7F06BFA6-B675-483C-9BF3-F59B88230382', 2)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('EUR account', '7F06BFA6-B675-483C-9BF3-F59B88230382', 3)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('RUR account', '7F06BFA6-B675-483C-9BF3-F59B88230382', 4)
GO

-- Create default categories for default user 
INSERT INTO [dbo].[Categories] ([Name], [CategoryType], [User_Id]) VALUES ('Food',     1, '7F06BFA6-B675-483C-9BF3-F59B88230382')
INSERT INTO [dbo].[Categories] ([Name], [CategoryType], [User_Id]) VALUES ('Travel',   1, '7F06BFA6-B675-483C-9BF3-F59B88230382')
INSERT INTO [dbo].[Categories] ([Name], [CategoryType], [User_Id]) VALUES ('Presents', 1, '7F06BFA6-B675-483C-9BF3-F59B88230382')
INSERT INTO [dbo].[Categories] ([Name], [CategoryType], [User_Id]) VALUES ('Unknown',  1, '7F06BFA6-B675-483C-9BF3-F59B88230382')
GO