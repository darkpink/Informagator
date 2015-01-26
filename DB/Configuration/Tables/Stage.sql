CREATE TABLE [Configuration].[Stage] (
    [Id]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [WorkerId]   BIGINT         NOT NULL,
    [Name]       VARCHAR (100)  NOT NULL,
    [Sequence]   INT            NOT NULL,
    [AssemblyId] BIGINT         NOT NULL,
    [Type]       VARCHAR (1000) NOT NULL,
    CONSTRAINT [PK_Stage] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Stage_Assembly] FOREIGN KEY ([AssemblyId]) REFERENCES [Configuration].[Assembly] ([Id]),
    CONSTRAINT [FK_Stage_Worker] FOREIGN KEY ([WorkerId]) REFERENCES [Configuration].[Worker] ([Id]) ON DELETE CASCADE
);










GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_ThreadId_Sequence]
    ON [Configuration].[Stage]([WorkerId] ASC, [Sequence] ASC);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_ThreadId_Name]
    ON [Configuration].[Stage]([Name] ASC, [WorkerId] ASC);



