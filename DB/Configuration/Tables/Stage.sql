CREATE TABLE [Configuration].[Stage] (
    [Id]                            BIGINT         IDENTITY (1, 1) NOT NULL,
    [WorkerId]                      BIGINT         NOT NULL,
    [Name]                          VARCHAR (100)  NOT NULL,
    [Sequence]                      INT            NOT NULL,
    [StageAssemblyVersionId]        BIGINT         NOT NULL,
    [StageType]                     VARCHAR (1000) NOT NULL,
    [ErrorHandlerAssemblyVersionId] BIGINT         NOT NULL,
    [ErrorHandlerType]              VARCHAR (1000) NOT NULL,
    CONSTRAINT [PK_Stage] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Stage_AssemblyVersion_ErrorHandler] FOREIGN KEY ([ErrorHandlerAssemblyVersionId]) REFERENCES [Configuration].[AssemblyVersion] ([Id]),
    CONSTRAINT [FK_Stage_AssemblyVersion_Stage] FOREIGN KEY ([StageAssemblyVersionId]) REFERENCES [Configuration].[AssemblyVersion] ([Id]),
    CONSTRAINT [FK_Stage_Thread] FOREIGN KEY ([WorkerId]) REFERENCES [Configuration].[Worker] ([Id])
);






GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_ThreadId_Sequence]
    ON [Configuration].[Stage]([WorkerId] ASC, [Sequence] ASC);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_ThreadId_Name]
    ON [Configuration].[Stage]([Name] ASC, [WorkerId] ASC);



