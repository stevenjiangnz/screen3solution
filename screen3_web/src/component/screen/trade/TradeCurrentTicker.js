import React, { Component } from "react";
import TickerHelper from "../../../util/TickerHelper";

export class TradeCurrentTicker extends Component {
  constructor(props) {
    super(props);
  }
  render() {
    const ticker = this.props.ticker;
    if (ticker && ticker.t) {
      return (
        <div style={{ marginTop: 10 }}>
          <h5>Current Ticker:</h5>
          <div>
            Code: {ticker.t} &nbsp; &nbsp; Date: {ticker.p} &nbsp; &nbsp; Mid:{" "}
            {((ticker.h + ticker.l) / 2).toFixed(4)}
          </div>

          <div>
            Open: {ticker.o} &nbsp; High: {ticker.h} &nbsp; Low: {ticker.l}{" "}
            &nbsp; Close: {ticker.c}
          </div>
        </div>
      );
    } else {
      return null;
    }
  }
}

export default TradeCurrentTicker;
