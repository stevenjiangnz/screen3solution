import React, { Component } from "react";
import { v4 as uuidv4 } from "uuid";

export class TradeAccount extends Component {
  constructor(props) {
    super(props);

    this.state = {
      newAccountValue: "",
      accounts: [],
    };
  }

  onAddNewAccount = () => {
    this.setState({
      newAccountValue: "",
      accounts: [
        ...this.state.accounts,
        { id: uuidv4(), account: this.state.newAccountValue },
      ],
    });
  };

  onAccountNameChanged = (e) => {
    this.setState({
      newAccountValue: e.target.value,
    });
  };

  onAccountDelete = (id) => {
    const newAccounts = this.state.accounts.filter((acc) => acc.id !== id);
    this.setState({
      accounts: newAccounts,
    });
  };

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
                  (0) {acc.account}
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
