
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/13/2015 00:07:46
-- Generated from EDMX file: H:\Projects\BGK\BGK\Models\BGKModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__bgk_bildi__UyeID__3EDC53F0]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_bildirim] DROP CONSTRAINT [FK__bgk_bildi__UyeID__3EDC53F0];
GO
IF OBJECT_ID(N'[dbo].[FK__bgk_dokum__Dosya__54CB950F]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_dokuman] DROP CONSTRAINT [FK__bgk_dokum__Dosya__54CB950F];
GO
IF OBJECT_ID(N'[dbo].[FK__bgk_dokum__Kateg__53D770D6]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_dokuman] DROP CONSTRAINT [FK__bgk_dokum__Kateg__53D770D6];
GO
IF OBJECT_ID(N'[dbo].[FK__bgk_dokum__UyeID__4B422AD5]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_dokuman] DROP CONSTRAINT [FK__bgk_dokum__UyeID__4B422AD5];
GO
IF OBJECT_ID(N'[dbo].[FK__bgk_not__UyeID__3A179ED3]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_not] DROP CONSTRAINT [FK__bgk_not__UyeID__3A179ED3];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_anket_secenek_bgk_anket]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_anket_secenek] DROP CONSTRAINT [FK_bgk_anket_secenek_bgk_anket];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_anket_secim_bgk_anket_secenek]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_anket_secim] DROP CONSTRAINT [FK_bgk_anket_secim_bgk_anket_secenek];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_anket_secim_bgk_uye]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_anket_secim] DROP CONSTRAINT [FK_bgk_anket_secim_bgk_uye];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_butce_bgk_dosya]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_butce] DROP CONSTRAINT [FK_bgk_butce_bgk_dosya];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_butce_bgk_etkinlik]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_butce] DROP CONSTRAINT [FK_bgk_butce_bgk_etkinlik];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_butce_bgk_firma]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_butce] DROP CONSTRAINT [FK_bgk_butce_bgk_firma];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_dosya_bgk_uye]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_dosya] DROP CONSTRAINT [FK_bgk_dosya_bgk_uye];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_etkinlik_bgk_dosya]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_etkinlik] DROP CONSTRAINT [FK_bgk_etkinlik_bgk_dosya];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_etkinlik_bgk_firma]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_etkinlik] DROP CONSTRAINT [FK_bgk_etkinlik_bgk_firma];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_etkinlik_gorevli_bgk_etkinlik]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_etkinlik_gorevli] DROP CONSTRAINT [FK_bgk_etkinlik_gorevli_bgk_etkinlik];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_etkinlik_gorevli_bgk_gorev]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_etkinlik_gorevli] DROP CONSTRAINT [FK_bgk_etkinlik_gorevli_bgk_gorev];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_etkinlik_gorevli_bgk_uye]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_etkinlik_gorevli] DROP CONSTRAINT [FK_bgk_etkinlik_gorevli_bgk_uye];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_etkinlik_konusmaci_bgk_etkinlik]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_etkinlik_konusmaci] DROP CONSTRAINT [FK_bgk_etkinlik_konusmaci_bgk_etkinlik];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_firma_bgk_dosya]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_firma] DROP CONSTRAINT [FK_bgk_firma_bgk_dosya];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_firma_bgk_uye]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_firma] DROP CONSTRAINT [FK_bgk_firma_bgk_uye];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_galeri_resim_bgk_dosya]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_galeri_resim] DROP CONSTRAINT [FK_bgk_galeri_resim_bgk_dosya];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_galeri_resim_bgk_galeri]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_galeri_resim] DROP CONSTRAINT [FK_bgk_galeri_resim_bgk_galeri];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_gorev_bgk_gorev_kategori]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_gorev] DROP CONSTRAINT [FK_bgk_gorev_bgk_gorev_kategori];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_gorev_bgk_grup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_gorev] DROP CONSTRAINT [FK_bgk_gorev_bgk_grup];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_gorev_bgk_uye]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_gorev] DROP CONSTRAINT [FK_bgk_gorev_bgk_uye];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_gorev_kategori_bgk_dosya]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_gorev_kategori] DROP CONSTRAINT [FK_bgk_gorev_kategori_bgk_dosya];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_gorev_kategori_bgk_uye]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_gorev_kategori] DROP CONSTRAINT [FK_bgk_gorev_kategori_bgk_uye];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_gorev_kategori_uye_bgk_gorev_kategori]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_gorev_kategori_uye] DROP CONSTRAINT [FK_bgk_gorev_kategori_uye_bgk_gorev_kategori];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_gorev_uye_bgk_dosya]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_gorev_uye] DROP CONSTRAINT [FK_bgk_gorev_uye_bgk_dosya];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_gorev_uye_bgk_gorev]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_gorev_uye] DROP CONSTRAINT [FK_bgk_gorev_uye_bgk_gorev];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_gorev_uye_bgk_uye]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_gorev_uye] DROP CONSTRAINT [FK_bgk_gorev_uye_bgk_uye];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_grup_uye_bgk_grup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_grup_uye] DROP CONSTRAINT [FK_bgk_grup_uye_bgk_grup];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_grup_uye_bgk_uye]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_grup_uye] DROP CONSTRAINT [FK_bgk_grup_uye_bgk_uye];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_kategori_uye_bgk_uye]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_gorev_kategori_uye] DROP CONSTRAINT [FK_bgk_kategori_uye_bgk_uye];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_oylama_bgk_uye]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_oylama] DROP CONSTRAINT [FK_bgk_oylama_bgk_uye];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_oylama_bgk_yazi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_oylama] DROP CONSTRAINT [FK_bgk_oylama_bgk_yazi];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_yazi_bgk_kategori]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_yazi] DROP CONSTRAINT [FK_bgk_yazi_bgk_kategori];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_yazi_bgk_uye]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_yazi] DROP CONSTRAINT [FK_bgk_yazi_bgk_uye];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_yorum_bgk_uye]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_yorum] DROP CONSTRAINT [FK_bgk_yorum_bgk_uye];
GO
IF OBJECT_ID(N'[dbo].[FK_bgk_yorum_bgk_yazi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[bgk_yorum] DROP CONSTRAINT [FK_bgk_yorum_bgk_yazi];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[bgk_anket]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_anket];
GO
IF OBJECT_ID(N'[dbo].[bgk_anket_secenek]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_anket_secenek];
GO
IF OBJECT_ID(N'[dbo].[bgk_anket_secim]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_anket_secim];
GO
IF OBJECT_ID(N'[dbo].[bgk_ayar]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_ayar];
GO
IF OBJECT_ID(N'[dbo].[bgk_bildirim]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_bildirim];
GO
IF OBJECT_ID(N'[dbo].[bgk_butce]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_butce];
GO
IF OBJECT_ID(N'[dbo].[bgk_dokuman]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_dokuman];
GO
IF OBJECT_ID(N'[dbo].[bgk_dokuman_kategori]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_dokuman_kategori];
GO
IF OBJECT_ID(N'[dbo].[bgk_dosya]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_dosya];
GO
IF OBJECT_ID(N'[dbo].[bgk_etkinlik]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_etkinlik];
GO
IF OBJECT_ID(N'[dbo].[bgk_etkinlik_gorevli]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_etkinlik_gorevli];
GO
IF OBJECT_ID(N'[dbo].[bgk_etkinlik_konusmaci]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_etkinlik_konusmaci];
GO
IF OBJECT_ID(N'[dbo].[bgk_firma]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_firma];
GO
IF OBJECT_ID(N'[dbo].[bgk_galeri]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_galeri];
GO
IF OBJECT_ID(N'[dbo].[bgk_galeri_resim]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_galeri_resim];
GO
IF OBJECT_ID(N'[dbo].[bgk_gorev]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_gorev];
GO
IF OBJECT_ID(N'[dbo].[bgk_gorev_kategori]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_gorev_kategori];
GO
IF OBJECT_ID(N'[dbo].[bgk_gorev_kategori_uye]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_gorev_kategori_uye];
GO
IF OBJECT_ID(N'[dbo].[bgk_gorev_uye]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_gorev_uye];
GO
IF OBJECT_ID(N'[dbo].[bgk_grup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_grup];
GO
IF OBJECT_ID(N'[dbo].[bgk_grup_uye]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_grup_uye];
GO
IF OBJECT_ID(N'[dbo].[bgk_kategori]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_kategori];
GO
IF OBJECT_ID(N'[dbo].[bgk_menu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_menu];
GO
IF OBJECT_ID(N'[dbo].[bgk_mesaj]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_mesaj];
GO
IF OBJECT_ID(N'[dbo].[bgk_not]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_not];
GO
IF OBJECT_ID(N'[dbo].[bgk_oylama]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_oylama];
GO
IF OBJECT_ID(N'[dbo].[bgk_sayfa]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_sayfa];
GO
IF OBJECT_ID(N'[dbo].[bgk_seviye]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_seviye];
GO
IF OBJECT_ID(N'[dbo].[bgk_uye]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_uye];
GO
IF OBJECT_ID(N'[dbo].[bgk_yazi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_yazi];
GO
IF OBJECT_ID(N'[dbo].[bgk_yetki]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_yetki];
GO
IF OBJECT_ID(N'[dbo].[bgk_yorum]', 'U') IS NOT NULL
    DROP TABLE [dbo].[bgk_yorum];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'bgk_yetki'
CREATE TABLE [dbo].[bgk_yetki] (
    [Id] int  NOT NULL,
    [Adi] nvarchar(50)  NOT NULL,
    [Aciklama] nvarchar(max)  NULL,
    [Yazi] bit  NOT NULL,
    [Yorum] bit  NOT NULL,
    [Sayfa] bit  NOT NULL,
    [Ayar] bit  NOT NULL,
    [Etkinlik] bit  NOT NULL,
    [Butce] bit  NOT NULL,
    [Dosya] bit  NOT NULL,
    [Grup] bit  NOT NULL,
    [Gorev] bit  NOT NULL,
    [Bildirim] bit  NOT NULL,
    [Firma] bit  NOT NULL,
    [Link] bit  NOT NULL,
    [Uye] bit  NOT NULL,
    [Seviye] bit  NOT NULL,
    [Dokuman] bit  NOT NULL,
    [Not] bit  NOT NULL,
    [Galeri] bit  NOT NULL,
    [Anket] bit  NOT NULL,
    [YaziOnay] bit  NOT NULL,
    [YorumOnay] bit  NOT NULL,
    [GorevVerme] bit  NOT NULL,
    [Kod] int  NULL,
    [YaziYazma] bit  NOT NULL,
    [YorumYazma] bit  NOT NULL,
    [BakimdaGiris] bit  NOT NULL,
    [OyKullanma] bit  NOT NULL
);
GO

-- Creating table 'bgk_anket'
CREATE TABLE [dbo].[bgk_anket] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Soru] nvarchar(255)  NOT NULL,
    [CokluSecim] bit  NOT NULL,
    [SadeceUye] bit  NOT NULL,
    [BaslangicTarihi] datetime  NOT NULL,
    [BitisTarihi] datetime  NOT NULL,
    [Sira] int  NOT NULL,
    [Onay] bit  NOT NULL
);
GO

-- Creating table 'bgk_anket_secenek'
CREATE TABLE [dbo].[bgk_anket_secenek] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Adi] nvarchar(255)  NOT NULL,
    [AnketID] int  NOT NULL,
    [Sira] int  NOT NULL
);
GO

-- Creating table 'bgk_anket_secim'
CREATE TABLE [dbo].[bgk_anket_secim] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UyeID] int  NULL,
    [SecenekID] int  NOT NULL,
    [OylayanIP] nvarchar(50)  NULL,
    [SecimTarihi] datetime  NOT NULL
);
GO

-- Creating table 'bgk_ayar'
CREATE TABLE [dbo].[bgk_ayar] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Adi] nvarchar(50)  NOT NULL,
    [Aciklama] nvarchar(255)  NULL,
    [Deger] nvarchar(max)  NULL
);
GO

-- Creating table 'bgk_bildirim'
CREATE TABLE [dbo].[bgk_bildirim] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Icerik] nvarchar(max)  NOT NULL,
    [UyeID] int  NOT NULL,
    [Tarih] datetime  NOT NULL,
    [Okundu] bit  NOT NULL
);
GO

-- Creating table 'bgk_butce'
CREATE TABLE [dbo].[bgk_butce] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [HarcananYer] nvarchar(50)  NULL,
    [Miktar] decimal(19,4)  NOT NULL,
    [Aciklama] nvarchar(max)  NOT NULL,
    [EtkinlikID] int  NULL,
    [FaturaID] int  NULL,
    [FirmaID] int  NOT NULL,
    [IslemTarihi] datetime  NULL,
    [Islem] int  NOT NULL
);
GO

-- Creating table 'bgk_dokuman'
CREATE TABLE [dbo].[bgk_dokuman] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Adi] nvarchar(255)  NOT NULL,
    [Icerik] nvarchar(max)  NOT NULL,
    [UyeID] int  NOT NULL,
    [EklenmeTarihi] datetime  NOT NULL,
    [Onay] bit  NOT NULL,
    [Seo] nvarchar(50)  NOT NULL,
    [Sira] int  NOT NULL,
    [KategoriID] int  NOT NULL,
    [DosyaID] int  NULL
);
GO

-- Creating table 'bgk_dokuman_kategori'
CREATE TABLE [dbo].[bgk_dokuman_kategori] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Adi] nvarchar(50)  NOT NULL,
    [Aciklama] nvarchar(max)  NULL,
    [Onay] bit  NOT NULL,
    [Seo] nvarchar(50)  NOT NULL,
    [Sira] int  NOT NULL
);
GO

-- Creating table 'bgk_dosya'
CREATE TABLE [dbo].[bgk_dosya] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Aciklama] nvarchar(255)  NULL,
    [DosyaAdi] nvarchar(255)  NOT NULL,
    [DosyaTipi] nvarchar(50)  NOT NULL,
    [YukleyenID] int  NOT NULL,
    [YuklenmeTarihi] datetime  NOT NULL,
    [Adres] nvarchar(255)  NOT NULL
);
GO

-- Creating table 'bgk_etkinlik'
CREATE TABLE [dbo].[bgk_etkinlik] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Adi] nvarchar(50)  NOT NULL,
    [Aciklama] nvarchar(max)  NOT NULL,
    [FirmaID] int  NULL,
    [AfisID] int  NULL,
    [BaslangicTarihi] datetime  NOT NULL,
    [BitisTarihi] datetime  NOT NULL
);
GO

-- Creating table 'bgk_etkinlik_gorevli'
CREATE TABLE [dbo].[bgk_etkinlik_gorevli] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UyeID] int  NOT NULL,
    [GorevID] int  NOT NULL,
    [EtkinlikID] int  NOT NULL
);
GO

-- Creating table 'bgk_etkinlik_konusmaci'
CREATE TABLE [dbo].[bgk_etkinlik_konusmaci] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AdSoyad] nvarchar(50)  NOT NULL,
    [Konu] nvarchar(255)  NOT NULL,
    [Aciklama] nvarchar(max)  NULL,
    [EtkinlikID] int  NOT NULL
);
GO

-- Creating table 'bgk_firma'
CREATE TABLE [dbo].[bgk_firma] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Adi] nvarchar(50)  NOT NULL,
    [Aciklama] nvarchar(max)  NULL,
    [UyeID] int  NULL,
    [ResimID] int  NOT NULL
);
GO

-- Creating table 'bgk_galeri'
CREATE TABLE [dbo].[bgk_galeri] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Adi] nvarchar(50)  NOT NULL,
    [Aciklama] nvarchar(max)  NULL,
    [Seo] nvarchar(50)  NOT NULL,
    [OlusturulmaTarihi] datetime  NOT NULL,
    [Onay] bit  NOT NULL
);
GO

-- Creating table 'bgk_galeri_resim'
CREATE TABLE [dbo].[bgk_galeri_resim] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Aciklama] nvarchar(255)  NULL,
    [ResimID] int  NOT NULL,
    [GaleriID] int  NOT NULL,
    [Sira] int  NOT NULL,
    [EklenmeTarihi] datetime  NOT NULL,
    [Onay] bit  NOT NULL
);
GO

-- Creating table 'bgk_gorev'
CREATE TABLE [dbo].[bgk_gorev] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Adi] nvarchar(50)  NOT NULL,
    [Aciklama] nvarchar(max)  NOT NULL,
    [GrupID] int  NULL,
    [KategoriID] int  NULL,
    [OlusturanID] int  NOT NULL,
    [Puan] int  NOT NULL,
    [CezaPuani] int  NULL,
    [GunSayisi] int  NULL,
    [OlusturulmaTarihi] datetime  NOT NULL,
    [BitisTarihi] datetime  NULL,
    [Tip] int  NOT NULL,
    [Onay] bit  NOT NULL,
    [Sira] int  NULL
);
GO

-- Creating table 'bgk_gorev_kategori'
CREATE TABLE [dbo].[bgk_gorev_kategori] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Adi] nvarchar(50)  NOT NULL,
    [Aciklama] nvarchar(max)  NULL,
    [ResimID] int  NOT NULL,
    [OlusturanID] int  NOT NULL,
    [PuanSiniri] int  NULL,
    [Sira] int  NOT NULL,
    [Onay] bit  NOT NULL
);
GO

-- Creating table 'bgk_gorev_kategori_uye'
CREATE TABLE [dbo].[bgk_gorev_kategori_uye] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UyeID] int  NOT NULL,
    [KategoriID] int  NOT NULL,
    [BaslamaTarihi] datetime  NOT NULL
);
GO

