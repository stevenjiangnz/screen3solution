import React, { Component } from "react";
import Tab from "react-bootstrap/Tab";
import Tabs from "react-bootstrap/Tabs";
import TradingPanel from "./trade/TradingPanel";
import TradeAccount from "./trade/TradeAccount";

export class TradeContainer extends Component {
  constructor(props) {
    super(props);

    this.state = {
      selectedTab: "accountTab",
    };
  }

  setTab = (k) => {
    this.setState({
      selectedTab: k,
    });
  };

  render() {
    return (
      <div>
        <div style={{ marginTop: 10 }}>
          <Tabs
            activeKey={this.state.selectedTab}
            id="trade-tab"
            onSelect={(k) => this.setTab(k)}
          >
            <Tab eventKey="tradeTab" title="Trade">
              <TradingPanel></TradingPanel>
            </Tab>
            <Tab eventKey="accountTab" title="Account">
              <TradeAccount></TradeAccount>
            </Tab>
          </Tabs>
        </div>
      </div>
    );
  }
}

export default TradeContainer;
