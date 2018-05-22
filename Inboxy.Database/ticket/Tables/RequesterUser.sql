CREATE TABLE [Ticket].[RequesterUser]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(250) NOT NULL, 
    [Email] NVARCHAR(50) NOT NULL, 
    [LinkedFolderId] INT NOT NULL
)
