import React, { Component } from "react";
import Tab from "react-bootstrap/Tab";
import Tabs from "react-bootstrap/Tabs";

import ScreenInput from "./ScreenInput";
import ScreenResultList from "./ScreenResultList";
import TradeContainer from "./TradeContainer";

import AppContext from "../../Context";
import StockChart from "../stock/StockChart";

export class Screen extends Component {
  context;
  state = {
    selectedTab: "inputTab",
  };

  setTab = (k) => {
    this.setState({
      selectedTab: k,
    });
  };

  switcchToChartPane = () => {
    this.setState({
      selectedTab: "chartTab",
    });
  };

  render() {
    return (
      <AppContext.Consumer>
        {(context) => {
          this.context = context;
          return (
            <div className="row">
              <div className="col-sm-2">
                {context.state.screenResult.length > 0 && (
                  <ScreenResultList
                    onItemClicked={this.switcchToChartPane}
                  ></ScreenResultList>
                )}
              </div>
              <div className="col-sm-7">
                <div style={{ marginTop: 10 }}>
                  <Tabs
                    activeKey={this.state.selectedTab}
                    id="screen-tab"
                    onSelect={(k) => this.setTab(k)}
                  >
                    <Tab eventKey="inputTab" title="Input">
                      <ScreenInput></ScreenInput>
                    </Tab>
                    <Tab eventKey="chartTab" title="Chart">
                      <div>
                        <StockChart
                          name="stockScreen"
                          stock={context.state.currentScreenStock}
                          screenResult={this.context.state.currentScreenResult}
                        ></StockChart>
                      </div>
                    </Tab>
                  </Tabs>
                </div>
              </div>
              <div className="col-sm-3">
                <TradeContainer></TradeContainer>
              </div>
            </div>
          );
        }}
      </AppContext.Consumer>
    );
  }
}

export default Screen;
