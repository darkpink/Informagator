CREATE TABLE [Configuration].[SystemConfiguration] (
    [Id]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [CreateDttm]    DATETIME      CONSTRAINT [DF_ApplicationVersion_CreateDttm] DEFAULT (getdate()) NOT NULL,
    [Description]   VARCHAR (100) NULL,
    [IsActive]      BIT           NOT NULL,
    [EffectiveDttm] DATETIME      NULL,
    CONSTRAINT [PK_Version] PRIMARY KEY CLUSTERED ([Id] ASC)
);

