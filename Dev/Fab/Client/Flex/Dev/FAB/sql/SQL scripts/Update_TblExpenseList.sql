UPDATE [ProfitAndExpense].[dbo].[TblExpenseList]
   SET [ExpenseGroupId] = <ExpenseGroupId, int,>
      ,[Name] = <Name, nvarchar(50),>
      ,[Cost] = <Cost, money,>
      ,[CreationDate] = <CreationDate, datetime,>
      ,[Memo] = <Memo, nvarchar(256),>
      ,[IsDeleted] = <IsDeleted, bit,>
 WHERE <Search Conditions,,>