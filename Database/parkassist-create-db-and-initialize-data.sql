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

-- optional data
INSERT INTO Users (Username,PasswordHash,Salt,FirstName,LastName,Email,Phone,CreateDate,UpdateDate) VALUES ('brian','placeholder','placeholder','Brian','LeMaster','brian@email.com','1111111111',GETDATE(),NULL);
INSERT INTO Users (Username,PasswordHash,Salt,FirstName,LastName,Email,Phone,CreateDate,UpdateDate) VALUES ('oscar','placeholder','placeholder','Oscar','Petersen','oscar@email.com','2222222222',GETDATE(),NULL);
INSERT INTO Users (Username,PasswordHash,Salt,FirstName,LastName,Email,Phone,CreateDate,UpdateDate) VALUES ('george','placeholder','placeholder','George','Rodriguez','george@email.com','3333333333',GETDATE(),NULL);
INSERT INTO Users (Username,PasswordHash,Salt,FirstName,LastName,Email,Phone,CreateDate,UpdateDate) VALUES ('bernice','placeholder','placeholder','Bernice','Anderson','bernice@email.com','4444444444',GETDATE(),NULL);
INSERT INTO Users (Username,PasswordHash,Salt,FirstName,LastName,Email,Phone,CreateDate,UpdateDate) VALUES ('doug','placeholder','placeholder','Doug','Smith','doug@email.com','5555555555',GETDATE(),NULL);
INSERT INTO Users (Username,PasswordHash,Salt,FirstName,LastName,Email,Phone,CreateDate,UpdateDate) VALUES ('sarah','placeholder','placeholder','Sarah','Furman','sarah@email.com','6666666666',GETDATE(),NULL);
INSERT INTO Users (Username,PasswordHash,Salt,FirstName,LastName,Email,Phone,CreateDate,UpdateDate) VALUES ('ari','placeholder','placeholder','Ari','Echols','ari@email.com','7777777777',GETDATE(),NULL);
INSERT INTO Users (Username,PasswordHash,Salt,FirstName,LastName,Email,Phone,CreateDate,UpdateDate) VALUES ('wanda','placeholder','placeholder','Wanda','Waterson','wanda@email.com','8888888888',GETDATE(),NULL);

CREATE TABLE Owners (
	Id					int IDENTITY(1,1)					NOT NULL,
	UserId				int									NOT NULL,
	CONSTRAINT PK_Owners PRIMARY KEY(Id),
	CONSTRAINT UC_OwnersUserId UNIQUE(UserId),
	CONSTRAINT FK_Owners_Users FOREIGN KEY(UserId) REFERENCES Users(Id),
)
GO

-- optional data
INSERT INTO Owners (UserId) VALUES ((SELECT u.Id FROM Users u WHERE u.Username = 'sarah'));
INSERT INTO Owners (UserId) VALUES ((SELECT u.Id FROM Users u WHERE u.Username = 'ari'));

CREATE TABLE Customers (
	Id					int IDENTITY(1,1)					NOT NULL,
	UserId				int									NOT NULL,
	CONSTRAINT PK_Customers PRIMARY KEY(Id),
	CONSTRAINT UC_CustomersUserId UNIQUE(UserId),
	CONSTRAINT FK_Customers_Users FOREIGN KEY(UserId) REFERENCES Users(Id), 
)
GO

-- optional data
INSERT INTO Customers (UserId) VALUES ((SELECT u.Id FROM Users u WHERE u.Username = 'brian'));
INSERT INTO Customers (UserId) VALUES ((SELECT u.Id FROM Users u WHERE u.Username = 'oscar'));

CREATE TABLE Admins (
	Id					int IDENTITY(1,1)					NOT NULL,
	UserId				int									NOT NULL,
	CONSTRAINT PK_Admins PRIMARY KEY(Id),
	CONSTRAINT UC_AdminsUserId UNIQUE(UserId),
	CONSTRAINT FK_Admins_Users FOREIGN KEY(UserId) REFERENCES Users(Id),
)
GO

-- optional data
INSERT INTO Admins (UserId) VALUES ((SELECT u.Id FROM Users u WHERE u.Username = 'bernice'));
INSERT INTO Admins (UserId) VALUES ((SELECT u.Id FROM Users u WHERE u.Username = 'doug'));

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

-- optional data
INSERT INTO ParkingLots (OwnerId,Name,Address,City,[State],Zip,CreateDate,UpdateDate) VALUES ((SELECT o.Id FROM Owners o INNER JOIN Users u ON (o.UserId = u.Id) WHERE u.Username = 'sarah'),'A1 Parking','123 Fake Street','Cincinnati','Ohio','45111',GETDATE(),NULL);
INSERT INTO ParkingLots (OwnerId,Name,Address,City,[State],Zip,CreateDate,UpdateDate) VALUES ((SELECT o.Id FROM Owners o INNER JOIN Users u ON (o.UserId = u.Id) WHERE u.Username = 'ari'),'Central Parking','One Main Parkway','Cincinnati','Ohio','45111',GETDATE(),NULL);
INSERT INTO ParkingLots (OwnerId,Name,Address,City,[State],Zip,CreateDate,UpdateDate) VALUES ((SELECT o.Id FROM Owners o INNER JOIN Users u ON (o.UserId = u.Id) WHERE u.Username = 'sarah'),'Express Parking','987 Court Street','Cincinnati','Ohio','45111',GETDATE(),NULL);
INSERT INTO ParkingLots (OwnerId,Name,Address,City,[State],Zip,CreateDate,UpdateDate) VALUES ((SELECT o.Id FROM Owners o INNER JOIN Users u ON (o.UserId = u.Id) WHERE u.Username = 'ari'),'Skyline Parking','2394 Uptown Boulevard','Cincinnati','Ohio','45111',GETDATE(),NULL);

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

