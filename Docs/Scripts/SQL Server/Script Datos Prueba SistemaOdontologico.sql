USE SistemaOdontologico;
GO

-- Cobertura
INSERT INTO Cobertura (Nombre, Descripcion, Estado) VALUES
('Particular', 'Pacientes sin cobertura médica', 'A'),
('OSDE', 'Cobertura médica privada', 'A'),
('Swiss Medical', 'Cobertura premium odontológica', 'A'),
('Galeno', 'Cobertura familiar con amplia cartilla', 'I'),
('Medicus', 'Plan integral de salud dental', 'A'),
('Federada Salud', 'Cobertura con atención odontológica propia', 'A'),
('Omint', 'Cobertura integral con odontología preventiva', 'A'),
('Sancor Salud', 'Planes odontológicos familiares', 'I'),
('IOMA', 'Cobertura estatal para empleados públicos', 'A'),
('OSPE', 'Cobertura de estaciones de servicio', 'A');

-- Rol
INSERT INTO Rol (Nombre, Descripcion, Estado) VALUES
('Administrador', 'Acceso total al sistema', 'A'),
('Recepcionista', 'Gestión de pacientes y turnos', 'A'),
('Médico', 'Acceso a agenda y registro clínico', 'A');

-- Permiso
INSERT INTO Permiso (Modulo, Accion, Descripcion) VALUES
('Inicio','Ver','Ver dashboard del sistema'),
('Turnos','Ver','Ver listado de turnos'),
('Turnos','Crear','Registrar nuevos turnos'),
('Turnos','Editar','Editar turnos existentes'),
('Turnos','Eliminar','Eliminar turnos'),
('Turnos','Activar','Reactivar turnos'),
('Turnos','Desactivar','Cancelar/Desactivar turnos'),
('Pacientes','Ver','Ver pacientes'),
('Pacientes','Crear','Crear paciente'),
('Pacientes','Editar','Editar paciente'),
('Pacientes','Eliminar','Eliminar paciente'),
('Pacientes','Activar','Activar paciente'),
('Pacientes','Desactivar','Desactivar paciente'),
('Medicos','Ver','Ver médicos'),
('Medicos','Crear','Crear médico'),
('Medicos','Editar','Editar médico'),
('Medicos','Eliminar','Eliminar médico'),
('Medicos','Activar','Activar médico'),
('Medicos','Desactivar','Desactivar médico'),
('Coberturas','Ver','Ver coberturas'),
('Coberturas','Crear','Crear cobertura'),
('Coberturas','Editar','Editar cobertura'),
('Coberturas','Eliminar','Eliminar cobertura'),
('Coberturas','Activar','Activar cobertura'),
('Coberturas','Desactivar','Desactivar cobertura'),
('Planes','Ver','Ver planes'),
('Planes','Crear','Crear plan'),
('Planes','Editar','Editar plan'),
('Planes','Eliminar','Eliminar plan'),
('Planes','Activar','Activar plan'),
('Planes','Desactivar','Desactivar plan'),
('Especialidades','Ver','Ver especialidades'),
('Especialidades','Crear','Crear especialidad'),
('Especialidades','Editar','Editar especialidad'),
('Especialidades','Eliminar','Eliminar especialidad'),
('Especialidades','Activar','Activar especialidad'),
('Especialidades','Desactivar','Desactivar especialidad'),
('Tratamientos','Ver','Ver tratamientos'),
('Tratamientos','Crear','Crear tratamiento'),
('Tratamientos','Editar','Editar tratamiento'),
('Tratamientos','Eliminar','Eliminar tratamiento'),
('Tratamientos','Activar','Activar tratamiento'),
('Tratamientos','Desactivar','Desactivar tratamiento'),
('Reportes','Ver','Ver reportes'),
('Reportes','Crear','Crear reporte'),
('Reportes','Editar','Editar reporte'),
('Reportes','Eliminar','Eliminar reporte'),
('Reportes','Activar','Activar reporte'),
('Reportes','Desactivar','Desactivar reporte'),
('Usuarios','Ver','Ver usuarios'),
('Usuarios','Crear','Crear usuario'),
('Usuarios','Editar','Editar usuario'),
('Usuarios','Eliminar','Eliminar usuario'),
('Usuarios','Activar','Activar usuario'),
('Usuarios','Desactivar','Desactivar usuario'),
('Roles','Ver','Ver roles'),
('Roles','Crear','Crear rol'),
('Roles','Editar','Editar rol'),
('Roles','Eliminar','Eliminar rol'),
('Configuracion','Ver','Ver configuración'),
('Configuracion','Editar','Editar configuración'),
('ParametroSistema','Ver','Ver parámetros'),
('ParametroSistema','Editar','Editar parámetros'),
('Atencion','Ver','Ver página de atención del paciente');

