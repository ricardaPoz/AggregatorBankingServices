﻿ALTER DATABASE [KnowledgeBase]
SET SINGLE_USER WITH ROLLBACK IMMEDIATE;

ALTER DATABASE [KnowledgeBase]
COLLATE Cyrillic_General_100_CS_AS;

ALTER DATABASE [KnowledgeBase]
SET MULTI_USER WITH ROLLBACK IMMEDIATE;

SELECT name, collation_name
FROM sys.databases
WHERE name = 'KnowledgeBase';