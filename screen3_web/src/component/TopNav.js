import React, { Component } from "react";
import { Link } from "react-router-dom";

export class TopNav extends Component {
  render() {
    return (
      <div>
        thisi s the top Nav
        <ul>
          <li>
            <Link to="/">Stock</Link>
          </li>
          <li>
            <Link to="/screen">Screen</Link>
          </li>
        </ul>
      </div>
    );
  }
}

export default TopNav;
