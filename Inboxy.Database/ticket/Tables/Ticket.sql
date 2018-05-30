CREATE TABLE [Ticket].[Ticket]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Subject] nvarchar(500) null,
	[AssigneeUserId] int null,
	[CreatedByUserId] int null, 
    [CreatedOn] DATETIME NOT NULL, 
    [RequesterUserId] INT NOT NULL, 
    [StatusId] INT NOT NULL, 
    [LinkedFolderId] INT  NULL, 
	[InboxId] int not null,
	[Priority] smallInt ,
	[Type] smallInt ,
    CONSTRAINT [FK_ticket_ToRequester] FOREIGN KEY ([RequesterUserId]) REFERENCES Ticket.RequesterUser([Id])
)
