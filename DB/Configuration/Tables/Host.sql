CREATE TABLE [Configuration].[Host] (
    [Id]                   BIGINT        IDENTITY (1, 1) NOT NULL,
    [ApplicationVersionId] BIGINT        NOT NULL,
    [Name]                 VARCHAR (100) NOT NULL,
    [IPAddress]            VARCHAR (15)  NULL,
    [Description]          VARCHAR (MAX) NULL,
    CONSTRAINT [PK_Host] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Host_ApplicationVersion] FOREIGN KEY ([ApplicationVersionId]) REFERENCES [Configuration].[ApplicationVersion] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_HostName_ApplicationVersion]
    ON [Configuration].[Host]([Name] ASC, [ApplicationVersionId] ASC);

