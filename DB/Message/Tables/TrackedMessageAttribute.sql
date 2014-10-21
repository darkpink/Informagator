CREATE TABLE [Message].[TrackedMessageAttribute] (
    [TrackedMessageId] BIGINT        NOT NULL,
    [Attribute]        VARCHAR (100) NOT NULL,
    [Value]            VARCHAR (MAX) NULL,
    CONSTRAINT [PK_TrackedMessageAttribute] PRIMARY KEY CLUSTERED ([TrackedMessageId] ASC, [Attribute] ASC),
    CONSTRAINT [FK_TrackedMessageAttribute_TrackedMessage] FOREIGN KEY ([TrackedMessageId]) REFERENCES [Message].[TrackedMessage] ([Id])
);

