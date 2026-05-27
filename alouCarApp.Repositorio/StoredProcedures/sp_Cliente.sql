CREATE OR ALTER PROCEDURE sp_Cliente_Create
    @Nome       NVARCHAR(50),
    @Email      NVARCHAR(50),
    @Telefone   NVARCHAR(15),
    @Ativo      BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Clientes (Nome, Email, Telefone, Ativo)
    VALUES (@Nome, @Email, @Telefone, @Ativo);

    SELECT SCOPE_IDENTITY();
END
GO


CREATE OR ALTER PROCEDURE sp_Cliente_Update
    @Id         INT,
    @Nome       NVARCHAR(50),
    @Email      NVARCHAR(50),
    @Telefone   NVARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Clientes
    SET Nome     = @Nome,
        Email    = @Email,
        Telefone = @Telefone
    WHERE Id = @Id;
END
GO


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


CREATE OR ALTER PROCEDURE sp_Cliente_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Clientes
    SET Ativo = 0
    WHERE Id = @Id;
END
GO