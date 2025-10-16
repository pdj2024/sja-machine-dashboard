window.renderMultiLineChart = (labels, datasets) => {
    const ctx = document.getElementById("chart-diagram").getContext("2d");

    if (window.multiLineChart) {
        window.multiLineChart.destroy();
    }

    const customPlugin = {
        id: 'customCanvasBackgroundColor',
        beforeDraw: (chart, args, options) => {
            const { ctx } = chart;
            ctx.save();
            ctx.globalCompositeOperation = 'destination-over';
            ctx.fillStyle = options.color || '#99ffff';
            ctx.fillRect(0, 0, chart.width, chart.height);
            ctx.restore();
        }
    };

    window.multiLineChart = new Chart(ctx, {
        type: "line",
        data: {
            labels: labels,
            datasets: datasets.map((ds, index) => ({
                label: ds.label,
                data: ds.data,
                borderColor: ds.color,
                backgroundColor: ds.backgroundColor ?? "transparent",
                borderWidth: 2,
                tension: 0.2,
                pointBackgroundColor: ds.color,
                pointRadius: 3,
                fill: false
            }))
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                customCanvasBackgroundColor: {
                    color: 'transparent',
                },
                legend: {
                    display: true,
                    position: 'bottom',
                    labels: { color: '#444', boxWidth: 20 }
                }
            },
            interaction: { intersect: false, mode: 'index' },
            scales: {
                y: {
                    beginAtZero: true,
                    grid: { color: '#eee' },
                    ticks: { color: '#737373' }
                },
                x: {
                    grid: { display: false },
                    ticks: { color: '#737373' }
                }
            }
        },
        plugins: [customPlugin]
    });
};
