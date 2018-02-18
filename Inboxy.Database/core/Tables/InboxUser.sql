CREATE TABLE [dbo].[InboxUser]
(
	[InboxId] INT NOT NULL,
	[UserId] INT NOT NULL,
	CONSTRAINT PK_InboxUser_InboxId_UserId PRIMARY KEY CLUSTERED (InboxId, UserId),
	INDEX IX_InboxUser_UserId (UserId)
)
