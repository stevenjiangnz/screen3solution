import React, { Component } from "react";
import StockList from "./StockList";

export class Stock extends Component {
  render() {
    return (
      <div>
        this is the stock page...
        <StockList />
      </div>
    );
  }
}

export default Stock;
