USE [HotelixOfferDb];

CREATE TABLE Location(
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar(20) NOT NULL,
	Description nvarchar(200) NOT NULL,
	Address nvarchar(100) NOT NULL,
	City nvarchar(50) NOT NULL,
	PostalCode nvarchar(6) NOT NULL,
	IsHidden bit NOT NULL DEFAULT 0,
	CONSTRAINT PK_Location PRIMARY KEY CLUSTERED (Id)
);

GO

USE [HotelixReservationDb];

CREATE TABLE Location(
	Id int NOT NULL,
	Name nvarchar(20) NOT NULL,
	Description nvarchar(200) NOT NULL,
	Address nvarchar(100) NOT NULL,
	City nvarchar(50) NOT NULL,
	PostalCode nvarchar(6) NOT NULL,
	CONSTRAINT PK_Location PRIMARY KEY CLUSTERED (Id)
);

GO