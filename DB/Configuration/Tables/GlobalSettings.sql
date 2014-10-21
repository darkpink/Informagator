CREATE TABLE [Configuration].[GlobalSettings] (
    [Id]                   BIGINT        IDENTITY (1, 1) NOT NULL,
    [ApplicationVersionId] BIGINT        NOT NULL,
    [Name]                 VARCHAR (100) NOT NULL,
    [Value]                VARCHAR (MAX) NULL,
    CONSTRAINT [PK_GlobalSettings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GlobalSettings_ApplicationVersion] FOREIGN KEY ([ApplicationVersionId]) REFERENCES [Configuration].[ApplicationVersion] ([Id])
);

