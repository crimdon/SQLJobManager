CREATE TABLE [dbo].[ServerConfig] (
    [Id]                 INT            IDENTITY (1, 1) NOT NULL,
    [ServerName]         NVARCHAR (MAX) NOT NULL,
    [AuthenticationType] INT            NOT NULL,
    [UserName]           NVARCHAR (MAX) NOT NULL,
    [Password]           NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_dbo.ServerConfig] PRIMARY KEY CLUSTERED ([Id] ASC)
);

