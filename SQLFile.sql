USE [master]
GO

CREATE DATABASE [Wikimedia]
GO

CREATE TABLE [Wikidump](
	[Period] [datetime] NULL,
	[Lang] [varchar](20) NULL,
	[Domain] [varchar](20) NULL,
	[PageTitle] [varchar](150) NULL,
	[ViewCount] [int] NULL,
	[ResponseSize] [int] NULL
)
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Casapía, Manuel>
-- Create date: <02/14/2021>
-- Description:	<Inserts a new Wikidup record>
-- =============================================
CREATE PROCEDURE USP_InsertWikidump
	-- Add the parameters for the stored procedure here
	@Period datetime,
	@Lang varchar(20),
	@Domain varchar(20),
	@PageTitle varchar(150),
	@ViewCount int,
	@ResponseSize int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO [Wikidump] ([Period], [Lang], [Domain], [PageTitle], [ViewCount], [ResponseSize])
		 VALUES (@Period, @Lang, @Domain, @PageTitle, @ViewCount, @ResponseSize)

END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Casapía, Manuel>
-- Create date: <02/14/2021>
-- Description:	<Gets the top 1 day group by language and domain>
-- =============================================
CREATE PROCEDURE USP_LanDomTopViewPerDay
	-- Add the parameters for the stored procedure here
	@DateFrom datetime,
	@DateTo datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 WITH TIES
		CONVERT(DATE, DATEADD(HH, -1, [Period])) 'Period', [Lang], [Domain], SUM([ViewCount]) 'ViewCount'
	FROM [Wikidump] WITH(NOLOCK)
	WHERE CONVERT(DATE, DATEADD(HH, -1, [Period])) BETWEEN @DateFrom AND @DateTo
	GROUP BY CONVERT(DATE, DATEADD(HH, -1, [Period])), [Lang], [Domain]
	ORDER BY ROW_NUMBER() OVER(PARTITION BY CONVERT(DATE, DATEADD(HH, -1, [Period])) ORDER BY SUM([ViewCount]) DESC);

END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Casapía, Manuel>
-- Create date: <02/14/2021>
-- Description:	<Gets the top 1 per day group by page>
-- =============================================
CREATE PROCEDURE USP_PageTopViewPerDay
	-- Add the parameters for the stored procedure here
	@DateFrom datetime,
	@DateTo datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT  TOP 1 WITH TIES
		CONVERT(DATE, DATEADD(HH, -1, [Period])) 'Period', [PageTitle], SUM([ViewCount]) 'ViewCount'
	FROM [Wikidump] WITH(NOLOCK)
	WHERE CONVERT(DATE, DATEADD(HH, -1, [Period])) BETWEEN @DateFrom AND @DateTo
	GROUP BY CONVERT(DATE, DATEADD(HH, -1, [Period])), [PageTitle]
	ORDER BY ROW_NUMBER() OVER(PARTITION BY CONVERT(DATE, DATEADD(HH, -1, [Period])) ORDER BY SUM([ViewCount]) DESC);

END
GO