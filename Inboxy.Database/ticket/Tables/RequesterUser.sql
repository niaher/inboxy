CREATE TABLE [Ticket].[RequesterUser]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Email] NVARCHAR(50) NOT NULL, 
    [LinkedFolderId] INT NOT NULL
)
