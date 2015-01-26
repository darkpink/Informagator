CREATE TABLE [Configuration].[WorkerErrorHandler] (
    [Id]             BIGINT IDENTITY (1, 1) NOT NULL,
    [WorkerId]       BIGINT NOT NULL,
    [ErrorHandlerId] BIGINT NOT NULL,
    CONSTRAINT [PK_WorkerErrorHandler] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WorkerErrorHandler_ErrorHandler] FOREIGN KEY ([ErrorHandlerId]) REFERENCES [Configuration].[ErrorHandler] ([Id]),
    CONSTRAINT [FK_WorkerErrorHandler_Worker] FOREIGN KEY ([WorkerId]) REFERENCES [Configuration].[Worker] ([Id]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_WorkerId_ErrorHandlerId]
    ON [Configuration].[WorkerErrorHandler]([WorkerId] ASC, [ErrorHandlerId] ASC);

