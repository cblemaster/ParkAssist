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
	UserId				int IDENTITY(1,1)					NOT NULL,
	Username			varchar(50)							NOT NULL,
	PasswordHash		varchar(200)						NOT NULL,
	Salt				varchar(200)						NOT NULL,
	FirstName			varchar(255)						NOT NULL,
	LastName			varchar(255)						NOT NULL,
	Email				varchar(255)						NOT NULL,
	Phone				varchar(10)							NOT NULL,
	CreateDate			datetime							NOT NULL,
	UpdateDate			datetime							NULL,
	CONSTRAINT PK_Users PRIMARY KEY(UserId),
	CONSTRAINT UC_Username UNIQUE(Username),
	CONSTRAINT UC_Email UNIQUE(Email),
	CONSTRAINT UC_Phone UNIQUE(Phone),
)
GO

-- optional sample data
INSERT INTO Users(Username, PasswordHash, Salt, FirstName, LastName, Email, Phone, CreateDate, UpdateDate)
	VALUES('brian', 'placeholder', 'placeholder', 'Brian', 'LeMaster', 'brian@email.com', '1111111111', GETDATE(), NULL);

INSERT INTO Users(Username, PasswordHash, Salt, FirstName, LastName, Email, Phone, CreateDate, UpdateDate)
	VALUES('oscar', 'placeholder', 'placeholder', 'Oscar', 'Rodriguez', 'oscar@email.com', '2222222222', GETDATE(), NULL);

INSERT INTO Users(Username, PasswordHash, Salt, FirstName, LastName, Email, Phone, CreateDate, UpdateDate)
	VALUES('george', 'placeholder', 'placeholder', 'George', 'Simpson', 'george@email.com', '3333333333', GETDATE(), NULL);

INSERT INTO Users(Username, PasswordHash, Salt, FirstName, LastName, Email, Phone, CreateDate, UpdateDate)
	VALUES('bernice', 'placeholder', 'placeholder', 'Bernice', 'Andersen', 'bernice@email.com', '4444444444', GETDATE(), NULL);

INSERT INTO Users(Username, PasswordHash, Salt, FirstName, LastName, Email, Phone, CreateDate, UpdateDate)
	VALUES('ari', 'placeholder', 'placeholder', 'Ari', 'Pfeifer', 'ari@email.com', '5555555555', GETDATE(), NULL);

INSERT INTO Users(Username, PasswordHash, Salt, FirstName, LastName, Email, Phone, CreateDate, UpdateDate)
	VALUES('sarah', 'placeholder', 'placeholder', 'Sarah', 'Liebowitz', 'sarah@email.com', '6666666666', GETDATE(), NULL);

INSERT INTO Users(Username, PasswordHash, Salt, FirstName, LastName, Email, Phone, CreateDate, UpdateDate)
	VALUES('wanda', 'placeholder', 'placeholder', 'Wanda', 'Waterson', 'wanda@email.com', '7777777777', GETDATE(), NULL);

INSERT INTO Users(Username, PasswordHash, Salt, FirstName, LastName, Email, Phone, CreateDate, UpdateDate)
	VALUES('siggy', 'placeholder', 'placeholder', 'Siggy', 'Ferdinand', 'siggy@email.com', '8888888888', GETDATE(), NULL);
--

CREATE TABLE Customers (
	CustomerId			int IDENTITY(1,1)					NOT NULL,
	UserId				int									NOT NULL,
	CONSTRAINT PK_Customers PRIMARY KEY(CustomerId),
	CONSTRAINT FK_Customers_Users FOREIGN KEY(UserId) REFERENCES Users(UserId),
	CONSTRAINT UC_CustomerUserId UNIQUE(UserId),
)
GO

-- optional sample data
INSERT INTO Customers(UserId)
	VALUES((SELECT u.UserId FROM Users u WHERE u.Username = 'bernice'));

INSERT INTO Customers(UserId)
	VALUES((SELECT u.UserId FROM Users u WHERE u.Username = 'brian'));

INSERT INTO Customers(UserId)
	VALUES((SELECT u.UserId FROM Users u WHERE u.Username = 'oscar'));

INSERT INTO Customers(UserId)
	VALUES((SELECT u.UserId FROM Users u WHERE u.Username = 'wanda'));
--

CREATE TABLE Owners (
	OwnerId				int IDENTITY(1,1)					NOT NULL,
	UserId				int									NOT NULL,
	CONSTRAINT PK_Owners PRIMARY KEY(OwnerId),
	CONSTRAINT FK_Owners_Users FOREIGN KEY(UserId) REFERENCES Users(UserId),
	CONSTRAINT UC_OwnerUserId UNIQUE(UserId),
)
GO

-- optional sample data
INSERT INTO Owners(UserId)
	VALUES((SELECT u.UserId FROM Users u WHERE u.Username = 'ari'));

INSERT INTO Owners(UserId)
	VALUES((SELECT u.UserId FROM Users u WHERE u.Username = 'sarah'));
--

