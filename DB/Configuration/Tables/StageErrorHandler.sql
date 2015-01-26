CREATE TABLE [Configuration].[StageErrorHandler] (
    [Id]             BIGINT IDENTITY (1, 1) NOT NULL,
    [StageId]        BIGINT NOT NULL,
    [ErrorHandlerId] BIGINT NOT NULL,
    CONSTRAINT [PK_StageErrorHandler] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StageErrorHandler_ErrorHandler] FOREIGN KEY ([ErrorHandlerId]) REFERENCES [Configuration].[ErrorHandler] ([Id]),
    CONSTRAINT [FK_StageErrorHandler_Stage] FOREIGN KEY ([StageId]) REFERENCES [Configuration].[Stage] ([Id]) ON DELETE CASCADE
);

