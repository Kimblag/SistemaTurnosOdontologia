# Modelo de Dominio – Sistema de Gestión de Turnos Odontológicos

## Tipos Compartidos (Dominio)

### Enums

* **EstadoEntidad:** `Activo`, `Inactivo`
  Controla la vigencia lógica de las entidades.
* **EstadoTurno:** `Nuevo`, `Reprogramado`, `Cancelado`, `NoAsistio`, `Cerrado`.
  Representa el ciclo de vida de un turno.
* **Genero:** `Masculino`, `Femenino`, `Otro`, `PrefiereNoDecir`.
  Define el género de personas (pacientes y médicos).
* **Modulo:** `Inicio`, `Turnos`, `Pacientes`, `Medicos`, `Coberturas`, `Planes`, `Especialidades`, `Tratamientos`, `Reportes`, `Configuracion`, `Usuarios`, `Roles`, `ParametrosSistema`.
  Representa la vista de un módulo.
* **TipoAccion:** `Ver`, `Crear`, `Editar`, `Eliminar`, `Activar`, `Desactivar`.
  Representa las acciones que se pueden realizar dentro del sistema.

### Objetos de Valor (Value Objects)
Representan objetos inmutables que validan su propio estado y comparan por valor.

| Objeto                 | Propiedades                       | Reglas e Invariantes                                                    |
| ---------------------- | --------------------------------- | ----------------------------------------------------------------------- |
| **Email**              | `Valor:string`                    | Debe tener formato de email válido, sin espacios, en minúsculas.        |
| **Telefono**           | `Numero:string`                   | Solo dígitos y separadores permitidos; opcionalmente prefijo país/área. |
| **DocumentoIdentidad** | `Numero:string`                   | 8 dígitos, sin letras (para DNI).                                       |
| **HorarioTurno**       | `Inicio:DateTime`, `Fin:DateTime` | Fin mayor que Inicio; duración múltiplo de 60 min según parámetro de sistema. |

Cada uno incluye métodos internos como `EsValido()`, `Normalizar()` y `ToString()`.
Se usan dentro de las entidades para garantizar integridad de datos.


## Clase base

### Persona (abstracta)

**Responsabilidad:** representar datos comunes de personas.

- **Propiedades**
    - `Nombre:string`
    - `Apellido:string`
    - `Dni:DocumentoIdentidad`
    - `FechaNacimiento:DateTime`
    - `Genero:Genero`
    - `Telefono:Telefono`
    - `Email:Email`
    - `Estado:EstadoEntidad` *(default: Activo)*
    - `FechaAlta:DateTime`
    - `FechaModificacion:DateTime?`
- **Constructor (obligatorios)**
    - `(nombre, apellido, dni, fechaNacimiento, genero, telefono, email)`
- **Algunos métodos**
    - `NombreCompleto():string`
    - `CalcularEdad():int`
    - `Activar()` / `Desactivar()`
- **Reglas internas**
    - Nombre/Apellido requeridos; DNI/Email/Teléfono válidos por sus objetos de valor.

---

## Personas del sistema

### Paciente : Persona

**Responsabilidad:** representar paciente.

- **Propiedades**
    - `IdPaciente: int`
    - `Cobertura:Cobertura` (agregación)
    - `Plan:Plan` (agregación; dependiente de Cobertura)
    - `Turnos:List<Turno>` (composición : el paciente “posee” su historia de turnos)
    - `HistoriaClinica:List<HistoriaClinicaRegistro>` (agregación, registros generados desde Turnos siempre y cuando el turno haya sido éxitoso)
- **Constructor (obligatorios)**
    - Hereda de Persona.
    - Constructor con datos para crar pro primera vez un paciente.
    - Construtor para cargar un paciente existente.
- **Reglas internas**
    - Si tiene Cobertura, Plan debe pertenecer a esa Cobertura.
    - LA cobertura por default es PArticular.
