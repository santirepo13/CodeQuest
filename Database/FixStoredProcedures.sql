-- Arreglar procedimientos almacenados para devolver INT explícitamente

-- Crear Usuario
CREATE OR ALTER PROCEDURE spUser_New
@username varchar(50)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Users(Username) VALUES (@username);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS UserID;
END
GO

-- Crear ronda para usuario
CREATE OR ALTER PROCEDURE spRounds_New
@UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Rounds (UserID) VALUES (@UserID);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS RoundID;
END
GO

-- Crear opción
CREATE OR ALTER PROCEDURE spOption_new
@QuestionID INT,
@ChoiceText VARCHAR(500),
@IsCorrect BIT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Choices (QuestionID, ChoiceText, IsCorrect)
    VALUES (@QuestionID, @ChoiceText, @IsCorrect);
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS ChoiceID;
END
GO

-- Registrar respuesta a una pregunta de una ronda
CREATE OR ALTER PROCEDURE spRounds_Answer
@RoundID INT,
@QuestionID INT,
@ChoiceID INT,
@TimeSpentSec INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM Choices WHERE ChoiceID=@ChoiceID AND QuestionID=@QuestionID)
    BEGIN
        RAISERROR(N'La opción no pertenece a la pregunta indicada.', 16, 1);
        RETURN;
    END;
    INSERT INTO RoundAnswers (RoundID, QuestionID, ChoiceID, IsCorrect, TimeSpentSec)
    SELECT @RoundID, @QuestionID, @ChoiceID, c.IsCorrect, @TimeSpentSec
    FROM Choices c
    WHERE c.ChoiceID = @ChoiceID;
    SELECT CAST(SCOPE_IDENTITY() AS INT) AS RoundAnswerID;
END
GO