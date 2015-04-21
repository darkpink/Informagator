CREATE TABLE [Configuration].[SystemConfiguration] (
    [Id]                       BIGINT        IDENTITY (1, 1) NOT NULL,
    [CreateDttm]               DATETIME      CONSTRAINT [DF_ApplicationVersion_CreateDttm] DEFAULT (getdate()) NOT NULL,
    [Name]                     VARCHAR (100) NULL,
    [IsActive]                 BIT           NOT NULL,
    [EffectiveDttm]            DATETIME      NULL,
    [DefaultAdminServicePort]  INT           NULL,
    [DefaultInfoServicePort]   INT           NULL,
    [DefaultAdminServiceGroup] VARCHAR (100) NULL,
    [DefaultInfoServiceGroup]  VARCHAR (100) NULL,
    CONSTRAINT [PK_SystemConfiguration] PRIMARY KEY CLUSTERED ([Id] ASC)
);










GO


CREATE trigger [Configuration].[SystemConfigurationOnlyOneActive] ON [Configuration].[SystemConfiguration] FOR insert, update as

DECLARE @newActiveCount int;

select @newActiveCount = count(*)
from inserted 
where IsActive = 1;

if @newActiveCount > 1
BEGIN
      RAISERROR ('Only one configuration may be made active at a time', 16, 1)
      ROLLBACK TRANSACTION
      RETURN
END

if @newActiveCount = 1
BEGIN
      Update Configuration.SystemConfiguration
      SET IsActive = 0
      where Id NOT IN (Select Id from inserted)
END
GO
CREATE UNIQUE NONCLUSTERED INDEX [UC_Name]
    ON [Configuration].[SystemConfiguration]([Name] ASC);

