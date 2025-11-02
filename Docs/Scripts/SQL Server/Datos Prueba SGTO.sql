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
('Recepcionista', 'Gestiona turnos y pacientes', 'A'),
('Médico', 'Acceso a agenda y registro clínico', 'A'),
('Auditor', 'Visualiza reportes y estadísticas', 'I'),
('Soporte', 'Mantenimiento técnico y parámetros', 'A'),
('Asistente', 'Asiste en recepción', 'A'),
('Supervisor', 'Monitorea turnos y personal médico', 'A'),
('Contable', 'Acceso a reportes económicos', 'I'),
('Becario', 'Acceso limitado a datos clínicos', 'A'),
('Invitado', 'Solo lectura', 'I');



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



-- RolPermiso

-- Administrador: todos los permisos
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT 1, IdPermiso FROM Permiso;

-- Recepcionista: TODO sobre Turnos y Pacientes
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT 2, IdPermiso FROM Permiso WHERE Modulo IN ('Turnos','Pacientes');

-- Médico: Turnos(Ver/Editar) + Pacientes(Ver) + Tratamientos(Ver) + Especialidades(Ver)
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT 3, IdPermiso
FROM Permiso
WHERE (Modulo='Turnos' AND Accion IN ('Ver','Editar'))
   OR (Modulo='Pacientes' AND Accion='Ver')
   OR (Modulo='Tratamientos' AND Accion='Ver')
   OR (Modulo='Especialidades' AND Accion='Ver');

-- Auditor: Reportes(Ver)
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT 4, IdPermiso FROM Permiso WHERE Modulo='Reportes' AND Accion='Ver';

-- Soporte: Configuración(Ver/Editar) + Parámetros(Ver/Editar)
INSERT INTO RolPermiso (IdRol, IdPermiso)
SELECT 5, IdPermiso
FROM Permiso
WHERE (Modulo='Configuracion' AND Accion IN ('Ver','Editar'))
   OR (Modulo='ParametrosSistema' AND Accion IN ('Ver','Editar'));



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
('Luis','Pérez','luis.perez@sgto.com','lperez','hash2',2,'A'),
('Sofía','López','sofia.lopez@sgto.com','slopez','hash3',3,'A'),
('Martín','Ruiz','martin.ruiz@sgto.com','mruiz','hash4',3,'A'),
('Paula','Mendoza','paula.mendoza@sgto.com','pmendoza','hash5',2,'I'),
('Esteban','Fernández','esteban.fernandez@sgto.com','efernandez','hash6',1,'A'),
('Valeria','Díaz','valeria.diaz@sgto.com','vdiaz','hash7',4,'A'),
('Camila','Rossi','camila.rossi@sgto.com','crossi','hash8',5,'A'),
('Nicolás','Benítez','nicolas.benitez@sgto.com','nbenitez','hash9',3,'A'),
('Lucía','Romero','lucia.romero@sgto.com','lromero','hash10',3,'I');



-- Medico
INSERT INTO Medico (Nombre, Apellido, NumeroDocumento, Genero, FechaNacimiento, Telefono, Email, Matricula, IdUsuario, Estado)
VALUES
('Martín','Ruiz','30234567','M','1985-04-15','1123456789','martin.ruiz@sgto.com','MP1234',4,'A'),
('Sofía','López','31234568','F','1988-10-20','1123456790','sofia.lopez@sgto.com','MP1235',3,'A'),
('Nicolás','Benítez','29234569','M','1984-02-28','1123456791','nicolas.benitez@sgto.com','MP1236',9,'A'),
('Lucía','Romero','28234570','F','1990-07-18','1123456792','lucia.romero@sgto.com','MP1237',10,'I'),
('Camila','Rossi','32234571','F','1991-05-12','1123456793','camila.rossi@sgto.com','MP1238',8,'A'),
('Carlos','Méndez','27234572','M','1982-11-02','1123456794','carlos.mendez@sgto.com','MP1239',6,'A'),
('Julia','Ferrer','29234573','F','1993-03-23','1123456795','julia.ferrer@sgto.com','MP1240',7,'A'),
('Sebastián','Arias','28234574','M','1987-12-01','1123456796','sebastian.arias@sgto.com','MP1241',5,'A'),
('Patricia','Vega','27234575','F','1980-09-09','1123456797','patricia.vega@sgto.com','MP1242',2,'A'),
('Diego','Luna','29234576','M','1986-01-10','1123456798','diego.luna@sgto.com','MP1243',1,'I');



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
('Hernán','Molina','48111229','M','1983-12-30','1150012240','hernan.molina@correo.com',8,10,'A'),
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
(6,'Lunes','13:00','17:00','A'),
(7,'Martes','08:00','12:00','A'),
(8,'Miércoles','09:00','13:00','A'),
(9,'Viernes','14:00','18:00','I');




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
(7,7,5,2,7,9,'2025-10-28 10:00','2025-10-28 11:00','C','Cancelado por clima'),
(8,8,8,8,8,10,'2025-10-29 09:00','2025-10-29 10:00','Z','Radiografía realizada'),
(9,9,4,9,9,8,'2025-10-29 11:00','2025-10-29 12:00','N','Selladores'),
(10,10,3,1,10,NULL,'2025-10-30 10:00','2025-10-30 11:00','N','Limpieza inicial');



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
(7,7,7,5,2,'Pieza 48 afectada','Extracción reprogramada','2025-10-28'),
(8,8,8,8,8,'Revisión completa','Sin hallazgos','2025-10-29'),
(9,9,9,4,9,'Prevención caries','Aplicación de selladores','2025-10-29'),
(10,10,10,3,1,'Tártaro leve','Se indicó limpieza y control','2025-10-30');
GO
