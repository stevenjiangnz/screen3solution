import React, { Component } from "react";
import { Link } from "react-router-dom";

export class TopNav extends Component {
  render() {
    return (
      <div>
        <nav
          id="top-nav-bar"
          className="navbar navbar-default navbar-fixed-top"
        >
          <div className="top-menu">
            <ul>
              <li className="logo">
                <span className="navbar-brand">
                  <i className="fa fa-lg fa-chain "></i>Screen<span>.3</span>
                </span>
              </li>
              <li>
                <Link to="/stock">Stock</Link>
              </li>
              <li>
                <Link to="/screen">Screen</Link>
              </li>
            </ul>
          </div>
        </nav>
      </div>
    );
  }
}

export default TopNav;
