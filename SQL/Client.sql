USE [HotelixReservationsDb]

CREATE TABLE [Client](
	UserId nvarchar(450) NOT NULL,
	Name nvarchar(50) NOT NULL,
	Surname nvarchar(50) NOT NULL,
	Address nvarchar(100) NOT NULL,
	City nvarchar(50) NOT NULL,
	PostalCode nvarchar(6) NOT NULL
	CONSTRAINT PK_Client PRIMARY KEY CLUSTERED (UserId)
);