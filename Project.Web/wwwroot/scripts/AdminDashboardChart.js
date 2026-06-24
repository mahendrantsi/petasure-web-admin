
//function GetTotalTransactionChart(rawData) {

//    var xData = rawData.map(item => item.dateStr); // Month is 0-indexed in JS
//    var yData = rawData.map(item => item.totalAmount);

//    Highcharts.chart('Container1', {
//        chart: {
//            type: 'area',
//            height: 290
//        },
//        title: {
//            text: null
//        },
//        exporting: {
//            enabled: false
//        },
//        credits: {
//            enabled: false
//        },
//        xAxis: {
//            categories: xData
//        },
//        yAxis: {
//            title: {
//                text: null
//            }
//        },
//        tooltip: {
//            pointFormat: '{series.name} ₤{point.y:,.0f}'
//        },
//        plotOptions: {
//            area: {
//                marker: {
//                    enabled: false,
//                    symbol: 'circle',
//                    radius: 2,
//                    states: {
//                        hover: {
//                            enabled: true
//                        }
//                    }
//                }
//            }
//        },
//        series: [{
//            name: 'Total Amount',
//            data: xData.map((time, index) => [time, yData[index]])
//        }]
//    });
//}


function GetTotalTransactionChart(rawData) {
    var xData = rawData.map(item => item.dateStr);
    var yData = rawData.map(item => item.totalAmount);

    var seriesData = xData.map((time, index) => {
        return { x: time, y: yData[index] };
    });

    var options = {
        chart: {
            type: 'area',
            height: 290,
        },
        title: {
           /* text: 'Total Transaction Amount Over Time'*/
        },
        xaxis: {
            categories: xData,
            title: {
                text: 'Date'
            }
        },
        yaxis: {
            title: {
                text: 'Total Amount (₤)'
            }
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return '₤' + val.toFixed(0).replace(/\d(?=(\d{3})+$)/g, '$&,'); // Formats y-axis value
                }
            }
        },
        series: [{
            name: 'Total Amount',
            data: seriesData
        }],
        markers: {
            size: 2,
            hover: {
                size: 6
            }
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            curve: 'smooth'
        },
        toolbar: {
            show: false // Hide toolbar
        }
    };

    var chart = new ApexCharts(document.querySelector("#Container1"), options);
    chart.render();
}
//function GetTotalUsersChart(rawData) {
//    //var rawData = [{
//    //    "year": 2022,
//    //    "month": 1,
//    //    "totalUsers": 85,
//    //}];
   
//    var xData = rawData.map(item => item.dateStr); 
//    var yData = rawData.map(item => item.transactionCount);

///*    console.log(xData,12)*/
//    Highcharts.chart('Container2', {
//        chart: {
//            type: 'column',
//            height: 300
//        },
//        title: {
//            align: 'left',
//            text: null
//        },
//        exporting: {
//            enabled: false
//        },
//        credits: {
//            enabled: false
//        },
//        accessibility: {
//            announceNewData: {
//                enabled: true
//            }
//        },
//        xAxis: {
//            categories: xData,
//        },
//        //xAxis: {
//        //    type: 'datetime',
//        //    dateTimeLabelFormats: {
//        //        month: '%b %Y' 
//        //    }
//        //},
//        yAxis: {
//            title: {
//                text: null
//            }

//        },
//        legend: {
//            enabled: false
//        },
//        plotOptions: {
//            series: {
//                borderWidth: 0,
//                dataLabels: {
//                    enabled: true,
//                    format: '{point.y}'
//                }

//            }
//        },

//        tooltip: {
//            headerFormat: '<span style="font-size:11px"><b  style="color:{point.color}">{point.y}</b> {series.name}</span><br>',
//            pointFormat: '<span style="color:{point.color}"><b>{point.name}</span>'
//        },

//        series: [{
//            name: 'Transactions',
//            data: xData.map((time, index) => [time, yData[index]])
//        }],
//        drilldown: {
//            breadcrumbs: {
//                position: {
//                    align: 'right'
//                }
//            },
//            series: [
              

//            ]
//        }
//    });
//}
function GetTotalUsersChart(rawData) {
    var xData = rawData.map(item => item.dateStr);
    var yData = rawData.map(item => item.transactionCount);

    var seriesData = xData.map((time, index) => {
        return { x: time, y: yData[index] };
    });

    var options = {
        chart: {
            type: 'bar',
            height: 300,
            toolbar: {
                show: false
            }
        },
        title: {
            /*text: null,*/
            align: 'left'
        },
        xaxis: {
            categories: xData,
            title: {
                text: 'Date'
            }
        },
        yaxis: {
            title: {
                text: 'Total Transactions'
            }
        },
        plotOptions: {
            bar: {
                horizontal: false,
                columnWidth: '55%',
                dataLabels: {
                    position: 'top',
                }
            }
        },
        tooltip: {
            enabled: true,
            shared: true,
            intersect: false,
            y: {
                formatter: function (val) {
                    return val + " transactions";
                }
            }
        },
        series: [{
            name: 'Transactions',
            data: seriesData
        }],
        legend: {
            show: false
        }
    };

    var chart = new ApexCharts(document.querySelector("#Container2"), options);
    chart.render();
}

