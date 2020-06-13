import React, { Component } from "react";
import TradeService from "../../../service/TradeService";
import AppContext from "../../../Context";
export class TradingPanel extends Component {
  tradeService;
  context;
  constructor(props) {
    super(props);

    this.tradeService = new TradeService();
    this.state = {
      accounts: [],
      selectedAccountId: {},
    };
  }

  handleAccountChange = (e) => {
    this.setState({
      selectedAccountId: e.target.value,
    });
  };

  componentDidMount() {
    this.tradeService.getAllAccount().then((resp) => {
      this.setState({
        accounts: resp.data,
      });
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
                    return <option value={account.id}>{account.name}</option>;
                  })}
                </select>
              </div>
              <div>
                <button type="button" className="btn btn-success btn-sm">
                  Long
                </button>
                <button
                  type="button"
                  className="btn btn-danger btn-sm"
                  style={{ marginLeft: 10 }}
                >
                  Short
                </button>
              </div>
              {JSON.stringify(this.context.state.currentTradeTicker)}
            </div>
          );
        }}
      </AppContext.Consumer>
    );
  }
}

export default TradingPanel;