-- Especialidad
INSERT INTO Especialidad (Nombre, Descripcion, Estado) VALUES
('Ortodoncia', 'Corrección dental', 'A'),
('Endodoncia', 'Tratamiento de conductos', 'A'),
('Periodoncia', 'Tratamiento de encías', 'I'),
('Odontopediatría', 'Atención infantil', 'A'),
('Cirugía Bucal', 'Extracciones complejas', 'A'),
('Implantología', 'Implantes dentales', 'A'),
('Prótesis', 'Rehabilitación dental', 'A'),
('Diagnóstico', 'Radiología', 'I'),
('Estética Dental', 'Tratamientos estéticos', 'A'),
('General', 'Odontología general', 'A');

-- ParametroSistema
INSERT INTO ParametroSistema (Nombre, Valor, Descripcion) VALUES
('DuracionTurnoMinutos', '60', 'Duración predeterminada de turno'),
('HoraInicioJornada', '08:00', 'Inicio jornada'),
('HoraFinJornada', '18:00', 'Fin jornada'),
('SMTP_Server', 'smtp.gmail.com', 'Servidor de correo'),
('SMTP_Port', '587', 'Puerto SMTP'),
('Email_From', 'tpweb.grupo9c.promoweb2025@gmail.com', 'Correo remitente'),
('NombreClinica', 'Clínica SGTO', 'Nombre visible del sistema'),
('UsuarioCorreo', 'tpweb.grupo9c.promoweb2025@gmail.com', 'Nombre usuario SMTP del sistema'),
('ReintentosEmail', '3', 'Cantidad de intentos para reenviar un email');

-- Plan
INSERT INTO [Plan] (Nombre, Descripcion, PorcentajeCobertura, IdCobertura, Estado) VALUES
('Particular', 'Pago total', 0, 1, 'A'),
('OSDE 210', 'Cobertura parcial', 70, 2, 'A'),
('OSDE 310', 'Cobertura alta', 90, 2, 'A'),
('SM30', 'Plan básico Swiss Medical', 80, 3, 'A'),
('SM50', 'Plan avanzado Swiss Medical', 90, 3, 'A'),
('Galeno G100', 'Plan familiar', 75, 4, 'I'),
('Medicus Plus', 'Integral Medicus', 85, 5, 'A'),
('FS20', 'Federada individual', 60, 6, 'A'),
('Omint O30', 'Preventivo', 70, 7, 'A'),
('Sancor S40', 'Básico Sancor', 65, 8, 'I');

-- RolPermiso
-- Administrador: todos los permisos
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT 1, IdPermiso FROM Permiso;

-- Recepcionista: TODO sobre Turnos y Pacientes
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT 2, IdPermiso FROM Permiso
WHERE Modulo IN ('Turnos','Pacientes', 'Medicos');

GO
-- Recepcionista: Sólo ver
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT 2, IdPermiso
FROM Permiso
WHERE
    (Modulo = 'Coberturas' AND Accion IN ('Ver'))
    OR
    (Modulo = 'Planes' AND Accion = 'Ver')
    OR
    (Modulo = 'Especialidades' AND Accion = 'Ver')
    OR
    (Modulo = 'Tratamientos' AND Accion = 'Ver');

GO

-- Médico: Turnos(Ver/Editar) + Pacientes(Ver) + Tratamientos(Ver) + Especialidades(Ver)
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT 3, IdPermiso
FROM Permiso
WHERE
    (Modulo = 'Turnos' AND Accion IN ('Ver'))
    OR
    (Modulo = 'Pacientes' AND Accion = 'Ver')
    OR
    (Modulo = 'Atencion' AND Accion = 'Ver');

