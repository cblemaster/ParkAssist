USE master
GO

DECLARE @SQL nvarchar(1000);
IF EXISTS (SELECT 1 FROM sys.databases WHERE name = N'ParkAssist')
BEGIN
    SET @SQL = N'USE ParkAssist;

                 ALTER DATABASE ParkAssist SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                 USE master;

                 DROP DATABASE ParkAssist;';
    EXEC (@SQL);
END;

CREATE DATABASE ParkAssist
GO

USE ParkAssist
GO

CREATE TABLE Users (
	Id					int IDENTITY(1,1)					NOT NULL,
	Username			varchar(50)							NOT NULL,
	PasswordHash		varchar(200)						NOT NULL,
	Salt				varchar(200)						NOT NULL,
	FirstName			varchar(255)						NOT NULL,
	LastName			varchar(255)						NOT NULL,
	Email				varchar(255)						NOT NULL,
	Phone				varchar(10)							NOT NULL,
	CreateDate			datetime							NOT NULL,
	UpdateDate			datetime							NULL,
	CONSTRAINT PK_Users PRIMARY KEY(Id),
	CONSTRAINT UC_Username UNIQUE(Username),
	CONSTRAINT UC_Email UNIQUE(Email),
	CONSTRAINT UC_Phone UNIQUE(Phone),
)
GO

CREATE TABLE Owners (
	Id					int IDENTITY(1,1)					NOT NULL,
	UserId				int									NOT NULL,
	CONSTRAINT PK_Owners PRIMARY KEY(Id),
	CONSTRAINT UC_OwnersUserId UNIQUE(UserId),
	CONSTRAINT FK_Owners_Users FOREIGN KEY(UserId) REFERENCES Users(Id),
)
GO

CREATE TABLE Customers (
	Id					int IDENTITY(1,1)					NOT NULL,
	UserId				int									NOT NULL,
	CONSTRAINT PK_Customers PRIMARY KEY(Id),
	CONSTRAINT UC_CustomersUserId UNIQUE(UserId),
	CONSTRAINT FK_Customers_Users FOREIGN KEY(UserId) REFERENCES Users(Id), 
)
GO

CREATE TABLE Admins (
	Id					int IDENTITY(1,1)					NOT NULL,
	UserId				int									NOT NULL,
	CONSTRAINT PK_Admins PRIMARY KEY(Id),
	CONSTRAINT UC_AdminsUserId UNIQUE(UserId),
	CONSTRAINT FK_Admins_Users FOREIGN KEY(UserId) REFERENCES Users(Id),
)
GO

CREATE TABLE ParkingLots (
	Id					int IDENTITY(1,1)					NOT NULL,
	OwnerId				int									NOT NULL,
	Name                varchar(255)						NOT NULL,
	Address				varchar(255)						NOT NULL,
	City				varchar(255)						NOT NULL,
	State				varchar(255)						NOT NULL,	
	Zip					varchar(10)							NOT NULL,
	CreateDate			datetime							NOT NULL,
	UpdateDate			datetime							NULL,
	CONSTRAINT PK_ParkingLots PRIMARY KEY(Id),
	CONSTRAINT FK_ParkingLots_Owners FOREIGN KEY(OwnerId) REFERENCES Owners(Id),
	CONSTRAINT UC_Name UNIQUE(Name),
)
GO

CREATE TABLE ParkingSpots (
	Id					int IDENTITY(1,1)					NOT NULL,
	ParkingLotId		int									NOT NULL,
	Name                varchar(255)						NOT NULL,
	CreateDate			datetime							NOT NULL,
	UpdateDate			datetime							NULL,
	CONSTRAINT PK_ParkingSpots PRIMARY KEY(Id),
	CONSTRAINT FK_ParkingSpots_ParkingLots FOREIGN KEY(ParkingLotId) REFERENCES ParkingLots(Id),
)
GO

