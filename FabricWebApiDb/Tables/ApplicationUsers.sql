CREATE TABLE [dbo].[ApplicationUsers]
(
	[Id]			INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Username]		NVARCHAR(50) NOT NULL,
	[Password]		NVARCHAR(MAX) NOT NULL,
	[IsBlocked]		BIT NULL
)
