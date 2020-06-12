import React, { Component } from "react";
import { v4 as uuidv4 } from "uuid";
import TradeService from "../../../service/TradeService";
import TickerService from "../../../service/TickerService";

export class TradeAccount extends Component {
  tradeService;

  constructor(props) {
    super(props);

    this.tradeService = new TradeService();
    this.state = {
      newAccountValue: "",
      accounts: [],
    };
  }

  onAddNewAccount = () => {
    const newAccount = { id: uuidv4(), name: this.state.newAccountValue };
    this.tradeService
      .createNewAccount(newAccount.id, newAccount.name)
      .then(() => {
        this.setState({
          newAccountValue: "",
          accounts: [...this.state.accounts, newAccount],
        });
      });
  };

  onAccountNameChanged = (e) => {
    this.setState({
      newAccountValue: e.target.value,
    });
  };

  onAccountDelete = (id) => {
    this.tradeService.deleteAccount(id).then(() => {
      const newAccounts = this.state.accounts.filter((acc) => acc.id !== id);
      this.setState({
        accounts: newAccounts,
      });
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
      <div style={{ padding: 2 }}>
        {/* <div className="row">
          <p>{JSON.stringify(this.state.accounts)}</p>
        </div> */}
        <div className="input-group mb-3" style={{ marginTop: 10 }}>
          <input
            type="text"
            className="form-control"
            placeholder="Add new account"
            aria-label="Add new account"
            aria-describedby="basic-addon2"
            value={this.state.newAccountValue}
            onChange={this.onAccountNameChanged}
          ></input>
          <div className="input-group-append">
            <button
              className="btn btn-primary"
              type="button"
              onClick={this.onAddNewAccount}
            >
              New
            </button>
          </div>
        </div>
        <div>
          <ui className="list-group">
            {this.state.accounts.map((acc) => {
              return (
                <li
                  className="list-group-item d-flex justify-content-between align-items-center"
                  key={acc.id}
                >
                  {acc.name}
                  <span
                    style={{ cursor: "pointer" }}
                    onClick={() => this.onAccountDelete(acc.id)}
                  >
                    X
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

export default TradeAccount;