- **Métodos**
    - `AgregarTurno(Turno t)`: Permite agregar un nuevo turno al paciente manteniendo las reglas de consistencia.
        - No solapar con otros turnos.
        - Solo agregar turnos futuros o válidos.
            
            Este método se usaría en la capa de Negocio, en el servicio de Turnos y puede que también en el servicio de Pacientes cuando se agenda un nuevo turno. El servicio debe coordinar la acción, pero tiene que delegarle al dominio la validacióon. Por ejemplo:
            
            1. La UI llama al `TurnoServicio.AgendarTurno(...)`.
            2. El servicio obtiene el `Paciente` desde el repositorio.
            3. Llama a `paciente.AgregarTurno(turnoNuevo)`.
            4. La entidad verifica si ya tiene un turno en esa fecha/hora.
            5. Si está todo bien, lo agrega a su lista interna `Turnos`.
            6. Luego el servicio guarda los cambios en la base de datos.
                
                Un paciente no puede tener dos turnos en el mismo horario.
                
    - `AgregarRegistroHistoria(HistoriaClinicaRegistro r)`: Agrega un registro a la historia clínica del paciente cuando un turno es cerrado.
        - Este debería ejecutarse cuando se cambia el estado de un turno a cerrado (diagmos que habrá un `TurnoServicio.CerrarTurno()`).
        - El turno genera el registro clínico (`turno.GenerarRegistroHistoria()`).
        - Luego el servicio obtiene el paciente y llama a `paciente.AgregarRegistroHistoria(registro)`
            
            Solo se agregan registros derivados de turnos válidos y cerrados.
            
    - `ObtenerTurnosPorEstado(EstadoTurno estado):List<Turno>`: Consultar los turnos del paciente filtrando por estado.
        - Se utilizará en la capa de negocio cuando se necesite filtrat turnos del paciente, pro ejemplo en la vista de detalle. Puede ser en reportes para estadísticas de ese paciente.
            
            Por ejemplo, el flujo puede ser:
              1. El `PacienteServicio.ObtenerDetallePaciente(id)` carga el paciente.
            
            1. Llama a `paciente.ObtenerTurnosPorEstado(EstadoTurno.Cerrado)` para mostrar el historial clínico.
            2. El resultado se mapea a DTOs para la UI y así los datos ya van formateados para la tabla.
    - `TieneTurnoEnHorario(DateTime fechaHora):bool`: Permite saber si el paciente ya tiene un turno registrado en un horario determinado. Se utiliza para validar la disponibilidad del paciente antes de crear un nuevo turno.
        - Se utilizará en la capa negocio en la validacion de nuevo turno: `TurnoServicio.AgendarTurno`, por ejemplo:
            - Recepcionista intenta crear turno → llama servicio.
            - Servicio obtiene paciente.
            - Llama `paciente.TieneTurnoEnHorario(fechaHoraSolicitada)`.
            - Si ya hay uno (true) → **rechaza** con mensaje “El paciente ya tiene un turno en ese horario” (excepcion de negocio).
            - Si no, se agenda el turno y se envía el email al paciente con el servicio de email.


---

### Medico : Persona

**Responsabilidad:** representar al profesional y su práctica.

- **Propiedades**
    - `Matricula:string`
    - `Especialidades:List<Especialidad>` (agregación)
    - `TurnosAsignados:List<Turno>` (composición)
- **Constructor**
    - Hereda de Persona + `(matricula)`
    - Constructor para nuevo Medico
    - Constructor para cargar un médico existente.
