
window.initializeBarChart = (canvasId, labels, data, title, isHorizontal) => {
    const ctx = document.getElementById(canvasId);
    if (!ctx || ctx.dataset.chartInitialized) return;

    new Chart(ctx, {
        type: "bar",
        data: {
            labels: labels,
            datasets: [
                {
                    label: title,
                    tension: 0.4,
                    borderWidth: 0,
                    pointRadius: 0,
                    backgroundColor: "#fb6340",
                    data: data,
                    barThickness: 50,
                    maxBarThickness: 80,
                },
            ],
        },
        options: {
            indexAxis: isHorizontal ? "y" : "x",
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false },
                tooltip: { mode: "index", intersect: false },
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: { color: "#8898aa", font: { family: "Open Sans", size: 16, color: "#141715" } },
                    grid: { borderDash: [2] }
                },
                x: {
                    ticks: { color: "#8898aa", font: { family: "Open Sans", size: 16, color: "#141715" } },
                    grid: { display: false }
                }
            }
        }
    });

    ctx.dataset.chartInitialized = true;
};