-- Tratamiento
INSERT INTO Tratamiento (Nombre, Descripcion, CostoBase, IdEspecialidad, Estado) VALUES
('Limpieza Dental', 'Profilaxis', 5000, 10, 'A'),
('Extracción Simple', 'Extracción sin cirugía', 7000, 5, 'A'),
('Endodoncia Unirradicular', 'Trat. conducto 1 raíz', 15000, 2, 'A'),
('Colocación de Brackets', 'Ortodoncia metálica', 35000, 1, 'A'),
('Implante Unitario', 'Implante con corona', 80000, 6, 'I'),
('Blanqueamiento', 'Estético dental', 12000, 9, 'A'),
('Prótesis Removible', 'Rehabilitación', 30000, 7, 'A'),
('Radiografía Panorámica', 'Estudio diagnóstico', 4000, 8, 'A'),
('Selladores', 'Prevención caries', 6000, 4, 'A'),
('Control General', 'Consulta básica', 3500, 10, 'A'),
('Raspaje y Alisado', 'Limpieza profunda de encías por cuadrante', 18000, 3, 'A'),
('Mantenimiento Periodontal', 'Control y limpieza semestral', 12000, 3, 'A'),
('Implante de Titanio', 'Colocación de implante estándar', 95000, 6, 'A'),
('Corona sobre Implante', 'Fase final de rehabilitación', 60000, 6, 'A'),
('Tomografía Cone Beam', 'Estudio 3D para implantes', 25000, 8, 'A');

-- Usuario
INSERT INTO Usuario (Nombre, Apellido, Email, NombreUsuario, PasswordHash, IdRol, Estado, FechaAlta)
VALUES (
    'Super', 
    'Usuario', 
    'root@sistema.com', 
    'root', 
    'ihYD5DAPxK1tnFBpV8qIdT395LegNh3Uxd3v+oOE6xB0iKNN3CG0OwzZdvyNUq6x', 
    (SELECT Top 1 IdRol FROM Rol WHERE Nombre = 'Administrador'), 
    'A', 
    GETDATE()
);

GO

INSERT INTO Usuario (Nombre, Apellido, Email, NombreUsuario, PasswordHash, IdRol, Estado)
VALUES
('Ana','García','ana.garcia@sgto.com','agarcia','hash1',1,'A'),
('Esteban','Fernández','esteban.fernandez@sgto.com','efernandez','hash2',1,'A'),
('Luis','Pérez','luis.perez@sgto.com','lperez','7b2XJvi68rgst3PU1cDtstsE5FTWfbV1G3h6Vtyd1ZOH2j5tfsoQKIWy8PgZFh7G',2,'A'),
('Paula','Mendoza','paula.mendoza@sgto.com','pmendoza','hash4',2,'I'),
('Sofía','López','sofia.lopez@sgto.com','slopez','MJo60JzWVcscL09tb/kPwrURprGtAFi/syvMf2wE5O2TshiezY/7Ns22GWQxGsPi',3,'A'),
('Martín','Ruiz','martin.ruiz@sgto.com','mruiz','hash6',3,'A'),
('Nicolás','Benítez','nicolas.benitez@sgto.com','nbenitez','mIq1I4frcDeymJq8kAiX+tP1WqnW5jEidaecycFtsMQ3dG6vmMrsUdBHMzWYrufN',3,'A'),
('Lucía','Romero','lucia.romero@sgto.com','lromero','hash8',3,'I'),
('Camila','Rossi','camila.rossi@sgto.com','crossi','hash9',3,'A'),
('Carlos','Méndez','carlos.mendez@sgto.com','cmendez','hash10',3,'A');

-- Medico
INSERT INTO Medico
(Nombre, Apellido, NumeroDocumento, Genero, FechaNacimiento, Telefono, Matricula, IdUsuario, Estado)
VALUES
('Sofía','López','31234568','F','1988-10-20','1123456790','121235',6,'A'),
('Martín','Ruiz','30234567','M','1985-04-15','1123456789','121234',7,'A'),
('Nicolás','Benítez','29234569','M','1984-02-28','1123456791','121236',8,'A'),
('Lucía','Romero','28234570','F','1990-07-18','1123456792','121237',9,'I'),
('Camila','Rossi','32234571','F','1991-05-12','1123456793','121238',10,'A'),
('Carlos','Méndez','27234572','M','1982-11-02','1123456794','121239',11,'A');

GO

