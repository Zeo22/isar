$(document).ready(function () {
    $("#FechaInicio, #FechaFinal").datepicker({
        language: 'es',
        format: 'dd/mm/yyyy',
        autoclose: true
    });

    var calculate_week_range = function (e) {

        var input = e.currentTarget;

        // remove all active class
        $('body').find('.datepicker-days table tbody tr').removeClass('week-active');

        // add active class
        var tr = $('body').find('.datepicker-days table tbody tr td.active.day').parent();
        tr.addClass('week-active');

        // find start and end date of the week

        var date = e.date;
        var start_date = new Date(date.getFullYear(), date.getMonth(), date.getDate() - date.getDay());
        var end_date = new Date(date.getFullYear(), date.getMonth(), date.getDate() - date.getDay() + 6);
        $("#MetaFechaInicio").val($.fn.datepicker.DPGlobal.formatDate(start_date, "dd/mm/yyyy", "es"));
        $("#MetaFechaFinal").val($.fn.datepicker.DPGlobal.formatDate(end_date, "dd/mm/yyyy", "es"));
        // make a friendly string

        var friendly_string = $("#MetaFechaInicio").val() + ' a '
            + $("#MetaFechaFinal").val();

        //console.log(friendly_string);

        $(input).val(friendly_string);

    }

    var settings = {
        "sDom": "<'table-responsive't><'row'<'pull-right'p> <'pull-left'i>>",
        "sPaginationType": "bootstrap",
        "destroy": true,
        "scrollCollapse": true,
        "oLanguage": {
            "sLengthMenu": "_MENU_ ",
            "sInfo": "Mostrando items del <b>_START_ al _END_</b> de un total de _TOTAL_ items",
            "sEmptyTable": "No existe información para mostrar.",
            "sInfoEmpty": ""
        },
        "iDisplayLength": 10,
        "data": initial_data,
        "columns": [
            {
                name: "Dia", data: 'inicio', render: function (data, type, full, meta) {
                    if (full.inicio == full.fin) {
                        return full.inicio;
                    } else {
                        return full.inicio + ' a ' + full.fin;
                    }
                }
            },
            { name: "Meta", data: "meta", render: function (data, type, full, meta) { return data + ' ' + $("#UnidadDeMedida_ID_").children(':selected').text() } },
            {
                name: "Resultados", data: "resultados", render: function (data, type, full, meta) {
                    return data + ' ' + $("#UnidadDeMedida_ID_").children(':selected').text() +
                        '<a href="javascript: $.noop();" style="color:green" class="fa fa-edit pull-right" data-toggle="modal" data-target="#EditResultado"></a>'
                }
            },
            {
                name: "Cerrar<br />Meta", data: "metaCerrada", className: "text-center", render: function (data, type, full, meta) {
                    if (data) {
                        return '<a href="javascript: $.noop();" style="color:red" class="fa fa-lock"></a>';
                    }
                    else {
                        return '<a href="javascript: $.noop();" style="color:green" class="fa fa-check cus-cerrarmeta"></a>';
                    }
                }
            },
            {
                name: "Abrir<br />Meta", data: "metaCerrada", className: "text-center", render: function (data, type, full, meta) {
                    if (data) {
                        return '<a href="javascript: $.noop();" style="color:green" class="fa fa-unlock cus-abrirmeta"></a>';
                    }
                    else {
                        return '';
                    }
                }
            },
            {
                name: "Cerrar<br />Resultado", data: "resultadoCerrado", className: "text-center", render: function (data, type, full, meta) {
                    if (data) {
                        return '<a href="javascript: $.noop();" style="color:red" class="fa fa-lock"></a>';
                    }
                    else {
                        return '<a href="javascript: $.noop();" style="color:green" class="fa fa-check cus-cerrarresulado"></a>';
                    }
                }
            }
        ]
    };

    if ($("#AbrirMeta").length == 0) {
        for (var i = 0; i < settings.columns.length; i++) {
            if (settings.columns[i].name == "Abrir<br />Meta") {
                settings.columns.splice(i, 1);
            }
        }
    }

    $('#dtMetas').dataTable(settings);

    $('#dtMetas tbody').on('click', 'a.cus-cerrarmeta', function () {
        var table = $('#dtMetas').DataTable();
        var data = table
            .row($(this).parents('tr'))
            .data();

        data.metaCerrada = true;

        table
            .row($(this).parents('tr'))
            .data(data)
            .draw();
    });

    $('#dtMetas tbody').on('click', 'a.cus-cerrarresulado', function () {
        var table = $('#dtMetas').DataTable();
        var data = table
            .row($(this).parents('tr'))
            .data();

        data.resultadoCerrado = true;

        table
            .row($(this).parents('tr'))
            .data(data)
            .draw();
    });

    $('#dtMetas tbody').on('click', 'a.cus-abrirmeta', function () {
        var table = $('#dtMetas').DataTable();
        var data = table
            .row($(this).parents('tr'))
            .data();

        data.metaCerrada = false;

        table
            .row($(this).parents('tr'))
            .data(data)
            .draw();
    });

    $('#EditResultado').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget) // Button that triggered the modal
        var table = $('#dtMetas').DataTable();
        var index = data = table
            .row(button.parents('tr'))
            .index();
        var data = table
            .row(button.parents('tr'))
            .data();

        $("#MetaModal").val(data.meta);
        $("#ResultadoModal").val(data.resultados);
        $("#RowIndexMeta").val(index);

        if (data.metaCerrada) {
            $("#MetaModal").parents('div').addClass("disabled");
            $("#MetaModal").attr("disabled", "diabled")
        }
        else {
            $("#MetaModal").parents('div').removeClass("disabled");
            $("#MetaModal").removeAttr("disabled");
        }
        if (data.resultadoCerrado) {
            $("#ResultadoModal").parents('div').addClass("disabled");
            $("#ResultadoModal").attr("disabled", "diabled")
        }
        else {
            $("#ResultadoModal").parents('div').removeClass("disabled");
            $("#ResultadoModal").removeAttr("disabled");
        }
    })

    $("#EditarModal").on("click", function () {
        var meta = $("#MetaModal").val();
        var res = $("#ResultadoModal").val();
        var index = $("#RowIndexMeta").val();
        var table = $('#dtMetas').DataTable();
        var data = table
            .row(index)
            .data();

        data.meta = meta;
        data.resultados = res;

        table
           .row(index)
           .data(data)
           .draw();
    });

    $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {
        $("#lblUnidad").html($("#UnidadDeMedida_ID_").children(':selected').text());
        $("#lblUnidadMetaModal").html($("#UnidadDeMedida_ID_").children(':selected').text());
        $("#lblUnidadResModal").html($("#UnidadDeMedida_ID_").children(':selected').text());
        $("#lblFrecuencia").val($("#Frecuencia_ID_").children(':selected').text());

        $("#Fecha").val('');
        $("#Fecha").datepicker("remove");
        switch ($("#lblFrecuencia").val()){
            case "Diario":
                $("#Fecha").datepicker({
                    language: 'es',
                    format: 'dd/mm/yyyy',
                    autoclose: true
                }).on("changeDate", function (e) {
                    //alert($.fn.datepicker.DPGlobal.formatDate(e.date, "dd/mm/yyyy", "es"))
                    var date = e.date;
                    var start_date = date;
                    var end_date = date;
                    $("#MetaFechaInicio").val($.fn.datepicker.DPGlobal.formatDate(start_date, "dd/mm/yyyy", "es"));
                    $("#MetaFechaFinal").val($.fn.datepicker.DPGlobal.formatDate(end_date, "dd/mm/yyyy", "es"));
                    //alert($.fn.datepicker.DPGlobal.formatDate(start_date, "dd/mm/yyyy", "es"))
                    //alert($.fn.datepicker.DPGlobal.formatDate(end_date, "dd/mm/yyyy", "es"))
                });
                break;
            case "Semanal":
                $('#Fecha').datepicker({
                    language: 'es',
                    format: "dd/mm/yyyy",
                    autoclose: true
                }).on('show', function (e) {

                    var tr = $('body').find('.datepicker-days table tbody tr');

                    tr.mouseover(function () {
                        $(this).addClass('week');
                    });

                    tr.mouseout(function () {
                        $(this).removeClass('week');
                    });

                    calculate_week_range(e);

                }).on('hide', function (e) {
                    //console.log('date changed');
                    calculate_week_range(e);
                });
                break;
            case "Mensual":
                $('#Fecha').datepicker({
                    language: 'es',
                    format: "MM, yyyy",
                    minViewMode: 1,
                    autoclose: true
                }).on("changeDate", function (e) {
                    //alert($.fn.datepicker.DPGlobal.formatDate(e.date, "dd/mm/yyyy", "es"))
                    var date = e.date;
                    var start_date = new Date(date.getFullYear(), date.getMonth(), 1);
                    var end_date = new Date(date.getFullYear(), date.getMonth() + 1, 0);
                    $("#MetaFechaInicio").val($.fn.datepicker.DPGlobal.formatDate(start_date, "dd/mm/yyyy", "es"));
                    $("#MetaFechaFinal").val($.fn.datepicker.DPGlobal.formatDate(end_date, "dd/mm/yyyy", "es"));
                    //alert($.fn.datepicker.DPGlobal.formatDate(start_date, "dd/mm/yyyy", "es"))
                    //alert($.fn.datepicker.DPGlobal.formatDate(end_date, "dd/mm/yyyy", "es"))
                });
                break;
            case "Anual":
                $('#Fecha').datepicker({
                    language: 'es',
                    format: "yyyy",
                    minViewMode: 2,
                    autoclose: true
                }).on("changeDate", function (e) {
                    //alert($.fn.datepicker.DPGlobal.formatDate(e.date, "dd/mm/yyyy", "es"))
                    var date = e.date;
                    var start_date = new Date(date.getFullYear(), date.getMonth(), 1);
                    var end_date = new Date(date.getFullYear(), 12, 0);
                    $("#MetaFechaInicio").val($.fn.datepicker.DPGlobal.formatDate(start_date, "dd/mm/yyyy", "es"));
                    $("#MetaFechaFinal").val($.fn.datepicker.DPGlobal.formatDate(end_date, "dd/mm/yyyy", "es"));
                    //alert($.fn.datepicker.DPGlobal.formatDate(start_date, "dd/mm/yyyy", "es"))
                    //alert($.fn.datepicker.DPGlobal.formatDate(end_date, "dd/mm/yyyy", "es"))
                });
                break;
            default:
                $("#Fecha").datepicker({
                    language: 'es',
                    format: 'dd/mm/yyyy',
                    autoclose: true
                }).on("changeDate", function (e) {
                    //alert($.fn.datepicker.DPGlobal.formatDate(e.date, "dd/mm/yyyy", "es"))
                    var date = e.date;
                    var start_date = date;
                    var end_date = date;
                    $("#MetaFechaInicio").val($.fn.datepicker.DPGlobal.formatDate(start_date, "dd/mm/yyyy", "es"));
                    $("#MetaFechaFinal").val($.fn.datepicker.DPGlobal.formatDate(end_date, "dd/mm/yyyy", "es"));
                    //alert($.fn.datepicker.DPGlobal.formatDate(start_date, "dd/mm/yyyy", "es"))
                    //alert($.fn.datepicker.DPGlobal.formatDate(end_date, "dd/mm/yyyy", "es"))
                });
                break;
        }
    })

    $("#AgregarMeta").on("click", function () {
        if ($("#Meta").val() == "") return;
        var table = $('#dtMetas').DataTable();
        var inicio = $("#MetaFechaInicio").val();
        var fin = $("#MetaFechaFinal").val();
        var meta = $("#Meta").val();
        var exists = false;

        table.rows().every(function () {
            var data = this.data();

            if (data.inicio == inicio) {
                exists = true;
                return;
            }
        });

        if (exists) {
            $('body').pgNotification({
                style: 'flip',
                message: 'La Fecha seleccionada ya esta asignada.',
                type: 'danger'
            }).show();
            return;
        }

        if (isNaN(meta)) {
            $('body').pgNotification({
                style: 'flip',
                message: 'La Meta debe ser numérica.',
                type: 'danger'
            }).show();
            return;
        }

        table.row.add({
            inicio: inicio,
            fin: fin,
            meta: meta,
            resultados: 0,
            metaCerrada: false,
            resultadoCerrado: false,
            cerrarMeta: '<a href="javascript: $.noop();" style="color:red" class="fa fa-minus"></a>',
            abrirMeta: '<a href="javascript: $.noop();" style="color:red" class="fa fa-minus"></a>',
            cerrarResultado: '<a href="javascript: $.noop();" style="color:red" class="fa fa-minus"></a>'
        }).draw();
    });

    $("#Submit").on("click", function () {
        var table = $('#dtMetas').DataTable();
        var i = 0;
        table.rows().every(function () {
            var data = this.data();

            $('<input>').attr({
                type: 'hidden',
                //id: 'foo',
                name: 'Metas[' + i + '][ID]',
                value: typeof (data.id) == "undefined" ? 0 : data.id
            }).appendTo('form');
            $('<input>').attr({
                type: 'hidden',
                //id: 'foo',
                name: 'Metas[' + i + '][FechaInicio]',
                value: data.inicio
            }).appendTo('form');
            $('<input>').attr({
                type: 'hidden',
                //id: 'foo',
                name: 'Metas[' + i + '][FechaFin]',
                value: data.fin
            }).appendTo('form');
            $('<input>').attr({
                type: 'hidden',
                //id: 'foo',
                name: 'Metas[' + i + '][Meta]',
                value: data.meta
            }).appendTo('form');
            $('<input>').attr({
                type: 'hidden',
                //id: 'foo',
                name: 'Metas[' + i + '][Resultado]',
                value: data.resultados
            }).appendTo('form');
            $('<input>').attr({
                type: 'hidden',
                //id: 'foo',
                name: 'Metas[' + i + '][Cumplimiento]',
                value: (data.meta - data.resultados) / data.meta
            }).appendTo('form');
            $('<input>').attr({
                type: 'hidden',
                //id: 'foo',
                name: 'Metas[' + i + '][MetaCerrada]',
                value: data.metaCerrada
            }).appendTo('form');
            $('<input>').attr({
                type: 'hidden',
                //id: 'foo',
                name: 'Metas[' + i + '][ResultadoCerrado]',
                value: data.resultadoCerrado
            }).appendTo('form');
            i++;
        });
        $("form").submit();
    });
});