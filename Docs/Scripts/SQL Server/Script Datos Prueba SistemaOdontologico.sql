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
   --Módulos con 6 acciones: Turnos, Pacientes, Medicos, Coberturas,
   --                        Planes, Especialidades, Tratamientos, Reportes, Usuarios
   --Módulos con 4 acciones: Roles (Ver/Crear/Editar/Eliminar)
   --Módulos con 2 acciones: Configuracion (Ver/Editar),
   --  ParametrosSistema (Ver/Editar)
INSERT INTO Permiso (Modulo, Accion, Descripcion) VALUES
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
('ParametrosSistema','Ver','Ver parámetros'),
('ParametrosSistema','Editar','Editar parámetros');



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
('Email_From', 'no-reply@sgto.com', 'Correo remitente'),
('Moneda', 'ARS', 'Moneda local'),
('DiasAnticipacionTurno', '30', 'Días de anticipación máximos'),
('MaxTurnosPorDia', '20', 'Máximo de turnos/día'),
('NombreClinica', 'Clínica SGTO', 'Nombre visible del sistema');



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



-- RolPermiso;
-- Administrador: todos los permisos
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT 1, IdPermiso FROM Permiso;

-- Recepcionista: TODO sobre Turnos y Pacientes
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT 2, IdPermiso FROM Permiso
WHERE Modulo IN ('Turnos','Pacientes');

-- Médico: Turnos(Ver/Editar) + Pacientes(Ver) + Tratamientos(Ver) + Especialidades(Ver)
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT 3, IdPermiso
FROM Permiso
WHERE
    (Modulo = 'Turnos' AND Accion IN ('Ver','Editar'))
    OR
    (Modulo = 'Pacientes' AND Accion = 'Ver');;




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
('Control General', 'Consulta básica', 3500, 10, 'A');



-- Usuario
INSERT INTO Usuario (Nombre, Apellido, Email, NombreUsuario, PasswordHash, IdRol, Estado)
VALUES
('Ana','García','ana.garcia@sgto.com','agarcia','hash1',1,'A'),
('Esteban','Fernández','esteban.fernandez@sgto.com','efernandez','hash2',1,'A'),
('Luis','Pérez','luis.perez@sgto.com','lperez','hash3',2,'A'),
('Paula','Mendoza','paula.mendoza@sgto.com','pmendoza','hash4',2,'I'),
('Sofía','López','sofia.lopez@sgto.com','slopez','hash5',3,'A'),
('Martín','Ruiz','martin.ruiz@sgto.com','mruiz','hash6',3,'A'),
('Nicolás','Benítez','nicolas.benitez@sgto.com','nbenitez','hash7',3,'A'),
('Lucía','Romero','lucia.romero@sgto.com','lromero','hash8',3,'I'),
('Camila','Rossi','camila.rossi@sgto.com','crossi','hash9',3,'A'),
('Carlos','Méndez','carlos.mendez@sgto.com','cmendez','hash10',3,'A');


-- Medico
INSERT INTO Medico
(Nombre, Apellido, NumeroDocumento, Genero, FechaNacimiento, Telefono, Email, Matricula, IdUsuario, Estado)
VALUES
('Sofía','López','31234568','F','1988-10-20','1123456790','sofia.lopez@sgto.com','MP1235',5,'A'),
('Martín','Ruiz','30234567','M','1985-04-15','1123456789','martin.ruiz@sgto.com','MP1234',6,'A'),
('Nicolás','Benítez','29234569','M','1984-02-28','1123456791','nicolas.benitez@sgto.com','MP1236',7,'A'),
('Lucía','Romero','28234570','F','1990-07-18','1123456792','lucia.romero@sgto.com','MP1237',8,'I'),
('Camila','Rossi','32234571','F','1991-05-12','1123456793','camila.rossi@sgto.com','MP1238',9,'A'),
('Carlos','Méndez','27234572','M','1982-11-02','1123456794','carlos.mendez@sgto.com','MP1239',10,'A')



