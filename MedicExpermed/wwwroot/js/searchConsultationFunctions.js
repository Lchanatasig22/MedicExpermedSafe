document.addEventListener('DOMContentLoaded', function () {
    function setupSearch(opcionId, buscarFieldsId, buscarCriterioId, noResultsId, entityType) {
        const opcionSelect = document.getElementById(opcionId);
        const buscarFields = document.getElementById(buscarFieldsId);
        const buscarCriterio = document.getElementById(buscarCriterioId);
        const noResults = document.getElementById(noResultsId);

        if (opcionSelect && buscarFields && buscarCriterio && noResults) {
            opcionSelect.addEventListener('change', function () {
                onSearchChanged(entityType);
            });

            window.onSearch = function () {
                const criterio = buscarCriterio.value.toLowerCase();
                const optionValue = opcionSelect.value;
                const rows = document.querySelectorAll(`tbody tr[data-type="${entityType}"]`);
                const cards = document.querySelectorAll(`.card-body[data-type="${entityType}"]`);
                let found = false;

                rows.forEach(row => {
                    const cellValue = getCellValue(row, optionValue).toLowerCase();
                    if (cellValue.includes(criterio)) {
                        row.style.display = '';
                        found = true;
                    } else {
                        row.style.display = 'none';
                    }
                });

                cards.forEach(card => {
                    const cellValue = getCardValue(card, optionValue).toLowerCase();
                    if (cellValue.includes(criterio)) {
                        card.parentElement.style.display = '';
                        found = true;
                    } else {
                        card.parentElement.style.display = 'none';
                    }
                });

                noResults.classList.toggle('d-none', found);
            };

            function onSearchChanged(entityType) {
                if (opcionSelect.value === '-2') {
                    buscarFields.classList.add('d-none');
                    showAllRowsAndCards(entityType);
                    noResults.classList.add('d-none');
                } else {
                    buscarFields.classList.remove('d-none');
                }
            }
        }
    }

    function getCellValue(row, optionValue) {
        return row.querySelector(`[data-search-type="${optionValue}"]`).innerText || '';
    }

    function getCardValue(card, optionValue) {
        return card.querySelector(`[data-search-type="${optionValue}"]`).innerText || '';
    }

    function showAllRowsAndCards(entityType) {
        document.querySelectorAll(`tbody tr[data-type="${entityType}"], .card-body[data-type="${entityType}"]`).forEach(el => el.style.display = '');
    }

    // Inicializar la búsqueda para Consultas
    setupSearch('opcionConsultas', 'buscarFieldsConsultas', 'buscarCriterioConsultas', 'noResultsConsultas', 'consultas');
});

