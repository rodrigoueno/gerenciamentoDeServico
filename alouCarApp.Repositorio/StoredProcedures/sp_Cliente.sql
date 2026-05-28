CREATE OR ALTER PROCEDURE sp_Cliente_Read
    @Id    INT  = NULL,
    @Ativo BIT  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Nome, Email, Telefone, Ativo
    FROM   Clientes
    WHERE  (@Id    IS NULL OR Id    = @Id)
      AND  (@Ativo IS NULL OR Ativo = @Ativo);
END
GO