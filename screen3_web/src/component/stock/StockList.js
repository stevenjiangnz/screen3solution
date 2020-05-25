import React, { Component } from "react";
import RequestHelper from "../../util/RequestHelper";

export class StockList extends Component {
  state = {
    stocks: [],
  };

  componentDidMount() {
    const req = new RequestHelper().getIntance();

    req
      .get("stock")
      .then((resp) => {
        console.log("resp: ", resp.data);
      })
      .catch((error) => {
        console.log("error", error);
      });
  }
  render() {
    return <div>this is the stock list</div>;
  }
}

export default StockList;
