CREATE TABLE [dbo].[LinkedFolder]
(
	[Id] INT NOT NULL IDENTITY (1, 1) PRIMARY KEY CLUSTERED,
	CreatedOn DATETIME NOT NULL,
	[InboxId] INT NOT NULL,
	[Name] NVARCHAR(200) NULL,
	[NewItemsFolder] NVARCHAR(200) NOT NULL,
	[ProcessedItemsFolder] NVARCHAR(200) NOT NULL,
	INDEX UX_LinkedFolder_InboxId_NewItemsFolder UNIQUE (InboxId, NewItemsFolder),
	CONSTRAINT FK_LinkedFolder_InboxId FOREIGN KEY (InboxId) REFERENCES Inbox(Id)
)