-- Paciente
INSERT INTO Paciente
(Nombre, Apellido, NumeroDocumento, Genero, FechaNacimiento, Telefono, Email, IdCobertura, IdPlan, Estado)
VALUES
('Andrés','Suárez','40111222','M','1992-05-15','1150012233','andres.suarez@correo.com',1,NULL,'A'),       
('Belén','Gómez','42111223','F','1994-09-20','1150012234','belen.gomez@correo.com',2,2,'A'),               
('Carlos','Vega','43111224','M','1987-03-11','1150012235','carlos.vega@correo.com',3,3,'A'),               
('Diana','Pérez','44111225','F','1990-08-02','1150012236','diana.perez@correo.com',4,6,'I'),              
('Elena','Rodríguez','45111226','F','1989-10-12','1150012237','elena.rodriguez@correo.com',5,7,'A'),      
('Francisco','Luna','46111227','M','1985-11-22','1150012238','francisco.luna@correo.com',6,8,'A'),         
('Gabriela','Fernández','47111228','F','1991-06-13','1150012239','gabriela.fernandez@correo.com',7,9,'A'),
('Hernán','Molina','48111229','M','1983-12-30','1150012240','hernan.molina@correo.com',8,10,'I'),          
('Isabel','Núñez','49111230','F','1996-04-21','1150012241','isabel.nunez@correo.com',9,8,'A'),           
('Jorge','Santos','50111231','M','1980-01-05','1150012242','jorge.santos@correo.com',10,NULL,'I');



-- HorarioAtencion
INSERT INTO HorarioAtencion (IdMedico, DiaSemana, HoraInicio, HoraFin, Estado) VALUES
(1,'Lunes','08:00','12:00','A'),
(1,'Miércoles','14:00','18:00','A'),
(2,'Martes','09:00','13:00','A'),
(3,'Jueves','10:00','14:00','A'),
(4,'Viernes','08:00','12:00','I'),
(5,'Sábado','09:00','13:00','A'),
(6,'Lunes','13:00','17:00','A');




-- Turno
INSERT INTO Turno
(IdPaciente, IdMedico, IdEspecialidad, IdTratamiento, IdCobertura, IdPlan, FechaInicio, FechaFin, Estado, Observaciones)
VALUES
(1,1,10,10,1,NULL,'2025-10-25 09:00','2025-10-25 10:00','N','Control general'),
(2,2,1,4,2,2,'2025-10-26 10:00','2025-10-26 11:00','R','Reprogramado por médico'),
(3,3,2,3,3,3,'2025-10-26 11:00','2025-10-26 12:00','C','Cancelado por paciente'),
(4,4,9,6,4,6,'2025-10-27 15:00','2025-10-27 16:00','X','No asistió'),
(5,5,6,5,5,7,'2025-10-27 09:00','2025-10-27 10:00','N','Implante programado'),
(6,6,7,7,6,8,'2025-10-28 14:00','2025-10-28 15:00','Z','Prótesis realizada'),
(7,1,5,2,7,9,'2025-10-28 10:00','2025-10-28 11:00','C','Cancelado por clima'),
(8,2,8,8,8,10,'2025-10-29 09:00','2025-10-29 10:00','Z','Radiografía realizada'),
(9,3,4,9,9,8,'2025-10-29 11:00','2025-10-29 12:00','N','Selladores'),
(10,4,3,1,10,NULL,'2025-10-30 10:00','2025-10-30 11:00','N','Limpieza inicial');



-- HistoriaClinicaRegistro
INSERT INTO HistoriaClinicaRegistro
(IdTurno, IdPaciente, IdMedico, IdEspecialidad, IdTratamiento, Diagnostico, Observaciones, FechaAtencion)
VALUES
(1,1,1,10,10,'Buen estado general','Control cada 6 meses','2025-10-25'),
(2,2,2,1,4,'Mordida cruzada','Reprogramado; ajuste plan','2025-10-26'),
(3,3,3,2,3,'Infección tratada','Revisión en 10 días','2025-10-26'),
(4,4,4,9,6,'Manchas dentales','Ausencia al turno','2025-10-27'),
(5,5,5,6,5,'Pérdida pieza 24','Implante programado','2025-10-27'),
(6,6,6,7,7,'Ausencia 36-37','Prótesis colocada','2025-10-28'),
(7,7,1,5,2,'Pieza 48 afectada','Extracción reprogramada','2025-10-28'),
(8,8,2,8,8,'Revisión completa','Sin hallazgos','2025-10-29'),
(9,9,3,4,9,'Prevención caries','Aplicación de selladores','2025-10-29'),
(10,10,4,3,1,'Tártaro leve','Se indicó limpieza y control','2025-10-30');


