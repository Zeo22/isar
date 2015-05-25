$(document).ready(function () {
    $("#SelectedPeriod").on("change", function () {
        //alert($("#SelectedPeriod").val());

        $.post("/Periodos/ChangeSelectedPeriod", "periodId=" + $("#SelectedPeriod").val(), function () {
            location.reload(true);
        });
    });
})