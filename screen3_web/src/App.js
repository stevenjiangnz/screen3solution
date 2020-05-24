import React, { Component } from "react";
import TopNav from "./component/TopNav";
import Stock from "./component/stock/Stock";
import Screen from "./component/screen/Screen";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";

export class App extends Component {
  render() {
    return (
      <Router>
        <div>
          <TopNav />
          <Switch>
            <Route exact path="/">
              <Stock />
            </Route>
            <Route exact path="/screen">
              <Screen />
            </Route>
          </Switch>
        </div>
      </Router>
    );
  }
}

export default App;
