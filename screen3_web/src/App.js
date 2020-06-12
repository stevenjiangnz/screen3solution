import React, { Component } from "react";
import TopNav from "./component/TopNav";
import Stock from "./component/stock/Stock";
import Screen from "./component/screen/Screen";
import StockService from "./service/StockService";
import "ag-grid-community/dist/styles/ag-grid.css";
import "ag-grid-community/dist/styles/ag-theme-balham.css";

import {
  BrowserRouter as Router,
  Switch,
  Route,
  Redirect,
} from "react-router-dom";
import AppContext from "./Context";

export class App extends Component {
  indexDefault = {
    code: "XAO",
    company: "All Index",
    sector: "Index",
    cap: 0,
    weight: 0,
  };
  state = {
    selectedStock: this.indexDefault,
    screenResult: [],
    stockList: [],
    currentScreenStock: {},
    currentScreenResult: [],
    selectedScreenPoint: {},
  };

  onSelectedStockChanged = (stock) => {
    this.setState({
      selectedStock: stock,
    });
  };

  onChartSettingsChanged = (name, settings) => {
    this.setState({
      [`${name}`]: settings,
    });
  };

  onScreenResultChanged = (screenResult) => {
    this.setState({
      screenResult: screenResult,
    });
  };

  onCurrentScreenStockChanged = (screenPoint) => {
    const stock = this.state.stockList.find(
      (stock) => stock.code === screenPoint.code
    );
    const currentScreenResult = this.state.screenResult.filter(
      (result) => result.code === screenPoint.code
    );
    this.setState({
      currentScreenStock: stock,
      currentScreenResult,
      selectedScreenPoint: screenPoint,
    });
  };

  componentDidMount() {
    const service = new StockService();
    service
      .getStockList()
      .then((resp) => {
        this.setState({
          stockList: resp.data,
        });
      })
      .catch((error) => {
        console.error("error", error);
      });
  }

  render() {
    return (
      <AppContext.Provider
        value={{
          state: this.state,
          updateSelectedStock: this.onSelectedStockChanged,
          updateChartSettings: this.onChartSettingsChanged,
          setScreenResult: this.onScreenResultChanged,
          updateCurrentScreenStock: this.onCurrentScreenStockChanged,
        }}
      >
        <Router>
          <div>
            <TopNav />
            <Switch>
              <Route exact path="/">
                <Redirect to="/screen" />
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
