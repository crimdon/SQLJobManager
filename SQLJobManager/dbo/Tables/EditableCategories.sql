CREATE TABLE [dbo].[EditableCategories] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [CategoryName] NVARCHAR (MAX) NOT NULL,
    [Editable]     BIT            NOT NULL,
    CONSTRAINT [PK_dbo.EditableCategories] PRIMARY KEY CLUSTERED ([Id] ASC)
);

