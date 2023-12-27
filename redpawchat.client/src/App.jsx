import { useEffect, useState } from 'react';
import './App.css';

function App() {
    const [forecasts, setForecasts] = useState();

    useEffect(() => {
        populateWeatherData();
    }, []);

    const contents = forecasts === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                {forecasts}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tabelLabel">Weather forecast</h1>
            <p>This component demonstrates fetching data from the server.</p>
          
        </div>
    );
    
    async function populateWeatherData() {
        try {
            const response = await fetch('https://localhost:5123/api/account/getall', {
                method: 'GET',
                credentials: 'include',
                headers: {
                    "Content-Type": "application/json",
                    'Accept': 'application/json', 
                }
            });
        
            const data = await response.json();
            console.log(data);
           
        } catch (error) {
            console.error('Error fetching data:', error);
        }
    }
}

export default App;