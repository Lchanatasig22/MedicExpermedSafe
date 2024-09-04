using MedicExpermed.Models;
using MedicExpermed.Services;
using Microsoft.AspNetCore.Mvc;

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
        /// <summary>
        /// Listar todas las citas
        /// </summary>
        /// <returns></returns>
        [HttpGet("Listar-Consultas")]
        public async Task<IActionResult> ListarConsultas()
        {
            try
            {
                // Llama al servicio para obtener todas las consultas
                var consultas = await _consultationService.GetAllConsultasAsync();
                ViewBag.UsuarioEspecialidad = HttpContext.Session.GetString("UsuarioEspecialidad");

                // Pasa el modelo a la vista
                return View(consultas);
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que ocurra y enviar un mensaje de error a la vista
                ViewBag.Error = ex.Message;
                return View(new List<Consultum>()); // Devolver una lista vacía para evitar el error de null
            }
        }

        //Vista Crear Consulta
        [HttpGet("Crear-Consulta")]
        public async Task<IActionResult> CrearConsulta()
        {
            int ningunTipoParienteId = await _catalogService.ObtenerIdParentescoNingunoAsync();
            //int ningunTipoAlergiaId = await _catalogService.ObtenerIdAlergiasNingunoAsync();
            //int ningunTipoCirugiasId = await _catalogService.ObtenerIdCirugiasNingunoAsync();
     
            ViewBag.UsuarioNombre = HttpContext.Session.GetString("UsuarioNombre");
            ViewBag.UsuarioId = HttpContext.Session.GetInt32("UsuarioId");
            ViewBag.UsuarioIdEspecialidad = HttpContext.Session.GetInt32("UsuarioIdEspecialidad");


            await CargarListasDesplegables();
            var model = new Consultation()


            {

               TipoParienteID = ningunTipoParienteId,
                //AlergiasID = new List<int> { ningunTipoAlergiaId }, // Corregido para asignar una lista
                //CirugiasID = new List<int> { ningunTipoCirugiasId }, // Corregido para asignar una lista

            };

            if (model.ConsultaAntecedentesFamiliares == null)
            {
                model.ConsultaAntecedentesFamiliares = new AntecedentesFamiliare();
            }
            return View(model);
        }



        [HttpPost("Crear-Consulta")]
        public async Task<IActionResult> CrearConsulta([FromBody] Consultation consultaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (idConsulta, secuencial) = await _consultationService.CreateConsultationAsync(consultaDto);

            if (idConsulta == 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al crear la consulta.");
            }

            return Ok(new { IdConsulta = idConsulta, Secuencial = secuencial });
        }



        /// <summary>
        /// Metodo para buscar paciente
        /// </summary>
        /// <param name="cedula"></param>
        /// <param name="primerNombre"></param>
        /// <param name="primerApellido"></param>
        /// <returns></returns>
        [HttpGet("Buscar")]
        public async Task<IActionResult> BuscarPacientes(int? cedula, string primerNombre, string primerApellido)
        {
            // Verifica que al menos un parámetro fue proporcionado
            if (cedula == null && string.IsNullOrEmpty(primerNombre) && string.IsNullOrEmpty(primerApellido))
            {
                return BadRequest("Debe proporcionar al menos un criterio de búsqueda: cédula, primer nombre o primer apellido.");
            }

            // Llama al servicio para buscar pacientes usando lógica de OR
            var pacientes = await _consultationService.BuscarPacientesAsync(cedula, primerNombre, primerApellido);

            if (pacientes == null || !pacientes.Any())
            {
                return NotFound("No se encontraron pacientes con los criterios proporcionados.");
            }

            return Ok(pacientes);
        }



    }
}
