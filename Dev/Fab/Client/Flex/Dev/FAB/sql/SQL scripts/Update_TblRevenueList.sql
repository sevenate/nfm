UPDATE [ProfitAndExpense].[dbo].[TblRevenueList]
   SET [RevenueGroupId] = <RevenueGroupId, int,>
      ,[SourceName] = <SourceName, nvarchar(50),>
      ,[AmountGRN] = <AmountGRN, money,>
      ,[CreationDate] = <CreationDate, datetime,>
      ,[Memo] = <Memo, nvarchar(256),>
      ,[ExchangeRate] = <ExchangeRate, float,>
      ,[CurrencyId] = <CurrencyId, int,>
      ,[IsDeleted] = <IsDeleted, bit,>
 WHERE <Search Conditions,,>