- **Métodos**
    - `AgregarEspecialidad(Especialidad esp)`: vincula un medico con una especialidad. (puede tener varias).
        - Se utilizaría en la capa de negocio al crear un médico en el servicio de Médicos o para editarlo y actualizar sus especialidades.
        - Por ejemplo:
            1. La UI (Admin o Recepcionista) abre el formulario de nuevo médico.
            2. El usuario selecciona una o varias especialidades.
            3. `MedicoServicio.CrearMedico(...)` recibe la lista de especialidades.
            4. Al construir el objeto `Medico`, el servicio itera y llama `medico.AgregarEspecialidad(especialidad)`
            5. La entidad `Medico` valida que **no haya duplicados** y la agrega a su colección `Especialidades`.
        - No se pueden agregar especialidades duplicadas.
        - Solo se agregan especialidades activas (`Estado = Activo`).
    - `ObtenerTurnosPorFecha(DateTime fecha):List<Turno>` : permite que el médico tenga todos los turnos asignados para un día específico. Con este podremos ver su agenda diaria.
        - Lo usaríamos en negocio, en el servicio de turnos o médicos cuando se genere el listado diario del médico. Para validar si tiene horarios libres en un día, se cargan los turnos del médico logueado. Por ejemplo:
            1. `TurnoServicio.ObtenerTurnosDelMedico(idMedico, fecha)`:
                - Carga la entidad `Medico`.
                - Llama a `medico.ObtenerTurnosPorFecha(fecha)`.
                - El dominio filtra la lista `TurnosAsignados` por día (comparando `Horario.Inicio.Date`).
            2. El servicio mapea esos turnos a `TurnoDto` y los devuelve a la UI.
        - Solo devuelve turnos que correspondan exactamente a esa fecha (día completo).
        - Puede filtrar también por estado (opcional)
    - `EstaDisponible(DateTime inicio, DateTime fin):bool` : Detrmina si el médico está disponible en un rango de horario determinado (chequea contra `TurnosAsignados`). Necesario para la asignación o reprogramación de turnos.
        - Se utilizaría en negocio dentro del servicio de turnos : `TurnoServicio.AgendarTurno()` o `TurnoServicio.ReprogramarTurno()`, por ejemplo:
            1. El servicio obtiene la entidad `Medico`.
            2. Llama a `medico.EstaDisponible(fechaInicio, fechaFin)`.
            3. Recorre la lista `TurnosAsignados`.
            4. Verifica si alguno se solapa con el rango inicio - fin. Usa el método `Turno.SolapaCon(otroTurno)` para determinar conflictos.
            5. Si devuelve false, lanza una `BusinessException` → “El médico no está disponible en ese horario”.
        - El médico no puede tener dos turnos que se superpongan en el tiempo.
        - Considera duración de los turnos (60 minutos por defecto o lo que diga `ParámetroSistema`).
        - Si un turno está cancelado, puede ignorarse en la comparación.
- **Reglas internas**
    - Matrícula requerida; especialidades no duplicadas.

---

## Catálogo clínico

### Especialidad

**Responsabilidad:** rama odontológica del médico.

- **Propiedades**
    - `Id:int`
    - `Nombre:string`
    - `Descripcion:string`
    - `TratamientosAsociados:List<Tratamiento>` (composición)
    - `Estado:EstadoEntidad`
- **Constructor**
    - constructor para crear la especialidad
    - constructor para cargar una especialidad existente.
- **Métodos**
    - `AgregarTratamiento(Tratamiento t)`: Permite asociar un nuevo tratamiento a la especialidad, asegurando que pertenezca correctamente a esa rama odontológica y evitando duplicaciones.
        - Se utilizaría en Negocio, dentro del servicio de Especialidades, cuando se da de alta o se edita una especialidad, y se asocian sus tratamientos. por ejemplo:
            1. La UI de administración de tratamientos permite agregar un nuevo tratamiento a una especialidad seleccionada.
            2. El `EspecialidadServicio` carga la entidad correspondiente desde el repositorio.
            3. Llama a `especialidad.AgregarTratamiento(tratamiento)` para asociarlo.
            4. La entidad valida que no exista otro tratamiento con el mismo nombre dentro de su lista.
            5. Si es válido, lo agrega a `TratamientosAsociados`.
        - No duplicar tratamientos con el mismo nombre.
        - Solo permitir agregar tratamientos activos.
        - Mantener la relación 1:N entre Especialidad y Tratamientos.
    - `EsDuplicadaDe(Especialidad otra):bool`: Permite comparar dos especialidades y determinar si representan la misma rama médica, evitando redundancias.
        - Se utilizaría en el servicio de especialidades al validar altas o ediciones de especialidades. Por ejemplo:
            1. El servicio recibe una nueva especialidad para registrar.
            2. Consulta las existentes en el repositorio.
            3. Llama a `especialidadExistente.EsDuplicadaDe(especialidadNueva)` para verificar coincidencia.
            4. Si devuelve true, se rechaza la creación.
        - Considera duplicada si el nombre coincide (ignorando mayúsculas/minúsculas).
