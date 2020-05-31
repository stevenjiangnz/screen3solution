import React from "react";
import Title from "../Title";
import StockChart from "./StockChart";

function StockDetail() {
  return (
    <div className="detail-panel">
      <Title title="Stock"></Title>
      <div>
        <StockChart name="stockChart"></StockChart>
      </div>
    </div>
  );
}

export default StockDetail;
