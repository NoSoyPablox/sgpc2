
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/06/2024 17:02:35
-- Generated from EDMX file: C:\Users\xjerr\source\repos\MangoFizz\sgsc\Modelsgsc.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [sgsc];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CustomerId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerContactInfoes] DROP CONSTRAINT [FK_CustomerId];
GO
IF OBJECT_ID(N'[dbo].[FK_BankAccountCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BankAccounts] DROP CONSTRAINT [FK_BankAccountCustomer];
GO
IF OBJECT_ID(N'[dbo].[FK_CreditRequestCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [FK_CreditRequestCustomer];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contacts] DROP CONSTRAINT [FK_CustomerContact];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[BankAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BankAccounts];
GO
IF OBJECT_ID(N'[dbo].[Contacts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contacts];
GO
IF OBJECT_ID(N'[dbo].[CreditConditionCreditRequest]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditConditionCreditRequest];
GO
IF OBJECT_ID(N'[dbo].[CreditConditions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditConditions];
GO
IF OBJECT_ID(N'[dbo].[CreditPolicies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditPolicies];
GO
IF OBJECT_ID(N'[dbo].[CreditRequests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditRequests];
GO
IF OBJECT_ID(N'[dbo].[CustomerAddresses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerAddresses];
GO
IF OBJECT_ID(N'[dbo].[CustomerContactInfoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerContactInfoes];
GO
IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[Payments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Payments];
GO
IF OBJECT_ID(N'[dbo].[WorkCenters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WorkCenters];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'BankAccounts'
CREATE TABLE [dbo].[BankAccounts] (
    [BankAccountId] int IDENTITY(1,1) NOT NULL,
    [InterbankCode] nvarchar(max)  NULL,
    [CardNumber] nvarchar(max)  NULL,
    [AccountType] nvarchar(max)  NULL,
    [CardType] nvarchar(max)  NULL,
    [BankName] nvarchar(max)  NULL,
    [Customer_CustormerId] int  NULL,
    [CreditRequestId] int  NULL
);
GO

-- Creating table 'Contacts'
CREATE TABLE [dbo].[Contacts] (
    [ContactId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [FirstSurname] nvarchar(max)  NULL,
    [SecondSurname] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [CustormerId] int  NULL
);
GO

-- Creating table 'CreditConditionCreditRequests'
CREATE TABLE [dbo].[CreditConditionCreditRequests] (
    [CreditConditionIdRequest] int IDENTITY(1,1) NOT NULL,
    [CreditCondition_CreditConditionId] int  NULL,
    [CreditRequests_CreditRequestId] int  NULL
);
GO

-- Creating table 'CreditConditions'
CREATE TABLE [dbo].[CreditConditions] (
    [CreditConditionId] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'CreditPolicies'
CREATE TABLE [dbo].[CreditPolicies] (
    [CreditPolicyId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Active] bit  NULL,
    [EffectiveDate] datetime  NULL
);
GO

-- Creating table 'CreditRequests'
CREATE TABLE [dbo].[CreditRequests] (
    [CreditRequestId] int IDENTITY(1,1) NOT NULL,
    [FileNumber] nvarchar(max)  NULL,
    [Amount] float  NULL,
    [Status] int  NULL,
    [TimePeriod] datetime  NULL,
    [Purpose] nvarchar(max)  NULL,
    [InterestRate] decimal(18,0)  NULL,
    [CreationDate] datetime  NULL,
    [EmployeeId] int  NULL
);
GO

-- Creating table 'CustomerAddresses'
CREATE TABLE [dbo].[CustomerAddresses] (
    [CustomerAddressId] int IDENTITY(1,1) NOT NULL,
    [Street] nvarchar(max)  NULL,
    [ZipCode] nvarchar(max)  NULL,
    [ExternalNumber] nvarchar(max)  NULL,
    [InternalNumber] nvarchar(max)  NULL,
    [CustormerId] int  NULL
);
GO

-- Creating table 'CustomerContactInfoes'
CREATE TABLE [dbo].[CustomerContactInfoes] (
    [CustomerContactInfoId] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NULL,
    [PhoneNumberOne] nvarchar(max)  NULL,
    [PhoneNumberTwo] nvarchar(max)  NULL,
    [CustomerId] int  NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [CustormerId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [FirstSurname] nvarchar(max)  NULL,
    [SecondSurname] nvarchar(max)  NULL,
    [Curp] nvarchar(max)  NULL,
    [Rfc] nvarchar(max)  NULL,
    [CreditRequestId] int  NULL,
    [CustomerContactInfoId] int  NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [EmployeeId] int IDENTITY(1,1) NOT NULL,
    [FirstSurname] nvarchar(max)  NULL,
    [SecondSurname] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [Password] nvarchar(max)  NULL
);
GO

-- Creating table 'Payments'
CREATE TABLE [dbo].[Payments] (
    [PaymentId] int IDENTITY(1,1) NOT NULL,
    [FileNumber] nvarchar(max)  NULL,
    [PaymentDate] datetime  NULL,
    [Amount] nvarchar(max)  NULL,
    [CreditRequestId] int  NULL
);
GO

-- Creating table 'WorkCenters'
CREATE TABLE [dbo].[WorkCenters] (
    [WorkCenterId] int IDENTITY(1,1) NOT NULL,
    [CenterName] nvarchar(60)  NULL,
    [Street] nvarchar(60)  NULL,
    [Colony] nvarchar(60)  NULL,
    [InnerNumber] int  NULL,
    [OutsideNumber] int  NULL,
    [ZipCode] int  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [CustormerId] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [BankAccountId] in table 'BankAccounts'
ALTER TABLE [dbo].[BankAccounts]
ADD CONSTRAINT [PK_BankAccounts]
    PRIMARY KEY CLUSTERED ([BankAccountId] ASC);
GO

-- Creating primary key on [ContactId] in table 'Contacts'
ALTER TABLE [dbo].[Contacts]
ADD CONSTRAINT [PK_Contacts]
    PRIMARY KEY CLUSTERED ([ContactId] ASC);
GO

-- Creating primary key on [CreditConditionIdRequest] in table 'CreditConditionCreditRequests'
ALTER TABLE [dbo].[CreditConditionCreditRequests]
ADD CONSTRAINT [PK_CreditConditionCreditRequests]
    PRIMARY KEY CLUSTERED ([CreditConditionIdRequest] ASC);
GO

-- Creating primary key on [CreditConditionId] in table 'CreditConditions'
ALTER TABLE [dbo].[CreditConditions]
ADD CONSTRAINT [PK_CreditConditions]
    PRIMARY KEY CLUSTERED ([CreditConditionId] ASC);
GO

-- Creating primary key on [CreditPolicyId] in table 'CreditPolicies'
ALTER TABLE [dbo].[CreditPolicies]
ADD CONSTRAINT [PK_CreditPolicies]
    PRIMARY KEY CLUSTERED ([CreditPolicyId] ASC);
GO

-- Creating primary key on [CreditRequestId] in table 'CreditRequests'
ALTER TABLE [dbo].[CreditRequests]
ADD CONSTRAINT [PK_CreditRequests]
    PRIMARY KEY CLUSTERED ([CreditRequestId] ASC);
GO

-- Creating primary key on [CustomerAddressId] in table 'CustomerAddresses'
ALTER TABLE [dbo].[CustomerAddresses]
ADD CONSTRAINT [PK_CustomerAddresses]
    PRIMARY KEY CLUSTERED ([CustomerAddressId] ASC);
GO

-- Creating primary key on [CustomerContactInfoId] in table 'CustomerContactInfoes'
ALTER TABLE [dbo].[CustomerContactInfoes]
ADD CONSTRAINT [PK_CustomerContactInfoes]
    PRIMARY KEY CLUSTERED ([CustomerContactInfoId] ASC);
GO

-- Creating primary key on [CustormerId] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([CustormerId] ASC);
GO

-- Creating primary key on [EmployeeId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([EmployeeId] ASC);
GO

-- Creating primary key on [PaymentId] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [PK_Payments]
    PRIMARY KEY CLUSTERED ([PaymentId] ASC);
GO

-- Creating primary key on [WorkCenterId] in table 'WorkCenters'
ALTER TABLE [dbo].[WorkCenters]
ADD CONSTRAINT [PK_WorkCenters]
    PRIMARY KEY CLUSTERED ([WorkCenterId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Customer_CustormerId] in table 'BankAccounts'
ALTER TABLE [dbo].[BankAccounts]
ADD CONSTRAINT [FK_BankAccountCustomer]
    FOREIGN KEY ([Customer_CustormerId])
    REFERENCES [dbo].[Customers]
        ([CustormerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BankAccountCustomer'
CREATE INDEX [IX_FK_BankAccountCustomer]
ON [dbo].[BankAccounts]
    ([Customer_CustormerId]);
GO

-- Creating foreign key on [CustormerId] in table 'Contacts'
ALTER TABLE [dbo].[Contacts]
ADD CONSTRAINT [FK_CustomerContact]
    FOREIGN KEY ([CustormerId])
    REFERENCES [dbo].[Customers]
        ([CustormerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerContact'
CREATE INDEX [IX_FK_CustomerContact]
ON [dbo].[Contacts]
    ([CustormerId]);
GO

-- Creating foreign key on [CreditRequestId] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [FK_CreditRequestCustomer]
    FOREIGN KEY ([CreditRequestId])
    REFERENCES [dbo].[CreditRequests]
        ([CreditRequestId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreditRequestCustomer'
CREATE INDEX [IX_FK_CreditRequestCustomer]
ON [dbo].[Customers]
    ([CreditRequestId]);
GO

-- Creating foreign key on [CustomerId] in table 'CustomerContactInfoes'
ALTER TABLE [dbo].[CustomerContactInfoes]
ADD CONSTRAINT [FK_CustomerId]
    FOREIGN KEY ([CustomerId])
    REFERENCES [dbo].[Customers]
        ([CustormerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerId'
CREATE INDEX [IX_FK_CustomerId]
ON [dbo].[CustomerContactInfoes]
    ([CustomerId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------