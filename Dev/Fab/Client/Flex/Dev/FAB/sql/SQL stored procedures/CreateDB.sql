/*
drop table TblCurrencyList;
drop table TblExpenseGroups;
drop table TblExpenseList;
drop table TblRevenueGroups;
drop table TblRevenueList;
*/
create table TblCurrencyList
(Id INT PRIMARY KEY NOT NULL,
 Name NVARCHAR(10) NOT NULL)
;
create table TblExpenseGroups
(Id INT PRIMARY KEY NOT NULL,
Name NVARCHAR(50) NOT NULL,
CreationDate DATETIME NOT NULL,
IsDeleted BIT NOT NULL)
;
create table TblExpenseList
(Id INT PRIMARY KEY NOT NULL,
ExpenseGroupId INT NOT NULL,
Name NVARCHAR(50) NOT NULL,
Cost MONEY NOT NULL,
CreationDate DATETIME NOT NULL,
IsDeleted BIT NOT NULL,
Memo NVARCHAR(256))
;
create table TblRevenueGroups
(Id INT PRIMARY KEY NOT NULL,
Name NVARCHAR(50) NOT NULL,
CreationDate DATETIME NOT NULL,
IsDeleted BIT NOT NULL)
;
create table TblRevenueList
(Id INT PRIMARY KEY NOT NULL,
RevenueGroupId INT NOT NULL,
SourceName NVARCHAR(50) NOT NULL,
AmountGRN MONEY NOT NULL,
CreationDate DATETIME NOT NULL,
Memo NVARCHAR(256) NOT NULL,
ExchangeRate FLOAT NOT NULL,
CurrencyId INT NOT NULL,
IsDeleted BIT NOT NULL)
;


