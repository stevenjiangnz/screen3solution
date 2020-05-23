import React, { Component } from "react";
import TopNav from "./component/TopNav";
import Stock from "./component/stock/Stock";
import Screen from "./component/screen/Screen";

export class App extends Component {
  render() {
    return (
      <div>
        <TopNav />
        <Stock />
        <Screen />
      </div>
    );
  }
}

export default App;
