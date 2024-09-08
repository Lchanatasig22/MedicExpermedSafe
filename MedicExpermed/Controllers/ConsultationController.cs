﻿using MedicExpermed.Models;
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


      [HttpPost("Crear-Consulta")]
public async Task<IActionResult> CrearConsulta([FromBody] Consultation consultaDto)
{
    if (!ModelState.IsValid)
    {
        // Si el modelo no es válido, devolver un JSON con los errores
        return Json(new
        {
            success = false,
            message = "Datos inválidos",
            errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
        });
    }

    try
    {
        // Llama al servicio para crear la consulta
        await _consultationService.CrearConsultaAsync(
            consultaDto.UsuarioCreacion,
            consultaDto.HistorialConsulta,
            consultaDto.PacienteId,
            consultaDto.MotivoConsulta,
            consultaDto.EnfermedadConsulta,
            consultaDto.NombrePariente,
            consultaDto.SignosAlarma,
            consultaDto.ReconocimientoFarmacologico,
            consultaDto.TipoPariente,
            consultaDto.TelefonoPariente,
            consultaDto.Temperatura,
            consultaDto.FrecuenciaRespiratoria,
            consultaDto.PresionArterialSistolica,
            consultaDto.PresionArterialDiastolica,
            consultaDto.Pulso,
            consultaDto.Peso,
            consultaDto.Talla,
            consultaDto.PlanTratamiento,
            consultaDto.Observacion,
            consultaDto.AntecedentesPersonales,
            consultaDto.DiasIncapacidad,
            consultaDto.MedicoId,
            consultaDto.EspecialidadId,
            consultaDto.TipoConsultaId,
            consultaDto.NotasEvolucion,
            consultaDto.ConsultaPrincipal,
            consultaDto.EstadoConsulta,
            // Parámetros de órganos y sistemas
            consultaDto.OrganosSistemas.OrgSentidos,
            consultaDto.OrganosSistemas.ObserOrgSentidos,
            consultaDto.OrganosSistemas.Respiratorio,
            consultaDto.OrganosSistemas.ObserRespiratorio,
            consultaDto.OrganosSistemas.CardioVascular,
            consultaDto.OrganosSistemas.ObserCardioVascular,
            consultaDto.OrganosSistemas.Digestivo,
            consultaDto.OrganosSistemas.ObserDigestivo,
            consultaDto.OrganosSistemas.Genital,
            consultaDto.OrganosSistemas.ObserGenital,
            consultaDto.OrganosSistemas.Urinario,
            consultaDto.OrganosSistemas.ObserUrinario,
            consultaDto.OrganosSistemas.MEsqueletico,
            consultaDto.OrganosSistemas.ObserMEsqueletico,
            consultaDto.OrganosSistemas.Endocrino,
            consultaDto.OrganosSistemas.ObserEndocrino,
            consultaDto.OrganosSistemas.Linfatico,
            consultaDto.OrganosSistemas.ObserLinfatico,
            consultaDto.OrganosSistemas.Nervioso,
            consultaDto.OrganosSistemas.ObserNervioso,
            // Parámetros de examen físico
            consultaDto.ExamenFisico.Cabeza,
            consultaDto.ExamenFisico.ObserCabeza,
            consultaDto.ExamenFisico.Cuello,
            consultaDto.ExamenFisico.ObserCuello,
            consultaDto.ExamenFisico.Torax,
            consultaDto.ExamenFisico.ObserTorax,
            consultaDto.ExamenFisico.Abdomen,
            consultaDto.ExamenFisico.ObserAbdomen,
            consultaDto.ExamenFisico.Pelvis,
            consultaDto.ExamenFisico.ObserPelvis,
            consultaDto.ExamenFisico.Extremidades,
            consultaDto.ExamenFisico.ObserExtremidades,
            // Parámetros de antecedentes familiares
            consultaDto.AntecedentesFamiliares.Cardiopatia,
            consultaDto.AntecedentesFamiliares.ObserCardiopatia,
            consultaDto.AntecedentesFamiliares.ParentescocatalogoCardiopatia,
            consultaDto.AntecedentesFamiliares.Diabetes,
            consultaDto.AntecedentesFamiliares.ObserDiabetes,
            consultaDto.AntecedentesFamiliares.ParentescocatalogoDiabetes,
            consultaDto.AntecedentesFamiliares.EnfCardiovascular,
            consultaDto.AntecedentesFamiliares.ObserEnfCardiovascular,
            consultaDto.AntecedentesFamiliares.ParentescocatalogoEnfcardiovascular,
            consultaDto.AntecedentesFamiliares.Hipertension,
            consultaDto.AntecedentesFamiliares.ObserHipertension,
            consultaDto.AntecedentesFamiliares.ParentescocatalogoHipertension,
            consultaDto.AntecedentesFamiliares.Cancer,
            consultaDto.AntecedentesFamiliares.ObserCancer,
            consultaDto.AntecedentesFamiliares.ParentescocatalogoCancer,
            consultaDto.AntecedentesFamiliares.Tuberculosis,
            consultaDto.AntecedentesFamiliares.ObserTuberculosis,
            consultaDto.AntecedentesFamiliares.ParentescocatalogoTuberculosis,
            consultaDto.AntecedentesFamiliares.EnfMental,
            consultaDto.AntecedentesFamiliares.ObserEnfMental,
            consultaDto.AntecedentesFamiliares.ParentescocatalogoEnfmental,
            consultaDto.AntecedentesFamiliares.EnfInfecciosa,
            consultaDto.AntecedentesFamiliares.ObserEnfInfecciosa,
            consultaDto.AntecedentesFamiliares.ParentescocatalogoEnfinfecciosa,
            consultaDto.AntecedentesFamiliares.MalFormacion,
            consultaDto.AntecedentesFamiliares.ObserMalFormacion,
            consultaDto.AntecedentesFamiliares.ParentescocatalogoMalformacion,
            consultaDto.AntecedentesFamiliares.Otro,
            consultaDto.AntecedentesFamiliares.ObserOtro,
            consultaDto.AntecedentesFamiliares.ParentescocatalogoOtro,
            // Tablas relacionadas
            consultaDto.Alergias,
            consultaDto.Cirugias,
            consultaDto.Medicamentos,
            consultaDto.Laboratorios,
            consultaDto.Imagenes,
            consultaDto.Diagnosticos
        );

        // Devuelve un JSON con éxito si es una solicitud AJAX
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new { success = true, message = "Consulta creada exitosamente" });

        }
        else
        {
            // Redirigir en caso de una solicitud normal (no AJAX)
                        return Json(new { success = true, redirectUrl = Url.Action("ListarConsultas") });
        }
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al crear la consulta");

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new { success = false, message = "Ocurrió un error en el servidor.", details = ex.Message });
        }
        else
        {
            return View("Error", new ErrorViewModel { RequestId = ex.Message });
        }
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
// Parámetros para órganos y sistemas con manejo de booleanos nullable
request.OrganosSistemas.OrgSentidos ?? false,        // Manejo de bool?
request.OrganosSistemas.ObserOrgSentidos,
request.OrganosSistemas.Respiratorio ?? false,       // Manejo de bool?
request.OrganosSistemas.ObserRespiratorio,
request.OrganosSistemas.CardioVascular ?? false,     // Manejo de bool?
request.OrganosSistemas.ObserCardioVascular,
request.OrganosSistemas.Digestivo ?? false,          // Manejo de bool?
request.OrganosSistemas.ObserDigestivo,
request.OrganosSistemas.Genital ?? false,            // Manejo de bool?
request.OrganosSistemas.ObserGenital,
request.OrganosSistemas.Urinario ?? false,           // Manejo de bool?
request.OrganosSistemas.ObserUrinario,
request.OrganosSistemas.MEsqueletico ?? false,       // Manejo de bool?
request.OrganosSistemas.ObserMEsqueletico,
request.OrganosSistemas.Endocrino ?? false,          // Manejo de bool?
request.OrganosSistemas.ObserEndocrino,
request.OrganosSistemas.Linfatico ?? false,          // Manejo de bool?
request.OrganosSistemas.ObserLinfatico,
request.OrganosSistemas.Nervioso ?? false,           // Manejo de bool?
request.OrganosSistemas.ObserNervioso,


