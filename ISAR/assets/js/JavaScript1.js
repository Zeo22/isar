var linechart1Data = [
                {
                    "lineColor": "#ffc321",
                    "date": "2012-01-01",
                    "duration": 408
                },
                {
                    "date": "2012-02-01",
                    "duration": 482
                },
                {
                    "date": "2012-03-01",
                    "duration": 562
                },
                {
                    "date": "2012-04-01",
                    "duration": 379
                },
                {
            
                    "date": "2012-05-01",
                    "duration": 501
                },
                {
                    "date": "2012-06-01",
                    "duration": 443
                },
                {
                    "date": "2012-07-01",
                    "duration": 405
                },
                {
                    "date": "2012-08-01",
                    "duration": 309
                },
                {
                    "date": "2012-09-10",
                    "duration": 287
                },
                {
                    "date": "2012-10-01",
                    "duration": 485
                },
                {
                    "date": "2012-11-01",
                    "duration": 890
                },
                {
                    "date": "2012-12-01",
                    "duration": 810
                }
];
var linechart1;

AmCharts.ready(function () {
    // SERIAL CHART
    linechart1 = new AmCharts.AmSerialChart();
    linechart1.dataProvider = linechart1Data;
    linechart1.categoryField = "date";
    linechart1.dataDateFormat = "YYYY-MM-DD";

    var balloon = linechart1.balloon;
    balloon.cornerRadius = 6;
    balloon.adjustBorderColor = false;
    balloon.horizontalPadding = 10;
    balloon.verticalPadding = 10;

    // AXES
    // category axis
    var categoryAxis = linechart1.categoryAxis;
    categoryAxis.parseDates = true; // as our data is date-based, we set parseDates to true
    categoryAxis.minPeriod = "MM"; // our data is daily, so we set minPeriod to DD
    categoryAxis.autoGridCount = false;
    categoryAxis.gridCount = 50;
    categoryAxis.gridAlpha = 0;
    categoryAxis.gridColor = "#000000";
    categoryAxis.axisColor = "#555555";
    // we want custom date formatting, so we change it in next line
    categoryAxis.dateFormats = [{

        period: 'MM',
        format: 'MMM DD'
    }];

    // as we have data of different units, we create two different value axes
    // Duration value axis
    var durationAxis = new AmCharts.ValueAxis();
    durationAxis.gridAlpha = 0.05;
    durationAxis.axisAlpha = 0;
    // the following line makes this value axis to convert values to duration
    // it tells the axis what duration unit it should use. mm - minute, hh - hour...
    durationAxis.duration = "mm";
    durationAxis.durationUnits = {
        DD: "d. ",
        hh: "h ",
        mm: "min",
        ss: ""
    };
    linechart1.addValueAxis(durationAxis);


    // GRAPHS
    // duration graph
    var durationGraph = new AmCharts.AmGraph();
    durationGraph.title = "duration";
    durationGraph.valueField = "duration";
    durationGraph.type = "line";
    durationGraph.valueAxis = durationAxis; // indicate which axis should be used
    durationGraph.lineColorField = "lineColor";
    durationGraph.fillColorsField = "lineColor";
    durationGraph.fillAlphas = 0.3;
    durationGraph.balloonText = "[[value]]";
    durationGraph.lineThickness = 1;
    durationGraph.legendValueText = "[[value]]";
    durationGraph.bullet = "square";
    durationGraph.bulletBorderThickness = 1;
    durationGraph.bulletBorderAlpha = 1;
    linechart1.addGraph(durationGraph);

    // CURSOR
    var linechart1Cursor = new AmCharts.ChartCursor();
    linechart1Cursor.zoomable = false;
    linechart1Cursor.categoryBalloonDateFormat = "YYYY MMM DD";
    linechart1Cursor.cursorAlpha = 0;
    linechart1.addChartCursor(linechart1Cursor);


    var linechart1Scrollbar = new AmCharts.ChartScrollbar();
    linechart1.addChartScrollbar(linechart1Scrollbar);

    // WRITE
    linechart1.write("linechart1div");

})