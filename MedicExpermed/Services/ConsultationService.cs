using MedicExpermed.Controllers;
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
        public void CreateConsultation(Consultum consultation)
        {
            using (SqlConnection conn = new SqlConnection(_context.Database.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("sp_CreateConsultation", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Parámetros de la tabla consulta
                cmd.Parameters.AddWithValue("@usuariocreacion_consulta", consultation.UsuariocreacionConsulta);
                cmd.Parameters.AddWithValue("@historial_consulta", consultation.HistorialConsulta);
                cmd.Parameters.AddWithValue("@paciente_consulta_p", consultation.PacienteConsultaP);
                cmd.Parameters.AddWithValue("@motivo_consulta", consultation.MotivoConsulta);
                cmd.Parameters.AddWithValue("@enfermedad_consulta", consultation.EnfermedadConsulta);
                cmd.Parameters.AddWithValue("@nombrepariente_consulta", consultation.NombreparienteConsulta);
                cmd.Parameters.AddWithValue("@signosalarma_consulta", consultation.SignosalarmaConsulta);
                cmd.Parameters.AddWithValue("@reconofarmacologicas", consultation.Reconofarmacologicas);
                cmd.Parameters.AddWithValue("@tipopariente_consulta", consultation.TipoparienteConsulta);
                cmd.Parameters.AddWithValue("@telefono_pariente_consulta", consultation.TelefonoParienteConsulta);
                cmd.Parameters.AddWithValue("@temperatura_consulta", consultation.TemperaturaConsulta);
                cmd.Parameters.AddWithValue("@frecuenciarespiratoria_consulta", consultation.FrecuenciarespiratoriaConsulta);
                cmd.Parameters.AddWithValue("@presionarterialsistolica_consulta", consultation.PresionarterialsistolicaConsulta);
                cmd.Parameters.AddWithValue("@presionarterialdiastolica_consulta", consultation.PresionarterialdiastolicaConsulta);
                cmd.Parameters.AddWithValue("@pulso_consulta", consultation.PulsoConsulta);
                cmd.Parameters.AddWithValue("@peso_consulta", consultation.PesoConsulta);
                cmd.Parameters.AddWithValue("@talla_consulta", consultation.TallaConsulta);
                cmd.Parameters.AddWithValue("@plantratamiento_consulta", consultation.PlantratamientoConsulta);
                cmd.Parameters.AddWithValue("@observacion_consulta", consultation.ObservacionConsulta);
                cmd.Parameters.AddWithValue("@antecedentespersonales_consulta", consultation.AntecedentespersonalesConsulta);
                cmd.Parameters.AddWithValue("@diasincapacidad_consulta", consultation.DiasincapacidadConsulta);
                cmd.Parameters.AddWithValue("@medico_consulta_d", consultation.MedicoConsultaD);
                cmd.Parameters.AddWithValue("@especialidad_id", consultation.EspecialidadId);
                cmd.Parameters.AddWithValue("@tipo_consulta_c", consultation.TipoConsulta);
                cmd.Parameters.AddWithValue("@notasevolucion_consulta", consultation.NotasEvolucion);
                cmd.Parameters.AddWithValue("@consultaprincipal_consulta", consultation.ConsultaPrincipal);
                cmd.Parameters.AddWithValue("@estado_consulta_c", consultation.EstadoConsulta);

                // Otros parámetros adicionales (organos_sistemas, examen_fisico, antecedentes_familiares, etc.)
                cmd.Parameters.AddWithValue("@org_sentidos", consultation.OrgSentidos);
                cmd.Parameters.AddWithValue("@obser_org_sentidos", consultation.ObserOrgSentidos);
                cmd.Parameters.AddWithValue("@respiratorio", consultation.Respiratorio);
                cmd.Parameters.AddWithValue("@obser_respiratorio", consultation.ObserRespiratorio);
                cmd.Parameters.AddWithValue("@cardio_vascular", consultation.CardioVascular);
                cmd.Parameters.AddWithValue("@obser_cardio_vascular", consultation.ObserCardioVascular);
                cmd.Parameters.AddWithValue("@digestivo", consultation.Digestivo);
                cmd.Parameters.AddWithValue("@obser_digestivo", consultation.ObserDigestivo);
                cmd.Parameters.AddWithValue("@genital", consultation.Genital);
                cmd.Parameters.AddWithValue("@obser_genital", consultation.ObserGenital);
                cmd.Parameters.AddWithValue("@urinario", consultation.Urinario);
                cmd.Parameters.AddWithValue("@obser_urinario", consultation.ObserUrinario);
                cmd.Parameters.AddWithValue("@m_esqueletico", consultation.MEsqueletico);
                cmd.Parameters.AddWithValue("@obser_m_esqueletico", consultation.ObserMEsqueletico);
                cmd.Parameters.AddWithValue("@endocrino", consultation.Endocrino);
                cmd.Parameters.AddWithValue("@obser_endocrino", consultation.ObserEndocrino);
                cmd.Parameters.AddWithValue("@linfatico", consultation.Linfatico);
                cmd.Parameters.AddWithValue("@obser_linfatico", consultation.ObserLinfatico);
                cmd.Parameters.AddWithValue("@nervioso", consultation.Nervioso);
                cmd.Parameters.AddWithValue("@obser_nervioso", consultation.ObserNervioso);

                // Parámetros para examen físico
                cmd.Parameters.AddWithValue("@cabeza", consultation.Cabeza);
                cmd.Parameters.AddWithValue("@obser_cabeza", consultation.ObserCabeza);
                cmd.Parameters.AddWithValue("@cuello", consultation.Cuello);
                cmd.Parameters.AddWithValue("@obser_cuello", consultation.ObserCuello);
                cmd.Parameters.AddWithValue("@torax", consultation.Torax);
                cmd.Parameters.AddWithValue("@obser_torax", consultation.ObserTorax);
                cmd.Parameters.AddWithValue("@abdomen", consultation.Abdomen);
                cmd.Parameters.AddWithValue("@obser_abdomen", consultation.ObserAbdomen);
                cmd.Parameters.AddWithValue("@pelvis", consultation.Pelvis);
                cmd.Parameters.AddWithValue("@obser_pelvis", consultation.ObserPelvis);
                cmd.Parameters.AddWithValue("@extremidades", consultation.Extremidades);
                cmd.Parameters.AddWithValue("@obser_extremidades", consultation.ObserExtremidades);

                // Parámetros tipo tabla (DataTable)
                cmd.Parameters.AddWithValue("@Alergias", consultation.Alergias);
                cmd.Parameters.AddWithValue("@Cirugias", consultation.Cirugias);
                cmd.Parameters.AddWithValue("@Medicamentos", consultation.Medicamentos);
                cmd.Parameters.AddWithValue("@Laboratorio", consultation.Laboratorio);
                cmd.Parameters.AddWithValue("@Imagenes", consultation.Imagenes);
                cmd.Parameters.AddWithValue("@Diagnosticos", consultation.Diagnosticos);

                // Abrir la conexión y ejecutar el procedimiento
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


    }
}
