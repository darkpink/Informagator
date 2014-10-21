CREATE procedure [Message].[Dequeue] @QueueName varchar(100) AS
BEGIN

WITH MatchingMessage as 
(
SELECT TOP 1 Id
FROM Message.Message
WHERE QueueName = @QueueName and DequeueDttm is null
ORDER BY AddDttm
)
UPDATE m
SET DequeueDttm = CURRENT_TIMESTAMP
OUTPUT Inserted.Id
FROM Message.Message m inner join MatchingMessage mm on m.Id = mm.Id

END