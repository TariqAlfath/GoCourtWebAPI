USE [master]
GO
/****** Object:  Database [GoCourt]    Script Date: 21/11/2023 14:41:25 ******/
CREATE DATABASE [GoCourt]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'GoCourt', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\GoCourt.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'GoCourt_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\GoCourt_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [GoCourt] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [GoCourt].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [GoCourt] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [GoCourt] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [GoCourt] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [GoCourt] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [GoCourt] SET ARITHABORT OFF 
GO
ALTER DATABASE [GoCourt] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [GoCourt] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [GoCourt] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [GoCourt] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [GoCourt] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [GoCourt] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [GoCourt] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [GoCourt] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [GoCourt] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [GoCourt] SET  DISABLE_BROKER 
GO
ALTER DATABASE [GoCourt] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [GoCourt] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [GoCourt] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [GoCourt] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [GoCourt] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [GoCourt] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [GoCourt] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [GoCourt] SET RECOVERY FULL 
GO
ALTER DATABASE [GoCourt] SET  MULTI_USER 
GO
ALTER DATABASE [GoCourt] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [GoCourt] SET DB_CHAINING OFF 
GO
ALTER DATABASE [GoCourt] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [GoCourt] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [GoCourt] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [GoCourt] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'GoCourt', N'ON'
GO
ALTER DATABASE [GoCourt] SET QUERY_STORE = OFF
GO
USE [GoCourt]
GO
/****** Object:  Table [dbo].[tbl_jenis_lapangan]    Script Date: 21/11/2023 14:41:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_jenis_lapangan](
	[id_jenis_lapangan] [int] IDENTITY(1,1) NOT NULL,
	[nama_jenis_lapangan] [varchar](88) NULL,
	[createdAt] [datetime] NULL,
	[createdBy] [varchar](50) NULL,
	[modifiedAt] [datetime] NULL,
	[modifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_jenis_lapangan] PRIMARY KEY CLUSTERED 
(
	[id_jenis_lapangan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_lapangan]    Script Date: 21/11/2023 14:41:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_lapangan](
	[id_lapangan] [int] IDENTITY(1,1) NOT NULL,
	[id_jenis_lapangan] [int] NULL,
	[nama_lapangan] [varchar](88) NULL,
	[harga_lapangan] [decimal](18, 0) NULL,
	[createdAt] [datetime] NULL,
	[createdBy] [varchar](50) NULL,
	[modifiedAt] [datetime] NULL,
	[modifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_lapangan] PRIMARY KEY CLUSTERED 
(
	[id_lapangan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_order]    Script Date: 21/11/2023 14:41:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_order](
	[id_order] [int] IDENTITY(1,1) NOT NULL,
	[id_lapangan] [int] NULL,
	[id_user] [uniqueidentifier] NULL,
	[rent_start] [datetime] NULL,
	[rent_end] [datetime] NULL,
	[payment_proof] [varbinary](max) NULL,
	[status] [varchar](50) NULL,
	[createdAt] [datetime] NULL,
	[createdBy] [varchar](50) NULL,
	[modifiedAt] [datetime] NULL,
	[modifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_order] PRIMARY KEY CLUSTERED 
(
	[id_order] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_review]    Script Date: 21/11/2023 14:41:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_review](
	[id_review] [int] IDENTITY(1,1) NOT NULL,
	[id_transaksi] [int] NULL,
	[deskripsi] [varchar](255) NULL,
	[score] [int] NULL,
	[createdAt] [datetime] NULL,
	[createdBy] [varchar](50) NULL,
	[modifiedAt] [datetime] NULL,
	[modifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_review] PRIMARY KEY CLUSTERED 
(
	[id_review] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_transaksi]    Script Date: 21/11/2023 14:41:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_transaksi](
	[id_transaksi] [int] IDENTITY(1,1) NOT NULL,
	[id_order] [int] NULL,
	[harga_total] [decimal](18, 0) NULL,
	[createdAt] [datetime] NULL,
	[createdBy] [varchar](50) NULL,
	[modifiedAt] [datetime] NULL,
	[modifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_transaksi] PRIMARY KEY CLUSTERED 
(
	[id_transaksi] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_user]    Script Date: 21/11/2023 14:41:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_user](
	[id_user] [uniqueidentifier] NOT NULL,
	[username] [varchar](50) NULL,
	[password] [varchar](max) NULL,
	[nama] [varchar](50) NULL,
	[alamat] [varchar](50) NULL,
	[nomor_telefon] [varchar](25) NULL,
	[email] [varchar](50) NULL,
	[role] [varchar](20) NULL,
	[createdAt] [datetime] NULL,
	[createdBy] [varchar](50) NULL,
	[modifiedAt] [datetime] NULL,
	[modifiedBy] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_user] PRIMARY KEY CLUSTERED 
(
	[id_user] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_user] ADD  CONSTRAINT [DF_tbl_user_id_user]  DEFAULT (newid()) FOR [id_user]
GO
ALTER TABLE [dbo].[tbl_lapangan]  WITH CHECK ADD  CONSTRAINT [FK_tbl_lapangan_tbl_lapangan] FOREIGN KEY([id_jenis_lapangan])
REFERENCES [dbo].[tbl_jenis_lapangan] ([id_jenis_lapangan])
GO
ALTER TABLE [dbo].[tbl_lapangan] CHECK CONSTRAINT [FK_tbl_lapangan_tbl_lapangan]
GO
ALTER TABLE [dbo].[tbl_order]  WITH CHECK ADD  CONSTRAINT [FK_tbl_order_tbl_lapangan] FOREIGN KEY([id_user])
REFERENCES [dbo].[tbl_user] ([id_user])
GO
ALTER TABLE [dbo].[tbl_order] CHECK CONSTRAINT [FK_tbl_order_tbl_lapangan]
GO
ALTER TABLE [dbo].[tbl_review]  WITH CHECK ADD  CONSTRAINT [FK_tbl_review_tbl_transaksi] FOREIGN KEY([id_transaksi])
REFERENCES [dbo].[tbl_transaksi] ([id_transaksi])
GO
ALTER TABLE [dbo].[tbl_review] CHECK CONSTRAINT [FK_tbl_review_tbl_transaksi]
GO
ALTER TABLE [dbo].[tbl_transaksi]  WITH CHECK ADD  CONSTRAINT [FK_tbl_transaksi_tbl_transaksi] FOREIGN KEY([id_order])
REFERENCES [dbo].[tbl_order] ([id_order])
GO
ALTER TABLE [dbo].[tbl_transaksi] CHECK CONSTRAINT [FK_tbl_transaksi_tbl_transaksi]
GO
USE [master]
GO
ALTER DATABASE [GoCourt] SET  READ_WRITE 
GO
