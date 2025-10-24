# 🦷 Sistema de Gestión de Turnos – Clínica Odontológica

Aplicación web desarrollada en **ASP.NET WebForms (.NET Framework 4.8)** para la administración integral de una clínica odontológica.  
Permite gestionar turnos, pacientes, médicos, coberturas, tratamientos, usuarios y reportes, con arquitectura en capas, manejo de excepciones y validaciones completas.

---

## Características principales

- **Gestión de Turnos**: alta, edición, reprogramación, cancelación y cierre de turnos.
- **Validación de solapamientos**: control de disponibilidad de médicos y pacientes.
- **Historia clínica automática**: generación de registros clínicos desde turnos cerrados.
- **Módulo de Pacientes y Médicos**: administración de datos personales, coberturas y especialidades.
- **Coberturas y Planes**: gestión completa de obras sociales y planes asociados.
- **Usuarios y Roles**: seguridad con autenticación y autorización por perfil (Administrador, Médico, Recepcionista).
- **Reportes**: exportación a PDF o Excel de turnos por fecha, médico, estado o pacientes atendidos.
- **Validaciones globales y manejo de errores**: sistema robusto con logs y alertas visuales.
- **Diseño UI moderno y responsivo** con Bootstrap.

---

## Arquitectura

El proyecto sigue una **arquitectura en capas** para lograr separación de responsabilidades y facilidad de mantenimiento:

```

📦 SGTO.Solucion
┣ 📂 SGTO.Presentacion       → Capa UI (WebForms + Bootstrap)
┣ 📂 SGTO.Negocio            → Lógica de negocio y validaciones
┣ 📂 SGTO.Datos              → ADO.NET + SQL Server
┣ 📂 SGTO.Dominio            → Modelo de entidades y asociaciones
┣ 📂 SGTO.Comun              → Utilidades, validadores y logger

````

---

## Base de datos

- **Motor:** SQL Server  
- **Integración:** ADO.NET con comandos parametrizados y transacciones.  
- **Diseño:** Tablas normalizadas con relaciones y bajas lógicas.  
- **Principales tablas:** `Pacientes`, `Medicos`, `Especialidades`, `Tratamientos`, `Turnos`, `Coberturas`, `Planes`, `Usuarios`, `Roles`, `HistoriaClinicaRegistros`, `EstadosTurno`.

---

## Roles del sistema

| Rol | Permisos principales |
|------|----------------------|
| **Administrador** | Acceso completo a todos los módulos, reportes y configuración. |
| **Recepcionista** | Gestión de pacientes, médicos, coberturas y turnos. |
| **Médico** | Visualización y cierre de turnos propios, con registro de observaciones clínicas. |

---

## Reglas de negocio destacadas

- Un médico o paciente no puede tener dos turnos en el mismo horario.  
- Los turnos no se eliminan, solo cambian de estado (Nuevo, Reprogramado, Cancelado, No Asistió, Cerrado).  
- Las contraseñas se almacenan encriptadas (hash SHA256).  
- Validaciones duplicadas: lado servidor y cliente.  
- Manejo global de excepciones y registro en logs.

---

## Tecnologías utilizadas

- **Lenguaje:** C# (.NET Framework 4.8)  
- **Frontend:** ASP.NET WebForms + Bootstrap  
- **Backend:** ADO.NET (sin ORMs, consultas parametrizadas)  
- **Base de datos:** SQL Server  
- **Arquitectura:** N-Capas  

---

## Reportes disponibles

- Turnos por rango de fechas.  
- Turnos por médico y estado.  
- Pacientes atendidos en un período.  
- Estadísticas generales de estados de turno.  
- Exportación a PDF o Excel.
