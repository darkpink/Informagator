CREATE TABLE [Configuration].[ErrorHandlerParameter] (
    [Id]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [StageId] BIGINT        NOT NULL,
    [Name]    VARCHAR (200) NOT NULL,
    [Value]   VARCHAR (MAX) NULL,
    CONSTRAINT [PK_ErrorHandlerParameter_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ErrorHandlerParameter_Stage] FOREIGN KEY ([StageId]) REFERENCES [Configuration].[Stage] ([Id]) ON DELETE CASCADE
);