-- MedicoEspecialidad
-- Médico 1: Sofía López - Estética Dental + General
INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES
(1, 9),  -- Estética Dental
(1, 10); -- Odontología General

-- Médico 2: Martín Ruiz - Ortodoncia + Endodoncia
INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES
(2, 1),  -- Ortodoncia
(2, 2);  -- Endodoncia

-- Médico 3: Nicolás Benítez - Endodoncia + Cirugía Bucal
INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES
(3, 2),  -- Endodoncia
(3, 5);  -- Cirugía Bucal

-- Médico 4: Lucía Romero - Cirugía Bucal (inactiva)
INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES
(4, 5);  -- Cirugía Bucal

-- Médico 5: Camila Rossi - Implantología + Estética Dental
INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES
(5, 6),  -- Implantología
(5, 9);  -- Estética Dental

-- Médico 6: Carlos Méndez - General + Prótesis
INSERT INTO MedicoEspecialidad (IdMedico, IdEspecialidad) VALUES
(6, 10), -- Odontología General
(6, 7);  -- Prótesis (AGREGADO para que coincida con los turnos)

GO

-- Paciente
INSERT INTO Paciente
(Nombre, Apellido, NumeroDocumento, Genero, FechaNacimiento, Telefono, Email, IdCobertura, IdPlan, Estado)
VALUES
('Andrés','Suárez','40111222','M','1992-05-15','1150012233','andres.suarez@correo.com',1,NULL,'A'),       
('Belén','Gómez','42111223','F','1994-09-20','1150012234','belen.gomez@correo.com',2,2,'A'),               
('Carlos','Vega','43111224','M','1987-03-11','1150012235','carlos.vega@correo.com',3,4,'A'),               
('Diana','Pérez','44111225','F','1990-08-02','1150012236','diana.perez@correo.com',4,6,'I'),              
('Elena','Rodríguez','45111226','F','1989-10-12','1150012237','elena.rodriguez@correo.com',5,7,'A'),      
('Francisco','Luna','46111227','M','1985-11-22','1150012238','francisco.luna@correo.com',6,8,'A'),         
('Gabriela','Fernández','47111228','F','1991-06-13','1150012239','gabriela.fernandez@correo.com',7,9,'A'),
('Hernán','Molina','48111229','M','1983-12-30','1150012240','hernan.molina@correo.com',8,10,'I'),          
('Isabel','Núñez','49111230','F','1996-04-21','1150012241','isabel.nunez@correo.com',9,8,'A'),           
('Jorge','Santos','50111231','M','1980-01-05','1150012242','jorge.santos@correo.com',10,NULL,'I');

-- HorarioSemanalMedico
INSERT INTO HorarioSemanalMedico (IdMedico, DiaSemana, HoraInicio, HoraFin, Estado)
VALUES
    (1, 1, '08:00', '12:00', 'A'), -- Lunes
    (1, 3, '14:00', '18:00', 'A'), -- Miércoles
    (2, 2, '09:00', '13:00', 'A'), -- Martes
    (3, 4, '10:00', '14:00', 'A'), -- Jueves
    (4, 5, '08:00', '12:00', 'I'), -- Viernes (inactivo)
    (5, 6, '09:00', '13:00', 'A'), -- Sábado
    (6, 1, '13:00', '17:00', 'A'); -- Lunes
GO

-- TURNOS CON FECHAS DINÁMICAS Y COHERENTES PARA QUE NO DE ERROR AL PROBAR TURNOS
-- Se calculan fechas desde HOY en adelante, respetando:
-- - Día de la semana del horario del médico
-- - Especialidades correctas del médico
-- - Estados coherentes (N, R, C, X, Z)

DECLARE @Hoy DATETIME = CAST(GETDATE() AS DATE);

-- Turno 1: Paciente 1, Médico 1, Lunes
INSERT INTO Turno (IdPaciente, IdMedico, IdEspecialidad, IdCobertura, IdPlan,
    FechaInicio, FechaFin, Estado, Observaciones)
VALUES (
    1, 1, 10, 1, NULL,
    DATEADD(DAY, (1 - CASE DATEPART(WEEKDAY, @Hoy) WHEN 1 THEN 7 ELSE DATEPART(WEEKDAY, @Hoy)-1 END + 7) % 7, @Hoy) + CAST('09:00' AS DATETIME),
    DATEADD(DAY, (1 - CASE DATEPART(WEEKDAY, @Hoy) WHEN 1 THEN 7 ELSE DATEPART(WEEKDAY, @Hoy)-1 END + 7) % 7, @Hoy) + CAST('10:00' AS DATETIME),
    'N', 'Control general'
);

