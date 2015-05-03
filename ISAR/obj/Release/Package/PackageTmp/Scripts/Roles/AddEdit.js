$(document).ready(function () {
    $('#myFormWizard').bootstrapWizard({
        onTabShow: function (tab, navigation, index) {
            var $total = navigation.find('li').length;
            var $current = index + 1;

            // If it's the last tab then hide the last button and show the finish instead
            if ($current >= $total) {
                $('#myFormWizard').find('.pager .next').hide();
                $('#myFormWizard').find('.pager .finish').show();
                $('#myFormWizard').find('.pager .finish').removeClass('disabled');
            } else {
                $('#myFormWizard').find('.pager .next').show();
                $('#myFormWizard').find('.pager .finish').hide();
            }

            var li = navigation.find('li.active');

            var btnNext = $('#myFormWizard').find('.pager .next').find('button');
            var btnPrev = $('#myFormWizard').find('.pager .previous').find('button');

            // remove fontAwesome icon classes
            function removeIcons(btn) {
                //btn.removeClass(function (index, css) {
                //    return (css.match(/(^|\s)fa-\S+/g) || []).join(' ');
                //});
            }

            if ($current > 1 && $current < $total) {

                var nextIcon = li.next().find('.fa');
                var nextIconClass = nextIcon.attr('class').match(/fa-[\w-]*/).join();

                removeIcons(btnNext);
                btnNext.addClass(nextIconClass + ' btn-animated from-left fa');

                var prevIcon = li.prev().find('.fa');
                var prevIconClass = prevIcon.attr('class').match(/fa-[\w-]*/).join();

                removeIcons(btnPrev);
                btnPrev.addClass(prevIconClass + ' btn-animated from-left fa');
            } else if ($current == 1) {
                // remove classes needed for button animations from previous button
                btnPrev.removeClass('btn-animated from-left fa');
                removeIcons(btnPrev);
            } else {
                // remove classes needed for button animations from next button
                btnNext.removeClass('btn-animated from-left fa');
                removeIcons(btnNext);
            }
        },
        onNext: function(){
            return $("form").validate().form();
        },
        onInit: function () {
            $('#myFormWizard ul').removeClass('nav-pills');
        }
    });

    $("#Pantallas").select2({
        minimumResultsForSearch: -1
    });

    var settings = {
        "sDom": "<'table-responsive't><'row'<'pull-right'p> <'pull-left'i>>",
        "sPaginationType": "bootstrap",
        "destroy": true,
        "scrollCollapse": true,
        "oLanguage": {
            "sLengthMenu": "_MENU_ ",
            "sInfo": "Showing <b>_START_ to _END_</b> of _TOTAL_ entries"
        },
        "iDisplayLength": 10,
        "data": initial_data,
        "columns": [
            { name: "Grupo", data: 'Grupo' },
            { name: "Nombre", data: "Nombre" },
            {
                name: "Lectura", data: "Lectura", render: function (data, type, full, meta) {
                    if (data) {
                        return '<div class="checkbox check-success">' +
                            '<input type="checkbox" disabled="disabled" checked="checked" value="1" id="checkbox1">' +
                            '<label for="checkbox1">&nbsp;</label>' +
                        '</div>'
                    }
                    else {
                        return '<div class="checkbox check-success">' +
                            '<input type="checkbox" disabled="disabled" value="1" id="checkbox1">' +
                            '<label for="checkbox1">&nbsp;</label>' +
                        '</div>'
                    }
                }
            },
            {
                name: "Escritura", data: "Escritura", render: function (data, type, full, meta) {
                    if (data) {
                        return '<div class="checkbox check-success">' +
                            '<input type="checkbox" disabled="disabled" checked="checked" value="1" id="checkbox1">' +
                            '<label for="checkbox1">&nbsp;</label>' +
                        '</div>'
                    }
                    else {
                        return '<div class="checkbox check-success">' +
                            '<input type="checkbox" disabled="disabled" value="1" id="checkbox1">' +
                            '<label for="checkbox1">&nbsp;</label>' +
                        '</div>'
                    }
                }
            },
            { name: "Eliminar", data: "Eliminar" }
        ]
    };

    $('#dtTable').dataTable(settings);

    $("#btnAdd").on("click", function () {
        if ($("#Pantallas").val() == null) return;
        var table = $('#dtTable').DataTable();
        var option = $("#Pantallas").prop("selectedOptions")[0];
        var grupo = option.parentElement.label;
        var pantalla = option.label;
        var exists = false;

        table.rows().every(function () {
            var data = this.data();

            if (data.PantallaId == $("#Pantallas").val()) {
                exists = true;
                return;
            }
        });

        if (exists) {
            $('body').pgNotification({
                style: 'flip',
                message: 'La Pantalla seleccionada ya esta asignada al Rol.',
                type: 'danger'
            }).show();
            return;
        }

        table.row.add({
            Grupo: grupo,
            Nombre: pantalla,
            Lectura: $("#Lectura").prop("checked"),
            Escritura: $("#Escritura").prop("checked"),
            Eliminar: '<a href="#" style="color:red" class="fa fa-minus"></a>',
            PantallaId: $("#Pantallas").val()
        }).draw();

        //$('#Pantallas option:selected').remove();
        //$('#Pantallas').select2("val", "");
    });

    $('#dtTable tbody').on('click', 'a.fa-minus', function () {
        var table = $('#dtTable').DataTable();

        table
            .row($(this).parents('tr'))
            .remove()
            .draw();
    });

    $("form").validate({
        submitHandler: function (form) {
            if (!$("form").validate().form()) return false;
            var table = $('#dtTable').DataTable();
            var data = table.rows().data();
            var permisos = [];

            for (var i = 0; i < data.length; i++) {
                permisos[i] = data[i];
                delete permisos[i].Eliminar;
            }
            var rol = {
                Id: $("#Id").val(),
                Nombre: $("#Name").val(),
                Permisos: permisos
            }
            $.post(action_url, rol);
        }
    });
});