//import Chart from '/Chart.js/chart.umd.js'

function billCountsByParty() {
    (async () => {
        const response = await fetch('/Api/Stats/BillCountsByParty');
        const data = await response.json();
        repData = [data.republican[0].value, data.republican[1].value];
        demData = [data.democrat[0].value, data.democrat[1].value];
        const ctx = document.getElementById('billsByParty');

        var options = {
            chart: {
                type: 'bar'
            },
            colors: ['#FF0000', '#0000FF'],
            xaxis: {
                categories: ["Filed", "Passed"]
            },
            fill: {
                opacity: .6
            },
            title: {
                text: "Number of bills by party",
                align: 'center'
            },
            plotOptions: {
                bar: {
                    dataLabels: {
                        position: 'top'
                    }
                }
            },
            series: [{
                name: "Republican",
                data: repData //[700, 450]
            }, {
                name: "Democrat",
                data: demData //[250, 175]
            }]
        }

        var chart = new ApexCharts(ctx, options);
        chart.render();
    })()
}
