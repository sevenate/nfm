-- --------------------------------------------------
-- Date Created: 03/19/2010 02:28:20
-- Add default user with 4 default accounts for each of AssetTypes
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
--USE [Database]
--GO

-- Create default user
INSERT INTO [dbo].[Users] ([Id], [Login], [Password], [IsDisabled]) VALUES ('7F06BFA6-B675-483C-9BF3-F59B88230382', 'default', 'default', 0)
GO

-- Create default user accounts for each of AssetType
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('default UAH' ,'7F06BFA6-B675-483C-9BF3-F59B88230382' ,1)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('default USD' ,'7F06BFA6-B675-483C-9BF3-F59B88230382' ,2)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('default EUR' ,'7F06BFA6-B675-483C-9BF3-F59B88230382' ,3)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('default RUR' ,'7F06BFA6-B675-483C-9BF3-F59B88230382' ,4)
GO