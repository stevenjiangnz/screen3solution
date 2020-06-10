import React, { Component } from "react";

export class TradeAccount extends Component {
  constructor(props) {
    super(props);

    this.state = {
      newAccountValue: "",
    };
  }

  onAddNewAccount = () => {
    console.log("about to add new account");
    this.setState({
      newAccountValue: "",
    });
  };

  onAccountNameChanged = (e) => {
    this.setState({
      newAccountValue: e.target.value,
      accounts: [],
    });
  };

  render() {
    return (
      <div>
        <div className="row">
          <p>{this.state.newAccountValue}</p>
        </div>
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
      </div>
    );
  }
}

export default TradeAccount;
