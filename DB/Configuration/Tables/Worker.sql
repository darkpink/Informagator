CREATE TABLE [Configuration].[Worker] (
    [Id]                          BIGINT         IDENTITY (1, 1) NOT NULL,
    [MachineId]                   BIGINT         NOT NULL,
    [Name]                        VARCHAR (100)  NOT NULL,
    [WorkerAssemblyName]          VARCHAR (200)  NOT NULL,
    [WorkerAssemblyDotNetVersion] VARCHAR (100)  NOT NULL,
    [WorkerType]                  VARCHAR (1000) NOT NULL,
    [AutoStart]                   BIT            NOT NULL,
    CONSTRAINT [PK_Thread] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Thread_Assembly] FOREIGN KEY ([WorkerAssemblyName], [WorkerAssemblyDotNetVersion]) REFERENCES [Configuration].[Assembly] ([Name], [DotNetVersion]),
    CONSTRAINT [FK_Thread_Host] FOREIGN KEY ([MachineId]) REFERENCES [Configuration].[Machine] ([Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_HostId_Name]
    ON [Configuration].[Worker]([MachineId] ASC, [Name] ASC);

