CREATE TABLE [Configuration].[Stage] (
    [Id]                                BIGINT         IDENTITY (1, 1) NOT NULL,
    [ThreadId]                          BIGINT         NOT NULL,
    [Name]                              VARCHAR (100)  NOT NULL,
    [Sequence]                          INT            NOT NULL,
    [StageAssemblyName]                 VARCHAR (200)  NOT NULL,
    [StageAssemblyDotNetVersion]        VARCHAR (100)  NOT NULL,
    [StageType]                         VARCHAR (1000) NOT NULL,
    [ErrorHandlerAssemblyName]          VARCHAR (200)  NOT NULL,
    [ErrorHandlerAssemblyDotNetVersion] VARCHAR (100)  NOT NULL,
    [ErrorHandlerType]                  VARCHAR (1000) NOT NULL,
    CONSTRAINT [PK_Stage] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Stage_Assembly] FOREIGN KEY ([StageAssemblyName], [StageAssemblyDotNetVersion]) REFERENCES [Configuration].[Assembly] ([Name], [DotNetVersion]),
    CONSTRAINT [FK_Stage_Assembly1] FOREIGN KEY ([ErrorHandlerAssemblyName], [ErrorHandlerAssemblyDotNetVersion]) REFERENCES [Configuration].[Assembly] ([Name], [DotNetVersion]),
    CONSTRAINT [FK_Stage_Thread] FOREIGN KEY ([ThreadId]) REFERENCES [Configuration].[Worker] ([Id])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_ThreadId_Sequence]
    ON [Configuration].[Stage]([ThreadId] ASC, [Sequence] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_ThreadId_Name]
    ON [Configuration].[Stage]([Name] ASC, [ThreadId] ASC);

