import React, { Component } from "react";
import TickerHelper from "../../../util/TickerHelper";

export class TradeClosedPositions extends Component {
  constructor(props) {
    super(props);
  }

  //   getColor = (diff) => {
  //     if (diff > 0) {
  //       return "gain";
  //     }

  //     if (diff < 0) {
  //       return "loss";
  //     }

  //     return;
  //   };
  //   getPL = (trade, ticker) => {
  //     if (
  //       ticker.t === trade.code &&
  //       ticker.p >= trade.entryDate &&
  //       trade.entryPrice !== 0
  //     ) {
  //       const mid = (ticker.l + ticker.h) / 2;
  //       const diff = (
  //         ((mid - trade.entryPrice) / trade.entryPrice) *
  //         trade.direction *
  //         100
  //       ).toFixed(2);
  //       return (
  //         <>
  //           <span> | </span> <span className={this.getColor(diff)}> {diff}%</span>
  //         </>
  //       );
  //     } else {
  //       return null;
  //     }
  //   };

  render() {
    const { trades } = this.props;

    return (
      <div style={{ marginTop: 10 }}>
        <h5>Close Positions ({trades?.length}):</h5>
        <div></div>
      </div>
    );
  }
}

export default TradeClosedPositions;
