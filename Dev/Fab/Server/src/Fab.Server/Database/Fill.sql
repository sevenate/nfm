-- --------------------------------------------------
-- Date Created: 01/26/2010 00:19:35
-- Fill AssetTypes, Users and Accounts tables with initial data
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
INSERT INTO [dbo].[Users] ([Id], [Login], [Password], [IsDisabled]) VALUES ('6184b6dd-26d0-4d06-ba2c-95c850ccfebe', 'CASH', 'CASH', 1)
GO

-- Create system cash accounts for each of AssetType
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('CASH UAH', '6184b6dd-26d0-4d06-ba2c-95c850ccfebe', 1)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('CASH USD', '6184b6dd-26d0-4d06-ba2c-95c850ccfebe', 2)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('CASH EUR', '6184b6dd-26d0-4d06-ba2c-95c850ccfebe', 3)
INSERT INTO [dbo].[Accounts] ([Name], [User_Id], [AssetType_Id]) VALUES ('CASH RUR', '6184b6dd-26d0-4d06-ba2c-95c850ccfebe', 4)
GO