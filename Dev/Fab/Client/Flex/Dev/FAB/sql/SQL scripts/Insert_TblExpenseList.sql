INSERT INTO [ProfitAndExpense].[dbo].[TblExpenseList]
           ([ExpenseGroupId]
           ,[Name]
           ,[Cost]
           ,[CreationDate]
           ,[Memo]
           ,[IsDeleted])
     VALUES
           (<ExpenseGroupId, int,>
           ,<Name, nvarchar(50),>
           ,<Cost, money,>
           ,<CreationDate, datetime,>
           ,<Memo, nvarchar(256),>
           ,<IsDeleted, bit,>)