
-- --------------------------------------------------
-- Date Created: 01/26/2010 00:19:35
-- Generated from EDMX file: B:\Workspace\Dev\Fab\Server\src\Fab.Server\Core\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
SET ANSI_NULLS ON;
GO

--USE [Database]
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]')
GO

-- --------------------------------------------------
-- Dropping existing FK constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AccountPosting]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posting] DROP CONSTRAINT [FK_AccountPosting]
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transaction] DROP CONSTRAINT [FK_CategoryTransaction]
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Category] DROP CONSTRAINT [FK_CategoryUser]
GO
IF OBJECT_ID(N'[dbo].[FK_JournalPosting]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posting] DROP CONSTRAINT [FK_JournalPosting]
GO
IF OBJECT_ID(N'[dbo].[FK_JournalTypeJournal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Journal] DROP CONSTRAINT [FK_JournalTypeJournal]
GO
IF OBJECT_ID(N'[dbo].[FK_PostingAssetType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posting] DROP CONSTRAINT [FK_PostingAssetType]
GO
IF OBJECT_ID(N'[dbo].[FK_Transaction_inherits_Journal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Transaction] DROP CONSTRAINT [FK_Transaction_inherits_Journal]
GO
IF OBJECT_ID(N'[dbo].[FK_UserAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Account] DROP CONSTRAINT [FK_UserAccount]
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Account]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Account];
GO
IF OBJECT_ID(N'[dbo].[AssetType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AssetType];
GO
IF OBJECT_ID(N'[dbo].[Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Category];
GO
IF OBJECT_ID(N'[dbo].[Journal]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Journal];
GO
IF OBJECT_ID(N'[dbo].[Transaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transaction];
GO
IF OBJECT_ID(N'[dbo].[JournalType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JournalType];
GO
IF OBJECT_ID(N'[dbo].[Posting]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Posting];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'User'
CREATE TABLE [dbo].[User] (
    [Id] uniqueidentifier  NOT NULL  DEFAULT (newid()),
    [Login] nvarchar(50)  NOT NULL,
    [Password] nvarchar(256)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [Registered] datetime  NOT NULL  DEFAULT (getdate()),
    [LastAccess] datetime  NULL,
    [IsDisabled] bit  NOT NULL  DEFAULT (0)
);
GO
-- Creating table 'Account'
CREATE TABLE [dbo].[Account] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Created] datetime  NOT NULL  DEFAULT (getdate()),
    [IsDeleted] bit  NOT NULL  DEFAULT (0),
    [User_Id] uniqueidentifier  NOT NULL
);
GO
-- Creating table 'AssetType'
CREATE TABLE [dbo].[AssetType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO
-- Creating table 'Posting'
CREATE TABLE [dbo].[Posting] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL  DEFAULT (getdate()),
    [Amount] money  NOT NULL,
    [Account_Id] int  NOT NULL,
    [AssetType_Id] int  NOT NULL,
    [Journal_Id] int  NOT NULL
);
GO
-- Creating table 'Journal'
CREATE TABLE [dbo].[Journal] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JournalType_Id] int  NOT NULL
);
GO
-- Creating table 'JournalType'
CREATE TABLE [dbo].[JournalType] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO
-- Creating table 'Category'
CREATE TABLE [dbo].[Category] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [User_Id] uniqueidentifier  NOT NULL
);
GO
-- Creating table 'Transaction'
CREATE TABLE [dbo].[Transaction] (
    [Id] int  NOT NULL,
    [Quantity] smallmoney  NULL,
    [Price] money  NULL,
    [Comment] nvarchar(256)  NULL,
    [IsDeleted] bit  NOT NULL  DEFAULT (0),
    [Category_Id] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all Primary Key Constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'User'
ALTER TABLE [dbo].[User] WITH NOCHECK 
ADD CONSTRAINT [PK_User]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Id] in table 'Account'
ALTER TABLE [dbo].[Account] WITH NOCHECK 
ADD CONSTRAINT [PK_Account]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Id] in table 'AssetType'
ALTER TABLE [dbo].[AssetType] WITH NOCHECK 
ADD CONSTRAINT [PK_AssetType]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Id] in table 'Posting'
ALTER TABLE [dbo].[Posting] WITH NOCHECK 
ADD CONSTRAINT [PK_Posting]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Id] in table 'Journal'
ALTER TABLE [dbo].[Journal] WITH NOCHECK 
ADD CONSTRAINT [PK_Journal]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Id] in table 'JournalType'
ALTER TABLE [dbo].[JournalType] WITH NOCHECK 
ADD CONSTRAINT [PK_JournalType]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Id] in table 'Category'
ALTER TABLE [dbo].[Category] WITH NOCHECK 
ADD CONSTRAINT [PK_Category]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Id] in table 'Transaction'
ALTER TABLE [dbo].[Transaction] WITH NOCHECK 
ADD CONSTRAINT [PK_Transaction]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO

-- --------------------------------------------------
-- Creating all Foreign Key Constraints
-- --------------------------------------------------

-- Creating foreign key on [User_Id] in table 'Account'
ALTER TABLE [dbo].[Account] WITH NOCHECK 
ADD CONSTRAINT [FK_UserAccount]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [Account_Id] in table 'Posting'
ALTER TABLE [dbo].[Posting] WITH NOCHECK 
ADD CONSTRAINT [FK_AccountPosting]
    FOREIGN KEY ([Account_Id])
    REFERENCES [dbo].[Account]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [AssetType_Id] in table 'Posting'
ALTER TABLE [dbo].[Posting] WITH NOCHECK 
ADD CONSTRAINT [FK_PostingAssetType]
    FOREIGN KEY ([AssetType_Id])
    REFERENCES [dbo].[AssetType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [Journal_Id] in table 'Posting'
ALTER TABLE [dbo].[Posting] WITH NOCHECK 
ADD CONSTRAINT [FK_JournalPosting]
    FOREIGN KEY ([Journal_Id])
    REFERENCES [dbo].[Journal]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [JournalType_Id] in table 'Journal'
ALTER TABLE [dbo].[Journal] WITH NOCHECK 
ADD CONSTRAINT [FK_JournalTypeJournal]
    FOREIGN KEY ([JournalType_Id])
    REFERENCES [dbo].[JournalType]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [Category_Id] in table 'Transaction'
ALTER TABLE [dbo].[Transaction] WITH NOCHECK 
ADD CONSTRAINT [FK_CategoryTransaction]
    FOREIGN KEY ([Category_Id])
    REFERENCES [dbo].[Category]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [User_Id] in table 'Category'
ALTER TABLE [dbo].[Category] WITH NOCHECK 
ADD CONSTRAINT [FK_CategoryUser]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[User]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [Id] in table 'Transaction'
ALTER TABLE [dbo].[Transaction] WITH NOCHECK 
ADD CONSTRAINT [FK_Transaction_inherits_Journal]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Journal]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------