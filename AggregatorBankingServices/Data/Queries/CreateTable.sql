Create table [BankNames]
(
	[Name] varchar(450) not null primary key
)

create table [Contributions]
(
	[Id] int not null primary key identity(1,1),
	[Name] varchar(450) not null,
	[Rate] decimal(11,3) not null,
	[DepositAmountFrom] decimal(20,3) not null,
	[DepositAmountTo] decimal(20,3) not null,
	[TermFrom] bigint not null, 
	[TermTo] bigint not null,
	[Capitalization] varchar(450) not null,
	[PaymentInterest] varchar(450) null,
	[Replenishment] varchar(450) not null,
	[PartialRemoval] varchar(450) not null,
	[NameBank] varchar(450) not null foreign key ([NameBank]) references [BankNames]([Name]) on delete cascade on update cascade
);

create table [Loan]
(
	[Id] int not null primary key identity(1,1),
	[Name] varchar(450) null,
	[Rate] decimal(11,3) null,
	[LoanAmountFrom] decimal(20,3) null,
	[LoanAmountTo] decimal(20,3) null,
	[TypePayment] varchar(450) null,
	[PaymentFrequency] varchar(450) null,
	[ApplicationReview] varchar(450) null,
	MandatoryInsurance varchar(450) null,
	[RequiredDocuments] varchar(450) null,
	[IncomeVerification] varchar(450) null,
	[AgeRedemptionMan] bigint null,
	[AgeRedemptionWomen] bigint null,
	[BorrowerAge] bigint null,
	[TermFrom] bigint null, 
	[TermTo] bigint null, 
	[NameBank] varchar(450) not null foreign key ([NameBank]) references [BankNames]([Name]) on delete cascade on update cascade
);

create table [User]
(
	[Login] nvarchar(450) not null primary key,
	[Password] varchar(max) not null,
	[Scoring] varchar(450)
);