-- optional data
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'1',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'2',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'3',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'4',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'5',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'6',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'7',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'8',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'9',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'10',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'11',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'A1',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'A2',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'A3',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'A4',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'A5',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'A6',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'A7',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'A8',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'A9',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'A10',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'A11',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'1A',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'2A',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'3A',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'4A',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'1B',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'2B',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'3B',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'4B',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'1C',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'2C',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'3C',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'Alpha',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'Bravo',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'Charlie',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'Delta',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'Echo',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'Foxtrot',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'Golf',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'Hotel',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'Indigo',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'Juliet',GETDATE(),NULL);
INSERT INTO ParkingSpots (ParkingLotId,Name,CreateDate,UpdateDate) VALUES ((SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'A1 Parking'),'Kilo',GETDATE(),NULL);


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


-- optional data
INSERT INTO Valets (UserId, ParkingLotId) VALUES ((SELECT u.Id FROM Users u WHERE u.Username = 'george'),(SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'Skyline Parking'));
INSERT INTO Valets (UserId, ParkingLotId) VALUES ((SELECT u.Id FROM Users u WHERE u.Username = 'wanda'),(SELECT pl.Id FROM ParkingLots pl WHERE pl.Name = 'Express Parking'));

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

-- optional data
INSERT INTO Vehicles (CustomerId,Make,Model,[Year],LicensePlate,Color,CreateDate,UpdateDate) VALUES((SELECT c.Id FROM Customers c INNER JOIN Users u ON (c.UserId = u.Id) WHERE u.Username = 'brian'),'Mazda','CX-5','2018','AAA111','Black',GETDATE(),NULL);
INSERT INTO Vehicles (CustomerId,Make,Model,[Year],LicensePlate,Color,CreateDate,UpdateDate) VALUES((SELECT c.Id FROM Customers c INNER JOIN Users u ON (c.UserId = u.Id) WHERE u.Username = 'oscar'),'Subaru','Outback','2015','BBB222','Silver',GETDATE(),NULL);
INSERT INTO Vehicles (CustomerId,Make,Model,[Year],LicensePlate,Color,CreateDate,UpdateDate) VALUES((SELECT c.Id FROM Customers c INNER JOIN Users u ON (c.UserId = u.Id) WHERE u.Username = 'brian'),'Honda','Accord','2020','CCC333','Blue',GETDATE(),NULL);
INSERT INTO Vehicles (CustomerId,Make,Model,[Year],LicensePlate,Color,CreateDate,UpdateDate) VALUES((SELECT c.Id FROM Customers c INNER JOIN Users u ON (c.UserId = u.Id) WHERE u.Username = 'brian'),'Porche','Cayenne','2023','DDD444','Silver',GETDATE(),NULL);
INSERT INTO Vehicles (CustomerId,Make,Model,[Year],LicensePlate,Color,CreateDate,UpdateDate) VALUES((SELECT c.Id FROM Customers c INNER JOIN Users u ON (c.UserId = u.Id) WHERE u.Username = 'oscar'),'Nissan','Altima','2011','EEE555','Gray',GETDATE(),NULL);
INSERT INTO Vehicles (CustomerId,Make,Model,[Year],LicensePlate,Color,CreateDate,UpdateDate) VALUES((SELECT c.Id FROM Customers c INNER JOIN Users u ON (c.UserId = u.Id) WHERE u.Username = 'oscar'),'Toyota','Highlander','2017','FFF666','Red',GETDATE(),NULL);

CREATE TABLE ParkingStatuses (
	Id					int IDENTITY(1,1)					NOT NULL,
	ParkingStatus		varchar(20)							NOT NULL,
	CONSTRAINT PK_ParkingStatuses PRIMARY KEY(Id),
	CONSTRAINT UC_ParkingStatus UNIQUE(ParkingStatus),
)
GO

-- REQUIRED DATA
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

-- optional data
INSERT INTO Discounts (Type,Description,Multiplier) VALUES ('Military','Discount for active and former military, ID required',0.85);
INSERT INTO Discounts (Type,Description,Multiplier) VALUES ('Senior','Discount for seniors aged 55+, ID required',0.9);
INSERT INTO Discounts (Type,Description,Multiplier) VALUES ('Student','Discount for college students, ID required',0.93);

CREATE TABLE ParkingSlips (
	Id					int IDENTITY(1,1)					NOT NULL,
	ValetId				int									NOT NULL,
	VehicleId			int									NOT NULL,
	ParkingStatusId		int									NOT NULL,
	ParkingSpotId		int									NOT NULL,
	TimeIn				datetime							NOT NULL,
	TimeOut				datetime							NULL,
	AmountDue			decimal(13,2)						NULL,
	CreateDate			datetime							NOT NULL,
	AmountPaid			decimal(13,2)						NULL,
	UpdateDate			datetime							NULL,	
	CONSTRAINT PK_ParkingSlips PRIMARY KEY(Id),
	-- CONSTRAINT FK_ParkingSlips_Valets FOREIGN KEY(ValetId) REFERENCES Valets(Id),
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