// Parámetros para examen físico con manejo de booleanos nullable
request.ExamenFisico.Cabeza ?? false, // Manejo de bool?
request.ExamenFisico.ObserCabeza,
request.ExamenFisico.Cuello ?? false, // Manejo de bool?
request.ExamenFisico.ObserCuello,
request.ExamenFisico.Torax ?? false, // Manejo de bool?
request.ExamenFisico.ObserTorax,
request.ExamenFisico.Abdomen ?? false, // Manejo de bool?
request.ExamenFisico.ObserAbdomen,
request.ExamenFisico.Pelvis ?? false, // Manejo de bool?
request.ExamenFisico.ObserPelvis,
request.ExamenFisico.Extremidades ?? false, // Manejo de bool?
request.ExamenFisico.ObserExtremidades
,
// Parámetros para antecedentes familiares, manejo de nulos con valor predeterminado
request.AntecedentesFamiliares.Cardiopatia ?? false,
request.AntecedentesFamiliares.ObserCardiopatia,
request.AntecedentesFamiliares.ParentescocatalogoCardiopatia ?? default(int),
request.AntecedentesFamiliares.Diabetes ?? false,
request.AntecedentesFamiliares.ObserDiabetes,
request.AntecedentesFamiliares.ParentescocatalogoDiabetes ?? default(int),
request.AntecedentesFamiliares.EnfCardiovascular ?? false,
request.AntecedentesFamiliares.ObserEnfCardiovascular,
request.AntecedentesFamiliares.ParentescocatalogoEnfcardiovascular ?? default(int),
request.AntecedentesFamiliares.Hipertension ?? false,
request.AntecedentesFamiliares.ObserHipertension,
request.AntecedentesFamiliares.ParentescocatalogoHipertension ?? default(int),
request.AntecedentesFamiliares.Cancer ?? false,
request.AntecedentesFamiliares.ObserCancer,
request.AntecedentesFamiliares.ParentescocatalogoCancer ?? default(int),
request.AntecedentesFamiliares.Tuberculosis ?? false,
request.AntecedentesFamiliares.ObserTuberculosis,
request.AntecedentesFamiliares.ParentescocatalogoTuberculosis ?? default(int),
request.AntecedentesFamiliares.EnfMental ?? false,
request.AntecedentesFamiliares.ObserEnfMental,
request.AntecedentesFamiliares.ParentescocatalogoEnfmental ?? default(int),
request.AntecedentesFamiliares.EnfInfecciosa ?? false,
request.AntecedentesFamiliares.ObserEnfInfecciosa,
request.AntecedentesFamiliares.ParentescocatalogoEnfinfecciosa ?? default(int),
request.AntecedentesFamiliares.MalFormacion ?? false,
request.AntecedentesFamiliares.ObserMalFormacion,
request.AntecedentesFamiliares.ParentescocatalogoMalformacion ?? default(int),
request.AntecedentesFamiliares.Otro ?? false,
request.AntecedentesFamiliares.ObserOtro,
request.AntecedentesFamiliares.ParentescocatalogoOtro ?? default(int),
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
