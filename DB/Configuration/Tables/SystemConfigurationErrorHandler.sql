CREATE TABLE [Configuration].[SystemConfigurationErrorHandler] (
    [Id]                    BIGINT IDENTITY (1, 1) NOT NULL,
    [SystemConfigurationId] BIGINT NOT NULL,
    [ErrorHandlerId]        BIGINT NOT NULL,
    CONSTRAINT [PK_SystemConfigurationErrorHandler] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SystemConfigurationErrorHandler_ErrorHandler] FOREIGN KEY ([ErrorHandlerId]) REFERENCES [Configuration].[ErrorHandler] ([Id]),
    CONSTRAINT [FK_SystemConfigurationErrorHandler_SystemConfiguration] FOREIGN KEY ([SystemConfigurationId]) REFERENCES [Configuration].[SystemConfiguration] ([Id]) ON DELETE CASCADE
);