-- Creating table 'bgk_gorev_uye'
CREATE TABLE [dbo].[bgk_gorev_uye] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Aciklama] nvarchar(max)  NULL,
    [UyeID] int  NOT NULL,
    [GorevID] int  NOT NULL,
    [Rapor] nvarchar(max)  NULL,
    [DosyaID] int  NULL,
    [BaslangicTarihi] datetime  NOT NULL,
    [BitisTarihi] datetime  NOT NULL,
    [Tamamlandi] bit  NOT NULL,
    [Kabul] bit  NULL,
    [Onay] bit  NULL
);
GO

-- Creating table 'bgk_grup'
CREATE TABLE [dbo].[bgk_grup] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Adi] nvarchar(50)  NOT NULL,
    [Aciklama] nvarchar(max)  NULL,
    [Tip] int  NOT NULL
);
GO

-- Creating table 'bgk_grup_uye'
CREATE TABLE [dbo].[bgk_grup_uye] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Aciklama] nvarchar(max)  NULL,
    [UyeID] int  NOT NULL,
    [GrupID] int  NOT NULL,
    [BaslangicTarihi] datetime  NOT NULL,
    [BitisTarihi] datetime  NULL,
    [Tip] int  NOT NULL,
    [Aktif] bit  NOT NULL
);
GO

