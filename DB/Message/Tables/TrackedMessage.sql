CREATE TABLE [Message].[TrackedMessage] (
    [Id]                   BIGINT           NOT NULL,
    [TrackDateTime]        DATETIME         NOT NULL,
    [MessageId]            BIGINT           NOT NULL,
    [MessageBody]          VARCHAR (MAX)    NOT NULL,
    [ProcessingSequenceId] UNIQUEIDENTIFIER NULL,
    [StageSequence]        INT              NULL,
    [MessageSequence]      INT              NULL,
    [StageName]            VARCHAR (100)    NULL,
    [Exception]            VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_TrackedMessage] PRIMARY KEY CLUSTERED ([Id] ASC)
);

