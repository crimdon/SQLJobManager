CREATE TABLE [dbo].[ActivityLog] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [DateTime]   DATETIME       NOT NULL,
    [UserName]   NVARCHAR (MAX) NULL,
    [ServerName] NVARCHAR (MAX) NULL,
    [JobName]    NVARCHAR (MAX) NULL,
    [Action]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.ActivityLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

