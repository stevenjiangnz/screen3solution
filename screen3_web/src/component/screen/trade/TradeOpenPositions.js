import React, { Component } from "react";
import TickerHelper from "../../../util/TickerHelper";

export class TradeOpenPositions extends Component {
  constructor(props) {
    super(props);
  }

  getColor = (diff) => {
    if (diff > 0) {
      return "gain";
    }

    if (diff < 0) {
      return "loss";
    }

    return;
  };
  getPL = (trade, ticker) => {
    if (
      ticker.t === trade.code &&
      ticker.p >= trade.entryDate &&
      trade.entryPrice !== 0
    ) {
      const mid = (ticker.l + ticker.h) / 2;
      const diff = (
        ((ticker.c - trade.entryPrice) / trade.entryPrice) *
        trade.direction *
        100
      ).toFixed(2);
      return (
        <>
          <span> | </span> <span className={this.getColor(diff)}> {diff}%</span>
        </>
      );
    } else {
      return null;
    }
  };

  render() {
    const { trades, ticker } = this.props;

    return (
      <div style={{ marginTop: 10 }}>
        <h5>Open Positions ({trades.length}):</h5>
        <div>
          <ui className="list-group">
            {trades.map((trade) => {
              const isCloseable =
                ticker &&
                ticker.t === trade.code &&
                ticker.p >= trade.entryDate;
              return (
                <li
                  className="list-group-item d-flex justify-content-between align-items-center"
                  key={trade.id}
                >
                  {`${trade.code}, ${trade.direction > 0 ? "+" : "-"}, ${
                    trade.entryDate
                  }, ${trade.entryPrice}`}{" "}
                  {this.getPL(trade, ticker)}
                  <span
                    style={{ cursor: "pointer" }}
                    onClick={() => this.props.onTradeClose(trade, ticker)}
                  >
                    <button
                      className="btn btn-primary btn-sm"
                      type="button"
                      disabled={!isCloseable}
                    >
                      X
                    </button>
                  </span>
                </li>
              );
            })}
          </ui>
        </div>
      </div>
    );
  }
}

export default TradeOpenPositions;