function buscarPaciente() {
    const searchValue = document.getElementById('search-input').value.trim();
    const searchCriteria = document.getElementById('search-criteria').value;

    let cedula = null;
    let primerNombre = null;
    let primerApellido = null;

    if (searchCriteria === 'cedula') {
        cedula = searchValue;
    } else if (searchCriteria === 'nombre') {
        primerNombre = searchValue;
    } else if (searchCriteria === 'apellido') {
        primerApellido = searchValue;
    }

    const baseUrl = document.getElementById('search-container').getAttribute('data-url');
    const url = `${baseUrl}?cedula=${encodeURIComponent(cedula)}&primerNombre=${encodeURIComponent(primerNombre)}&primerApellido=${encodeURIComponent(primerApellido)}`;
    console.log("URL construida:", url);

    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log("Datos recibidos:", data);
            const pacientes = data["$values"] || [];

            if (pacientes.length > 0) {
                const paciente = pacientes[0];  // Asumimos que solo necesitas llenar con el primer resultado

                // Rellenar los campos del formulario con los datos del paciente
                document.getElementById('idPaciente').value = paciente.idPacientes;
                document.getElementById('primerApellido').value = paciente.primerapellidoPacientes || '';
                document.getElementById('segundoApellido').value = paciente.segundoapellidoPacientes || '';
                document.getElementById('primerNombre').value = paciente.primernombrePacientes || '';
                document.getElementById('segundoNombre').value = paciente.segundonombrePacientes || '';
                document.getElementById('tipoDocumentoSelect').value = paciente.tipodocumentoPacientesCa || '';
                document.getElementById('numeroDocumento').value = paciente.ciPacientes || '';
                document.getElementById('tipoSangre').value = paciente.tiposangrePacientesCa || '';
                document.getElementById('esdonante').value = paciente.donantePacientes ? 'Si' : 'No';
                document.getElementById('fechaNacimiento').value = paciente.fechanacimientoPacientes ? paciente.fechanacimientoPacientes.split('T')[0] : '';
                document.getElementById('edad').value = paciente.edadPacientes || '';
                document.getElementById('sexoSelect').value = paciente.sexoPacientesCa || '';
                document.getElementById('estadoCivilSelect').value = paciente.estadocivilPacientesCa || '';
                document.getElementById('formacionProfesionalSelect').value = paciente.formacionprofesionalPacientesCa || '';
                document.getElementById('nacionalidadSelect').value = paciente.nacionalidadPacientesPa || '';
                document.getElementById('direccion').value = paciente.direccionPacientes || '';
                document.getElementById('telefono').value = paciente.telefonofijoPacientes || '';
                document.getElementById('telefonoCelular').value = paciente.telefonocelularPacientes || '';
                document.getElementById('email').value = paciente.emailPacientes || '';
                document.getElementById('ocupacion').value = paciente.ocupacionPacientes || '';
                document.getElementById('empresa').value = paciente.empresaPacientes || '';
                document.getElementById('seguroSelect').value = paciente.segurosaludPacientesCa || '';
                document.getElementById('historiaClinica').value = paciente.ciPacientes || '';

             

            } else {
                alert('No se encontraron pacientes.');
            }
        })
        .catch(error => {
            console.error('Error al buscar pacientes:', error);
            alert('Error al buscar pacientes.');
        });
}

// Función para convertir texto a "Title Case"
function toTitleCase(str) {
    return str.toLowerCase().replace(/\b\w/g, function (letter) {
        return letter.toUpperCase();
    });
}

// Función para manejar la transición entre steps y aplicar Title Case
function goToNextStep(stepNumber, event) {
    event.preventDefault();

    if (stepNumber === 2) {
        const historiaClinica = document.getElementById('historiaClinica').value.trim();
        if (!historiaClinica) {
            swal({
                title: "Campo obligatorio",
                text: "El campo 'Historia Clínica' es obligatorio.",
                icon: "warning",
                button: "Ok",
            });
            return;
        }

        // Actualiza la tabla con los datos del paciente
        var pacienteNombre = toTitleCase($('#primerNombre').val()) + ' ' + toTitleCase($('#primerApellido').val());
        var numeroDocumento = $('#numeroDocumento').val();
        var sexo = toTitleCase($('#sexoSelect option:selected').text()); // Muestra el texto del sexo
        var tipoSangre = toTitleCase($('#tipoSangre option:selected').text()); // Muestra el texto del tipo de sangre
        var fechaNacimiento = $('#fechaNacimiento').val();
        var edad = $('#edad').val();
        var estadoCivil = toTitleCase($('#estadoCivilSelect option:selected').text()); // Muestra el texto del estado civil
        var formacionProfesional = toTitleCase($('#formacionProfesionalSelect option:selected').text()); // Muestra el texto de la formación profesional
        var nacionalidad = toTitleCase($('#nacionalidadSelect option:selected').text()); // Muestra el texto de la nacionalidad
        var telefono = $('#telefono').val();
        var telefonoCelular = $('#telefonoCelular').val();
        var email = $('#email').val();

        // Actualiza todos los steps con los datos
        $('#pacienteNombre, #pacienteNombreStep2, #pacienteNombreStep3, #pacienteNombreStep4').text(pacienteNombre);
        $('#numeroDocumentoTabla, #numeroDocumentoTablaStep2, #numeroDocumentoTablaStep3,#numeroDocumentoTablaStep4').text(numeroDocumento);
        $('#sexoTabla, #sexoTablaStep2, #sexoTablaStep3,#sexoTablaStep4').text(sexo);
        $('#tipoSangreTabla, #tipoSangreTablaStep2, #tipoSangreTablaStep3,#tipoSangreTablaStep4').text(tipoSangre);
        $('#fechaNacimientoTabla, #fechaNacimientoTablaStep2, #fechaNacimientoTablaStep3,#fechaNacimientoTablaStep4').text(fechaNacimiento);
        $('#edadTabla, #edadTablaStep2, #edadTablaStep3,#edadTablaStep4').text(edad);
        $('#estadoCivilTabla, #estadoCivilTablaStep2, #estadoCivilTablaStep3,#estadoCivilTablaStep4').text(estadoCivil);
        $('#formacionProfesionalTabla, #formacionProfesionalTablaStep2, #formacionProfesionalTablaStep3,#formacionProfesionalTablaStep4').text(formacionProfesional);
        $('#nacionalidadTabla, #nacionalidadTablaStep2, #nacionalidadTablaStep3,#nacionalidadTablaStep4').text(nacionalidad);
        $('#telefonoTabla, #telefonoTablaStep2, #telefonoTablaStep3,#telefonoTablaStep4').text(telefono);
        $('#telefonoCelularTabla, #telefonoCelularTablaStep2, #telefonoCelularTablaStep3,#telefonoCelularTablaStep4').text(telefonoCelular);
        $('#emailTabla, #emailTablaStep2, #emailTablaStep3,#emailTablaStep4').text(email);
    }

    // Ocultar todos los steps
    const steps = document.querySelectorAll('.setup-content');
    steps.forEach(step => {
        step.style.display = 'none';
    });

    // Mostrar el step seleccionado
    const selectedStep = document.getElementById(`step-${stepNumber}`);
    if (selectedStep) {
        selectedStep.style.display = 'block';
    }

    // Cambiar el estado de los botones en el stepwizard
    const stepButtons = document.querySelectorAll('.stepwizard-step .btn-circle');
    stepButtons.forEach(button => {
        if (parseInt(button.getAttribute('data-step')) === stepNumber) {
            button.classList.remove('btn-secondary');
            button.classList.add('btn-primary');
        } else {
            button.classList.remove('btn-primary');
            button.classList.add('btn-secondary');
        }
    });
}

