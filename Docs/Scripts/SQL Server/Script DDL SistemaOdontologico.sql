USE master;
GO
-- esto es para que no de error si hay conexiones activas
ALTER DATABASE SistemaOdontologico SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

DROP DATABASE IF EXISTS SistemaOdontologico;
GO

CREATE DATABASE SistemaOdontologico;
GO

USE SistemaOdontologico;
GO

CREATE TABLE Cobertura (
    IdCobertura INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(80) NOT NULL,
    Descripcion NVARCHAR(200) NULL,
    Estado CHAR(1) NOT NULL DEFAULT 'A',

    CONSTRAINT CHK_Cobertura_Estado CHECK (Estado IN ('A','I'))
);

GO

CREATE TABLE Rol (
    IdRol INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(200) NULL,
    Estado CHAR(1) NOT NULL DEFAULT 'A',

    CONSTRAINT CHK_Rol_Estado CHECK (Estado IN ('A','I'))
);

GO

CREATE TABLE Permiso (
    IdPermiso INT PRIMARY KEY IDENTITY(1,1),
    Modulo NVARCHAR(80) NOT NULL,
    Accion NVARCHAR(50) NOT NULL,
    Descripcion NVARCHAR(150) NULL
);

GO

CREATE TABLE Especialidad (
    IdEspecialidad INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(80) NOT NULL,
    Descripcion NVARCHAR(200) NULL,
    Estado CHAR(1) NOT NULL DEFAULT 'A',

    CONSTRAINT CHK_Especialidad_Estado CHECK (Estado IN ('A','I'))
);


GO

CREATE TABLE ParametroSistema (
    IdParametroSistema INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(80) NOT NULL,
    Valor NVARCHAR(150) NOT NULL,
    Descripcion NVARCHAR(200) NOT NULL
);

GO

CREATE TABLE [Plan] (
    IdPlan INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(80) NOT NULL,
    Descripcion NVARCHAR(200) NULL,
    PorcentajeCobertura DECIMAL(10, 2) NOT NULL,
    IdCobertura INT NOT NULL,
    Estado CHAR(1) NOT NULL DEFAULT 'A',

    CONSTRAINT FK_Plan_Cobertura FOREIGN KEY(IdCobertura) REFERENCES Cobertura(IdCobertura),
    CONSTRAINT CHK_Plan_Estado CHECK (Estado IN ('A','I'))
);

GO

CREATE TABLE RolPermiso (
    IdRol INT NOT NULL,
    IdPermiso INT NOT NULL,

    CONSTRAINT PK_RolPermiso PRIMARY KEY(IdRol, IdPermiso),
	CONSTRAINT FK_RolPermiso_Archivos FOREIGN KEY(IdRol) References Rol(IdRol),
	CONSTRAINT FK_RolPermiso_Usuarios FOREIGN KEY(IdPermiso) References Permiso(IdPermiso)
);

GO

CREATE TABLE Tratamiento (
    IdTratamiento INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(80) NOT NULL,
    Descripcion NVARCHAR(200) NULL,
    CostoBase DECIMAL(10, 2) NOT NULL,
    IdEspecialidad INT NOT NULL,
    Estado CHAR(1) NOT NULL DEFAULT 'A',

    CONSTRAINT FK_Tratamiento_Especialidad FOREIGN KEY(IdEspecialidad) REFERENCES Especialidad(IdEspecialidad),
    CONSTRAINT CHK_Tratamiento_Estado CHECK (Estado IN ('A','I'))
);

GO

CREATE TABLE Usuario (
    IdUsuario INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Email NVARCHAR(120) NOT NULL,
    NombreUsuario NVARCHAR(80) NOT NULL,
    PasswordHash NVARCHAR(200) NOT NULL,
    IdRol INT NOT NULL,
    Estado CHAR(1) NOT NULL DEFAULT 'A',
    FechaAlta DATETIME NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Usuario_Rol FOREIGN KEY(IdRol) REFERENCES Rol(IdRol),
    CONSTRAINT CHK_Usuario_Estado CHECK (Estado IN ('A','I')),
    CONSTRAINT UQ_Usuario_Email UNIQUE (Email),
    CONSTRAINT UQ_Usuario_NombreUsuario UNIQUE (NombreUsuario)
);

GO

