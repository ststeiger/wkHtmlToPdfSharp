
-- USE [COR_Basic_Sursee];
-- USE [SwissRe_Test_V3];
GO 


IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[T_AP_Ref_Mandant_Logo]') AND type in (N'U'))
EXECUTE('
CREATE TABLE [dbo].[T_AP_Ref_Mandant_Logo](
	[LOGO_UID] [uniqueidentifier] NOT NULL,
	[LOGO_MDT_ID] [int] NULL,
	[LOGO_MDT_Lang_DE] [varchar](255) NULL,
	[LOGO_Path] [nvarchar](4000) NULL,
	[LOGO_Filename] [nvarchar](1001) NULL,
	[LOGO_Name] [nvarchar](1001) NULL,
	[LOGO_Code] [nvarchar](100) NULL,
	[LOGO_Kurz_DE] [varchar](50) NULL,
	[LOGO_Kurz_FR] [varchar](50) NULL,
	[LOGO_Kurz_IT] [varchar](50) NULL,
	[LOGO_Kurz_EN] [varchar](50) NULL,
	[LOGO_Lang_DE] [varchar](max) NULL,
	[LOGO_Lang_FR] [varchar](max) NULL,
	[LOGO_Lang_IT] [varchar](max) NULL,
	[LOGO_Lang_EN] [varchar](max) NULL,
	[LOGO_Width] [int] NOT NULL,
	[LOGO_Height] [int] NOT NULL,
	[LOGO_PaddingLeft] [float] NULL,
	[LOGO_PaddingRight] [float] NULL,
	[LOGO_PaddingTop] [float] NULL,
	[LOGO_PaddingBottom] [float] NULL,
	[LOGO_Hide] [bit] NOT NULL,
	[LOGO_DatumVon] [datetime] NOT NULL,
	[LOGO_DatumBis] [datetime] NOT NULL,
	[LOGO_Status] [int] NOT NULL,
	[LOGO_Sort] [int] NOT NULL,
	[LOGO_StylizerFore] [int] NULL,
	[LOGO_StylizerBack] [int] NULL,
	[LOGO_StylizerPattern] [int] NULL,
	[LOGO_StylizerLine] [int] NULL,
	[LOGO_IsDefault] [bit] NOT NULL,
	[LOGO_DatumMut] [datetime] NULL,
	[LOGO_DatumUser] [varchar](100) NULL,
 CONSTRAINT [PK_T_AP_Ref_Mandant_Logo] PRIMARY KEY CLUSTERED 
(
	[LOGO_UID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
') 

GO 
