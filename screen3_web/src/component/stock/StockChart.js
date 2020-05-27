import React, { Component } from "react";
import AppContext from "../../Context";
import Highcharts from "highcharts/highstock";
import HighchartsReact from "highcharts-react-official";

export class StockChart extends Component {
  context;
  chartRef;

  constructor(props) {
    super(props);
    this.chartRef = React.createRef();

    this.state = {};
  }

  render() {
    const options = {
      title: {
        text: "My stock chart",
      },
      series: [
        {
          data: [1, 2, 1, 4, 3, 6, 7, 3, 8, 6, 9],
        },
      ],
    };

    return (
      <AppContext.Consumer>
        {(context) => {
          this.context = context;

          return (
            <div>
              <HighchartsReact
                highcharts={Highcharts}
                constructorType={"stockChart"}
                options={options}
              />
            </div>
          );
        }}
      </AppContext.Consumer>
    );
  }
}

export default StockChart;
