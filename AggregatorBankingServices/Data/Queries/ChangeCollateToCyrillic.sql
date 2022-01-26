ALTER DATABASE BankProductDataBase
SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

ALTER DATABASE BankProductDataBase
COLLATE Cyrillic_General_100_CS_AS;

ALTER DATABASE BankProductDataBase
SET MULTI_USER WITH ROLLBACK IMMEDIATE;

SELECT name, collation_name
FROM sys.databases
WHERE name = 'BankProductDataBase';