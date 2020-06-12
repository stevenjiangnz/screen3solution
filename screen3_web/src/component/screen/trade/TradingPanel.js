import React, { Component } from "react";
import TradeService from "../../../service/TradeService";

export class TradingPanel extends Component {
  tradeService;
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
      <div style={{ paddingTop: 10, paddingLeft: 2, paddingRight: 2 }}>
        <div class="form-group">
          <select
            class="form-control"
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
          <button type="button" class="btn btn-success btn-sm">
            Long
          </button>
          <button
            type="button"
            class="btn btn-danger btn-sm"
            style={{ marginLeft: 10 }}
          >
            Short
          </button>
        </div>
        {/* {JSON.stringify(this.state.selectedAccountId)} */}
      </div>
    );
  }
}

export default TradingPanel;