---
### Tratamiento

**Responsabilidad:** procedimiento aplicable.

- **Propiedades**
    - `Id:int`
    - `Nombre:string`
    - `Descripcion:string`
    - `CostoBase:decimal`
    - `Especialidad:Especialidad` (composición: un tratamiento no puede existir sin una especialidad asociada )
    - `Estado:EstadoEntidad`
- **Constructor**
    - constructor para crear nuevo tratamiento.
    - constructor para cargar tratamiento existente.
- **Métodos**
    - `PerteneceA(Especialidad esp):bool`: permite saber si un tratamiento está correctamente asociado a una especialidad concreta.
        - Puede ser usado en el servicio de tratamiento antes de guardar o editar un tratamiento. Por ejemplo:
            1. Al crear o editar un tratamiento, el servicio invoca `tratamiento.PerteneceA(especialidadSeleccionada)`.
            2. El método compara la referencia interna de `Tratamiento.Especialidad` con la recibida.
            3. Devuelve verdadero si coinciden.
        - Un tratamiento no puede asignarse a una especialidad distinta de la declarada.
    - `CalcularCostoFinal(decimal porcentajeCobertura):decimal`: permite saber el costo final que debe abonar el paciente en función del porcentaje cubierto por su plan de cobertura.
        - Se usaría en el servicio de turnos, por ejemplo:
            
            al calcular el importe a cobrar o mostrar información en la UI sobre cuánto cubre la obra social.
            
            1. El servicio obtiene el tratamiento y el plan de cobertura del paciente.
            2. Invoca `tratamiento.CalcularCostoFinal(plan.PorcentajeCobertura)`.
            3. El método devuelve el monto total a pagar por el paciente.
        - El porcentaje debe estar entre 0 y 100.
        - Si el plan no aplica cobertura (porcentaje = 0), devuelve el costo base completo.
- **Reglas internas**
    - CostoBase mayor o igual que 0; Nombre requerido.

---

## Coberturas

### Cobertura

**Responsabilidad:** obra social o prepaga.

- **Propiedades**
    - `Id:int`
    - `Nombre:string`
    - `Descripcion:string`
    - `Planes:List<Plan>` (composición)
    - `Estado:EstadoEntidad`
- **Constructor (obligatorios)**
    - `(nombre)`
