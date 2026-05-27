CREATE OR ALTER PROCEDURE sp_Servico_Create
    @ClienteId  INT,
    @VeiculoId  INT,
    @Descricao  NVARCHAR(500),
    @Ativo      BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Servicos (ClienteId, VeiculoId, Descricao, Ativo)
    VALUES (@ClienteId, @VeiculoId, @Descricao, @Ativo);

    SELECT SCOPE_IDENTITY();
END
GO


CREATE OR ALTER PROCEDURE sp_Servico_Update
    @Id        INT,
    @Descricao NVARCHAR(500),
    @Ativo     BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Servicos
    SET Descricao = @Descricao,
        Ativo     = @Ativo
    WHERE Id = @Id;
END
GO


CREATE OR ALTER PROCEDURE sp_Servico_Read
    @Id    INT = NULL,
    @Ativo BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.Id, s.Descricao, s.Ativo,
        c.Id   AS ClienteId, c.Nome, c.Email, c.Telefone,
        v.Id   AS VeiculoId, v.Placa, v.Modelo,
        i.Id   AS ItemId,    i.Descricao AS ItemDescricao, i.Valor
    FROM      Servicos    s
    JOIN      Clientes    c ON c.Id = s.ClienteId
    JOIN      Veiculos    v ON v.Id = s.VeiculoId
    LEFT JOIN ItemServico i ON i.ServicoId = s.Id
    WHERE  (@Id    IS NULL OR s.Id    = @Id)
      AND  (@Ativo IS NULL OR s.Ativo = @Ativo);
END
GO


CREATE OR ALTER PROCEDURE sp_Servico_ReadByCliente
    @ClienteId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.Id, s.Descricao, s.Ativo,
        v.Id AS VeiculoId, v.Placa, v.Modelo,
        i.Id AS ItemId,    i.Descricao AS ItemDescricao, i.Valor
    FROM      Servicos    s
    JOIN      Veiculos    v ON v.Id = s.VeiculoId
    LEFT JOIN ItemServico i ON i.ServicoId = s.Id
    WHERE  s.ClienteId = @ClienteId
      AND  s.Ativo     = 1;
END
GO


CREATE OR ALTER PROCEDURE sp_Servico_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Servicos
    SET Ativo = 0
    WHERE Id = @Id;
END
GO