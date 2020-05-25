import React, { Component } from "react";
import RequestHelper from "../../util/RequestHelper";

export class StockList extends Component {
  state = {
    stocks: [],
  };

  componentDidMount() {
    const req = new RequestHelper().getIntance();

    req.get("/users").then((resp) => {
      console.log("resp: ", resp.data);
    });
  }
  render() {
    return <div>this is the stock list</div>;
  }
}

export default StockList;
