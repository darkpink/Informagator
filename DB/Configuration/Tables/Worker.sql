CREATE TABLE [Configuration].[Worker] (
    [Id]                          BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]                        VARCHAR (100)  NOT NULL,
    [MachineId]                   BIGINT         NOT NULL,
    [AssemblyId]                  BIGINT         NOT NULL,
    [Type]                        VARCHAR (1000) NOT NULL,
    [AutoStart]                   BIT            CONSTRAINT [DF_Worker_AutoStart] DEFAULT ((1)) NOT NULL,
    [SuppressParentErrorHandlers] BIT            NOT NULL,
    CONSTRAINT [PK_Worker] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Worker_Assembly] FOREIGN KEY ([AssemblyId]) REFERENCES [Configuration].[Assembly] ([Id]),
    CONSTRAINT [FK_Worker_Machine] FOREIGN KEY ([MachineId]) REFERENCES [Configuration].[Machine] ([Id]) ON DELETE CASCADE
);










GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_MachineId_Name]
    ON [Configuration].[Worker]([MachineId] ASC, [Name] ASC);

