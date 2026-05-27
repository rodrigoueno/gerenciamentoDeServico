CREATE OR ALTER PROCEDURE sp_Veiculo_Create
    @Placa     NVARCHAR(10),
    @Modelo    NVARCHAR(50),
    @ClienteId INT,
    @Ativo     BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Veiculos (Placa, Modelo, ClienteId, Ativo)
    VALUES (@Placa, @Modelo, @ClienteId, @Ativo);

    SELECT SCOPE_IDENTITY();
END
GO


CREATE OR ALTER PROCEDURE sp_Veiculo_Update
    @Id     INT,
    @Placa  NVARCHAR(10),
    @Modelo NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Veiculos
    SET Placa  = @Placa,
        Modelo = @Modelo
    WHERE Id = @Id;
END
GO


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


CREATE OR ALTER PROCEDURE sp_Veiculo_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Veiculos
    SET Ativo = 0
    WHERE Id = @Id;
END
GO