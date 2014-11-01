CREATE TABLE [Configuration].[AssemblyVersion] (
    [Id]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [AssemblyName]          VARCHAR (200)   NOT NULL,
    [AssemblyDotNetVersion] VARCHAR (100)   NOT NULL,
    [LoadDttm]              DATETIME        CONSTRAINT [DF_AssemblyVersion_LoadDttm] DEFAULT (getdate()) NOT NULL,
    [Executable]            VARBINARY (MAX) NOT NULL,
    [DebuggingSymbols]      VARBINARY (MAX) NULL,
    CONSTRAINT [PK_AssemblyVersion_1] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AssemblyVersion_Assembly] FOREIGN KEY ([AssemblyName], [AssemblyDotNetVersion]) REFERENCES [Configuration].[Assembly] ([Name], [DotNetVersion])
);