- **Métodos**
    - `AgregarPlan(Plan plan)`: Asociar uno o varios planes a una cobertura, asegurando que todos pertenezcan a la misma obra social y sin duplicaciones.
        - se usaría en negocio, dentro del servicio de coberturas, durante la creación o edición de una cobertura cuando se cargan los planes asociados.
            1. La UI permite crear una cobertura e ingresar sus planes en la misma operación.
            2. El servicio de cobertura crea la entidad Cobertura y por cada plan invoca `AgregarPlan(plan)`.
            3. El método valida que el plan no exista ya en la lista Planes.
            4. Si es válido, lo agrega a la colección.
        - No se pueden agregar dos planes con el mismo nombre dentro de una misma cobertura.
        - Solo se agregan planes activos.
        - Un plan no puede pertenecer a más de una cobertura.
    - `ExistePlan(string nombre):bool`: permite determinar si una cobertura ya contiene un plan con un nombre específico.
        - Se usaría en el servicio de cobertura, antes de agregar un nuevo plan o durante validaciones de integridad de datos.
            1. El servicio recibe una solicitud para agregar un nuevo plan a una cobertura.
            2. Invoca `cobertura.ExistePlan(nombrePlan)` para comprobar duplicidad.
            3. Si devuelve verdadero, la operación se rechaza.
        - Coincidencia de nombre (ignorando mayúsculas/minúsculas).
        - Garantiza unicidad de los planes dentro de una cobertura.
    - `BuscarPlanPorNombre(string nombre):Plan?`: permite recuperar un plan específico por su nombre dentro de la cobertura.
        - Se puede usar en el servicio de pacientes o de turnos cuando se necesita obtener el plan exacto asociado a un paciente o calcular cobertura en un turno.
            1. El servicio obtiene la cobertura del paciente.
            2. Llama a `BuscarPlanPorNombre(nombre)` para identificar el plan.
            3. Devuelve la instancia del plan si existe, o nada en caso contrario.
        - Devuelve un único resultado, sin generar listas.
        - El nombre debe coincidir con un plan existente dentro de la colección `Planes`.
- **Reglas internas**
    - Nombre único a nivel negocio (validado afuera).
---
### Plan

**Responsabilidad:** plan de cobertura con % de reintegro/cobertura.

- **Propiedades**
    - `Id:int`
    - `Nombre:string`
    - `Descripcion:string`
    - `PorcentajeCobertura:decimal` (0–100 %)
    - `Estado:EstadoEntidad`
    - `Cobertura:Cobertura` (composición, un plan no existe sin cobertura)
- **Constructor**
    - constructor par acreae nuevo plan
    - constructor para cargar plan existente.
- **Métodos**
    - `AplicarCobertura(decimal montoTratamiento):decimal`: permite calcular el valor que el paciente debe pagar según el porcentaje de cobertura del plan.
        - Se usaría en el negocio, en el servico de turno, por ejemplo:
            1. El servicio obtiene el tratamiento y su costo base.
            2. Consulta el plan asociado al paciente.
            3. Llama a `plan.AplicarCobertura(tratamiento.CostoBase)`.
            4. El método calcula y devuelve el monto restante a abonar por el paciente.
        - El porcentaje de cobertura debe estar entre 0 y 100.
        - Si el porcentaje es 100, el costo final es 0.
        - Si el porcentaje es 0, el paciente abona el total del tratamiento.
    - `EstaActivo():bool`: permite determinar si el plan se encuentra activo y puede ser utilizado.
        - Se usaría en el servicio de coberturas, pacientes y turnos para validar si un plan puede ser asignado o usado en un turno.
            1. Antes de asignar el plan a un paciente o usarlo en un turno, el servicio invoca `plan.EstaActivo()`.
            2. Si devuelve false, el sistema impide la operación.
        - Verifica el estado interno Estado = Activo.
        - Evita el uso de planes inactivos o dados de baja lógica.
- **Reglas internas**
    - Porcentaje entre 0 y 100; Nombre requerido.

---

## Turnos e historia clínica

### Turno

**Responsabilidad:** cita entre paciente y médico, con tratamiento y estado.

- **Propiedades**
    - `Id:int`
    - `Paciente:Paciente`
    - `Medico:Medico`
    - `Especialidad:Especialidad`
    - `Tratamiento:Tratamiento`
    - `Horario:HorarioTurno`
    - `Cobertura:Cobertura`
    - `Plan:Plan` *(*opcional ya que los Particulares no tienen planes)
    - `Estado:EstadoTurno`
    - `Observaciones:string`
- **Constructor**
    - constructor para crear un turno.
    - constructor para cargar un turno existente.
