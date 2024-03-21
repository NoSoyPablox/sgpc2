
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/21/2024 16:39:49
-- Generated from EDMX file: C:\Users\aiwass\source\repos\SGSC\Model.edmx
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

IF OBJECT_ID(N'[dbo].[FK_CustomerContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contacts] DROP CONSTRAINT [FK_CustomerContact];
GO
IF OBJECT_ID(N'[dbo].[FK_CreditRequestCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [FK_CreditRequestCustomer];
GO
IF OBJECT_ID(N'[dbo].[FK_BankAccountCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BankAccounts] DROP CONSTRAINT [FK_BankAccountCustomer];
GO
IF OBJECT_ID(N'[dbo].[FK_BankAccountCreditRequest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BankAccounts] DROP CONSTRAINT [FK_BankAccountCreditRequest];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentCreditRequest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Payments] DROP CONSTRAINT [FK_PaymentCreditRequest];
GO
IF OBJECT_ID(N'[dbo].[FK_WorkCenterCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[WorkCenters] DROP CONSTRAINT [FK_WorkCenterCustomer];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerContactInfoCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Customers] DROP CONSTRAINT [FK_CustomerContactInfoCustomer];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerAddressCustomer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerAddresses] DROP CONSTRAINT [FK_CustomerAddressCustomer];
GO
IF OBJECT_ID(N'[dbo].[FK_CreditRequestEmployee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CreditRequests] DROP CONSTRAINT [FK_CreditRequestEmployee];
GO
IF OBJECT_ID(N'[dbo].[FK_CreditConditionCreditRequest_CreditCondition]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CreditConditionCreditRequest] DROP CONSTRAINT [FK_CreditConditionCreditRequest_CreditCondition];
GO
IF OBJECT_ID(N'[dbo].[FK_CreditConditionCreditRequest_CreditRequest]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CreditConditionCreditRequest] DROP CONSTRAINT [FK_CreditConditionCreditRequest_CreditRequest];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[Contacts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contacts];
GO
IF OBJECT_ID(N'[dbo].[WorkCenters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[WorkCenters];
GO
IF OBJECT_ID(N'[dbo].[CreditRequests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditRequests];
GO
IF OBJECT_ID(N'[dbo].[BankAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BankAccounts];
GO
IF OBJECT_ID(N'[dbo].[Payments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Payments];
GO
IF OBJECT_ID(N'[dbo].[CustomerContactInfoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerContactInfoes];
GO
IF OBJECT_ID(N'[dbo].[CustomerAddresses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerAddresses];
GO
IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[CreditConditions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditConditions];
GO
IF OBJECT_ID(N'[dbo].[CreditPolicies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditPolicies];
GO
IF OBJECT_ID(N'[dbo].[CreditConditionCreditRequest]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CreditConditionCreditRequest];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [CustormerId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [FirstSurname] nvarchar(max)  NOT NULL,
    [SecondSurname] nvarchar(max)  NOT NULL,
    [Curp] nvarchar(max)  NOT NULL,
    [Rfc] nvarchar(max)  NOT NULL,
    [CreditRequest_CreditRequestId] int  NOT NULL,
    [CustomerContactInfo_CustomerContactInfoId] int  NOT NULL
);
GO

-- Creating table 'Contacts'
CREATE TABLE [dbo].[Contacts] (
    [ContactId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [FirstSurname] nvarchar(max)  NOT NULL,
    [SecondSurname] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NOT NULL,
    [Customer_CustormerId] int  NOT NULL
);
GO

-- Creating table 'WorkCenters'
CREATE TABLE [dbo].[WorkCenters] (
    [WorkCenterId] int IDENTITY(1,1) NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NOT NULL,
    [Customer_CustormerId] int  NOT NULL
);
GO

-- Creating table 'CreditRequests'
CREATE TABLE [dbo].[CreditRequests] (
    [CreditRequestId] int IDENTITY(1,1) NOT NULL,
    [FileNumber] nvarchar(max)  NOT NULL,
    [Amount] float  NOT NULL,
    [Status] int  NOT NULL,
    [TimePeriod] datetime  NOT NULL,
    [Purpose] nvarchar(max)  NOT NULL,
    [InterestRate] decimal(18,0)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [Employees_EmployeeId] int  NOT NULL
);
GO

-- Creating table 'BankAccounts'
CREATE TABLE [dbo].[BankAccounts] (
    [BankAccountId] int IDENTITY(1,1) NOT NULL,
    [InterbankCode] nvarchar(max)  NOT NULL,
    [CardNumber] time  NOT NULL,
    [AccountType] nvarchar(max)  NOT NULL,
    [CardType] nvarchar(max)  NOT NULL,
    [BankName] nvarchar(max)  NOT NULL,
    [Customer_CustormerId] int  NOT NULL,
    [CreditRequests_CreditRequestId] int  NOT NULL
);
GO

-- Creating table 'Payments'
CREATE TABLE [dbo].[Payments] (
    [PaymentId] int IDENTITY(1,1) NOT NULL,
    [FileNumber] nvarchar(max)  NOT NULL,
    [PaymentDate] datetime  NOT NULL,
    [Amount] nvarchar(max)  NOT NULL,
    [CreditRequest_CreditRequestId] int  NOT NULL
);
GO

-- Creating table 'CustomerContactInfoes'
CREATE TABLE [dbo].[CustomerContactInfoes] (
    [CustomerContactInfoId] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [PhoneNumberOne] nvarchar(max)  NOT NULL,
    [PhoneNumberTwo] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CustomerAddresses'
CREATE TABLE [dbo].[CustomerAddresses] (
    [CustomerAddressId] int IDENTITY(1,1) NOT NULL,
    [Street] nvarchar(max)  NOT NULL,
    [ZipCode] nvarchar(max)  NOT NULL,
    [ExternalNumber] nvarchar(max)  NOT NULL,
    [InternalNumber] nvarchar(max)  NOT NULL,
    [Customers_CustormerId] int  NOT NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [EmployeeId] int IDENTITY(1,1) NOT NULL,
    [FirstSurname] nvarchar(max)  NOT NULL,
    [SecondSurname] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CreditConditions'
CREATE TABLE [dbo].[CreditConditions] (
    [CreditConditionId] int IDENTITY(1,1) NOT NULL,
    [Iva] decimal(18,0)  NOT NULL,
    [TimePeriod] datetime  NOT NULL,
    [InterestRate] decimal(18,0)  NOT NULL,
    [Active] bit  NOT NULL
);
GO

-- Creating table 'CreditPolicies'
CREATE TABLE [dbo].[CreditPolicies] (
    [CreditPolicyId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Active] bit  NOT NULL,
    [EffectiveDate] datetime  NOT NULL
);
GO

-- Creating table 'CreditConditionCreditRequest'
CREATE TABLE [dbo].[CreditConditionCreditRequest] (
    [CreditCondition_CreditConditionId] int  NOT NULL,
    [CreditRequests_CreditRequestId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [CustormerId] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([CustormerId] ASC);
GO

-- Creating primary key on [ContactId] in table 'Contacts'
ALTER TABLE [dbo].[Contacts]
ADD CONSTRAINT [PK_Contacts]
    PRIMARY KEY CLUSTERED ([ContactId] ASC);
GO

-- Creating primary key on [WorkCenterId] in table 'WorkCenters'
ALTER TABLE [dbo].[WorkCenters]
ADD CONSTRAINT [PK_WorkCenters]
    PRIMARY KEY CLUSTERED ([WorkCenterId] ASC);
GO

-- Creating primary key on [CreditRequestId] in table 'CreditRequests'
ALTER TABLE [dbo].[CreditRequests]
ADD CONSTRAINT [PK_CreditRequests]
    PRIMARY KEY CLUSTERED ([CreditRequestId] ASC);
GO

-- Creating primary key on [BankAccountId] in table 'BankAccounts'
ALTER TABLE [dbo].[BankAccounts]
ADD CONSTRAINT [PK_BankAccounts]
    PRIMARY KEY CLUSTERED ([BankAccountId] ASC);
GO

-- Creating primary key on [PaymentId] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [PK_Payments]
    PRIMARY KEY CLUSTERED ([PaymentId] ASC);
GO

-- Creating primary key on [CustomerContactInfoId] in table 'CustomerContactInfoes'
ALTER TABLE [dbo].[CustomerContactInfoes]
ADD CONSTRAINT [PK_CustomerContactInfoes]
    PRIMARY KEY CLUSTERED ([CustomerContactInfoId] ASC);
GO

-- Creating primary key on [CustomerAddressId] in table 'CustomerAddresses'
ALTER TABLE [dbo].[CustomerAddresses]
ADD CONSTRAINT [PK_CustomerAddresses]
    PRIMARY KEY CLUSTERED ([CustomerAddressId] ASC);
GO

-- Creating primary key on [EmployeeId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([EmployeeId] ASC);
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

-- Creating primary key on [CreditCondition_CreditConditionId], [CreditRequests_CreditRequestId] in table 'CreditConditionCreditRequest'
ALTER TABLE [dbo].[CreditConditionCreditRequest]
ADD CONSTRAINT [PK_CreditConditionCreditRequest]
    PRIMARY KEY CLUSTERED ([CreditCondition_CreditConditionId], [CreditRequests_CreditRequestId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Customer_CustormerId] in table 'Contacts'
ALTER TABLE [dbo].[Contacts]
ADD CONSTRAINT [FK_CustomerContact]
    FOREIGN KEY ([Customer_CustormerId])
    REFERENCES [dbo].[Customers]
        ([CustormerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerContact'
CREATE INDEX [IX_FK_CustomerContact]
ON [dbo].[Contacts]
    ([Customer_CustormerId]);
GO

-- Creating foreign key on [CreditRequest_CreditRequestId] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [FK_CreditRequestCustomer]
    FOREIGN KEY ([CreditRequest_CreditRequestId])
    REFERENCES [dbo].[CreditRequests]
        ([CreditRequestId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreditRequestCustomer'
CREATE INDEX [IX_FK_CreditRequestCustomer]
ON [dbo].[Customers]
    ([CreditRequest_CreditRequestId]);
GO

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

-- Creating foreign key on [CreditRequests_CreditRequestId] in table 'BankAccounts'
ALTER TABLE [dbo].[BankAccounts]
ADD CONSTRAINT [FK_BankAccountCreditRequest]
    FOREIGN KEY ([CreditRequests_CreditRequestId])
    REFERENCES [dbo].[CreditRequests]
        ([CreditRequestId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BankAccountCreditRequest'
CREATE INDEX [IX_FK_BankAccountCreditRequest]
ON [dbo].[BankAccounts]
    ([CreditRequests_CreditRequestId]);
GO

-- Creating foreign key on [CreditRequest_CreditRequestId] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [FK_PaymentCreditRequest]
    FOREIGN KEY ([CreditRequest_CreditRequestId])
    REFERENCES [dbo].[CreditRequests]
        ([CreditRequestId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentCreditRequest'
CREATE INDEX [IX_FK_PaymentCreditRequest]
ON [dbo].[Payments]
    ([CreditRequest_CreditRequestId]);
GO

-- Creating foreign key on [Customer_CustormerId] in table 'WorkCenters'
ALTER TABLE [dbo].[WorkCenters]
ADD CONSTRAINT [FK_WorkCenterCustomer]
    FOREIGN KEY ([Customer_CustormerId])
    REFERENCES [dbo].[Customers]
        ([CustormerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_WorkCenterCustomer'
CREATE INDEX [IX_FK_WorkCenterCustomer]
ON [dbo].[WorkCenters]
    ([Customer_CustormerId]);
GO

-- Creating foreign key on [CustomerContactInfo_CustomerContactInfoId] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [FK_CustomerContactInfoCustomer]
    FOREIGN KEY ([CustomerContactInfo_CustomerContactInfoId])
    REFERENCES [dbo].[CustomerContactInfoes]
        ([CustomerContactInfoId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerContactInfoCustomer'
CREATE INDEX [IX_FK_CustomerContactInfoCustomer]
ON [dbo].[Customers]
    ([CustomerContactInfo_CustomerContactInfoId]);
GO

-- Creating foreign key on [Customers_CustormerId] in table 'CustomerAddresses'
ALTER TABLE [dbo].[CustomerAddresses]
ADD CONSTRAINT [FK_CustomerAddressCustomer]
    FOREIGN KEY ([Customers_CustormerId])
    REFERENCES [dbo].[Customers]
        ([CustormerId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerAddressCustomer'
CREATE INDEX [IX_FK_CustomerAddressCustomer]
ON [dbo].[CustomerAddresses]
    ([Customers_CustormerId]);
GO

-- Creating foreign key on [Employees_EmployeeId] in table 'CreditRequests'
ALTER TABLE [dbo].[CreditRequests]
ADD CONSTRAINT [FK_CreditRequestEmployee]
    FOREIGN KEY ([Employees_EmployeeId])
    REFERENCES [dbo].[Employees]
        ([EmployeeId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreditRequestEmployee'
CREATE INDEX [IX_FK_CreditRequestEmployee]
ON [dbo].[CreditRequests]
    ([Employees_EmployeeId]);
GO

-- Creating foreign key on [CreditCondition_CreditConditionId] in table 'CreditConditionCreditRequest'
ALTER TABLE [dbo].[CreditConditionCreditRequest]
ADD CONSTRAINT [FK_CreditConditionCreditRequest_CreditCondition]
    FOREIGN KEY ([CreditCondition_CreditConditionId])
    REFERENCES [dbo].[CreditConditions]
        ([CreditConditionId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [CreditRequests_CreditRequestId] in table 'CreditConditionCreditRequest'
ALTER TABLE [dbo].[CreditConditionCreditRequest]
ADD CONSTRAINT [FK_CreditConditionCreditRequest_CreditRequest]
    FOREIGN KEY ([CreditRequests_CreditRequestId])
    REFERENCES [dbo].[CreditRequests]
        ([CreditRequestId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CreditConditionCreditRequest_CreditRequest'
CREATE INDEX [IX_FK_CreditConditionCreditRequest_CreditRequest]
ON [dbo].[CreditConditionCreditRequest]
    ([CreditRequests_CreditRequestId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------