CREATE DATABASE [Expo]  ON (NAME = N'Expo', FILENAME = N'c:\Archivos de programa\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\Expo.mdf' , SIZE = 35, FILEGROWTH = 1) LOG ON (NAME = N'Expo_log', FILENAME = N'c:\Archivos de programa\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\Expo_log.ldf' , SIZE = 28, MAXSIZE = 2097152, FILEGROWTH = 10%)
GO

exec sp_dboption N'Expo', N'autoclose', N'false'
GO

exec sp_dboption N'Expo', N'bulkcopy', N'false'
GO

exec sp_dboption N'Expo', N'trunc. log', N'true'
GO

exec sp_dboption N'Expo', N'torn page detection', N'false'
GO

exec sp_dboption N'Expo', N'read only', N'false'
GO

exec sp_dboption N'Expo', N'dbo use', N'false'
GO

exec sp_dboption N'Expo', N'single', N'false'
GO

exec sp_dboption N'Expo', N'autoshrink', N'false'
GO

exec sp_dboption N'Expo', N'ANSI null default', N'false'
GO

exec sp_dboption N'Expo', N'recursive triggers', N'false'
GO

exec sp_dboption N'Expo', N'ANSI nulls', N'false'
GO

exec sp_dboption N'Expo', N'concat null yields null', N'false'
GO

exec sp_dboption N'Expo', N'cursor close on commit', N'false'
GO

exec sp_dboption N'Expo', N'default to local cursor', N'false'
GO

exec sp_dboption N'Expo', N'quoted identifier', N'false'
GO

exec sp_dboption N'Expo', N'ANSI warnings', N'false'
GO

exec sp_dboption N'Expo', N'auto create statistics', N'true'
GO

exec sp_dboption N'Expo', N'auto update statistics', N'true'
GO

