//import Chart from '/Chart.js/chart.umd.js'

function billCountsByParty() {
    (async() => {
        const response = await fetch('/Api/Stats/BillCountsByParty');
        const data = await response.json();
        
        const ctx = document.getElementById('billsByParty');
        
        new Chart(ctx, {
            type: 'bar',
            data: {

                labels: ['Republicans', 'Democrats'],
                datasets: [{
                    label: 'Filed',
                    data: data.map(row => row.Filed),
                    backgroundColor: '#0000ff50',
                    borderwidth: 1,
                    parsing: {
                        yAxisKey: 'Filed'
                    }
                    
                },
                {
                    label: 'Passed',
                    data: data.map(row => row.Passed),
                    backgroundColor: '#034e0050',
                    borderwidth: 1,
                    parsing: {
                        yAxisKey: 'Passed'
                    }
                }]
            },
            options: {
                plugins: {
                    title: {
                        display: true,
                        text: 'Number of Bills'
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });        
    })();
}
