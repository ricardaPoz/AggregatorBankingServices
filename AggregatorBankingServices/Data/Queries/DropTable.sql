--drop table [User];
--drop table [Loan];
--drop table [Contributions];
--drop table [BankNames];

select count([Contributions].[Id])
from Contributions