-- Turno 2: Paciente 2, Médico 2, Martes +1 semana
INSERT INTO Turno (IdPaciente, IdMedico, IdEspecialidad, IdCobertura, IdPlan,
    FechaInicio, FechaFin, Estado, Observaciones)
VALUES (
    2, 2, 1, 2, 2,
    DATEADD(DAY, (2 - CASE DATEPART(WEEKDAY, @Hoy) WHEN 1 THEN 7 ELSE DATEPART(WEEKDAY, @Hoy)-1 END + 7) % 7 + 7, @Hoy) + CAST('10:00' AS DATETIME),
    DATEADD(DAY, (2 - CASE DATEPART(WEEKDAY, @Hoy) WHEN 1 THEN 7 ELSE DATEPART(WEEKDAY, @Hoy)-1 END + 7) % 7 + 7, @Hoy) + CAST('11:00' AS DATETIME),
    'R', 'Reprogramado por médico'
);

-- Turno 3: Paciente 3, Médico 3, Jueves pasado (hace 7 días)
INSERT INTO Turno (IdPaciente, IdMedico, IdEspecialidad, IdCobertura, IdPlan,
    FechaInicio, FechaFin, Estado, Observaciones)
VALUES (
    3, 3, 2, 3, 4,
    DATEADD(DAY, -7, @Hoy) + CAST('11:00' AS DATETIME),
    DATEADD(DAY, -7, @Hoy) + CAST('12:00' AS DATETIME),
    'C', 'Cancelado por paciente'
);

-- Turno 4: Paciente 4, Médico 4, Viernes pasado
INSERT INTO Turno (IdPaciente, IdMedico, IdEspecialidad, IdCobertura, IdPlan,
    FechaInicio, FechaFin, Estado, Observaciones)
VALUES (
    4, 4, 5, 4, 6,
    DATEADD(DAY, -5, @Hoy) + CAST('09:00' AS DATETIME),
    DATEADD(DAY, -5, @Hoy) + CAST('10:00' AS DATETIME),
    'X', 'No asistió'
);

-- Turno 5: Paciente 5, Médico 5, Sábado +1 semana
INSERT INTO Turno (IdPaciente, IdMedico, IdEspecialidad, IdCobertura, IdPlan,
    FechaInicio, FechaFin, Estado, Observaciones)
VALUES (
    5, 5, 6, 5, 7,
    DATEADD(DAY, (6 - CASE DATEPART(WEEKDAY, @Hoy) WHEN 1 THEN 7 ELSE DATEPART(WEEKDAY, @Hoy)-1 END + 7) % 7 + 7, @Hoy) + CAST('09:00' AS DATETIME),
    DATEADD(DAY, (6 - CASE DATEPART(WEEKDAY, @Hoy) WHEN 1 THEN 7 ELSE DATEPART(WEEKDAY, @Hoy)-1 END + 7) % 7 + 7, @Hoy) + CAST('10:00' AS DATETIME),
    'N', 'Implante programado'
);

-- Turno 6: Paciente 6, Médico 6, Lunes hace 2 semanas
INSERT INTO Turno (IdPaciente, IdMedico, IdEspecialidad, IdCobertura, IdPlan,
    FechaInicio, FechaFin, Estado, Observaciones)
VALUES (
    6, 6, 7, 6, 8,
    DATEADD(DAY, -14, @Hoy) + CAST('14:00' AS DATETIME),
    DATEADD(DAY, -14, @Hoy) + CAST('15:00' AS DATETIME),
    'Z', 'Prótesis realizada'
);

-- Turno 7: Paciente 7, Médico 1, Miércoles esta semana
INSERT INTO Turno (IdPaciente, IdMedico, IdEspecialidad, IdCobertura, IdPlan,
    FechaInicio, FechaFin, Estado, Observaciones)
