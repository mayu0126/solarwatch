import "./HomePage.css";
import { Link } from "react-router-dom";
import { useContext } from 'react';
import { UserContext } from '../../index.js';
import SolarWatch from "../SolarWatch";

function HomePage() {
  const context  = useContext(UserContext);
  return (
    <div className="main-container">
      {context.user ? (
        <>
          <Link to="/" className="solar">
            SOLAR
          </Link>
          <Link to="/" className="watch">
            WATCH
          </Link>
          <div className='welcome-text'>Welcome <b>{context.user.userName}</b>!</div>
        </>
      ):(
        <>
        <Link to="/login" className="solar">
          SOLAR
        </Link>
        <Link to="/login" className="watch">
          WATCH
        </Link>
        </>
      )}

      <div className="main-message-1">
        Get the sunrise and sunset data for your favourite cities
      </div>
      {context.user ? (
        <>
          <SolarWatch/>
        </>
      ) : (
        <div className="main-message-2">
        - Sign In or Register to use the amazing SolarWatch app -
        </div>
      )}
    </div>
    );
}

export default HomePage;