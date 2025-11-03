-- Procedimientos almacenados para administración del ranking
-- Ejecutar estos en la base de datos CodeQuest

-- Procedimiento para modificar nombre de usuario
CREATE OR ALTER PROCEDURE spUser_UpdateUsername
    @UserID INT,
    @NewUsername VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Verificar que el usuario existe
        IF NOT EXISTS (SELECT 1 FROM Users WHERE UserID = @UserID)
        BEGIN
            RAISERROR(N'El usuario con ID %d no existe.', 16, 1, @UserID);
            RETURN;
        END
        
        -- Verificar que el nuevo nombre no esté en uso
        IF EXISTS (SELECT 1 FROM Users WHERE Username = @NewUsername AND UserID != @UserID)
        BEGIN
            RAISERROR(N'El nombre de usuario "%s" ya está en uso.', 16, 1, @NewUsername);
            RETURN;
        END
        
        -- Actualizar el nombre de usuario
        UPDATE Users 
        SET Username = @NewUsername 
        WHERE UserID = @UserID;
        
        -- Retornar información del usuario actualizado
        SELECT UserID, Username, Xp, Level, CreatedAt 
        FROM Users 
        WHERE UserID = @UserID;
        
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

-- Procedimiento para eliminar usuario y todos sus datos
CREATE OR ALTER PROCEDURE spUser_DeleteComplete
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Verificar que el usuario existe
        IF NOT EXISTS (SELECT 1 FROM Users WHERE UserID = @UserID)
        BEGIN
            RAISERROR(N'El usuario con ID %d no existe.', 16, 1, @UserID);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Obtener información del usuario antes de eliminar
        DECLARE @Username VARCHAR(50);
        SELECT @Username = Username FROM Users WHERE UserID = @UserID;
        
        -- Eliminar respuestas de rondas (RoundAnswers)
        DELETE ra FROM RoundAnswers ra
        INNER JOIN Rounds r ON ra.RoundID = r.RoundID
        WHERE r.UserID = @UserID;
        
        -- Eliminar rondas del usuario
        DELETE FROM Rounds WHERE UserID = @UserID;
        
        -- Eliminar el usuario
        DELETE FROM Users WHERE UserID = @UserID;
        
        COMMIT TRANSACTION;
        
        -- Retornar confirmación
        SELECT 
            @UserID as DeletedUserID, 
            @Username as DeletedUsername,
            'Usuario eliminado exitosamente' as Message;
            
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

-- Procedimiento para resetear XP de un usuario
CREATE OR ALTER PROCEDURE spUser_ResetXP
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        -- Verificar que el usuario existe
        IF NOT EXISTS (SELECT 1 FROM Users WHERE UserID = @UserID)
        BEGIN
            RAISERROR(N'El usuario con ID %d no existe.', 16, 1, @UserID);
            RETURN;
        END
        
        -- Resetear XP y Level
        UPDATE Users 
        SET Xp = 0, Level = 1 
        WHERE UserID = @UserID;
        
        -- Retornar información del usuario actualizado
        SELECT UserID, Username, Xp, Level, CreatedAt 
        FROM Users 
        WHERE UserID = @UserID;
        
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

-- Procedimiento para obtener estadísticas detalladas de un usuario
CREATE OR ALTER PROCEDURE spUser_GetDetailedStats
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        u.UserID,
        u.Username,
        u.Xp,
        u.Level,
        u.CreatedAt,
        COUNT(r.RoundID) as TotalRounds,
        ISNULL(SUM(r.Score), 0) as TotalScore,
        ISNULL(AVG(CAST(r.Score AS FLOAT)), 0) as AverageScore,
        ISNULL(SUM(r.XpEarned), 0) as TotalXpEarned,
        ISNULL(MIN(r.StartedAt), u.CreatedAt) as FirstRound,
        ISNULL(MAX(r.CompletedAt), u.CreatedAt) as LastRound
    FROM Users u
    LEFT JOIN Rounds r ON u.UserID = r.UserID AND r.CompletedAt IS NOT NULL
    WHERE u.UserID = @UserID
    GROUP BY u.UserID, u.Username, u.Xp, u.Level, u.CreatedAt;
END
GO