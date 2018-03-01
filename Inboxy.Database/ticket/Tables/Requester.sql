CREATE TABLE [ticket].[Requester]
(
	[Id] INT NOT NULL  IDENTITY, 
    [RequesterUserId] INT NOT NULL, 
    [TicketCommentId] INT NOT NULL, 
    CONSTRAINT [PK_Requester] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Requester_ToTicketComment] FOREIGN KEY (TicketCommentId) REFERENCES [ticket].[TicketComment]([Id])
)
