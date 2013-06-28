USE [cs496database]
GO

/****** Object:  Table [dbo].[qtc]    Script Date: 04/30/2013 17:37:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[qtc](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[document_id] [varchar](50) NULL,
	[claim_number] [varchar](50) NULL,
	[contract_number] [varchar](50) NULL,
	[date_of_request] [varchar](255) NULL,
	[request_type] [varchar](255) NULL,
	[status] [varchar](255) NULL,
	[xml_data] [xml] NULL,
	[xml_data_malformed] [text] NULL,
	[created_by] [varchar](50) NULL,
	[created_date] [varchar](50) NULL,
	[LOB] [varchar](255) NULL,
 CONSTRAINT [PK_qtc] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


