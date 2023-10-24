import "./SolarWatch.css";
import { Link } from "react-router-dom";
import { useContext } from 'react';
import { UserContext } from '../../index.js';

function SolarWatch() {
  const { user, setUser, login, logout } = useContext(UserContext);

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
      </>
    ):(null)}
    </div>
  );
}

export default SolarWatch;