- **Métodos**
    - `CambiarEstado(EstadoTurno nuevo)`: permite modificar el estado del turno (nuevo, reprogramado, cancelado, no asistió o cerrado).
        - Se utilizaría en negocio, dentro del servicio de turnos, al ejecutar operaciones que afectan el estado del turno, como cancelación, reprogramación o cierre, pro ejemplo:
            1. El servicio obtiene el turno desde el repositorio.
            2. Invoca `turno.CambiarEstado(nuevoEstado)`.
            3. El método valida si la transición de estado es válida.
            4. Si lo es, actualiza el campo `Estado` y la fecha de modificación.
        - Solo se permiten transiciones válidas según la lógica del sistema (por ejemplo, de Nuevo a Reprogramado o Cancelado, pero no de Cerrado a Nuevo).
        - Un turno con estado Cerrado no puede volver a abrirse.
        - Un turno Cancelado no puede ser marcado como Cerrado.
    - `SolapaCon(Turno otro):bool` : permite saber si el turno actual se solapa en horario con otro turno, sea del mismo médico o del mismo paciente. (usa HorarioTurno).
        - Se utilizaría en el servicio de turnos, dentro de las validaciones de disponibilidad al agendar o reprogramar un turno.
            1. El servicio obtiene la lista de turnos activos del médico y del paciente.
            2. Por cada turno existente, invoca `turnoNuevo.SolapaCon(turnoExistente)`.
            3. El método compara los rangos horarios (inicio y fin).
            4. Devuelve true si los intervalos se superponen.
        - Dos turnos no pueden ocupar el mismo rango horario.
        - Se ignoran turnos cancelados al evaluar solapamientos.
        - Evitar agendar dos turnos para el mismo médico en la misma hora.
        - Impedir que un paciente tenga citas superpuestas.
    - `EsCancelable():bool`: permite conocer si el turno puede ser cancelado, según su estado actual y la política del sistema ( si no está Cerrado).
        - Se utilizaría en TurnoServicio.CancelarTurno o en la UI, para habilitar o no el botón de  cancelar, por ejemplo:
            1. El servicio o la UI consultan `turno.EsCancelable()`.
            2. El método evalúa el estado actual.
            3. Devuelve `true` si el turno está en estado *Nuevo* o *Reprogramado*; en caso contrario, `false`.
        - Turnos cerrados o con atención finalizada no se pueden cancelar.
        - Turnos ya cancelados o con estado No asistió tampoco se pueden cancelar nuevamente.
    - `GenerarRegistroHistoria():HistoriaClinicaRegistro` : permite crear un nuevo registro de historia clínica basado en la información del turno cerrado (solo si Estado=Cerrado).
        - se usaría en `TurnoServicio.CerrarTurno`, inmediatamente después de cambiar el estado a Cerrado.
            1. El médico o el sistema cambian el estado del turno a *Cerrado*.
            2. `TurnoServicio` invoca `turno.GenerarRegistroHistoria()`.
            3. El método construye una nueva instancia de `HistoriaClinicaRegistro`, tomando datos del turno (paciente, médico, especialidad, tratamiento, observaciones, diagnóstico).
            4. Devuelve el registro para que sea agregado al historial del paciente mediante `paciente.AgregarRegistroHistoria()`.
        - Solo se puede generar un registro si el turno está Cerrado.
        - Un turno solo puede generar un registro de historia una vez.
        - Automatizar la creación de la historia clínica al cerrar la atención.
        - Registrar observaciones y diagnóstico como parte del seguimiento del paciente.
- **Reglas internas**
    - No permitir generar registro historia si no está `Cerrado`.
---
### HistoriaClinicaRegistro

**Responsabilidad:** registro clínico derivado del cierre de un turno.

- **Propiedades**
    - `Id:int`
    - `FechaAtencion:DateTime`
    - `Medico:Medico`
    - `Paciente:Paciente`
    - `Especialidad:Especialidad`
    - `TratamientoAplicado:Tratamiento`
    - `Diagnostico:string`
    - `Observaciones:string`
    - `TurnoOrigen:Turno`
