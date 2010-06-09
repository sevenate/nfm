-- --------------------------------------------------
-- Date Created: 01/26/2010 00:19:35
-- Fill AssetTypes, Users and Accounts tables
-- Add default user with 4 default accounts for each of AssetTypes and 4 default categories
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
--USE [Database]
--GO

-- Create common AssetTypes
INSERT INTO [dbo].[AssetTypes] ([Name]) VALUES ('UAH')
INSERT INTO [dbo].[AssetTypes] ([Name]) VALUES ('USD')
INSERT INTO [dbo].[AssetTypes] ([Name]) VALUES ('EUR')
INSERT INTO [dbo].[AssetTypes] ([Name]) VALUES ('RUR')
GO

-- Create system user
INSERT INTO [dbo].[Users] ([Id], [Login], [Password], [IsDisabled]) VALUES ('6184b6dd-26d0-4d06-ba2c-95c850ccfebe', 'AFK', 'AFK', 1)
GO

-- Create system cash accounts for each of AssetType
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('CASH UAH' ,'6184b6dd-26d0-4d06-ba2c-95c850ccfebe' ,1)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('CASH USD' ,'6184b6dd-26d0-4d06-ba2c-95c850ccfebe' ,2)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('CASH EUR' ,'6184b6dd-26d0-4d06-ba2c-95c850ccfebe' ,3)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('CASH RUR' ,'6184b6dd-26d0-4d06-ba2c-95c850ccfebe' ,4)
GO

-- Create default user
INSERT INTO [dbo].[Users] ([Id], [Login], [Password], [IsDisabled]) VALUES ('7F06BFA6-B675-483C-9BF3-F59B88230382', 'default', 'default', 0)
GO

-- Create default user accounts for each of AssetType
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('default UAH' ,'7F06BFA6-B675-483C-9BF3-F59B88230382' ,1)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('default USD' ,'7F06BFA6-B675-483C-9BF3-F59B88230382' ,2)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('default EUR' ,'7F06BFA6-B675-483C-9BF3-F59B88230382' ,3)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('default RUR' ,'7F06BFA6-B675-483C-9BF3-F59B88230382' ,4)
GO

-- Create default categories for default user 
INSERT INTO [dbo].[Categories] ([Name], [User_Id]) VALUES ('Еда' ,'7F06BFA6-B675-483C-9BF3-F59B88230382')
INSERT INTO [dbo].[Categories] ([Name], [User_Id]) VALUES ('Транспорт' ,'7F06BFA6-B675-483C-9BF3-F59B88230382')
INSERT INTO [dbo].[Categories] ([Name], [User_Id]) VALUES ('Подарки' ,'7F06BFA6-B675-483C-9BF3-F59B88230382')
INSERT INTO [dbo].[Categories] ([Name], [User_Id]) VALUES ('Неизвестно' ,'7F06BFA6-B675-483C-9BF3-F59B88230382')
GO