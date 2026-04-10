CREATE TABLE [dbo].[OG_Verification_Approval](
    [Id] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [OGVerifyNo] [int] NOT NULL,
    [TotalWeight] [decimal](18, 2) NULL,
    [TotalValue] [decimal](18, 2) NULL,
    [CurrentValueToBeReduced] [decimal](18, 2) NULL,
    [ApprovedUser] [nvarchar](15) NULL,
    [ApprovedDate] [datetime] NOT NULL
);