- **Constructor (obligatorios)**
    - constructor vacío para inicializar vacío cuando aún no existe un registro
    - constructor para crear un nuevo registro
    - constructor para cargar un registro existente.
- **Métodos**
    - `Resumen():string`: permite devolver  una descripción resumida del registro clínico, útil para mostrar en listados o reportes.
        - Se usaría en el servicio de pacientes o de reportes, al mostrar la historia clínica del paciente o generar reportes.
            1. La UI solicita el historial del paciente.
            2. El servicio obtiene los registros desde el repositorio o desde la entidad Paciente.
            3. Por cada registro, se llama a `registro.Resumen()`.
            4. El método concatena la fecha, tratamiento, médico y diagnóstico en un texto corto.
        - Listado de historia clínica dentro del detalle del paciente.
        - Exportación de reportes médicos o resúmenes de atención.
    - `EsDelMismoPaciente(Paciente p):bool`: Verifica si el registro pertenece al paciente indicado.
        - Se utilizaría en `e`l servicio de pacientes o de reportes, cuando se filtran registros clínicos por paciente, por ejemplo:
            1. El servicio obtiene un conjunto de registros clínicos.
            2. Llama a `registro.EsDelMismoPaciente(pacienteActual)` para filtrar los que corresponden.
            3. Devuelve solo los registros asociados al paciente solicitado.
        - La comparación se hace por identidad del paciente (Id o referencia en memoria).
        - Validar consistencia de la historia clínica al generar listados.
        - Evitar que un registro clínico aparezca en el historial de otro paciente.
- **Reglas internas**
    - Debe existir un `TurnoOrigen` con `Estado=Cerrado`.

---

## Seguridad

### Usuario

**Responsabilidad:** representa a quien inicia sesión y actúa en el sistema.

- **Propiedades**
    - `Id:int`
    - `Nombre:string`
    - `Apellido:string`
    - `Email:Email`
    - `NombreUsuario:string`
    - `PasswordHash:string`
    - `Rol:Rol`
    - `Estado:EstadoEntidad`
    - `FechaAlta:DateTime`
    - `FechaModificacion:DateTime?`
- **Constructor**
    - constructor para crear un nuevo usuario.
    - constructor para cargar usuario existente.
- **Métodos**
    - `NombreCompleto():string`: Retorna el nombre completo del usuario en formato Apellido, Nombre, utilizado para mostrar información del usuario logueado en la UI.
        - se utilizaría en Negocio y en la UI, principalmente en encabezados, menús o reportes donde se necesita identificar al usuario que ejecuta una acción.
            1. La UI obtiene el usuario activo desde el contexto de sesión.
            2. Llama a `usuario.NombreCompleto()`.
            3. El método concatena los campos Apellido y Nombre.
        - Ambos campos deben tener valores válidos.
        - Mostrar “Pérez, Juan” en la barra superior.
        - Imprimir el nombre del usuario en reportes.
    - `EsAdministrador():bool` / `EsMedico():bool` / `EsRecepcionista():bool`: Verificar el rol asignado al usuario para determinar permisos y accesos a módulos.
        - Se usaría en `e`l servicio de Autenticación y y Autorización para aplicar control de acceso; en la UI, para mostrar u ocultar secciones del menú.
            1. Durante la autenticación, el sistema carga el usuario con su rol asociado.
            2. En cada acción o vista, el sistema llama al método correspondiente (por ejemplo, `usuario.EsAdministrador()`).
            3. Devuelve true si el nombre del rol coincide con el esperado.
        - Cada usuario tiene exactamente un rol asignado.
        - El nombre del rol debe coincidir con los predefinidos (Administrador, Médico, Recepcionista).
        - Control de acceso a páginas y operaciones.
        - Ocultar o mostrar funcionalidades específicas en la UI según el tipo de usuario.
