
-- --------------------------------------------------
-- Date Created: 01/25/2010 02:54:01
-- Generated from EDMX file: B:\Workspace\Dev\Fab\Server\src\Fab.Server\Core\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
SET ANSI_NULLS ON;
GO

USE [Database]
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]')
GO

-- --------------------------------------------------
-- Dropping existing FK constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AccountPosting]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostingSet] DROP CONSTRAINT [FK_AccountPosting]
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JournalSet_Transaction] DROP CONSTRAINT [FK_CategoryTransaction]
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CategorySet] DROP CONSTRAINT [FK_CategoryUser]
GO
IF OBJECT_ID(N'[dbo].[FK_JournalPosting]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostingSet] DROP CONSTRAINT [FK_JournalPosting]
GO
IF OBJECT_ID(N'[dbo].[FK_JournalTypeJournal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JournalSet] DROP CONSTRAINT [FK_JournalTypeJournal]
GO
IF OBJECT_ID(N'[dbo].[FK_PostingAssetType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PostingSet] DROP CONSTRAINT [FK_PostingAssetType]
GO
IF OBJECT_ID(N'[dbo].[FK_Transaction_inherits_Journal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[JournalSet_Transaction] DROP CONSTRAINT [FK_Transaction_inherits_Journal]
GO
IF OBJECT_ID(N'[dbo].[FK_UserAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountSet] DROP CONSTRAINT [FK_UserAccount]
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AccountSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountSet];
GO
IF OBJECT_ID(N'[dbo].[AssetTypeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AssetTypeSet];
GO
IF OBJECT_ID(N'[dbo].[CategorySet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CategorySet];
GO
IF OBJECT_ID(N'[dbo].[JournalSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JournalSet];
GO
IF OBJECT_ID(N'[dbo].[JournalSet_Transaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JournalSet_Transaction];
GO
IF OBJECT_ID(N'[dbo].[JournalTypeSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JournalTypeSet];
GO
IF OBJECT_ID(N'[dbo].[PostingSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostingSet];
GO
IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [Id] int  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [Password] nvarchar(256)  NOT NULL,
    [Registered] datetime  NOT NULL,
    [IsDisabled] bit  NOT NULL,
    [Login] nvarchar(50)  NOT NULL
);
GO
-- Creating table 'AccountSet'
CREATE TABLE [dbo].[AccountSet] (
    [Id] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Created] datetime  NOT NULL,
    [IsDeleted] bit  NOT NULL,
    [User_Id] int  NOT NULL
);
GO
-- Creating table 'AssetTypeSet'
CREATE TABLE [dbo].[AssetTypeSet] (
    [Id] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO
-- Creating table 'PostingSet'
CREATE TABLE [dbo].[PostingSet] (
    [Id] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [Amount] money  NOT NULL,
    [Account_Id] int  NOT NULL,
    [AssetType_Id] int  NOT NULL,
    [Journal_Id] int  NOT NULL
);
GO
-- Creating table 'JournalSet'
CREATE TABLE [dbo].[JournalSet] (
    [Id] int  NOT NULL,
    [JournalType_Id] int  NOT NULL
);
GO
-- Creating table 'JournalTypeSet'
CREATE TABLE [dbo].[JournalTypeSet] (
    [Id] int  NOT NULL,
    [Type] nvarchar(50)  NOT NULL
);
GO
-- Creating table 'CategorySet'
CREATE TABLE [dbo].[CategorySet] (
    [Id] int  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [User_Id] int  NOT NULL
);
GO
-- Creating table 'JournalSet_Transaction'
CREATE TABLE [dbo].[JournalSet_Transaction] (
    [Comment] nvarchar(256)  NULL,
    [Price] money  NULL,
    [Quantity] smallmoney  NULL,
    [IsDeleted] bit  NOT NULL,
    [Id] int  NOT NULL,
    [Category_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all Primary Key Constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet] WITH NOCHECK 
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Id] in table 'AccountSet'
ALTER TABLE [dbo].[AccountSet] WITH NOCHECK 
ADD CONSTRAINT [PK_AccountSet]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Id] in table 'AssetTypeSet'
ALTER TABLE [dbo].[AssetTypeSet] WITH NOCHECK 
ADD CONSTRAINT [PK_AssetTypeSet]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Id] in table 'PostingSet'
ALTER TABLE [dbo].[PostingSet] WITH NOCHECK 
ADD CONSTRAINT [PK_PostingSet]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Id] in table 'JournalSet'
ALTER TABLE [dbo].[JournalSet] WITH NOCHECK 
ADD CONSTRAINT [PK_JournalSet]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Id] in table 'JournalTypeSet'
ALTER TABLE [dbo].[JournalTypeSet] WITH NOCHECK 
ADD CONSTRAINT [PK_JournalTypeSet]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Id] in table 'CategorySet'
ALTER TABLE [dbo].[CategorySet] WITH NOCHECK 
ADD CONSTRAINT [PK_CategorySet]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO
-- Creating primary key on [Id] in table 'JournalSet_Transaction'
ALTER TABLE [dbo].[JournalSet_Transaction] WITH NOCHECK 
ADD CONSTRAINT [PK_JournalSet_Transaction]
    PRIMARY KEY CLUSTERED ([Id] ASC)
    ON [PRIMARY]
GO

-- --------------------------------------------------
-- Creating all Foreign Key Constraints
-- --------------------------------------------------

-- Creating foreign key on [User_Id] in table 'AccountSet'
ALTER TABLE [dbo].[AccountSet] WITH NOCHECK 
ADD CONSTRAINT [FK_UserAccount]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [Account_Id] in table 'PostingSet'
ALTER TABLE [dbo].[PostingSet] WITH NOCHECK 
ADD CONSTRAINT [FK_AccountPosting]
    FOREIGN KEY ([Account_Id])
    REFERENCES [dbo].[AccountSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [AssetType_Id] in table 'PostingSet'
ALTER TABLE [dbo].[PostingSet] WITH NOCHECK 
ADD CONSTRAINT [FK_PostingAssetType]
    FOREIGN KEY ([AssetType_Id])
    REFERENCES [dbo].[AssetTypeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [Journal_Id] in table 'PostingSet'
ALTER TABLE [dbo].[PostingSet] WITH NOCHECK 
ADD CONSTRAINT [FK_JournalPosting]
    FOREIGN KEY ([Journal_Id])
    REFERENCES [dbo].[JournalSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [JournalType_Id] in table 'JournalSet'
ALTER TABLE [dbo].[JournalSet] WITH NOCHECK 
ADD CONSTRAINT [FK_JournalTypeJournal]
    FOREIGN KEY ([JournalType_Id])
    REFERENCES [dbo].[JournalTypeSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [Category_Id] in table 'JournalSet_Transaction'
ALTER TABLE [dbo].[JournalSet_Transaction] WITH NOCHECK 
ADD CONSTRAINT [FK_CategoryTransaction]
    FOREIGN KEY ([Category_Id])
    REFERENCES [dbo].[CategorySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [User_Id] in table 'CategorySet'
ALTER TABLE [dbo].[CategorySet] WITH NOCHECK 
ADD CONSTRAINT [FK_CategoryUser]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO
-- Creating foreign key on [Id] in table 'JournalSet_Transaction'
ALTER TABLE [dbo].[JournalSet_Transaction] WITH NOCHECK 
ADD CONSTRAINT [FK_Transaction_inherits_Journal]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[JournalSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------