-- Creating table 'bgk_kategori'
CREATE TABLE [dbo].[bgk_kategori] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Adi] nvarchar(50)  NOT NULL,
    [Seo] nvarchar(50)  NOT NULL,
    [Sira] int  NOT NULL,
    [Onay] bit  NOT NULL
);
GO

-- Creating table 'bgk_menu'
CREATE TABLE [dbo].[bgk_menu] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Adi] nvarchar(50)  NOT NULL,
    [Adres] nvarchar(255)  NOT NULL,
    [Sira] int  NOT NULL,
    [SadeceUye] bit  NOT NULL,
    [Onay] bit  NOT NULL
);
GO

-- Creating table 'bgk_mesaj'
CREATE TABLE [dbo].[bgk_mesaj] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Mesaj] nvarchar(max)  NOT NULL,
    [Tip] int  NOT NULL,
    [GonderenID] int  NOT NULL,
    [AliciID] int  NOT NULL,
    [Okundu] bit  NOT NULL,
    [YazimTarihi] datetime  NOT NULL
);
GO

-- Creating table 'bgk_not'
CREATE TABLE [dbo].[bgk_not] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Konu] nvarchar(255)  NOT NULL,
    [Icerik] nvarchar(max)  NOT NULL,
    [UyeID] int  NOT NULL,
    [Tarih] datetime  NOT NULL
);
GO