GO

INSERT INTO PacienteCoberturaHistorial (IdPaciente, IdCobertura, IdPlan, FechaInicio, Estado)
SELECT IdPaciente, IdCobertura, IdPlan, FechaAlta, 'A'
FROM Paciente;

GO

-- Casos para testear:
--  - Pacientes con 1 historial actual (común)
--  - Pacientes con 2+ historiales (cambio de plan o cobertura)
--  - Pacientes inactivos (historial inactivo)
--  - Coberturas y planes coherentes con tus inserts actuales

INSERT INTO PacienteCoberturaHistorial (IdPaciente, IdCobertura, IdPlan, FechaInicio, FechaFin, Estado, MotivoCambio)
VALUES
-- 1. Andrés Suárez – pasó de OSDE 210 a Particular
(1, 2, 2, '2024-01-01', '2025-02-01', 'I', 'Cambio de cobertura a Particular'),
(1, 1, NULL, '2025-02-02', NULL, 'A', 'Paciente ahora sin cobertura'),

-- 2. Belén Gómez – sigue con OSDE 210 (actual)
(2, 2, 2, '2024-05-01', NULL, 'A', 'Cobertura activa sin cambios'),

-- 3. Carlos Vega – Swiss Medical: cambió de plan SM30 → SM50
(3, 3, 3, '2024-03-15', '2025-03-15', 'I', 'Actualización a plan superior'),
(3, 3, 4, '2025-03-16', NULL, 'A', 'Upgrade a Swiss Medical SM50'),

-- 4. Diana Pérez – Galeno (inactiva)
(4, 4, 6, '2024-01-01', NULL, 'I', 'Cobertura y paciente inactivos'),

-- 5. Elena Rodríguez – Medicus Plus (sin cambios)
(5, 5, 7, '2024-08-01', NULL, 'A', 'Afiliación estable'),

-- 6. Francisco Luna – Federada: tuvo interrupción y reactivación
(6, 6, 8, '2023-12-01', '2024-12-31', 'I', 'Baja temporal de cobertura'),
(6, 6, 8, '2025-01-01', NULL, 'A', 'Reactivación de cobertura Federada'),

-- 7. Gabriela Fernández – Omint (sin cambios)
(7, 7, 9, '2024-04-01', NULL, 'A', 'Cobertura estable'),

-- 8. Hernán Molina – Sancor (todo inactivo)
(8, 8, 10, '2024-01-01', NULL, 'I', 'Cobertura dada de baja'),

-- 9. Isabel Núñez – IOMA: tuvo Federada previamente
(9, 6, 8, '2023-09-01', '2024-11-30', 'I', 'Cambio de cobertura a IOMA'),
(9, 9, 8, '2024-12-01', NULL, 'A', 'Cobertura actual IOMA'),

-- 10. Jorge Santos – OSPE (paciente inactivo)
(10, 10, NULL, '2023-10-01', NULL, 'I', 'Paciente dado de baja');
GO


INSERT INTO CoberturaPorcentajeHistorial
    (IdCobertura, PorcentajeCobertura, FechaInicio, FechaFin, Estado, MotivoCambio)
VALUES
    (9, 45, '2023-01-01', '2024-11-30', 'I', 'Convenio provincial 2023'),
    (9, 40, '2024-12-01', NULL, 'A', 'Nuevo convenio estatal 2024');

-- OSPE (IdCobertura = 10):
--   2023 cubría 50%
--   2024 sube un poco a 55%
INSERT INTO CoberturaPorcentajeHistorial
    (IdCobertura, PorcentajeCobertura, FechaInicio, FechaFin, Estado, MotivoCambio)
VALUES
    (10, 50, '2023-01-01', '2024-06-30', 'I', 'Valor inicial de cobertura'),
    (10, 55, '2024-07-01', NULL, 'A', 'Ajuste por nuevos aranceles');
GO


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

