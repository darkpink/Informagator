CREATE TABLE [Tracking].[Message] (
    [Id]                    BIGINT           NOT NULL,
    [MessageId]             UNIQUEIDENTIFIER NOT NULL,
    [ProcessingSequenceId]  UNIQUEIDENTIFIER NOT NULL,
    [TrackDateTime]         DATETIME         NOT NULL,
    [MachineName]           VARCHAR (100)    NULL,
    [WorkerName]            VARCHAR (100)    NULL,
    [StageName]             VARCHAR (100)    NULL,
    [StageSequence]         INT              NULL,
    [InputMessageSequence]  INT              NULL,
    [OutputMessageSequence] INT              NULL,
    [MessageBody]           VARCHAR (MAX)    NOT NULL,
    [Exception]             VARCHAR (MAX)    NULL,
    [Info]                  VARCHAR (MAX)    NULL,
    CONSTRAINT [PK_TrackedMessage] PRIMARY KEY CLUSTERED ([Id] ASC)
);