- **Reglas internas**
    - Email válido; rol requerido.
---
### Rol

**Responsabilidad:** agrupar permisos por perfil.

- **Propiedades**
    - `Id:int`
    - `Nombre:string` *(Administrador, Recepcionista, Médico)*
    - `Descripcion:string`
    - `Permisos:List<Permiso>`
    - `Estado:EstadoEntidad`
- **Constructor**
    - constructor para crear un rol
    - constructor para cargar rol existente
- **Métodos**
    - `AgregarPermiso(Permiso p)` / `QuitarPermiso(Permiso p)`: asocia o elimina un permiso al rol, garantizando que no existan duplicados y que el permiso sea válido.
        - se usaría en Negocio, dentro del servicio de roles, al crear o editar un rol.
            1. El administrador crea o modifica un rol en la UI.
            2. El servicio de negocio recibe la lista de permisos seleccionados.
            3. Por cada permiso, invoca `rol.AgregarPermiso(p)`.
            4. El método verifica si ya existe uno igual en la lista antes de agregarlo.
        - No se pueden agregar permisos duplicados (mismo módulo y acción).
        - Solo se pueden agregar permisos activos.
        - Configurar roles personalizados.
        - Gestionar permisos de nuevos módulos sin duplicación.
    - `TienePermiso(string modulo, string accion):bool`: permite saber si el rol tiene permiso para ejecutar una acción sobre un módulo concreto.
        - Se usaría en srvicio de autorización, cuando el sistema valida si el usuario puede acceder a una página o ejecutar una operación, por ejemplo:
            1. El servicio de autorización recibe la solicitud de acción (modulo, accion).
            2. Llama a `rol.TienePermiso(modulo, accion)`.
            3. El método busca en la colección de permisos del rol un permiso coincidente.
            4. Devuelve true si lo encuentra, de lo contrario false.
        - La comparación debe ser exacta por módulo y acción.
        - Solo se consideran permisos con estado activo.
        - Validar accesos antes de cargar una vista o ejecutar una operación.
        - Controlar visibilidad de botones o enlaces según el rol del usuario.
- **Reglas internas**
    - Nombre requerido; no duplicar permisos.
---
### Permiso

**Responsabilidad:** capacidad concreta sobre módulo/acción.

- **Propiedades**
    - `Id:int`
    - `Modulo:string` *(Pacientes, Médicos, Turnos, etc.)*
    - `Accion:string` *(Ver, Crear, Editar, Desactivar, CambiarEstado, Exportar, Configurar)*
    - `Descripcion:string`
- **Constructor (obligatorios)**
    - `(modulo, accion)`
- **Métodos**
    - `Clave():string` : permite crear una representación única del permiso combinando el nombre del módulo y la acción, facilitando las búsquedas o comparaciones como Turnos.Crear.
        - se usaría en el servicio de autorización y roles para identificar permisos dentro de colecciones y validar accesos.
        1. Al agregarse o buscarse un permiso, el sistema llama a `permiso.Clave()`.
        2. El método concatena el módulo y la acción, separados por un punto (por ejemplo, “Turnos.Crear”).
        3. Se utiliza esa clave para comparar o indexar permisos.
        - Los valores de módulo y acción deben existir.
        - La combinación debe ser única dentro del conjunto de permisos.
        - Definir claves simples para control de acceso.
        - Evitar duplicados en listas de permisos de roles.

---

## Configuración del sistema

### ParametroSistema

**Responsabilidad:** parámetros configurables por Admin.

- **Propiedades**
    - `Id:int`
    - `Nombre:string`
    - `Valor:string`
    - `Descripcion:string`
- **Constructor (obligatorios)**
    - `(nombre, valor)`
- **Ejemplos**
    - `DuracionTurnoMinutos = 60`
    - `SMTP_Server`, `SMTP_Port`, `Email_From`