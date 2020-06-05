import React, { Component } from "react";
import Tab from "react-bootstrap/Tab";
import Tabs from "react-bootstrap/Tabs";

import ScreenInput from "./ScreenInput";
import ScreenResultList from "./ScreenResultList";
import ScreenResultDetail from "./ScreenResultDetail";
export class Screen extends Component {
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
      <div className="row">
        <div className="col-sm-3">
          <ScreenResultList></ScreenResultList>
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
                <div>chart tab place holder</div>
              </Tab>
            </Tabs>
          </div>
        </div>
        <div className="col-sm-2">
          <ScreenResultDetail></ScreenResultDetail>
        </div>
      </div>
    );
  }
}

export default Screen;
