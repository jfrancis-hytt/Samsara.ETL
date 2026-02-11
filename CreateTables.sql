-- Samsara ETL Database Tables
-- SQL Server
-- Run parent tables first, then child tables

-----------------------------------------------
-- Sensors
-----------------------------------------------
CREATE TABLE [dbo].[Sensors]
(
    [SensorId]      BIGINT          NOT NULL,
    [Name]          NVARCHAR(255)   NOT NULL,
    [MacAddress]    VARCHAR(17)     NOT NULL,
    [CreatedAt]     DATETIME2       NOT NULL    DEFAULT GETUTCDATE(),
    [UpdatedAt]     DATETIME2       NOT NULL    DEFAULT GETUTCDATE(),

    CONSTRAINT [PK_Sensors] PRIMARY KEY ([SensorId])
);

-----------------------------------------------
-- Gateways
-----------------------------------------------
CREATE TABLE [dbo].[Gateways]
(
    [Serial]                    NVARCHAR(100)   NOT NULL,
    [Model]                     NVARCHAR(100)   NOT NULL,
    [AssetId]                   NVARCHAR(100)   NOT NULL,
    [SamsaraSerial]             NVARCHAR(100)   NOT NULL,
    [SamsaraVin]                NVARCHAR(50)    NULL,
    [HealthStatus]              NVARCHAR(50)    NOT NULL,
    [LastConnected]             DATETIME2       NULL,
    [CellularDataUsageBytes]    BIGINT          NOT NULL    DEFAULT 0,
    [HotspotUsageBytes]         BIGINT          NOT NULL    DEFAULT 0,
    [CreatedAt]                 DATETIME2       NOT NULL    DEFAULT GETUTCDATE(),
    [UpdatedAt]                 DATETIME2       NOT NULL    DEFAULT GETUTCDATE(),

    CONSTRAINT [PK_Gateways] PRIMARY KEY ([Serial])
);

-----------------------------------------------
-- AccessoryDevices (child of Gateways)
-----------------------------------------------
CREATE TABLE [dbo].[AccessoryDevices]
(
    [Serial]            NVARCHAR(100)   NOT NULL,
    [Model]             NVARCHAR(100)   NOT NULL,
    [GatewaySerial]     NVARCHAR(100)   NOT NULL,
    [CreatedAt]         DATETIME2       NOT NULL    DEFAULT GETUTCDATE(),
    [UpdatedAt]         DATETIME2       NOT NULL    DEFAULT GETUTCDATE(),

    CONSTRAINT [PK_AccessoryDevices] PRIMARY KEY ([Serial]),
    CONSTRAINT [FK_AccessoryDevices_Gateways] FOREIGN KEY ([GatewaySerial])
        REFERENCES [dbo].[Gateways] ([Serial])
);

-----------------------------------------------
-- Trailers
-----------------------------------------------
CREATE TABLE [dbo].[Trailers]
(
    [Id]                NVARCHAR(100)   NOT NULL,
    [Name]              NVARCHAR(255)   NOT NULL,
    [GatewaySerial]     NVARCHAR(100)   NULL,
    [GatewayModel]      NVARCHAR(100)   NULL,
    [SamsaraSerial]     NVARCHAR(100)   NULL,
    [SamsaraVin]        NVARCHAR(50)    NULL,
    [LicensePlate]      NVARCHAR(50)    NULL,
    [Notes]             NVARCHAR(MAX)   NULL,
    [EnabledForMobile]  BIT             NOT NULL    DEFAULT 0,
    [CreatedAt]         DATETIME2       NOT NULL    DEFAULT GETUTCDATE(),
    [UpdatedAt]         DATETIME2       NOT NULL    DEFAULT GETUTCDATE(),

    CONSTRAINT [PK_Trailers] PRIMARY KEY ([Id])
);

-----------------------------------------------
-- TrailerTags (child of Trailers)
-----------------------------------------------
CREATE TABLE [dbo].[TrailerTags]
(
    [Id]            NVARCHAR(100)   NOT NULL,
    [Name]          NVARCHAR(255)   NOT NULL,
    [ParentTagId]   NVARCHAR(100)   NOT NULL,
    [TrailerId]     NVARCHAR(100)   NOT NULL,
    [CreatedAt]     DATETIME2       NOT NULL    DEFAULT GETUTCDATE(),
    [UpdatedAt]     DATETIME2       NOT NULL    DEFAULT GETUTCDATE(),

    CONSTRAINT [PK_TrailerTags] PRIMARY KEY ([Id], [TrailerId]),
    CONSTRAINT [FK_TrailerTags_Trailers] FOREIGN KEY ([TrailerId])
        REFERENCES [dbo].[Trailers] ([Id])
);

-----------------------------------------------
-- SensorTemperatureReadings (child of Sensors)
-----------------------------------------------
CREATE TABLE [dbo].[SensorTemperatureReadings]
(
    [Id]                        BIGINT          NOT NULL    IDENTITY(1, 1),
    [SensorId]                  BIGINT          NOT NULL,
    [Name]                      NVARCHAR(255)   NOT NULL,
    [AmbientTemperature]        INT             NULL,
    [AmbientTemperatureTime]    DATETIME2       NULL,
    [ProbeTemperature]          INT             NULL,
    [ProbeTemperatureTime]      DATETIME2       NULL,
    [TrailerId]                 BIGINT          NULL,
    [CreatedAt]                 DATETIME2       NOT NULL    DEFAULT GETUTCDATE(),

    CONSTRAINT [PK_SensorTemperatureReadings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SensorTemperatureReadings_Sensors] FOREIGN KEY ([SensorId])
        REFERENCES [dbo].[Sensors] ([SensorId])
);

-----------------------------------------------
-- SensorHistoryReadings (child of Sensors)
-----------------------------------------------
CREATE TABLE [dbo].[SensorHistoryReadings]
(
    [Id]                    BIGINT          NOT NULL    IDENTITY(1, 1),
    [SensorId]              BIGINT          NOT NULL,
    [TimeMs]                BIGINT          NOT NULL,
    [ProbeTemperature]      INT             NULL,
    [AmbientTemperature]    INT             NULL,
    [CreatedAt]             DATETIME2       NOT NULL    DEFAULT GETUTCDATE(),

    CONSTRAINT [PK_SensorHistoryReadings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_SensorHistoryReadings_Sensors] FOREIGN KEY ([SensorId])
        REFERENCES [dbo].[Sensors] ([SensorId])
);

-----------------------------------------------
-- Indexes
-----------------------------------------------
CREATE INDEX [IX_AccessoryDevices_GatewaySerial]
    ON [dbo].[AccessoryDevices] ([GatewaySerial]);

CREATE INDEX [IX_TrailerTags_TrailerId]
    ON [dbo].[TrailerTags] ([TrailerId]);

CREATE INDEX [IX_SensorTemperatureReadings_SensorId]
    ON [dbo].[SensorTemperatureReadings] ([SensorId]);

CREATE INDEX [IX_SensorTemperatureReadings_CreatedAt]
    ON [dbo].[SensorTemperatureReadings] ([CreatedAt]);

CREATE INDEX [IX_SensorHistoryReadings_SensorId]
    ON [dbo].[SensorHistoryReadings] ([SensorId]);

CREATE INDEX [IX_SensorHistoryReadings_TimeMs]
    ON [dbo].[SensorHistoryReadings] ([TimeMs]);
