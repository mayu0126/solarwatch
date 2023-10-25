import { useContext } from 'react';
import { Outlet, Link } from "react-router-dom";
import { UserContext } from '../../index.js';

import "./Layout.css";

const Layout = () => {
    const context  = useContext(UserContext);

    /*
    const handleLogout = () => {
        context.setUser(null);
    };
    */

    return (
        <div className="Layout">
            <nav>
                    <div className="main-logo">
                    </div>
                    <div className="menu-container">
                        <div className="home">
                            <Link to="/" className="link">HOME</Link>
                        </div>
                        {!context.user && (
                            <>
                            <Link to="/register">
                                <button type="button">Register</button>
                            </Link>
                            <Link to="/login">
                                <button type="button">Log In</button>
                            </Link>
                            </>
                        )}
                        {context.user && (
                            <>
                            <Link to="/">
                                <button type="button" onClick={context.logout}>Log Out</button>
                            </Link>
                            </>
                        )}
                    </div>
            </nav>
        <Outlet />
        </div>
    );
};
export default Layout;