function goToPreviousStep(stepNumber) {
    // Ocultar todos los steps
    const steps = document.querySelectorAll('.setup-content');
    steps.forEach(step => {
        step.style.display = 'none';
    });

    // Mostrar el step seleccionado (anterior)
    const selectedStep = document.getElementById(`step-${stepNumber}`);
    if (selectedStep) {
        selectedStep.style.display = 'block';
    }

    // Cambiar el estado de los botones en el stepwizard
    const stepButtons = document.querySelectorAll('.stepwizard-step .btn-circle');
    stepButtons.forEach(button => {
        if (parseInt(button.getAttribute('data-step')) === stepNumber) {
            button.classList.remove('btn-secondary');
            button.classList.add('btn-primary');
        } else {
            button.classList.remove('btn-primary');
            button.classList.add('btn-secondary');
        }
    });
}


//Speech
var recognition;
var recognizing = false;

function toggleDictation(textareaId, iconId) {
    if (recognizing) {
        stopDictation(iconId);
    } else {
        startDictation(textareaId, iconId);
    }
}

function startDictation(textareaId, iconId) {
    if (window.hasOwnProperty('webkitSpeechRecognition')) {
        recognition = new webkitSpeechRecognition();

        recognition.continuous = true; // Permite que la grabación sea continua
        recognition.interimResults = false;

        recognition.lang = "es-ES"; // Cambia el idioma según sea necesario

        recognition.onstart = function () {
            recognizing = true;
            updateIconState(iconId);
            console.log("Reconocimiento de voz iniciado. Por favor, hable.");
        };

        recognition.onresult = function (event) {
            const newText = event.results[event.results.length - 1][0].transcript;
            document.getElementById(textareaId).value += ' ' + newText; // Concatena al texto existente
        };

        recognition.onerror = function (event) {
            console.error("Error en el reconocimiento de voz: ", event.error);
        };

        recognition.onend = function () {
            recognizing = false;
            updateIconState(iconId);
            console.log("El reconocimiento de voz ha finalizado.");
        };

        recognition.start();
    } else {
        alert("Tu navegador no soporta el reconocimiento de voz.");
    }
}

