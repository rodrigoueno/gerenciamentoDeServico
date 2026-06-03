CREATE OR ALTER PROCEDURE sp_Servico_Read
    @Id    INT = NULL,
    @Ativo BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.Id,
        s.ClienteId,
        s.VeiculoId,
        s.DataAgendamento,
        s.Situacao,
        s.ValorPrevisto,
        s.ValorTotal,
        s.Observacao,
        s.Ativo,
        s.DataCriacao,
        s.DataConclusao,

        c.Id        AS ClienteId,
        c.Nome,
        c.Email,
        c.Telefone,

        v.Id        AS VeiculoId,
        v.Placa,
        v.Modelo,

        i.Id        AS ItemId,
        i.TipoServico,
        i.Valor,
        i.KilometragemNaRevisao

    FROM      Servicos      s
    JOIN      Clientes      c ON c.Id = s.ClienteId
    JOIN      Veiculos      v ON v.Id = s.VeiculoId
    LEFT JOIN ServicoItens  i ON i.ServicoId = s.Id
    W


CREATE OR ALTER PROCEDURE sp_Servico_ReadByCliente
    @ClienteId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.Id,
        s.ClienteId,
        s.VeiculoId,
        s.DataAgendamento,
        s.Situacao,
        s.ValorPrevisto,
        s.ValorTotal,
        s.Observacao,
        s.Ativo,
        s.DataCriacao,
        s.DataConclusao,

        v.Id        AS VeiculoId,
        v.Placa,
        v.Modelo,

        i.Id        AS ItemId,
        i.TipoServico,
        i.Valor,
        i.KilometragemNaRevisao

    FROM      Servicos    s
    JOIN      Veiculos    v ON v.Id = s.VeiculoId
    LEFT JOIN ServicoItens i ON i.ServicoId = s.Id
    WHERE  s.ClienteId = @ClienteId
      AND  s.Ativo     = 1;
END
GO