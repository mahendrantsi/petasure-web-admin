



function GetFailedTransactionChart(totalReceived, totalSpend) {
    var totalTransactions = totalSpend + totalReceived;
    var ReceivedPercentage = parseFloat(((totalReceived / totalTransactions) * 100).toFixed(2));
    var SpendPercentage = parseFloat(((totalSpend / totalTransactions) * 100).toFixed(2));

    var options = {
        chart: {
            type: 'donut',
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
        labels: ['Spend', 'Received'],
        series: [SpendPercentage, ReceivedPercentage],
        //colors: ['#0b4a46', '#ED561B'],
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
                    return 'Received: ' + totalSpend.toFixed(2);
                } else {
                    return 'Spend: ' + totalReceived.toFixed(2);
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
                endAngle: 270,
                donut: {
                    size: '70%',
                    labels: {
                        show: true,
                        name: {
                            show: true,
                            fontSize: '22px',
                            fontWeight: '600',
                            offsetY: -10
                        },
                        value: {
                            show: true,
                            fontSize: '16px',
                            fontWeight: '400',
                            offsetY: 16,
                            formatter: function (val) {
                                return val + '%';
                            }
                        },
                        total: {
                            show: true,
                            showAlways: true,
                            label: 'Total',
                            fontSize: '22px',
                            fontWeight: 600,
                            formatter: function (w) {
                                return (totalSpend + totalReceived).toFixed(2);
                            }
                        }
                    }
                }
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
function GetTotalTransactionChart(rawData, sendData) {
  
    var xData = rawData.map(item => item.dateStr);
    var yData = rawData.map(item => item.totalAmount);

    var xData2 = sendData.map(item => item.dateStr);
    var yData2 = sendData.map(item => item.totalAmount);

    var seriesData1 = xData.map((time, index) => {
        return { x: time, y: yData[index] };
    });

    var seriesData2 = xData2.map((time, index) => {
        return { x: time, y: yData2[index] };
    });

    var options = {
        chart: {
            type: 'area',
            height: 290
        },
        title: {
            /*text: null*/
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
                    return val.toFixed(2);
                }
            }
        },
        series: [{
            name: 'Send Money',
            data: seriesData1
        },
        {
            name: 'Receive Money',
            data: seriesData2
        }],
        markers: {
            size: 2,
            hover: {
                size: 6
            }
        },
        toolbar: {
            show: false // Hide the toolbar
        }
    };

    // Initializing ApexCharts instance
    var chart = new ApexCharts(document.querySelector("#Container1"), options);

    // Rendering the chart
    chart.render();
}

//changes the code 08-05-2024
function GetTotalUsersChart(rawData, sendData) {
    var xData = rawData.map(item => item.dateStr);
    var yData = rawData.map(item => item.transactionCount);

    var xData2 = sendData.map(item => item.dateStr);
    var yData2 = sendData.map(item => item.transactionCount);

    var seriesData1 = xData.map((time, index) => {
        return { x: time, y: yData[index] };
    });

    var seriesData2 = xData2.map((time, index) => {
        return { x: time, y: yData2[index] };
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
            name: 'Send Transactions',
            data: seriesData1
        },
        {
            name: 'Receive Transactions',
            data: seriesData2
        }],
        legend: {
            show: false
        }
    };

    var chart = new ApexCharts(document.querySelector("#Container2"), options);
    chart.render();
}
