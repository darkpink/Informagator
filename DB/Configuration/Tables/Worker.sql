CREATE TABLE [Configuration].[Worker] (
    [Id]                      BIGINT         IDENTITY (1, 1) NOT NULL,
    [MachineId]               BIGINT         NOT NULL,
    [Name]                    VARCHAR (100)  NOT NULL,
    [WorkerAssemblyVersionId] BIGINT         NOT NULL,
    [WorkerType]              VARCHAR (1000) NOT NULL,
    [AutoStart]               BIT            NOT NULL,
    CONSTRAINT [PK_Thread] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Thread_Host] FOREIGN KEY ([MachineId]) REFERENCES [Configuration].[Machine] ([Id]),
    CONSTRAINT [FK_Worker_AssemblyVersion] FOREIGN KEY ([WorkerAssemblyVersionId]) REFERENCES [Configuration].[AssemblyVersion] ([Id])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_HostId_Name]
    ON [Configuration].[Worker]([MachineId] ASC, [Name] ASC);

