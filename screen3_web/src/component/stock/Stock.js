import React, { Component } from "react";
import StockList from "./StockList";
import StockDetail from "./StockDetail";

export class Stock extends Component {
  render() {
    return (
      <div id="main-panel" className="container-fluid">
        <div className="row">
          <div className="col-sm-3">
            <StockList />
          </div>
          <div className="col-sm-9">
            <StockDetail />
          </div>
        </div>
      </div>
    );
  }
}

export default Stock;
