using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Timers;
using ASME_C_WPF.core;
using Microsoft.SqlServer;
using System.Data.SqlClient;
using System.IO;
using ASME_C_WPF.ui.dialog;

namespace ASME_C_WPF
{
    /// <summary>
    /// Interaction logic for splash.xaml
    /// </summary>
    public partial class splash : Window
    {
        CoreDataContext db = new CoreDataContext();
        public splash()
        {
            InitializeComponent();
            nteu();
            systemcheck();
            if (db.master_users.Count() == 1)
            {
                dialog_initial_user dd = new dialog_initial_user();
                dd.ShowDialog();
            }
            Login log = new Login();
            log.Show();
            this.Close();
            
        }

        private void systemcheck()
        {
            string script = @"USE [master]
GO
/****** Object:  User [##MS_PolicyEventProcessingLogin##]    Script Date: 11/1/2016 3:31:33 PM ******/
CREATE USER [##MS_PolicyEventProcessingLogin##] FOR LOGIN [##MS_PolicyEventProcessingLogin##] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  User [##MS_AgentSigningCertificate##]    Script Date: 11/1/2016 3:31:34 PM ******/
CREATE USER [##MS_AgentSigningCertificate##] FOR LOGIN [##MS_AgentSigningCertificate##]
GO
/****** Object:  Table [dbo].[abc_log]    Script Date: 11/1/2016 3:31:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[abc_log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[month] [varchar](32) NOT NULL,
	[year] [int] NOT NULL,
	[produk] [int] NOT NULL,
	[cost] [float] NOT NULL,
	[costdriver] [int] NOT NULL,
	[unit_produksi] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Bahan_baku]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bahan_baku](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[nama] [varchar](100) NOT NULL,
	[harga_beli] [bigint] NOT NULL,
	[hpp_digunakan] [bigint] NOT NULL,
	[satuan] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[used] [int] NOT NULL,
	[active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[bb_log]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bb_log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[tipe] [int] NOT NULL,
	[kode_stock] [int] NOT NULL,
	[harga_beli] [bigint] NOT NULL,
	[hpp_digunakan] [bigint] NOT NULL,
	[date] [datetime] NOT NULL,
	[satuan] [int] NOT NULL,
	[add] [int] NOT NULL,
	[used] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[bb_stock]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bb_stock](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[tipe] [int] NOT NULL,
	[harga_beli] [bigint] NOT NULL,
	[hpp_digunakan] [bigint] NOT NULL,
	[satuan] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[used] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Beban]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Beban](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[nama] [varchar](100) NOT NULL,
	[jumlah] [bigint] NOT NULL,
	[repeatable] [bit] NOT NULL,
	[scope] [nchar](10) NOT NULL,
	[active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Category]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[nama] [varchar](100) NOT NULL,
	[active] [bit] NOT NULL,
	[pic] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[cost_driver]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cost_driver](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[nama] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Karyawan]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Karyawan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[nama] [varchar](100) NOT NULL,
	[gaji_pokok] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[master_user]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[master_user](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[pass] [varchar](32) NOT NULL,
	[role] [int] NOT NULL,
	[last_login] [datetime] NULL,
	[active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[master_user_log]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[master_user_log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[user] [int] NOT NULL,
	[activity] [int] NOT NULL,
	[date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Modal]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modal](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[detail] [varchar](100) NOT NULL,
	[jumlah] [bigint] NOT NULL,
	[sisa] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[p_bb]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[p_bb](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[produk] [int] NOT NULL,
	[bb] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Peralatan]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Peralatan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[nama] [varchar](100) NOT NULL,
	[harga] [bigint] NOT NULL,
	[quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Perlengkapan]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Perlengkapan](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[nama] [varchar](100) NOT NULL,
	[harga] [bigint] NOT NULL,
	[quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Piutang]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Piutang](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[detail] [varchar](100) NOT NULL,
	[jumlah] [bigint] NOT NULL,
	[terbayar] [bigint] NOT NULL,
	[date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[pos_log]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pos_log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[action] [varchar](100) NOT NULL,
	[date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[pos_order]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pos_order](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[details] [varchar](100) NOT NULL,
	[table_id] [int] NOT NULL,
	[date] [datetime] NOT NULL,
	[status] [varchar](10) NOT NULL,
	[discount] [int] NOT NULL,
	[disc_amount] [bigint] NOT NULL,
	[user] [int] NOT NULL,
	[service] [bigint] NOT NULL,
	[ppn] [bigint] NOT NULL,
	[total] [bigint] NOT NULL,
	[pembayaran] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[pos_order_list]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pos_order_list](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[peroduk] [int] NOT NULL,
	[order_id] [int] NOT NULL,
	[date] [datetime] NOT NULL,
	[quantity] [int] NOT NULL,
	[status] [varchar](10) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[pos_reservation]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pos_reservation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[details] [varchar](100) NOT NULL,
	[table_id] [int] NOT NULL,
	[date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[pos_table]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[pos_table](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[is_empty] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Produk]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Produk](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[nama] [varchar](100) NOT NULL,
	[SKU] [varchar](32) NOT NULL,
	[harga_jual] [bigint] NOT NULL,
	[active] [bit] NOT NULL,
	[category] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Satuan_bb]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Satuan_bb](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[nama] [varchar](100) NOT NULL,
	[active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Transaction]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[date] [datetime] NOT NULL,
	[type] [varchar](50) NULL,
	[quantity] [int] NOT NULL,
	[user] [int] NOT NULL,
	[details] [varchar](500) NULL,
	[jumlah] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Utang]    Script Date: 11/1/2016 3:31:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Utang](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[detail] [varchar](100) NOT NULL,
	[jumlah] [bigint] NOT NULL,
	[terbayar] [bigint] NOT NULL,
	[date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[abc_log] ADD  DEFAULT ((0.0)) FOR [cost]
GO
ALTER TABLE [dbo].[Bahan_baku] ADD  DEFAULT ((0)) FOR [harga_beli]
GO
ALTER TABLE [dbo].[Bahan_baku] ADD  DEFAULT ((0)) FOR [hpp_digunakan]
GO
ALTER TABLE [dbo].[Bahan_baku] ADD  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[Bahan_baku] ADD  DEFAULT ((0)) FOR [used]
GO
ALTER TABLE [dbo].[Bahan_baku] ADD  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[bb_log] ADD  DEFAULT ((0)) FOR [harga_beli]
GO
ALTER TABLE [dbo].[bb_log] ADD  DEFAULT ((0)) FOR [hpp_digunakan]
GO
ALTER TABLE [dbo].[bb_log] ADD  DEFAULT (getdate()) FOR [date]
GO
ALTER TABLE [dbo].[bb_log] ADD  DEFAULT ((0)) FOR [add]
GO
ALTER TABLE [dbo].[bb_log] ADD  DEFAULT ((0)) FOR [used]
GO
ALTER TABLE [dbo].[bb_stock] ADD  DEFAULT ((0)) FOR [harga_beli]
GO
ALTER TABLE [dbo].[bb_stock] ADD  DEFAULT ((0)) FOR [hpp_digunakan]
GO
ALTER TABLE [dbo].[bb_stock] ADD  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[bb_stock] ADD  DEFAULT ((0)) FOR [used]
GO
ALTER TABLE [dbo].[Beban] ADD  DEFAULT ((0)) FOR [jumlah]
GO
ALTER TABLE [dbo].[Beban] ADD  DEFAULT ((0)) FOR [repeatable]
GO
ALTER TABLE [dbo].[Beban] ADD  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[Category] ADD  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[Category] ADD  DEFAULT ((1)) FOR [pic]
GO
ALTER TABLE [dbo].[Karyawan] ADD  DEFAULT ((0)) FOR [gaji_pokok]
GO
ALTER TABLE [dbo].[master_user] ADD  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[Modal] ADD  DEFAULT ((0)) FOR [jumlah]
GO
ALTER TABLE [dbo].[Modal] ADD  DEFAULT ((0)) FOR [sisa]
GO
ALTER TABLE [dbo].[Peralatan] ADD  DEFAULT ((0)) FOR [harga]
GO
ALTER TABLE [dbo].[Peralatan] ADD  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[Perlengkapan] ADD  DEFAULT ((0)) FOR [harga]
GO
ALTER TABLE [dbo].[Perlengkapan] ADD  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[Piutang] ADD  DEFAULT ((0)) FOR [jumlah]
GO
ALTER TABLE [dbo].[Piutang] ADD  DEFAULT ((0)) FOR [terbayar]
GO
ALTER TABLE [dbo].[Piutang] ADD  DEFAULT (getdate()) FOR [date]
GO
ALTER TABLE [dbo].[pos_log] ADD  DEFAULT (getdate()) FOR [date]
GO
ALTER TABLE [dbo].[pos_order] ADD  DEFAULT (getdate()) FOR [date]
GO
ALTER TABLE [dbo].[pos_order] ADD  DEFAULT ((0)) FOR [discount]
GO
ALTER TABLE [dbo].[pos_order] ADD  DEFAULT ((0)) FOR [disc_amount]
GO
ALTER TABLE [dbo].[pos_order] ADD  DEFAULT ((0)) FOR [service]
GO
ALTER TABLE [dbo].[pos_order] ADD  DEFAULT ((0)) FOR [ppn]
GO
ALTER TABLE [dbo].[pos_order] ADD  DEFAULT ((0)) FOR [total]
GO
ALTER TABLE [dbo].[pos_order] ADD  DEFAULT ((0)) FOR [pembayaran]
GO
ALTER TABLE [dbo].[pos_order_list] ADD  DEFAULT (getdate()) FOR [date]
GO
ALTER TABLE [dbo].[pos_table] ADD  DEFAULT ((1)) FOR [is_empty]
GO
ALTER TABLE [dbo].[Produk] ADD  DEFAULT ((0)) FOR [harga_jual]
GO
ALTER TABLE [dbo].[Produk] ADD  DEFAULT ((0)) FOR [active]
GO
ALTER TABLE [dbo].[Satuan_bb] ADD  DEFAULT ((1)) FOR [active]
GO
ALTER TABLE [dbo].[Transaction] ADD  DEFAULT (getdate()) FOR [date]
GO
ALTER TABLE [dbo].[Transaction] ADD  DEFAULT ((0)) FOR [quantity]
GO
ALTER TABLE [dbo].[Transaction] ADD  DEFAULT ((0)) FOR [jumlah]
GO
ALTER TABLE [dbo].[Utang] ADD  DEFAULT ((0)) FOR [jumlah]
GO
ALTER TABLE [dbo].[Utang] ADD  DEFAULT ((0)) FOR [terbayar]
GO
ALTER TABLE [dbo].[Utang] ADD  DEFAULT (getdate()) FOR [date]
GO
ALTER TABLE [dbo].[abc_log]  WITH CHECK ADD  CONSTRAINT [abc_log_c] FOREIGN KEY([costdriver])
REFERENCES [dbo].[cost_driver] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[abc_log] CHECK CONSTRAINT [abc_log_c]
GO
ALTER TABLE [dbo].[abc_log]  WITH CHECK ADD  CONSTRAINT [abc_log_p] FOREIGN KEY([produk])
REFERENCES [dbo].[Produk] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[abc_log] CHECK CONSTRAINT [abc_log_p]
GO
ALTER TABLE [dbo].[Bahan_baku]  WITH CHECK ADD  CONSTRAINT [s_bb] FOREIGN KEY([satuan])
REFERENCES [dbo].[Satuan_bb] ([Id])
GO
ALTER TABLE [dbo].[Bahan_baku] CHECK CONSTRAINT [s_bb]
GO
ALTER TABLE [dbo].[bb_log]  WITH CHECK ADD  CONSTRAINT [bb_log_s] FOREIGN KEY([satuan])
REFERENCES [dbo].[Satuan_bb] ([Id])
GO
ALTER TABLE [dbo].[bb_log] CHECK CONSTRAINT [bb_log_s]
GO
ALTER TABLE [dbo].[bb_log]  WITH CHECK ADD  CONSTRAINT [bb_logs_b] FOREIGN KEY([tipe])
REFERENCES [dbo].[Bahan_baku] ([Id])
GO
ALTER TABLE [dbo].[bb_log] CHECK CONSTRAINT [bb_logs_b]
GO
ALTER TABLE [dbo].[bb_stock]  WITH CHECK ADD  CONSTRAINT [bb_stock_n] FOREIGN KEY([tipe])
REFERENCES [dbo].[Bahan_baku] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[bb_stock] CHECK CONSTRAINT [bb_stock_n]
GO
ALTER TABLE [dbo].[bb_stock]  WITH CHECK ADD  CONSTRAINT [bb_stock_s] FOREIGN KEY([satuan])
REFERENCES [dbo].[Satuan_bb] ([Id])
GO
ALTER TABLE [dbo].[bb_stock] CHECK CONSTRAINT [bb_stock_s]
GO
ALTER TABLE [dbo].[master_user_log]  WITH CHECK ADD  CONSTRAINT [user_log_u] FOREIGN KEY([user])
REFERENCES [dbo].[master_user] ([Id])
GO
ALTER TABLE [dbo].[master_user_log] CHECK CONSTRAINT [user_log_u]
GO
ALTER TABLE [dbo].[p_bb]  WITH CHECK ADD  CONSTRAINT [p_bb_b] FOREIGN KEY([bb])
REFERENCES [dbo].[Bahan_baku] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[p_bb] CHECK CONSTRAINT [p_bb_b]
GO
ALTER TABLE [dbo].[p_bb]  WITH CHECK ADD  CONSTRAINT [p_bb_p] FOREIGN KEY([produk])
REFERENCES [dbo].[Produk] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[p_bb] CHECK CONSTRAINT [p_bb_p]
GO
ALTER TABLE [dbo].[pos_log]  WITH CHECK ADD  CONSTRAINT [log_user_list] FOREIGN KEY([user_id])
REFERENCES [dbo].[master_user] ([Id])
GO
ALTER TABLE [dbo].[pos_log] CHECK CONSTRAINT [log_user_list]
GO
ALTER TABLE [dbo].[pos_order]  WITH CHECK ADD  CONSTRAINT [order_user] FOREIGN KEY([user])
REFERENCES [dbo].[master_user] ([Id])
GO
ALTER TABLE [dbo].[pos_order] CHECK CONSTRAINT [order_user]
GO
ALTER TABLE [dbo].[pos_order]  WITH CHECK ADD  CONSTRAINT [table_list_order] FOREIGN KEY([table_id])
REFERENCES [dbo].[pos_table] ([Id])
GO
ALTER TABLE [dbo].[pos_order] CHECK CONSTRAINT [table_list_order]
GO
ALTER TABLE [dbo].[pos_order_list]  WITH CHECK ADD  CONSTRAINT [order_list] FOREIGN KEY([order_id])
REFERENCES [dbo].[pos_order] ([Id])
GO
ALTER TABLE [dbo].[pos_order_list] CHECK CONSTRAINT [order_list]
GO
ALTER TABLE [dbo].[pos_order_list]  WITH CHECK ADD  CONSTRAINT [order_list_product] FOREIGN KEY([peroduk])
REFERENCES [dbo].[Produk] ([Id])
GO
ALTER TABLE [dbo].[pos_order_list] CHECK CONSTRAINT [order_list_product]
GO
ALTER TABLE [dbo].[pos_reservation]  WITH CHECK ADD  CONSTRAINT [table_list] FOREIGN KEY([table_id])
REFERENCES [dbo].[pos_table] ([Id])
GO
ALTER TABLE [dbo].[pos_reservation] CHECK CONSTRAINT [table_list]
GO
ALTER TABLE [dbo].[Produk]  WITH CHECK ADD  CONSTRAINT [product_category] FOREIGN KEY([category])
REFERENCES [dbo].[Category] ([Id])
GO
ALTER TABLE [dbo].[Produk] CHECK CONSTRAINT [product_category]
GO
ALTER TABLE [dbo].[Transaction]  WITH CHECK ADD  CONSTRAINT [transaction_u] FOREIGN KEY([user])
REFERENCES [dbo].[master_user] ([Id])
GO
ALTER TABLE [dbo].[Transaction] CHECK CONSTRAINT [transaction_u]
GO
";
               
            try
            {
                var user = db.master_users.Count();
            }catch
            {
                db.ExecuteCommand(script);
            }
            if (db.master_users.FirstOrDefault(c => c.Id == 1) == null)
            {
                master_user mds = new master_user();
                mds.name = "SYSTEM";
                mds.pass = Core.calculateMD5("jonsnow");
                mds.role = 1;
                db.master_users.InsertOnSubmit(mds);
                db.SubmitChanges();
            }
            
           
        }

        public void nteu()
        {
            this.Show();
            System.Threading.Thread.Sleep(2500);
        }
    }
}
