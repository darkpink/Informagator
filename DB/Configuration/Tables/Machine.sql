CREATE TABLE [Configuration].[Machine] (
    [Id]                          BIGINT        IDENTITY (1, 1) NOT NULL,
    [SystemConfigurationId]       BIGINT        NOT NULL,
    [Name]                        VARCHAR (100) NOT NULL,
    [IPAddress]                   VARCHAR (15)  NULL,
    [Description]                 VARCHAR (MAX) NULL,
    [AdminServicePort]            INT           NOT NULL,
    [InfoServicePort]             INT           NOT NULL,
    [SuppressParentErrorHandlers] BIT           NOT NULL,
    [AdminServiceGroup]           VARCHAR (100) NULL,
    [InfoServiceGroup]            VARCHAR (100) NULL,
    CONSTRAINT [PK_Machine] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Machine_SystemConfigurationId] FOREIGN KEY ([SystemConfigurationId]) REFERENCES [Configuration].[SystemConfiguration] ([Id]) ON DELETE CASCADE
);










GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_Name_SystemConfiguration]
    ON [Configuration].[Machine]([Name] ASC, [SystemConfigurationId] ASC);

