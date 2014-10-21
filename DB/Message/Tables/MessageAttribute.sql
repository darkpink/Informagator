CREATE TABLE [Message].[MessageAttribute] (
    [MessageId] BIGINT        NOT NULL,
    [Attribute] VARCHAR (100) NOT NULL,
    [Value]     VARCHAR (MAX) NULL,
    CONSTRAINT [PK_MessageAttribute] PRIMARY KEY CLUSTERED ([MessageId] ASC, [Attribute] ASC),
    CONSTRAINT [FK_MessageAttribute_Message] FOREIGN KEY ([MessageId]) REFERENCES [Message].[Message] ([Id])
);

