﻿using MedicExpermed.Controllers;
using MedicExpermed.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MedicExpermed.Services
{
    public class ConsultationService
    {
        private readonly medicossystembdIContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PatientService> _logger;

        public ConsultationService(medicossystembdIContext context, IHttpContextAccessor httpContextAccessor, ILogger<PatientService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;


        }
        /// <summary>
        /// Obtener todas las consultas
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Consultum>> GetAllConsultasAsync()
        {
            // Obtener el nombre de usuario de la sesión
            var loginUsuario = _httpContextAccessor.HttpContext.Session.GetString("UsuarioNombre");

            if (string.IsNullOrEmpty(loginUsuario))
            {
                throw new Exception("El nombre de usuario no está disponible en la sesión.");
            }

            // Filtrar las consultas por el usuario de creación y el estado igual a 0
            var consultas = await _context.Consulta
                .Where(c => c.UsuariocreacionConsulta == loginUsuario)
                .Include(c => c.ConsultaDiagnostico)
                .Include(c => c.ConsultaImagen)
                .Include(c => c.ConsultaLaboratorio)
                .Include(c => c.ConsultaMedicamentos)
                .Include(c => c.PacienteConsultaPNavigation)
                .OrderBy(c => c.FechacreacionConsulta) // Ordenar por fecha de la consulta Ocupar esto mismo para cualquier tabla 

                .ToListAsync();

            return consultas;
        }

        /// <summary>
        /// Busqueda de pacientes
        /// </summary>
        /// <param name="cedula"></param>
        /// <param name="primerNombre"></param>
        /// <param name="primerApellido"></param>
        /// <returns></returns>

        public async Task<IEnumerable<Paciente>> BuscarPacientesAsync(int? cedula, string primerNombre, string primerApellido)
        {
            var query = _context.Pacientes.AsQueryable();

            if (cedula.HasValue)
            {
                query = query.Where(p => p.CiPacientes == cedula.Value);
            }

            if (!string.IsNullOrEmpty(primerNombre))
            {
                query = query.Where(p => p.PrimernombrePacientes.Contains(primerNombre));
            }

            if (!string.IsNullOrEmpty(primerApellido))
            {
                query = query.Where(p => p.PrimerapellidoPacientes.Contains(primerApellido));
            }

            // Aquí combinamos las condiciones usando lógica OR
            query = _context.Pacientes.Where(p =>
                (cedula.HasValue && p.CiPacientes == cedula.Value) ||
                (!string.IsNullOrEmpty(primerNombre) && p.PrimernombrePacientes.Contains(primerNombre)) ||
                (!string.IsNullOrEmpty(primerApellido) && p.PrimerapellidoPacientes.Contains(primerApellido))
            );

            return await query.ToListAsync();
        }

        /// <summary>
        /// Crear una nueva consulta
        /// </summary>
        /// <param name="consultation"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>


        public async Task CrearConsultaAsync(
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
    // Parámetros para órganos y sistemas
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
    // Parámetros para examen físico
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
    // Parámetros para antecedentes familiares
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
    // Parámetros para tablas relacionadas
    List<ConsultaAlergia> alergias,
    List<ConsultaCirugia> cirugias,
    List<ConsultaMedicamento> medicamentos,
    List<ConsultaLaboratorio> laboratorios,
    List<ConsultaImagen> imagenes,
    List<ConsultaDiagnostico> diagnosticos)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("sp_CreateConsultation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros de consulta
                    command.Parameters.AddWithValue("@usuariocreacion_consulta", usuariocreacionConsulta);
                    command.Parameters.AddWithValue("@historial_consulta", historialConsulta);
                    command.Parameters.AddWithValue("@paciente_consulta_p", pacienteConsultaP);
                    command.Parameters.AddWithValue("@motivo_consulta", motivoConsulta);
                    command.Parameters.AddWithValue("@enfermedad_consulta", enfermedadConsulta);
                    command.Parameters.AddWithValue("@nombrepariente_consulta", nombreparienteConsulta);
                    command.Parameters.AddWithValue("@signosalarma_consulta", signosalarmaConsulta);
                    command.Parameters.AddWithValue("@reconofarmacologicas", reconofarmacologicas);
                    command.Parameters.AddWithValue("@tipopariente_consulta", tipoparienteConsulta);
                    command.Parameters.AddWithValue("@telefono_pariente_consulta", telefonoParienteConsulta);
                    command.Parameters.AddWithValue("@temperatura_consulta", temperaturaConsulta);
                    command.Parameters.AddWithValue("@frecuenciarespiratoria_consulta", frecuenciarespiratoriaConsulta);
                    command.Parameters.AddWithValue("@presionarterialsistolica_consulta", presionarterialsistolicaConsulta);
                    command.Parameters.AddWithValue("@presionarterialdiastolica_consulta", presionarterialdiastolicaConsulta);
                    command.Parameters.AddWithValue("@pulso_consulta", pulsoConsulta);
                    command.Parameters.AddWithValue("@peso_consulta", pesoConsulta);
                    command.Parameters.AddWithValue("@talla_consulta", tallaConsulta);
                    command.Parameters.AddWithValue("@plantratamiento_consulta", plantratamientoConsulta);
                    command.Parameters.AddWithValue("@observacion_consulta", observacionConsulta);
                    command.Parameters.AddWithValue("@antecedentespersonales_consulta", antecedentespersonalesConsulta);
                    command.Parameters.AddWithValue("@diasincapacidad_consulta", diasincapacidadConsulta);
                    command.Parameters.AddWithValue("@medico_consulta_d", medicoConsultaD);
                    command.Parameters.AddWithValue("@especialidad_id", especialidadId);
                    command.Parameters.AddWithValue("@tipo_consulta_c", tipoConsultaC);
                    command.Parameters.AddWithValue("@notasevolucion_consulta", notasevolucionConsulta);
                    command.Parameters.AddWithValue("@consultaprincipal_consulta", consultaprincipalConsulta);
                    command.Parameters.AddWithValue("@estado_consulta_c", estadoConsultaC);

                    // Parámetros de órganos y sistemas
                    command.Parameters.AddWithValue("@org_sentidos", orgSentidos);
                    command.Parameters.AddWithValue("@obser_org_sentidos", obserOrgSentidos);
                    command.Parameters.AddWithValue("@respiratorio", respiratorio);
                    command.Parameters.AddWithValue("@obser_respiratorio", obserRespiratorio);
                    command.Parameters.AddWithValue("@cardio_vascular", cardioVascular);
                    command.Parameters.AddWithValue("@obser_cardio_vascular", obserCardioVascular);
                    command.Parameters.AddWithValue("@digestivo", digestivo);
                    command.Parameters.AddWithValue("@obser_digestivo", obserDigestivo);
                    command.Parameters.AddWithValue("@genital", genital);
                    command.Parameters.AddWithValue("@obser_genital", obserGenital);
                    command.Parameters.AddWithValue("@urinario", urinario);
                    command.Parameters.AddWithValue("@obser_urinario", obserUrinario);
                    command.Parameters.AddWithValue("@m_esqueletico", mEsqueletico);
                    command.Parameters.AddWithValue("@obser_m_esqueletico", obserMEsqueletico);
                    command.Parameters.AddWithValue("@endocrino", endocrino);
                    command.Parameters.AddWithValue("@obser_endocrino", obserEndocrino);
                    command.Parameters.AddWithValue("@linfatico", linfatico);
                    command.Parameters.AddWithValue("@obser_linfatico", obserLinfatico);
                    command.Parameters.AddWithValue("@nervioso", nervioso);
                    command.Parameters.AddWithValue("@obser_nervioso", obserNervioso);

                    // Parámetros de examen físico
                    command.Parameters.AddWithValue("@cabeza", cabeza);
                    command.Parameters.AddWithValue("@obser_cabeza", obserCabeza);
                    command.Parameters.AddWithValue("@cuello", cuello);
                    command.Parameters.AddWithValue("@obser_cuello", obserCuello);
                    command.Parameters.AddWithValue("@torax", torax);
                    command.Parameters.AddWithValue("@obser_torax", obserTorax);
                    command.Parameters.AddWithValue("@abdomen", abdomen);
                    command.Parameters.AddWithValue("@obser_abdomen", obserAbdomen);
                    command.Parameters.AddWithValue("@pelvis", pelvis);
                    command.Parameters.AddWithValue("@obser_pelvis", obserPelvis);
                    command.Parameters.AddWithValue("@extremidades", extremidades);
                    command.Parameters.AddWithValue("@obser_extremidades", obserExtremidades);

                    // Parámetros de antecedentes familiares
                    command.Parameters.AddWithValue("@cardiopatia", cardiopatia);
                    command.Parameters.AddWithValue("@obser_cardiopatia", obserCardiopatia);
                    command.Parameters.AddWithValue("@parentescocatalogo_cardiopatia", parentescocatalogoCardiopatia);
                    command.Parameters.AddWithValue("@diabetes", diabetes);
                    command.Parameters.AddWithValue("@obser_diabetes", obserDiabetes);
                    command.Parameters.AddWithValue("@parentescocatalogo_diabetes", parentescocatalogoDiabetes);
                    command.Parameters.AddWithValue("@enf_cardiovascular", enfCardiovascular);
                    command.Parameters.AddWithValue("@obser_enf_cardiovascular", obserEnfCardiovascular);
                    command.Parameters.AddWithValue("@parentescocatalogo_enfcardiovascular", parentescocatalogoEnfCardiovascular);
                    command.Parameters.AddWithValue("@hipertension", hipertension);
                    command.Parameters.AddWithValue("@obser_hipertension", obserHipertension);
                    command.Parameters.AddWithValue("@parentescocatalogo_hipertension", parentescocatalogoHipertension);
                    command.Parameters.AddWithValue("@cancer", cancer);
                    command.Parameters.AddWithValue("@obser_cancer", obserCancer);
                    command.Parameters.AddWithValue("@parentescocatalogo_cancer", parentescocatalogoCancer);
                    command.Parameters.AddWithValue("@tuberculosis", tuberculosis);
                    command.Parameters.AddWithValue("@obser_tuberculosis", obserTuberculosis);
                    command.Parameters.AddWithValue("@parentescocatalogo_tuberculosis", parentescocatalogoTuberculosis);
                    command.Parameters.AddWithValue("@enf_mental", enfMental);
                    command.Parameters.AddWithValue("@obser_enf_mental", obserEnfMental);
                    command.Parameters.AddWithValue("@parentescocatalogo_enfmental", parentescocatalogoEnfMental);
                    command.Parameters.AddWithValue("@enf_infecciosa", enfInfecciosa);
                    command.Parameters.AddWithValue("@obser_enf_infecciosa", obserEnfInfecciosa);
                    command.Parameters.AddWithValue("@parentescocatalogo_enfinfecciosa", parentescocatalogoEnfInfecciosa);
                    command.Parameters.AddWithValue("@mal_formacion", malFormacion);
                    command.Parameters.AddWithValue("@obser_mal_formacion", obserMalFormacion);
                    command.Parameters.AddWithValue("@parentescocatalogo_malformacion", parentescocatalogoMalFormacion);
                    command.Parameters.AddWithValue("@otro", otro);
                    command.Parameters.AddWithValue("@obser_otro", obserOtro);
                    command.Parameters.AddWithValue("@parentescocatalogo_otro", parentescocatalogoOtro);

                    // Tablas relacionadas
                    command.Parameters.AddWithValue("@Alergias", CreateDataTable(alergias));
                    command.Parameters.AddWithValue("@Cirugias", CreateDataTable(cirugias));
                    command.Parameters.AddWithValue("@Medicamentos", CreateDataTable(medicamentos));
                    command.Parameters.AddWithValue("@Laboratorio", CreateDataTable(laboratorios));
                    command.Parameters.AddWithValue("@Imagenes", CreateDataTable(imagenes));
                    command.Parameters.AddWithValue("@Diagnosticos", CreateDataTable(diagnosticos));

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

 



        public async Task ActualizarConsultaAsync(
    int idConsulta,
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
    // Parámetros para órganos y sistemas
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
    // Parámetros para examen físico
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
    // Parámetros para antecedentes familiares
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
    // Parámetros para tablas relacionadas
    List<ConsultaAlergia> alergias,
    List<ConsultaCirugia> cirugias,
    List<ConsultaMedicamento> medicamentos,
    List<ConsultaLaboratorio> laboratorios,
    List<ConsultaImagen> imagenes,
    List<ConsultaDiagnostico> diagnosticos)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("sp_UpdateConsultation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetro de ID de consulta
                    command.Parameters.AddWithValue("@idConsulta", idConsulta);

                    // Parámetros de consulta
                    command.Parameters.AddWithValue("@usuariocreacion_consulta", usuariocreacionConsulta);
                    command.Parameters.AddWithValue("@historial_consulta", historialConsulta);
                    command.Parameters.AddWithValue("@paciente_consulta_p", pacienteConsultaP);
                    command.Parameters.AddWithValue("@motivo_consulta", motivoConsulta);
                    command.Parameters.AddWithValue("@enfermedad_consulta", enfermedadConsulta);
                    command.Parameters.AddWithValue("@nombrepariente_consulta", nombreparienteConsulta);
                    command.Parameters.AddWithValue("@signosalarma_consulta", signosalarmaConsulta);
                    command.Parameters.AddWithValue("@reconofarmacologicas", reconofarmacologicas);
                    command.Parameters.AddWithValue("@tipopariente_consulta", tipoparienteConsulta);
                    command.Parameters.AddWithValue("@telefono_pariente_consulta", telefonoParienteConsulta);
                    command.Parameters.AddWithValue("@temperatura_consulta", temperaturaConsulta);
                    command.Parameters.AddWithValue("@frecuenciarespiratoria_consulta", frecuenciarespiratoriaConsulta);
                    command.Parameters.AddWithValue("@presionarterialsistolica_consulta", presionarterialsistolicaConsulta);
                    command.Parameters.AddWithValue("@presionarterialdiastolica_consulta", presionarterialdiastolicaConsulta);
                    command.Parameters.AddWithValue("@pulso_consulta", pulsoConsulta);
                    command.Parameters.AddWithValue("@peso_consulta", pesoConsulta);
                    command.Parameters.AddWithValue("@talla_consulta", tallaConsulta);
                    command.Parameters.AddWithValue("@plantratamiento_consulta", plantratamientoConsulta);
                    command.Parameters.AddWithValue("@observacion_consulta", observacionConsulta);
                    command.Parameters.AddWithValue("@antecedentespersonales_consulta", antecedentespersonalesConsulta);
                    command.Parameters.AddWithValue("@diasincapacidad_consulta", diasincapacidadConsulta);
                    command.Parameters.AddWithValue("@medico_consulta_d", medicoConsultaD);
                    command.Parameters.AddWithValue("@especialidad_id", especialidadId);
                    command.Parameters.AddWithValue("@tipo_consulta_c", tipoConsultaC);
                    command.Parameters.AddWithValue("@notasevolucion_consulta", notasevolucionConsulta);
                    command.Parameters.AddWithValue("@consultaprincipal_consulta", consultaprincipalConsulta);
                    command.Parameters.AddWithValue("@estado_consulta_c", estadoConsultaC);

                    // Parámetros de órganos y sistemas
                    command.Parameters.AddWithValue("@org_sentidos", orgSentidos);
                    command.Parameters.AddWithValue("@obser_org_sentidos", obserOrgSentidos);
                    command.Parameters.AddWithValue("@respiratorio", respiratorio);
                    command.Parameters.AddWithValue("@obser_respiratorio", obserRespiratorio);
                    command.Parameters.AddWithValue("@cardio_vascular", cardioVascular);
                    command.Parameters.AddWithValue("@obser_cardio_vascular", obserCardioVascular);
                    command.Parameters.AddWithValue("@digestivo", digestivo);
                    command.Parameters.AddWithValue("@obser_digestivo", obserDigestivo);
                    command.Parameters.AddWithValue("@genital", genital);
                    command.Parameters.AddWithValue("@obser_genital", obserGenital);
                    command.Parameters.AddWithValue("@urinario", urinario);
                    command.Parameters.AddWithValue("@obser_urinario", obserUrinario);
                    command.Parameters.AddWithValue("@m_esqueletico", mEsqueletico);
                    command.Parameters.AddWithValue("@obser_m_esqueletico", obserMEsqueletico);
                    command.Parameters.AddWithValue("@endocrino", endocrino);
                    command.Parameters.AddWithValue("@obser_endocrino", obserEndocrino);
                    command.Parameters.AddWithValue("@linfatico", linfatico);
                    command.Parameters.AddWithValue("@obser_linfatico", obserLinfatico);
                    command.Parameters.AddWithValue("@nervioso", nervioso);
                    command.Parameters.AddWithValue("@obser_nervioso", obserNervioso);

                    // Parámetros de examen físico
                    command.Parameters.AddWithValue("@cabeza", cabeza);
                    command.Parameters.AddWithValue("@obser_cabeza", obserCabeza);
                    command.Parameters.AddWithValue("@cuello", cuello);
                    command.Parameters.AddWithValue("@obser_cuello", obserCuello);
                    command.Parameters.AddWithValue("@torax", torax);
                    command.Parameters.AddWithValue("@obser_torax", obserTorax);
                    command.Parameters.AddWithValue("@abdomen", abdomen);
                    command.Parameters.AddWithValue("@obser_abdomen", obserAbdomen);
                    command.Parameters.AddWithValue("@pelvis", pelvis);
                    command.Parameters.AddWithValue("@obser_pelvis", obserPelvis);
                    command.Parameters.AddWithValue("@extremidades", extremidades);
                    command.Parameters.AddWithValue("@obser_extremidades", obserExtremidades);

                    // Parámetros de antecedentes familiares
                    command.Parameters.AddWithValue("@cardiopatia", cardiopatia);
                    command.Parameters.AddWithValue("@obser_cardiopatia", obserCardiopatia);
                    command.Parameters.AddWithValue("@parentescocatalogo_cardiopatia", parentescocatalogoCardiopatia);
                    command.Parameters.AddWithValue("@diabetes", diabetes);
                    command.Parameters.AddWithValue("@obser_diabetes", obserDiabetes);
                    command.Parameters.AddWithValue("@parentescocatalogo_diabetes", parentescocatalogoDiabetes);
                    command.Parameters.AddWithValue("@enf_cardiovascular", enfCardiovascular);
                    command.Parameters.AddWithValue("@obser_enf_cardiovascular", obserEnfCardiovascular);
                    command.Parameters.AddWithValue("@parentescocatalogo_enfcardiovascular", parentescocatalogoEnfCardiovascular);
                    command.Parameters.AddWithValue("@hipertension", hipertension);
                    command.Parameters.AddWithValue("@obser_hipertension", obserHipertension);
                    command.Parameters.AddWithValue("@parentescocatalogo_hipertension", parentescocatalogoHipertension);
                    command.Parameters.AddWithValue("@cancer", cancer);
                    command.Parameters.AddWithValue("@obser_cancer", obserCancer);
                    command.Parameters.AddWithValue("@parentescocatalogo_cancer", parentescocatalogoCancer);
                    command.Parameters.AddWithValue("@tuberculosis", tuberculosis);
                    command.Parameters.AddWithValue("@obser_tuberculosis", obserTuberculosis);
                    command.Parameters.AddWithValue("@parentescocatalogo_tuberculosis", parentescocatalogoTuberculosis);
                    command.Parameters.AddWithValue("@enf_mental", enfMental);
                    command.Parameters.AddWithValue("@obser_enf_mental", obserEnfMental);
                    command.Parameters.AddWithValue("@parentescocatalogo_enfmental", parentescocatalogoEnfMental);
                    command.Parameters.AddWithValue("@enf_infecciosa", enfInfecciosa);
                    command.Parameters.AddWithValue("@obser_enf_infecciosa", obserEnfInfecciosa);
                    command.Parameters.AddWithValue("@parentescocatalogo_enfinfecciosa", parentescocatalogoEnfInfecciosa);
                    command.Parameters.AddWithValue("@mal_formacion", malFormacion);
                    command.Parameters.AddWithValue("@obser_mal_formacion", obserMalFormacion);
                    command.Parameters.AddWithValue("@parentescocatalogo_malformacion", parentescocatalogoMalFormacion);
                    command.Parameters.AddWithValue("@otro", otro);
                    command.Parameters.AddWithValue("@obser_otro", obserOtro);
                    command.Parameters.AddWithValue("@parentescocatalogo_otro", parentescocatalogoOtro);

                    // Tablas relacionadas (TVP)
                    command.Parameters.AddWithValue("@Alergias", CreateDataTable(alergias));
                    command.Parameters.AddWithValue("@Cirugias", CreateDataTable(cirugias));
                    command.Parameters.AddWithValue("@Medicamentos", CreateDataTable(medicamentos));
                    command.Parameters.AddWithValue("@Laboratorio", CreateDataTable(laboratorios));
                    command.Parameters.AddWithValue("@Imagenes", CreateDataTable(imagenes));
                    command.Parameters.AddWithValue("@Diagnosticos", CreateDataTable(diagnosticos));

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        // Método para crear un DataTable desde una lista de objetos
        private DataTable CreateDataTable<T>(List<T> list)
        {
            var table = new DataTable();
            var properties = typeof(T).GetProperties();

            // Crear columnas en el DataTable basadas en las propiedades de la clase
            foreach (var prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // Rellenar las filas del DataTable con los valores de los objetos
            foreach (var item in list)
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



    }
}
