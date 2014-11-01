CREATE TABLE [Configuration].[GlobalSettings] (
    [Id]                    BIGINT        IDENTITY (1, 1) NOT NULL,
    [SystemConfigurationId] BIGINT        NOT NULL,
    [Name]                  VARCHAR (100) NOT NULL,
    [Value]                 VARCHAR (MAX) NULL,
    CONSTRAINT [PK_GlobalSettings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GlobalSettings_ApplicationVersion] FOREIGN KEY ([SystemConfigurationId]) REFERENCES [Configuration].[SystemConfiguration] ([Id])
);