CREATE TABLE PricingSchedules (
	Id					int IDENTITY(1,1)					NOT NULL,
	WeekdayRate			decimal								NOT NULL,
	WeekendRate			decimal								NOT NULL,
	SpecialEventRate	decimal								NOT NULL,
	CONSTRAINT PK_PricingSchedules PRIMARY KEY(Id),
)
GO

-- optional sample data
INSERT INTO PricingSchedules(WeekdayRate, WeekendRate, SpecialEventRate)
	VALUES(5.99, 7.99, 12.99);

INSERT INTO PricingSchedules(WeekdayRate, WeekendRate, SpecialEventRate)
	VALUES(6.99, 8.99, 13.99);

INSERT INTO PricingSchedules(WeekdayRate, WeekendRate, SpecialEventRate)
	VALUES(4.99, 6.99, 11.99);
--

CREATE TABLE ParkingLots (
	Id					int IDENTITY(1,1)					NOT NULL,
	OwnerId				int									NOT NULL,
	PricingScheduleID	int									NOT NULL,
	Name                varchar(255)						NOT NULL,
	Address				varchar(255)						NOT NULL,
	City				varchar(255)						NOT NULL,
	State				varchar(255)						NOT NULL,	
	Zip					varchar(10)							NOT NULL,
	CreateDate			datetime							NOT NULL,
	UpdateDate			datetime							NULL,
	CONSTRAINT PK_ParkingLots PRIMARY KEY(Id),
	CONSTRAINT FK_ParkingLots_Owners FOREIGN KEY(OwnerId) REFERENCES Owners(OwnerId),
	CONSTRAINT FK_ParkingLots_PricingSchedules FOREIGN KEY(PricingScheduleId) REFERENCES PricingSchedules(Id),
	CONSTRAINT UC_Name UNIQUE(Name),
)
GO

-- optional sample data
INSERT INTO ParkingLots (OwnerId, PricingScheduleID, Name, Address, City, State, Zip, CreateDate, UpdateDate)
	VALUES((SELECT o.OwnerId FROM Owners o INNER JOIN Users u ON (o.UserId = u.UserId) WHERE u.Username = 'ari'),(SELECT p.Id FROM PricingSchedules p WHERE p.Id = 1), 'A1 Parking', '123 Fake Street', 'Cincinnati', 'Ohio', '45111', GETDATE(), NULL);

INSERT INTO ParkingLots (OwnerId, PricingScheduleID, Name, Address, City, State, Zip, CreateDate, UpdateDate)
	VALUES((SELECT o.OwnerId FROM Owners o INNER JOIN Users u ON (o.UserId = u.UserId) WHERE u.Username = 'sarah'),(SELECT p.Id FROM PricingSchedules p WHERE p.Id = 2), 'EZ Parking', 'One Main Boulevard', 'Cincinnati', 'Ohio', '45111', GETDATE(), NULL);

INSERT INTO ParkingLots (OwnerId, PricingScheduleID, Name, Address, City, State, Zip, CreateDate, UpdateDate)
	VALUES((SELECT o.OwnerId FROM Owners o INNER JOIN Users u ON (o.UserId = u.UserId) WHERE u.Username = 'sarah'),(SELECT p.Id FROM PricingSchedules p WHERE p.Id = 3), 'Court Street Parking', '65313 Court Street', 'Cincinnati', 'Ohio', '45111', GETDATE(), NULL);
--

CREATE TABLE Valets (
	ValetId				int IDENTITY(1,1)					NOT NULL,
	UserId				int									NOT NULL,
	ParkingLotId		int									NOT NULL,
	CONSTRAINT PK_Valets PRIMARY KEY(ValetId),
	CONSTRAINT FK_Valets_Users FOREIGN KEY(UserId) REFERENCES Users(UserId),
	CONSTRAINT FK_Valets_ParkingLots FOREIGN KEY(ParkingLotId) REFERENCES ParkingLots(Id),
	CONSTRAINT UC_ValetUserId UNIQUE(UserId),
)
GO

-- optional sample data
INSERT INTO Valets(UserId, ParkingLotId)
	VALUES((SELECT u.UserId FROM Users u WHERE u.Username = 'george'),(SELECT p.Id FROM ParkingLots p WHERE p.Name = 'A1 Parking'));

INSERT INTO Valets(UserId, ParkingLotId)
	VALUES((SELECT u.UserId FROM Users u WHERE u.Username = 'siggy'),(SELECT p.Id FROM ParkingLots p WHERE p.Name = 'EZ Parking'));
--

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

-- optional sample data
INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'A1 Parking'), 'A1', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'A1 Parking'), 'A2', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'A1 Parking'), 'A3', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'A1 Parking'), 'B1', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'A1 Parking'), 'B2', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'A1 Parking'), 'B3', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'A1 Parking'), 'C1', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'A1 Parking'), 'C2', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'A1 Parking'), 'C3', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'A1 Parking'), 'D1', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'A1 Parking'), 'D2', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'A1 Parking'), 'D3', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'A1 Parking'), 'D4', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'EZ Parking'), '1', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'EZ Parking'), '2', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'EZ Parking'), '3', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'EZ Parking'), '4', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'EZ Parking'), '5', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'EZ Parking'), '6', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'EZ Parking'), '7', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'EZ Parking'), '8', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'EZ Parking'), '9', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'EZ Parking'), '10', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'EZ Parking'), '11', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'Court Street Parking'), 'Alpha', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'Court Street Parking'), 'Bravo', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'Court Street Parking'), 'Charlie', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'Court Street Parking'), 'Delta', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'Court Street Parking'), 'Echo', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'Court Street Parking'), 'Foxtrot', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'Court Street Parking'), 'Golf', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'Court Street Parking'), 'Hotel', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'Court Street Parking'), 'Indigo', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'Court Street Parking'), 'Juliet', GETDATE(), NULL);

