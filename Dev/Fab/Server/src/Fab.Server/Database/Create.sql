
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 02/22/2010 02:33:06
-- Generated from EDMX file: B:\Workspace\Dev\Fab\Server\src\Fab.Server\Core\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
--USE [Database];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AccountAssetType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Accounts] DROP CONSTRAINT [FK_AccountAssetType];
GO
IF OBJECT_ID(N'[dbo].[FK_AccountPosting]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Postings] DROP CONSTRAINT [FK_AccountPosting];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryTransaction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Journals_Transaction] DROP CONSTRAINT [FK_CategoryTransaction];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Categories] DROP CONSTRAINT [FK_CategoryUser];
GO
IF OBJECT_ID(N'[dbo].[FK_JournalPosting]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Postings] DROP CONSTRAINT [FK_JournalPosting];
GO
IF OBJECT_ID(N'[dbo].[FK_JournalTypeJournal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Journals] DROP CONSTRAINT [FK_JournalTypeJournal];
GO
IF OBJECT_ID(N'[dbo].[FK_PostingAssetType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Postings] DROP CONSTRAINT [FK_PostingAssetType];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Accounts] DROP CONSTRAINT [FK_UserAccount];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Accounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Accounts];
GO
IF OBJECT_ID(N'[dbo].[AssetTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AssetTypes];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Journals_Transaction]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Journals_Transaction];
GO
IF OBJECT_ID(N'[dbo].[Journals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Journals];
GO
IF OBJECT_ID(N'[dbo].[JournalTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[JournalTypes];
GO
IF OBJECT_ID(N'[dbo].[Postings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Postings];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Accounts'
CREATE TABLE [dbo].[Accounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Created] datetime  NOT NULL  DEFAULT (getdate()),
    [IsDeleted] bit  NOT NULL  DEFAULT (0),
    [User_Id] uniqueidentifier  NOT NULL,
    [AssetType_Id] int  NOT NULL
);
GO

-- Creating table 'AssetTypes'
CREATE TABLE [dbo].[AssetTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [IsDeleted] bit  NOT NULL  DEFAULT (0),
    [User_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Journals'
CREATE TABLE [dbo].[Journals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [JournalType_Id] int  NOT NULL
);
GO

-- Creating table 'JournalTypes'
CREATE TABLE [dbo].[JournalTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Postings'
CREATE TABLE [dbo].[Postings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL  DEFAULT (getdate()),
    [Amount] money  NOT NULL,
    [Account_Id] int  NOT NULL,
    [AssetType_Id] int  NOT NULL,
    [Journal_Id] int  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] uniqueidentifier  NOT NULL  DEFAULT (newid()),
    [Login] nvarchar(50)  NOT NULL,
    [Password] nvarchar(256)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [Registered] datetime  NOT NULL  DEFAULT (getdate()),
    [LastAccess] datetime  NULL,
    [IsDisabled] bit  NOT NULL  DEFAULT (0)
);
GO

-- Creating table 'Journals_Transaction'
CREATE TABLE [dbo].[Journals_Transaction] (
    [Id] int  NOT NULL,
    [Quantity] smallmoney  NOT NULL,
    [Price] money  NOT NULL,
    [Comment] nvarchar(256)  NULL,
    [IsDeleted] bit  NOT NULL  DEFAULT (0),
    [Category_Id] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [PK_Accounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AssetTypes'
ALTER TABLE [dbo].[AssetTypes]
ADD CONSTRAINT [PK_AssetTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Journals'
ALTER TABLE [dbo].[Journals]
ADD CONSTRAINT [PK_Journals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'JournalTypes'
ALTER TABLE [dbo].[JournalTypes]
ADD CONSTRAINT [PK_JournalTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);   
GO

-- Creating primary key on [Id] in table 'Postings'
ALTER TABLE [dbo].[Postings]
ADD CONSTRAINT [PK_Postings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Journals_Transaction'
ALTER TABLE [dbo].[Journals_Transaction]
ADD CONSTRAINT [PK_Journals_Transaction]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Account_Id] in table 'Postings'
ALTER TABLE [dbo].[Postings]
ADD CONSTRAINT [FK_AccountPosting]
    FOREIGN KEY ([Account_Id])
    REFERENCES [dbo].[Accounts]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountPosting'
CREATE INDEX [IX_FK_AccountPosting]
ON [dbo].[Postings]
    ([Account_Id]);
GO

-- Creating foreign key on [User_Id] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [FK_UserAccount]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccount'
CREATE INDEX [IX_FK_UserAccount]
ON [dbo].[Accounts]
    ([User_Id]);
GO

-- Creating foreign key on [AssetType_Id] in table 'Postings'
ALTER TABLE [dbo].[Postings]
ADD CONSTRAINT [FK_PostingAssetType]
    FOREIGN KEY ([AssetType_Id])
    REFERENCES [dbo].[AssetTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PostingAssetType'
CREATE INDEX [IX_FK_PostingAssetType]
ON [dbo].[Postings]
    ([AssetType_Id]);
GO

-- Creating foreign key on [Category_Id] in table 'Journals_Transaction'
ALTER TABLE [dbo].[Journals_Transaction]
ADD CONSTRAINT [FK_CategoryTransaction]
    FOREIGN KEY ([Category_Id])
    REFERENCES [dbo].[Categories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryTransaction'
CREATE INDEX [IX_FK_CategoryTransaction]
ON [dbo].[Journals_Transaction]
    ([Category_Id]);
GO

-- Creating foreign key on [User_Id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [FK_CategoryUser]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryUser'
CREATE INDEX [IX_FK_CategoryUser]
ON [dbo].[Categories]
    ([User_Id]);
GO

-- Creating foreign key on [Journal_Id] in table 'Postings'
ALTER TABLE [dbo].[Postings]
ADD CONSTRAINT [FK_JournalPosting]
    FOREIGN KEY ([Journal_Id])
    REFERENCES [dbo].[Journals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_JournalPosting'
CREATE INDEX [IX_FK_JournalPosting]
ON [dbo].[Postings]
    ([Journal_Id]);
GO

-- Creating foreign key on [JournalType_Id] in table 'Journals'
ALTER TABLE [dbo].[Journals]
ADD CONSTRAINT [FK_JournalTypeJournal]
    FOREIGN KEY ([JournalType_Id])
    REFERENCES [dbo].[JournalTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_JournalTypeJournal'
CREATE INDEX [IX_FK_JournalTypeJournal]
ON [dbo].[Journals]
    ([JournalType_Id]);
GO

-- Creating foreign key on [AssetType_Id] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [FK_AccountAssetType]
    FOREIGN KEY ([AssetType_Id])
    REFERENCES [dbo].[AssetTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AccountAssetType'
CREATE INDEX [IX_FK_AccountAssetType]
ON [dbo].[Accounts]
    ([AssetType_Id]);
GO

-- Creating foreign key on [Id] in table 'Journals_Transaction'
ALTER TABLE [dbo].[Journals_Transaction]
ADD CONSTRAINT [FK_Transaction_inherits_Journal]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[Journals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------