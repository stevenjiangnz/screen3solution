import React from "react";
import Title from "../Title";
import StockChart from "./StockChart";
import AppContext from "../../Context";

function StockDetail() {
  return (
    <AppContext.Consumer>
      {(context) => {
        return (
          <div className="detail-panel">
            <Title title="Stock"></Title>
            <div>
              <StockChart
                name="stockChart"
                stock={context.state.selectedStock}
              ></StockChart>
            </div>
          </div>
        );
      }}
    </AppContext.Consumer>
  );
}

export default StockDetail;