CREATE TABLE Medico (
    IdMedico INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    NumeroDocumento VARCHAR(15) NOT NULL,
    Genero CHAR(1) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Telefono VARCHAR(30) NOT NULL,
    Matricula NVARCHAR(50) NOT NULL,
    IdUsuario INT NOT NULL,
    Estado CHAR(1) NOT NULL DEFAULT 'A',
    FechaAlta DATETIME NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Medico_Usuario FOREIGN KEY(IdUsuario) REFERENCES Usuario(IdUsuario),
    CONSTRAINT CHK_Medico_Estado CHECK (Estado IN ('A','I')),
    CONSTRAINT CHK_Medico_Genero CHECK (Genero IN ('M','F','O','N')),
    CONSTRAINT UQ_Medico_NumeroDocumento UNIQUE (NumeroDocumento),
    CONSTRAINT UQ_Medico_Matricula UNIQUE (Matricula)
);

GO

CREATE TABLE MedicoEspecialidad (
    IdMedico INT NOT NULL,
    IdEspecialidad INT NOT NULL,
    CONSTRAINT PK_MedicoEspecialidad PRIMARY KEY (IdMedico, IdEspecialidad),
    CONSTRAINT FK_ME_Medico FOREIGN KEY(IdMedico) REFERENCES Medico(IdMedico),
    CONSTRAINT FK_ME_Especialidad FOREIGN KEY(IdEspecialidad) REFERENCES Especialidad(IdEspecialidad)
);

GO


CREATE TABLE Paciente (
    IdPaciente INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    NumeroDocumento VARCHAR(15) NOT NULL,
    Genero CHAR(1) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Telefono VARCHAR(30) NOT NULL,
    Email NVARCHAR(120) NOT NULL,
    IdCobertura INT NOT NULL,
    IdPlan INT NULL,
    Estado CHAR(1) NOT NULL DEFAULT 'A',
    FechaAlta DATETIME NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Paciente_Cobertura FOREIGN KEY(IdCobertura) REFERENCES Cobertura(IdCobertura),
    CONSTRAINT FK_Paciente_Plan FOREIGN KEY(IdPlan) REFERENCES [Plan](IdPlan),
    CONSTRAINT CHK_Paciente_Estado CHECK (Estado IN ('A','I')),
    CONSTRAINT CHK_Paciente_Genero CHECK (Genero IN ('M','F','O','N')),
    CONSTRAINT UQ_Paciente_NumeroDocumento UNIQUE (NumeroDocumento)
);

GO

CREATE TABLE Turno (
    IdTurno INT PRIMARY KEY IDENTITY(1,1),
    IdPaciente INT NOT NULL,
    IdMedico INT NOT NULL,
    IdEspecialidad INT NOT NULL,
    IdTratamiento INT NOT NULL,
    IdCobertura INT NOT NULL,
    IdPlan INT NULL,
    FechaInicio DATETIME NOT NULL,
    FechaFin DATETIME NOT NULL,
    Estado CHAR(1) NOT NULL,
    Observaciones NVARCHAR(250) NULL,

    CONSTRAINT FK_Turno_Paciente FOREIGN KEY(IdPaciente) REFERENCES Paciente(IdPaciente),
    CONSTRAINT FK_Turno_Medico FOREIGN KEY(IdMedico) REFERENCES Medico(IdMedico),
    CONSTRAINT FK_Turno_Especialidad FOREIGN KEY(IdEspecialidad) REFERENCES Especialidad(IdEspecialidad),
    CONSTRAINT FK_Turno_Tratamiento FOREIGN KEY(IdTratamiento) REFERENCES Tratamiento(IdTratamiento),
    CONSTRAINT FK_Turno_Cobertura FOREIGN KEY(IdCobertura) REFERENCES Cobertura(IdCobertura),
    CONSTRAINT FK_Turno_Plan FOREIGN KEY(IdPlan) REFERENCES [Plan](IdPlan),
    CONSTRAINT CHK_Turno_Estado CHECK (Estado IN ('N','P','R','X','C','Z'))
);


GO

CREATE TABLE HorarioSemanalMedico (
    IdHorarioSemanal INT IDENTITY(1,1) PRIMARY KEY,
    IdMedico INT NOT NULL,
    DiaSemana TINYINT NOT NULL,          -- 1 = Lunes ... 7 = Domingo
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,
    Estado CHAR(1) NOT NULL DEFAULT 'A',
    CONSTRAINT FK_HorarioSemanal_Medico
        FOREIGN KEY (IdMedico) REFERENCES dbo.Medico (IdMedico),
    CONSTRAINT CHK_HorarioSemanal_Dia CHECK (DiaSemana BETWEEN 1 AND 7),
    CONSTRAINT CHK_HorarioSemanal_Rango CHECK (HoraInicio < HoraFin),
    CONSTRAINT CHK_HorarioSemanal_Estado CHECK (Estado IN ('A','I'))

);
GO

