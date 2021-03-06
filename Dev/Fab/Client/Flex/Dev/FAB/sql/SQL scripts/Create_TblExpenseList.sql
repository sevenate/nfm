USE [ProfitAndExpense]
GO
/****** Object:  Table [dbo].[TblExpenseList]    Script Date: 01/13/2010 15:00:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TblExpenseList](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ExpenseGroupId] [int] NOT NULL,
	[Name] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Cost] [money] NOT NULL,
	[CreationDate] [datetime] NOT NULL CONSTRAINT [DF_TblExpensesList_Date]  DEFAULT (getdate()),
	[Memo] [nvarchar](256) COLLATE Cyrillic_General_CI_AS NULL,
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_TblExpensesList_IsDeleted]  DEFAULT ((0)),
 CONSTRAINT [PK_TblExpensesList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[TblExpenseList]  WITH CHECK ADD  CONSTRAINT [FK_TblExpensesList_TblExpenseGroups] FOREIGN KEY([ExpenseGroupId])
REFERENCES [dbo].[TblExpenseGroups] ([Id])
GO
ALTER TABLE [dbo].[TblExpenseList] CHECK CONSTRAINT [FK_TblExpensesList_TblExpenseGroups]