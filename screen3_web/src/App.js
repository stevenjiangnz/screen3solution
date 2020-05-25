import React, { Component } from "react";
import TopNav from "./component/TopNav";
import Stock from "./component/stock/Stock";
import Screen from "./component/screen/Screen";
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";

export class App extends Component {
  render() {
    // console.log(
    //   "env config REACT_APP_API_BASE_URL",
    //   process.env.REACT_APP_API_BASE_URL
    // );
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
            <Route path="*">
              require('dotenv').config()
              <NotFound />
            </Route>
          </Switch>
        </div>
      </Router>
    );
  }
}

function NotFound() {
  return <h3>404. not found</h3>;
}
export default App;
