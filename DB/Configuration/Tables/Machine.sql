CREATE TABLE [Configuration].[Machine] (
    [Id]                    BIGINT        IDENTITY (1, 1) NOT NULL,
    [SystemConfigurationId] BIGINT        NOT NULL,
    [Name]                  VARCHAR (100) NOT NULL,
    [IPAddress]             VARCHAR (15)  NULL,
    [Description]           VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Host] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Host_ApplicationVersion] FOREIGN KEY ([SystemConfigurationId]) REFERENCES [Configuration].[SystemConfiguration] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_HostName_ApplicationVersion]
    ON [Configuration].[Machine]([Name] ASC, [SystemConfigurationId] ASC);