function stopDictation(iconId) {
    if (recognition && recognizing) {
        recognizing = false;
        recognition.stop();
        updateIconState(iconId);
        console.log("Reconocimiento de voz detenido.");
    }
}

function updateIconState(iconId) {
    var icon = document.getElementById(iconId);

    if (recognizing) {
        icon.classList.remove('fa-microphone');
        icon.classList.add('fa-microphone-slash');
    } else {
        icon.classList.remove('fa-microphone-slash');
        icon.classList.add('fa-microphone');
    }
}

// Asignar eventos de clic a los iconos
document.getElementById('dictationIcon1').addEventListener('click', function () {
    toggleDictation('antecedentespersonalesConsulta', 'dictationIcon1');
});

document.getElementById('dictationIcon2').addEventListener('click', function () {
    toggleDictation('enfermedadProblema', 'dictationIcon2');
});

document.getElementById('dictationIconPlan').addEventListener('click', function () {
    toggleDictation('plantratamiento_consulta', 'dictationIconPlan');
});

document.getElementById('dictationIconReconofarmacologicas').addEventListener('click', function () {
    toggleDictation('reconofarmacologicas', 'dictationIconReconofarmacologicas');
});

document.getElementById('dictationIconSignosAlarma').addEventListener('click', function () {
    toggleDictation('alergias_consulta', 'dictationIconSignosAlarma');
});
// Asignar eventos de clic al icono
document.getElementById('dictationIconObservacion').addEventListener('click', function () {
    toggleDictation('observacion_consulta', 'dictationIconObservacion');
});

//Tarjetitas

$('.select2-tags').select2({
    placeholder: "Buscar",
    width: '100%'
});

function updateTags(selectId, containerId) {
    const selectedOptions = $(selectId).select2('data');
    const container = $(containerId);
    container.empty(); // Limpiar el contenedor antes de agregar los nuevos tags

    selectedOptions.forEach(option => {
        if (option.id) {
            const tag = $('<span class="tag"></span>').text(option.text);
            const removeIcon = $('<span class="remove-tag">&times;</span>').click(function () {
                const optionElement = $(selectId).find(`option[value='${option.id}']`);
                optionElement.prop('selected', false).trigger('change');
            });
            tag.append(removeIcon);
            container.append(tag);
        }
    });
}

$('#tipoAlergiaSelect').on('change', function () {
    updateTags('#tipoAlergiaSelect', '#alergiasTagsContainer');
});

$('#tipoCirugiaSelect').on('change', function () {
    updateTags('#tipoCirugiaSelect', '#cirugiasTagsContainer');
});

// Inicializa las tarjetitas en caso de que haya valores preseleccionados
updateTags('#tipoAlergiaSelect', '#alergiasTagsContainer');
updateTags('#tipoCirugiaSelect', '#cirugiasTagsContainer');

// Función para mostrar u ocultar la observación según el estado del switch
$('.consulta-antecedente-checked').on('change', function () {
    var id = $(this).attr('id').replace('consulta-antecedente-checked-', '');
    var targetObservacion = $('#consulta-observacion-' + id);

    if ($(this).is(':checked')) {
        targetObservacion.show();
        targetObservacion.find('input, select').prop('disabled', false);
    } else {
        targetObservacion.hide();
        targetObservacion.find('input, select').prop('disabled', true);
    }
});

// Inicializa el estado de los campos al cargar la página
$('.consulta-antecedente-checked').each(function () {
    $(this).trigger('change');
});

//Preecion Arterial
document.getElementById('presionArterial').addEventListener('input', function (e) {
    let value = e.target.value.replace(/\D/g, ''); // Elimina cualquier caracter que no sea un dígito

    if (value.length > 3) {
        value = value.slice(0, 3) + '/' + value.slice(3); // Inserta el '/'
    }

    e.target.value = value; // Actualiza el campo de entrada con el nuevo valor

    // Opcional: Si deseas actualizar los campos ocultos para diastólica y sistólica
    if (value.length >= 5) {
        document.getElementById('PresionarterialdiastolicaConsulta').value = value.slice(4, 6);
        document.getElementById('PresionarterialsistolicaConsulta').value = value.slice(0, 3);
    } else {
        document.getElementById('PresionarterialdiastolicaConsulta').value = '';
        document.getElementById('PresionarterialsistolicaConsulta').value = value.slice(0, 3);
    }
});

