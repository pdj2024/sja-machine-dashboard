// ChartJS
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

// Table to CSV
window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? 'download';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
};


// Chart to PDF
//window.exportChart = (id) => {
//    const element = document.getElementById(id);

//    html2canvas(element, { allowTaint: true, useCORS: true }).then(canvas => {
//        const A4_WIDTH = 595.28;
//        const width = A4_WIDTH;
//        const height = (width / canvas.width) * canvas.height;
//        const image = canvas.toDataURL('image/png');
//        const doc = new jspdf.jsPDF('p', 'pt', 'a4');
//        doc.addImage(image, 'PNG', 0, 0, width, height);
//        doc.save('chart-export.pdf');
//    });
//};
