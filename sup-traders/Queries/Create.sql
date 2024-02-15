-- DATABASE
CREATE DATABASE [TradeData]


-- TABLES
CREATE TABLE [TradeData].[dbo].[Shares] (
  	[code] [varchar](3) NOT NULL PRIMARY KEY,
	[count] [int] NOT NULL,
	[price] DECIMAL(10,2) NOT NULL,
	[baseCount] [int] NULL,  -- To calculate updated share value
	[basePrice] DECIMAL(10,2) NULL,  -- To calculate updated share value
);


CREATE TABLE [TradeData].[dbo].[Users] (
	[id] [int] NOT NULL PRIMARY KEY IDENTITY (1, 1),
  	[name] [varchar](20) NULL,
	[balance] DECIMAL(10,2) NOT NULL,
);

CREATE TABLE [TradeData].[dbo].[Exchanges] (
  	[shareCode] [varchar](3) NOT NULL FOREIGN KEY REFERENCES Shares(code),
	[userId] [int] NOT NULL FOREIGN KEY REFERENCES Users(id),
	[shareAmount] [int] NOT NULL,
);


-- TRIGGERS

  CREATE TRIGGER Update_Price
  ON [TradeData].[dbo].[Shares]
  AFTER UPDATE 
  AS
  BEGIN
		declare @c varchar(3)
		SELECT @c = [code] FROM inserted
		UPDATE [TradeData].[dbo].[Shares] 
		SET [price] = [baseCount] * [basePrice] / [count]
		WHERE [code] = @c
  END;