VALUES (
    7, 1, 9, 7, 9,
    DATEADD(DAY, (3 - CASE DATEPART(WEEKDAY, @Hoy) WHEN 1 THEN 7 ELSE DATEPART(WEEKDAY, @Hoy)-1 END + 7) % 7, @Hoy) + CAST('14:00' AS DATETIME),
    DATEADD(DAY, (3 - CASE DATEPART(WEEKDAY, @Hoy) WHEN 1 THEN 7 ELSE DATEPART(WEEKDAY, @Hoy)-1 END + 7) % 7, @Hoy) + CAST('15:00' AS DATETIME),
    'C', 'Cancelado por clima'
);

-- Turno 8: Paciente 8, Médico 2, Martes pasado
INSERT INTO Turno (IdPaciente, IdMedico, IdEspecialidad, IdCobertura, IdPlan,
    FechaInicio, FechaFin, Estado, Observaciones)
VALUES (
    8, 2, 2, 8, 10,
    DATEADD(DAY, -10, @Hoy) + CAST('09:00' AS DATETIME),
    DATEADD(DAY, -10, @Hoy) + CAST('10:00' AS DATETIME),
    'Z', 'Endodoncia realizada'
);

-- Turno 9: Paciente 9, Médico 5, Sábado +2 semanas
INSERT INTO Turno (IdPaciente, IdMedico, IdEspecialidad, IdCobertura, IdPlan,
    FechaInicio, FechaFin, Estado, Observaciones)
VALUES (
    9, 5, 9, 9, 8,
    DATEADD(DAY, (6 - CASE DATEPART(WEEKDAY, @Hoy) WHEN 1 THEN 7 ELSE DATEPART(WEEKDAY, @Hoy)-1 END + 7) % 7 + 14, @Hoy) + CAST('11:00' AS DATETIME),
    DATEADD(DAY, (6 - CASE DATEPART(WEEKDAY, @Hoy) WHEN 1 THEN 7 ELSE DATEPART(WEEKDAY, @Hoy)-1 END + 7) % 7 + 14, @Hoy) + CAST('12:00' AS DATETIME),
    'N', 'Blanqueamiento'
);

-- Turno 10: Paciente 10, Médico 6, Lunes +1 semana
INSERT INTO Turno (IdPaciente, IdMedico, IdEspecialidad, IdCobertura, IdPlan,
    FechaInicio, FechaFin, Estado, Observaciones)
VALUES (
    10, 6, 10, 10, NULL,
    DATEADD(DAY, (1 - CASE DATEPART(WEEKDAY, @Hoy) WHEN 1 THEN 7 ELSE DATEPART(WEEKDAY, @Hoy)-1 END + 7) % 7 + 7, @Hoy) + CAST('13:00' AS DATETIME),
    DATEADD(DAY, (1 - CASE DATEPART(WEEKDAY, @Hoy) WHEN 1 THEN 7 ELSE DATEPART(WEEKDAY, @Hoy)-1 END + 7) % 7 + 7, @Hoy) + CAST('14:00' AS DATETIME),
    'C', 'Cancelado por paciente'
);

GO

-- HISTORIA CLÍNICA (SOLO PARA TURNOS CERRADOS)
INSERT INTO HistoriaClinicaRegistro
(IdTurno, IdPaciente, IdMedico, IdEspecialidad, IdTratamiento, Diagnostico, Observaciones, FechaAtencion)
VALUES
-- Turno 6: Prótesis realizada (CERRADO)
(6, 6, 6, 7, 7, 'Ausencia 36-37', 'Prótesis colocada', DATEADD(DAY, -14, CAST(GETDATE() AS DATE))),

-- Turno 8: Endodoncia realizada (CERRADO)
(8, 8, 2, 2, 3, 'Infección tratada', 'Revisión en 10 días', DATEADD(DAY, -10, CAST(GETDATE() AS DATE)));

GO

-- PacienteCoberturaHistorial (Estado actual)
INSERT INTO PacienteCoberturaHistorial (IdPaciente, IdCobertura, IdPlan, FechaInicio, Estado)
SELECT IdPaciente, IdCobertura, IdPlan, FechaAlta, 'A'
FROM Paciente;

GO

