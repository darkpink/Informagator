CREATE TABLE [Configuration].[Assembly] (
    [Id]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]                  VARCHAR (200)   NOT NULL,
    [Version]               VARCHAR (100)   NOT NULL,
    [SystemConfigurationId] BIGINT          NOT NULL,
    [LoadDttm]              DATETIME        NOT NULL,
    [Executable]            VARBINARY (MAX) NOT NULL,
    [DebuggingSymbols]      VARBINARY (MAX) NULL,
    CONSTRAINT [PK_Assembly] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Assembly_SystemConfiguration] FOREIGN KEY ([SystemConfigurationId]) REFERENCES [Configuration].[SystemConfiguration] ([Id]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_Name_Version_SystemConfigurationId]
    ON [Configuration].[Assembly]([Name] ASC, [Version] ASC, [SystemConfigurationId] ASC);

