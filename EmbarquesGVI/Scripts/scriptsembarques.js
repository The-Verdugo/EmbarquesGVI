$(function () {
    var type;
    var total_cajas = 0;
    var total_temp = 0;
    $("#plazaD").keydown(function (event) {
        if (event.which >= 48 && event.which <= 57) //Ejecuta Query para cuando se escriben numeros
        {
            type = 'num';
            complete();
        }
        else if (event.which >= 65 && event.which <= 90) //Ejecuta Query para cuando se escriben letras
        {
            type = 'text'
            complete();
        }
    }); 

    function complete() {
        $('#plazaD').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/embarques/GetPlazas",
                    type: "POST",
                    dataType: "json",
                    data: { text: request.term, type: type },
                    success: function (data) {
                        response($.map(data.plazas, function (item) {
                            return { label: item.U_Plaza + " - " + item.SlpName, value: item.SlpName };
                        }))
                    }
                })
            }
        });
    }

    $('#plazaD').change(function () {
        var value = $('#plazaD').val();
        $.ajax({
            url: "/embarques/GetAlmacen",
            type: "POST",
            dataType: "json",
            data: { slp: value },
            success: function (data) {
                $('#almacen').val(data.U_AlmPrincipal);
                $('#plazaD').removeClass("bg-danger bg-opacity-75");
                $('#plazaD').addClass("bg-success bg-opacity-75 text-white");
                $('#plaza').val(data.SlpCode);
                transfer();
            },
            error: function (error) {
                toastr["error"]("Seleccione una plaza válida");
                toastr.options = {
                    "closeButton": true,
                    "progressBar": true,
                    "positionClass": "toast-top-right",
                    "preventDuplicates": false
                }
                $('#plazaD').removeClass("bg-success bg-opacity-75");
                $('#plazaD').addClass("bg-danger text-white bg-opacity-75");
                $('#almacen').val('');
                $('#trans').empty();
                $('#trans').selectpicker('refresh');

            }
        });
    });

    function transfer() {
        $.ajax({
            url: "/embarques/GetTransfer",
            type: "POST",
            dataType: "json",
            data: { alm: $('#almacen').val() },
            success: function (data) {
                $('#trans').empty();
                $.each(data, function (index, val) {
                    $("#trans").append($("<option>", {
                        value: val.DocEntry,
                        text: val.DocEntry
                    }));
                });
                $('#trans').selectpicker('refresh');
            },
            error: function (error) {
                alert('error');
            }
        });
    };

    $('#trans').selectpicker({
        noneSelectedText: 'Seleccionar Transferencias',
        noneResultsText: 'Sin resultados para {0}',
        selectedTextFormat: 'count',
        countSelectedText: '{0} Elementos seleccionados',
        actionsBox: true,
        deselectAllText: 'Desmarcar todo',
        selectAllText: 'Marcar todo',
        liveSearchPlaceholder: 'Buscar',
        tickIcon: 'bi bi-check-circle-fill'
    });

    //deteccion de cambios en textbox de cajas
    $('.cajas').change(function () {
        var importe_total = 0
        $(".cajas").each(
            function (index, value) {
                if ($.isNumeric($(this).val())) {
                    if ($(this).val() < 0) {
                        toastr["warning"]("No se pueden ingresar valores negativos");
                        toastr.options = {
                            "closeButton": true,
                            "progressBar": true,
                            "positionClass": "toast-top-right",
                            "preventDuplicates": false
                        }
                        $(this).val(0);
                    } else
                        importe_total += eval($(this).val());
                    //console.log(importe_total);
                } else {
                    $(this).val(0);
                    toastr["warning"]("Carácter no válido");
                    toastr.options = {
                        "closeButton": true,
                        "progressBar": true,
                        "positionClass": "toast-top-right",
                        "preventDuplicates": false
                    }
                }
                
                
            }
        );
        $("#Total_Paquetes").val(importe_total);
    });
    
});