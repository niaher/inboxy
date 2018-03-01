CREATE TABLE [Ticket].[ticket]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[AssigneeUserId] int null,
	[CreatedByUserId] int null, 
    [CreatedOn] DATETIME NOT NULL, 
    [RequesterId] INT NOT NULL, 
    [Status] INT NOT NULL, 
    [LinkedFolderId] INT NOT NULL, 
    CONSTRAINT [FK_ticket_ToRequester] FOREIGN KEY (RequesterId) REFERENCES Ticket.Requester([Id])
)
