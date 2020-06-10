import React, { Component } from "react";

export class TradeAccount extends Component {
  render() {
    return (
      <div class="input-group mb-3" style={{ marginTop: 10 }}>
        <input
          type="text"
          class="form-control"
          placeholder="Add new account"
          aria-label="Add new account"
          aria-describedby="basic-addon2"
        ></input>
        <div class="input-group-append">
          <button class="btn btn-primary" type="button">
            New
          </button>
        </div>
      </div>
    );
  }
}

export default TradeAccount;