CREATE INDEX IX_Horario_Medico_Dia ON HorarioSemanalMedico(IdMedico, DiaSemana, HoraInicio);

GO


CREATE TABLE HistoriaClinicaRegistro(
    IdHistoriaClinicaRegistro INT PRIMARY KEY IDENTITY(1,1),
    IdTurno INT NOT NULL,
    IdPaciente INT NOT NULL,
    IdMedico INT NOT NULL,
    IdEspecialidad INT NOT NULL,
    IdTratamiento INT NOT NULL,
    Diagnostico NVARCHAR(250) NOT NULL,
    Observaciones NVARCHAR(250) NOT NULL,
    FechaAtencion DATETIME NOT NULL,

    CONSTRAINT FK_HistoriaClinicaRegistro_Turno FOREIGN KEY(IdTurno) REFERENCES Turno(IdTurno),
    CONSTRAINT FK_HistoriaClinicaRegistro_Paciente FOREIGN KEY(IdPaciente) REFERENCES Paciente(IdPaciente),
    CONSTRAINT FK_HistoriaClinicaRegistro_Medico FOREIGN KEY(IdMedico) REFERENCES Medico(IdMedico),
    CONSTRAINT FK_HistoriaClinicaRegistro_Especialidad FOREIGN KEY(IdEspecialidad) REFERENCES Especialidad(IdEspecialidad),
    CONSTRAINT FK_HistoriaClinicaRegistro_Tratamiento FOREIGN KEY(IdTratamiento) REFERENCES Tratamiento(IdTratamiento)
);

GO

CREATE TABLE PacienteCoberturaHistorial (
    IdPacienteCoberturaHistorial INT PRIMARY KEY IDENTITY(1,1),
    IdPaciente INT NOT NULL,
    IdCobertura INT NOT NULL,
    IdPlan INT NULL,
    FechaInicio DATE NOT NULL DEFAULT GETDATE(),
    FechaFin DATE NULL,
    MotivoCambio NVARCHAR(200) NULL,
    Estado CHAR(1) NOT NULL DEFAULT 'A', -- 'A' = vigente, 'I' = hist�rico cerrado

    CONSTRAINT FK_PacienteCoberturaHistorial_Paciente FOREIGN KEY(IdPaciente) REFERENCES Paciente(IdPaciente),
    CONSTRAINT FK_PacienteCoberturaHistorial_Cobertura FOREIGN KEY(IdCobertura) REFERENCES Cobertura(IdCobertura),
    CONSTRAINT FK_PacienteCoberturaHistorial_Plan FOREIGN KEY(IdPlan) REFERENCES [Plan](IdPlan),
    CONSTRAINT CHK_PacienteCoberturaHistorial_Estado CHECK (Estado IN ('A','I'))
);

GO

CREATE TABLE CoberturaPorcentajeHistorial (
    IdHistorial INT IDENTITY(1,1) PRIMARY KEY,
    IdCobertura INT NOT NULL,
    PorcentajeCobertura DECIMAL(5,2) NOT NULL,
    FechaInicio DATE NOT NULL DEFAULT GETDATE(),
    FechaFin DATE NULL,
    Estado CHAR(1) NOT NULL DEFAULT 'A',   -- A = Activo, I = Inactivo (cerrado)
    MotivoCambio NVARCHAR(200) NULL,
    CONSTRAINT FK_CoberturaPorcentajeHistorial_Cobertura 
        FOREIGN KEY (IdCobertura) REFERENCES Cobertura(IdCobertura),
    CONSTRAINT CHK_CoberturaPorcentajeHistorial_Estado CHECK (Estado IN ('A','I'))
);
GO

CREATE TABLE PlanPorcentajeHistorial (
    IdHistorial INT IDENTITY(1,1) PRIMARY KEY,
    IdPlan INT NOT NULL,
    PorcentajeCobertura DECIMAL(5,2) NOT NULL,
    FechaInicio DATE NOT NULL DEFAULT GETDATE(),
    FechaFin DATE NULL,
    Estado CHAR(1) NOT NULL DEFAULT 'A',   -- A = vigente, I = cerrado
    MotivoCambio NVARCHAR(200) NULL,
    CONSTRAINT FK_PlanPorcentajeHistorial_Plan 
        FOREIGN KEY (IdPlan) REFERENCES [Plan](IdPlan),
    CONSTRAINT CHK_PlanPorcentajeHistorial_Estado CHECK (Estado IN ('A','I'))
);
GO