if( (@@microsoftversion / power(2, 24) = 8) and (@@microsoftversion & 0xffff >= 724) )
	exec sp_dboption N'Expo', N'db chaining', N'false'
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CIERRE_EFECTIVO]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[CIERRE_EFECTIVO] (
	[ID_CIERRE] [bigint] IDENTITY (1, 1) NOT NULL ,
	[FECHA] [datetime] NOT NULL ,
	[EFECTIVO] [decimal](18, 0) NOT NULL ,
	[EGRESOS] [decimal](18, 0) NOT NULL ,
	[ESTADO] [int] NOT NULL ,
	CONSTRAINT [PK_CIERRE_EFECTIVO] PRIMARY KEY  CLUSTERED 
	(
		[ID_CIERRE]
	)  ON [PRIMARY] 
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CIERRE_VENTAS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[CIERRE_VENTAS] (
	[ID_CIERRE] [bigint] IDENTITY (1, 1) NOT NULL ,
	[FECHA] [datetime] NULL ,
	[TOTAL_VENTAS] [float] NULL ,
	[CREDITOS] [float] NULL ,
	[EFECTIVO] [float] NULL ,
	[SODEXO] [float] NULL ,
	[BIG_PASS] [float] NULL ,
	[OTROS] [float] NULL ,
	[TURNO] [bigint] NULL ,
	[ISLA] [bigint] NULL ,
	[COD_EMPLEADO] [bigint] NULL ,
	[TARJETAS] [float] NULL ,
	[TARJETA_PLUS] [float] NULL ,
	[TICKET_TRONIK] [float] NULL CONSTRAINT [DF_CIERRE_VENTAS_TICKET_TRONIK] DEFAULT ((0)),
	CONSTRAINT [PK_CIERRES_VENTAS] PRIMARY KEY  CLUSTERED 
	(
		[ID_CIERRE]
	)  ON [PRIMARY] 
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CLIENTES]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[CLIENTES] (
	[ID_CLIENTE] [bigint] IDENTITY (1, 1) NOT NULL ,
	[TIPO_ID] [bigint] NOT NULL ,
	[IDENTIFICACION] [varchar] (15) NOT NULL ,
	[NOMBRE] [varchar] (50) NOT NULL ,
	[DIRECCION] [varchar] (100) NULL ,
	[TELEFONO] [varchar] (20) NULL ,
	[CONTACTO] [varchar] (50) NULL ,
	[EMAIL] [varchar] (100) NULL ,
	[CODIGO] [varchar] (15) NULL ,
	[CONSECUTIVO] [varchar] (2) NOT NULL CONSTRAINT [DF_CLIENTES_CONSECUTIVO] DEFAULT ((0)),
	CONSTRAINT [PK_CLIENTES] PRIMARY KEY  CLUSTERED 
	(
		[ID_CLIENTE]
	)  ON [PRIMARY] 
) ON [PRIMARY]
 CREATE  UNIQUE  INDEX [NIT_INDEX] ON [dbo].[CLIENTES]([IDENTIFICACION], [CODIGO], [CONSECUTIVO]) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[COMPRAS_COMBUSTIBLE]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[COMPRAS_COMBUSTIBLE] (
	[ID_COMPRA] [bigint] IDENTITY (1, 1) NOT NULL ,
	[FACTURA] [bigint] NOT NULL ,
	[ID_PRODUCTO] [int] NOT NULL ,
	[CANTIDAD_GL] [float] NOT NULL ,
	[VALOR_UNITARIO] [float] NOT NULL ,
	[VALOR] [float] NOT NULL ,
	[FLETE] [float] NOT NULL ,
	[SOLDICOM] [float] NOT NULL ,
	[IMPUESTO] [float] NOT NULL ,
	[FECHA] [datetime] NOT NULL ,
	CONSTRAINT [PK_COMPRAS_COMBUSTIBLE] PRIMARY KEY  CLUSTERED 
	(
		[ID_COMPRA]
	)  ON [PRIMARY] 
) ON [PRIMARY]
 CREATE  INDEX [IX_FACTURA] ON [dbo].[COMPRAS_COMBUSTIBLE]([FACTURA]) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CONTROL_COMBUSTIBLE]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[CONTROL_COMBUSTIBLE] (
	[ID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[FECHA] [datetime] NULL ,
	[GALONES_COMPRA] [int] NULL ,
	[INV_INICIAL] [int] NULL ,
	[INV_FINAL] [int] NULL ,
	[VENTA_MEDIDA] [int] NULL ,
	[VENTA_SURTIDOR] [int] NULL ,
	[ID_PRODUCTO] [int] NULL ,
	[SOBRANTE_DIA] [int] NULL ,
	[SOBRANTE_ACUMULADO] [int] NULL ,
	[PORCENTAJE_EDS] [float] NULL ,
	CONSTRAINT [PK_CONTROL_COMBUSTIBLE] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EGRESOS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[EGRESOS] (
	[ID_EGRESO] [bigint] IDENTITY (1, 1) NOT NULL ,
	[TIPO_IDENTIFICACION] [bigint] NOT NULL ,
	[NUMERO] [bigint] NOT NULL ,
	[BENEFICIARIO] [varchar] (50) NULL ,
	[VALOR] [float] NOT NULL ,
	[FECHA] [datetime] NOT NULL ,
	[FECHA_APLICA] [datetime] NOT NULL ,
	[ISLA] [int] NOT NULL ,
	CONSTRAINT [PK_EGRESOS] PRIMARY KEY  CLUSTERED 
	(
		[ID_EGRESO]
	)  ON [PRIMARY] 
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[INGRESOS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[INGRESOS] (
	[ID_INGRESO] [bigint] IDENTITY (1, 1) NOT NULL ,
	[TIPO_IDENTIFICACION] [bigint] NOT NULL ,
	[CONSIGNACION] [bigint] NOT NULL ,
	[ID_BANCO] [bigint] NOT NULL ,
	[VALOR] [float] NOT NULL ,
	[FECHA] [datetime] NOT NULL ,
	CONSTRAINT [PK_INGRESOS] PRIMARY KEY  CLUSTERED 
	(
		[ID_INGRESO]
	)  ON [PRIMARY] 
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PRODUCTOS_TURNO]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[PRODUCTOS_TURNO] (
	[Id] [bigint] IDENTITY (1, 1) NOT NULL ,
	[Fecha] [datetime] NULL ,
	[Isla] [int] NULL ,
	[Turno] [int] NULL ,
	[Producto] [varchar] (50) NULL ,
	[Galones] [float] NULL ,
	[Valor] [float] NULL ,
	CONSTRAINT [PK_PRODUCTOS_TURNO] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)  ON [PRIMARY] 
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SOBRETASAS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[SOBRETASAS] (
	[ID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[MES_VIGENCIA] [int] NOT NULL ,
	[ANIO_VIGENCIA] [int] NOT NULL ,
	[ID_PRODUCTO] [int] NOT NULL ,
	[PRECIO_BASE] [float] NOT NULL ,
	[FLETES] [float] NULL ,
	[SOLDICOM] [float] NULL ,
	[SOBRETASA] [float] NOT NULL ,
	[VENTA] [float] NOT NULL ,
	[DIA_INICIO_VIGENCIA] [int] NOT NULL CONSTRAINT [DF_SOBRETASAS_DIA_INICIO_VIGENCIA] DEFAULT ((1)),
	[DIA_FIN_VIGENCIA] [int] NOT NULL CONSTRAINT [DF_SOBRETASAS_DIA_FIN_VIGENCIA] DEFAULT ((31)),
	CONSTRAINT [PK_SOBRETASAS] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USUARIOS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[USUARIOS] (
	[ID_USUARIO] [varchar] (15) NOT NULL ,
	[PWD] [varchar] (100) NOT NULL ,
	[ES_ADMIN] [int] NOT NULL ,
	CONSTRAINT [PK_USUARIOS] PRIMARY KEY  CLUSTERED 
	(
		[ID_USUARIO]
	)  ON [PRIMARY] 
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VENTAS]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[VENTAS] (
	[TIQUETE] [bigint] NOT NULL ,
	[FECHA] [datetime] NULL ,
	[NIT] [varchar] (20) NULL ,
	[CLIENTE] [varchar] (100) NULL ,
	[PRODUCTO] [varchar] (20) NULL ,
	[PLACA] [varchar] (10) NULL ,
	[KILOMETRAJE] [float] NULL ,
	[TOTAL] [float] NULL ,
	[ISLA] [int] NULL ,
	[TURNO] [int] NULL ,
	[MODO_PAGO] [bigint] NULL ,
	[FECHA_REGISTO] [datetime] NULL ,
	[COD_EMPLEADO] [bigint] NULL ,
	CONSTRAINT [PK_VENTAS] PRIMARY KEY  CLUSTERED 
	(
		[TIQUETE]
	)  ON [PRIMARY] 
) ON [PRIMARY]
END

GO


if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VENTAS_TURNO]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[VENTAS_TURNO] (
	[TIQUETE] [bigint] NOT NULL ,
	[FECHA] [datetime] NULL ,
	[PRODUCTO] [varchar] (20) NULL ,
	[TOTAL] [float] NULL ,
	[ISLA] [int] NULL ,
	[TURNO] [int] NULL ,
	[COD_EMPLEADO] [bigint] NULL ,
	[GALONES] [float] NULL ,
	CONSTRAINT [PK_VENTAS_TURNO] PRIMARY KEY  CLUSTERED 
	(
		[TIQUETE]
	)  ON [PRIMARY] 
) ON [PRIMARY]
END

GO


ALTER TABLE [dbo].[COMPRAS_COMBUSTIBLE] ADD CONSTRAINT [FK_COMPRAS_COMBUSTIBLE_PRODUCTOS] FOREIGN KEY 
	(
		[ID_PRODUCTO]
	) REFERENCES [PRODUCTOS] (
		[ID_PRODUCTO]
	)
GO
ALTER TABLE [dbo].[CONTROL_COMBUSTIBLE] ADD CONSTRAINT [FK_CONTROL_COMBUSTIBLE_PRODUCTOS] FOREIGN KEY 
	(
		[ID_PRODUCTO]
	) REFERENCES [PRODUCTOS] (
		[ID_PRODUCTO]
	)
GO
ALTER TABLE [dbo].[INGRESOS] ADD CONSTRAINT [FK_INGRESOS_BANCOS] FOREIGN KEY 
	(
		[ID_BANCO]
	) REFERENCES [BANCOS] (
		[ID_BANCO]
	)
GO
ALTER TABLE [dbo].[SOBRETASAS] ADD CONSTRAINT [FK_SOBRETASAS_PRODUCTOS] FOREIGN KEY 
	(
		[ID_PRODUCTO]
	) REFERENCES [PRODUCTOS] (
		[ID_PRODUCTO]
	)
GO
