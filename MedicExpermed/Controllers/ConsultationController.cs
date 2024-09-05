using MedicExpermed.Models;
using MedicExpermed.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;

namespace MedicExpermed.Controllers
{
    public class ConsultationController : Controller
    {
        private readonly AppointmentService _citaService;
        private readonly PatientService _patientService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ConsultationService _consultationService;
        private readonly CatalogService _catalogService;
        private readonly ILogger<ConsultationController> _logger;
        private readonly medicossystembdIContext _medical_SystemContext;

        public ConsultationController(AppointmentService citaService, PatientService patientService, IHttpContextAccessor httpContextAccessor, ConsultationService consultationService, CatalogService catalogService, ILogger<ConsultationController> logger, medicossystembdIContext medical_SystemContext)
        {
            _citaService = citaService;
            _patientService = patientService;
            _httpContextAccessor = httpContextAccessor;
            _consultationService = consultationService;
            _catalogService = catalogService;
            _logger = logger;
            _medical_SystemContext = medical_SystemContext;
        }

        private async Task CargarListasDesplegables()
        {
            ViewBag.TiposDocumentos = await _catalogService.ObtenerTiposDocumentoAsync();
            ViewBag.TiposSangre = await _catalogService.ObtenerTiposSangreAsync();
            ViewBag.TiposGenero = await _catalogService.ObtenerTiposGeneroAsync();
            ViewBag.TiposEstadoCivil = await _catalogService.ObtenerTiposEstadoCivilAsync();
            ViewBag.TiposFormacion = await _catalogService.ObtenerTiposFormacionAsync();
            ViewBag.TiposNacionalidad = await _catalogService.ObtenerTiposDeNacionalidadPAsync();
            ViewBag.TiposProvincia = await _catalogService.ObtenerTiposDeProvinciaPAsync();
            ViewBag.TiposSeguro = await _catalogService.ObtenerTiposSeguroSaludAsync();
            ViewBag.TiposPariente = await _catalogService.ObtenerTiposParentescoAsync();
            ViewBag.TiposAlergias = await _catalogService.ObtenerAlergiasAsync();
            ViewBag.TiposCirugias = await _catalogService.ObtenerCirugiasAsync();
            ViewBag.TiposParienteAntece = await _catalogService.ObtenerAntecedentesFAsync();
            ViewBag.TiposDiagnostico = await _catalogService.ObtenerDiagnosticosActivasAsync();
            ViewBag.TiposMedicamentos = await _catalogService.ObtenerMedicamentosActivasAsync();
            ViewBag.TiposLaboratorios = await _catalogService.ObtenerLaboratorioActivasAsync();
            ViewBag.TiposImagen = await _catalogService.ObtenerImagenActivasAsync();
        }

        [HttpGet("Listar-Consultas")]
        public async Task<IActionResult> ListarConsultas()
        {
            try
            {
                var consultas = await _consultationService.GetAllConsultasAsync();
                ViewBag.UsuarioEspecialidad = HttpContext.Session.GetString("UsuarioEspecialidad");

                return View(consultas);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<Consultum>());
            }
        }

        [HttpGet("Crear-Consulta")]
        public async Task<IActionResult> CrearConsulta()
        {
            int ningunTipoParienteId = await _catalogService.ObtenerIdParentescoNingunoAsync();

            ViewBag.UsuarioNombre = HttpContext.Session.GetString("UsuarioNombre");
            ViewBag.UsuarioId = HttpContext.Session.GetInt32("UsuarioId");
            ViewBag.UsuarioIdEspecialidad = HttpContext.Session.GetInt32("UsuarioIdEspecialidad");

            await CargarListasDesplegables();

            var model = new Consultation
            {
                TipoPariente = ningunTipoParienteId,
            };

          

            return View(model);
        }