CREATE TABLE Valets (
	Id					int IDENTITY(1,1)					NOT NULL,
	UserId				int									NOT NULL,
	ParkingLotId		int									NOT NULL,
	CONSTRAINT PK_Valets PRIMARY KEY(Id),
	CONSTRAINT UC_ValetUserId UNIQUE(UserId),
	CONSTRAINT FK_Valets_Users FOREIGN KEY(UserId) REFERENCES Users(Id),
	CONSTRAINT FK_Valets_ParkingLots FOREIGN KEY(ParkingLotId) REFERENCES ParkingLots(Id),
)
GO

CREATE TABLE Vehicles (
	Id					int IDENTITY(1,1)					NOT NULL,
	CustomerId			int									NOT NULL,
	Make				varchar(100)						NOT NULL,
	Model				varchar(100)						NOT NULL,
	[Year]				varchar(4)							NOT NULL,
	LicensePlate		varchar(6)							NOT NULL,
	Color				varchar(100)						NOT NULL,
	CreateDate			datetime							NOT NULL,
	UpdateDate			datetime							NULL,
	CONSTRAINT PK_Vehicles PRIMARY KEY(Id),
	CONSTRAINT FK_Vehicles_Customers FOREIGN KEY(CustomerId) REFERENCES Customers(Id),
	CONSTRAINT UC_LicensePlate UNIQUE(LicensePlate),
)
GO

CREATE TABLE ParkingStatuses (
	Id					int IDENTITY(1,1)					NOT NULL,
	ParkingStatus		varchar(20)							NOT NULL,
	CONSTRAINT PK_ParkingStatuses PRIMARY KEY(Id),
	CONSTRAINT UC_ParkingStatus UNIQUE(ParkingStatus),
)
GO

INSERT INTO ParkingStatuses(ParkingStatus) VALUES ('ParkingRequested');
INSERT INTO ParkingStatuses(ParkingStatus) VALUES ('VehicleParked');
INSERT INTO ParkingStatuses(ParkingStatus) VALUES ('PickupRequested');
INSERT INTO ParkingStatuses(ParkingStatus) VALUES ('VehiclePickedUp');

CREATE TABLE Discounts (
	Id					int IDENTITY(1,1)					NOT NULL,
	Type				varchar(255)						NOT NULL,
	Description			varchar(255)						NOT NULL,
	Multiplier			decimal(13,2)						NOT NULL,
	CONSTRAINT PK_Discounts PRIMARY KEY(Id),
	CONSTRAINT UC_Type UNIQUE(Type),
)
GO

CREATE TABLE ParkingSlips (
	Id					int IDENTITY(1,1)					NOT NULL,
	ValetId				int									NOT NULL,
	VehicleId			int									NOT NULL,
	ParkingStatusId		int									NOT NULL,
	ParkingSpotId		int									NOT NULL,
	TimeIn				datetime							NOT NULL,
	TimeOut				datetime							NULL,
	AmountDue			decimal(13,2)						NULL,
	AmountPaid			decimal(13,2)						NULL,
	CONSTRAINT PK_ParkingSlips PRIMARY KEY(Id),
	-- CONSTRAINT FK_ParkingSlips_Users FOREIGN KEY(ValetId) REFERENCES Users(Id),
	CONSTRAINT FK_ParkingSlips_Vehicles FOREIGN KEY(VehicleId) REFERENCES Vehicles(Id),
	CONSTRAINT FK_ParkingSlips_ParkingStatuses FOREIGN KEY(ParkingStatusId) REFERENCES ParkingStatuses(Id),
	CONSTRAINT FK_ParkingSlips_ParkingSpots FOREIGN KEY(ParkingSpotId) REFERENCES ParkingSpots(Id),
)
GO

CREATE TABLE ParkingSlipsDiscounts (
	ParkingSlipId		int									NOT NULL,
	DiscountId			int									NOT NULL,
	CONSTRAINT PK_ParkingSlipsDiscounts PRIMARY KEY CLUSTERED(ParkingSlipId, DiscountId),
	CONSTRAINT FK_ParkingSlipsDiscounts_ParkingSlips FOREIGN KEY(ParkingSlipId) REFERENCES ParkingSlips(Id),
	CONSTRAINT FK_ParkingSlipsDiscounts_Discounts FOREIGN KEY(DiscountId) REFERENCES Discounts(Id),
)
GO
