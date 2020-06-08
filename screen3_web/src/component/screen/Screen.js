import React, { Component } from "react";
import Tab from "react-bootstrap/Tab";
import Tabs from "react-bootstrap/Tabs";

import ScreenInput from "./ScreenInput";
import ScreenResultList from "./ScreenResultList";
import ScreenResultDetail from "./ScreenResultDetail";

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

  render() {
    return (
      <AppContext.Consumer>
        {(context) => {
          this.context = context;
          return (
            <div className="row">
              <div className="col-sm-2">
                {context.state.screenResult.length > 0 && (
                  <ScreenResultList></ScreenResultList>
                )}
              </div>
              <div className="col-sm-8">
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
                        ></StockChart>
                      </div>
                    </Tab>
                  </Tabs>
                </div>
              </div>
              <div className="col-sm-2">
                <ScreenResultDetail></ScreenResultDetail>
              </div>
            </div>
          );
        }}
      </AppContext.Consumer>
    );
  }
}

export default Screen;
