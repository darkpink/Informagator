CREATE TABLE [Configuration].[AssemblySystemConfiguration] (
    [AssemblyName]          VARCHAR (200) NOT NULL,
    [AssemblyDotNetVersion] VARCHAR (100) NOT NULL,
    [SystemConfigurationId] BIGINT        NOT NULL,
    [AssemblyVersionId]     BIGINT        NOT NULL,
    CONSTRAINT [PK_AssemblyApplicationVersion] PRIMARY KEY CLUSTERED ([SystemConfigurationId] ASC, [AssemblyName] ASC, [AssemblyDotNetVersion] ASC),
    CONSTRAINT [FK_AssemblyApplicationVersion_ApplicationVersion] FOREIGN KEY ([SystemConfigurationId]) REFERENCES [Configuration].[SystemConfiguration] ([Id]),
    CONSTRAINT [FK_AssemblyApplicationVersion_Assembly] FOREIGN KEY ([AssemblyName], [AssemblyDotNetVersion]) REFERENCES [Configuration].[Assembly] ([Name], [DotNetVersion]),
    CONSTRAINT [FK_AssemblyApplicationVersion_AssemblyVersion] FOREIGN KEY ([AssemblyVersionId]) REFERENCES [Configuration].[AssemblyVersion] ([Id])
);