//Tablas de a;adir DIAGNOSTICO
document.getElementById('anadirFila').addEventListener('click', function () {
    // Obtener el diagnóstico seleccionado y su texto
    var selectDiagnostico = document.getElementById('DiagnosticoId');
    var diagnosticoId = selectDiagnostico.value;
    var diagnosticoTexto = selectDiagnostico.options[selectDiagnostico.selectedIndex].text;

    if (diagnosticoId === "") {
        alert("Seleccione un diagnóstico antes de añadir.");
        return;
    }

    // Crear elementos HTML para la nueva fila
    var tr = document.createElement('tr');

    // Columna de Diagnóstico
    var tdDiagnostico = document.createElement('td');
    tdDiagnostico.textContent = diagnosticoTexto;
    tr.appendChild(tdDiagnostico);

    // Columna de Presuntivo/Definitivo con estilo toggle
    var tdTipo = document.createElement('td');
    var divGroup = document.createElement('div');
    divGroup.className = "btn-group btn-group-toggle";
    divGroup.setAttribute('data-toggle', 'buttons');

    var labelPresuntivo = document.createElement('label');
    labelPresuntivo.className = "btn btn-outline-primary";
    var checkboxPresuntivo = document.createElement('input');
    checkboxPresuntivo.type = "checkbox";
    checkboxPresuntivo.name = "Diagnosticos[" + diagnosticoId + "].PresuntivoDiagnosticos"; // Nombre para el binding del modelo
    checkboxPresuntivo.autocomplete = "off";
    labelPresuntivo.appendChild(checkboxPresuntivo);
    labelPresuntivo.append(" Presuntivo");

    var labelDefinitivo = document.createElement('label');
    labelDefinitivo.className = "btn btn-outline-primary";
    var checkboxDefinitivo = document.createElement('input');
    checkboxDefinitivo.type = "checkbox";
    checkboxDefinitivo.name = "Diagnosticos[" + diagnosticoId + "].DefinitivoDiagnosticos"; // Nombre para el binding del modelo
    checkboxDefinitivo.autocomplete = "off";
    labelDefinitivo.appendChild(checkboxDefinitivo);
    labelDefinitivo.append(" Definitivo");

    divGroup.appendChild(labelPresuntivo);
    divGroup.appendChild(labelDefinitivo);

    tdTipo.appendChild(divGroup);
    tr.appendChild(tdTipo);

    // Columna para el botón de eliminar
    var tdEliminar = document.createElement('td');
    var btnEliminar = document.createElement('button');
    btnEliminar.className = "btn btn-outline-danger";
    btnEliminar.innerHTML = '<i class="fas fa-trash"></i>';
    btnEliminar.addEventListener('click', function () {
        this.closest('tr').remove();
    });
    tdEliminar.appendChild(btnEliminar);
    tr.appendChild(tdEliminar);

    // Añadir la fila a la tabla
    document.getElementById('diagnosticoTableBody').appendChild(tr);

    // Campos ocultos para enviar el ID del diagnóstico y su observación
    var hiddenInputDiagnosticoId = document.createElement('input');
    hiddenInputDiagnosticoId.type = "hidden";
    hiddenInputDiagnosticoId.name = "Diagnosticos[" + diagnosticoId + "].DiagnosticoId"; // Nombre para el binding del modelo
    hiddenInputDiagnosticoId.value = diagnosticoId;
    tr.appendChild(hiddenInputDiagnosticoId);

    // Opcional: limpiar el select después de añadir
    selectDiagnostico.value = "";
});

