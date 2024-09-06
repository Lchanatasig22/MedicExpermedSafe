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
                FechaCreacion = DateTime.Now,
            };

          

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CrearConsulta(
               string usuariocreacionConsulta,
               string historialConsulta,
               int pacienteConsultaP,
               string motivoConsulta,
               string enfermedadConsulta,
               string nombreparienteConsulta,
               string signosalarmaConsulta,
               string reconofarmacologicas,
               int tipoparienteConsulta,
               string telefonoParienteConsulta,
               string temperaturaConsulta,
               string frecuenciarespiratoriaConsulta,
               string presionarterialsistolicaConsulta,
               string presionarterialdiastolicaConsulta,
               string pulsoConsulta,
               string pesoConsulta,
               string tallaConsulta,
               string plantratamientoConsulta,
               string observacionConsulta,
               string antecedentespersonalesConsulta,
               int diasincapacidadConsulta,
               int medicoConsultaD,
               int especialidadId,
               int tipoConsultaC,
               string notasevolucionConsulta,
               string consultaprincipalConsulta,
               int estadoConsultaC,
               // Parámetros de órganos y sistemas
               bool orgSentidos,
               string obserOrgSentidos,
               bool respiratorio,
               string obserRespiratorio,
               bool cardioVascular,
               string obserCardioVascular,
               bool digestivo,
               string obserDigestivo,
               bool genital,
               string obserGenital,
               bool urinario,
               string obserUrinario,
               bool mEsqueletico,
               string obserMEsqueletico,
               bool endocrino,
               string obserEndocrino,
               bool linfatico,
               string obserLinfatico,
               bool nervioso,
               string obserNervioso,
               // Parámetros de examen físico
               bool cabeza,
               string obserCabeza,
               bool cuello,
               string obserCuello,
               bool torax,
               string obserTorax,
               bool abdomen,
               string obserAbdomen,
               bool pelvis,
               string obserPelvis,
               bool extremidades,
               string obserExtremidades,
               // Parámetros de antecedentes familiares
               bool cardiopatia,
               string obserCardiopatia,
               int parentescocatalogoCardiopatia,
               bool diabetes,
               string obserDiabetes,
               int parentescocatalogoDiabetes,
               bool enfCardiovascular,
               string obserEnfCardiovascular,
               int parentescocatalogoEnfCardiovascular,
               bool hipertension,
               string obserHipertension,
               int parentescocatalogoHipertension,
               bool cancer,
               string obserCancer,
               int parentescocatalogoCancer,
               bool tuberculosis,
               string obserTuberculosis,
               int parentescocatalogoTuberculosis,
               bool enfMental,
               string obserEnfMental,
               int parentescocatalogoEnfMental,
               bool enfInfecciosa,
               string obserEnfInfecciosa,
               int parentescocatalogoEnfInfecciosa,
               bool malFormacion,
               string obserMalFormacion,
               int parentescocatalogoMalFormacion,
               bool otro,
               string obserOtro,
               int parentescocatalogoOtro,
               // Listas de alergias, cirugías, etc.
               List<ConsultaAlergia> alergias,
               List<ConsultaCirugia> cirugias,
               List<ConsultaMedicamento> medicamentos,
               List<ConsultaLaboratorio> laboratorios,
               List<ConsultaImagen> imagenes,
               List<ConsultaDiagnostico> diagnosticos)
        {
            try
            {
                // Llamar al servicio para crear la consulta
                await _consultationService.CrearConsultaAsync(
                    usuariocreacionConsulta,
                    historialConsulta,
                    pacienteConsultaP,
                    motivoConsulta,
                    enfermedadConsulta,
                    nombreparienteConsulta,
                    signosalarmaConsulta,
                    reconofarmacologicas,
                    tipoparienteConsulta,
                    telefonoParienteConsulta,
                    temperaturaConsulta,
                    frecuenciarespiratoriaConsulta,
                    presionarterialsistolicaConsulta,
                    presionarterialdiastolicaConsulta,
                    pulsoConsulta,
                    pesoConsulta,
                    tallaConsulta,
                    plantratamientoConsulta,
                    observacionConsulta,
                    antecedentespersonalesConsulta,
                    diasincapacidadConsulta,
                    medicoConsultaD,
                    especialidadId,
                    tipoConsultaC,
                    notasevolucionConsulta,
                    consultaprincipalConsulta,
                    estadoConsultaC,
                    orgSentidos,
                    obserOrgSentidos,
                    respiratorio,
                    obserRespiratorio,
                    cardioVascular,
                    obserCardioVascular,
                    digestivo,
                    obserDigestivo,
                    genital,
                    obserGenital,
                    urinario,
                    obserUrinario,
                    mEsqueletico,
                    obserMEsqueletico,
                    endocrino,
                    obserEndocrino,
                    linfatico,
                    obserLinfatico,
                    nervioso,
                    obserNervioso,
                    cabeza,
                    obserCabeza,
                    cuello,
                    obserCuello,
                    torax,
                    obserTorax,
                    abdomen,
                    obserAbdomen,
                    pelvis,
                    obserPelvis,
                    extremidades,
                    obserExtremidades,
                    cardiopatia,
                    obserCardiopatia,
                    parentescocatalogoCardiopatia,
                    diabetes,
                    obserDiabetes,
                    parentescocatalogoDiabetes,
                    enfCardiovascular,
                    obserEnfCardiovascular,
                    parentescocatalogoEnfCardiovascular,
                    hipertension,
                    obserHipertension,
                    parentescocatalogoHipertension,
                    cancer,
                    obserCancer,
                    parentescocatalogoCancer,
                    tuberculosis,
                    obserTuberculosis,
                    parentescocatalogoTuberculosis,
                    enfMental,
                    obserEnfMental,
                    parentescocatalogoEnfMental,
                    enfInfecciosa,
                    obserEnfInfecciosa,
                    parentescocatalogoEnfInfecciosa,
                    malFormacion,
                    obserMalFormacion,
                    parentescocatalogoMalFormacion,
                    otro,
                    obserOtro,
                    parentescocatalogoOtro,
                    alergias,
                    cirugias,
                    medicamentos,
                    laboratorios,
                    imagenes,
                    diagnosticos
                );

                // Redirigir o devolver un resultado exitoso
                return RedirectToAction("ConsultaCreada"); // Ajusta según la lógica que necesites
            }
            catch (Exception ex)
            {
                // Manejo de errores
                ModelState.AddModelError(string.Empty, $"Error al crear la consulta: {ex.Message}");
                return View(); // Devuelve la vista con los errores, si es necesario
            }
        }




        [HttpGet("Editar-Consulta")]
        public async Task<IActionResult> EditarConsulta()
        {
            int ningunTipoParienteId = await _catalogService.ObtenerIdParentescoNingunoAsync();

            ViewBag.UsuarioNombre = HttpContext.Session.GetString("UsuarioNombre");
            ViewBag.UsuarioId = HttpContext.Session.GetInt32("UsuarioId");
            ViewBag.UsuarioIdEspecialidad = HttpContext.Session.GetInt32("UsuarioIdEspecialidad");

            await CargarListasDesplegables();

            var model = new Consultation
            {
                TipoPariente = ningunTipoParienteId,
                FechaCreacion = DateTime.Now,
            };



            return View(model);
        }



        // Endpoint para actualizar una consulta existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsultation(
            int id,
            [FromBody] Consultation request)
        {
            if (id != request.Id)
            {
                return BadRequest("El ID de la consulta no coincide con el ID en el cuerpo de la solicitud.");
            }

            try
            {
                await _consultationService.ActualizarConsultaAsync(
                    request.Id,
                    request.UsuarioCreacion,
                    request.HistorialConsulta,
                    request.PacienteId,
                    request.MotivoConsulta,
                    request.EnfermedadConsulta,
                    request.NombrePariente,
                    request.SignosAlarma,
                    request.ReconocimientoFarmacologico,
                    request.TipoPariente,
                    request.TelefonoPariente,
                    request.Temperatura,
                    request.FrecuenciaRespiratoria,
                    request.PresionArterialSistolica,
                    request.PresionArterialDiastolica,
                    request.Pulso,
                    request.Peso,
                    request.Talla,
                    request.PlanTratamiento,
                    request.Observacion,
                    request.AntecedentesPersonales,
                    request.DiasIncapacidad,
                    request.MedicoId,
                    request.EspecialidadId,
                    request.TipoConsultaId,
                    request.NotasEvolucion,
                    request.ConsultaPrincipal,
                    request.EstadoConsulta,
                    // Parámetros para órganos y sistemas
                    request.OrganosSistemas.OrgSentidos,
                    request.OrganosSistemas.ObserOrgSentidos,
                    request.OrganosSistemas.Respiratorio,
                    request.OrganosSistemas.ObserRespiratorio,
                    request.OrganosSistemas.CardioVascular,
                    request.OrganosSistemas.ObserCardioVascular,
                    request.OrganosSistemas.Digestivo,
                    request.OrganosSistemas.ObserDigestivo,
                    request.OrganosSistemas.Genital,
                    request.OrganosSistemas.ObserGenital,
                    request.OrganosSistemas.Urinario,
                    request.OrganosSistemas.ObserUrinario,
                    request.OrganosSistemas.MEsqueletico,
                    request.OrganosSistemas.ObserMEsqueletico,
                    request.OrganosSistemas.Endocrino,
                    request.OrganosSistemas.ObserEndocrino,
                    request.OrganosSistemas.Linfatico,
                    request.OrganosSistemas.ObserLinfatico,
                    request.OrganosSistemas.Nervioso,
                    request.OrganosSistemas.ObserNervioso,
                    // Parámetros para examen físico
                    request.ExamenFisico.Cabeza,
                    request.ExamenFisico.ObserCabeza,
                    request.ExamenFisico.Cuello,
                    request.ExamenFisico.ObserCuello,
                    request.ExamenFisico.Torax,
                    request.ExamenFisico.ObserTorax,
                    request.ExamenFisico.Abdomen,
                    request.ExamenFisico.ObserAbdomen,
                    request.ExamenFisico.Pelvis,
                    request.ExamenFisico.ObserPelvis,
                    request.ExamenFisico.Extremidades,
                    request.ExamenFisico.ObserExtremidades,
                    // Parámetros para antecedentes familiares
                    request.AntecedentesFamiliares.Cardiopatia,
                    request.AntecedentesFamiliares.ObserCardiopatia,
                    request.AntecedentesFamiliares.ParentescocatalogoCardiopatia,
                    request.AntecedentesFamiliares.Diabetes,
                    request.AntecedentesFamiliares.ObserDiabetes,
                    request.AntecedentesFamiliares.ParentescocatalogoDiabetes,
                    request.AntecedentesFamiliares.EnfCardiovascular,
                    request.AntecedentesFamiliares.ObserEnfCardiovascular,
                    request.AntecedentesFamiliares.ParentescocatalogoEnfcardiovascular,
                    request.AntecedentesFamiliares.Hipertension,
                    request.AntecedentesFamiliares.ObserHipertension,
                    request.AntecedentesFamiliares.ParentescocatalogoHipertension,
                    request.AntecedentesFamiliares.Cancer,
                    request.AntecedentesFamiliares.ObserCancer,
                    request.AntecedentesFamiliares.ParentescocatalogoCancer,
                    request.AntecedentesFamiliares.Tuberculosis,
                    request.AntecedentesFamiliares.ObserTuberculosis,
                    request.AntecedentesFamiliares.ParentescocatalogoTuberculosis,
                    request.AntecedentesFamiliares.EnfMental,
                    request.AntecedentesFamiliares.ObserEnfMental,
                    request.AntecedentesFamiliares.ParentescocatalogoEnfmental,
                    request.AntecedentesFamiliares.EnfInfecciosa,
                    request.AntecedentesFamiliares.ObserEnfInfecciosa,
                    request.AntecedentesFamiliares.ParentescocatalogoEnfinfecciosa,
                    request.AntecedentesFamiliares.MalFormacion,
                    request.AntecedentesFamiliares.ObserMalFormacion,
                    request.AntecedentesFamiliares.ParentescocatalogoMalformacion,
                    request.AntecedentesFamiliares.Otro,
                    request.AntecedentesFamiliares.ObserOtro,
                    request.AntecedentesFamiliares.ParentescocatalogoOtro,
                    // Tablas relacionadas
                    request.Alergias,
                    request.Cirugias,
                    request.Medicamentos,
                    request.Laboratorios,
                    request.Imagenes,
                    request.Diagnosticos
                );

                return Ok(new { Message = "Consulta actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones (puedes hacer logging aquí si es necesario)
                return StatusCode(500, new { Message = "Ocurrió un error al actualizar la consulta.", Details = ex.Message });
            }
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
