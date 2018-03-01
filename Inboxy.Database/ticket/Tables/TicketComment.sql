CREATE TABLE [Ticket].[TicketComment]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Comment] NVARCHAR(MAX) NOT NULL, 
    [EmailId] INT NULL, 
    [TicketId] INT NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    CONSTRAINT [FK_TicketComment_ToTicket] FOREIGN KEY (TicketId) REFERENCES [Ticket].[Ticket]([Id])
)