//function GetFailedTransactionChart(totalSpend, totalReceived) {

//    Highcharts.chart('Container3', {
//        chart: {
//            plotBackgroundColor: null,
//            plotBorderWidth: 0,
//            plotShadow: false
//        },
//        title: {
//            text: 'Total Users',
//            align: 'center',
//            verticalAlign: 'middle',
//            y: 60
//        },
//        colors: ['#0b4a46', '#ED561B', '#DDDF00'],
//        tooltip: {
//            pointFormat: 'Share: <b>{point.percentage:.1f}%</b>'
//        }, exporting: {
//            enabled: false
//        },
//        credits: {
//            enabled: false
//        },
//        accessibility: {
//            point: {
//                valueSuffix: '%'
//            }
//        },
//        plotOptions: {
//            pie: {
//                dataLabels: {
//                    enabled: true,
//                    distance: -50,
//                    style: {
//                        fontWeight: 'bold',
//                        color: 'white'
//                    }
//                },
//                startAngle: -90,
//                endAngle: 90,
//                center: ['50%', '75%'],
//                size: '100%'
//            }
//        },
//        series: [{
//            type: 'pie',
//            name: 'Browser share',
//            innerSize: '50%',
//            data: [
//                ['Merchant', totalReceived],
//                ['Users', totalSpend],

//                //{
//                //    name: 'Other',
//                //    y: 3.77,
//                //    dataLabels: {
//                //        enabled: false
//                //    }
//                //}
//            ]
//        }]
//    });

//}


function GetFailedTransactionChart(totalSpend, totalReceived) {
    var totalTransactions = totalSpend + totalReceived;
    var userPercentage = parseFloat(((totalSpend / totalTransactions) * 100).toFixed(2));
    var merchantPercentage = parseFloat(((totalReceived / totalTransactions) * 100).toFixed(2)) ;
     
    //var peruser = userPercentage.toFixed(0);
    //var permarchant = merchantPercentage.toFixed(0);

    var options = {
        /*series: [totalReceived, totalSpend],*/
        chart: {
            type: 'pie',
            height: 300,
        },
        title: {
           /* text: 'Total Users',*/
            align: 'left',
            offsetY: 60,
            style: {
                fontSize: '16px'
            }
        },
        labels: ['Merchant', 'Users'],
        series: [merchantPercentage, userPercentage],
        colors: ['#0b4a46', '#ED561B'],
        tooltip: {
            enabled: true,
            y: {
                formatter: function (value) {
                    console.log(value, 123123);
                    return value + '%';
                }
            }
        },

        dataLabels: {
            enabled: true,
            formatter: function (val, opts) {
                if (opts.seriesIndex === 0) {
                    return 'Merchant: ' + totalReceived.toFixed(0);
                } else {
                    return 'User:' + totalSpend.toFixed(0);
                    /*return 'Users: ' + val.toFixed(0) + '%';*/
                }
            },
            style: {
                colors: ['#fff'],
                fontSize: '14px',
                fontWeight: 'bold'
            }
        },
        plotOptions: {
            pie: {
                startAngle: -90,
                endAngle: 270
            }
        },
        legend: {
            show: false
        },
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    height: 300
                },
                legend: {
                    position: 'bottom'
                }
            }
        }]
    };

    var chart = new ApexCharts(document.querySelector("#Container3"), options);
    chart.render();
}

//function GetFailedTransactionChart(totalSpend, totalReceived) {
//    var totalTransactions = totalSpend + totalReceived;
//    var userPercentage = (totalSpend / totalTransactions) * 100;
//    var merchantPercentage = (totalReceived / totalTransactions) * 100;

//    var options = {
//        chart: {
//            type: 'pie',
//            height: 300,
//        },
//        title: {
//            text: 'Total Users',
//            align: 'left',
//            offsetY: 60,
//            style: {
//                fontSize: '16px'
//            }
//        },
//        series: [{
//            /*name: 'Total Users',*/
//            data: [
//                { name: 'Merchant', y: totalReceived },
//                { name: 'Users', y: totalSpend }
//            ]
//        }],
//        colors: ['#0b4a46', '#ED561B'],
//        tooltip: {
//            enabled: true,
//            formatter: function (value, { point }) {
//                var percentage = (value / totalTransactions) * 100;
//                return point.name + ': ' + percentage.toFixed(1) + '%';
//            }
//        },
//        dataLabels: {
//            enabled: true,
//            formatter: function (val, opts) {
//                if (opts.seriesIndex === 0) {
//                    return 'Merchant: ' + val.toFixed(0) + '%';
//                } else {
//                    return 'Users: ' + val.toFixed(0) + '%';
//                }
//            },
//            style: {
//                colors: ['#fff'],
//                fontSize: '14px',
//                fontWeight: 'bold'
//            }
//        },
//        plotOptions: {
//            pie: {
//                startAngle: -90,
//                endAngle: 270
//            }
//        },
//        legend: {
//            show: false
//        },
//        responsive: [{
//            breakpoint: 480,
//            options: {
//                chart: {
//                    height: 300
//                },
//                legend: {
//                    position: 'bottom'
//                }
//            }
//        }]
//    };

//    var chart = new ApexCharts(document.querySelector("#Container3"), options);
//    chart.render();
//}