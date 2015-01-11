CREATE TABLE [Tracking].[MessageAttribute] (
    [TrackedMessageId] BIGINT        NOT NULL,
    [Attribute]        VARCHAR (100) NOT NULL,
    [Value]            VARCHAR (MAX) NULL,
    CONSTRAINT [PK_TrackedMessageAttribute] PRIMARY KEY CLUSTERED ([TrackedMessageId] ASC, [Attribute] ASC),
    CONSTRAINT [FK_TrackedMessageAttribute_TrackedMessage] FOREIGN KEY ([TrackedMessageId]) REFERENCES [Tracking].[Message] ([Id]) ON DELETE CASCADE
);

