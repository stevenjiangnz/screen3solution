import React, { Component } from "react";
import TopNav from "./component/TopNav";
import Stock from "./component/stock/Stock";
import Screen from "./component/screen/Screen";
import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from "react-router-dom";
import AppContext from "./Context";

export class App extends Component {
  state = {
    selectedStock: {
      code: "XAO",
      company: "All Index",
      sector: "Index",
      cap: 0,
      weight: 0,
    },
  };

  onSelectedStockChanged = (stock) => {
    this.setState({
      selectedStock: stock,
    });
  };

  render() {
    return (
      <AppContext.Provider
        value={{
          state: this.state,
          updateSelectedStock: this.onSelectedStockChanged,
        }}
      >
        <Router>
          <div>
            <TopNav />
            <Switch>
              <Route exact path="/">
                <Redirect to="/stock" />
              </Route>
              <Route path="/stock">
                <Stock />
              </Route>
              <Route exact path="/screen">
                <Screen />
              </Route>
              <Route path="*">
                <NotFound />
              </Route>
            </Switch>
          </div>
        </Router>
      </AppContext.Provider>
    );
  }
}

function NotFound() {
  return <h3>404. not found</h3>;
}
export default App;
