var chart3;

var chart3Data = [
    {
        "category": "",
        "excelent": 20,
        "good": 20,
        "average": 20,
        "poor": 20,
        "bad": 20,
        "limit": 80,
        "full": 100,
        "bullet": 75
    }
];


AmCharts.ready(function () {

    // FIRST BULLET CHART
    // bullet chart3 is a simple serial chart3
    chart3 = new AmCharts.AmSerialChart();
    chart3.dataProvider = chart3Data;
    chart3.categoryField = "category";
    chart3.rotate = true; // if you want vertical bullet chart3, set rotate to false
    chart3.columnWidth = 1;
    chart3.startDuration = 1;

    // AXES
    // category
    var categoryAxis = chart3.categoryAxis;
    categoryAxis.gridAlpha = 0;

    // value
    var valueAxis = new AmCharts.ValueAxis();
    valueAxis.maximum = 100;
    valueAxis.axisAlpha = 1;
    valueAxis.gridAlpha = 0;
    valueAxis.stackType = "regular"; // we use stacked graphs to make color fills
    chart3.addValueAxis(valueAxis);

    // this graph displays the short dash, which usually indicates maximum value reached.
    var graph = new AmCharts.AmGraph();
    graph.valueField = "limit";
    graph.lineColor = "#000000";
    // it's a step line with no risers
    graph.type = "step";
    graph.noStepRisers = true;
    graph.lineAlpha = 1;
    graph.lineThickness = 3;
    graph.columnWidth = 0.5; // change this if you want wider dash
    graph.stackable = false; // this graph shouldn't be stacked
    chart3.addGraph(graph);

    // The following graphs produce color bands
    graph = new AmCharts.AmGraph();
    graph.valueField = "bad";
    graph.lineColor = "#fb7116";
    graph.showBalloon = false;
    graph.type = "column";
    graph.fillAlphas = 0.8;
    chart3.addGraph(graph);

    graph = new AmCharts.AmGraph();
    graph.valueField = "poor";
    graph.lineColor = "#f6d32b";
    graph.showBalloon = false;
    graph.type = "column";
    graph.fillAlphas = 0.8;
    chart3.addGraph(graph);

    graph = new AmCharts.AmGraph();
    graph.valueField = "average";
    graph.lineColor = "#f4fb16";
    graph.showBalloon = false;
    graph.type = "column";
    graph.fillAlphas = 0.8;
    chart3.addGraph(graph);

    graph = new AmCharts.AmGraph();
    graph.valueField = "good";
    graph.lineColor = "#b4dd1e";
    graph.showBalloon = false;
    graph.type = "column";
    graph.fillAlphas = 0.8;
    chart3.addGraph(graph);

    graph = new AmCharts.AmGraph();
    graph.valueField = "excelent";
    graph.lineColor = "#19d228";
    graph.showBalloon = false;
    graph.type = "column";
    graph.fillAlphas = 0.8;
    chart3.addGraph(graph);

    // this is the "bullet" graph - black bar showing current value
    graph = new AmCharts.AmGraph();
    graph.valueField = "bullet";
    graph.lineColor = "#000000";
    graph.type = "column";
    graph.lineAlpha = 1;
    graph.fillAlphas = 1;
    graph.columnWidth = 0.3; // this makes it narrower than color graphs
    graph.stackable = false; // bullet graph should not stack
    graph.clustered = false; // this makes the trick - one column above another
    chart3.addGraph(graph);

    // WRITE
    chart3.write("bullet3");

});