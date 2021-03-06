use informagator;

DECLARE @srcRoot nvarchar(1000);

merge configuration.SystemConfiguration as t
using (select 'DevelopmentAlt' Description, CAST(1 as bit) IsActive, current_timestamp EffectiveDttm,
9001 as DefaultAdminServicePort, 9002 as DefaultInfoServicePort) as s
ON t.Description = s.Description
when matched then
UPDATE SET IsActive = 1, t.DefaultAdminServicePort = s.DefaultAdminServicePort, t.DefaultInfoServicePort = s.DefaultInfoServicePort
when not matched by target then
INSERT (Description, IsActive, EffectiveDttm, DefaultAdminServicePort, DefaultInfoServicePort) VALUES ('DevelopmentAlt', 1, current_timestamp, 9001, 9002);

merge configuration.AssemblyVersion as t
using (select 'Informagator.CommonComponents.dll' AssemblyName, '1.0.0.0' AssemblyDotNetVersion, current_timestamp LoadDttm, Exe.BulkColumn Executable, Pdb.BulkColumn DebuggingSymbols
from OPENROWSET(BULK N'E:\Github\Informagator\CommonComponents\bin\Debug\Informagator.CommonComponents.dll', SINGLE_BLOB) as Exe
full outer join OPENROWSET(BULK N'E:\Github\Informagator\CommonComponents\bin\Debug\Informagator.CommonComponents.pdb', SINGLE_BLOB) as Pdb on 1 = 1 
) as s
ON t.AssemblyName = s.AssemblyName and t.AssemblyDotNetVersion = s.AssemblyDotNetVersion
when matched then
UPDATE SET t.Executable = s.Executable
when not matched by target then
INSERT (AssemblyName, AssemblyDotNetVersion, LoadDttm, Executable, DebuggingSymbols) VALUES ('Informagator.CommonComponents.dll', '1.0.0.0', current_timestamp, s.Executable, s.DebuggingSymbols);

DECLARE @configId BIGINT, @informagatorId BIGINT, @commonId BIGINT;
SELECT @configId = Id FROM Configuration.SystemConfiguration where Description = 'DevelopmentAlt';
SELECT @commonId = Id from Configuration.AssemblyVersion where AssemblyName = 'Informagator.CommonComponents.dll' and AssemblyDotNetVersion = '1.0.0.0';

merge configuration.AssemblySystemConfiguration as t
using (select 1 jnk) as s
ON t.AssemblyName = 'Informagator.CommonComponents.dll' and t.AssemblyDotNetVersion = '1.0.0.0' and t.SystemConfigurationId = @configId
when matched then
UPDATE SET t.AssemblyVersionId = @commonId
when not matched by target then
INSERT (AssemblyName, AssemblyDotNetVersion, SystemConfigurationId, AssemblyVersionId) VALUES ('Informagator.CommonComponents.dll', '1.0.0.0', @configId, @commonId);

merge configuration.machine as t
using (select 1 jnk) as s
ON t.Name = 'Development' and t.SystemConfigurationId = @configId
when matched then
UPDATE SET t.IPAddress = '127.0.0.1', Description = 'Local dev machine', AdminServicePort = 9001, InfoServicePort = 9002
when not matched by target then
INSERT (SystemConfigurationId, Name, IPAddress, Description, AdminServicePort, InfoServicePort) VALUES (@configId, 'Development', '127.0.0.1', 'Local dev machine Alt', 9001, 9002);

DECLARE @machineId BIGINT;
select @machineId = Id from configuration.machine where name = 'Development' and SystemConfigurationId = @configId;

merge configuration.Worker as t
using (select @machineId MachineId, 'FileMover' Name, @commonId WorkerAssemblyVersionId, 'Informagator.CommonComponents.Workers.PollingStageWorker' WorkerType, Cast(1 as bit) AutoStart) as s
on t.MachineId = s.machineId and t.Name = s.Name
WHEN Matched then
update set t.WorkerAssemblyVersionId = s.WorkerAssemblyVersionId, t.WorkerType = s.WorkerType, t.AutoStart = s.AutoStart
WHEN NOT MATCHED THEN
insert (machineId, name, workerassemblyversionid, workerType, AutoStart) VALUES (s.machineId, s.name, s.workerassemblyversionid, s.workertype, s.AutoStart);

DECLARE @workerId BIGINT;
select @workerId = Id from configuration.worker where name = 'FileMover' and machineId = @machineId;

merge configuration.Stage as t
using (select @workerId WorkerId, 'Input' Name, 10 sequence, @commonId StageAssemblyVersionId, 'Informagator.CommonComponents.SupplierStages.OldestFileFromFolderSupplier' StageType, @commonId ErrorHandlerAssemblyVersionId, 'Informagator.CommonComponents.ErrorHandlers.IgnoreErrorHandler' ErrorHandlerType) as s
on t.workerId = s.workerId and t.Name = s.Name
WHEN Matched then
update set t.sequence = s.sequence, t.stageassemblyversionid = s.stageassemblyversionid, t.stagetype = s.stagetype, t.ErrorHandlerAssemblyVersionId = s.ErrorHandlerAssemblyVersionId, t.ErrorHandlerType = s.ErrorHandlerType
WHEN NOT MATCHED THEN
insert (workerId, name, Sequence, Stageassemblyversionid, stageType, errorhandlerassemblyversionid, errorhandlertype) VALUES (s.workerId, s.name, s.sequence, s.stageassemblyversionid, s.stagetype, s.errorhandlerassemblyversionid, s.errorhandlertype);

merge configuration.Stage as t
using (select @workerId WorkerId, 'Output' Name, 20 sequence, @commonId StageAssemblyVersionId, 'Informagator.CommonComponents.ConsumerStages.StaticOutputFolderConsumer' StageType, @commonId ErrorHandlerAssemblyVersionId, 'Informagator.CommonComponents.ErrorHandlers.IgnoreErrorHandler' ErrorHandlerType) as s
on t.workerId = s.workerId and t.Name = s.Name
WHEN Matched then
update set t.sequence = s.sequence, t.stageassemblyversionid = s.stageassemblyversionid, t.stagetype = s.stagetype, t.ErrorHandlerAssemblyVersionId = s.ErrorHandlerAssemblyVersionId, t.ErrorHandlerType = s.ErrorHandlerType
WHEN NOT MATCHED THEN
insert (workerId, name, Sequence, Stageassemblyversionid, stageType, errorhandlerassemblyversionid, errorhandlertype) VALUES (s.workerId, s.name, s.sequence, s.stageassemblyversionid, s.stagetype, s.errorhandlerassemblyversionid, s.errorhandlertype);

DECLARE @inputId BIGINT;
DECLARE @outputId BIGINT;
select @inputId = Id from configuration.stage where name = 'Input' and workerId = @workerId;
select @outputId = Id from configuration.stage where name = 'Output' and workerId = @workerId;

merge configuration.StageParameter as t
using (select @inputId StageId, 'FolderPath' Name, 'C:\Informagator\Source2' Value) as s
on t.stageId = s.stageId and t.Name = s.Name
WHEN Matched then
update set t.value = s.value
WHEN NOT MATCHED THEN
insert (stageId, name, value) VALUES (s.stageId, s.name, s.value);

merge configuration.StageParameter as t
using (select @outputId StageId, 'FolderPath' Name, 'C:\Informagator\Destination2' Value) as s
on t.stageId = s.stageId and t.Name = s.Name
WHEN Matched then
update set t.value = s.value
WHEN NOT MATCHED THEN
insert (stageId, name, value) VALUES (s.stageId, s.name, s.value);

select * From configuration.SystemConfiguration