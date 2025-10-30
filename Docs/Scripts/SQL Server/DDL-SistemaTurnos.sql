CREATE DATABASE SistemaOdontologico;

GO

USE SistemaOdontologico;

GO

CREATE TABLE Coberturas (
    IdCobertura INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL UNIQUE,
    Descripcion NVARCHAR(255),
    Estado BIT NOT NULL DEFAULT 1
);


-- datos de test
INSERT INTO Coberturas (Nombre, Descripcion, Estado) VALUES
('Particular', 'Pacientes sin cobertura médica', 1),
('OSDE', 'Cobertura médica privada de amplia red de odontólogos', 1),
('Swiss Medical', 'Cobertura premium con planes de salud odontológica', 1),
('Galeno', 'Cobertura con planes familiares y amplia cartilla', 1),
('Medicus', 'Plan odontológico integral con cobertura total', 1),
('Federada Salud', 'Cobertura con atención odontológica en red propia', 1),
('Omint', 'Cobertura integral con programas preventivos dentales', 1),
('Sancor Salud', 'Planes odontológicos familiares y corporativos', 1),
('IOMA', 'Cobertura estatal para empleados públicos y docentes', 1),
('OSPE', 'Obra social de personal de estaciones de servicio', 1),
('OSDEPYM', 'Cobertura médica para pequeñas y medianas empresas', 1),
('Prevención Salud', 'Cobertura privada con odontología básica y avanzada', 1),
('OSPERYHRA', 'Obra social del personal de restaurantes y hoteles', 0),
('Unión Personal', 'Cobertura con planes odontológicos opcionales', 1),
('OSPAT', 'Cobertura de trabajadores de la actividad del transporte', 1),
('ACA Salud', 'Cobertura médica con red odontológica nacional', 1),
('DOSEM', 'Cobertura para empleados del sector educativo provincial', 1),
('PAMI', 'Cobertura estatal para jubilados y pensionados', 1),
('OSECAC', 'Obra social de empleados de comercio', 1),
('OSSEG', 'Obra social del seguro, con cobertura odontológica limitada', 0);