        [HttpPost("Crear-Consulta")]
        public IActionResult CrearConsulta([FromBody] ConsultationDto consultaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Mapeamos el DTO a la entidad Consultation que el servicio espera
                var consultation = new Consultation
                {
                    UsuarioCreacion = consultaDto.UsuarioCreacion,
                    Historial = consultaDto.Historial,
                    PacienteId = consultaDto.PacienteId,
                    Motivo = consultaDto.Motivo,
                    Enfermedad = consultaDto.Enfermedad,
                    NombrePariente = consultaDto.NombrePariente,
                    SignosAlarma = consultaDto.SignosAlarma,
                    ReconoFarmacologicas = consultaDto.ReconoFarmacologicas,
                    TipoPariente = consultaDto.TipoPariente,
                    TelefonoPariente = consultaDto.TelefonoPariente,
                    Temperatura = consultaDto.Temperatura,
                    FrecuenciaRespiratoria = consultaDto.FrecuenciaRespiratoria,
                    PresionArterialSistolica = consultaDto.PresionArterialSistolica,
                    PresionArterialDiastolica = consultaDto.PresionArterialDiastolica,
                    Pulso = consultaDto.Pulso,
                    Peso = consultaDto.Peso,
                    Talla = consultaDto.Talla,
                    PlanTratamiento = consultaDto.PlanTratamiento,
                    Observacion = consultaDto.Observacion,
                    AntecedentesPersonales = consultaDto.AntecedentesPersonales,
                    DiasIncapacidad = consultaDto.DiasIncapacidad,
                    MedicoId = consultaDto.MedicoId,
                    EspecialidadId = consultaDto.EspecialidadId,
                    TipoConsulta = consultaDto.TipoConsulta,
                    NotasEvolucion = consultaDto.NotasEvolucion,
                    ConsultaPrincipal = consultaDto.ConsultaPrincipal,
                    EstadoConsulta = consultaDto.EstadoConsulta,

                    OrgSentidos = consultaDto.OrgSentidos,
                    ObserOrgSentidos = consultaDto.ObserOrgSentidos,
                    Respiratorio = consultaDto.Respiratorio,
                    ObserRespiratorio = consultaDto.ObserRespiratorio,
                    CardioVascular = consultaDto.CardioVascular,
                    ObserCardioVascular = consultaDto.ObserCardioVascular,
                    Digestivo = consultaDto.Digestivo,
                    ObserDigestivo = consultaDto.ObserDigestivo,
                    Genital = consultaDto.Genital,
                    ObserGenital = consultaDto.ObserGenital,
                    Urinario = consultaDto.Urinario,
                    ObserUrinario = consultaDto.ObserUrinario,
                    MEsqueletico = consultaDto.MEsqueletico,
                    ObserMEsqueletico = consultaDto.ObserMEsqueletico,
                    Endocrino = consultaDto.Endocrino,
                    ObserEndocrino = consultaDto.ObserEndocrino,
                    Linfatico = consultaDto.Linfatico,
                    ObserLinfatico = consultaDto.ObserLinfatico,
                    Nervioso = consultaDto.Nervioso,
                    ObserNervioso = consultaDto.ObserNervioso,

                    Cabeza = consultaDto.Cabeza,
                    ObserCabeza = consultaDto.ObserCabeza,
                    Cuello = consultaDto.Cuello,
                    ObserCuello = consultaDto.ObserCuello,
                    Torax = consultaDto.Torax,
                    ObserTorax = consultaDto.ObserTorax,
                    Abdomen = consultaDto.Abdomen,
                    ObserAbdomen = consultaDto.ObserAbdomen,
                    Pelvis = consultaDto.Pelvis,
                    ObserPelvis = consultaDto.ObserPelvis,
                    Extremidades = consultaDto.Extremidades,
                    ObserExtremidades = consultaDto.ObserExtremidades,

                    // Parámetros tipo tabla
                    Alergias = CreateDataTable(consultaDto.Alergias),
                    Cirugias = CreateDataTable(consultaDto.Cirugias),
                    Medicamentos = CreateDataTable(consultaDto.Medicamentos),
                    Laboratorio = CreateDataTable(consultaDto.Laboratorio),
                    Imagenes = CreateDataTable(consultaDto.Imagenes),
                    Diagnosticos = CreateDataTable(consultaDto.Diagnosticos)
                };

                // Llamada al servicio que ejecuta el procedimiento almacenado
                _consultationService.CreateConsultation(consultation);

                return Ok("Consulta creada exitosamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la consulta.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        // Método auxiliar para convertir listas a DataTable
        private DataTable CreateDataTable<T>(List<T> items)
        {
            DataTable table = new DataTable();
            if (items == null || items.Count == 0)
                return table;

            // Crear columnas basadas en las propiedades del objeto
            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            // Llenar el DataTable con datos
            foreach (var item in items)
            {
                var row = table.NewRow();
                foreach (var prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }

        [HttpGet("Buscar")]
        public async Task<IActionResult> BuscarPacientes(int? cedula, string primerNombre, string primerApellido)
        {
            if (cedula == null && string.IsNullOrEmpty(primerNombre) && string.IsNullOrEmpty(primerApellido))
            {
                return BadRequest("Debe proporcionar al menos un criterio de búsqueda: cédula, primer nombre o primer apellido.");
            }

            var pacientes = await _consultationService.BuscarPacientesAsync(cedula, primerNombre, primerApellido);

            if (pacientes == null || !pacientes.Any())
            {
                return NotFound("No se encontraron pacientes con los criterios proporcionados.");
            }

            return Ok(pacientes);
        }
    }

    // DTO (Data Transfer Object) que se espera recibir en la solicitud HTTP
    public class ConsultationDto
    {
        public string UsuarioCreacion { get; set; }
        public string Historial { get; set; }
        public int PacienteId { get; set; }
        public string Motivo { get; set; }
        public string Enfermedad { get; set; }
        public string NombrePariente { get; set; }
        public string SignosAlarma { get; set; }
        public string ReconoFarmacologicas { get; set; }
        public int TipoPariente { get; set; }
        public string TelefonoPariente { get; set; }
        public string Temperatura { get; set; }
        public string FrecuenciaRespiratoria { get; set; }
        public string PresionArterialSistolica { get; set; }
        public string PresionArterialDiastolica { get; set; }
        public string Pulso { get; set; }
        public string Peso { get; set; }
        public string Talla { get; set; }
        public string PlanTratamiento { get; set; }
        public string Observacion { get; set; }
        public string AntecedentesPersonales { get; set; }
        public int DiasIncapacidad { get; set; }
        public int MedicoId { get; set; }
        public int EspecialidadId { get; set; }
        public int TipoConsulta { get; set; }
        public string NotasEvolucion { get; set; }
        public string ConsultaPrincipal { get; set; }
        public int EstadoConsulta { get; set; }

        // Otros parámetros adicionales (organos_sistemas, examen_fisico, etc.)
        public bool OrgSentidos { get; set; }
        public string ObserOrgSentidos { get; set; }
        public bool Respiratorio { get; set; }
        public string ObserRespiratorio { get; set; }
        public bool CardioVascular { get; set; }
        public string ObserCardioVascular { get; set; }
        public bool Digestivo { get; set; }
        public string ObserDigestivo { get; set; }
        public bool Genital { get; set; }
        public string ObserGenital { get; set; }
        public bool Urinario { get; set; }
        public string ObserUrinario { get; set; }
        public bool MEsqueletico { get; set; }
        public string ObserMEsqueletico { get; set; }
        public bool Endocrino { get; set; }
        public string ObserEndocrino { get; set; }
        public bool Linfatico { get; set; }
        public string ObserLinfatico { get; set; }
        public bool Nervioso { get; set; }
        public string ObserNervioso { get; set; }

        // Parámetros para examen físico
        public bool Cabeza { get; set; }
        public string ObserCabeza { get; set; }
        public bool Cuello { get; set; }
        public string ObserCuello { get; set; }
        public bool Torax { get; set; }
        public string ObserTorax { get; set; }
        public bool Abdomen { get; set; }
        public string ObserAbdomen { get; set; }
        public bool Pelvis { get; set; }
        public string ObserPelvis { get; set; }
        public bool Extremidades { get; set; }
        public string ObserExtremidades { get; set; }

        // Parámetros tipo tabla (listas de objetos)
        public List<ConsultaAlergia> Alergias { get; set; }
        public List<ConsultaCirugia> Cirugias { get; set; }
        public List<ConsultaMedicamento> Medicamentos { get; set; }
        public List<ConsultaLaboratorio> Laboratorio { get; set; }
        public List<ConsultaImagen> Imagenes { get; set; }
        public List<ConsultaDiagnostico> Diagnosticos { get; set; }
    }
}
