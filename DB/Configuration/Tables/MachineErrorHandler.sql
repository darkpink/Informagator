CREATE TABLE [Configuration].[MachineErrorHandler] (
    [Id]             BIGINT IDENTITY (1, 1) NOT NULL,
    [MachineId]      BIGINT NOT NULL,
    [ErrorHandlerId] BIGINT NOT NULL,
    CONSTRAINT [PK_MachineErrorHandler] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_MachineErrorHandler_ErrorHandler] FOREIGN KEY ([ErrorHandlerId]) REFERENCES [Configuration].[ErrorHandler] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MachineErrorHandler_Machine] FOREIGN KEY ([MachineId]) REFERENCES [Configuration].[Machine] ([Id])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_MachineId_ErrorHandlerId]
    ON [Configuration].[MachineErrorHandler]([MachineId] ASC, [ErrorHandlerId] ASC);

