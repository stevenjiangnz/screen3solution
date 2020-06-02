import React, { Component } from "react";
import StockList from "../stock/StockList";
export class Screen extends Component {
  render() {
    return (
      <div className="row">
        <div className="col-sm-3">"left"</div>
        <div className="col-sm-7">middle</div>
        <div className="col-sm-2">right</div>
      </div>
    );
  }
}

export default Screen;
