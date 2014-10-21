CREATE TABLE [Configuration].[Assembly] (
    [Name]          VARCHAR (200)   NOT NULL,
    [DotNetVersion] VARCHAR (100)   NOT NULL,
    [Description]   VARBINARY (MAX) NULL,
    CONSTRAINT [PK_Assembly_1] PRIMARY KEY CLUSTERED ([Name] ASC, [DotNetVersion] ASC)
);

