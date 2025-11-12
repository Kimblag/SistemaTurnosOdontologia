USE SistemaOdontologico;
GO

CREATE OR ALTER PROCEDURE sp_Actualizar_Agenda_Cambio_Horario
    @IdMedico INT,
    @Desde DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET DATEFIRST 1;

    IF @Desde IS NULL
        SET @Desde = CAST(GETDATE() AS DATE);

    -- marcar turnos pendientes de reprogramar
    UPDATE T
    SET T.Estado = 'P'
    FROM Turno T
    WHERE T.Estado IN ('N','R')
      AND T.IdTurno IN (
            SELECT A.IdTurno
            FROM AgendaMedico A
            WHERE A.IdMedico = @IdMedico
              AND A.Fecha >= @Desde
              AND NOT EXISTS (
                    SELECT 1
                    FROM HorarioSemanalMedico H
                    WHERE H.IdMedico = A.IdMedico
                      AND H.Estado = 'A'
                      AND H.DiaSemana = DATEPART(WEEKDAY, A.Fecha)
                      AND A.HoraInicio >= H.HoraInicio
                      AND A.HoraFin <= H.HoraFin
                )
      );

    -- luego inactivar slots libres fuera de horario
    UPDATE A
    SET A.Estado = 'I',
        A.Observacion = 
            CASE 
                WHEN A.Observacion IS NULL OR LTRIM(RTRIM(A.Observacion)) = ''
                    THEN 'Inactivado por cambio de horario del médico'
                ELSE A.Observacion + ' | Inactivado por cambio de horario del médico'
            END
    FROM AgendaMedico A
    WHERE A.IdMedico = @IdMedico
      AND A.Fecha >= @Desde
      AND A.Estado = 'L'
      AND NOT EXISTS (
            SELECT 1
            FROM HorarioSemanalMedico H
            WHERE H.IdMedico = A.IdMedico
              AND H.Estado = 'A'
              AND H.DiaSemana = DATEPART(WEEKDAY, A.Fecha)
              AND A.HoraInicio >= H.HoraInicio
              AND A.HoraFin <= H.HoraFin
        );

    -- se deben marcar los slots ocupados fuera de horario
    UPDATE A
    SET A.Observacion =
            CASE 
                WHEN A.Observacion IS NULL OR LTRIM(RTRIM(A.Observacion)) = ''
                    THEN 'Fuera de horario vigente: requiere reprogramación'
                ELSE A.Observacion + ' | Fuera de horario vigente: requiere reprogramación'
            END
    FROM AgendaMedico A
    WHERE A.IdMedico = @IdMedico
      AND A.Fecha >= @Desde
      AND A.Estado = 'O'
      AND NOT EXISTS (
            SELECT 1
            FROM HorarioSemanalMedico H
            WHERE H.IdMedico = A.IdMedico
              AND H.Estado = 'A'
              AND H.DiaSemana = DATEPART(WEEKDAY, A.Fecha)
              AND A.HoraInicio >= H.HoraInicio
              AND A.HoraFin <= H.HoraFin
        );
END;
GO
