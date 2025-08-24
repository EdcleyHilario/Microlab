document.addEventListener("DOMContentLoaded", function () {
    // ==============================
    // Gráfico principal multi-eixos
    // ==============================
    const canvas = document.getElementById("graficoExamesMultiAxis");
    if (canvas) {
        const labels = JSON.parse(canvas.getAttribute("data-labels"));
        const urinaData = JSON.parse(canvas.getAttribute("data-urina"));
        const fezesData = JSON.parse(canvas.getAttribute("data-fezes"));

        const ctx = canvas.getContext("2d");

        new Chart(ctx, {
            type: "bar",
            data: {
                labels: labels,
                datasets: [
                    {
                        label: "SUMÁRIO DE URINA",
                        data: urinaData,
                        backgroundColor: "rgba(54, 162, 235, 0.6)",
                        borderColor: "rgba(54, 162, 235, 1)",
                        borderWidth: 1,
                        yAxisID: "y"
                    },
                    {
                        label: "PARACITOLÓGICO DE FEZES",
                        data: fezesData,
                        backgroundColor: "rgba(255, 206, 86, 0.6)",
                        borderColor: "rgba(255, 206, 86, 1)",
                        borderWidth: 1,
                        yAxisID: "y1"
                    }
                ]
            },
            options: {
                responsive: true,
                interaction: { mode: "index", intersect: false },
                stacked: false,
                plugins: {
                    legend: { position: "top" },
                    title: { display: true, text: "Pacientes por Tipo de Exame" }
                },
                scales: {
                    y: {
                        type: "linear",
                        display: true,
                        position: "left",
                        title: { display: true, text: "Urina" }
                    },
                    y1: {
                        type: "linear",
                        display: true,
                        position: "right",
                        grid: { drawOnChartArea: false },
                        title: { display: true, text: "Fezes" }
                    }
                }
            }
        });
    }
    document.addEventListener("DOMContentLoaded", function () {

        function renderMiniChart(canvasId, type = "bar", color = "rgba(255,255,255,0.9)") {
            const canvas = document.getElementById(canvasId);
            if (!canvas) return;

            // Pega os dados do HTML
            const labels = JSON.parse(canvas.dataset.labels || "[]");
            const values = JSON.parse(canvas.dataset.values || "[]");

            console.log(`MiniChart: ${canvasId}`);
            console.log("Labels:", labels);
            console.log("Values:", values);

            if (labels.length === 0 || values.length === 0) {
                console.warn(`Gráfico ${canvasId} não renderizado: dados ausentes.`);
                return;
            }

            // Ajusta altura mínima do canvas se necessário
            canvas.height = 80;

            new Chart(canvas.getContext("2d"), {
                type: type,
                data: {
                    labels: labels,
                    datasets: [{
                        data: values,
                        backgroundColor: type === "bar" ? color : "transparent",
                        borderColor: color,
                        borderWidth: 2,
                        borderRadius: type === "bar" ? 6 : 0,
                        fill: type === "line",
                        tension: 0.3,
                        pointRadius: type === "line" ? 3 : 0,
                        pointBackgroundColor: color
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    plugins: { legend: { display: false } },
                    scales: { x: { display: false }, y: { display: false } },
                    elements: { line: { borderJoinStyle: "round" } }
                }
            });
        }

        // Inicializa todos os mini-gráficos
        renderMiniChart("miniChartUrina", "bar", "rgba(255,255,255,0.9)");       // card azul
        renderMiniChart("miniChartFeces", "bar", "rgba(0,0,0,0.7)");             // card amarelo
        renderMiniChart("miniChartLiberados", "bar", "rgba(255,255,255,0.9)");   // card verde
        renderMiniChart("miniChartSolicitados", "bar", "rgba(255,255,255,0.9)"); // card vermelho

    });
