CREATE TABLE [Configuration].[WorkerParameter] (
    [Id]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [WorkerId] BIGINT        NOT NULL,
    [Name]     VARCHAR (200) NOT NULL,
    [Value]    VARCHAR (MAX) NULL,
    CONSTRAINT [PK_WorkerParameter] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WorkerParameter_Worker] FOREIGN KEY ([WorkerId]) REFERENCES [Configuration].[Worker] ([Id]) ON DELETE CASCADE
);