-- Creating table 'bgk_oylama'
CREATE TABLE [dbo].[bgk_oylama] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UyeID] int  NULL,
    [YaziID] int  NOT NULL,
    [Puan] int  NOT NULL,
    [OylayanIP] nvarchar(50)  NULL
);
GO

-- Creating table 'bgk_sayfa'
CREATE TABLE [dbo].[bgk_sayfa] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Adi] nvarchar(50)  NOT NULL,
    [Icerik] nvarchar(max)  NOT NULL,
    [Seo] nvarchar(50)  NOT NULL,
    [Onay] bit  NOT NULL
);
GO

-- Creating table 'bgk_seviye'
CREATE TABLE [dbo].[bgk_seviye] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Adi] nvarchar(50)  NOT NULL,
    [Aciklama] nvarchar(max)  NULL,
    [AltSinir] int  NOT NULL
);
GO

-- Creating table 'bgk_uye'
CREATE TABLE [dbo].[bgk_uye] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AdSoyad] nvarchar(50)  NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [Sifre] nvarchar(255)  NOT NULL,
    [Telefon] int  NULL,
    [OgrenciNo] int  NOT NULL,
    [Fakulte] nvarchar(50)  NOT NULL,
    [Bolum] nvarchar(50)  NOT NULL,
    [GirisYili] int  NOT NULL,
    [Puan] int  NOT NULL,
    [CezaPuani] int  NOT NULL,
    [Yetki] int  NOT NULL,
    [SonGiris] datetime  NOT NULL,
    [KayitTarihi] datetime  NOT NULL,
    [Onay] bit  NOT NULL
);
GO

-- Creating table 'bgk_yazi'
CREATE TABLE [dbo].[bgk_yazi] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Baslik] nvarchar(255)  NOT NULL,
    [Icerik] nvarchar(max)  NOT NULL,
    [Ozet] nvarchar(max)  NULL,
    [KategoriID] int  NOT NULL,
    [UyeID] int  NOT NULL,
    [Seo] nvarchar(255)  NOT NULL,
    [Okunma] int  NOT NULL,
    [YazimTarihi] datetime  NOT NULL,
    [Manset] bit  NOT NULL,
    [Onay] bit  NOT NULL
);
GO

-- Creating table 'bgk_yorum'
CREATE TABLE [dbo].[bgk_yorum] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Yorum] nvarchar(max)  NOT NULL,
    [YaziID] int  NOT NULL,
    [UyeID] int  NULL,
    [Yazan] nvarchar(50)  NULL,
    [YazilmaTarihi] datetime  NOT NULL,
    [Onay] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'bgk_yetki'
