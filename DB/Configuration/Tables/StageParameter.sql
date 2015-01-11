CREATE TABLE [Configuration].[StageParameter] (
    [Id]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [StageId] BIGINT        NOT NULL,
    [Name]    VARCHAR (200) NOT NULL,
    [Value]   VARCHAR (MAX) NULL,
    CONSTRAINT [PK_StageParameter_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StageParameter_Stage] FOREIGN KEY ([StageId]) REFERENCES [Configuration].[Stage] ([Id]) ON DELETE CASCADE
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_StageId_Name]
    ON [Configuration].[StageParameter]([StageId] ASC, [Name] ASC);