//Tablas de a;adir medicamento
document.getElementById('anadirFilaMedicamento').addEventListener('click', function () {
    // Obtener el medicamento seleccionado y su texto
    var selectMedicamento = document.getElementById('MedicamentoId');
    var medicamentoId = selectMedicamento.value;
    var medicamentoTexto = selectMedicamento.options[selectMedicamento.selectedIndex].text;

    if (medicamentoId === "") {
        alert("Seleccione un medicamento antes de añadir.");
        return;
    }

    // Crear elementos HTML para la nueva fila
    var tr = document.createElement('tr');

    // Columna de Medicamento
    var tdMedicamento = document.createElement('td');
    tdMedicamento.textContent = medicamentoTexto;
    tr.appendChild(tdMedicamento);

    // Columna de Cantidad
    var tdCantidad = document.createElement('td');
    var inputCantidad = document.createElement('input');
    inputCantidad.type = "number";
    inputCantidad.className = "form-control";
    inputCantidad.name = "Medicamentos[" + medicamentoId + "].Cantidad"; // Nombre para el binding del modelo
    inputCantidad.min = "1";
    inputCantidad.placeholder = "Cantidad";
    tdCantidad.appendChild(inputCantidad);
    tr.appendChild(tdCantidad);

    // Columna de Observación
    var tdObservacion = document.createElement('td');
    var inputObservacion = document.createElement('input');
    inputObservacion.type = "text";
    inputObservacion.className = "form-control";
    inputObservacion.name = "Medicamentos[" + medicamentoId + "].ObservacionMedicamento"; // Nombre para el binding del modelo
    inputObservacion.placeholder = "Observación";
    tdObservacion.appendChild(inputObservacion);
    tr.appendChild(tdObservacion);

    // Columna para el botón de eliminar
    var tdEliminar = document.createElement('td');
    var btnEliminar = document.createElement('button');
    btnEliminar.className = "btn btn-outline-danger";
    btnEliminar.innerHTML = '<i class="fas fa-trash"></i>';
    btnEliminar.addEventListener('click', function () {
        this.closest('tr').remove();
    });
    tdEliminar.appendChild(btnEliminar);
    tr.appendChild(tdEliminar);

    // Añadir la fila a la tabla
    document.getElementById('medicamentosTableBody').appendChild(tr);

    // Campo oculto para enviar el ID del medicamento
    var hiddenInputMedicamentoId = document.createElement('input');
    hiddenInputMedicamentoId.type = "hidden";
    hiddenInputMedicamentoId.name = "Medicamentos[" + medicamentoId + "].MedicamentoId"; // Nombre para el binding del modelo
    hiddenInputMedicamentoId.value = medicamentoId;
    tr.appendChild(hiddenInputMedicamentoId);

    // Opcional: limpiar el select después de añadir
    selectMedicamento.value = "";
});

//Tabla a;adir imagenes
document.getElementById('anadirFilaImagen').addEventListener('click', function () {
    // Obtener la imagen seleccionada y su texto
    var selectImagen = document.getElementById('ImagenId');
    var imagenId = selectImagen.value;
    var imagenTexto = selectImagen.options[selectImagen.selectedIndex].text;

    if (imagenId === "") {
        alert("Seleccione una imagen antes de añadir.");
        return;
    }

    // Crear elementos HTML para la nueva fila
    var tr = document.createElement('tr');

    // Columna de Imagen
    var tdImagen = document.createElement('td');
    tdImagen.textContent = imagenTexto;
    tr.appendChild(tdImagen);

    // Columna de Cantidad
    var tdCantidad = document.createElement('td');
    var inputCantidad = document.createElement('input');
    inputCantidad.type = "number";
    inputCantidad.className = "form-control";
    inputCantidad.name = "Imagenes[" + imagenId + "].CantidadImagen"; // Nombre para el binding del modelo
    inputCantidad.min = "1";
    inputCantidad.placeholder = "Cantidad";
    tdCantidad.appendChild(inputCantidad);
    tr.appendChild(tdCantidad);

    // Columna de Observación
    var tdObservacion = document.createElement('td');
    var inputObservacion = document.createElement('input');
    inputObservacion.type = "text";
    inputObservacion.className = "form-control";
    inputObservacion.name = "Imagenes[" + imagenId + "].ObservacionImagen"; // Nombre para el binding del modelo
    inputObservacion.placeholder = "Observación";
    tdObservacion.appendChild(inputObservacion);
    tr.appendChild(tdObservacion);

    // Columna para el botón de eliminar
    var tdEliminar = document.createElement('td');
    var btnEliminar = document.createElement('button');
    btnEliminar.className = "btn btn-outline-danger";
    btnEliminar.innerHTML = '<i class="fas fa-trash"></i>';
    btnEliminar.addEventListener('click', function () {
        this.closest('tr').remove();
    });
    tdEliminar.appendChild(btnEliminar);
    tr.appendChild(tdEliminar);

    // Campo oculto para enviar el ID de la imagen
    var hiddenInputImagenId = document.createElement('input');
    hiddenInputImagenId.type = "hidden";
    hiddenInputImagenId.name = "Imagenes[" + imagenId + "].ImagenId"; // Nombre para el binding del modelo
    hiddenInputImagenId.value = imagenId;
    tr.appendChild(hiddenInputImagenId);

    // Añadir la fila a la tabla
    document.getElementById('imagenesTableBody').appendChild(tr);

    // Opcional: limpiar el select después de añadir
    selectImagen.value = "";
});

