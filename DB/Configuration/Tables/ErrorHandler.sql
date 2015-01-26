CREATE TABLE [Configuration].[ErrorHandler] (
    [Id]                    BIGINT         IDENTITY (1, 1) NOT NULL,
    [SystemConfigurationId] BIGINT         NOT NULL,
    [Name]                  VARCHAR (100)  NOT NULL,
    [AssemblyId]            BIGINT         NOT NULL,
    [Type]                  VARCHAR (1000) NOT NULL,
    CONSTRAINT [PK_ErrorHandler] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ErrorHandler_Assembly] FOREIGN KEY ([AssemblyId]) REFERENCES [Configuration].[Assembly] ([Id]),
    CONSTRAINT [FK_ErrorHandler_SystemConfiguration] FOREIGN KEY ([SystemConfigurationId]) REFERENCES [Configuration].[SystemConfiguration] ([Id]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_Name_SystemConfigurationId]
    ON [Configuration].[ErrorHandler]([SystemConfigurationId] ASC, [Name] ASC);

