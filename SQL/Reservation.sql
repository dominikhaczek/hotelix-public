USE [HotelixReservationsDb];

CREATE TABLE Reservation(
	Id int IDENTITY(1,1) NOT NULL,
	UserId nvarchar(450) NOT NULL,
	RoomId int NOT NULL,
	StartTime datetime NOT NULL,
	EndTime datetime NOT NULL,

	LocationId int NOT NULL,
	Name nvarchar(50) NOT NULL,
	PricePerNight decimal(11,4) NOT NULL,
	GuestLimit int NOT NULL,
	BedType nvarchar(50) NOT NULL,
	Description nvarchar(500) NOT NULL,
	ImageUrl varchar(2048) NOT NULL

	CONSTRAINT PK_Reservation PRIMARY KEY CLUSTERED (Id),
	CONSTRAINT FK_ReservationClient FOREIGN KEY (UserId) REFERENCES Client(UserId),
	CONSTRAINT FK_ReservationLocation FOREIGN KEY (LocationId) REFERENCES Location(Id)
);