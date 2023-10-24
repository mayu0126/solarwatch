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
                <ul>
                    <div className="main-logo">
                    </div>
                    <div className="menu-container">
                        <li className="menu">
                            <Link to="/" className="link">Home</Link>
                        </li>
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
                            <div>
                                Welcome {context.user.userName}!
                            </div>
                            </>
                        )}
                    </div>
                </ul>
            </nav>
        <Outlet />
        </div>
    );
};
export default Layout;
