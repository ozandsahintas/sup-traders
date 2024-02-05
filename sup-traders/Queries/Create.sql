CREATE DATABASE [TradeData]

CREATE TABLE [TradeData].[dbo].[Shares] (
  	[code] [varchar](3) NOT NULL PRIMARY KEY,
	[count] [int] NOT NULL,
	[price] [float] NOT NULL,
);

CREATE TABLE [TradeData].[dbo].[Users] (
  	[name] [varchar](20) NULL,
	[balance] [float] NOT NULL,
);