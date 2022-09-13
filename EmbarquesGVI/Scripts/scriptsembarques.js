$(function () {
    var type;
    $("#plaza").keydown(function (event) {
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
        $('#plaza').autocomplete({
            source: function (request, response) {
                $.ajax({
                    url: "/embarques/GetPlazas",
                    type: "POST",
                    dataType: "json",
                    data: { text: request.term, type: type },
                    success: function (data) {
                        response($.map(data, function (item) {
                            return { label: item.U_Plaza + " - " + item.SlpName, value: item.SlpCode };
                        }))
                    }
                })
            }
        });
    }

    $('#plaza').change(function () {
        
    });
});