ALTER TABLE [dbo].[bgk_yetki]
ADD CONSTRAINT [PK_bgk_yetki]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_anket'
ALTER TABLE [dbo].[bgk_anket]
ADD CONSTRAINT [PK_bgk_anket]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_anket_secenek'
ALTER TABLE [dbo].[bgk_anket_secenek]
ADD CONSTRAINT [PK_bgk_anket_secenek]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_anket_secim'
ALTER TABLE [dbo].[bgk_anket_secim]
ADD CONSTRAINT [PK_bgk_anket_secim]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_ayar'
ALTER TABLE [dbo].[bgk_ayar]
ADD CONSTRAINT [PK_bgk_ayar]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_bildirim'
ALTER TABLE [dbo].[bgk_bildirim]
ADD CONSTRAINT [PK_bgk_bildirim]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_butce'
ALTER TABLE [dbo].[bgk_butce]
ADD CONSTRAINT [PK_bgk_butce]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_dokuman'
ALTER TABLE [dbo].[bgk_dokuman]
ADD CONSTRAINT [PK_bgk_dokuman]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_dokuman_kategori'
ALTER TABLE [dbo].[bgk_dokuman_kategori]
ADD CONSTRAINT [PK_bgk_dokuman_kategori]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_dosya'
ALTER TABLE [dbo].[bgk_dosya]
ADD CONSTRAINT [PK_bgk_dosya]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_etkinlik'
ALTER TABLE [dbo].[bgk_etkinlik]
ADD CONSTRAINT [PK_bgk_etkinlik]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_etkinlik_gorevli'
ALTER TABLE [dbo].[bgk_etkinlik_gorevli]
ADD CONSTRAINT [PK_bgk_etkinlik_gorevli]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_etkinlik_konusmaci'
ALTER TABLE [dbo].[bgk_etkinlik_konusmaci]
ADD CONSTRAINT [PK_bgk_etkinlik_konusmaci]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_firma'
ALTER TABLE [dbo].[bgk_firma]
ADD CONSTRAINT [PK_bgk_firma]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_galeri'
ALTER TABLE [dbo].[bgk_galeri]
ADD CONSTRAINT [PK_bgk_galeri]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_galeri_resim'
ALTER TABLE [dbo].[bgk_galeri_resim]
ADD CONSTRAINT [PK_bgk_galeri_resim]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_gorev'
ALTER TABLE [dbo].[bgk_gorev]
ADD CONSTRAINT [PK_bgk_gorev]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_gorev_kategori'
ALTER TABLE [dbo].[bgk_gorev_kategori]
ADD CONSTRAINT [PK_bgk_gorev_kategori]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_gorev_kategori_uye'
ALTER TABLE [dbo].[bgk_gorev_kategori_uye]
ADD CONSTRAINT [PK_bgk_gorev_kategori_uye]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_gorev_uye'
ALTER TABLE [dbo].[bgk_gorev_uye]
ADD CONSTRAINT [PK_bgk_gorev_uye]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_grup'
ALTER TABLE [dbo].[bgk_grup]
ADD CONSTRAINT [PK_bgk_grup]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_grup_uye'
ALTER TABLE [dbo].[bgk_grup_uye]
ADD CONSTRAINT [PK_bgk_grup_uye]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_kategori'
ALTER TABLE [dbo].[bgk_kategori]
ADD CONSTRAINT [PK_bgk_kategori]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_menu'
ALTER TABLE [dbo].[bgk_menu]
ADD CONSTRAINT [PK_bgk_menu]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_mesaj'
ALTER TABLE [dbo].[bgk_mesaj]
ADD CONSTRAINT [PK_bgk_mesaj]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_not'
ALTER TABLE [dbo].[bgk_not]
ADD CONSTRAINT [PK_bgk_not]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_oylama'
ALTER TABLE [dbo].[bgk_oylama]
ADD CONSTRAINT [PK_bgk_oylama]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_sayfa'
ALTER TABLE [dbo].[bgk_sayfa]
ADD CONSTRAINT [PK_bgk_sayfa]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_seviye'
ALTER TABLE [dbo].[bgk_seviye]
ADD CONSTRAINT [PK_bgk_seviye]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_uye'
ALTER TABLE [dbo].[bgk_uye]
ADD CONSTRAINT [PK_bgk_uye]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_yazi'
ALTER TABLE [dbo].[bgk_yazi]
ADD CONSTRAINT [PK_bgk_yazi]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'bgk_yorum'
ALTER TABLE [dbo].[bgk_yorum]
ADD CONSTRAINT [PK_bgk_yorum]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AnketID] in table 'bgk_anket_secenek'
ALTER TABLE [dbo].[bgk_anket_secenek]
ADD CONSTRAINT [FK_bgk_anket_secenek_bgk_anket]
    FOREIGN KEY ([AnketID])
    REFERENCES [dbo].[bgk_anket]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_anket_secenek_bgk_anket'
CREATE INDEX [IX_FK_bgk_anket_secenek_bgk_anket]
ON [dbo].[bgk_anket_secenek]
    ([AnketID]);
GO

-- Creating foreign key on [SecenekID] in table 'bgk_anket_secim'
ALTER TABLE [dbo].[bgk_anket_secim]
ADD CONSTRAINT [FK_bgk_anket_secim_bgk_anket_secenek]
    FOREIGN KEY ([SecenekID])
    REFERENCES [dbo].[bgk_anket_secenek]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_anket_secim_bgk_anket_secenek'
CREATE INDEX [IX_FK_bgk_anket_secim_bgk_anket_secenek]
ON [dbo].[bgk_anket_secim]
    ([SecenekID]);
GO