//tabla a;adir laboratorio
document.getElementById('anadirFilaLaboratorio').addEventListener('click', function () {
    // Obtener el laboratorio seleccionado y su texto
    var selectLaboratorio = document.getElementById('LaboratorioId');
    var laboratorioId = selectLaboratorio.value;
    var laboratorioTexto = selectLaboratorio.options[selectLaboratorio.selectedIndex].text;

    if (laboratorioId === "") {
        alert("Seleccione un laboratorio antes de añadir.");
        return;
    }

    // Crear elementos HTML para la nueva fila
    var tr = document.createElement('tr');

    // Columna de Laboratorio
    var tdLaboratorio = document.createElement('td');
    tdLaboratorio.textContent = laboratorioTexto;
    tr.appendChild(tdLaboratorio);

    // Columna de Cantidad
    var tdCantidad = document.createElement('td');
    var inputCantidad = document.createElement('input');
    inputCantidad.type = "number";
    inputCantidad.className = "form-control";
    inputCantidad.name = "Laboratorios[" + laboratorioId + "].CantidadLaboratorio"; // Nombre para el binding del modelo
    inputCantidad.min = "1";
    inputCantidad.placeholder = "Cantidad";
    tdCantidad.appendChild(inputCantidad);
    tr.appendChild(tdCantidad);

    // Columna de Observación
    var tdObservacion = document.createElement('td');
    var inputObservacion = document.createElement('input');
    inputObservacion.type = "text";
    inputObservacion.className = "form-control";
    inputObservacion.name = "Laboratorios[" + laboratorioId + "].Observacion"; // Nombre para el binding del modelo
    inputObservacion.placeholder = "Observación";
    tdObservacion.appendChild(inputObservacion);
    tr.appendChild(tdObservacion);

    // Columna para el botón de eliminar
    var tdEliminar = document.createElement('td');
    var btnEliminar = document.createElement('button');
    btnEliminar.className = "btn btn-outline-danger";
    btnEliminar.innerHTML = '<i class="fas fa-trash"></i>';
    btnEliminar.addEventListener('click', function () {
        this.closest('tr').remove();
    });
    tdEliminar.appendChild(btnEliminar);
    tr.appendChild(tdEliminar);

    // Campo oculto para enviar el ID del laboratorio
    var hiddenInputLaboratorioId = document.createElement('input');
    hiddenInputLaboratorioId.type = "hidden";
    hiddenInputLaboratorioId.name = "Laboratorios[" + laboratorioId + "].CatalogoLaboratorioId"; // Nombre para el binding del modelo
    hiddenInputLaboratorioId.value = laboratorioId;
    tr.appendChild(hiddenInputLaboratorioId);

    // Añadir la fila a la tabla
    document.getElementById('laboratorioTableBody').appendChild(tr);

    // Opcional: limpiar el select después de añadir
    selectLaboratorio.value = "";
});

