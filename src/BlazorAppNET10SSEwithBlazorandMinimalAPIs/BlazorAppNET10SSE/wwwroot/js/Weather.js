
let eventSource = null;

export function InitEventSource() {
    StopEventSource();
    eventSource = new EventSource('https://localhost:6001/sse-json-weatherforecast');

    eventSource.addEventListener('weatherforecasts', (event) => {
        // console.log(event);
        const data = JSON.parse(event.data);
        // console.log(data);

        const timeElement = document.getElementById('time');
        timeElement.innerHTML = `${data.timestamp} ${data.heartRate}`;

        const table = document.getElementById('dataTable');

        const itemElements = document.getElementsByName("item");
        for (let i = itemElements.length - 1; i >= 0; i--) {
            itemElements[i].remove();
        }

        const wfs = data.weatherForecasts;
        wfs.forEach(forecast => {
            const tr = document.createElement('tr');
            tr.setAttribute('name', 'item');

            const tddate = document.createElement('td');
            tddate.textContent = forecast.date;
            tr.appendChild(tddate);
            const tdsummary = document.createElement('td');
            tdsummary.textContent = forecast.summary;
            tr.appendChild(tdsummary);
            const tdtemperatureC = document.createElement('td');
            tdtemperatureC.textContent = forecast.temperatureC;
            tr.appendChild(tdtemperatureC);
            const tdtemperatureF = document.createElement('td');
            tdtemperatureF.textContent = forecast.temperatureF;
            tr.appendChild(tdtemperatureF);
            table.appendChild(tr);
        });

    });

    eventSource.onopen = () => {
        console.log('Connection opened');
    };

    eventSource.onmessage = (event) => {
        console.log('message:', event);
    };

    eventSource.onerror = () => {
        if (eventSource.readyState === EventSource.CONNECTING) {
            console.log('Reconnecting...');
        }
    };

};

export function StopEventSource() {
    if (eventSource != null) {
        eventSource.close();
        eventSource = null;
    }
};