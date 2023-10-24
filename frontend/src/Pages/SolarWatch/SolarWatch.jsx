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

        let errorMessage = "Request failed";
        if (data && data.errors) {
          if (data.errors.date && data.errors.date[0]) {
            errorMessage = data.errors.date[0];
          }
          else if (data.errors.cityName && data.errors.cityName[0]) {
            errorMessage = data.errors.cityName[0];
          }
        }

        throw new Error(errorMessage);
      });
    }
    return res.json(); //if the response is "ok"
  });
};

function SolarWatch() {
  const { user, setUser, login, logout } = useContext(UserContext);
  const [loading, setLoading] = useState(false);
  const [isErrorMessageVisible, setIsErrorMessageVisible] = useState(false);
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
        setIsErrorMessageVisible(true);

        setTimeout(() => {
          setIsErrorMessageVisible(false);
        }, 3000);

      });
  };

  return (
    <div className="solarwatch-container">
      {user ? (
      <>
        <SolarWatchFeature
          onSave={handleSolarDataRequest}
          disabled={loading}
          errorMessage={errorMessage}
          isErrorMessageVisible={isErrorMessageVisible}
        />
        <div>{solarData ? (
          <div className="solar-data-container">
            <div className="sunrise-data">Sunrise: {solarData.sunrise}</div>
            <div className="sunset-data">Sunset: {solarData.sunset}</div>
          </div>
          ) : (null)}
        </div>
      </>
    ):(null)}
    </div>
  );
}

export default SolarWatch;