import React, { Component } from "react";
import TradeService from "../../../service/TradeService";
import AppContext from "../../../Context";
import TradeCurrentTicker from "./TradeCurrentTicker";
import TickerHelper from "../../../util/TickerHelper";
import TradeOpenPositions from "./TradeOpenPositions";

export class TradingPanel extends Component {
  tradeService;
  context;
  allTrades;
  constructor(props) {
    super(props);

    this.tradeService = new TradeService();
    this.state = {
      accounts: [],
      selectedAccountId: {},
      openPositions: [],
      closedPositions: [],
    };
  }

  handleAccountChange = (e) => {
    this.setState({
      selectedAccountId: e.target.value,
    });
  };

  onOpenPosition = (direction) => {
    const ticker = this.context.state.currentTradeTicker;
    const entryPrice = TickerHelper.formatNum((ticker.h + ticker.l) / 2, 4);

    this.tradeService
      .openPositionAccount(this.state.selectedAccountId, {
        operation: "open",
        id: new Date().getTime().toString(),
        code: ticker.t,
        direction,
        entryDate: ticker.p,
        entryPrice: parseFloat(entryPrice),
      })
      .then(() => {
        console.log("open position done");
      });
  };

  onClosePosition = (trade, ticker) => {
    console.log("about to close position: ", trade, ticker);
  };

  loadAccountDetails = (accountId) => {
    this.tradeService.getAccountDetails(accountId).then((resp) => {
      this.allTrades = resp.data.trades;

      const openPositions = [];
      const closedPositions = [];

      for (var i = 0; i < this.allTrades.length; i++) {
        if (this.allTrades[i].exitPrice && this.allTrades[i].exitDate) {
          closedPositions.push(this.allTrades[i]);
        } else {
          openPositions.push(this.allTrades[i]);
        }
      }

      this.setState({
        openPositions,
        closedPositions,
      });
    });
  };

  componentDidMount() {
    this.tradeService.getAllAccount().then((resp) => {
      this.setState({
        accounts: resp.data,
        selectedAccountId: resp.data[0]?.id,
      });
      this.loadAccountDetails(this.state.selectedAccountId);
    });
  }

  render() {
    return (
      <AppContext.Consumer>
        {(context) => {
          this.context = context;
          return (
            <div style={{ paddingTop: 10, paddingLeft: 2, paddingRight: 2 }}>
              <div className="form-group">
                <select
                  className="form-control"
                  id="accountSelector"
                  value={this.state.selectedAccountId}
                  onChange={this.handleAccountChange}
                >
                  {this.state.accounts.map((account) => {
                    return (
                      <option key={account.id} value={account.id}>
                        {account.name}
                      </option>
                    );
                  })}
                </select>
              </div>
              <div>
                <button
                  type="button"
                  className="btn btn-success btn-sm"
                  disabled={
                    !(
                      this.context.state.currentTradeTicker &&
                      this.context.state.currentTradeTicker.t
                    )
                  }
                  onClick={() => this.onOpenPosition(1)}
                >
                  Long
                </button>
                <button
                  type="button"
                  className="btn btn-danger btn-sm"
                  disabled={
                    !(
                      this.context.state.currentTradeTicker &&
                      this.context.state.currentTradeTicker.t
                    )
                  }
                  style={{ marginLeft: 10 }}
                  onClick={() => this.onOpenPosition(-1)}
                >
                  Short
                </button>
              </div>
              <div>
                <TradeCurrentTicker
                  ticker={this.context.state.currentTradeTicker}
                ></TradeCurrentTicker>
              </div>
              <div>
                <TradeOpenPositions
                  trades={this.state.openPositions}
                  ticker={this.context.state.currentTradeTicker}
                  onTradeClose={this.onClosePosition}
                ></TradeOpenPositions>
              </div>
            </div>
          );
        }}
      </AppContext.Consumer>
    );
  }
}

export default TradingPanel;