-- Casos históricos de cambio de cobertura
INSERT INTO PacienteCoberturaHistorial (IdPaciente, IdCobertura, IdPlan, FechaInicio, FechaFin, Estado, MotivoCambio)
VALUES
(1, 2, 2, '2024-01-01', '2025-02-01', 'I', 'Cambio de cobertura a Particular'),
(1, 1, NULL, '2025-02-02', NULL, 'A', 'Paciente ahora sin cobertura'),
(2, 2, 2, '2024-05-01', NULL, 'A', 'Cobertura activa sin cambios'),
(3, 3, 3, '2024-03-15', '2025-03-15', 'I', 'Actualización a plan superior'),
(3, 3, 4, '2025-03-16', NULL, 'A', 'Upgrade a Swiss Medical SM50'),
(4, 4, 6, '2024-01-01', NULL, 'I', 'Cobertura y paciente inactivos'),
(5, 5, 7, '2024-08-01', NULL, 'A', 'Afiliación estable'),
(6, 6, 8, '2023-12-01', '2024-12-31', 'I', 'Baja temporal de cobertura'),
(6, 6, 8, '2025-01-01', NULL, 'A', 'Reactivación de cobertura Federada'),
(7, 7, 9, '2024-04-01', NULL, 'A', 'Cobertura estable'),
(8, 8, 10, '2024-01-01', NULL, 'I', 'Cobertura dada de baja'),
(9, 6, 8, '2023-09-01', '2024-11-30', 'I', 'Cambio de cobertura a IOMA'),
(9, 9, 8, '2024-12-01', NULL, 'A', 'Cobertura actual IOMA'),
(10, 10, NULL, '2023-10-01', NULL, 'I', 'Paciente dado de baja');
GO

-- CoberturaPorcentajeHistorial
INSERT INTO CoberturaPorcentajeHistorial
    (IdCobertura, PorcentajeCobertura, FechaInicio, FechaFin, Estado, MotivoCambio)
VALUES
    -- IOMA (IdCobertura = 9)
    (9, 45, '2023-01-01', '2024-11-30', 'I', 'Convenio provincial 2023'),
    (9, 40, '2024-12-01', NULL, 'A', 'Nuevo convenio estatal 2024'),
    
    -- OSPE (IdCobertura = 10)
    (10, 50, '2023-01-01', '2024-06-30', 'I', 'Valor inicial de cobertura'),
    (10, 55, '2024-07-01', NULL, 'A', 'Ajuste por nuevos aranceles');
GO

-- PlanPorcentajeHistorial
INSERT INTO PlanPorcentajeHistorial (IdPlan, PorcentajeCobertura, FechaInicio, FechaFin, Estado, MotivoCambio)
VALUES
-- 1. Particular
(1, 0, '2023-01-01', NULL, 'A', 'Sin cobertura'),

-- 2. OSDE 210 – subió del 65% al 70%
(2, 65, '2023-01-01', '2024-06-30', 'I', 'Convenio 2023 cerrado'),
(2, 70, '2024-07-01', NULL, 'A', 'Convenio 2024 vigente'),

-- 3. OSDE 310 – estable 90%
(3, 90, '2023-01-01', NULL, 'A', 'Cobertura completa'),

-- 4. SM30 – estable
(4, 80, '2023-01-01', NULL, 'A', 'Plan básico Swiss Medical'),

-- 5. SM50 – subió del 85% al 90%
(5, 85, '2023-01-01', '2024-05-31', 'I', 'Actualización de cobertura'),
(5, 90, '2024-06-01', NULL, 'A', 'Plan avanzado vigente'),

-- 6. Galeno G100 – redujo del 80% al 75%
(6, 80, '2023-01-01', '2024-01-31', 'I', 'Ajuste pre-baja'),
(6, 75, '2024-02-01', NULL, 'A', 'Último valor antes de baja'),

-- 7. Medicus Plus – estable
(7, 85, '2023-01-01', NULL, 'A', 'Cobertura integral sin cambios'),

-- 8. FS20 – aumentó de 55% a 60%
(8, 55, '2023-01-01', '2024-03-31', 'I', 'Revisión anual'),
(8, 60, '2024-04-01', NULL, 'A', 'Cobertura actualizada'),

-- 9. Omint O30 – estable
(9, 70, '2023-01-01', NULL, 'A', 'Plan preventivo'),

-- 10. Sancor S40 – bajó de 70% a 65%
(10, 70, '2023-01-01', '2024-02-29', 'I', 'Reducción 2024'),
(10, 65, '2024-03-01', NULL, 'A', 'Último porcentaje previo a inactividad');
GO

