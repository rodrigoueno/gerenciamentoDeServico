CREATE OR ALTER PROCEDURE sp_Veiculo_Read
    @Id    INT = NULL,
    @Ativo BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Placa, Modelo, ClienteId, Ativo
    FROM   Veiculos
    WHERE  (@Id    IS NULL OR Id    = @Id)
      AND  (@Ativo IS NULL OR Ativo = @Ativo);
END
GO


CREATE OR ALTER PROCEDURE sp_Veiculo_ReadByCliente
    @ClienteId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Placa, Modelo, ClienteId, Ativo
    FROM   Veiculos
    WHERE  ClienteId = @ClienteId
      AND  Ativo     = 1;
END
GO