-- Creating foreign key on [UyeID] in table 'bgk_anket_secim'
ALTER TABLE [dbo].[bgk_anket_secim]
ADD CONSTRAINT [FK_bgk_anket_secim_bgk_uye]
    FOREIGN KEY ([UyeID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_anket_secim_bgk_uye'
CREATE INDEX [IX_FK_bgk_anket_secim_bgk_uye]
ON [dbo].[bgk_anket_secim]
    ([UyeID]);
GO

-- Creating foreign key on [UyeID] in table 'bgk_bildirim'
ALTER TABLE [dbo].[bgk_bildirim]
ADD CONSTRAINT [FK__bgk_bildi__UyeID__3EDC53F0]
    FOREIGN KEY ([UyeID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__bgk_bildi__UyeID__3EDC53F0'
CREATE INDEX [IX_FK__bgk_bildi__UyeID__3EDC53F0]
ON [dbo].[bgk_bildirim]
    ([UyeID]);
GO

-- Creating foreign key on [FaturaID] in table 'bgk_butce'
ALTER TABLE [dbo].[bgk_butce]
ADD CONSTRAINT [FK_bgk_butce_bgk_dosya]
    FOREIGN KEY ([FaturaID])
    REFERENCES [dbo].[bgk_dosya]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_butce_bgk_dosya'
CREATE INDEX [IX_FK_bgk_butce_bgk_dosya]
ON [dbo].[bgk_butce]
    ([FaturaID]);
GO

-- Creating foreign key on [EtkinlikID] in table 'bgk_butce'
ALTER TABLE [dbo].[bgk_butce]
ADD CONSTRAINT [FK_bgk_butce_bgk_etkinlik]
    FOREIGN KEY ([EtkinlikID])
    REFERENCES [dbo].[bgk_etkinlik]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_butce_bgk_etkinlik'
CREATE INDEX [IX_FK_bgk_butce_bgk_etkinlik]
ON [dbo].[bgk_butce]
    ([EtkinlikID]);
GO

-- Creating foreign key on [FirmaID] in table 'bgk_butce'
ALTER TABLE [dbo].[bgk_butce]
ADD CONSTRAINT [FK_bgk_butce_bgk_firma]
    FOREIGN KEY ([FirmaID])
    REFERENCES [dbo].[bgk_firma]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_butce_bgk_firma'
CREATE INDEX [IX_FK_bgk_butce_bgk_firma]
ON [dbo].[bgk_butce]
    ([FirmaID]);
GO

-- Creating foreign key on [DosyaID] in table 'bgk_dokuman'
ALTER TABLE [dbo].[bgk_dokuman]
ADD CONSTRAINT [FK__bgk_dokum__Dosya__54CB950F]
    FOREIGN KEY ([DosyaID])
    REFERENCES [dbo].[bgk_dosya]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__bgk_dokum__Dosya__54CB950F'
CREATE INDEX [IX_FK__bgk_dokum__Dosya__54CB950F]
ON [dbo].[bgk_dokuman]
    ([DosyaID]);
GO

-- Creating foreign key on [KategoriID] in table 'bgk_dokuman'
ALTER TABLE [dbo].[bgk_dokuman]
ADD CONSTRAINT [FK__bgk_dokum__Kateg__53D770D6]
    FOREIGN KEY ([KategoriID])
    REFERENCES [dbo].[bgk_dokuman_kategori]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__bgk_dokum__Kateg__53D770D6'
CREATE INDEX [IX_FK__bgk_dokum__Kateg__53D770D6]
ON [dbo].[bgk_dokuman]
    ([KategoriID]);
GO

-- Creating foreign key on [UyeID] in table 'bgk_dokuman'
ALTER TABLE [dbo].[bgk_dokuman]
ADD CONSTRAINT [FK__bgk_dokum__UyeID__4B422AD5]
    FOREIGN KEY ([UyeID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__bgk_dokum__UyeID__4B422AD5'
CREATE INDEX [IX_FK__bgk_dokum__UyeID__4B422AD5]
ON [dbo].[bgk_dokuman]
    ([UyeID]);
GO

-- Creating foreign key on [YukleyenID] in table 'bgk_dosya'
ALTER TABLE [dbo].[bgk_dosya]
ADD CONSTRAINT [FK_bgk_dosya_bgk_uye]
    FOREIGN KEY ([YukleyenID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_dosya_bgk_uye'
CREATE INDEX [IX_FK_bgk_dosya_bgk_uye]
ON [dbo].[bgk_dosya]
    ([YukleyenID]);
GO

-- Creating foreign key on [AfisID] in table 'bgk_etkinlik'
ALTER TABLE [dbo].[bgk_etkinlik]
ADD CONSTRAINT [FK_bgk_etkinlik_bgk_dosya]
    FOREIGN KEY ([AfisID])
    REFERENCES [dbo].[bgk_dosya]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_etkinlik_bgk_dosya'
CREATE INDEX [IX_FK_bgk_etkinlik_bgk_dosya]
ON [dbo].[bgk_etkinlik]
    ([AfisID]);
GO

-- Creating foreign key on [ResimID] in table 'bgk_firma'
ALTER TABLE [dbo].[bgk_firma]
ADD CONSTRAINT [FK_bgk_firma_bgk_dosya]
    FOREIGN KEY ([ResimID])
    REFERENCES [dbo].[bgk_dosya]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_firma_bgk_dosya'
CREATE INDEX [IX_FK_bgk_firma_bgk_dosya]
ON [dbo].[bgk_firma]
    ([ResimID]);
GO

-- Creating foreign key on [ResimID] in table 'bgk_galeri_resim'
ALTER TABLE [dbo].[bgk_galeri_resim]
ADD CONSTRAINT [FK_bgk_galeri_resim_bgk_dosya]
    FOREIGN KEY ([ResimID])
    REFERENCES [dbo].[bgk_dosya]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_galeri_resim_bgk_dosya'
CREATE INDEX [IX_FK_bgk_galeri_resim_bgk_dosya]
ON [dbo].[bgk_galeri_resim]
    ([ResimID]);
GO

-- Creating foreign key on [ResimID] in table 'bgk_gorev_kategori'
ALTER TABLE [dbo].[bgk_gorev_kategori]
ADD CONSTRAINT [FK_bgk_gorev_kategori_bgk_dosya]
    FOREIGN KEY ([ResimID])
    REFERENCES [dbo].[bgk_dosya]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_gorev_kategori_bgk_dosya'
CREATE INDEX [IX_FK_bgk_gorev_kategori_bgk_dosya]
ON [dbo].[bgk_gorev_kategori]
    ([ResimID]);
GO

-- Creating foreign key on [DosyaID] in table 'bgk_gorev_uye'
ALTER TABLE [dbo].[bgk_gorev_uye]
ADD CONSTRAINT [FK_bgk_gorev_uye_bgk_dosya]
    FOREIGN KEY ([DosyaID])
    REFERENCES [dbo].[bgk_dosya]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_gorev_uye_bgk_dosya'
CREATE INDEX [IX_FK_bgk_gorev_uye_bgk_dosya]
ON [dbo].[bgk_gorev_uye]
    ([DosyaID]);
GO

-- Creating foreign key on [FirmaID] in table 'bgk_etkinlik'
ALTER TABLE [dbo].[bgk_etkinlik]
ADD CONSTRAINT [FK_bgk_etkinlik_bgk_firma]
    FOREIGN KEY ([FirmaID])
    REFERENCES [dbo].[bgk_firma]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_etkinlik_bgk_firma'
CREATE INDEX [IX_FK_bgk_etkinlik_bgk_firma]
ON [dbo].[bgk_etkinlik]
    ([FirmaID]);
GO

-- Creating foreign key on [EtkinlikID] in table 'bgk_etkinlik_gorevli'
ALTER TABLE [dbo].[bgk_etkinlik_gorevli]
ADD CONSTRAINT [FK_bgk_etkinlik_gorevli_bgk_etkinlik]
    FOREIGN KEY ([EtkinlikID])
    REFERENCES [dbo].[bgk_etkinlik]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_etkinlik_gorevli_bgk_etkinlik'
CREATE INDEX [IX_FK_bgk_etkinlik_gorevli_bgk_etkinlik]
ON [dbo].[bgk_etkinlik_gorevli]
    ([EtkinlikID]);
GO

-- Creating foreign key on [EtkinlikID] in table 'bgk_etkinlik_konusmaci'
ALTER TABLE [dbo].[bgk_etkinlik_konusmaci]
ADD CONSTRAINT [FK_bgk_etkinlik_konusmaci_bgk_etkinlik]
    FOREIGN KEY ([EtkinlikID])
    REFERENCES [dbo].[bgk_etkinlik]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_etkinlik_konusmaci_bgk_etkinlik'
CREATE INDEX [IX_FK_bgk_etkinlik_konusmaci_bgk_etkinlik]
ON [dbo].[bgk_etkinlik_konusmaci]
    ([EtkinlikID]);
GO

-- Creating foreign key on [GorevID] in table 'bgk_etkinlik_gorevli'
ALTER TABLE [dbo].[bgk_etkinlik_gorevli]
ADD CONSTRAINT [FK_bgk_etkinlik_gorevli_bgk_gorev]
    FOREIGN KEY ([GorevID])
    REFERENCES [dbo].[bgk_gorev]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_etkinlik_gorevli_bgk_gorev'
CREATE INDEX [IX_FK_bgk_etkinlik_gorevli_bgk_gorev]
ON [dbo].[bgk_etkinlik_gorevli]
    ([GorevID]);
GO

-- Creating foreign key on [UyeID] in table 'bgk_etkinlik_gorevli'
ALTER TABLE [dbo].[bgk_etkinlik_gorevli]
ADD CONSTRAINT [FK_bgk_etkinlik_gorevli_bgk_uye]
    FOREIGN KEY ([UyeID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_etkinlik_gorevli_bgk_uye'
CREATE INDEX [IX_FK_bgk_etkinlik_gorevli_bgk_uye]
ON [dbo].[bgk_etkinlik_gorevli]
    ([UyeID]);
GO

-- Creating foreign key on [UyeID] in table 'bgk_firma'
ALTER TABLE [dbo].[bgk_firma]
ADD CONSTRAINT [FK_bgk_firma_bgk_uye]
    FOREIGN KEY ([UyeID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_firma_bgk_uye'
CREATE INDEX [IX_FK_bgk_firma_bgk_uye]
ON [dbo].[bgk_firma]
    ([UyeID]);
GO

-- Creating foreign key on [GaleriID] in table 'bgk_galeri_resim'
ALTER TABLE [dbo].[bgk_galeri_resim]
ADD CONSTRAINT [FK_bgk_galeri_resim_bgk_galeri]
    FOREIGN KEY ([GaleriID])
    REFERENCES [dbo].[bgk_galeri]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_galeri_resim_bgk_galeri'
CREATE INDEX [IX_FK_bgk_galeri_resim_bgk_galeri]
ON [dbo].[bgk_galeri_resim]
    ([GaleriID]);
GO

-- Creating foreign key on [KategoriID] in table 'bgk_gorev'
ALTER TABLE [dbo].[bgk_gorev]
ADD CONSTRAINT [FK_bgk_gorev_bgk_gorev_kategori]
    FOREIGN KEY ([KategoriID])
    REFERENCES [dbo].[bgk_gorev_kategori]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_gorev_bgk_gorev_kategori'
CREATE INDEX [IX_FK_bgk_gorev_bgk_gorev_kategori]
ON [dbo].[bgk_gorev]
    ([KategoriID]);
GO

-- Creating foreign key on [GrupID] in table 'bgk_gorev'
ALTER TABLE [dbo].[bgk_gorev]
ADD CONSTRAINT [FK_bgk_gorev_bgk_grup]
    FOREIGN KEY ([GrupID])
    REFERENCES [dbo].[bgk_grup]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_gorev_bgk_grup'
CREATE INDEX [IX_FK_bgk_gorev_bgk_grup]
ON [dbo].[bgk_gorev]
    ([GrupID]);
GO

-- Creating foreign key on [OlusturanID] in table 'bgk_gorev'
ALTER TABLE [dbo].[bgk_gorev]
ADD CONSTRAINT [FK_bgk_gorev_bgk_uye]
    FOREIGN KEY ([OlusturanID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_gorev_bgk_uye'
CREATE INDEX [IX_FK_bgk_gorev_bgk_uye]
ON [dbo].[bgk_gorev]
    ([OlusturanID]);
GO

-- Creating foreign key on [GorevID] in table 'bgk_gorev_uye'
ALTER TABLE [dbo].[bgk_gorev_uye]
ADD CONSTRAINT [FK_bgk_gorev_uye_bgk_gorev]
    FOREIGN KEY ([GorevID])
    REFERENCES [dbo].[bgk_gorev]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_gorev_uye_bgk_gorev'
CREATE INDEX [IX_FK_bgk_gorev_uye_bgk_gorev]
ON [dbo].[bgk_gorev_uye]
    ([GorevID]);
GO

-- Creating foreign key on [OlusturanID] in table 'bgk_gorev_kategori'
ALTER TABLE [dbo].[bgk_gorev_kategori]
ADD CONSTRAINT [FK_bgk_gorev_kategori_bgk_uye]
    FOREIGN KEY ([OlusturanID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_gorev_kategori_bgk_uye'
CREATE INDEX [IX_FK_bgk_gorev_kategori_bgk_uye]
ON [dbo].[bgk_gorev_kategori]
    ([OlusturanID]);
GO

-- Creating foreign key on [KategoriID] in table 'bgk_gorev_kategori_uye'
ALTER TABLE [dbo].[bgk_gorev_kategori_uye]
ADD CONSTRAINT [FK_bgk_gorev_kategori_uye_bgk_gorev_kategori]
    FOREIGN KEY ([KategoriID])
    REFERENCES [dbo].[bgk_gorev_kategori]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_gorev_kategori_uye_bgk_gorev_kategori'
CREATE INDEX [IX_FK_bgk_gorev_kategori_uye_bgk_gorev_kategori]
ON [dbo].[bgk_gorev_kategori_uye]
    ([KategoriID]);
GO

-- Creating foreign key on [UyeID] in table 'bgk_gorev_kategori_uye'
ALTER TABLE [dbo].[bgk_gorev_kategori_uye]
ADD CONSTRAINT [FK_bgk_kategori_uye_bgk_uye]
    FOREIGN KEY ([UyeID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_kategori_uye_bgk_uye'
CREATE INDEX [IX_FK_bgk_kategori_uye_bgk_uye]
ON [dbo].[bgk_gorev_kategori_uye]
    ([UyeID]);
GO

-- Creating foreign key on [UyeID] in table 'bgk_gorev_uye'
ALTER TABLE [dbo].[bgk_gorev_uye]
ADD CONSTRAINT [FK_bgk_gorev_uye_bgk_uye]
    FOREIGN KEY ([UyeID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_gorev_uye_bgk_uye'
CREATE INDEX [IX_FK_bgk_gorev_uye_bgk_uye]
ON [dbo].[bgk_gorev_uye]
    ([UyeID]);
GO

-- Creating foreign key on [GrupID] in table 'bgk_grup_uye'
ALTER TABLE [dbo].[bgk_grup_uye]
ADD CONSTRAINT [FK_bgk_grup_uye_bgk_grup]
    FOREIGN KEY ([GrupID])
    REFERENCES [dbo].[bgk_grup]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_grup_uye_bgk_grup'
CREATE INDEX [IX_FK_bgk_grup_uye_bgk_grup]
ON [dbo].[bgk_grup_uye]
    ([GrupID]);
GO

-- Creating foreign key on [UyeID] in table 'bgk_grup_uye'
ALTER TABLE [dbo].[bgk_grup_uye]
ADD CONSTRAINT [FK_bgk_grup_uye_bgk_uye]
    FOREIGN KEY ([UyeID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_grup_uye_bgk_uye'
CREATE INDEX [IX_FK_bgk_grup_uye_bgk_uye]
ON [dbo].[bgk_grup_uye]
    ([UyeID]);
GO

-- Creating foreign key on [KategoriID] in table 'bgk_yazi'
ALTER TABLE [dbo].[bgk_yazi]
ADD CONSTRAINT [FK_bgk_yazi_bgk_kategori]
    FOREIGN KEY ([KategoriID])
    REFERENCES [dbo].[bgk_kategori]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_yazi_bgk_kategori'
CREATE INDEX [IX_FK_bgk_yazi_bgk_kategori]
ON [dbo].[bgk_yazi]
    ([KategoriID]);
GO

-- Creating foreign key on [UyeID] in table 'bgk_not'
ALTER TABLE [dbo].[bgk_not]
ADD CONSTRAINT [FK__bgk_not__UyeID__3A179ED3]
    FOREIGN KEY ([UyeID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__bgk_not__UyeID__3A179ED3'
CREATE INDEX [IX_FK__bgk_not__UyeID__3A179ED3]
ON [dbo].[bgk_not]
    ([UyeID]);
GO

-- Creating foreign key on [UyeID] in table 'bgk_oylama'
ALTER TABLE [dbo].[bgk_oylama]
ADD CONSTRAINT [FK_bgk_oylama_bgk_uye]
    FOREIGN KEY ([UyeID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_oylama_bgk_uye'
CREATE INDEX [IX_FK_bgk_oylama_bgk_uye]
ON [dbo].[bgk_oylama]
    ([UyeID]);
GO

-- Creating foreign key on [YaziID] in table 'bgk_oylama'
ALTER TABLE [dbo].[bgk_oylama]
ADD CONSTRAINT [FK_bgk_oylama_bgk_yazi]
    FOREIGN KEY ([YaziID])
    REFERENCES [dbo].[bgk_yazi]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_oylama_bgk_yazi'
CREATE INDEX [IX_FK_bgk_oylama_bgk_yazi]
ON [dbo].[bgk_oylama]
    ([YaziID]);
GO

-- Creating foreign key on [UyeID] in table 'bgk_yazi'
ALTER TABLE [dbo].[bgk_yazi]
ADD CONSTRAINT [FK_bgk_yazi_bgk_uye]
    FOREIGN KEY ([UyeID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_yazi_bgk_uye'
CREATE INDEX [IX_FK_bgk_yazi_bgk_uye]
ON [dbo].[bgk_yazi]
    ([UyeID]);
GO

-- Creating foreign key on [UyeID] in table 'bgk_yorum'
ALTER TABLE [dbo].[bgk_yorum]
ADD CONSTRAINT [FK_bgk_yorum_bgk_uye]
    FOREIGN KEY ([UyeID])
    REFERENCES [dbo].[bgk_uye]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_yorum_bgk_uye'
CREATE INDEX [IX_FK_bgk_yorum_bgk_uye]
ON [dbo].[bgk_yorum]
    ([UyeID]);
GO

-- Creating foreign key on [YaziID] in table 'bgk_yorum'
ALTER TABLE [dbo].[bgk_yorum]
ADD CONSTRAINT [FK_bgk_yorum_bgk_yazi]
    FOREIGN KEY ([YaziID])
    REFERENCES [dbo].[bgk_yazi]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_bgk_yorum_bgk_yazi'
CREATE INDEX [IX_FK_bgk_yorum_bgk_yazi]
ON [dbo].[bgk_yorum]
    ([YaziID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------