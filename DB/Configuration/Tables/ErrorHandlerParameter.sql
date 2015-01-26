CREATE TABLE [Configuration].[ErrorHandlerParameter] (
    [Id]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [ErrorHandlerId] BIGINT        NOT NULL,
    [Name]           VARCHAR (200) NOT NULL,
    [Value]          VARCHAR (MAX) NULL,
    CONSTRAINT [PK_ErrorHandlerParameter_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ErrorHandlerParameter_ErrorHandler] FOREIGN KEY ([ErrorHandlerId]) REFERENCES [Configuration].[ErrorHandler] ([Id]) ON DELETE CASCADE
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_ErrorHandlerId_Name]
    ON [Configuration].[ErrorHandlerParameter]([ErrorHandlerId] ASC, [Name] ASC);