INSERT INTO ParkingSpots(ParkingLotId, Name, CreateDate, UpdateDate)
	VALUES((SELECT p.Id FROM ParkingLots p WHERE p.Name = 'Court Street Parking'), 'Kilo', GETDATE(), NULL);
--

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
	CONSTRAINT FK_Vehicles_Customers FOREIGN KEY(CustomerId) REFERENCES Customers(CustomerId),
	CONSTRAINT UC_LicensePlate UNIQUE(LicensePlate),
)
GO

-- optional sample data
INSERT INTO Vehicles(CustomerId, Make, Model, [Year], LicensePlate, Color, CreateDate, UpdateDate)
	VALUES((SELECT c.CustomerId FROM Customers c INNER JOIN Users u ON (c.UserId = u.UserId) WHERE u.Username = 'bernice'), 'Nissan', 'Altima', '2015', 'AAA111', 'Black', GETDATE(), NULL);

INSERT INTO Vehicles(CustomerId, Make, Model, [Year], LicensePlate, Color, CreateDate, UpdateDate)
	VALUES((SELECT c.CustomerId FROM Customers c INNER JOIN Users u ON (c.UserId = u.UserId) WHERE u.Username = 'bernice'), 'Subaru', 'Forester', '2019', 'BBB222', 'Silver', GETDATE(), NULL);

INSERT INTO Vehicles(CustomerId, Make, Model, [Year], LicensePlate, Color, CreateDate, UpdateDate)
	VALUES((SELECT c.CustomerId FROM Customers c INNER JOIN Users u ON (c.UserId = u.UserId) WHERE u.Username = 'brian'), 'Mazda', 'CX-5', '2018', 'CCC333', 'Black', GETDATE(), NULL);

INSERT INTO Vehicles(CustomerId, Make, Model, [Year], LicensePlate, Color, CreateDate, UpdateDate)
	VALUES((SELECT c.CustomerId FROM Customers c INNER JOIN Users u ON (c.UserId = u.UserId) WHERE u.Username = 'brian'), 'Toyota', 'Highlander', '2020', 'DDD444', 'Blue', GETDATE(), NULL);

INSERT INTO Vehicles(CustomerId, Make, Model, [Year], LicensePlate, Color, CreateDate, UpdateDate)
	VALUES((SELECT c.CustomerId FROM Customers c INNER JOIN Users u ON (c.UserId = u.UserId) WHERE u.Username = 'brian'), 'Honda', 'Civic', '2023', 'EEE555', 'Gray', GETDATE(), NULL);

INSERT INTO Vehicles(CustomerId, Make, Model, [Year], LicensePlate, Color, CreateDate, UpdateDate)
	VALUES((SELECT c.CustomerId FROM Customers c INNER JOIN Users u ON (c.UserId = u.UserId) WHERE u.Username = 'oscar'), 'Kia', 'Sportage', '2024', 'FFF666', 'Red', GETDATE(), NULL);

INSERT INTO Vehicles(CustomerId, Make, Model, [Year], LicensePlate, Color, CreateDate, UpdateDate)
	VALUES((SELECT c.CustomerId FROM Customers c INNER JOIN Users u ON (c.UserId = u.UserId) WHERE u.Username = 'oscar'), 'Ford', 'Ranger', '2009', 'GGG777', 'Green', GETDATE(), NULL);

INSERT INTO Vehicles(CustomerId, Make, Model, [Year], LicensePlate, Color, CreateDate, UpdateDate)
	VALUES((SELECT c.CustomerId FROM Customers c INNER JOIN Users u ON (c.UserId = u.UserId) WHERE u.Username = 'wanda'), 'Mazda', 'CX-90', '2024', 'HHH888', 'Blue', GETDATE(), NULL);

INSERT INTO Vehicles(CustomerId, Make, Model, [Year], LicensePlate, Color, CreateDate, UpdateDate)
	VALUES((SELECT c.CustomerId FROM Customers c INNER JOIN Users u ON (c.UserId = u.UserId) WHERE u.Username = 'wanda'), 'Suzuki', 'Esteem', '2007', 'III999', 'Yellow', GETDATE(), NULL);
--

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
--

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
--

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
	CreateDate			datetime							NOT NULL,
	UpdateDate			datetime							NULL,
	CONSTRAINT PK_ParkingSlips PRIMARY KEY(Id),
	CONSTRAINT FK_ParkingSlips_Valets FOREIGN KEY(ValetId) REFERENCES Valets(ValetId),
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
