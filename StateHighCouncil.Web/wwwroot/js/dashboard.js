//import Chart from '/Chart.js/chart.umd.js'

function billCountsByParty() {
    (async () => {
        const response = await fetch('/Api/Stats/BillCountsByParty');
        const data = await response.json();
        repData = [data.republican[0].value, data.republican[1].value];
        demData = [data.democrat[0].value, data.democrat[1].value];
        const ctx = document.getElementById('billsByParty');
        const repColor = '#FF0000';
        const demColor = '#0000FF';

        var options = {
            chart: {
                type: 'bar'
            },
            colors: [repColor, demColor],
            xaxis: {
                categories: ["Filed", "Passed"]
            },
            fill: {
                opacity: .6
            },
            title: {
                text: "Number of bills filed by party",
                align: 'left'
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
                data: repData 
            }, {
                name: "Democrat",
                data: demData 
            }]
        }

        var chart = new ApexCharts(ctx, options);
        chart.render();
    })()
}

function topNLegislators(legCount) {
    (async () => {
        const response = await fetch('/Api/Stats/topnlegislators/?count=' + legCount);
        const data = await response.json();

        const repColor = '#FF0000';
        const demColor = '#0000FF';
        const neutralColor = '#006400';
        var barData = [];
        var barColors = [];
        var barLabels = [];

        for (var i = 0; i < data.length; i++) {
            barData[barData.length] = data[i].value;
            barLabels[barLabels.length] = data[i].description;
            barColors[barColors.length] = (data[i].party === 'R' ? neutralColor : demColor);
        }
        
        var options = {
            chart: {
                type: 'bar'
            },
            fill: {
                opacity: .6
            },
            title: {
                text: "Top 10 Legislators by bills filed",
                align: 'left'
            },
            colors: barColors,
            
            dataLabels:
            {
                enabled: true,
                textAnchor: 'start'
            },
            plotOptions: {
                bar: {
                    horizontal: true                    
                }
            },
            xaxis:
            {
                categories: barLabels
            },
            series: [{
                data: barData
            }]
        }

        const ctx = document.getElementById('top10Legislators');
        var chart = new ApexCharts(ctx, options);
        chart.render();
    })()
}

function topNSubjects(subjectCount) {
    (async () => {
        const response = await fetch('/Api/Stats/topnsubjects/?count=' + subjectCount);
        const data = await response.json();

        const repColor = '#FF0000';
        const demColor = '#0000FF';
        const neutralColor = '#006400';
        var barData = [];
        var barColors = [];
        var barLabels = [];

        for (var i = 0; i < data.length; i++) {
            barData[barData.length] = data[i].value;
            barLabels[barLabels.length] = data[i].description;
            barColors[barColors.length] = neutralColor;
        }
        
        var options = {
            chart: {
                type: 'bar'
            },
            fill: {
                opacity: .6
            },
            title: {
                text: 'Top 10 Subjects',
                align: 'left'
            },
            subtitle: {
                text: 'A bill can have more than one subject',
                align: 'left'
            },
            colors: barColors,

            dataLabels:
            {
                enabled: true,
                textAnchor: 'start'
            },
            plotOptions: {
                bar: {
                    horizontal: true
                }
            },
            xaxis:
            {
                categories: barLabels
            },
            series: [{
                data: barData
            }]
        }

        const ctx = document.getElementById('top10Subjects');
        var chart = new ApexCharts(ctx, options);
        chart.render();
    })()
}

function legislatorsByParty() {
    (async () => {
        const response = await fetch('/Api/Stats/LegislatorsByParty');
        const data = await response.json();
        repData = [data[2].value, data[3].value];
        demData = [data[0].value, data[1].value];
        
        const ctx = document.getElementById('legislatorsGender');
        const repColor = '#0000FF';
        const demColor = '#FFC0CB';

        var options = {
            chart: {
                type: 'bar'
            },
            colors: [repColor, demColor],
            xaxis: {
                categories: ["House", "Senate"]
            },
            fill: {
                opacity: .6
            },
            title: {
                text: "Legislators by Gender",
                align: 'left'
            },
            plotOptions: {
                bar: {
                    dataLabels: {
                        position: 'top'
                    }
                }
            },
            series: [{
                name: "Male",
                data: repData
            }, {
                name: "Female",
                data: demData
            }]
        }

        var chart = new ApexCharts(ctx, options);
        chart.render();
    })()
}