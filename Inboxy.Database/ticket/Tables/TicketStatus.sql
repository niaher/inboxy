CREATE TABLE [Ticket].[TicketStatus]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL,
	[InboxId] int NOT NULL 
)