document.getElementById('consultationForm').addEventListener('submit', async function (event) {
    event.preventDefault(); // Evita que el formulario se envíe de forma tradicional

    // Capturamos los valores del formulario
    const fechaCreacion = document.getElementById('FechacreacionConsulta').value;
    const usuarioCreacion = document.getElementById('usuarioNombre').value;
    const medicoID = document.getElementById('medicoId').value;
    const especialidadID = document.getElementById('especialidadId').value;
    const pacienteID = document.getElementById('idPaciente').value;
    const motivo = document.getElementById('motivoConsulta').value;
    const enfermedad = document.getElementById('enfermedadProblema').value;
    const nombrePariente = document.getElementById('acompañante').value;
    const telefono = document.getElementById('telefonoPariente').value;
    const temperatura = document.getElementById('temperatura_consulta').value;
    const frecuenciaRespiratoria = document.getElementById('frecuenciarespiratoria_consulta').value;
    const presionArterialSistolica = document.getElementById('PresionarterialsistolicaConsulta').value;
    const presionArterialDiastolica = document.getElementById('PresionarterialdiastolicaConsulta').value;
    const pulso = document.getElementById('pulso_consulta').value;
    const peso = document.getElementById('peso_consulta').value;
    const talla = document.getElementById('talla_consulta').value;
    const planTratamiento = document.getElementById('plantratamiento_consulta').value;
    const diasIncapacidad = document.getElementById('diasincapacidad_consulta').value;

    // Validaciones
    if (!fechaCreacion || !usuarioCreacion || !medicoID || !especialidadID || !pacienteID || !motivo || !enfermedad || !telefono || !temperatura || !frecuenciaRespiratoria || !presionArterialSistolica || !presionArterialDiastolica || !pulso || !peso || !talla || !planTratamiento) {
        alert("Por favor, complete todos los campos obligatorios.");
        return;
    }

    if (isNaN(parseInt(medicoID, 10)) || isNaN(parseInt(especialidadID, 10)) || isNaN(parseInt(pacienteID, 10))) {
        alert("Algunos de los identificadores no son válidos. Verifique los datos ingresados.");
        return;
    }

    if (isNaN(parseFloat(temperatura)) || isNaN(parseFloat(frecuenciaRespiratoria)) || isNaN(parseFloat(presionArterialSistolica)) || isNaN(parseFloat(presionArterialDiastolica)) || isNaN(parseFloat(pulso)) || isNaN(parseFloat(peso)) || isNaN(parseFloat(talla))) {
        alert("Por favor, ingrese valores numéricos válidos en los campos de signos vitales y antropometría.");
        return;
    }

    if (diasIncapacidad && isNaN(parseInt(diasIncapacidad, 10))) {
        alert("El campo de días de incapacidad debe ser un número válido.");
        return;
    }

    const consultaDto = {
        FechaCreacion: fechaCreacion,
        UsuarioCreacion: usuarioCreacion,
        MedicoID: parseInt(medicoID, 10),
        EspecialidadID: parseInt(especialidadID, 10),
        PacienteID: parseInt(pacienteID, 10),
        Motivo: motivo,
        Enfermedad: enfermedad,
        NombrePariente: nombrePariente,
        SignosAlarma: document.getElementById('signosAlarma').value,
        ReconocFarmacologicas: document.getElementById('reconofarmacologicas').value,
        TipoParienteID: parseInt(document.getElementById('tipoParienteSelect').value, 10),
        Telefono: telefono,
        Temperatura: temperatura,
        FrecuenciaRespiratoria: frecuenciaRespiratoria,
        PresionArterialSistolica: presionArterialSistolica,
        PresionArterialDiastolica: presionArterialDiastolica,
        Pulso: pulso,
        Peso: peso,
        Talla: talla,
        PlanTratamiento: planTratamiento,
        Observacion: document.getElementById('observacion_consulta').value,
        DiasIncapacidad: diasIncapacidad ? parseInt(diasIncapacidad, 10) : null,
        // Asegúrate de agregar todos los campos que necesitas
    };

    try {
        // Envío de la solicitud al servidor usando fetch
        const response = await fetch(consultaUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ consultaDto: consultaDto }) // Envolviendo el objeto en un JSON con clave `consultaDto`
        });

        const result = await response.json(); // Parseamos la respuesta a JSON

        if (response.ok) {
            // Maneja el caso de éxito
            console.log('Consulta creada exitosamente:', result);
            // Podrías redirigir o mostrar un mensaje de éxito al usuario aquí
        } else {
            // Maneja el caso de error
            console.error('Error al crear la consulta:', result);
            // Aquí podrías manejar los errores, como mostrar mensajes de error en el formulario
        }
    } catch (error) {
        // Maneja errores de la solicitud
        console.error('Error de la solicitud:', error);
    }
});


