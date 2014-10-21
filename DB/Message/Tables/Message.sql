CREATE TABLE [Message].[Message] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [QueueName]   VARCHAR (100) NOT NULL,
    [AddDttm]     DATETIME      CONSTRAINT [DF__Message__AddDate__117F9D94] DEFAULT (getdate()) NOT NULL,
    [DequeueDttm] DATETIME      NULL,
    [Body]        VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED ([Id] ASC)
);

