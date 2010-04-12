-- --------------------------------------------------
-- Date Created: 03/19/2010 02:28:20
-- Add default categories for default user
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
--USE [Database]
--GO

-- Create default categories for default user 
INSERT INTO [dbo].[Categories] ([Name], [User_Id]) VALUES ('Еда' ,'7F06BFA6-B675-483C-9BF3-F59B88230382')
INSERT INTO [dbo].[Categories] ([Name], [User_Id]) VALUES ('Транспорт' ,'7F06BFA6-B675-483C-9BF3-F59B88230382')
INSERT INTO [dbo].[Categories] ([Name], [User_Id]) VALUES ('Подарки' ,'7F06BFA6-B675-483C-9BF3-F59B88230382')
INSERT INTO [dbo].[Categories] ([Name], [User_Id]) VALUES ('Неизвестно' ,'7F06BFA6-B675-483C-9BF3-F59B88230382')
GO