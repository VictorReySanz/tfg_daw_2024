CREATE TABLE [dbo].[CV] (
    [cv_id]           INT           NOT NULL,
    [nombre_completo] VARCHAR (100) NULL,
    [visible]         BIT           NULL,
    [categoria_id]    INT           NULL,
    PRIMARY KEY CLUSTERED ([cv_id] ASC),
    FOREIGN KEY ([categoria_id]) REFERENCES [dbo].[Categorias] ([categoria_id])
);

