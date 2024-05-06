CREATE TABLE [dbo].[Libros] (
    [libro_id]     INT           NOT NULL,
    [titulo]       VARCHAR (100) NULL,
    [autor]        VARCHAR (100) NULL,
    [descripcion]  TEXT          NULL,
    [visible]      BIT           NULL,
    [categoria_id] INT           NULL,
    [usuario_id]   INT           NULL,
    [genero] NCHAR(20) NULL, 
    PRIMARY KEY CLUSTERED ([libro_id] ASC),
    FOREIGN KEY ([categoria_id]) REFERENCES [dbo].[Categorias] ([categoria_id]),
    FOREIGN KEY ([usuario_id]) REFERENCES [dbo].[Usuarios] ([usuario_id])
);

