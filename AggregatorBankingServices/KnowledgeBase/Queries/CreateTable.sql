﻿create table [Domains]
(
	[Name] varchar(450) not null primary key
);

create table [DomainValues]
(
	[Name] varchar(450) not null primary key,
	[DomainName] varchar(450) not null foreign key ([DomainName]) references [Domains]([Name]) 
);

--on delete no action on update no action

create table [VariablesTypes] 
(
	[Name] varchar(450) not null primary key
);

create table [Variables]
(
	[Name] varchar(450) not null primary key,
	[DomainName] varchar(450) not null foreign key ([DomainName]) references [Domains]([Name]),
	[VariableTypeName] varchar(450) not null foreign key ([VariableTypeName]) references [VariablesTypes]([Name]),
	[Question] varchar(max) null
);

-- on delete cascade on update cascade

create table [Facts]
(
	[Id] int not null primary key identity(1,1),
	[VariableName] varchar(450) not null foreign key ([VariableName]) references [Variables]([Name]),
	[DomainValueName] varchar(450) not null foreign key ([DomainValueName]) references [DomainValues]([Name]) 
);

create table [Rules]
(
	[Id] int not null primary key identity(1,1),
	[Name] varchar(450) not null,
	[FactId] int not null foreign key ([FactId]) references [Facts]([Id]),
	[FactResultId] int null foreign key ([FactResultId]) references [Facts]([Id]),
	[AdditionalRuleId] int null,
	[Description] varchar(max) null
);

-- foreign key ([AdditionalRuleId]) references [Rules]([Id]) on delete no action on update no action