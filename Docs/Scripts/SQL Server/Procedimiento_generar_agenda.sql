USE SistemaOdontologico;
GO

CREATE OR ALTER PROCEDURE dbo.sp_GenerarAgendaMedica
    @DiasAdelante INT = 60,
    @IdMedico INT = NULL,              -- NULL = todos
    @IncluirMedicosInactivos BIT = 0   -- 0 = solo médicos activos
AS
BEGIN
    SET NOCOUNT ON;

    -- muy importante: así 1 = lunes y coincide con lo que guardamos
    SET DATEFIRST 1;

    DECLARE @Duracion INT;

    SELECT @Duracion = TRY_CAST(Valor AS INT)
    FROM dbo.ParametroSistema
    WHERE Nombre = 'DuracionTurnoMinutos';

    IF @Duracion IS NULL OR @Duracion <= 0
        SET @Duracion = 60; -- fallback

    DECLARE @FechaDesde DATE = CAST(GETDATE() AS DATE);
    DECLARE @FechaHasta DATE = DATEADD(DAY, @DiasAdelante, @FechaDesde);

    -- traemos los horarios semanales vigentes, cruzados con Medico
    DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
        SELECT H.IdMedico, H.DiaSemana, H.HoraInicio, H.HoraFin
        FROM HorarioSemanalMedico H
        INNER JOIN Medico M ON M.IdMedico = H.IdMedico
        WHERE H.Estado = 'A'
          AND (@IdMedico IS NULL OR H.IdMedico = @IdMedico)
          AND (
                @IncluirMedicosInactivos = 1
                OR M.Estado = 'A'
              );

    DECLARE @C_IdMedico INT;
    DECLARE @C_DiaSemana TINYINT;
    DECLARE @C_HoraInicio TIME;
    DECLARE @C_HoraFin TIME;

    OPEN cur;
    FETCH NEXT FROM cur INTO @C_IdMedico, @C_DiaSemana, @C_HoraInicio, @C_HoraFin;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        DECLARE @F DATE = @FechaDesde;

        WHILE @F <= @FechaHasta
        BEGIN
            -- si el día de la fecha coincide con el día de la semana del horario
            IF DATEPART(WEEKDAY, @F) = @C_DiaSemana
            BEGIN
                DECLARE @SlotInicio TIME = @C_HoraInicio;
                DECLARE @SlotFin TIME;

                WHILE @SlotInicio < @C_HoraFin
                BEGIN
                    SET @SlotFin = DATEADD(MINUTE, @Duracion, @SlotInicio);

                    -- Evitar que un slot corte después del fin del horario
                    IF @SlotFin <= @C_HoraFin
                    BEGIN
                        -- Insertar solo si no existe ya
                        IF NOT EXISTS (
                            SELECT 1
                            FROM AgendaMedico AM
                            WHERE AM.IdMedico = @C_IdMedico
                              AND AM.Fecha = @F
                              AND AM.HoraInicio = @SlotInicio
                        )
                        BEGIN
                            INSERT INTO AgendaMedico
                                (IdMedico, Fecha, HoraInicio, HoraFin, Estado)
                            VALUES
                                (@C_IdMedico, @F, @SlotInicio, @SlotFin, 'L');
                        END
                    END

                    SET @SlotInicio = DATEADD(MINUTE, @Duracion, @SlotInicio);
                END
            END

            SET @F = DATEADD(DAY, 1, @F);
        END

        FETCH NEXT FROM cur INTO @C_IdMedico, @C_DiaSemana, @C_HoraInicio, @C_HoraFin;
    END

    CLOSE cur;
    DEALLOCATE cur;
END;
GO


EXEC sp_GenerarAgendaMedica
    @DiasAdelante = 60,
    @IdMedico = NULL,
    @IncluirMedicosInactivos = 1;


SELECT TOP (1000) [IdAgendaMedico]
      ,[IdMedico]
      ,[Fecha]
      ,[HoraInicio]
      ,[HoraFin]
      ,[Estado]
      ,[IdTurno]
      ,[Observacion]
  FROM [SistemaOdontologico].[dbo].[AgendaMedico]