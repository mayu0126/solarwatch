import "./SolarWatch.css";
import { Link } from "react-router-dom";
import { useState, useContext } from 'react';
import { UserContext } from '../../index.js';
import SolarWatchFeature from "../../Components/SolarWatchFeature/SolarWatchFeature";

const sendSolarDataRequest = (solarDataRequest, user) => {
  console.log(solarDataRequest);
  const url = process.env.REACT_APP_MY_URL;

  return fetch(`${url}/SolarWatch/GetSunriseAndSunset?cityName=${solarDataRequest.cityName}&date=${solarDataRequest.date}`,
  {
    method: 'GET',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + user.token //the space is necessary after "Bearer"
    },
  })
    .then((res) => {
    if (!res.ok) {
      return res.json().then((data) => {
        throw new Error("Request failed");
      });
    }
    return res.json(); //if the response is "ok"
  });
};

function SolarWatch() {
  const { user, setUser, login, logout } = useContext(UserContext);
  const [loading, setLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [solarData, setSolarData] = useState(null);

  const handleSolarDataRequest = (solarDataRequest) => {
    setLoading(true);
    sendSolarDataRequest(solarDataRequest, user)
      .then((data) => {
        setLoading(false);
        setSolarData(data); //set the solar data in the state
      })
      .catch((error) => {
        setLoading(false);
        console.error("Request error:", error.message);
        setErrorMessage(error.message);
      });
  };

  return (
    <div className="solarwatch-container">
      {user ? (
      <>
        <div className="solarwatch-message-1">
          Here comes the functionality
        </div>
        <div className="solarwatch-message-2">
          - For registered users only -
        </div>
        <div className="solarwatch-picture">
        </div>
        <SolarWatchFeature
          onSave={handleSolarDataRequest}
          disabled={loading}
          errorMessage={errorMessage}
        />
        <div>{solarData ? (
          <>
            <div>Sunrise: {solarData.sunrise}</div>
            <div>Sunset: {solarData.sunset}</div>
          </>
          ) : (null)}
        </div>
      </>
    ):(null)}
    </div>
  );
}

export default SolarWatch;