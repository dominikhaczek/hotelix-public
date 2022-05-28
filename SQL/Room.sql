USE [HotelixOfferDb];

CREATE TABLE Room(
	Id int IDENTITY(1,1) NOT NULL,
	LocationId int NOT NULL,
	Name nvarchar(50) NOT NULL,
	PricePerNight decimal(11,4) NOT NULL,
	GuestLimit int NOT NULL,
	BedType nvarchar(50) NOT NULL,
	Description nvarchar(500) NOT NULL,
	ImageUrl varchar(2048) NOT NULL,
	StartTime datetime NULL,
	EndTime datetime NULL,
	IsHidden bit NOT NULL DEFAULT 0
	CONSTRAINT PK_Room PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT FK_RoomLocation
	FOREIGN KEY (LocationId) REFERENCES Location(Id)
);